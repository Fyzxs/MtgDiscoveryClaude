import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    watch: {
      usePolling: true, // Enable polling for WSL2
    },
    proxy: {
      '/graphql': {
        target: 'https://localhost:65203',
        changeOrigin: true,
        secure: false, // Allow self-signed certificates
      },
    },
  },
  build: {
    // Use esbuild for faster builds with console removal
    minify: 'esbuild',
    // Note: esbuild's drop option removes ALL console.* calls including warn/error
    // Our logger utility handles this properly by checking isDevelopment
    // In production, only logger.warn() and logger.error() remain via tree-shaking

    // Increase chunk size warning limit (we're using code splitting)
    chunkSizeWarningLimit: 800,

    // Manual chunking for better caching and performance
    rollupOptions: {
      output: {
        manualChunks: {
          // React core
          'react-vendor': ['react', 'react-dom', 'react-router-dom'],

          // Material-UI core
          'mui-core': ['@mui/material', '@mui/system'],

          // Material-UI icons (separate chunk due to size)
          'mui-icons': ['@mui/icons-material'],

          // Apollo Client and GraphQL
          'apollo': ['@apollo/client', 'graphql'],

          // Auth0
          'auth0': ['@auth0/auth0-react'],
        },
      },
    },
  },
})
