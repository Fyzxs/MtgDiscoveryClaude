import { useState, useMemo, useEffect, useTransition, useCallback } from 'react';
import { useUrlState } from './useUrlState';
import { useFilterState } from './useFilterState';
import { useCollectorParam } from './useCollectorParam';
import { useCardQueries } from './useCardQueries';
import { useOptimizedSort } from './useOptimizedSort';
import { useMinimumLoadingTime } from './useMinimumLoadingTime';
import { RARITY_ORDER } from '../config/cardSortOptions';
import {
  getUniqueRarities,
  getUniqueSets,
  getUniqueFormats,
  createCardFilterFunctions
} from '../utils/cardUtils';
import { toPascalCase, getArtistNameInfo } from '../utils/artistUtils';
import type { Card } from '../types/card';

const EMPTY_CARDS_ARRAY: Card[] = [];

/**
 * Custom hook to manage all ArtistCardsPage data, filtering, and state
 * Extracted from ArtistCardsPage to reduce component complexity
 */
export const useArtistCardsData = (artistName: string | undefined, decodedArtistName: string) => {
  const { hasCollector, collectorId } = useCollectorParam();

  // URL state configuration
  const urlStateConfig = {
    search: { default: '' },
    rarities: { default: [] },
    sets: { default: [] },
    formats: { default: [] },
    sort: { default: 'release-desc' },
    counts: { default: [] },
    signed: { default: [] }
  };

  const { getInitialValues } = useUrlState({}, urlStateConfig);
  const [initialValues] = useState(() => getInitialValues());

  // Loading state with minimum display time
  const { isLoading: isFiltering, showLoading, hideLoading } = useMinimumLoadingTime(400);
  const [isPending, startTransition] = useTransition();

  // Card data fetching - store cards directly, not wrapped in GraphQL response structure
  const { fetchCardsByArtist } = useCardQueries();
  const [cardsLoading, setCardsLoading] = useState(true);
  const [cardsError, setCardsError] = useState<Error | null>(null);
  const [cards, setCards] = useState<Card[]>(EMPTY_CARDS_ARRAY);

  // Load cards effect
  useEffect(() => {
    if (!artistName) return;

    const loadCards = async () => {
      setCardsLoading(true);
      setCardsError(null);
      try {
        const fetchedCards = await fetchCardsByArtist(decodedArtistName);
        setCards(fetchedCards);
      } catch (err) {
        setCardsError(err as Error);
        setCards(EMPTY_CARDS_ARRAY);
      } finally {
        setCardsLoading(false);
      }
    };

    loadCards();
  }, [artistName, decodedArtistName, fetchCardsByArtist]);

  // Get unique values - cards is now directly the Card[] array
  const allRarities = useMemo(() => getUniqueRarities(cards), [cards]);
  const allSetObjects = useMemo(() => getUniqueSets(cards), [cards]);
  const allSets = useMemo(() => allSetObjects.map(set => set.value), [allSetObjects]);
  const allFormats = useMemo(() => getUniqueFormats(cards), [cards]);

  // Set label map
  const setLabelMap = useMemo(() => {
    const map = new Map<string, string>();
    allSetObjects.forEach(set => map.set(set.value, set.label));
    return map;
  }, [allSetObjects]);

  // Filter configuration
  const filterConfig = useMemo(() => {
    const cardFilterFunctions = createCardFilterFunctions<Card>();

    return {
      searchFields: ['name'] as (keyof Card)[],
      sortOptions: {
        'release-desc': (a: Card, b: Card) => {
          if (!a.releasedAt && !b.releasedAt) return 0;
          if (!a.releasedAt) return 1;
          if (!b.releasedAt) return -1;
          return new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime();
        },
        'release-asc': (a: Card, b: Card) => {
          if (!a.releasedAt && !b.releasedAt) return 0;
          if (!a.releasedAt) return 1;
          if (!b.releasedAt) return -1;
          return new Date(a.releasedAt).getTime() - new Date(b.releasedAt).getTime();
        },
        'name-asc': (a: Card, b: Card) => a.name.localeCompare(b.name),
        'name-desc': (a: Card, b: Card) => b.name.localeCompare(a.name),
        'rarity': (a: Card, b: Card) => (RARITY_ORDER[a.rarity?.toLowerCase() || ''] ?? 99) - (RARITY_ORDER[b.rarity?.toLowerCase() || ''] ?? 99),
        'price-desc': (a: Card, b: Card) => parseFloat(b.prices?.usd || '0') - parseFloat(a.prices?.usd || '0'),
        'price-asc': (a: Card, b: Card) => parseFloat(a.prices?.usd || '0') - parseFloat(b.prices?.usd || '0'),
        'set-asc': (a: Card, b: Card) => (a.setName || '').localeCompare(b.setName || ''),
        'collection-asc': (a: Card, b: Card) => {
          const getTotal = (card: Card) => {
            if (!card.userCollection) return 0;
            const collectionArray = Array.isArray(card.userCollection) ? card.userCollection : [card.userCollection];
            return collectionArray.reduce((sum, item) => sum + item.count, 0);
          };
          return getTotal(a) - getTotal(b);
        },
        'collection-desc': (a: Card, b: Card) => {
          const getTotal = (card: Card) => {
            if (!card.userCollection) return 0;
            const collectionArray = Array.isArray(card.userCollection) ? card.userCollection : [card.userCollection];
            return collectionArray.reduce((sum, item) => sum + item.count, 0);
          };
          return getTotal(b) - getTotal(a);
        }
      },
      filterFunctions: cardFilterFunctions,
      defaultSort: 'release-desc'
    };
  }, []);

  // Filter state
  const {
    searchTerm,
    sortBy,
    filters,
    filteredData: unsortedFilteredCards,
    setSearchTerm,
    setSortBy,
    updateFilter
  } = useFilterState(
    cards,
    filterConfig,
    {
      search: (typeof initialValues.search === 'string' ? initialValues.search : '') || '',
      sort: (typeof initialValues.sort === 'string' ? initialValues.sort : 'release-desc') || 'release-desc',
      filters: {
        rarities: Array.isArray(initialValues.rarities) ? initialValues.rarities : (initialValues.rarities ? [initialValues.rarities] : []),
        sets: Array.isArray(initialValues.sets) ? initialValues.sets : (initialValues.sets ? [initialValues.sets] : []),
        formats: Array.isArray(initialValues.formats) ? initialValues.formats : (initialValues.formats ? [initialValues.formats] : []),
        collectionCounts: Array.isArray(initialValues.counts) ? initialValues.counts : (initialValues.counts ? [initialValues.counts] : []),
        signedCards: Array.isArray(initialValues.signed) ? initialValues.signed : (initialValues.signed ? [initialValues.signed] : [])
      }
    }
  );

  const selectedRarities = (Array.isArray(filters.rarities) ? filters.rarities : []) as string[];
  const selectedSets = (Array.isArray(filters.sets) ? filters.sets : []) as string[];
  const selectedFormats = (Array.isArray(filters.formats) ? filters.formats : []) as string[];

  // Wrapped versions of setSortBy and updateFilter that use transitions
  const setSortByWithTransition = useCallback((newSortBy: string) => {
    showLoading();
    startTransition(() => {
      setSortBy(newSortBy);
      hideLoading();
    });
  }, [setSortBy, showLoading, hideLoading]);

  const updateFilterWithTransition = useCallback((filterName: string, value: unknown) => {
    showLoading();
    startTransition(() => {
      updateFilter(filterName, value);
      hideLoading();
    });
  }, [updateFilter, showLoading, hideLoading]);

  // Apply sorting to filtered cards using useOptimizedSort
  const sortFunction = useMemo(() => {
    return filterConfig.sortOptions[sortBy] || filterConfig.sortOptions['release-desc'];
  }, [filterConfig.sortOptions, sortBy]);

  // Include collector context in sort key to disable caching when in collector mode
  const sortKey = hasCollector ? `${sortBy}-ctor-${collectorId}` : sortBy;
  const filteredCards = useOptimizedSort(unsortedFilteredCards, sortKey, sortFunction);

  // Get artist name info
  const pascalCasedName = toPascalCase(decodedArtistName);
  const artistNameInfo = useMemo(() => {
    if (cards.length === 0) {
      return { primaryName: pascalCasedName, alternateNames: [] };
    }
    return getArtistNameInfo(cards, decodedArtistName);
  }, [cards, decodedArtistName, pascalCasedName]);

  // URL state sync
  useUrlState(
    {
      rarities: selectedRarities,
      sets: selectedSets,
      formats: selectedFormats,
      sort: sortBy,
      counts: (Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[],
      signed: (Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[]
    },
    {
      rarities: { default: [] },
      sets: { default: [] },
      formats: { default: [] },
      sort: { default: 'release-desc' },
      counts: { default: [] },
      signed: { default: [] }
    },
    { debounceMs: 0 }
  );

  useUrlState(
    { search: searchTerm },
    { search: { default: '' } },
    { debounceMs: 300 }
  );

  return {
    // Data
    cards,
    cardsLoading,
    cardsError,
    // Filter values
    searchTerm,
    sortBy,
    filters,
    selectedRarities,
    selectedSets,
    // Filter options
    allRarities,
    allSets,
    allFormats,
    setLabelMap,
    // Filtered data
    filteredCards,
    // Artist info
    displayArtistName: artistNameInfo.primaryName,
    alternateNames: artistNameInfo.alternateNames,
    // Handlers
    setSearchTerm,
    setSortBy: setSortByWithTransition,
    updateFilter: updateFilterWithTransition,
    // Loading state
    isFilteringOrSorting: isFiltering || isPending,
    // Collector state
    hasCollector
  };
};
