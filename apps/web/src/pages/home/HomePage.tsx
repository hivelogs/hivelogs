import { Card, CardContent, CardHeader, CardTitle } from '@/shared/ui/card'

export function HomePage() {
  return (
    <Card className="w-full max-w-lg">
      <CardHeader>
        <CardTitle>HiveLogs Web App Foundation</CardTitle>
      </CardHeader>
      <CardContent>
        <p className="text-muted-foreground">
          Admin dashboard foundation. Initial setup screen ships in the next feature.
        </p>
      </CardContent>
    </Card>
  )
}
