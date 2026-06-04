import { createBrowserRouter } from 'react-router-dom'
import { AppLayout } from '@/app/App'
import { DashboardPlaceholderPage } from '@/pages/dashboard/DashboardPlaceholderPage'
import { HomePage } from '@/pages/home/HomePage'
import { LoginPlaceholderPage } from '@/pages/login/LoginPlaceholderPage'
import { SetupPlaceholderPage } from '@/pages/setup/SetupPlaceholderPage'

export const router = createBrowserRouter([
  {
    path: '/',
    element: <AppLayout />,
    children: [
      { index: true, element: <HomePage /> },
      { path: 'setup', element: <SetupPlaceholderPage /> },
      { path: 'login', element: <LoginPlaceholderPage /> },
      { path: 'dashboard', element: <DashboardPlaceholderPage /> },
    ],
  },
])
