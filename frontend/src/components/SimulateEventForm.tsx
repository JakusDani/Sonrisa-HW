import { useState } from 'react'
import TextField from '@mui/material/TextField'
import MenuItem from '@mui/material/MenuItem'
import Button from '@mui/material/Button'
import Stack from '@mui/material/Stack'
import { EventTypeValues, type EventType } from '../types/alert'
import type { SimulateEventRequest } from '../types/admin'

interface SimulateEventFormProps {
  submitting: boolean
  onSubmit: (request: SimulateEventRequest) => void
}

const _eventTypeOptions = Object.values(EventTypeValues)

function SimulateEventForm({ submitting, onSubmit }: SimulateEventFormProps) {
  const [eventType, setEventType] = useState<EventType>(EventTypeValues.BreakingNews)
  const [message, setMessage] = useState('')

  const _isValid = message.trim().length > 0

  const _handleSubmit = () => {
    onSubmit({ eventType, message })
    setMessage('')
  }

  return (
    <Stack spacing={2} sx={{ maxWidth: 480 }}>
      <TextField
        select
        label="Event Type"
        value={eventType}
        onChange={(e) => setEventType(e.target.value as EventType)}
        fullWidth
      >
        {_eventTypeOptions.map((option: EventType) => (
          <MenuItem key={option} value={option}>
            {option}
          </MenuItem>
        ))}
      </TextField>
      <TextField
        label="Message"
        value={message}
        onChange={(e) => setMessage(e.target.value)}
        multiline
        minRows={3}
        fullWidth
        required
      />
      <Button
        variant="contained"
        onClick={_handleSubmit}
        disabled={!_isValid || submitting}
      >
        Fire Event
      </Button>
    </Stack>
  )
}

export default SimulateEventForm
