import { HTTPError } from 'ky'
import type {
  InitializeSetupRequest,
  InitializeSetupResponse,
  SetupStatusResponse,
} from '@/features/setup/types/setup-types'
import { parseApiError } from '@/shared/api/parse-api-error'
import { httpClient } from '@/shared/api/http-client'

export async function getSetupStatus(): Promise<SetupStatusResponse> {
  return httpClient.get('setup/status').json<SetupStatusResponse>()
}

export async function initializeSetup(
  body: InitializeSetupRequest
): Promise<InitializeSetupResponse> {
  try {
    return await httpClient
      .post('setup/initialize', { json: body })
      .json<InitializeSetupResponse>()
  } catch (error) {
    if (error instanceof HTTPError) {
      throw await parseApiError(error.response)
    }
    throw error
  }
}
