import type { ProblemDetails } from '@/shared/api/problem-details'

export class ApiError extends Error {
  problem: ProblemDetails
  status?: number

  constructor(problem: ProblemDetails, status?: number) {
    super(problem.title ?? problem.detail ?? 'API request failed')
    this.name = 'ApiError'
    this.problem = problem
    this.status = status
  }
}
