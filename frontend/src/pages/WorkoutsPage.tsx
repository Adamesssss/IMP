import { useEffect, useState } from 'react'
import { Link } from 'react-router'
import { ApiError, apiFetch } from '../api'
import { dayAndMonth, formatKg } from '../format'
import type { WorkoutListItem } from '../types'

// Seznam mých tréninků od nejnovějšího (v designu záložka Historie).
export default function WorkoutsPage() {
  const [workouts, setWorkouts] = useState<WorkoutListItem[] | null>(null)
  const [error, setError] = useState('')

  useEffect(() => {
    apiFetch<WorkoutListItem[]>('/api/workouts')
      .then(setWorkouts)
      .catch((err) => setError(err instanceof ApiError ? err.message : 'Nepodařilo se načíst tréninky.'))
  }, [])

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Tréninky</h1>
          <p className="muted">Odcvičené tréninky od nejnovějšího</p>
        </div>
        <Link to="/treninky/novy" className="btn btn-primary">
          + Nový trénink
        </Link>
      </div>

      {error && <div className="error-box">{error}</div>}
      {workouts === null && !error && <p className="muted">Načítám…</p>}

      {workouts?.length === 0 && (
        <div className="card center">
          <p className="bold">Zatím tu nemáš žádný trénink.</p>
          <p className="muted">Zapiš první a uvidíš ho tady.</p>
          <Link to="/treninky/novy" className="btn btn-primary">
            Zapsat první trénink
          </Link>
        </div>
      )}

      <div className="list">
        {workouts?.map((w) => {
          const { day, month } = dayAndMonth(w.date)
          return (
            <Link key={w.id} to={`/treninky/${w.id}`} className="card workout-row">
              <div className="date-tile">
                <div className="date-day">{day}</div>
                <div className="muted small">{month}</div>
              </div>
              <div className="grow">
                <div className="bold">{w.name || 'Trénink'}</div>
                <div className="muted small">
                  {w.exerciseCount} {pluralCviky(w.exerciseCount)} · {w.setCount} {pluralSerie(w.setCount)}
                  {w.durationMin ? ` · ${w.durationMin} min` : ''}
                </div>
              </div>
              <div className="bold">{formatKg(w.volumeKg)}</div>
            </Link>
          )
        })}
      </div>
    </>
  )
}

// Čeština: 1 cvik, 2–4 cviky, 5+ cviků
function pluralCviky(n: number) {
  if (n === 1) return 'cvik'
  if (n >= 2 && n <= 4) return 'cviky'
  return 'cviků'
}

// 1–4 série, 5+ sérií
function pluralSerie(n: number) {
  if (n >= 1 && n <= 4) return 'série'
  return 'sérií'
}
