import { useState, type FormEvent } from 'react'
import { Link, Navigate, useNavigate } from 'react-router'
import { ApiError } from '../api'
import { useAuth } from '../authContext'

// Stejná stránka slouží pro přihlášení i registraci (záložky v designu).
export default function LoginPage({ mode }: { mode: 'login' | 'register' }) {
  const { user, login, register } = useAuth()
  const navigate = useNavigate()
  const isRegister = mode === 'register'

  const [email, setEmail] = useState('')
  const [displayName, setDisplayName] = useState('')
  const [password, setPassword] = useState('')
  const [password2, setPassword2] = useState('')
  const [privacyConsent, setPrivacyConsent] = useState(false)
  const [ageConfirmed, setAgeConfirmed] = useState(false)
  const [errors, setErrors] = useState<string[]>([])
  const [sending, setSending] = useState(false)

  // Přihlášeného uživatele rovnou pošlu na tréninky.
  if (user) return <Navigate to="/treninky" replace />

  // Kontrola ve frontendu jen pro pohodlí uživatele. Kontrolu jako takovou dělá backend.
  function validate(): string[] {
    const list: string[] = []
    if (!email.trim()) list.push('Zadej e-mail.')
    if (!password) list.push('Zadej heslo.')
    if (isRegister) {
      if (!displayName.trim()) list.push('Zadej jméno.')
      if (password && !/^(?=.*\p{Lu})(?=.*\d).{8,100}$/u.test(password))
        list.push('Heslo musí mít aspoň 8 znaků, jedno velké písmeno a číslici.')
      if (password !== password2) list.push('Hesla se neshodují.')
      if (!privacyConsent) list.push('Bez souhlasu se zásadami nelze účet založit.')
      if (!ageConfirmed) list.push('Aplikaci můžeš používat od 15 let.')
    }
    return list
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    const list = validate()
    setErrors(list)
    if (list.length > 0) return

    setSending(true)
    try {
      if (isRegister) {
        await register({ email, password, displayName, privacyConsent, ageConfirmed })
      } else {
        await login(email, password)
      }
      navigate('/treninky')
    } catch (err) {
      setErrors(err instanceof ApiError ? err.messages : ['Něco se pokazilo.'])
    } finally {
      setSending(false)
    }
  }

  return (
    <div className="auth-page">
      <div className="auth-box">
        <div className="auth-header">
          <div className="logo big">
            My<span>Workout</span>
          </div>
          <p className="muted">Deník tréninků s AI trenérem, který zná tvůj progres</p>
        </div>

        <form className="card" onSubmit={handleSubmit} noValidate>
          <div className="tabs">
            <Link to="/prihlaseni" className={isRegister ? 'tab' : 'tab active'}>
              Přihlášení
            </Link>
            <Link to="/registrace" className={isRegister ? 'tab active' : 'tab'}>
              Registrace
            </Link>
          </div>

          <label className="field">
            <span>E-mail</span>
            <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} autoComplete="email" />
          </label>

          {isRegister && (
            <label className="field">
              <span>Jméno</span>
              <input value={displayName} onChange={(e) => setDisplayName(e.target.value)} maxLength={50} />
            </label>
          )}

          <div className="field">
            <label className="field">
              <span>Heslo</span>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                autoComplete={isRegister ? 'new-password' : 'current-password'}
              />
            </label>
            {isRegister && <small className="muted">Aspoň 8 znaků, jedno velké písmeno a číslice</small>}
          </div>

          {isRegister && (
            <>
              <label className="field">
                <span>Heslo znovu</span>
                <input
                  type="password"
                  value={password2}
                  onChange={(e) => setPassword2(e.target.value)}
                  autoComplete="new-password"
                />
              </label>
              <label className="checkbox">
                <input type="checkbox" checked={privacyConsent} onChange={(e) => setPrivacyConsent(e.target.checked)} />
                <span>
                  Souhlasím se zásadami ochrany osobních údajů a se zpracováním údajů o tréninku a těle AI
                  trenérem
                </span>
              </label>
              <label className="checkbox">
                <input type="checkbox" checked={ageConfirmed} onChange={(e) => setAgeConfirmed(e.target.checked)} />
                <span>Je mi alespoň 15 let</span>
              </label>
            </>
          )}

          {errors.length > 0 && (
            <div className="error-box" role="alert">
              {errors.map((m) => (
                <div key={m}>{m}</div>
              ))}
            </div>
          )}

          <button className="btn btn-primary btn-wide" disabled={sending}>
            {sending ? 'Odesílám…' : isRegister ? 'Vytvořit účet' : 'Přihlásit se'}
          </button>
        </form>

        <p className="center small muted">Tvoje tréninky vidíš jen ty.</p>
      </div>
    </div>
  )
}
