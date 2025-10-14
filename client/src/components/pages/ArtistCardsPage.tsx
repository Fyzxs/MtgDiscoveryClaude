import React, { startTransition } from 'react';
import { useParams } from 'react-router-dom';
import { Alert } from '../atoms';
import { BrowseTemplate } from '../templates/pages/BrowseTemplate';
import { ArtistPageHeader } from '../organisms/ArtistPageHeader';
import { ArtistPageFilters } from '../organisms/ArtistPageFilters';
import { ArtistPageCardDisplay } from '../organisms/ArtistPageCardDisplay';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { QueryStateContainer } from '../molecules/shared/QueryStateContainer';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { SectionErrorBoundary } from '../ErrorBoundaries';
import { useArtistCardsData } from '../../hooks/useArtistCardsData';

/**
 * ArtistCardsPage - Display all cards by a specific artist with filtering
 * Refactored to use useArtistCardsData hook for reduced complexity
 */
export const ArtistCardsPage: React.FC = () => {
  const { artistName } = useParams<{ artistName: string }>();
  const decodedArtistName = decodeURIComponent(artistName || '').replace(/-/g, ' ');

  // All data and state management extracted to custom hook
  const {
    cards,
    cardsData,
    cardsLoading,
    cardsError,
    searchTerm,
    sortBy,
    filters,
    selectedRarities,
    selectedSets,
    allRarities,
    allSets,
    allFormats,
    setLabelMap,
    filteredCards,
    displayArtistName,
    alternateNames,
    setSearchTerm,
    setSortBy,
    updateFilter,
    hasCollector
  } = useArtistCardsData(artistName, decodedArtistName);

  // Handler functions
  const handleClearFilters = () => {
    startTransition(() => {
      setSearchTerm('');
      updateFilter('rarities', []);
      updateFilter('sets', []);
      if (hasCollector) {
        updateFilter('collectionCounts', []);
        updateFilter('signedCards', []);
      }
    });
  };

  const handleSearchChange = (value: string) => {
    startTransition(() => setSearchTerm(value));
  };

  // Error handling
  if (!artistName) {
    return (
      <Alert severity="error" sx={{ m: 4 }}>
        No artist name provided. Please provide an artist name in the URL.
      </Alert>
    );
  }

  if (cardsData?.cardsByArtistName?.__typename === 'FailureResponse') {
    return (
      <Alert severity="error" sx={{ m: 4 }}>
        {cardsData.cardsByArtistName.status?.message || 'Failed to load cards'}
      </Alert>
    );
  }

  // Render using template composition pattern
  return (
    <QueryStateContainer
      loading={cardsLoading}
      error={cardsError}
      containerProps={{ maxWidth: false }}
    >
      <BrowseTemplate
        header={
          <SectionErrorBoundary name="ArtistPageHeader">
            <ArtistPageHeader
              displayArtistName={displayArtistName}
              alternateNames={alternateNames}
            />
          </SectionErrorBoundary>
        }
        filters={
          <ArtistPageFilters
            totalCards={cards.length}
            displayArtistName={displayArtistName}
            hasCollector={hasCollector}
            filters={{
              search: {
                value: searchTerm,
                onChange: handleSearchChange
              },
              rarities: {
                value: selectedRarities,
                onChange: (value: string[]) => updateFilter('rarities', value),
                options: allRarities,
                shouldShow: allRarities.length > 1
              },
              sets: {
                value: selectedSets,
                onChange: (value: string[]) => updateFilter('sets', value),
                options: allSets,
                shouldShow: allSets.length > 1,
                getOptionLabel: (setCode: string) => setLabelMap.get(setCode) || setCode.toUpperCase()
              },
              sort: {
                value: sortBy,
                onChange: setSortBy
              },
              collectionCounts: hasCollector ? {
                value: (Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[],
                onChange: (value: string[]) => updateFilter('collectionCounts', value)
              } : undefined,
              signedCards: hasCollector ? {
                value: (Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[],
                onChange: (value: string[]) => updateFilter('signedCards', value)
              } : undefined,
              formats: {
                value: (Array.isArray(filters.formats) ? filters.formats : []) as string[],
                onChange: (value: string[]) => updateFilter('formats', value),
                shouldShow: allFormats.length > 1
              }
            }}
          />
        }
        summary={
          <ResultsSummary
            current={filteredCards.length}
            total={cards.length}
            label="cards"
            textAlign="center"
          />
        }
        content={
          <ArtistPageCardDisplay
            filteredCards={filteredCards}
            displayArtistName={displayArtistName}
            hasCollector={hasCollector}
            isLoading={cardsLoading}
            onClearFilters={handleClearFilters}
          />
        }
      />
      <BackToTopFab />
    </QueryStateContainer>
  );
};

export default ArtistCardsPage;
