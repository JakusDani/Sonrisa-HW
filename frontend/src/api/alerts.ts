import apiClient from './client'
import type { Alert, CreateAlertRequest, DeleteAlertResponse, UpdateAlertRequest } from '../types/alert'

const _basePath = '/alerts'

export async function getAlerts(): Promise<Alert[]> {
  const response = await apiClient.get<Alert[]>(_basePath)
  return response.data
}

export async function createAlert(request: CreateAlertRequest): Promise<Alert> {
  const response = await apiClient.post<Alert>(_basePath, request)
  return response.data
}

export async function updateAlert(id: string, request: UpdateAlertRequest): Promise<Alert> {
  const response = await apiClient.put<Alert>(`${_basePath}/${id}`, request)
  return response.data
}

export async function deleteAlert(id: string): Promise<DeleteAlertResponse> {
  const response = await apiClient.delete<DeleteAlertResponse>(`${_basePath}/${id}`)
  return response.data
}
