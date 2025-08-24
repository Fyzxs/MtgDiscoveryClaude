import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/graphql': {
        target: 'https://localhost:65203',
        changeOrigin: true,
        secure: false, // Allow self-signed certificates
      },
    },
  },
})
