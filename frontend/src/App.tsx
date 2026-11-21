import { Navigate, Route, Routes } from 'react-router'
import Layout from './components/Layout'
import RequireAuth from './components/RequireAuth'
import LoginPage from './pages/LoginPage'
import NewWorkoutPage from './pages/NewWorkoutPage'
import WorkoutDetailPage from './pages/WorkoutDetailPage'
import WorkoutsPage from './pages/WorkoutsPage'

// Která adresa zobrazí kterou stránku.
export default function App() {
  return (
    <Routes>
      <Route path="/prihlaseni" element={<LoginPage key="login" mode="login" />} />
      <Route path="/registrace" element={<LoginPage key="register" mode="register" />} />

      <Route
        element={
          <RequireAuth>
            <Layout />
          </RequireAuth>
        }
      >
        <Route path="/treninky" element={<WorkoutsPage />} />
        <Route path="/treninky/novy" element={<NewWorkoutPage />} />
        <Route path="/treninky/:id" element={<WorkoutDetailPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/treninky" replace />} />
    </Routes>
  )
}
