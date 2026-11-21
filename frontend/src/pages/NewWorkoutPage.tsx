import { useEffect, useState, type FormEvent } from 'react'
import { Link, useNavigate } from 'react-router'
import { ApiError, apiFetch } from '../api'
import { formatKg, todayIso } from '../format'
import type { CreateWorkoutRequest, Exercise, WorkoutDetail } from '../types'

// Série a cviky ve formuláři držíme jako text (tak, jak je uživatel napsal).
// Na čísla je převedeme až při uložení.
interface SetForm {
  reps: string
  weightKg: string
}

interface ExerciseForm {
  exercise: Exercise
  sets: SetForm[]
}

export default function NewWorkoutPage() {
  const navigate = useNavigate()

  const [allExercises, setAllExercises] = useState<Exercise[]>([])
  const [date, setDate] = useState(todayIso())
  const [name, setName] = useState('')
  const [durationMin, setDurationMin] = useState('')
  const [note, setNote] = useState('')
  const [items, setItems] = useState<ExerciseForm[]>([])
  const [pickedId, setPickedId] = useState('')
  const [errors, setErrors] = useState<string[]>([])
  const [showFieldErrors, setShowFieldErrors] = useState(false)
  const [saving, setSaving] = useState(false)

  // Po otevření stránky načteme databázi cviků pro výběr.
  useEffect(() => {
    apiFetch<Exercise[]>('/api/exercises')
      .then(setAllExercises)
      .catch(() => setErrors(['Nepodařilo se načíst databázi cviků.']))
  }, [])

  // ---------- Úpravy formuláře ----------

  function addExercise() {
    const exercise = allExercises.find((e) => e.id === Number(pickedId))
    if (!exercise) return
    setItems([...items, { exercise, sets: [{ reps: '', weightKg: '' }] }])
    setPickedId('')
  }

  function removeExercise(index: number) {
    setItems(items.filter((_, i) => i !== index))
  }

  function addSet(exerciseIndex: number) {
    setItems(
      items.map((item, i) => {
        if (i !== exerciseIndex) return item
        // Nová série převezme hodnoty z poslední série (většinou se cvičí stejně).
        const last = item.sets[item.sets.length - 1] ?? { reps: '', weightKg: '' }
        return { ...item, sets: [...item.sets, { ...last }] }
      }),
    )
  }

  function removeSet(exerciseIndex: number, setIndex: number) {
    setItems(
      items.map((item, i) =>
        i === exerciseIndex ? { ...item, sets: item.sets.filter((_, j) => j !== setIndex) } : item,
      ),
    )
  }

  function changeSet(exerciseIndex: number, setIndex: number, field: keyof SetForm, value: string) {
    setItems(
      items.map((item, i) =>
        i === exerciseIndex
          ? { ...item, sets: item.sets.map((s, j) => (j === setIndex ? { ...s, [field]: value } : s)) }
          : item,
      ),
    )
  }

  // ---------- Kontrola hodnot ----------

  function repsError(value: string): string | null {
    if (value.trim() === '') return 'Zadej opakování'
    const n = Number(value)
    if (!Number.isInteger(n) || n <= 0) return 'Musí být větší než 0'
    if (n > 1000) return 'Nejvýš 1000'
    return null
  }

  function weightError(value: string): string | null {
    if (value.trim() === '') return 'Zadej váhu (0 = vlastní váha)'
    const n = Number(value.replace(',', '.'))
    if (Number.isNaN(n) || n < 0) return 'Nesmí být záporná'
    if (n > 1000) return 'Nejvýš 1000 kg'
    return null
  }

  function validate(): string[] {
    const list: string[] = []
    if (!date) list.push('Zadej datum tréninku.')
    if (durationMin.trim() !== '') {
      const d = Number(durationMin)
      if (!Number.isInteger(d) || d < 1 || d > 600) list.push('Délka musí být 1 až 600 minut.')
    }
    if (items.length === 0) list.push('Přidej aspoň jeden cvik.')
    for (const item of items) {
      if (item.sets.length === 0) list.push(`${item.exercise.name}: přidej aspoň jednu sérii.`)
      if (item.sets.some((s) => repsError(s.reps) || weightError(s.weightKg)))
        list.push(`${item.exercise.name}: oprav červeně označené hodnoty.`)
    }
    return list
  }

  // Objem = součet (opakování × váha) přes všechny vyplněné série.
  const volume = items
    .flatMap((item) => item.sets)
    .reduce((sum, s) => {
      const reps = Number(s.reps)
      const weight = Number(s.weightKg.replace(',', '.'))
      return repsError(s.reps) || weightError(s.weightKg) ? sum : sum + reps * weight
    }, 0)
  const setCount = items.reduce((sum, item) => sum + item.sets.length, 0)

  // ---------- Uložení ----------

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setShowFieldErrors(true)
    const list = validate()
    setErrors(list)
    if (list.length > 0) return

    const body: CreateWorkoutRequest = {
      date,
      name: name.trim() || null,
      note: note.trim() || null,
      durationMin: durationMin.trim() ? Number(durationMin) : null,
      exercises: items.map((item) => ({
        exerciseId: item.exercise.id,
        sets: item.sets.map((s) => ({ reps: Number(s.reps), weightKg: Number(s.weightKg.replace(',', '.')) })),
      })),
    }

    setSaving(true)
    try {
      const saved = await apiFetch<WorkoutDetail>('/api/workouts', { method: 'POST', body })
      navigate(`/treninky/${saved.id}`, { state: { justSaved: true } })
    } catch (err) {
      setErrors(err instanceof ApiError ? err.messages : ['Trénink se nepodařilo uložit.'])
    } finally {
      setSaving(false)
    }
  }

  // Cviky seskupené podle svalové skupiny pro rozbalovací seznam.
  const groups = [...new Set(allExercises.map((e) => e.muscleGroup))]

  return (
    <form className="narrow" onSubmit={handleSubmit} noValidate>
      <Link to="/treninky" className="back-link">
        ← Zpět na tréninky
      </Link>
      <h1>Nový trénink</h1>

      <div className="card stack">
        <div className="row wrap">
          <label className="field grow">
            <span>Datum</span>
            <input type="date" value={date} onChange={(e) => setDate(e.target.value)} />
          </label>
          <label className="field grow-2">
            <span>Název</span>
            <input value={name} onChange={(e) => setName(e.target.value)} placeholder="např. Push den" maxLength={100} />
          </label>
          <label className="field grow">
            <span>Délka (min)</span>
            <input
              type="number"
              min={1}
              max={600}
              value={durationMin}
              onChange={(e) => setDurationMin(e.target.value)}
            />
          </label>
        </div>
        <label className="field">
          <span>Poznámka</span>
          <textarea
            value={note}
            onChange={(e) => setNote(e.target.value)}
            placeholder="Jak ses cítil, co příště zlepšit…"
            maxLength={1000}
            rows={2}
          />
        </label>
      </div>

      {items.map((item, i) => (
        <div className="card stack" key={i}>
          <div className="row space-between">
            <div>
              <h2>
                {i + 1} · {item.exercise.name}
              </h2>
              <div className="muted small">
                {item.exercise.muscleGroup}
                {item.exercise.equipment ? ` · ${item.exercise.equipment}` : ''}
              </div>
            </div>
            <button type="button" className="btn btn-danger" onClick={() => removeExercise(i)}>
              Odebrat cvik
            </button>
          </div>

          <div className="set-row set-head muted small">
            <span>Série</span>
            <span>Váha (kg)</span>
            <span>Opakování</span>
            <span />
          </div>
          {item.sets.map((s, j) => {
            const wErr = showFieldErrors ? weightError(s.weightKg) : null
            const rErr = showFieldErrors ? repsError(s.reps) : null
            return (
              <div className="set-row" key={j}>
                <span className="bold">{j + 1}</span>
                <label className="field">
                  <input
                    type="number"
                    step="0.5"
                    min={0}
                    aria-label={`Váha, série ${j + 1}`}
                    className={wErr ? 'invalid' : ''}
                    value={s.weightKg}
                    onChange={(e) => changeSet(i, j, 'weightKg', e.target.value)}
                  />
                  {wErr && <small className="error-text">{wErr}</small>}
                </label>
                <label className="field">
                  <input
                    type="number"
                    step="1"
                    min={1}
                    aria-label={`Opakování, série ${j + 1}`}
                    className={rErr ? 'invalid' : ''}
                    value={s.reps}
                    onChange={(e) => changeSet(i, j, 'reps', e.target.value)}
                  />
                  {rErr && <small className="error-text">{rErr}</small>}
                </label>
                <button
                  type="button"
                  className="icon-btn"
                  aria-label={`Smazat sérii ${j + 1}`}
                  onClick={() => removeSet(i, j)}
                >
                  ×
                </button>
              </div>
            )
          })}
          <button type="button" className="link-btn" onClick={() => addSet(i)}>
            + Přidat sérii
          </button>
        </div>
      ))}

      <div className="card row wrap add-exercise">
        <label className="field grow">
          <span>Přidat cvik z databáze</span>
          <select value={pickedId} onChange={(e) => setPickedId(e.target.value)}>
            <option value="">Vyber cvik…</option>
            {groups.map((g) => (
              <optgroup key={g} label={g}>
                {allExercises
                  .filter((e) => e.muscleGroup === g)
                  .map((e) => (
                    <option key={e.id} value={e.id}>
                      {e.name}
                    </option>
                  ))}
              </optgroup>
            ))}
          </select>
        </label>
        <button type="button" className="btn btn-secondary" onClick={addExercise} disabled={!pickedId}>
          + Přidat cvik
        </button>
      </div>

      {errors.length > 0 && (
        <div className="error-box" role="alert">
          {errors.map((m) => (
            <div key={m}>{m}</div>
          ))}
        </div>
      )}

      <div className="footer-bar">
        <div>
          Zvednuto celkem: <span className="bold">{formatKg(volume)}</span> · {setCount}{' '}
          {setCount >= 1 && setCount <= 4 ? 'série' : 'sérií'}
        </div>
        <div className="row">
          <Link to="/treninky" className="btn btn-secondary">
            Zrušit
          </Link>
          <button className="btn btn-primary" disabled={saving}>
            {saving ? 'Ukládám…' : 'Uložit trénink'}
          </button>
        </div>
      </div>
    </form>
  )
}
