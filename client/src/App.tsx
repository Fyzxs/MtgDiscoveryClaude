import './App.css'
import { useState } from 'react'
import { CardDemoPage } from './pages/CardDemoPage'
import { SetDemoPage } from './pages/SetDemoPage'
import { Card } from './components/ui/Card'
import { Button } from './components/ui/Button'

function App() {
  const [currentPage, setCurrentPage] = useState<'home' | 'card-demo' | 'set-demo'>('home')

  if (currentPage === 'card-demo') {
    return <CardDemoPage />
  }

  if (currentPage === 'set-demo') {
    return <SetDemoPage />
  }

  return (
    <div className="min-h-screen bg-gray-950">
      <div className="container mx-auto px-4 py-12 max-w-4xl">
        <div className="text-center mb-12">
          <h1 className="text-5xl font-bold text-white mb-4">
            MTG Discovery
          </h1>
          <p className="text-xl text-gray-400 mb-8">
            Magic: The Gathering Collection Management Platform
          </p>
        </div>
        
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
              onClick={() => setCurrentPage('card-demo')}
              size="lg"
              className="w-full sm:w-auto"
            >
              View Card Component Demo
            </Button>
            <Button 
              onClick={() => setCurrentPage('set-demo')}
              size="lg"
              variant="outline"
              className="w-full sm:w-auto"
            >
              View Set Component Demo
            </Button>
          </div>
        </Card>
      </div>
    </div>
  )
}

export default App
