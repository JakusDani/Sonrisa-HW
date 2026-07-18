import Stack from '@mui/material/Stack'
import Skeleton from '@mui/material/Skeleton'

interface ListSkeletonProps {
  rows?: number
}

function ListSkeleton({ rows = 4 }: ListSkeletonProps) {
  return (
    <Stack spacing={1.5} sx={{ mt: 1 }}>
      {Array.from({ length: rows }, (_, index) => (
        <Skeleton key={index} variant="rounded" height={56} />
      ))}
    </Stack>
  )
}

export default ListSkeleton
