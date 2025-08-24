import React, { useState, useEffect } from 'react';
import { useLazyQuery } from '@apollo/client/react';
import { GET_CARDS_BY_IDS } from '../graphql/queries/cards';
import type { CardContext } from '../types/card';
import { MtgCard } from '../components/organisms/MtgCard';
import { MuiCard } from '../components/atoms/MuiCard';
import { MuiButton } from '../components/atoms/MuiButton';
import { MuiInput } from '../components/atoms/MuiInput';
import { Box, Typography, FormControlLabel, Checkbox } from '@mui/material';

export const CardDemoPage: React.FC = () => {
  const [inputValue, setInputValue] = useState('');
  const [selectedCardId, setSelectedCardId] = useState<string | null>(null);
  const [context, setContext] = useState<CardContext>({
    isOnSetPage: false,
    isOnArtistPage: false,
    isOnCardPage: false,
    showCollectorInfo: true
  });

  const [getCards, { loading, error, data }] = useLazyQuery(GET_CARDS_BY_IDS);

  // Get loaded card data
  const loadedCard = (data as any)?.cardsById?.data?.[0];
  
  // Debug: Log the loaded card data
  useEffect(() => {
    if (loadedCard) {
      console.log('Loaded card data:', loadedCard);
      console.log('Card name:', loadedCard.name);
      console.log('Card artist:', loadedCard.artist);
      console.log('Card set:', loadedCard.setName);
      console.log('Card rarity:', loadedCard.rarity);
      console.log('Card price:', loadedCard.prices);
    }
  }, [loadedCard]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    getCards({ variables: { ids: { cardIds: [inputValue] } } });
  };

  // Navigation handlers
  const handleCardClick = (clickedCardId?: string) => {
    console.log('Navigate to card:', clickedCardId);
  };

  const handleSetClick = (setCode?: string) => {
    console.log('Navigate to set:', setCode);
  };

  const handleArtistClick = (artistName: string, artistId?: string) => {
    console.log('Navigate to artist:', artistName, artistId);
  };

  const handleSelectionChange = (cardId: string, selected: boolean) => {
    if (selected) {
      setSelectedCardId(cardId);
    } else {
      setSelectedCardId(null);
    }
  };

  // Add click outside handler to deselect
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      const target = event.target as HTMLElement;
      // Check if click is on the background or parent containers, not on a card itself
      // Look for the specific card that has our click handler
      const clickedCard = target.closest('[data-mtg-card="true"]');
      if (!clickedCard) {
        setSelectedCardId(null);
      }
    };

    document.addEventListener('click', handleClickOutside);
    return () => {
      document.removeEventListener('click', handleClickOutside);
    };
  }, []);

  return (
    <div className="min-h-screen bg-gray-950">
      <div className="container mx-auto px-4 py-8">
        <div className="mb-8">
          <MuiButton 
            onClick={() => window.location.reload()}
            variant="outlined" 
            size="small"
            sx={{ mb: 2 }}
          >
            ← Back to Home
          </MuiButton>
          <h1 className="text-3xl font-bold text-white mb-2">Card Component Demo</h1>
          <p className="text-gray-400">
            Demonstration of MTG card display components
          </p>
        </div>

        {/* Card ID Input */}
        <MuiCard className="mb-8">
          <Box component="form" onSubmit={handleSubmit} sx={{ display: 'flex', gap: 2 }}>
            <MuiInput
              label="Card ID"
              value={inputValue}
              onChange={(e) => setInputValue(e.target.value)}
              placeholder="Enter card ID (e.g., d5c83259-9b90-47c2-b48e-c7d78519e792)"
              variant="outlined"
              sx={{ flexGrow: 1 }}
            />
            <MuiButton
              type="submit"
              variant="contained"
              color="primary"
              isLoading={loading}
              sx={{ minWidth: 120 }}
            >
              Load Card
            </MuiButton>
          </Box>
          {error && (
            <div className="mt-2 text-red-500 text-sm">
              <p>Error loading card: {error.message}</p>
              {error.message.includes('fetch') && (
                <p className="mt-1 text-yellow-500">
                  Make sure the GraphQL backend is running: 
                  <code className="ml-2 px-2 py-1 bg-gray-200 dark:bg-gray-800 text-gray-900 dark:text-gray-100 rounded">
                    dotnet run --project ../src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj
                  </code>
                </p>
              )}
            </div>
          )}
        </MuiCard>

        {/* Context Controls */}
        <MuiCard className="mb-8">
          <Typography variant="h6" gutterBottom>Context Settings</Typography>
          <Typography variant="body2" color="textSecondary" paragraph>
            Control how the card components render in different page contexts
          </Typography>
          <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
            <FormControlLabel
              control={
                <Checkbox
                  checked={context.isOnSetPage}
                  onChange={(e) => setContext({
                    ...context, 
                    isOnSetPage: e.target.checked,
                    currentSetCode: e.target.checked && loadedCard?.setCode ? loadedCard.setCode : undefined
                  })}
                />
              }
              label="On Set Page (hides set name)"
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={context.isOnArtistPage}
                  onChange={(e) => setContext({
                    ...context, 
                    isOnArtistPage: e.target.checked,
                    currentArtist: e.target.checked && loadedCard?.artist ? loadedCard.artist.split(/\s+(?:&|and)\s+/i)[0] : undefined
                  })}
                />
              }
              label="On Artist Page (hides single artist)"
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={context.isOnCardPage}
                  onChange={(e) => setContext({...context, isOnCardPage: e.target.checked})}
                />
              }
              label="On Card Page (hides card name)"
            />
          </Box>
        </MuiCard>

        {/* Card Display */}
        {loadedCard ? (
          <>
            {/* Single Card Display */}
            <MuiCard elevation={3} className="mb-8">
              <Typography variant="h5" gutterBottom>Single Card Display</Typography>
              <Box sx={{ display: 'flex', justifyContent: 'center', p: 2 }}>
                <MtgCard
                  card={loadedCard}
                  context={context}
                  isSelected={selectedCardId === loadedCard.id}
                  onSelectionChange={handleSelectionChange}
                  onCardClick={handleCardClick}
                  onSetClick={handleSetClick}
                  onArtistClick={handleArtistClick}
                />
              </Box>
            </MuiCard>

            {/* 3x3 Grid Display */}
            <MuiCard elevation={3} className="mb-8">
              <Typography variant="h5" gutterBottom>3×3 Grid Display</Typography>
              <Typography variant="body2" color="textSecondary" paragraph>
                Nine instances of the same card to demonstrate multi-card layouts
              </Typography>
              <Box sx={{ 
                display: 'grid', 
                gridTemplateColumns: 'repeat(3, 1fr)', 
                gap: 2, 
                justifyItems: 'center',
                maxWidth: '900px',
                mx: 'auto',
                p: 1
              }}>
                {Array.from({ length: 9 }, (_, index) => {
                  const uniqueCardId = `${loadedCard.id}-grid-${index}`;
                  return (
                    <Box key={index} sx={{ width: { xs: '160px', sm: '180px', md: '200px' } }}>
                      <MtgCard
                        card={{...loadedCard, id: uniqueCardId}}
                        context={context}
                        isSelected={selectedCardId === uniqueCardId}
                        onSelectionChange={handleSelectionChange}
                        onCardClick={handleCardClick}
                        onSetClick={handleSetClick}
                        onArtistClick={handleArtistClick}
                        className="scale-75 origin-center"
                      />
                    </Box>
                  );
                })}
              </Box>
            </MuiCard>
          </>
        ) : (
          <MuiCard elevation={2} className="mb-8">
            <Box sx={{ textAlign: 'center', py: 8 }}>
              <Box sx={{ fontSize: 64, opacity: 0.3, mb: 2 }}>🃏</Box>
              <Typography variant="h6" gutterBottom>No Card Loaded</Typography>
              <Typography variant="body2" color="textSecondary">
                Enter a card ID above and click "Load Card" to see the MTG card display components.
              </Typography>
            </Box>
          </MuiCard>
        )}
      </div>
    </div>
  );
};