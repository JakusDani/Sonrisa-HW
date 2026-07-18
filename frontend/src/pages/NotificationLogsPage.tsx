import Container from '@mui/material/Container'
import Typography from '@mui/material/Typography'

function NotificationLogsPage() {
  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Notification Logs
      </Typography>
      <Typography variant="body1">Notification log history will be implemented here.</Typography>
    </Container>
  )
}

export default NotificationLogsPage
