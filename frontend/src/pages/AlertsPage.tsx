import Container from '@mui/material/Container'
import Typography from '@mui/material/Typography'

function AlertsPage() {
  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Alerts
      </Typography>
      <Typography variant="body1">Alert management will be implemented here.</Typography>
    </Container>
  )
}

export default AlertsPage
