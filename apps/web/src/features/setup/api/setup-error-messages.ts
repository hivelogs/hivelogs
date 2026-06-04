import { ApiError } from '@/shared/api/api-error'

const CODE_MESSAGES: Record<string, string> = {
  'setup.invalid_setup_password':
    'The setup password is incorrect. Check the value configured on the server.',
  'setup.already_completed':
    'Initial setup has already been completed. Sign in when login is available.',
  'setup.password_not_configured':
    'Setup password is not configured on the server. Contact your administrator.',
  'users.email_already_exists':
    'A user with this email already exists. Use a different email address.',
  'setup.setup_password_required': 'Setup password is required.',
  'organizations.name_required': 'Organization name is required.',
  'organizations.name_too_long': 'Organization name is too long.',
  'users.name_required': 'Admin name is required.',
  'users.email_required': 'Admin email is required.',
  'users.email_invalid': 'Enter a valid email address.',
  'users.password_required': 'Password is required.',
  'users.password_too_weak':
    'Password must be at least 8 characters and include at least one letter and one number.',
}

export function getSetupErrorMessage(error: unknown): {
  message: string
  traceId?: string
} {
  if (!(error instanceof ApiError)) {
    return {
      message: 'Unable to reach the server. Check your connection and try again.',
    }
  }

  const apiError = error
  const code = apiError.problem.code
  const traceId = apiError.problem.traceId

  const message =
    (code && CODE_MESSAGES[code]) ||
    apiError.problem.detail ||
    apiError.problem.title ||
    'Something went wrong. Please try again.'

  return { message, traceId }
}
