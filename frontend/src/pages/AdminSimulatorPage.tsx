import Container from '@mui/material/Container'
import Typography from '@mui/material/Typography'

function AdminSimulatorPage() {
  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Admin Simulator
      </Typography>
      <Typography variant="body1">World event simulation will be implemented here.</Typography>
    </Container>
  )
}

export default AdminSimulatorPage
