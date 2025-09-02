import { useMemo } from 'react';
import { useFilterState } from './useFilterState';
import type { CardLike } from '../config/cardSortOptions';
import { createCardSortOptions } from '../config/cardSortOptions';
import { 
  getUniqueArtists, 
  getUniqueRarities, 
  getUniqueSets,
  createCardFilterFunctions 
} from '../utils/cardUtils';

interface CardFilterOptions {
  searchFields?: (keyof CardLike)[];
  defaultSort?: string;
  initialFilters?: {
    rarities?: string[];
    artists?: string[];
    sets?: string[];
    showDigital?: boolean;
  };
  initialSearch?: string;
  initialSort?: string;
  includeSets?: boolean; // Whether to include set filtering
}

export function useCardFiltering<T extends CardLike>(
  cards: T[] | undefined,
  options: CardFilterOptions = {}
) {
  const {
    searchFields = ['name'],
    defaultSort = 'collector-asc',
    initialFilters = {},
    initialSearch = '',
    initialSort,
    includeSets = false
  } = options;

  const data = cards || [];

  // Get unique values for filters
  const uniqueArtists = useMemo(() => getUniqueArtists(data), [data]);
  const uniqueRarities = useMemo(() => getUniqueRarities(data), [data]);
  const uniqueSets = useMemo(() => includeSets ? getUniqueSets(data) : [], [data, includeSets]);

  // Create filter configuration
  const filterConfig = useMemo(() => ({
    searchFields: searchFields as (keyof T)[],
    sortOptions: createCardSortOptions<T>(),
    filterFunctions: createCardFilterFunctions<T>(),
    defaultSort
  }), [searchFields, defaultSort]);

  // Use the filter state hook
  const {
    searchTerm,
    sortBy,
    filters,
    filteredData,
    setSearchTerm,
    setSortBy,
    updateFilter
  } = useFilterState(
    data,
    filterConfig,
    {
      search: initialSearch,
      sort: initialSort || defaultSort,
      filters: {
        rarities: initialFilters.rarities || [],
        artists: initialFilters.artists || [],
        sets: initialFilters.sets || [],
        showDigital: initialFilters.showDigital || false
      }
    }
  );

  return {
    // Filtered data
    filteredCards: filteredData,
    totalCards: data.length,
    
    // Current state
    searchTerm,
    sortBy,
    filters,
    
    // State setters
    setSearchTerm,
    setSortBy,
    updateFilter,
    
    // Available filter options
    uniqueArtists,
    uniqueRarities,
    uniqueSets,
    
    // Helpers
    hasMultipleArtists: uniqueArtists.length > 1,
    hasMultipleRarities: uniqueRarities.length > 1,
    hasMultipleSets: uniqueSets.length > 1
  };
}