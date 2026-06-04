import { Card, CardContent, CardHeader, CardTitle } from '@/shared/ui/card'

export function App() {
  return (
    <main className="flex min-h-screen items-center justify-center p-8">
      <Card className="w-full max-w-md">
        <CardHeader>
          <CardTitle>HiveLogs</CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-muted-foreground">Web foundation — Tailwind + shadcn/ui</p>
        </CardContent>
      </Card>
    </main>
  )
}
