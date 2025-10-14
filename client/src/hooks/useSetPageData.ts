import { useState, useEffect, useMemo, useRef, startTransition } from 'react';
import { useQuery } from '@apollo/client/react';
import { useCardQueries } from './useCardQueries';
import { GET_SET_BY_CODE_WITH_GROUPINGS } from '../graphql/queries/sets';
import { useUrlState } from './useUrlState';
import { useFilterState } from './useFilterState';
import { useCollectorParam } from './useCollectorParam';
import { useQueryStates } from '../components/molecules/shared/QueryStateContainer';
import { RARITY_ORDER, parseCollectorNumber } from '../config/cardSortOptions';
import {
  getUniqueArtists,
  getUniqueRarities,
  getUniqueFinishes,
  createCardFilterFunctions
} from '../utils/cardUtils';
import type { Card } from '../types/card';
import type { MtgSet } from '../types/set';
import { groupCardsOptimized, getGroupDisplayName, getGroupOrder } from '../utils/optimizedCardGrouping';
import { useOptimizedSort } from './useOptimizedSort';
import { logger } from '../utils/logger';

interface CardsSuccessResponse {
  cardsBySetCode: {
    __typename: string;
    data?: Card[];
    status?: {
      message: string;
    };
  };
}

interface SetsResponse {
  setsByCode: {
    __typename: string;
    data?: MtgSet[];
    status?: {
      message: string;
      statusCode: number;
    };
  };
}

interface CardGroupConfig {
  id: string;
  name: string;
  displayName: string;
  cards: Card[];
  totalCards: number;
  isVisible: boolean;
  isFoilOnly: boolean;
  isVariation: boolean;
  isBooster: boolean;
  isPromo: boolean;
}

const EMPTY_CARDS_ARRAY: Card[] = [];

/**
 * Custom hook to manage all SetPage data, filtering, and state
 * Extracted from SetPage to reduce component complexity
 */
export const useSetPageData = (setCode: string | undefined) => {
  const { hasCollector, collectorId } = useCollectorParam();

  // URL state configuration
  const urlStateConfig = {
    search: { default: '' },
    rarities: { default: [] },
    artists: { default: [] },
    groups: { default: [] },
    showGroups: { default: true },
    sort: { default: 'collector-asc' },
    counts: { default: [] },
    signed: { default: [] }
  };

  const { getInitialValues, updateUrl } = useUrlState({}, urlStateConfig);
  const [initialValues] = useState(() => getInitialValues());

  // Card data fetching
  const { fetchSetCards } = useCardQueries();
  const [cardsLoading, setCardsLoading] = useState(false);
  const [cardsError, setCardsError] = useState<Error | null>(null);
  const [cardsData, setCardsData] = useState<CardsSuccessResponse | null>(null);
  const cardsDataRef = useRef<CardsSuccessResponse | null>(null);

  // Keep ref in sync with state
  useEffect(() => {
    cardsDataRef.current = cardsData;
    logger.debug('[useSetPageData] cardsData updated, card count:', cardsData?.cardsBySetCode?.data?.length || 0);
  }, [cardsData]);

  // Load cards effect
  useEffect(() => {
    if (!setCode) return;

    const loadCards = async () => {
      setCardsLoading(true);
      setCardsError(null);
      setCardsData({ cardsBySetCode: { __typename: 'CardsSuccessResponse', data: [] } });

      try {
        const cards = await fetchSetCards(setCode);
        setCardsData({
          cardsBySetCode: {
            __typename: 'CardsSuccessResponse',
            data: cards
          }
        });
      } catch (error) {
        setCardsError(error as Error);
        setCardsData({
          cardsBySetCode: {
            __typename: 'FailureResponse',
            status: {
              message: (error as Error).message
            }
          }
        });
      } finally {
        setCardsLoading(false);
      }
    };

    loadCards();
  }, [setCode, collectorId, fetchSetCards]);

  // Listen for collection updates
  useEffect(() => {
    const handleCollectionUpdate = (event: Event) => {
      const detail = (event as CustomEvent).detail;

      queueMicrotask(() => {
        const currentData = cardsDataRef.current;
        if (!currentData?.cardsBySetCode?.data) return;

        const updatedCards: Card[] = [];
        for (const card of currentData.cardsBySetCode.data) {
          if (card.id === detail.cardId) {
            updatedCards.push({
              ...card,
              userCollection: detail.userCollection
            });
          } else {
            updatedCards.push(card);
          }
        }

        setCardsData({
          cardsBySetCode: {
            __typename: 'CardsSuccessResponse',
            data: updatedCards
          }
        });
      });
    };

    window.addEventListener('collection-updated', handleCollectionUpdate as EventListener);
    return () => window.removeEventListener('collection-updated', handleCollectionUpdate as EventListener);
  }, []);

  // Get unique values
  const cards = cardsData?.cardsBySetCode?.data || EMPTY_CARDS_ARRAY;
  const allArtists = useMemo(() => getUniqueArtists(cards), [cards]);
  const allRarities = useMemo(() => getUniqueRarities(cards), [cards]);
  const allFinishes = useMemo(() => getUniqueFinishes(cards), [cards]);

  // Artist normalization map
  const artistMap = useMemo(() => {
    const map = new Map<string, string>();
    allArtists.forEach(artist => map.set(artist.toLowerCase(), artist));
    return map;
  }, [allArtists]);

  // Initial values processing
  const initialArtists = useMemo(() => {
    let raw = initialValues.artists || [];
    if (!Array.isArray(raw)) raw = raw ? [raw] : [];
    return raw;
  }, [initialValues.artists]);

  const initialGroups = useMemo(() => {
    let raw = initialValues.groups || [];
    if (!Array.isArray(raw)) raw = raw ? [raw] : [];
    return raw;
  }, [initialValues.groups]);

  // Fetch set metadata
  const { loading: setLoading, error: setError, data: setData } = useQuery<SetsResponse>(
    GET_SET_BY_CODE_WITH_GROUPINGS,
    {
      variables: { codes: { setCodes: [setCode] } },
      skip: !setCode
    }
  );

  const setInfo = setData?.setsByCode?.data?.[0];

  // Filter configuration
  const filterConfig = useMemo(() => {
    const cardFilterFunctions = createCardFilterFunctions<Card>();

    return {
      searchFields: ['name'] as (keyof Card)[],
      sortOptions: {
        'collector-asc': (a: Card, b: Card) => parseCollectorNumber(a.collectorNumber || '') - parseCollectorNumber(b.collectorNumber || ''),
        'collector-desc': (a: Card, b: Card) => parseCollectorNumber(b.collectorNumber || '') - parseCollectorNumber(a.collectorNumber || ''),
        'name-asc': (a: Card, b: Card) => a.name.localeCompare(b.name),
        'name-desc': (a: Card, b: Card) => b.name.localeCompare(a.name),
        'rarity': (a: Card, b: Card) => (RARITY_ORDER[a.rarity?.toLowerCase() || ''] ?? 99) - (RARITY_ORDER[b.rarity?.toLowerCase() || ''] ?? 99),
        'price-desc': (a: Card, b: Card) => parseFloat(b.prices?.usd || '0') - parseFloat(a.prices?.usd || '0'),
        'price-asc': (a: Card, b: Card) => parseFloat(a.prices?.usd || '0') - parseFloat(b.prices?.usd || '0'),
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
      defaultSort: 'collector-asc'
    };
  }, []);

  // Filter state
  const {
    searchTerm,
    sortBy,
    filters,
    filteredData: filteredCards,
    setSearchTerm,
    setSortBy,
    updateFilter
  } = useFilterState(
    cards,
    filterConfig,
    {
      search: (typeof initialValues.search === 'string' ? initialValues.search : '') || '',
      sort: (typeof initialValues.sort === 'string' ? initialValues.sort : 'collector-asc') || 'collector-asc',
      filters: {
        rarities: Array.isArray(initialValues.rarities) ? initialValues.rarities : (initialValues.rarities ? [initialValues.rarities] : []),
        artists: initialArtists,
        groups: initialGroups,
        showGroups: initialValues.showGroups !== undefined ? initialValues.showGroups : true,
        formats: [],
        collectionCounts: Array.isArray(initialValues.counts) ? initialValues.counts : (initialValues.counts ? [initialValues.counts] : []),
        signedCards: Array.isArray(initialValues.signed) ? initialValues.signed : (initialValues.signed ? [initialValues.signed] : [])
      }
    }
  );

  const selectedRarities = (Array.isArray(filters.rarities) ? filters.rarities : []) as string[];
  const selectedGroupIds = useMemo(() => (Array.isArray(filters.groups) ? filters.groups : []) as string[], [filters.groups]);

  // Apply optimized sorting
  const sortKey = hasCollector ? `${sortBy}-ctor-${collectorId}` : sortBy;
  const sortFn = filterConfig.sortOptions[sortBy as keyof typeof filterConfig.sortOptions] || filterConfig.sortOptions['collector-asc'];
  const sortedCards = useOptimizedSort(filteredCards, sortKey, sortFn);

  // Artist normalization effect
  const hasNormalizedRef = useRef(false);
  useEffect(() => {
    if (hasNormalizedRef.current) return;

    const artistsFilter = Array.isArray(filters.artists) ? filters.artists as string[] : [];
    if (artistMap && artistMap.size > 0 && artistsFilter.length > 0) {
      const normalized = artistsFilter.map((artist: string) => artistMap.get(artist.toLowerCase()) || artist);
      const needsUpdate = normalized.some((norm: string, i: number) => norm !== artistsFilter[i]);
      if (needsUpdate) {
        hasNormalizedRef.current = true;
        updateFilter('artists', normalized);
      }
    }
  }, [artistMap, filters.artists, updateFilter]);

  // URL sync effects
  useEffect(() => {
    const raritiesArray = Array.isArray(filters.rarities) ? filters.rarities as string[] : [];
    const artistsArray = Array.isArray(filters.artists) ? filters.artists as string[] : [];
    const groupsArray = Array.isArray(filters.groups) ? filters.groups as string[] : [];
    const countsArray = Array.isArray(filters.collectionCounts) ? filters.collectionCounts as string[] : [];
    const signedArray = Array.isArray(filters.signedCards) ? filters.signedCards as string[] : [];
    const showGroupsValue = typeof filters.showGroups === 'boolean' ? filters.showGroups : true;

    updateUrl({
      search: searchTerm || null,
      sort: sortBy !== 'collector-asc' ? sortBy : null,
      rarities: raritiesArray.length > 0 ? raritiesArray : null,
      artists: artistsArray.length > 0 ? artistsArray : null,
      groups: groupsArray.length > 0 ? groupsArray : null,
      showGroups: showGroupsValue !== true ? showGroupsValue : null,
      counts: (hasCollector && countsArray.length > 0) ? countsArray : null,
      signed: (hasCollector && signedArray.length > 0) ? signedArray : null
    });
  }, [searchTerm, sortBy, filters, hasCollector, updateUrl]);

  // Normalized selected artists
  const selectedArtists = useMemo(() => {
    const raw = Array.isArray(filters.artists) ? filters.artists as string[] : [];
    if (!artistMap || artistMap.size === 0) return raw;
    return raw
      .map((artist: string) => artistMap.get(artist.toLowerCase()) || artist)
      .filter((artist: string) => allArtists.includes(artist));
  }, [filters.artists, artistMap, allArtists]);

  // URL state sync
  useUrlState(
    {
      rarities: selectedRarities,
      artists: (Array.isArray(filters.artists) ? filters.artists : []) as string[],
      groups: (Array.isArray(filters.groups) ? filters.groups : []) as string[],
      showGroups: filters.showGroups,
      sort: sortBy,
      counts: (Array.isArray(filters.collectionCounts) ? filters.collectionCounts : []) as string[],
      signed: (Array.isArray(filters.signedCards) ? filters.signedCards : []) as string[],
      finishes: (Array.isArray(filters.finishes) ? filters.finishes : []) as string[]
    },
    {
      rarities: { default: [] },
      artists: { default: [] },
      groups: { default: [] },
      showGroups: { default: true },
      sort: { default: 'collector-asc' },
      counts: { default: [] },
      signed: { default: [] },
      finishes: { default: [] }
    },
    { debounceMs: 0 }
  );

  useUrlState(
    { search: searchTerm },
    { search: { default: '' } },
    { debounceMs: 300 }
  );

  // Card groups computation
  const allCardGroups = useMemo(() => {
    if (!cards || cards.length === 0) return [];

    const totalCardsPerGroup = groupCardsOptimized(cards, setInfo?.groupings);
    const groupsArray: CardGroupConfig[] = [];
    const groupings = setInfo?.groupings || [];

    for (const [groupId, allGroupCards] of totalCardsPerGroup.entries()) {
      groupsArray.push({
        id: groupId,
        name: groupId,
        displayName: getGroupDisplayName(groupId, groupings),
        cards: allGroupCards,
        totalCards: allGroupCards.length,
        isVisible: true,
        isFoilOnly: false,
        isVariation: false,
        isBooster: false,
        isPromo: false
      });
    }

    groupsArray.sort((a, b) => {
      if (groupings.length > 0) {
        return getGroupOrder(a.id, groupings) - getGroupOrder(b.id, groupings);
      }
      if (a.id === 'default-cards') return 1;
      if (b.id === 'default-cards') return -1;
      return a.displayName.localeCompare(b.displayName);
    });

    return groupsArray;
  }, [setInfo?.groupings, cards]);

  const cardGroups = useMemo(() => {
    if (!sortedCards || allCardGroups.length === 0) return allCardGroups;

    const groupedSortedCards = groupCardsOptimized(sortedCards, setInfo?.groupings);
    return allCardGroups.map(group => ({
      ...group,
      cards: groupedSortedCards.get(group.id) || []
    }));
  }, [allCardGroups, sortedCards, setInfo?.groupings]);

  const visibleGroupIds = useMemo(() => {
    const nonEmptyGroups = cardGroups.filter(g => g.cards.length > 0);
    if (selectedGroupIds.length === 0) {
      return new Set(nonEmptyGroups.map(g => g.id));
    }
    return new Set(selectedGroupIds.filter((id: string) => nonEmptyGroups.some(g => g.id === id)));
  }, [selectedGroupIds, cardGroups]);

  // Combined query states
  const { isLoading, firstError } = useQueryStates([
    { loading: cardsLoading, error: cardsError },
    { loading: setLoading, error: setError }
  ]);

  // Calculated values
  const setName = setInfo?.name || cards[0]?.setName || setCode?.toUpperCase() || '';
  const allSameReleaseDate = cards.length > 0 && cards.every(card => card.releasedAt === cards[0].releasedAt);
  const currentCount = filters.showGroups === false
    ? sortedCards.length
    : cardGroups
        .filter(g => g.cards.length > 0 && visibleGroupIds.has(g.id))
        .reduce((sum, g) => sum + g.cards.length, 0);

  // Handler functions
  const handleRarityChange = (value: string[]) => updateFilter('rarities', value);
  const handleSortChange = (value: string) => setSortBy(value);
  const handleClearFilters = () => {
    setSearchTerm('');
    updateFilter('rarities', []);
    updateFilter('artists', []);
    updateFilter('groups', []);
    updateFilter('showGroups', true);
  };
  const handleSearchChange = (value: string) => {
    startTransition(() => setSearchTerm(value));
  };

  return {
    // Data
    cards,
    cardsData,
    setInfo,
    setName,
    // Loading states
    isLoading,
    firstError,
    cardsLoading,
    // Filter values
    searchTerm,
    sortBy,
    filters,
    selectedRarities,
    selectedArtists,
    selectedGroupIds,
    // Filter options
    allArtists,
    allRarities,
    allFinishes,
    // Filtered/sorted data
    filteredCards,
    sortedCards,
    cardGroups,
    allCardGroups,
    visibleGroupIds,
    // Computed values
    allSameReleaseDate,
    currentCount,
    // Handlers
    handleSearchChange,
    handleSortChange,
    handleRarityChange,
    handleClearFilters,
    updateFilter,
    // Collector state
    hasCollector,
    collectorId
  };
};
