import { useEffect, useState } from 'react'
import { Link, useLocation, useParams } from 'react-router'
import { ApiError, apiFetch } from '../api'
import { formatDate, formatKg } from '../format'
import type { WorkoutDetail } from '../types'

// Zobrazení jednoho uloženého tréninku.
export default function WorkoutDetailPage() {
  const { id } = useParams()
  const location = useLocation()
  const justSaved = (location.state as { justSaved?: boolean } | null)?.justSaved === true

  const [workout, setWorkout] = useState<WorkoutDetail | null>(null)
  const [error, setError] = useState('')

  useEffect(() => {
    apiFetch<WorkoutDetail>(`/api/workouts/${id}`)
      .then(setWorkout)
      .catch((err) => setError(err instanceof ApiError ? err.message : 'Trénink se nepodařilo načíst.'))
  }, [id])

  return (
    <div className="narrow">
      <Link to="/treninky" className="back-link">
        ← Zpět na tréninky
      </Link>

      {justSaved && <div className="success-box">Trénink je uložený.</div>}
      {error && <div className="error-box">{error}</div>}
      {!workout && !error && <p className="muted">Načítám…</p>}

      {workout && (
        <>
          <h1>{workout.name || 'Trénink'}</h1>
          <p className="muted">
            {formatDate(workout.date)}
            {workout.durationMin ? ` · ${workout.durationMin} min` : ''} · zvednuto celkem{' '}
            <span className="bold">{formatKg(workout.volumeKg)}</span>
          </p>
          {workout.note && <div className="card">{workout.note}</div>}

          {workout.exercises.map((ex) => (
            <div className="card stack" key={ex.orderIndex}>
              <div>
                <h2>
                  {ex.orderIndex} · {ex.name}
                </h2>
                <div className="muted small">
                  {ex.muscleGroup}
                  {ex.equipment ? ` · ${ex.equipment}` : ''}
                </div>
              </div>
              <table className="sets-table">
                <thead>
                  <tr>
                    <th>Série</th>
                    <th>Váha</th>
                    <th>Opakování</th>
                  </tr>
                </thead>
                <tbody>
                  {ex.sets.map((s) => (
                    <tr key={s.setNumber}>
                      <td>{s.setNumber}</td>
                      <td>{formatKg(s.weightKg)}</td>
                      <td>{s.reps}×</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ))}
        </>
      )}
    </div>
  )
}
