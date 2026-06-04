import { useQuery } from '@tanstack/react-query'
import { getSetupStatus } from '@/features/setup/api/setup-api'

export const setupStatusQueryKey = ['setup', 'status'] as const

export function useSetupStatus() {
  return useQuery({
    queryKey: setupStatusQueryKey,
    queryFn: getSetupStatus,
    staleTime: 30_000,
  })
}
