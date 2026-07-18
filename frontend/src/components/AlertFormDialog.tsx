import { useState } from 'react'
import Dialog from '@mui/material/Dialog'
import DialogTitle from '@mui/material/DialogTitle'
import DialogContent from '@mui/material/DialogContent'
import DialogActions from '@mui/material/DialogActions'
import TextField from '@mui/material/TextField'
import Button from '@mui/material/Button'
import MenuItem from '@mui/material/MenuItem'
import FormGroup from '@mui/material/FormGroup'
import FormControlLabel from '@mui/material/FormControlLabel'
import Checkbox from '@mui/material/Checkbox'
import Stack from '@mui/material/Stack'
import { ChannelTypeValues, EventTypeValues, type ChannelType, type CreateAlertRequest, type EventType } from '../types/alert'

interface AlertFormDialogProps {
  open: boolean
  onClose: () => void
  onSubmit: (request: CreateAlertRequest) => void
}

const _eventTypeOptions = Object.values(EventTypeValues)
const _channelOptions = Object.values(ChannelTypeValues)

function AlertFormDialog({ open, onClose, onSubmit }: AlertFormDialogProps) {
  const [name, setName] = useState('')
  const [eventType, setEventType] = useState<EventType>(EventTypeValues.BreakingNews)
  const [channels, setChannels] = useState<ChannelType[]>([])

  const _toggleChannel = (channel: ChannelType) => {
    setChannels((prev) =>
      prev.includes(channel) ? prev.filter((c) => c !== channel) : [...prev, channel],
    )
  }

  const _handleSubmit = () => {
    onSubmit({ name, eventType, enabledChannels: channels })
    setName('')
    setEventType(EventTypeValues.BreakingNews)
    setChannels([])
  }

  const _isValid = name.trim().length > 0 && channels.length > 0

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
      <DialogTitle>Create Alert</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ mt: 1 }}>
          <TextField
            label="Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            fullWidth
            required
          />
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
          <FormGroup row>
            {_channelOptions.map((channel: ChannelType) => (
              <FormControlLabel
                key={channel}
                control={
                  <Checkbox
                    checked={channels.includes(channel)}
                    onChange={() => _toggleChannel(channel)}
                  />
                }
                label={channel}
              />
            ))}
          </FormGroup>
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={_handleSubmit} variant="contained" disabled={!_isValid}>
          Create
        </Button>
      </DialogActions>
    </Dialog>
  )
}

export default AlertFormDialog
