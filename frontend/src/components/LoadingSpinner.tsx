import Box from '@mui/material/Box'
import CircularProgress from '@mui/material/CircularProgress'

function LoadingSpinner() {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
      <CircularProgress />
    </Box>
  )
}

export default LoadingSpinner
