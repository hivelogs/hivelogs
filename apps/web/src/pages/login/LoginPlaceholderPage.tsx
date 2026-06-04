import { Card, CardContent, CardHeader, CardTitle } from '@/shared/ui/card'

export function LoginPlaceholderPage() {
  return (
    <Card className="w-full max-w-lg">
      <CardHeader>
        <CardTitle>Login</CardTitle>
      </CardHeader>
      <CardContent>
        <p className="text-muted-foreground">Login screen coming soon.</p>
      </CardContent>
    </Card>
  )
}
