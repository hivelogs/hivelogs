import { ApiError } from '@/shared/api/api-error'
import type { ProblemDetails } from '@/shared/api/problem-details'

function extractProblemDetails(body: Record<string, unknown>): ProblemDetails {
  const extensions = body.extensions as Record<string, unknown> | undefined

  return {
    type: typeof body.type === 'string' ? body.type : undefined,
    title: typeof body.title === 'string' ? body.title : undefined,
    status: typeof body.status === 'number' ? body.status : undefined,
    detail: typeof body.detail === 'string' ? body.detail : undefined,
    instance: typeof body.instance === 'string' ? body.instance : undefined,
    code:
      typeof body.code === 'string'
        ? body.code
        : typeof extensions?.code === 'string'
          ? extensions.code
          : undefined,
    traceId:
      typeof body.traceId === 'string'
        ? body.traceId
        : typeof extensions?.traceId === 'string'
          ? extensions.traceId
          : undefined,
    errors:
      body.errors && typeof body.errors === 'object'
        ? (body.errors as Record<string, string[]>)
        : undefined,
  }
}

export async function parseApiError(response: Response): Promise<ApiError> {
  let problem: ProblemDetails = {
    title: response.statusText || 'API request failed',
    status: response.status,
  }

  const contentType = response.headers.get('content-type') ?? ''
  if (contentType.includes('application/problem+json') || contentType.includes('application/json')) {
    try {
      const body = (await response.json()) as Record<string, unknown>
      problem = { ...problem, ...extractProblemDetails(body) }
    } catch {
      // keep default problem
    }
  }

  return new ApiError(problem, response.status)
}
