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
    collectionCounts?: string[];
    signedCards?: string[];
  };
  initialSearch?: string;
  initialSort?: string;
  includeSets?: boolean; // Whether to include set filtering
  includeCollectorFilters?: boolean; // Whether to include collector-specific filters
}

// Define defaults outside to prevent recreating on every render
const DEFAULT_SEARCH_FIELDS: (keyof CardLike)[] = ['name'];
const DEFAULT_INITIAL_FILTERS = {};
const EMPTY_ARRAY: string[] = [];

export function useCardFiltering<T extends CardLike>(
  cards: T[] | undefined,
  options: CardFilterOptions = {}
) {
  const {
    searchFields = DEFAULT_SEARCH_FIELDS,
    defaultSort = 'collector-asc',
    initialFilters = DEFAULT_INITIAL_FILTERS,
    initialSearch = '',
    initialSort,
    includeSets = false,
    includeCollectorFilters = false
  } = options;

  const data = cards || [];

  // Get unique values for filters
  const uniqueArtists = useMemo(() => getUniqueArtists(data), [data]);
  const uniqueRarities = useMemo(() => getUniqueRarities(data), [data]);
  const uniqueSets = useMemo(() => includeSets ? getUniqueSets(data) : EMPTY_ARRAY, [data, includeSets]);

  // Create filter configuration
  const filterConfig = useMemo(() => ({
    searchFields: searchFields as (keyof T)[],
    sortOptions: createCardSortOptions<T>(),
    filterFunctions: createCardFilterFunctions<T>(),
    defaultSort
  }), [searchFields, defaultSort]);

  // Memoize initial state to prevent infinite re-renders
  const initialState = useMemo(() => ({
    search: initialSearch,
    sort: initialSort || defaultSort,
    filters: {
      rarities: initialFilters.rarities || EMPTY_ARRAY,
      artists: initialFilters.artists || EMPTY_ARRAY,
      sets: initialFilters.sets || EMPTY_ARRAY,
      showDigital: initialFilters.showDigital || false,
      ...(includeCollectorFilters ? {
        collectionCounts: initialFilters.collectionCounts || EMPTY_ARRAY,
        signedCards: initialFilters.signedCards || EMPTY_ARRAY
      } : {})
    }
  }), [
    initialSearch,
    initialSort,
    defaultSort,
    initialFilters.rarities,
    initialFilters.artists,
    initialFilters.sets,
    initialFilters.showDigital,
    initialFilters.collectionCounts,
    initialFilters.signedCards,
    includeCollectorFilters
  ]);

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
    initialState
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