// Jediné místo, odkud frontend volá backend.

const TOKEN_KEY = 'myworkout_token'

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY)
}

export function setToken(token: string | null) {
  if (token) localStorage.setItem(TOKEN_KEY, token)
  else localStorage.removeItem(TOKEN_KEY)
}

// Chyba z backendu: hlavní hláška + seznam všech chyb (např. z validace).
export class ApiError extends Error {
  status: number
  messages: string[]

  constructor(status: number, messages: string[]) {
    super(messages[0] ?? 'Něco se pokazilo.')
    this.status = status
    this.messages = messages
  }
}

// Co se má stát, když server řekne 401 (token chybí nebo vypršel). Nastaví ho AuthProvider.
let onUnauthorized: () => void = () => {}
export function setOnUnauthorized(handler: () => void) {
  onUnauthorized = handler
}

export async function apiFetch<T>(path: string, options: { method?: string; body?: unknown } = {}): Promise<T> {
  const headers: Record<string, string> = { 'Content-Type': 'application/json' }
  const token = getToken()
  if (token) headers['Authorization'] = `Bearer ${token}`

  let response: Response
  try {
    response = await fetch(path, {
      method: options.method ?? 'GET',
      headers,
      body: options.body === undefined ? undefined : JSON.stringify(options.body),
    })
  } catch {
    throw new ApiError(0, ['Server neodpovídá. Běží backend?'])
  }

  // Tělo odpovědi převedeme z JSON. Prázdné tělo nebo text, který není JSON bere jako null.
  const text = await response.text()
  let data: unknown = null
  try {
    data = text ? JSON.parse(text) : null
  } catch {
    data = null
  }

  if (!response.ok) {
    if (response.status === 401 && token) onUnauthorized()
    throw new ApiError(response.status, readErrorMessages(response.status, data))
  }

  return data as T
}

// Z odpovědi s chybou vytáhne čitelné hlášky.
function readErrorMessages(status: number, data: unknown): string[] {
  if (data && typeof data === 'object') {
    const d = data as { message?: string; errors?: Record<string, string[]>; title?: string }
    if (d.message) return [d.message]
    if (d.errors) return Object.values(d.errors).flat()
    if (d.title) return [d.title]
  }
  if (status === 401) return ['Nejsi přihlášený.']
  if (status === 404) return ['Záznam nebyl nalezen.']
  return ['Něco se pokazilo. Zkus to znovu.']
}
