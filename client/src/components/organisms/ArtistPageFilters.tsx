import React from 'react';
import { Box } from '../../atoms';
import { FilterPanel } from './filters/FilterPanel';
import { CollectorFiltersSection } from '../molecules/shared/CollectorFiltersSection';
import { FilterErrorBoundary } from '../ErrorBoundaries';
import { ARTIST_PAGE_SORT_OPTIONS, ARTIST_PAGE_COLLECTOR_SORT_OPTIONS } from '../../config/cardSortOptions';
import {
  getCollectionCountOptions,
  getSignedCardsOptions,
  getFormatOptions
} from '../../utils/cardUtils';
import { MultiSelectDropdown } from '../molecules/shared/MultiSelectDropdown';

interface ArtistPageFiltersProps {
  /** Total number of cards to determine if filters should show */
  totalCards: number;

  /** Display name of the artist for search placeholder */
  displayArtistName: string;

  /** Whether collector mode is active */
  hasCollector: boolean;

  /** Filter configuration and values */
  filters: {
    /** Search term configuration */
    search: {
      value: string;
      onChange: (value: string) => void;
    };

    /** Rarity filter configuration */
    rarities: {
      value: string[];
      onChange: (value: string[]) => void;
      options: string[];
      shouldShow: boolean;
    };

    /** Set filter configuration */
    sets: {
      value: string[];
      onChange: (value: string[]) => void;
      options: string[];
      shouldShow: boolean;
      getOptionLabel: (setCode: string) => string;
    };

    /** Sort configuration */
    sort: {
      value: string;
      onChange: (value: string) => void;
    };

    /** Collection count filter (collector mode only) */
    collectionCounts?: {
      value: string[];
      onChange: (value: string[]) => void;
    };

    /** Signed cards filter (collector mode only) */
    signedCards?: {
      value: string[];
      onChange: (value: string[]) => void;
    };

    /** Format filter (Digital/Paper) */
    formats?: {
      value: string[];
      onChange: (value: string[]) => void;
      shouldShow: boolean;
    };
  };
}

/**
 * ArtistPageFilters - Filter panel for artist page
 *
 * Renders search, rarity, set, and sort filters for artist cards.
 * Includes collector-specific filters when in collector mode.
 * Hides completely if only 1 card exists.
 */
export const ArtistPageFilters: React.FC<ArtistPageFiltersProps> = ({
  totalCards,
  displayArtistName,
  hasCollector,
  filters
}) => {
  // Hide filters completely if only 1 card
  if (totalCards <= 1) {
    return null;
  }

  return (
    <FilterErrorBoundary name="ArtistPageFilters">
      <Box sx={{ display: 'flex', flexDirection: 'column', width: '100%' }}>
        <FilterPanel
          config={{
            search: {
              value: filters.search.value,
              onChange: filters.search.onChange,
              placeholder: `Search ${displayArtistName}'s cards...`,
              debounceMs: 300,
              minWidth: 300
            },
            autocompletes: [
              ...(filters.rarities.shouldShow ? [{
                key: 'rarities',
                value: filters.rarities.value,
                onChange: filters.rarities.onChange,
                options: filters.rarities.options,
                label: 'Rarity',
                placeholder: 'All Rarities',
                minWidth: 180
              }] : []),
              ...(filters.sets.shouldShow ? [{
                key: 'sets',
                value: filters.sets.value,
                onChange: filters.sets.onChange,
                options: filters.sets.options,
                label: 'Set',
                placeholder: 'All Sets',
                minWidth: 200,
                getOptionLabel: filters.sets.getOptionLabel
              }] : [])
            ],
            sort: {
              value: filters.sort.value,
              onChange: filters.sort.onChange,
              options: hasCollector ? ARTIST_PAGE_COLLECTOR_SORT_OPTIONS : ARTIST_PAGE_SORT_OPTIONS,
              minWidth: 180
            },
            customFilters: (filters.formats && filters.formats.shouldShow) ? [
              <MultiSelectDropdown
                key="formats"
                value={filters.formats.value}
                onChange={filters.formats.onChange}
                options={getFormatOptions()}
                label="Format"
                placeholder="All Formats"
                minWidth={150}
                fullWidth={false}
              />
            ] : []
          }}
          layout="compact"
          sx={{ mb: hasCollector ? 2 : 4, display: 'flex', justifyContent: 'center' }}
        />

        {/* Collector-specific filters */}
        {hasCollector && filters.collectionCounts && filters.signedCards && (
          <CollectorFiltersSection
            config={{
              collectionCounts: {
                key: 'collectionCounts',
                value: filters.collectionCounts.value,
                onChange: filters.collectionCounts.onChange,
                options: getCollectionCountOptions(),
                label: 'Collection Count',
                placeholder: 'All Counts',
                minWidth: 180
              },
              signedCards: {
                key: 'signedCards',
                value: filters.signedCards.value,
                onChange: filters.signedCards.onChange,
                options: getSignedCardsOptions(),
                label: 'Signed Cards',
                placeholder: 'All Cards',
                minWidth: 150
              }
            }}
            title="Collection Filters"
            sx={{ mb: 3, mx: 'auto', maxWidth: '800px' }}
          />
        )}
      </Box>
    </FilterErrorBoundary>
  );
};