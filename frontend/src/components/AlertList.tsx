import List from '@mui/material/List'
import ListItem from '@mui/material/ListItem'
import ListItemText from '@mui/material/ListItemText'
import Switch from '@mui/material/Switch'
import Button from '@mui/material/Button'
import Chip from '@mui/material/Chip'
import Stack from '@mui/material/Stack'
import Typography from '@mui/material/Typography'
import type { Alert } from '../types/alert'

interface AlertListProps {
  alerts: Alert[]
  onToggle: (alert: Alert) => void
  onDelete: (alert: Alert) => void
}

function AlertList({ alerts, onToggle, onDelete }: AlertListProps) {
  return (
    <List>
      {alerts.map((alert) => (
        <ListItem
          key={alert.id}
          divider
          secondaryAction={
            <Stack direction="row" spacing={1} sx={{ alignItems: 'center' }}>
              <Switch checked={alert.isEnabled} onChange={() => onToggle(alert)} />
              <Button color="error" size="small" onClick={() => onDelete(alert)}>
                Delete
              </Button>
            </Stack>
          }
        >
          <ListItemText
            primary={alert.name}
            secondary={
              <Stack direction="row" spacing={1} sx={{ mt: 0.5 }}>
                <Typography variant="body2" component="span">
                  {alert.eventType}
                </Typography>
                {alert.enabledChannels.map((channel) => (
                  <Chip key={channel} label={channel} size="small" />
                ))}
              </Stack>
            }
          />
        </ListItem>
      ))}
    </List>
  )
}

export default AlertList
