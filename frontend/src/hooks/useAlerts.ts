import { useCallback, useEffect, useState } from 'react'
import { createAlert, deleteAlert, getAlerts, updateAlert } from '../api/alerts'
import type { Alert, CreateAlertRequest, UpdateAlertRequest } from '../types/alert'

export function useAlerts() {
  const [alerts, setAlerts] = useState<Alert[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const _refresh = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const data = await getAlerts()
      setAlerts(data)
    } catch {
      setError('Failed to load alerts.')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    _refresh()
  }, [_refresh])

  const _add = useCallback(async (request: CreateAlertRequest) => {
    setError(null)
    try {
      await createAlert(request)
      await _refresh()
    } catch {
      setError('Failed to create alert.')
    }
  }, [_refresh])

  const _remove = useCallback(async (id: string) => {
    setError(null)
    try {
      await deleteAlert(id)
      await _refresh()
    } catch {
      setError('Failed to delete alert.')
    }
  }, [_refresh])

  const _toggle = useCallback(async (alert: Alert) => {
    setError(null)
    const request: UpdateAlertRequest = {
      name: alert.name,
      eventType: alert.eventType,
      enabledChannels: alert.enabledChannels,
      isEnabled: !alert.isEnabled,
    }
    try {
      await updateAlert(alert.id, request)
      await _refresh()
    } catch {
      setError('Failed to update alert.')
    }
  }, [_refresh])

  return { alerts, loading, error, addAlert: _add, removeAlert: _remove, toggleAlert: _toggle, refresh: _refresh }
}
