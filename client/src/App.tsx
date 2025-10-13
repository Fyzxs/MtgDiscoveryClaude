import './App.css'
import { lazy, Suspense } from 'react'
import { BrowserRouter, Routes, Route, useNavigate, Navigate } from 'react-router-dom'
import { Container, Typography, Box, CircularProgress } from '@mui/material'
import { AppCard as Card } from './components/atoms/shared/AppCard'
import { AppButton as Button } from './components/atoms/shared/AppButton'
import { Layout } from './components/templates/Layout'
import { PageErrorBoundary } from './components/ErrorBoundaries'
import { CollectionProvider } from './contexts/CollectionContext'
import { UserProvider } from './contexts/UserContext'
import { I18nProvider } from './components/providers/I18nProvider'
import { globalSearchFocus } from './utils/globalSearchFocusHandler'

// Lazy load page components for code splitting
const AllSetsPage = lazy(() => import('./components/pages/AllSetsPage'))
const SetPage = lazy(() => import('./components/pages/SetPage'))
const CardSearchPage = lazy(() => import('./components/pages/CardSearchPage'))
const ArtistSearchPage = lazy(() => import('./components/pages/ArtistSearchPage'))
const ArtistCardsPage = lazy(() => import('./components/pages/ArtistCardsPage'))
const CardAllPrintingsPage = lazy(() => import('./components/pages/CardAllPrintingsPage'))
const SignInRedirectPage = lazy(() => import('./components/pages/SignInRedirectPage'))

// Initialize global search focus handler (double-tap Shift to focus search)
void globalSearchFocus;

// Loading fallback component for lazy-loaded pages
function PageLoadingFallback() {
  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: '60vh',
      }}
    >
      <CircularProgress />
    </Box>
  )
}

function HomePage() {
  const navigate = useNavigate();

  return (
    <Container
      maxWidth="lg"
      sx={{
        py: 12,
        display: 'flex',
        justifyContent: 'center'
      }}
    >
      <Card
        elevation={6}
        sx={{
          textAlign: 'center',
          maxWidth: 600,
          width: '100%',
          p: 4
        }}
      >
        <Typography variant="h4" component="h2" gutterBottom sx={{ fontWeight: 'bold' }}>
          Welcome to MTG Discovery
        </Typography>
        <Typography variant="body1" color="text.secondary" sx={{ mb: 3 }}>
          Explore our card component system built with atomic design principles. 
          View Magic: The Gathering cards with proper styling, rarity indicators, 
          and responsive layouts.
        </Typography>
        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center', flexWrap: 'wrap' }}>
          <Button 
            onClick={() => navigate('/sets')}
            size="large"
            variant="contained"
            color="primary"
            sx={{ 
              width: { xs: '100%', sm: 'auto' }
            }}
          >
            Browse All Sets
          </Button>
        </Box>
      </Card>
    </Container>
  );
}

function App() {
  return (
    <I18nProvider>
      <UserProvider>
        <BrowserRouter>
          <CollectionProvider>
            <Layout>
              <Suspense fallback={<PageLoadingFallback />}>
                <Routes>
                  <Route path="/" element={
                    <PageErrorBoundary name="HomePage">
                      <HomePage />
                    </PageErrorBoundary>
                  } />
                  <Route path="/sets" element={
                    <PageErrorBoundary name="AllSetsPage">
                      <AllSetsPage />
                    </PageErrorBoundary>
                  } />
                  <Route path="/set/:setCode" element={
                    <PageErrorBoundary name="SetPage">
                      <SetPage />
                    </PageErrorBoundary>
                  } />
                  <Route path="/search/cards" element={
                    <PageErrorBoundary name="CardSearchPage">
                      <CardSearchPage />
                    </PageErrorBoundary>
                  } />
                  <Route path="/search/artists" element={
                    <PageErrorBoundary name="ArtistSearchPage">
                      <ArtistSearchPage />
                    </PageErrorBoundary>
                  } />
                  <Route path="/artists/:artistName" element={
                    <PageErrorBoundary name="ArtistCardsPage">
                      <ArtistCardsPage />
                    </PageErrorBoundary>
                  } />
                  <Route path="/card/:cardName" element={
                    <PageErrorBoundary name="CardAllPrintingsPage">
                      <CardAllPrintingsPage />
                    </PageErrorBoundary>
                  } />
                  <Route path="/signin-redirect" element={
                    <PageErrorBoundary name="SignInRedirectPage">
                      <SignInRedirectPage />
                    </PageErrorBoundary>
                  } />
                  {/* Handle old query param URLs for backwards compatibility */}
                  <Route path="*" element={<LegacyRedirect />} />
                </Routes>
              </Suspense>
            </Layout>
          </CollectionProvider>
        </BrowserRouter>
      </UserProvider>
    </I18nProvider>
  )
}

// Component to handle old query param URLs
function LegacyRedirect() {
  const urlParams = new URLSearchParams(window.location.search);
  const page = urlParams.get('page');
  const setCode = urlParams.get('set');
  
  if (page === 'all-sets') {
    return <Navigate to="/sets" replace />;
  }
  if (page === 'set' && setCode) {
    return <Navigate to={`/set/${setCode}`} replace />;
  }
  
  // For any other unknown routes, redirect to home
  return <Navigate to="/" replace />;
}

export default App
