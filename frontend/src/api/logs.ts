import apiClient from './client'
import type { NotificationLog } from '../types/log'

export async function getLogs(): Promise<NotificationLog[]> {
  const response = await apiClient.get<NotificationLog[]>('/logs')
  return response.data
}
