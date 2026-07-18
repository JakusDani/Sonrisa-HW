import { useCallback, useEffect, useState } from 'react'
import { getLogs } from '../api/logs'
import type { NotificationLog } from '../types/log'

const _pollIntervalMs = 4000

export function useLogs() {
  const [logs, setLogs] = useState<NotificationLog[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const _refresh = useCallback(async () => {
    setError(null)
    try {
      const data = await getLogs()
      setLogs(data)
    } catch {
      setError('Failed to load notification logs.')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    _refresh()
    const intervalId = setInterval(_refresh, _pollIntervalMs)
    return () => clearInterval(intervalId)
  }, [_refresh])

  return { logs, loading, error, refresh: _refresh }
}
