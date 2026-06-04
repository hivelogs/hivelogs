import { Card, CardContent, CardHeader, CardTitle } from '@/shared/ui/card'

export function SetupPlaceholderPage() {
  return (
    <Card className="w-full max-w-lg">
      <CardHeader>
        <CardTitle>Setup</CardTitle>
      </CardHeader>
      <CardContent>
        <p className="text-muted-foreground">Initial setup screen coming next.</p>
      </CardContent>
    </Card>
  )
}
