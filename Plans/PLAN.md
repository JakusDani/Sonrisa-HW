# World Events Alert System — Implementation Plan

## Overview

A full-stack notification/alerting platform built with **ASP.NET Core (.NET 8/9) Web API** (backend) and **React + TypeScript + Vite** (frontend), following **Vertical Slice Architecture (VSA)**. Users configure `Alert`s that match `WorldEvent`s (breaking news, market movements, natural disasters, etc.) and get notified via pluggable channels (Email, Slack, extensible to more). An **Admin** view lets staff simulate world events and inspect notification logs.

Per user direction, no real message broker infrastructure is required for this exercise: the "queue" between event-matching and notification-dispatch is implemented via an **`INotificationQueue` abstraction backed by `InMemoryNotificationQueue`** (using a `System.Threading.Channels.Channel<T>` internally), consumed by a `BackgroundService`. This preserves the same architecture/seams a real RabbitMQ integration would use (publish → async consume → dispatch strategy → log), so swapping in real RabbitMQ later only requires a new `INotificationQueue` implementation — no slice code changes.

Email/Slack channels do **not** perform real I/O — they log to console and return success, to demonstrate the Strategy/Factory extensibility without real integration work.

## Architectural Rules Recap (must be enforced in all generated code)
- Vertical Slice Architecture: each feature owns its endpoint, command/query, handler, domain logic, and mapping — no shared "Service layer" leaking across features.
- `record` types for all DTOs/Commands/Queries.
- PascalCase method names; private methods prefixed with `_` (e.g., `_MatchAlerts`).
- LINQ lambda expressions preferred over imperative loops.
- Methods ≤ 80 LOC, classes ≤ 200 LOC.
- **Exactly one `return`** per method (Single Return Layer — use local result variables, guard flags, or ternaries instead of early returns).
- Global exception handling / logging / request manipulation via custom **middleware**, not try-catch scattered through code.
- Notification channel extensibility via **Strategy + Factory pattern** (`INotificationChannel` + `INotificationChannelFactory`).

## Solution Structure

```
/src
  /Api                          (ASP.NET Core Web API host project)
	Program.cs
	/Middleware
	  ExceptionHandlingMiddleware.cs
	  RequestLoggingMiddleware.cs
	/Common
	  /Records                  (shared ApiResponse<T>, ErrorResponse, etc.)
	  /Data
		FakeDatabase.cs         (singleton in-memory store)
	  /Notifications
		INotificationChannel.cs
		EmailNotificationChannel.cs
		SlackNotificationChannel.cs
		INotificationChannelFactory.cs
		NotificationChannelFactory.cs
		INotificationQueue.cs
		InMemoryNotificationQueue.cs
		NotificationQueueMessage.cs      (record)
		NotificationDispatchConsumer.cs  (BackgroundService, the "worker")
	  /Domain
		Alert.cs
		WorldEvent.cs
		NotificationLog.cs
		EventType.cs (enum)
		ChannelType.cs (enum: Email, Slack, ...)
	/Features
	  /Alerts
		/GetAlerts        (Query, Handler, Endpoint mapping, Response record)
		/CreateAlert      (Command, Handler, Endpoint, Validation)
		/DeleteAlert      (Command, Handler, Endpoint)
		/UpdateAlert      (Command, Handler, Endpoint) [optional PUT]
	  /Admin
		/SimulateEvent    (Command, Handler, Endpoint, Response record)
	  /Logs
		/GetLogs          (Query, Handler, Endpoint)
/frontend
  (Vite + React + TS app — MUI, React Router, Axios)
/PLAN.md
```

## Domain Models (records where read-only makes sense; entities as simple classes with mutable Id/State are fine since FakeDatabase mutates them)

- `EventType` enum: `BreakingNews, MarketMovement, NaturalDisaster, Custom`
- `ChannelType` enum: `Email, Slack`
- `Alert` — Id, Name, EventType, EnabledChannels (`IReadOnlyCollection<ChannelType>`), IsEnabled
- `WorldEvent` (record) — Id, EventType, Message, OccurredAtUtc
- `NotificationLog` (record) — Id, AlertId, AlertName, Channel, Message, Status (Success/Failed), TimestampUtc
- `NotificationQueueMessage` (record) — WorldEventId, AlertId, AlertName, Channel, Message

## Notification Pipeline (Strategy + Factory + In-Memory Queue)

1. `INotificationChannel` — `Task<NotificationResult> SendAsync(NotificationQueueMessage message)`.
2. `EmailNotificationChannel` / `SlackNotificationChannel` — write `Console.WriteLine($"[Email] Sending to ...: {message}")`, return success result. Single return each.
3. `INotificationChannelFactory` — `INotificationChannel Create(ChannelType channelType)` using a lambda-based dictionary map (`_channelMap.TryGetValue(...)`) — enables adding new channels by registering one more entry, no branching edits elsewhere.
4. `INotificationQueue` — `Task PublishAsync(NotificationQueueMessage message)` / `IAsyncEnumerable<NotificationQueueMessage> ReadAllAsync(CancellationToken ct)`.
5. `InMemoryNotificationQueue` — wraps `Channel<NotificationQueueMessage>.CreateUnbounded()`. Registered as singleton.
6. `NotificationDispatchConsumer : BackgroundService` — loops `ReadAllAsync`, resolves channel via factory, calls `SendAsync`, writes a `NotificationLog` entry to `FakeDatabase`. This is the seam where a real RabbitMQ consumer would later replace the in-memory read loop.

## Matching & Dispatch Flow (lives in `Admin/SimulateEvent` slice, matching logic co-located per VSA)

1. `POST /admin/simulate` receives `SimulateEventCommand` (EventType, Message).
2. Handler creates a `WorldEvent`, stores it in `FakeDatabase`.
3. Handler uses LINQ (`FakeDatabase.Alerts.Where(a => a.IsEnabled && a.EventType == event.EventType)`) to find matching alerts.
4. For each matched alert, for each enabled channel on that alert, handler publishes a `NotificationQueueMessage` to `INotificationQueue`.
5. Handler returns `SimulateEventResponse(MatchedAlertsCount, NotificationsQueuedCount)` — actual "sent" count is reflected asynchronously in `/logs` once the consumer processes the queue (near-instant in-memory, but decoupled by design).

## API Endpoints

| Method | Route | Slice |
|---|---|---|
| GET | `/alerts` | Alerts/GetAlerts |
| POST | `/alerts` | Alerts/CreateAlert |
| DELETE | `/alerts/{id}` | Alerts/DeleteAlert |
| PUT | `/alerts/{id}` | Alerts/UpdateAlert (optional) |
| POST | `/admin/simulate` | Admin/SimulateEvent |
| GET | `/logs` | Logs/GetLogs |

Use **Minimal API** endpoint mapping (`app.MapGet/MapPost/...`) per slice, each slice exposing a static `Map(IEndpointRouteBuilder app)` extension method to keep `Program.cs` thin.

## Cross-Cutting Middleware

- `ExceptionHandlingMiddleware` — catches unhandled exceptions once at the pipeline boundary, maps to a standard `ErrorResponse` record + HTTP status, logs via `ILogger`. This is the **only** place exceptions are caught — feature handlers do not use try-catch.
- `RequestLoggingMiddleware` — logs method, path, status code, elapsed ms for every request.
- Registered in `Program.cs` in order: ExceptionHandling → RequestLogging → Routing → Endpoints.

## FakeDatabase (Singleton)

Simple in-memory store using `ConcurrentDictionary<Guid, T>` collections for `Alerts`, `WorldEvents`, `NotificationLogs`. Exposes read-only LINQ-friendly properties (`IReadOnlyCollection<Alert>` via `.Values`) plus `Add/Remove/Update` methods (each with a single return).

## Frontend Plan (React + TS + Vite)

- Stack: Vite scaffold (`react-ts` template), MUI, React Router v6, Axios.
- `src/api/` — Axios instance + typed API client functions per feature (alerts, admin, logs) mirroring backend DTOs as TS interfaces.
- `src/pages/`: `DashboardPage`, `AlertsPage`, `AdminSimulatorPage`, `NotificationLogsPage`.
- `src/components/`: `NavBar`, `AlertList`, `AlertFormDialog`, `ConfirmDeleteDialog`, `SimulateEventForm`, `NotificationLogsTable`, `LoadingSpinner`, `AppSnackbar`.
- Routing: `/` (Dashboard), `/alerts`, `/admin`, `/logs`.
- State: local component state + simple custom hooks (`useAlerts`, `useLogs`) wrapping Axios calls; no need for Redux at this scale.
- Polish: MUI `Snackbar` for success/error, `Skeleton`/`CircularProgress` for loading, empty-state placeholders, `Dialog` confirmation before delete.

## Steps

1. Scaffold backend — Create `Api` ASP.NET Core Web API project (`dotnet new webapi -minimal`), enable nullable reference types, configure DI container in `Program.cs`, add CORS policy allowing the Vite dev server origin.
2. Create folder skeleton — Add `Middleware`, `Common/Data`, `Common/Notifications`, `Common/Domain`, `Features/Alerts`, `Features/Admin`, `Features/Logs` folders as outlined above.
3. Implement domain models — Add `EventType`, `ChannelType` enums and `Alert`, `WorldEvent`, `NotificationLog`, `NotificationQueueMessage` types (records where read-only) in `Common/Domain`.
4. Implement `FakeDatabase` — Singleton class with `ConcurrentDictionary`-backed collections and CRUD helper methods (single-return, PascalCase, `_`-prefixed private helpers); register via `builder.Services.AddSingleton<FakeDatabase>()`.
5. Implement notification channel strategy — Add `INotificationChannel`, `EmailNotificationChannel`, `SlackNotificationChannel` (console log + success result, single return each).
6. Implement channel factory — Add `INotificationChannelFactory` / `NotificationChannelFactory` mapping `ChannelType` → channel instance via lambda dictionary; register as singleton.
7. Implement in-memory queue — Add `INotificationQueue` interface and `InMemoryNotificationQueue` implementation wrapping `System.Threading.Channels.Channel<NotificationQueueMessage>`; register as singleton.
8. Implement dispatch consumer — Add `NotificationDispatchConsumer : BackgroundService` that reads from `INotificationQueue`, resolves channel via factory, sends, and writes a `NotificationLog` to `FakeDatabase`; register via `AddHostedService`.
9. Implement `Alerts` slice — `GetAlerts` (query+handler+endpoint), `CreateAlert`, `DeleteAlert`, and optional `UpdateAlert`, each using `record` Command/Query/Response types and LINQ against `FakeDatabase`.
10. Implement `Admin/SimulateEvent` slice — Command record, handler that creates `WorldEvent`, matches alerts via LINQ, publishes `NotificationQueueMessage`s to `INotificationQueue`, and returns `SimulateEventResponse(MatchedAlertsCount, NotificationsQueuedCount)`.
11. Implement `Logs/GetLogs` slice — Query + handler returning all `NotificationLog` entries from `FakeDatabase`, newest first.
12. Implement middleware — `ExceptionHandlingMiddleware` and `RequestLoggingMiddleware`; wire into `Program.cs` pipeline before endpoint mapping.
13. Wire up `Program.cs` — Register DI (FakeDatabase, factory, queue, consumer), CORS, middleware pipeline, and call each slice's `Map(app)` extension to register endpoints.
14. Scaffold frontend — Create Vite React-TS project under `/frontend`; install `@mui/material @emotion/react @emotion/styled react-router-dom axios`.
15. Build layout — `NavBar` component and route shell (`DashboardPage`, `AlertsPage`, `AdminSimulatorPage`, `NotificationLogsPage`) wired via React Router.
16. Build Alerts UI — List view, create dialog (Name, EventType select, Email/Slack checkboxes), delete with confirmation dialog, enable/disable toggle; call `/alerts` endpoints via Axios.
17. Build Admin Simulator UI — Form (EventType dropdown, Message textbox, "Fire Event" button) calling `POST /admin/simulate`, displaying matched/queued counts.
18. Build Notification Logs UI — Table (Time, Alert, Channel, Message, Status) fetched from `/logs`, with a manual/auto refresh after each simulation.
19. Add polish — Loading spinners/skeletons, success/error snackbars, empty states, and delete confirmation dialog across Alerts/Admin/Logs pages.
20. Write `PLAN.md` reference notes / README updates — Document how to run backend (`dotnet run`) and frontend (`npm run dev`), and note the `INotificationQueue` seam for future real RabbitMQ swap-in.

## Reference Notes (Step 20)

All 20 steps are implemented. See [README.md](../README.md) at the repository root for:

- Prerequisites (.NET 10 SDK, Node.js + npm)
- How to run the backend: `cd src/Api && dotnet run` (listens on `https://localhost:7235` / `http://localhost:5171`)
- How to run the frontend: `cd frontend && npm install && npm run dev` (runs on `http://localhost:5173`)
- The `INotificationQueue` seam and what swapping in a real RabbitMQ implementation later would require (a new `INotificationQueue` implementation + one DI registration change in `Program.cs` — no feature slice changes).
