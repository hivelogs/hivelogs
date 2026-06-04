import { createContext, useContext } from 'react'
import { Outlet } from 'react-router-dom'
import { useSetupStatus } from '@/features/setup/hooks/use-setup-status'
import type { SetupStatusResponse } from '@/features/setup/types/setup-types'

type SetupBootstrapContextValue = {
  statusQuery: ReturnType<typeof useSetupStatus>
}

const SetupBootstrapContext = createContext<SetupBootstrapContextValue | null>(
  null
)

export function useSetupBootstrap() {
  const ctx = useContext(SetupBootstrapContext)
  if (!ctx) {
    throw new Error('useSetupBootstrap must be used within SetupBootstrap')
  }
  return ctx
}

function SetupLoadingScreen() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-background">
      <p className="text-sm text-muted-foreground">Loading HiveLogs…</p>
    </div>
  )
}

function SetupNetworkError({ onRetry }: { onRetry: () => void }) {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center gap-4 bg-background px-6">
      <p className="text-center text-sm text-muted-foreground">
        Unable to reach the server. Check your connection and API URL.
      </p>
      <button
        type="button"
        onClick={onRetry}
        className="text-sm font-medium text-primary hover:underline"
      >
        Try again
      </button>
    </div>
  )
}

export function SetupBootstrap() {
  const statusQuery = useSetupStatus()

  if (statusQuery.isPending) {
    return <SetupLoadingScreen />
  }

  if (statusQuery.isError) {
    return <SetupNetworkError onRetry={() => statusQuery.refetch()} />
  }

  return (
    <SetupBootstrapContext.Provider value={{ statusQuery }}>
      <Outlet />
    </SetupBootstrapContext.Provider>
  )
}

export function useSetupStatusData(): SetupStatusResponse {
  const { statusQuery } = useSetupBootstrap()
  if (!statusQuery.data) {
    throw new Error('Setup status is not available')
  }
  return statusQuery.data
}
