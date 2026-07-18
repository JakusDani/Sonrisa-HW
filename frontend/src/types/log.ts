import type { ChannelType } from './alert'

export const NotificationStatusValues = {
  Success: 'Success',
  Failed: 'Failed',
} as const

export type NotificationStatus = (typeof NotificationStatusValues)[keyof typeof NotificationStatusValues]

export interface NotificationLog {
  id: string
  alertId: string
  alertName: string
  channel: ChannelType
  message: string
  status: NotificationStatus
  timestampUtc: string
}
