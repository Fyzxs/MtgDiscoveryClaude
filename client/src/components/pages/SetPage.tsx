import React, { useState, useEffect, useMemo, useRef, startTransition } from 'react';
import { useQuery } from '@apollo/client/react';
import { useParams } from 'react-router-dom';
import { useCardCache } from '../../hooks/useCardCache';
import {
  Container,
  Alert
} from '@mui/material';
import { GET_SET_BY_CODE_WITH_GROUPINGS } from '../../graphql/queries/sets';
import { useUrlState } from '../../hooks/useUrlState';
import { useFilterState } from '../../hooks/useFilterState';
import { useCollectorParam } from '../../hooks/useCollectorParam';
import { useQueryStates } from '../molecules/shared/QueryStateContainer';
import { RARITY_ORDER, parseCollectorNumber } from '../../config/cardSortOptions';
import {
  getUniqueArtists,
  getUniqueRarities,
  getUniqueFinishes,
  createCardFilterFunctions
} from '../../utils/cardUtils';
import type { Card } from '../../types/card';
import type { MtgSet } from '../../types/set';
import { groupCardsOptimized, getGroupDisplayName, getGroupOrder } from '../../utils/optimizedCardGrouping';
import { useOptimizedSort } from '../../hooks/useOptimizedSort';
import { SetPageTemplate } from '../templates/SetPageTemplate';
import { SetPageHeader } from '../organisms/SetPageHeader';
import { SetPageFilters } from '../organisms/SetPageFilters';
import { SetPageCardDisplay } from '../organisms/SetPageCardDisplay';

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
  totalCards: number; // Total cards in this group before filtering
  isVisible: boolean;
  isFoilOnly: boolean;
  isVariation: boolean;
  isBooster: boolean;
  isPromo: boolean;
}

// Stable empty array to prevent infinite re-renders
const EMPTY_CARDS_ARRAY: Card[] = [];

export const SetPage: React.FC = () => {
  console.log('SetPage component rendering');

  // Get set code from route params
  const { setCode } = useParams<{ setCode: string }>();

  // Check for collector parameter in URL
  const { hasCollector, collectorId } = useCollectorParam();


  // State for collection data

  // URL state configuration for query parameters
  const urlStateConfig = {
    search: { default: '' },
    rarities: { default: [] },
    artists: { default: [] },
    groups: { default: [] },
    showGroups: { default: true },
    sort: { default: 'collector-asc' },
    // Collector-specific filters
    counts: { default: [] },
    signed: { default: [] }
  };

  // Get initial values from URL only once on mount
  const { getInitialValues, updateUrl } = useUrlState({}, urlStateConfig);
  const [initialValues] = useState(() => getInitialValues());

  // Use card cache for fetching cards
  const { fetchSetCards } = useCardCache();
  const [cardsLoading, setCardsLoading] = useState(false);
  const [cardsError, setCardsError] = useState<Error | null>(null);
  const [cardsData, setCardsData] = useState<CardsSuccessResponse | null>(null);
  const cardsDataRef = useRef<CardsSuccessResponse | null>(null);

  // Keep ref in sync with state
  useEffect(() => {
    cardsDataRef.current = cardsData;
    console.log('[SetPage] cardsData state updated, card count:', cardsData?.cardsBySetCode?.data?.length || 0);
  }, [cardsData]);

  useEffect(() => {
    if (!setCode) return;

    const loadCards = async () => {
      setCardsLoading(true);
      setCardsError(null);
      // Clear stale cards immediately when collectorId changes
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

  // Listen for collection updates and update the card in place
  useEffect(() => {
    const handleCollectionUpdate = (event: Event) => {
      const detail = (event as CustomEvent).detail;
      console.log('[SetPage] Collection updated event detail:', detail);
      console.log('[SetPage] userCollection from event:', detail.userCollection);

      // Defer the heavy array operations to avoid blocking UI
      queueMicrotask(() => {
        // Update the specific card's userCollection in the existing cards array
        const currentData = cardsDataRef.current;

        if (!currentData?.cardsBySetCode?.data) {
          console.log('[SetPage] No cards data to update');
          return;
        }

        console.log('[SetPage] Searching for card in', currentData.cardsBySetCode.data.length, 'cards');
        let foundCard = false;

        // Force a completely new array with new card object
        const updatedCards: Card[] = [];

        for (const card of currentData.cardsBySetCode.data) {
          if (card.id === detail.cardId) {
            foundCard = true;
            console.log('[SetPage] ✅ Found and updating card:', card.name);
            console.log('[SetPage] Old userCollection:', card.userCollection);
            console.log('[SetPage] New userCollection:', detail.userCollection);

            try {
              // Create a completely new card object
              const updatedCard = {
                ...card,
                userCollection: detail.userCollection
              };
              console.log('[SetPage] Created updatedCard successfully');
              updatedCards.push(updatedCard);
              console.log('[SetPage] Pushed updated card, array now has', updatedCards.length, 'cards');
            } catch (err) {
              console.error('[SetPage] Error creating updated card:', err);
            }
          } else {
            updatedCards.push(card);
          }
        }

        if (!foundCard) {
          console.log('[SetPage] ❌ Card not found with ID:', detail.cardId);
          console.log('[SetPage] First 3 card IDs:', currentData.cardsBySetCode.data.slice(0, 3).map(c => c.id));
        }

        console.log('[SetPage] Updated cards array, triggering re-render');
        const newState = {
          cardsBySetCode: {
            __typename: 'CardsSuccessResponse',
            data: updatedCards
          }
        };
        console.log('[SetPage] Setting new state with', updatedCards.length, 'cards');
        setCardsData(newState);
      });
    };

    window.addEventListener('collection-updated', handleCollectionUpdate as EventListener);
    return () => window.removeEventListener('collection-updated', handleCollectionUpdate as EventListener);
  }, []);
  
  // Get unique artists, rarities, and finishes from data
  const allArtists = useMemo(() => getUniqueArtists(cardsData?.cardsBySetCode?.data || []), [cardsData]);
  const allRarities = useMemo(() => getUniqueRarities(cardsData?.cardsBySetCode?.data || []), [cardsData]);
  const allFinishes = useMemo(() => getUniqueFinishes(cardsData?.cardsBySetCode?.data || []), [cardsData]);


  // Create artist map for normalization
  const artistMap = useMemo(() => {
    const map = new Map<string, string>();
    allArtists.forEach(artist => {
      map.set(artist.toLowerCase(), artist);
    });
    return map;
  }, [allArtists]);

  // Get initial artists from URL as array
  const initialArtists = useMemo(() => {
    let raw = initialValues.artists || [];
    // Ensure raw is always an array (URL parsing might return a string for single values)
    if (!Array.isArray(raw)) {
      raw = raw ? [raw] : [];
    }
    return raw;
  }, [initialValues.artists]);

  // Get initial groups from URL as array
  const initialGroups = useMemo(() => {
    let raw = initialValues.groups || [];
    // Ensure raw is always an array (URL parsing might return a string for single values)
    if (!Array.isArray(raw)) {
      raw = raw ? [raw] : [];
    }
    return raw;
  }, [initialValues.groups]);

  const { loading: setLoading, error: setError, data: setData } = useQuery<SetsResponse>(GET_SET_BY_CODE_WITH_GROUPINGS, {
    variables: { codes: { setCodes: [setCode] } },
    skip: !setCode
  });

  // Define setInfo before using it in user cards query
  const setInfo = setData?.setsByCode?.data?.[0];



  // Configure filter state (memoized to prevent recreating on every render)
  const filterConfig = useMemo(() => {
    const cardFilterFunctions = createCardFilterFunctions<Card>();

    return {
      searchFields: ['name'] as (keyof Card)[],
      sortOptions: {
        'collector-asc': (a: Card, b: Card) => parseCollectorNumber(a.collectorNumber || '') - parseCollectorNumber(b.collectorNumber || ''),
        'collector-desc': (a: Card, b: Card) => parseCollectorNumber(b.collectorNumber || '') - parseCollectorNumber(a.collectorNumber || ''),
        'name-asc': (a: Card, b: Card) => a.name.localeCompare(b.name),
        'name-desc': (a: Card, b: Card) => b.name.localeCompare(a.name),
        'rarity': (a: Card, b: Card) => {
          return (RARITY_ORDER[a.rarity?.toLowerCase() || ''] ?? 99) - (RARITY_ORDER[b.rarity?.toLowerCase() || ''] ?? 99);
        },
        'price-desc': (a: Card, b: Card) => {
          const priceA = parseFloat(a.prices?.usd || '0');
          const priceB = parseFloat(b.prices?.usd || '0');
          return priceB - priceA;
        },
        'price-asc': (a: Card, b: Card) => {
          const priceA = parseFloat(a.prices?.usd || '0');
          const priceB = parseFloat(b.prices?.usd || '0');
          return priceA - priceB;
        },
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
        // Collection count sorting (only available for collector views)
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

  const {
    searchTerm,
    sortBy,
    filters,
    filteredData: filteredCards,
    setSearchTerm,
    setSortBy,
    updateFilter
  } = useFilterState(
    cardsData?.cardsBySetCode?.data,
    filterConfig,
    {
      search: (typeof initialValues.search === 'string' ? initialValues.search : '') || '',
      sort: (typeof initialValues.sort === 'string' ? initialValues.sort : 'collector-asc') || 'collector-asc',
      filters: {
        rarities: Array.isArray(initialValues.rarities) ? initialValues.rarities : (initialValues.rarities ? [initialValues.rarities] : []),
        artists: initialArtists,  // Use raw initial values, normalize later
        groups: initialGroups,   // Add groups filter
        showGroups: initialValues.showGroups !== undefined ? initialValues.showGroups : true,
        formats: [], // Empty means show all (both paper and digital)
        // Collector-specific filters
        collectionCounts: Array.isArray(initialValues.counts) ? initialValues.counts : (initialValues.counts ? [initialValues.counts] : []),
        signedCards: Array.isArray(initialValues.signed) ? initialValues.signed : (initialValues.signed ? [initialValues.signed] : [])
      }
    }
  );

  const selectedRarities = (Array.isArray(filters.rarities) ? filters.rarities : []) as string[];
  const selectedGroupIds = useMemo(() => (Array.isArray(filters.groups) ? filters.groups : []) as string[], [filters.groups]);

  // Apply optimized sorting to filtered cards
  // Include collectorId in sort key to invalidate cache when collection context changes
  const sortKey = hasCollector ? `${sortBy}-ctor-${collectorId}` : sortBy;
  const sortFn = filterConfig.sortOptions[sortBy as keyof typeof filterConfig.sortOptions] || filterConfig.sortOptions['collector-asc'];
  const sortedCards = useOptimizedSort(filteredCards, sortKey, sortFn);
  
  // Track if we've normalized artists to avoid infinite loops
  const hasNormalizedRef = useRef(false);

  // Update artist filter to normalize casing when data loads
  useEffect(() => {
    if (hasNormalizedRef.current) {
      return; // Already normalized, don't run again
    }

    const artistsFilter = Array.isArray(filters.artists) ? filters.artists as string[] : [];
    if (artistMap && artistMap.size > 0 && artistsFilter.length > 0) {
      const normalized = artistsFilter.map((artist: string) => {
        const normalizedArtist = artistMap.get(artist.toLowerCase());
        return normalizedArtist || artist;
      });

      // Only update if the normalized version is different
      const needsUpdate = normalized.some((norm: string, i: number) => norm !== artistsFilter[i]);
      if (needsUpdate) {
        hasNormalizedRef.current = true;
        updateFilter('artists', normalized);
      }
    }
  }, [artistMap, filters.artists, updateFilter]); // Run when artistMap or artists change

  // Update URL when filters change
  useEffect(() => {
    const raritiesArray = Array.isArray(filters.rarities) ? filters.rarities as string[] : [];
    const artistsArray = Array.isArray(filters.artists) ? filters.artists as string[] : [];
    const groupsArray = Array.isArray(filters.groups) ? filters.groups as string[] : [];
    const countsArray = Array.isArray(filters.collectionCounts) ? filters.collectionCounts as string[] : [];
    const signedArray = Array.isArray(filters.signedCards) ? filters.signedCards as string[] : [];

    const showGroupsValue = typeof filters.showGroups === 'boolean' ? filters.showGroups : true;
    const urlUpdates: Record<string, string | string[] | boolean | null> = {
      search: searchTerm || null,
      sort: sortBy !== 'collector-asc' ? sortBy : null,
      rarities: raritiesArray.length > 0 ? raritiesArray : null,
      artists: artistsArray.length > 0 ? artistsArray : null,
      groups: groupsArray.length > 0 ? groupsArray : null,
      showGroups: showGroupsValue !== true ? showGroupsValue : null,
      // Collector-specific filters (only persist if hasCollector is true)
      counts: (hasCollector && countsArray.length > 0) ? countsArray : null,
      signed: (hasCollector && signedArray.length > 0) ? signedArray : null
    };

    updateUrl(urlUpdates);
  }, [searchTerm, sortBy, filters.rarities, filters.artists, filters.groups, filters.showGroups, filters.collectionCounts, filters.signedCards, hasCollector, updateUrl]);

  // Normalize selected artists to match the case in our data
  const selectedArtists = useMemo(() => {
    const raw = Array.isArray(filters.artists) ? filters.artists as string[] : [];
    // If data hasn't loaded yet, use the raw values
    if (!artistMap || artistMap.size === 0) return raw;
    // Once we have data, normalize and validate
    return raw.map((artist: string) => {
      // Try to find the correctly cased version
      const normalized = artistMap.get(artist.toLowerCase());
      return normalized || artist;
    }).filter((artist: string) => allArtists.includes(artist)); // Only keep valid artists
  }, [filters.artists, artistMap, allArtists]);
  

  // Sync non-search filters with URL immediately
  useUrlState(
    {
      rarities: selectedRarities,
      artists: (Array.isArray(filters.artists) ? filters.artists : []) as string[],  // Use raw filter values to preserve URL state
      groups: (Array.isArray(filters.groups) ? filters.groups : []) as string[],
      showGroups: filters.showGroups,
      sort: sortBy,
      // Collector-specific filters
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
    {
      debounceMs: 0 // No debounce for dropdown/sort changes
    }
  );

  // Sync search term separately with debounce
  useUrlState(
    {
      search: searchTerm
    },
    {
      search: { default: '' }
    },
    {
      debounceMs: 300 // Match the search input debounce
    }
  );

  const cards = cardsData?.cardsBySetCode?.data || EMPTY_CARDS_ARRAY;

  // Debug logging for query errors
  if (cardsError) {
    console.error('SetPage: cardsError=', cardsError);
  }

  // Stable computation of all card groups (independent of sorting/filtering)
  const allCardGroups = useMemo(() => {
    if (!cards || cards.length === 0) {
      return [];
    }

    // Calculate total cards per group from ALL cards (unfiltered)
    const totalCardsPerGroup = groupCardsOptimized(cards, setInfo?.groupings);

    // Convert to CardGroupConfig format for compatibility
    const groupsArray: CardGroupConfig[] = [];

    // Get groupings metadata for proper ordering and display names
    const groupings = setInfo?.groupings || [];

    // Process each group using total cards (before any filtering/sorting)
    for (const [groupId, allGroupCards] of totalCardsPerGroup.entries()) {
      const displayName = getGroupDisplayName(groupId, groupings);

      groupsArray.push({
        id: groupId,
        name: groupId,
        displayName: displayName,
        cards: allGroupCards, // Will be replaced with filtered cards later
        totalCards: allGroupCards.length,
        isVisible: true,
        isFoilOnly: false,
        isVariation: false,
        isBooster: false,
        isPromo: false
      });
    }

    // Sort groups by their defined order (if groupings metadata exists)
    // Otherwise, sort alphabetically by display name with 'default-cards' last
    groupsArray.sort((a, b) => {
      if (groupings.length > 0) {
        // Use defined order from groupings metadata
        const orderA = getGroupOrder(a.id, groupings);
        const orderB = getGroupOrder(b.id, groupings);
        return orderA - orderB;
      } else {
        // Fallback: put 'default-cards' last, sort others alphabetically
        if (a.id === 'default-cards') return 1;
        if (b.id === 'default-cards') return -1;
        return a.displayName.localeCompare(b.displayName);
      }
    });


    return groupsArray;
  }, [setInfo?.groupings, cards]); // Only depends on raw data, not sorting/filtering

  // Apply filtering and sorting to the stable groups
  const cardGroups = useMemo(() => {
    if (!sortedCards || allCardGroups.length === 0) {
      return allCardGroups;
    }

    // Re-group the sorted/filtered cards
    const groupedSortedCards = groupCardsOptimized(sortedCards, setInfo?.groupings);

    // Update each group with filtered/sorted cards
    return allCardGroups.map(group => ({
      ...group,
      cards: groupedSortedCards.get(group.id) || []
    }));
  }, [allCardGroups, sortedCards, setInfo?.groupings]);

  // Compute visible groups: if no groups selected, show all; otherwise show only selected
  // Only include groups that have cards
  const visibleGroupIds = useMemo(() => {
    const nonEmptyGroups = cardGroups.filter(g => g.cards.length > 0);
    
    if (selectedGroupIds.length === 0) {
      // No selection = show all non-empty groups
      return new Set(nonEmptyGroups.map(g => g.id));
    }
    // Show only selected groups that have cards
    return new Set(selectedGroupIds.filter((id: string) => 
      nonEmptyGroups.some(g => g.id === id)
    ));
  }, [selectedGroupIds, cardGroups]);

  const handleRarityChange = (value: string[]) => {
    updateFilter('rarities', value);
  };

  const handleSortChange = (value: string) => {
    setSortBy(value);
  };

  // Combine query states
  const { isLoading, firstError } = useQueryStates([
    { loading: cardsLoading, error: cardsError },
    { loading: setLoading, error: setError }
  ]);

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

  const setName = setInfo?.name || cards[0]?.setName || setCode.toUpperCase();

  // Check if all cards have the same release date
  const allSameReleaseDate = cards.length > 0 &&
    cards.every(card => card.releasedAt === cards[0].releasedAt);

  // Clear all filters handler
  const handleClearFilters = () => {
    setSearchTerm('');
    updateFilter('rarities', []);
    updateFilter('artists', []);
    updateFilter('groups', []);
    updateFilter('showGroups', true);
  };

  // Calculate current count for results summary
  const currentCount = filters.showGroups === false
    ? sortedCards.length
    : cardGroups
        .filter(g => g.cards.length > 0 && visibleGroupIds.has(g.id))
        .reduce((sum, g) => sum + g.cards.length, 0);

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
          onSearchChange={(value: string) => {
            startTransition(() => {
              setSearchTerm(value);
            });
          }}
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