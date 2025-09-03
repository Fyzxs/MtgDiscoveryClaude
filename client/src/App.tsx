import './App.css'
import { BrowserRouter, Routes, Route, useNavigate, Navigate } from 'react-router-dom'
import { Container, Typography, Box } from '@mui/material'
import { AllSetsPage } from './pages/AllSetsPage'
import { SetPage } from './pages/SetPage'
import { CardSearchPage } from './pages/CardSearchPage'
import { ArtistSearchPage } from './pages/ArtistSearchPage'
import { ArtistCardsPage } from './pages/ArtistCardsPage'
import { CardAllPrintingsPage } from './pages/CardAllPrintingsPage'
import { AppCard as Card } from './components/atoms/shared/AppCard'
import { AppButton as Button } from './components/atoms/shared/AppButton'
import { Layout } from './components/templates/Layout'
import { PageErrorBoundary } from './components/ErrorBoundaries'
function HomePage() {
  const navigate = useNavigate();
  
  return (
    <Container maxWidth="lg" sx={{ py: 12 }}>        
      <Card 
        elevation={6} 
        sx={{ 
          textAlign: 'center', 
          maxWidth: 600, 
          mx: 'auto',
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
    <BrowserRouter>
      <Layout>
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
          {/* Handle old query param URLs for backwards compatibility */}
          <Route path="*" element={<LegacyRedirect />} />
        </Routes>
      </Layout>
    </BrowserRouter>
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
