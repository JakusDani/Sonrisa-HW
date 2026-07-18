import { Routes, Route } from 'react-router-dom'
import CssBaseline from '@mui/material/CssBaseline'
import NavBar from './components/NavBar'
import DashboardPage from './pages/DashboardPage'
import AlertsPage from './pages/AlertsPage'
import AdminSimulatorPage from './pages/AdminSimulatorPage'
import NotificationLogsPage from './pages/NotificationLogsPage'

function App() {
  return (
    <>
      <CssBaseline />
      <NavBar />
      <Routes>
        <Route path="/" element={<DashboardPage />} />
        <Route path="/alerts" element={<AlertsPage />} />
        <Route path="/admin" element={<AdminSimulatorPage />} />
        <Route path="/logs" element={<NotificationLogsPage />} />
      </Routes>
    </>
  )
}

export default App
