# Sonrisa-HW — World Events Alert System

A full-stack notification/alerting platform: **ASP.NET Core (.NET 10) Web API** backend (Vertical Slice Architecture) + **React + TypeScript + Vite** frontend (MUI, React Router, Axios).

See [Plans/PLAN.md](Plans/PLAN.md) for the full architecture and implementation plan.

## Prerequisites

- .NET 10 SDK
- Node.js (LTS) + npm

## Running the backend

```powershell
cd src/Api
dotnet run
```

The API listens on `https://localhost:7235` (and `http://localhost:5171`), per [launchSettings.json](src/Api/Properties/launchSettings.json). CORS is configured to allow the Vite dev server origin (`http://localhost:5173` / `https://localhost:5173`).

## Running the frontend

```powershell
cd frontend
npm install
npm run dev
```

The app runs at `http://localhost:5173` and calls the API at `https://localhost:7235` (configured in [frontend/src/api/client.ts](frontend/src/api/client.ts)).

Run both the backend and frontend simultaneously (in separate terminals) for the full experience.

## Notification queue seam (`INotificationQueue`)

Notification dispatch is decoupled from event matching via an `INotificationQueue` abstraction:

- `Admin/SimulateEvent` publishes `NotificationQueueMessage`s to `INotificationQueue`.
- `NotificationDispatchConsumer` (a `BackgroundService`) reads from the queue, resolves the channel via `INotificationChannelFactory`, sends, and writes a `NotificationLog`.

The current implementation, `InMemoryNotificationQueue`, wraps a `System.Threading.Channels.Channel<T>` and is registered as a singleton in `Program.cs`. Because slice code only depends on the `INotificationQueue` interface, swapping in a real broker (e.g., RabbitMQ) later only requires:

1. A new `IRabbitMqNotificationQueue : INotificationQueue` implementation (publish to an exchange/queue, consume via the broker's client).
2. Updating the DI registration in `Program.cs` (`AddSingleton<INotificationQueue, ...>`).

No changes to `Admin/SimulateEvent`, `NotificationDispatchConsumer`, or any other feature slice are required.