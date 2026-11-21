import { createContext, useContext } from 'react'
import type { User } from './types'

// Data o přihlášení, ke kterým se dostane každá stránka přes useAuth().
export interface AuthContextValue {
  user: User | null
  loading: boolean
  login: (email: string, password: string) => Promise<void>
  register: (data: RegisterData) => Promise<void>
  logout: () => void
}

export interface RegisterData {
  email: string
  password: string
  displayName: string
  privacyConsent: boolean
  ageConfirmed: boolean
}

export const AuthContext = createContext<AuthContextValue | null>(null)

export function useAuth(): AuthContextValue {
  const value = useContext(AuthContext)
  if (!value) throw new Error('useAuth musí být uvnitř AuthProvider.')
  return value
}
