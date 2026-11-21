import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    // Požadavky na /api přepošle vývojový server na backend v ASP.NET.
    proxy: {
      '/api': 'http://localhost:5080',
    },
  },
})
