import type { EventType } from './alert'

export interface SimulateEventRequest {
  eventType: EventType
  message: string
}

export interface SimulateEventResponse {
  matchedAlertsCount: number
  notificationsQueuedCount: number
}
