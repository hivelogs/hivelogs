import ky from 'ky'
import { env } from '@/shared/config/env'

export const httpClient = ky.create({
  prefixUrl: env.apiBaseUrl,
  timeout: 30_000,
})
