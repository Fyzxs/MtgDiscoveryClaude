import './App.css'
import { useState } from 'react'
import { AllSetsPage } from './pages/AllSetsPage'
import { SetPage } from './pages/SetPage'
import { Card } from './components/ui/Card'
import { Button } from './components/ui/Button'
import { Layout } from './components/templates/Layout'
import { PageErrorBoundary } from './components/ErrorBoundaries'

function App() {
  // Check URL params for initial page
  const urlParams = new URLSearchParams(window.location.search);
  const pageParam = urlParams.get('page') as 'home' | 'all-sets' | 'set' | null;
  
  const [currentPage, setCurrentPage] = useState<'home' | 'all-sets' | 'set'>(
    pageParam || 'home'  // Default back to home
  )

  if (currentPage === 'all-sets') {
    return (
      <PageErrorBoundary name="AllSetsPage">
        <Layout><AllSetsPage /></Layout>
      </PageErrorBoundary>
    )
  }

  if (currentPage === 'set') {
    return (
      <PageErrorBoundary name="SetPage">
        <Layout><SetPage /></Layout>
      </PageErrorBoundary>
    )
  }

  return (
    <PageErrorBoundary name="HomePage">
      <Layout>
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
              onClick={() => setCurrentPage('all-sets')}
              size="lg"
              className="w-full sm:w-auto"
            >
              Browse All Sets
            </Button>
          </div>
        </Card>
      </div>
      </Layout>
    </PageErrorBoundary>
  )
}

export default App
