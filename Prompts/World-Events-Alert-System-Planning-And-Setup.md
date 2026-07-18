# World Events Alert System — Planning & Setup Chat Log

Date: (session captured on request)

## Summary

This chat session covered planning and initial setup for the **World Events Alert System** project.

### 1. Architecture Planning Request

**Prompt:** Act as a Senior Software Architect specializing in .NET 8/9 and React (TypeScript). Analyze the project brief and produce a `PLAN.md`:

> "We want users to be able to set up alerts so they get notified when something important happens in the world — like breaking news, market movements, natural disasters, that kind of thing. Should work for both email and Slack. Make it flexible enough that we can add more channels later. We need an admin view too."

**Strict rules requested:**
1. Vertical Slice Architecture (VSA) — feature slices own their endpoints, commands/queries, domain logic, and mappings.
2. Use `record` types for DTOs/Commands/Queries.
3. PascalCase method names; private methods prefixed with `_`.
4. Prefer LINQ lambda expressions.
5. Methods ≤ 80 LOC, classes ≤ 200 LOC, exactly one `return` per method.
6. Custom middleware for exception handling/logging instead of scattered try-catch.
7. Strategy/Factory pattern for notification channels (Email, Slack, extensible).

**Follow-up:** Requested RabbitMQ for sending notifications, plus a detailed 13-step implementation checklist (backend setup, domain models, notification channels, notification service, Alert API, Admin API, Log API, React setup, layout, Alerts UI, Admin Simulator UI, Notification Logs UI, polish) — explicitly stating not to implement real email/Slack, just log messages.

### 2. Clarification on Messaging

Proposed design: `NotificationService` publishes to RabbitMQ; a background consumer processes messages via Strategy-pattern channels and logs results. Asked user to confirm topology.

**User response:** Accepted the design but requested using an **`InMemoryNotificationQueue`** instead of a real RabbitMQ broker — preserving the same publish/consume seam so a real RabbitMQ implementation could be swapped in later without changing feature-slice code.

### 3. Final Plan Produced

A full implementation plan was generated and registered, covering:
- Solution structure (`/src/Api` with Middleware, Common (Data/Notifications/Domain), Features (Alerts, Admin, Logs); `/frontend` React+TS+Vite app).
- Domain models: `EventType`, `ChannelType` enums; `Alert`, `WorldEvent`, `NotificationLog`, `NotificationQueueMessage`.
- Notification pipeline: `INotificationChannel` (Email/Slack, console-log only), `INotificationChannelFactory`, `INotificationQueue` / `InMemoryNotificationQueue` (backed by `System.Threading.Channels.Channel<T>`), `NotificationDispatchConsumer` (`BackgroundService`).
- API endpoints: `GET/POST /alerts`, `DELETE /alerts/{id}`, `PUT /alerts/{id}` (optional), `POST /admin/simulate`, `GET /logs`.
- Cross-cutting middleware: `ExceptionHandlingMiddleware`, `RequestLoggingMiddleware`.
- `FakeDatabase` singleton using `ConcurrentDictionary`.
- Frontend: Vite + React + TypeScript, MUI, React Router, Axios; pages for Dashboard, Alerts, Admin Simulator, Notification Logs; polish items (loading states, snackbars, empty states, delete confirmation).
- 20 concrete implementation steps.

The full plan document was saved to [`Plans/PLAN.md`](../Plans/PLAN.md).

### 4. Backend Scaffolding Started

- Attempted to scaffold the `Api` project via `dotnet new webapi -minimal` under `/src`; project already existed as `src/Api/Api.csproj` targeting `net10.0` with `Microsoft.AspNetCore.OpenApi` referenced.

### 5. Vulnerability Fix

**Issue:** `src/Api/Api.csproj` had a vulnerability warning caused by a transitively-resolved `Microsoft.OpenApi` v2.0.0 package (pulled in by `Microsoft.AspNetCore.OpenApi` 10.0.9).

**Fix applied:** Added an explicit direct `PackageReference` pinning `Microsoft.OpenApi` to the patched version `2.7.5`, then ran `dotnet restore` — restore and build succeeded.

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.9" />
  <PackageReference Include="Microsoft.OpenApi" Version="2.7.5" />
</ItemGroup>
```

## Next Steps

Continue executing the plan in `Plans/PLAN.md`, starting from Step 1 (finish backend scaffolding: DI, CORS, folder skeleton) through Step 20 (frontend polish and documentation).
