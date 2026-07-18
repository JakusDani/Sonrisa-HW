import AppBar from '@mui/material/AppBar'
import Toolbar from '@mui/material/Toolbar'
import Typography from '@mui/material/Typography'
import Button from '@mui/material/Button'
import Stack from '@mui/material/Stack'
import { NavLink } from 'react-router-dom'

const _navLinks = [
  { label: 'Dashboard', to: '/' },
  { label: 'Alerts', to: '/alerts' },
  { label: 'Admin Simulator', to: '/admin' },
  { label: 'Notification Logs', to: '/logs' },
]

function NavBar() {
  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          World Events Alert System
        </Typography>
        <Stack direction="row" spacing={1}>
          {_navLinks.map((link) => (
            <Button
              key={link.to}
              component={NavLink}
              to={link.to}
              color="inherit"
              sx={{
                '&.active': {
                  textDecoration: 'underline',
                  fontWeight: 'bold',
                },
              }}
            >
              {link.label}
            </Button>
          ))}
        </Stack>
      </Toolbar>
    </AppBar>
  )
}

export default NavBar
