import { useState } from 'react'
import Container from '@mui/material/Container'
import Typography from '@mui/material/Typography'
import Paper from '@mui/material/Paper'
import Stack from '@mui/material/Stack'
import Snackbar from '@mui/material/Snackbar'
import MuiAlert from '@mui/material/Alert'
import { simulateEvent } from '../api/admin'
import SimulateEventForm from '../components/SimulateEventForm'
import type { SimulateEventRequest, SimulateEventResponse } from '../types/admin'

function AdminSimulatorPage() {
  const [submitting, setSubmitting] = useState(false)
  const [result, setResult] = useState<SimulateEventResponse | null>(null)
  const [error, setError] = useState<string | null>(null)

  const _handleSubmit = async (request: SimulateEventRequest) => {
    setSubmitting(true)
    setError(null)
    try {
      const response = await simulateEvent(request)
      setResult(response)
    } catch {
      setError('Failed to simulate event.')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Admin Simulator
      </Typography>
      <Typography variant="body1" sx={{ mb: 3 }}>
        Simulate a world event to trigger matching alerts and notification dispatch.
      </Typography>

      <Stack direction={{ xs: 'column', md: 'row' }} spacing={3}>
        <Paper variant="outlined" sx={{ p: 3, flex: 1 }}>
          <SimulateEventForm submitting={submitting} onSubmit={_handleSubmit} />
        </Paper>

        {result && (
          <Paper variant="outlined" sx={{ p: 3, flex: 1 }}>
            <Typography variant="h6" gutterBottom>
              Simulation Result
            </Typography>
            <Typography variant="body1">Matched Alerts: {result.matchedAlertsCount}</Typography>
            <Typography variant="body1">
              Notifications Queued: {result.notificationsQueuedCount}
            </Typography>
          </Paper>
        )}
      </Stack>

      <Snackbar open={error !== null} autoHideDuration={4000} onClose={() => setError(null)}>
        <MuiAlert severity="error" onClose={() => setError(null)} sx={{ width: '100%' }}>
          {error}
        </MuiAlert>
      </Snackbar>
    </Container>
  )
}

export default AdminSimulatorPage
