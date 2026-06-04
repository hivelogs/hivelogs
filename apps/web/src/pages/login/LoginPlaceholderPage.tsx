import { useEffect, useState } from 'react'
import { useLocation, useSearchParams } from 'react-router-dom'
import { Card, CardContent, CardHeader, CardTitle } from '@/shared/ui/card'

export function LoginPlaceholderPage() {
  const [searchParams, setSearchParams] = useSearchParams()
  const location = useLocation()
  const [setupCompleted] = useState(
    () =>
      searchParams.get('setupCompleted') === 'true' ||
      (location.state as { setupCompleted?: boolean } | null)?.setupCompleted ===
        true
  )

  useEffect(() => {
    if (searchParams.get('setupCompleted') === 'true') {
      setSearchParams({}, { replace: true })
    }
  }, [searchParams, setSearchParams])

  return (
    <Card className="w-full max-w-lg">
      <CardHeader>
        <CardTitle>Login</CardTitle>
      </CardHeader>
      <CardContent className="flex flex-col gap-3">
        {setupCompleted && (
          <p
            role="status"
            className="rounded-md border border-[#123047] bg-[#061524] px-3 py-2 text-sm text-slate-300"
          >
            Setup completed. Login screen coming soon.
          </p>
        )}
        <p className="text-muted-foreground">Login screen coming soon.</p>
      </CardContent>
    </Card>
  )
}
