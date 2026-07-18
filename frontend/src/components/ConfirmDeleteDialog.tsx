import Dialog from '@mui/material/Dialog'
import DialogTitle from '@mui/material/DialogTitle'
import DialogContent from '@mui/material/DialogContent'
import DialogContentText from '@mui/material/DialogContentText'
import DialogActions from '@mui/material/DialogActions'
import Button from '@mui/material/Button'

interface ConfirmDeleteDialogProps {
  open: boolean
  itemName: string
  onCancel: () => void
  onConfirm: () => void
}

function ConfirmDeleteDialog({ open, itemName, onCancel, onConfirm }: ConfirmDeleteDialogProps) {
  return (
    <Dialog open={open} onClose={onCancel}>
      <DialogTitle>Delete Alert</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Are you sure you want to delete "{itemName}"? This action cannot be undone.
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={onCancel}>Cancel</Button>
        <Button onClick={onConfirm} color="error" variant="contained">
          Delete
        </Button>
      </DialogActions>
    </Dialog>
  )
}

export default ConfirmDeleteDialog
