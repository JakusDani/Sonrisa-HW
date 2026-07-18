import Container from '@mui/material/Container'
import Typography from '@mui/material/Typography'

function DashboardPage() {
  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Dashboard
      </Typography>
      <Typography variant="body1">
        Welcome to the World Events Alert System. Use the navigation bar to manage alerts,
        simulate world events, or review notification logs.
      </Typography>
    </Container>
  )
}

export default DashboardPage
