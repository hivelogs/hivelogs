import { Outlet } from 'react-router-dom'

export function AppLayout() {
  return (
    <div className="flex min-h-screen flex-col">
      <header className="border-b border-border px-6 py-4">
        <span className="text-lg font-semibold tracking-tight">HiveLogs</span>
      </header>
      <main className="flex flex-1 items-center justify-center p-8">
        <Outlet />
      </main>
    </div>
  )
}
