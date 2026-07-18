import { useState } from 'react'
import Container from '@mui/material/Container'
import Typography from '@mui/material/Typography'
import Button from '@mui/material/Button'
import Stack from '@mui/material/Stack'
import Snackbar from '@mui/material/Snackbar'
import MuiAlert from '@mui/material/Alert'
import { useAlerts } from '../hooks/useAlerts'
import AlertList from '../components/AlertList'
import AlertFormDialog from '../components/AlertFormDialog'
import ConfirmDeleteDialog from '../components/ConfirmDeleteDialog'
import ListSkeleton from '../components/ListSkeleton'
import type { Alert, CreateAlertRequest } from '../types/alert'

function AlertsPage() {
  const { alerts, loading, error, addAlert, removeAlert, toggleAlert } = useAlerts()
  const [createOpen, setCreateOpen] = useState(false)
  const [pendingDelete, setPendingDelete] = useState<Alert | null>(null)
  const [snackbarMessage, setSnackbarMessage] = useState<string | null>(null)

  const _handleCreate = async (request: CreateAlertRequest) => {
    setCreateOpen(false)
    await addAlert(request)
    setSnackbarMessage('Alert created.')
  }

  const _handleConfirmDelete = async () => {
    if (pendingDelete) {
      await removeAlert(pendingDelete.id)
      setSnackbarMessage('Alert deleted.')
    }
    setPendingDelete(null)
  }

  const _handleToggle = async (alert: Alert) => {
    await toggleAlert(alert)
    setSnackbarMessage('Alert updated.')
  }

  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Stack direction="row" sx={{ mb: 2, justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="h4">Alerts</Typography>
        <Button variant="contained" onClick={() => setCreateOpen(true)}>
          New Alert
        </Button>
      </Stack>

      {loading && <ListSkeleton />}

      {!loading && error && <MuiAlert severity="error">{error}</MuiAlert>}

      {!loading && !error && alerts.length === 0 && (
        <Typography variant="body1" color="text.secondary">
          No alerts configured yet. Create one to get started.
        </Typography>
      )}

      {!loading && !error && alerts.length > 0 && (
        <AlertList alerts={alerts} onToggle={_handleToggle} onDelete={setPendingDelete} />
      )}

      <AlertFormDialog
        open={createOpen}
        onClose={() => setCreateOpen(false)}
        onSubmit={_handleCreate}
      />

      <ConfirmDeleteDialog
        open={pendingDelete !== null}
        itemName={pendingDelete?.name ?? ''}
        onCancel={() => setPendingDelete(null)}
        onConfirm={_handleConfirmDelete}
      />

      <Snackbar
        open={snackbarMessage !== null}
        autoHideDuration={3000}
        onClose={() => setSnackbarMessage(null)}
      >
        <MuiAlert severity="success" onClose={() => setSnackbarMessage(null)} sx={{ width: '100%' }}>
          {snackbarMessage}
        </MuiAlert>
      </Snackbar>
    </Container>
  )
}

export default AlertsPage
