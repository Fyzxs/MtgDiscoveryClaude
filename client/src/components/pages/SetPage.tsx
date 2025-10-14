import React from 'react';
import { useParams } from 'react-router-dom';
import { Container, Alert } from '../atoms';
import { SetPageTemplate } from '../templates/SetPageTemplate';
import { SetPageHeader } from '../organisms/SetPageHeader';
import { SetPageFilters } from '../organisms/SetPageFilters';
import { SetPageCardDisplay } from '../organisms/SetPageCardDisplay';
import { useSetPageData } from '../../hooks/useSetPageData';

/**
 * SetPage - Display a specific Magic: The Gathering set with filtering and grouping
 * Refactored to use useSetPageData hook for reduced complexity
 */
export const SetPage: React.FC = () => {
  const { setCode } = useParams<{ setCode: string }>();

  // All data and state management extracted to custom hook
  const {
    cards,
    cardsData,
    setInfo,
    setName,
    isLoading,
    firstError,
    cardsLoading,
    searchTerm,
    sortBy,
    filters,
    selectedRarities,
    selectedArtists,
    selectedGroupIds,
    allArtists,
    allRarities,
    allFinishes,
    filteredCards,
    sortedCards,
    cardGroups,
    allCardGroups,
    visibleGroupIds,
    allSameReleaseDate,
    currentCount,
    handleSearchChange,
    handleSortChange,
    handleRarityChange,
    handleClearFilters,
    updateFilter,
    hasCollector
  } = useSetPageData(setCode);

  // Error handling
  if (!setCode) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          No set code provided. Please provide a set code in the URL (e.g., ?set=lea)
        </Alert>
      </Container>
    );
  }

  if (cardsData?.cardsBySetCode?.__typename === 'FailureResponse') {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          {cardsData.cardsBySetCode.status?.message || 'Failed to load cards'}
        </Alert>
      </Container>
    );
  }

  // Render using template composition pattern
  return (
    <SetPageTemplate
      isLoading={isLoading}
      error={firstError}
      currentCount={currentCount}
      totalCount={cards.length}
      header={
        <SetPageHeader
          setInfo={setInfo}
          setName={setName}
          setCode={setCode}
          availableGroupIds={allCardGroups.map(g => g.id)}
        />
      }
      filters={
        <SetPageFilters
          searchTerm={searchTerm}
          onSearchChange={handleSearchChange}
          sortBy={sortBy}
          onSortChange={handleSortChange}
          selectedRarities={selectedRarities}
          selectedArtists={selectedArtists}
          selectedGroupIds={selectedGroupIds}
          showGroups={filters.showGroups !== false}
          onRarityChange={handleRarityChange}
          onArtistChange={(value: string[]) => updateFilter('artists', value)}
          onGroupChange={(groupIds: string[]) => updateFilter('groups', groupIds)}
          onShowGroupsChange={(value: boolean) => updateFilter('showGroups', value)}
          allRarities={allRarities}
          allArtists={allArtists}
          allFinishes={allFinishes}
          cardGroups={cardGroups}
          cards={cards}
          hasCollector={hasCollector}
          collectionCounts={(Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[]}
          signedCards={(Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[]}
          finishes={(Array.isArray(filters.finishes) ? filters.finishes : []) as string[]}
          onCollectionCountsChange={(value: string[]) => updateFilter('collectionCounts', value)}
          onSignedCardsChange={(value: string[]) => updateFilter('signedCards', value)}
          onFinishesChange={(value: string[]) => updateFilter('finishes', value)}
        />
      }
      cardDisplay={
        <SetPageCardDisplay
          cardsLoading={cardsLoading}
          sortedCards={sortedCards}
          filteredCards={filteredCards}
          cardGroups={cardGroups}
          setInfo={setInfo}
          showGroups={filters.showGroups !== false}
          visibleGroupIds={visibleGroupIds}
          allSameReleaseDate={allSameReleaseDate}
          setCode={setCode}
          hasCollector={hasCollector}
          onClearFilters={handleClearFilters}
        />
      }
    />
  );
};

export default SetPage;
