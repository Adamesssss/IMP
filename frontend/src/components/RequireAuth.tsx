import type { ReactNode } from 'react'
import { Navigate } from 'react-router'
import { useAuth } from '../authContext'

// Obalí stránky, které vidí jen přihlášený uživatel.
export default function RequireAuth({ children }: { children: ReactNode }) {
  const { user, loading } = useAuth()

  if (loading) return <p className="center muted">Načítám…</p>
  if (!user) return <Navigate to="/prihlaseni" replace />
  return children
}
