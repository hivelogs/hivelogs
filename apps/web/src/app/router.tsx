import { createBrowserRouter, Navigate } from 'react-router-dom'
import { AppLayout } from '@/app/App'
import { RootRedirect, RequireSetupComplete, RequireSetupPending } from '@/app/routes/SetupGate'
import { SetupBootstrap } from '@/app/routes/SetupBootstrap'
import { DashboardPlaceholderPage } from '@/pages/dashboard/DashboardPlaceholderPage'
import { LoginPlaceholderPage } from '@/pages/login/LoginPlaceholderPage'
import { SetupPage } from '@/pages/setup/SetupPage'

export const router = createBrowserRouter([
  {
    element: <SetupBootstrap />,
    children: [
      {
        element: <RequireSetupPending />,
        children: [{ path: '/setup', element: <SetupPage /> }],
      },
      {
        element: <RequireSetupComplete />,
        children: [
          {
            element: <AppLayout />,
            children: [
              { index: true, element: <RootRedirect /> },
              { path: 'login', element: <LoginPlaceholderPage /> },
              { path: 'dashboard', element: <DashboardPlaceholderPage /> },
            ],
          },
        ],
      },
      { path: '*', element: <Navigate to="/" replace /> },
    ],
  },
])
