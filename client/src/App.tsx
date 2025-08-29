import './App.css'
import { BrowserRouter, Routes, Route, useNavigate, Navigate } from 'react-router-dom'
import { AllSetsPage } from './pages/AllSetsPage'
import { SetPage } from './pages/SetPage'
import { CardSearchPage } from './pages/CardSearchPage'
import { Card } from './components/ui/Card'
import { Button } from './components/ui/Button'
import { Layout } from './components/templates/Layout'
import { PageErrorBoundary } from './components/ErrorBoundaries'

function HomePage() {
  const navigate = useNavigate();
  
  return (
    <div className="container mx-auto px-4 py-12 max-w-4xl">        
      <Card variant="elevated" className="text-center max-w-2xl mx-auto">
        <h2 className="text-2xl font-semibold text-white mb-4">
          Welcome to MTG Discovery
        </h2>
        <p className="text-gray-400 mb-6">
          Explore our card component system built with atomic design principles. 
          View Magic: The Gathering cards with proper styling, rarity indicators, 
          and responsive layouts.
        </p>
        <div className="flex gap-4 justify-center flex-wrap">
          <Button 
            onClick={() => navigate('/sets')}
            size="lg"
            className="w-full sm:w-auto"
          >
            Browse All Sets
          </Button>
        </div>
      </Card>
    </div>
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
          <Route path="/card/:cardName" element={
            <PageErrorBoundary name="CardDetailPage">
              <div>Card Detail Page - Coming Soon</div>
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
