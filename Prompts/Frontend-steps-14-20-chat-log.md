# Chat Log — Frontend Implementation (Plan Steps 14–20)

Date: session covering World Events Alert System frontend build-out.

## Context

Continuing `Plans/PLAN.md` after all backend steps (1–13) were already complete. This session covered steps 14–20 (frontend scaffold through documentation).

---

## Step 14 — Scaffold frontend

- Confirmed `/frontend` already existed as a fully scaffolded Vite + React + TypeScript project with `node_modules` installed.
- Verified `package.json` already included the required dependencies: `@mui/material`, `@emotion/react`, `@emotion/styled`, `react-router-dom`, `axios`.
- No action needed — step already satisfied.

## Step 15 — Build layout

- Created `frontend/src/components/NavBar.tsx` — MUI `AppBar`/`Toolbar` with `NavLink` buttons to Dashboard, Alerts, Admin Simulator, Notification Logs.
- Created page stubs: `pages/DashboardPage.tsx`, `pages/AlertsPage.tsx`, `pages/AdminSimulatorPage.tsx`, `pages/NotificationLogsPage.tsx`.
- Rewrote `App.tsx` to render `NavBar` + `Routes` (`/`, `/alerts`, `/admin`, `/logs`).
- Updated `main.tsx` to wrap `<App />` in `<BrowserRouter>`.
- Hit a recurring IDE issue: `create_file` failed with "Could not get text view from file path" when recreating a file at a path that was just deleted (e.g. `App.tsx`). Workaround: create the file under a temporary name (e.g. `App2.tsx`) and rename/move it via terminal (`Move-Item`) to the target path.

## Step 16 — Build Alerts UI

- Inspected backend `Alerts` slice (`Alert.cs`, `AlertResponse`, `CreateAlertCommand`, `CreateAlertResponse`, `UpdateAlertCommand`, `DeleteAlertEndpoint`, `EventType`, `ChannelType` enums) and endpoint routes (`GET/POST /alerts`, `PUT/DELETE /alerts/{id}`).
- Added `System.Text.Json.Serialization.JsonStringEnumConverter` via `ConfigureHttpJsonOptions` in `Program.cs` so `EventType`/`ChannelType` serialize as strings instead of integers, matching TS string-union types.
- Created frontend files:
  - `types/alert.ts` — `Alert`, `CreateAlertRequest`, `UpdateAlertRequest`, `DeleteAlertResponse`, plus `EventType`/`ChannelType` modeled as `const ... as const` objects + derived union types (required because `tsconfig.app.json` has `erasableSyntaxOnly: true`, which disallows real TS `enum`).
  - `api/client.ts` — Axios instance pointed at `https://localhost:7235`.
  - `api/alerts.ts` — `getAlerts`, `createAlert`, `updateAlert`, `deleteAlert`.
  - `hooks/useAlerts.ts` — load/add/remove/toggle alerts with loading/error state.
  - `components/AlertList.tsx` — list with `Switch` (enable/disable) and delete button, event type + channel chips.
  - `components/AlertFormDialog.tsx` — create dialog (Name, EventType select, Email/Slack checkboxes).
  - `components/ConfirmDeleteDialog.tsx` — delete confirmation dialog.
  - `components/LoadingSpinner.tsx` — `CircularProgress` wrapper.
  - Rewired `pages/AlertsPage.tsx` to use all of the above with empty-state text and a `Snackbar` for feedback.
- Fixed several TypeScript issues surfaced by `get_errors`:
  - Real `enum` syntax rejected by `erasableSyntaxOnly` → switched to const-object + union-type pattern (`EventTypeValues`/`ChannelTypeValues` for runtime values, `EventType`/`ChannelType` as types) after an initial same-name const+type merge attempt also errored.
  - `verbatimModuleSyntax` required explicit `type` import specifiers for `EventType`/`ChannelType` in `AlertFormDialog.tsx`.
  - This MUI version's `Stack` component does **not** support `alignItems`/`justifyContent`/`component` as direct props (per its `StackBaseProps` type definition) — fixed by moving those into the `sx` prop instead.
  - Learned that `get_errors` can return stale/cached diagnostics immediately after an edit; re-running it a second time gives the fresh result.
- Avoided adding `@mui/icons-material` (not an installed dependency) — used a text "Delete" `Button` instead of an icon button.

## Step 17 — Build Admin Simulator UI

- Inspected backend `SimulateEventCommand`/`SimulateEventResponse`.
- Created `types/admin.ts` (`SimulateEventRequest`, `SimulateEventResponse`) and `api/admin.ts` (`simulateEvent` calling `POST /admin/simulate`).
- Created `components/SimulateEventForm.tsx` — EventType dropdown, Message textbox, "Fire Event" button (disabled while submitting or message empty).
- Rewrote `pages/AdminSimulatorPage.tsx` — wires the form, shows a result panel with matched/queued counts, and an error `Snackbar` + `Alert`.

## Step 18 — Build Notification Logs UI

- Inspected backend `NotificationLogResponse`, `NotificationLog` domain record, `NotificationStatus` enum, and `GET /logs` endpoint.
- Created `types/log.ts` (`NotificationLog`, `NotificationStatusValues`), `api/logs.ts` (`getLogs`).
- Created `hooks/useLogs.ts` — fetches logs and polls every 4 seconds for auto-refresh (so results appear automatically after a simulation run from the Admin page), plus manual `refresh()`.
- Created `components/NotificationLogsTable.tsx` — table with Time, Alert, Channel, Message, and a colored Status `Chip` (green success / red failed).
- Rewrote `pages/NotificationLogsPage.tsx` — table + loading/empty/error states + manual "Refresh" button.

## Step 19 — Add polish

- Added `components/ListSkeleton.tsx` — rounded `Skeleton` rows, used as the loading placeholder on `AlertsPage` and `NotificationLogsPage` (replacing the plain spinner for list/table views).
- `AlertsPage.tsx` — success `Snackbar` upgraded to a dismissible, styled green `Alert` (instead of plain Snackbar text), consistent with other pages.
- `AdminSimulatorPage.tsx` — added a success `Snackbar`/`Alert` ("Event simulated successfully.") alongside the existing error snackbar.
- `SimulateEventForm.tsx` — "Fire Event" button now shows an inline `CircularProgress` (`startIcon`) while submitting.
- Confirmed empty states (Alerts, Logs) and the delete confirmation dialog (Alerts) were already in place from earlier steps.

## Step 20 — Write PLAN.md reference notes / README updates

- Found the repository already had a near-empty `README.md` (`# Sonrisa-HW`) at the root — replaced its content with full documentation:
  - Prerequisites (.NET 10 SDK, Node.js + npm).
  - How to run the backend (`cd src/Api && dotnet run`, ports `https://localhost:7235` / `http://localhost:5171`, CORS policy note).
  - How to run the frontend (`cd frontend && npm install && npm run dev`, `http://localhost:5173`).
  - A dedicated section explaining the `INotificationQueue` seam and exactly what swapping in a real RabbitMQ implementation later would require (new `INotificationQueue` implementation + one DI registration change in `Program.cs`, no feature-slice changes).
- Appended a "Reference Notes (Step 20)" section to `Plans/PLAN.md` confirming all 20 steps are implemented and pointing to the README.

---

## Notable environment/tooling issues encountered

1. **Node.js/npm not on PATH** initially in the terminal session — blocked scaffolding via CLI; resolved because the frontend project already existed pre-scaffolded.
2. **Stale `create_file` "Could not get text view from file path" errors** when recreating a file immediately after deleting it at the same path (seen for `App.tsx`, `AlertsPage.tsx`, `AdminSimulatorPage.tsx`, `NotificationLogsPage.tsx`). Consistent workaround: create under a temporary filename, then `Move-Item` (rename) to the intended path via the terminal.
3. **Stale `get_errors` diagnostics** immediately following an edit — re-invoking `get_errors` a second time produced accurate, up-to-date results.
4. **TypeScript `erasableSyntaxOnly` + `verbatimModuleSyntax`** (in `tsconfig.app.json`) disallow real `enum` declarations and require explicit `type`-only imports — addressed via const-object + derived union-type pattern and `import { type X }` syntax.
5. **MUI `Stack` in the installed version** does not accept `alignItems`/`justifyContent`/`component` as top-level props — must be passed via `sx`.
