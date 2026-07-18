import Table from '@mui/material/Table'
import TableHead from '@mui/material/TableHead'
import TableBody from '@mui/material/TableBody'
import TableRow from '@mui/material/TableRow'
import TableCell from '@mui/material/TableCell'
import TableContainer from '@mui/material/TableContainer'
import Paper from '@mui/material/Paper'
import Chip from '@mui/material/Chip'
import { NotificationStatusValues } from '../types/log'
import type { NotificationLog } from '../types/log'

interface NotificationLogsTableProps {
  logs: NotificationLog[]
}

const _formatTimestamp = (timestampUtc: string) => new Date(timestampUtc).toLocaleString()

function NotificationLogsTable({ logs }: NotificationLogsTableProps) {
  return (
    <TableContainer component={Paper} variant="outlined">
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Time</TableCell>
            <TableCell>Alert</TableCell>
            <TableCell>Channel</TableCell>
            <TableCell>Message</TableCell>
            <TableCell>Status</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {logs.map((log) => (
            <TableRow key={log.id}>
              <TableCell>{_formatTimestamp(log.timestampUtc)}</TableCell>
              <TableCell>{log.alertName}</TableCell>
              <TableCell>{log.channel}</TableCell>
              <TableCell>{log.message}</TableCell>
              <TableCell>
                <Chip
                  label={log.status}
                  size="small"
                  color={log.status === NotificationStatusValues.Success ? 'success' : 'error'}
                />
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  )
}

export default NotificationLogsTable
