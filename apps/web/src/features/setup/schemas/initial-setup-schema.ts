import { z } from 'zod'

const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d).{8,}$/

export const initialSetupSchema = z
  .object({
    organizationName: z
      .string()
      .min(1, 'Organization name is required.')
      .min(2, 'Organization name must be at least 2 characters.')
      .max(100, 'Organization name is too long.'),
    adminName: z
      .string()
      .min(1, 'Admin name is required.')
      .max(100, 'Admin name is too long.'),
    adminEmail: z
      .string()
      .min(1, 'Admin email is required.')
      .email('Enter a valid email address.')
      .max(320, 'Email is too long.'),
    adminPassword: z
      .string()
      .min(1, 'Password is required.')
      .min(8, 'Password must contain at least 8 characters.')
      .regex(
        passwordRegex,
        'Password must contain at least 8 characters and include at least one letter and one number.'
      ),
    confirmAdminPassword: z.string().min(1, 'Please confirm your password.'),
    setupPassword: z.string().min(1, 'Setup password is required.'),
  })
  .refine((data) => data.adminPassword === data.confirmAdminPassword, {
    message: 'Passwords do not match.',
    path: ['confirmAdminPassword'],
  })

export type InitialSetupFormValues = z.infer<typeof initialSetupSchema>
