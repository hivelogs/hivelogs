import { Navigate, Outlet } from 'react-router-dom'
import { useSetupStatusData } from '@/app/routes/SetupBootstrap'

export function RequireSetupPending() {
  const { setupRequired } = useSetupStatusData()

  if (!setupRequired) {
    return <Navigate to="/login" replace />
  }

  return <Outlet />
}

export function RequireSetupComplete() {
  const { setupRequired } = useSetupStatusData()

  if (setupRequired) {
    return <Navigate to="/setup" replace />
  }

  return <Outlet />
}

export function RootRedirect() {
  return <Navigate to="/login" replace />
}
