export const EventTypeValues = {
  BreakingNews: 'BreakingNews',
  MarketMovement: 'MarketMovement',
  NaturalDisaster: 'NaturalDisaster',
  Custom: 'Custom',
} as const

export type EventType = (typeof EventTypeValues)[keyof typeof EventTypeValues]

export const ChannelTypeValues = {
  Email: 'Email',
  Slack: 'Slack',
} as const

export type ChannelType = (typeof ChannelTypeValues)[keyof typeof ChannelTypeValues]

export interface Alert {
  id: string
  name: string
  eventType: EventType
  enabledChannels: ChannelType[]
  isEnabled: boolean
}

export interface CreateAlertRequest {
  name: string
  eventType: EventType
  enabledChannels: ChannelType[]
}

export interface UpdateAlertRequest {
  name: string
  eventType: EventType
  enabledChannels: ChannelType[]
  isEnabled: boolean
}

export interface DeleteAlertResponse {
  deleted: boolean
}
