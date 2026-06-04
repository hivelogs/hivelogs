import { Card, CardContent, CardHeader, CardTitle } from '@/shared/ui/card'

export function DashboardPlaceholderPage() {
  return (
    <Card className="w-full max-w-lg">
      <CardHeader>
        <CardTitle>Dashboard</CardTitle>
      </CardHeader>
      <CardContent>
        <p className="text-muted-foreground">Dashboard coming soon.</p>
      </CardContent>
    </Card>
  )
}
