import logoUrl from '@/assets/logo.svg'
import { InitialSetupForm } from '@/features/setup/components/InitialSetupForm'

export function SetupPage() {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center bg-[#020617] px-6 py-11">
      <div className="flex w-full max-w-[500px] flex-col gap-5">
        <div className="flex flex-col items-center gap-3">
          <div className="flex items-center gap-3">
            <img src={logoUrl} alt="" className="size-11" aria-hidden />
            <span className="text-2xl font-bold tracking-tight text-slate-50">
              HiveLogs
            </span>
          </div>
          <div className="flex w-full flex-col gap-2.5 text-center">
            <h1 className="text-[30px] font-bold leading-tight text-slate-50">
              Set up your HiveLogs instance
            </h1>
            <p className="text-[15px] leading-relaxed text-slate-400">
              Create the first administrator account and organization for this
              self-hosted instance.
            </p>
          </div>
        </div>

        <div className="rounded-lg border border-[#1E3A4A] bg-[#0F172A] p-5 shadow-2xl shadow-black/30">
          <InitialSetupForm />
        </div>

        <p className="text-center font-mono text-[11px] text-slate-500">
          v0.1.0-alpha
        </p>
      </div>
    </div>
  )
}
