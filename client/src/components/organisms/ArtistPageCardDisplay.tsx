import React from 'react';
import { CardGrid } from './CardGrid';
import { SearchEmptyState } from '../molecules/shared/EmptyState';
import { CardGridErrorBoundary } from '../ErrorBoundaries';
import type { Card } from '../../types/card';

interface ArtistPageCardDisplayProps {
  /** Filtered cards to display */
  filteredCards: Card[];

  /** Display name of the current artist */
  displayArtistName: string;

  /** Whether collector mode is active */
  hasCollector: boolean;

  /** Whether cards are currently loading */
  isLoading: boolean;

  /** Handler for clearing all filters */
  onClearFilters: () => void;
}

/**
 * ArtistPageCardDisplay - Card grid display for artist page
 *
 * Renders the card grid with artist-specific context and handles
 * empty states when no cards match the current filters.
 */
export const ArtistPageCardDisplay: React.FC<ArtistPageCardDisplayProps> = ({
  filteredCards,
  displayArtistName,
  hasCollector,
  isLoading,
  onClearFilters
}) => {
  return (
    <>
      {/* Card Grid */}
      <CardGridErrorBoundary name="ArtistPageCardGrid">
        <CardGrid
          cards={filteredCards}
          groupId="artist-cards"
          context={{
            isOnArtistPage: true,
            currentArtist: displayArtistName,
            hideSetInfo: false,
            showCollectorInfo: true,
            hasCollector
          }}
          isLoading={isLoading}
          sx={{ mt: 3 }}
        />
      </CardGridErrorBoundary>

      {/* Empty State */}
      {!isLoading && filteredCards.length === 0 && (
        <SearchEmptyState
          itemType="cards"
          onClear={onClearFilters}
        />
      )}
    </>
  );
};