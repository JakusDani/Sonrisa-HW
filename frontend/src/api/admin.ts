import apiClient from './client'
import type { SimulateEventRequest, SimulateEventResponse } from '../types/admin'

export async function simulateEvent(request: SimulateEventRequest): Promise<SimulateEventResponse> {
  const response = await apiClient.post<SimulateEventResponse>('/admin/simulate', request)
  return response.data
}
