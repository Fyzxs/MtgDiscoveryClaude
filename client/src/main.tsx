import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { ApolloProvider } from '@apollo/client/react'
import { Auth0Provider } from '@auth0/auth0-react'
import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import './index.css'
import App from './App.tsx'
import { apolloClient } from './graphql/apollo-client'
import { theme } from './theme'
import { ErrorBoundary } from './components/ErrorBoundary'
import { Auth0TokenProvider } from './components/auth/Auth0TokenProvider'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ErrorBoundary level="page" name="Root">
      <Auth0Provider
        domain={import.meta.env.VITE_AUTH0_DOMAIN}
        clientId={import.meta.env.VITE_AUTH0_CLIENT_ID}
        authorizationParams={{
          redirect_uri: import.meta.env.VITE_AUTH0_REDIRECT_URI
        }}
      >
        <Auth0TokenProvider>
          <ThemeProvider theme={theme}>
            <CssBaseline />
            <ApolloProvider client={apolloClient}>
              <App />
            </ApolloProvider>
          </ThemeProvider>
        </Auth0TokenProvider>
      </Auth0Provider>
    </ErrorBoundary>
  </StrictMode>,
)
