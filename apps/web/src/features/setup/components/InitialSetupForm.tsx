import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation } from '@tanstack/react-query'
import {
  Building2,
  CircleAlert,
  Loader2,
  LockKeyhole,
  Mail,
  ShieldCheck,
  User,
} from 'lucide-react'
import { useState, type ReactNode } from 'react'
import { useForm } from 'react-hook-form'
import { useNavigate } from 'react-router-dom'
import { initializeSetup } from '@/features/setup/api/setup-api'
import { getSetupErrorMessage } from '@/features/setup/api/setup-error-messages'
import {
  initialSetupSchema,
  type InitialSetupFormValues,
} from '@/features/setup/schemas/initial-setup-schema'
import { Button } from '@/shared/ui/button'
import { Input } from '@/shared/ui/input'
import { Label } from '@/shared/ui/label'
import { cn } from '@/shared/lib/utils'

function FieldError({ message }: { message?: string }) {
  if (!message) return null

  return (
    <div className="flex items-center gap-2 text-xs text-red-300">
      <CircleAlert className="size-3.5 shrink-0" aria-hidden />
      <span>{message}</span>
    </div>
  )
}

function SetupField({
  id,
  label,
  icon: Icon,
  error,
  children,
}: {
  id: string
  label: string
  icon: typeof Building2
  error?: string
  children: ReactNode
}) {
  return (
    <div className="flex flex-col gap-1.5">
      <Label htmlFor={id} className="text-[13px] font-medium text-slate-200">
        {label}
      </Label>
      <div
        className={cn(
          'flex h-10 items-center gap-2.5 rounded-md border border-[#1D4055] bg-[#07111F] px-3 focus-within:ring-1 focus-within:ring-ring',
          error && 'border-red-800'
        )}
      >
        <Icon className="size-4 shrink-0 text-sky-400" aria-hidden />
        {children}
      </div>
      <FieldError message={error} />
    </div>
  )
}

export function InitialSetupForm() {
  const navigate = useNavigate()
  const [apiError, setApiError] = useState<{
    message: string
    traceId?: string
  } | null>(null)

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<InitialSetupFormValues>({
    resolver: zodResolver(initialSetupSchema),
    defaultValues: {
      organizationName: '',
      adminName: '',
      adminEmail: '',
      adminPassword: '',
      confirmAdminPassword: '',
      setupPassword: '',
    },
  })

  const mutation = useMutation({
    mutationFn: initializeSetup,
    onSuccess: () => {
      navigate('/login?setupCompleted=true', { replace: true })
    },
    onError: (error) => {
      setApiError(getSetupErrorMessage(error))
    },
  })

  const onSubmit = handleSubmit((values) => {
    setApiError(null)
    mutation.mutate({
      setupPassword: values.setupPassword,
      organizationName: values.organizationName.trim(),
      adminName: values.adminName.trim(),
      adminEmail: values.adminEmail.trim(),
      adminPassword: values.adminPassword,
    })
  })

  const inputClassName =
    'h-auto flex-1 border-0 bg-transparent p-0 text-sm text-foreground shadow-none focus-visible:ring-0'

  return (
    <form onSubmit={onSubmit} className="flex flex-col gap-3.5" noValidate>
      {apiError && (
        <div
          role="alert"
          className="flex flex-col gap-2 rounded-md border border-red-900 bg-[#2A0C14] px-3 py-2.5"
        >
          <div className="flex items-start gap-2">
            <CircleAlert
              className="mt-0.5 size-3.5 shrink-0 text-red-400"
              aria-hidden
            />
            <p className="text-xs font-medium text-red-200">{apiError.message}</p>
          </div>
          {apiError.traceId && (
            <p className="font-mono text-[11px] text-slate-500">
              Trace ID: {apiError.traceId}
            </p>
          )}
        </div>
      )}

      <SetupField
        id="organizationName"
        label="Organization name"
        icon={Building2}
        error={errors.organizationName?.message}
      >
        <Input
          id="organizationName"
          autoComplete="organization"
          className={inputClassName}
          {...register('organizationName')}
        />
      </SetupField>

      <SetupField
        id="adminName"
        label="Admin full name"
        icon={User}
        error={errors.adminName?.message}
      >
        <Input
          id="adminName"
          autoComplete="name"
          className={inputClassName}
          {...register('adminName')}
        />
      </SetupField>

      <SetupField
        id="adminEmail"
        label="Admin email"
        icon={Mail}
        error={errors.adminEmail?.message}
      >
        <Input
          id="adminEmail"
          type="email"
          autoComplete="email"
          className={inputClassName}
          {...register('adminEmail')}
        />
      </SetupField>

      <SetupField
        id="adminPassword"
        label="Password"
        icon={LockKeyhole}
        error={errors.adminPassword?.message}
      >
        <Input
          id="adminPassword"
          type="password"
          autoComplete="new-password"
          className={inputClassName}
          {...register('adminPassword')}
        />
      </SetupField>

      <SetupField
        id="confirmAdminPassword"
        label="Confirm password"
        icon={ShieldCheck}
        error={errors.confirmAdminPassword?.message}
      >
        <Input
          id="confirmAdminPassword"
          type="password"
          autoComplete="new-password"
          className={inputClassName}
          {...register('confirmAdminPassword')}
        />
      </SetupField>

      <div className="border-t border-[#1E3A4A] pt-3">
        <p className="mb-2 text-xs font-semibold uppercase tracking-wide text-slate-400">
          Setup access
        </p>
        <SetupField
          id="setupPassword"
          label="Setup password"
          icon={LockKeyhole}
          error={errors.setupPassword?.message}
        >
          <Input
            id="setupPassword"
            type="password"
            autoComplete="off"
            className={inputClassName}
            {...register('setupPassword')}
          />
        </SetupField>
        <p className="mt-1 text-xs text-slate-500">
          Password configured on the server for first-time setup.
        </p>
      </div>

      <div className="flex items-start gap-2.5 rounded-md border border-[#123047] bg-[#061524] px-3 py-2.5">
        <ShieldCheck className="mt-0.5 size-4 shrink-0 text-green-500" aria-hidden />
        <div className="flex flex-col gap-1 text-xs leading-snug text-slate-300">
          <p>
            Public registration is disabled. New users must be invited or created
            by an administrator.
          </p>
          <p className="text-slate-500">
            This setup screen will be disabled after the first administrator is
            created.
          </p>
          <p className="text-slate-500">
            Passwords are never stored in the browser.
          </p>
        </div>
      </div>

      <Button
        type="submit"
        disabled={mutation.isPending}
        className="h-11 w-full bg-gradient-to-r from-teal-500 via-cyan-500 to-blue-600 text-sm font-semibold text-slate-50 shadow-lg shadow-cyan-500/20 hover:opacity-95"
      >
        {mutation.isPending ? (
          <>
            <Loader2 className="size-4 animate-spin" aria-hidden />
            Creating instance…
          </>
        ) : (
          'Create instance'
        )}
      </Button>
    </form>
  )
}
