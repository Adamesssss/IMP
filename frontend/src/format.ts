// Pomocné funkce pro zobrazení čísel a dat česky.

// "2026-11-20" -> "20. 11. 2026"
export function formatDate(isoDate: string): string {
  const [year, month, day] = isoDate.split('-').map(Number)
  return `${day}. ${month}. ${year}`
}

// "2026-11-20" -> { day: "20", month: "lis" }
const MONTHS = ['led', 'úno', 'bře', 'dub', 'kvě', 'čvn', 'čvc', 'srp', 'zář', 'říj', 'lis', 'pro']
export function dayAndMonth(isoDate: string): { day: string; month: string } {
  const [, month, day] = isoDate.split('-').map(Number)
  return { day: String(day), month: MONTHS[month - 1] }
}

// 3420 -> "3 420 kg", 82.5 -> "82,5 kg"
export function formatKg(value: number): string {
  return `${value.toLocaleString('cs-CZ', { maximumFractionDigits: 2 })} kg`
}

// Dnešní datum ve tvaru "2026-11-20" podle místního času (pro <input type="date">).
export function todayIso(): string {
  const d = new Date()
  const month = String(d.getMonth() + 1).padStart(2, '0')
  const day = String(d.getDate()).padStart(2, '0')
  return `${d.getFullYear()}-${month}-${day}`
}
