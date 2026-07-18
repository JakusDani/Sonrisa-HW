import { useState } from 'react'
import Container from '@mui/material/Container'
import Typography from '@mui/material/Typography'
import Button from '@mui/material/Button'
import Stack from '@mui/material/Stack'
import MuiAlert from '@mui/material/Alert'
import { useLogs } from '../hooks/useLogs'
import NotificationLogsTable from '../components/NotificationLogsTable'
import LoadingSpinner from '../components/LoadingSpinner'

function NotificationLogsPage() {
  const { logs, loading, error, refresh } = useLogs()
  const [refreshing, setRefreshing] = useState(false)

  const _handleRefresh = async () => {
    setRefreshing(true)
    await refresh()
    setRefreshing(false)
  }

  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Stack direction="row" sx={{ mb: 2, justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="h4">Notification Logs</Typography>
        <Button variant="outlined" onClick={_handleRefresh} disabled={refreshing}>
          Refresh
        </Button>
      </Stack>

      {loading && <LoadingSpinner />}

      {!loading && error && <MuiAlert severity="error">{error}</MuiAlert>}

      {!loading && !error && logs.length === 0 && (
        <Typography variant="body1" color="text.secondary">
          No notifications have been dispatched yet.
        </Typography>
      )}

      {!loading && !error && logs.length > 0 && <NotificationLogsTable logs={logs} />}
    </Container>
  )
}

export default NotificationLogsPage
