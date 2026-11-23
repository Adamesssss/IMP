import { useEffect, useState, type ReactNode } from 'react'
import { apiFetch, getToken, setOnUnauthorized, setToken } from './api'
import { AuthContext, type RegisterData } from './authContext'
import type { AuthResponse, User } from './types'

// Drží informaci o přihlášeném uživateli pro celou aplikaci.
export default function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null)
  // Na začátku neví jestli je uložený token platný – zeptá se backendu.
  const [loading, setLoading] = useState(getToken() !== null)

  function logout() {
    setToken(null)
    setUser(null)
  }

  useEffect(() => {
    setOnUnauthorized(logout)

    if (getToken() === null) return
    apiFetch<User>('/api/auth/me')
      .then(setUser)
      .catch(logout)
      .finally(() => setLoading(false))
  }, [])

  async function login(email: string, password: string) {
    const result = await apiFetch<AuthResponse>('/api/auth/login', {
      method: 'POST',
      body: { email, password },
    })
    setToken(result.token)
    setUser(result.user)
  }

  async function register(data: RegisterData) {
    const result = await apiFetch<AuthResponse>('/api/auth/register', { method: 'POST', body: data })
    setToken(result.token)
    setUser(result.user)
  }

  return (
    <AuthContext.Provider value={{ user, loading, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  )
}
