import { NavLink, Outlet } from 'react-router'
import { useAuth } from '../authContext'

// Položky menu podle designu. Hotové jsou zatím jen Tréninky, ostatní přibudou podle harmonogramu.
const MENU = [
  { label: 'Hlavní stránka', to: null },
  { label: 'Tréninky', to: '/treninky' },
  { label: 'Cviky', to: null },
  { label: 'Statistiky', to: null },
  { label: 'AI trenér', to: null },
  { label: 'Profil', to: null },
]

export default function Layout() {
  const { user, logout } = useAuth()

  return (
    <div className="app">
      <aside className="sidebar">
        <div className="logo">
          My<span>Workout</span>
        </div>
        <nav>
          {MENU.map((item) =>
            item.to ? (
              <NavLink key={item.label} to={item.to} className="menu-item">
                {item.label}
              </NavLink>
            ) : (
              <span key={item.label} className="menu-item disabled" title="Připravujeme">
                {item.label}
              </span>
            ),
          )}
        </nav>
        <div className="sidebar-user">
          <div className="muted small">Přihlášen</div>
          <div className="bold">{user?.displayName}</div>
          <button className="btn btn-secondary btn-small" onClick={logout}>
            Odhlásit se
          </button>
        </div>
      </aside>
      <main className="content">
        <Outlet />
      </main>
    </div>
  )
}
