// Typy dat, která posílá backend. Odpovídají třídám v MyWorkout.Api/Dtos.

export interface User {
  id: number
  email: string
  displayName: string
  role: string
}

export interface AuthResponse {
  token: string
  expiresAt: string
  user: User
}

export interface Exercise {
  id: number
  name: string
  muscleGroup: string
  equipment: string | null
  difficulty: string | null
  description: string | null
}

export interface WorkoutListItem {
  id: number
  date: string
  name: string | null
  durationMin: number | null
  exerciseCount: number
  setCount: number
  volumeKg: number
}

export interface WorkoutSet {
  setNumber: number
  reps: number
  weightKg: number
}

export interface WorkoutExercise {
  exerciseId: number
  name: string
  muscleGroup: string
  equipment: string | null
  orderIndex: number
  sets: WorkoutSet[]
}

export interface WorkoutDetail {
  id: number
  date: string
  name: string | null
  note: string | null
  durationMin: number | null
  volumeKg: number
  exercises: WorkoutExercise[]
}

// Co posílá při uložení nového tréninku.
export interface CreateWorkoutRequest {
  date: string
  name: string | null
  note: string | null
  durationMin: number | null
  exercises: {
    exerciseId: number
    sets: { reps: number; weightKg: number }[]
  }[]
}
