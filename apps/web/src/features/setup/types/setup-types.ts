export type SetupStatus = 'SetupRequired' | 'Configured'

export type SetupStatusResponse = {
  status: SetupStatus
  setupRequired: boolean
}

export type InitializeSetupRequest = {
  setupPassword: string
  organizationName: string
  adminName: string
  adminEmail: string
  adminPassword: string
}

export type SetupOrganizationSummary = {
  id: string
  name: string
}

export type SetupAdminUserSummary = {
  id: string
  name: string
  email: string
  role: string
  mustChangePassword: boolean
}

export type InitializeSetupResponse = {
  setupCompleted: boolean
  organization: SetupOrganizationSummary
  adminUser: SetupAdminUserSummary
}
