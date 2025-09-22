import React, { useState, useEffect, useMemo } from 'react';
import { useQuery } from '@apollo/client/react';
import { useParams } from 'react-router-dom';
import {
  Container,
  Typography,
  Box,
  Alert,
  FormControlLabel,
  Switch
} from '@mui/material';
import { GET_CARDS_BY_SET_CODE } from '../graphql/queries/cards';
import { GET_SET_BY_CODE_WITH_GROUPINGS } from '../graphql/queries/sets';
import { MtgSetCard } from '../components/organisms/MtgSetCard';
import { CardGroup } from '../components/organisms/CardGroup';
import { ResultsSummary } from '../components/molecules/shared/ResultsSummary';
import { SearchEmptyState } from '../components/atoms/shared/EmptyState';
import { useUrlState } from '../hooks/useUrlState';
import { useFilterState } from '../hooks/useFilterState';
import { useCollectorParam } from '../hooks/useCollectorParam';
import { QueryStateContainer, useQueryStates } from '../components/molecules/shared/QueryStateContainer';
import { FilterPanel } from '../components/molecules/shared/FilterPanel';
import { RARITY_ORDER, parseCollectorNumber, SET_PAGE_SORT_OPTIONS, SET_PAGE_COLLECTOR_SORT_OPTIONS } from '../config/cardSortOptions';
import {
  getUniqueArtists,
  getUniqueRarities,
  getUniqueFinishes,
  getCollectionCountOptions,
  getSignedCardsOptions,
  createCardFilterFunctions
} from '../utils/cardUtils';
import { BackToTopFab } from '../components/molecules/shared/BackToTopFab';
import { 
  SectionErrorBoundary, 
  FilterErrorBoundary, 
  CardGridErrorBoundary 
} from '../components/ErrorBoundaries';
import type { Card } from '../types/card';
import type { MtgSet } from '../types/set';
import { groupCardsOptimized, getGroupDisplayName, getGroupOrder } from '../utils/optimizedCardGrouping';
import { useOptimizedSort } from '../hooks/useOptimizedSort';

interface CardsResponse {
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


export const SetPage: React.FC = () => {
  console.log('SetPage component rendering');

  // Get set code from route params
  const { setCode } = useParams<{ setCode: string }>();

  // Check for collector parameter in URL
  const { hasCollector, collectorId } = useCollectorParam();

  // Debug logging for collector data
  if (hasCollector) {
    console.log('SetPage: hasCollector=true, collectorId=', collectorId);
  }


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
  const { loading: cardsLoading, error: cardsError, data: cardsData } = useQuery<CardsResponse>(GET_CARDS_BY_SET_CODE, {
    variables: {
      setCode: {
        setCode,
        userId: collectorId || undefined
      }
    },
    skip: !setCode
  });
  
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
      search: initialValues.search || '',
      sort: initialValues.sort || 'collector-asc',
      filters: {
        rarities: Array.isArray(initialValues.rarities) ? initialValues.rarities : (initialValues.rarities ? [initialValues.rarities] : []),
        artists: initialArtists,  // Use raw initial values, normalize later
        groups: initialGroups,   // Add groups filter
        showGroups: initialValues.showGroups !== undefined ? initialValues.showGroups : true,
        showDigital: false,
        // Collector-specific filters
        collectionCounts: Array.isArray(initialValues.counts) ? initialValues.counts : (initialValues.counts ? [initialValues.counts] : []),
        signedCards: Array.isArray(initialValues.signed) ? initialValues.signed : (initialValues.signed ? [initialValues.signed] : [])
      }
    }
  );

  const selectedRarities = filters.rarities || [];
  const selectedGroupIds = filters.groups || [];
  
  // Apply optimized sorting to filtered cards
  const sortFn = (filterConfig.sortOptions as any)[sortBy] || filterConfig.sortOptions['collector-asc'];
  const sortedCards = useOptimizedSort(filteredCards, sortBy, sortFn);
  
  // Update artist filter to normalize casing when data loads
  useEffect(() => {
    if (artistMap && artistMap.size > 0 && filters.artists && filters.artists.length > 0) {
      const normalized = filters.artists.map((artist: string) => {
        const normalizedArtist = artistMap.get(artist.toLowerCase());
        return normalizedArtist || artist;
      });
      
      // Only update if the normalized version is different
      const needsUpdate = normalized.some((norm: string, i: number) => norm !== filters.artists[i]);
      if (needsUpdate) {
        updateFilter('artists', normalized);
      }
    }
  }, [artistMap]); // Only run when artistMap changes (when data loads)

  // Update URL when filters change
  useEffect(() => {
    const urlUpdates: Record<string, any> = {
      search: searchTerm || null,
      sort: sortBy !== 'collector-asc' ? sortBy : null,
      rarities: filters.rarities?.length > 0 ? filters.rarities : null,
      artists: filters.artists?.length > 0 ? filters.artists : null,
      groups: filters.groups?.length > 0 ? filters.groups : null,
      showGroups: filters.showGroups !== true ? filters.showGroups : null,
      // Collector-specific filters (only persist if hasCollector is true)
      counts: (hasCollector && filters.collectionCounts?.length > 0) ? filters.collectionCounts : null,
      signed: (hasCollector && filters.signedCards?.length > 0) ? filters.signedCards : null
    };

    updateUrl(urlUpdates);
  }, [searchTerm, sortBy, filters.rarities, filters.artists, filters.groups, filters.showGroups, filters.collectionCounts, filters.signedCards, hasCollector, updateUrl]);
  
  // Normalize selected artists to match the case in our data
  const selectedArtists = useMemo(() => {
    const raw = filters.artists || [];
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
      artists: filters.artists || [],  // Use raw filter values to preserve URL state
      groups: filters.groups || [],
      showGroups: filters.showGroups,
      sort: sortBy,
      // Collector-specific filters
      counts: filters.collectionCounts || [],
      signed: filters.signedCards || [],
      finishes: filters.finishes || []
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

  const cards = cardsData?.cardsBySetCode?.data || [];

  // Debug logging for query errors
  if (cardsError) {
    console.error('SetPage: cardsError=', cardsError);
    if (hasCollector) {
      console.error('SetPage: Error occurred with collectorId=', collectorId);
    }
  }
  if (hasCollector && cards.length > 0) {
    console.log('SetPage: First card userCollection=', cards[0].userCollection);
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
    
    // Get groupings for proper ordering
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
    
    // Sort groups by their defined order
    groupsArray.sort((a, b) => {
      const orderA = getGroupOrder(a.id, groupings);
      const orderB = getGroupOrder(b.id, groupings);
      return orderA - orderB;
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
  // Note: allArtists and artistMap are now created earlier for initial value normalization

  // Check if all cards have the same release date
  const allSameReleaseDate = cards.length > 0 &&
    cards.every(card => card.releasedAt === cards[0].releasedAt);

  return (
    <QueryStateContainer
      loading={isLoading}
      error={firstError}
      containerProps={{ maxWidth: false }}
    >
    <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
      {/* Set Information Card */}
      {setInfo && (
        <SectionErrorBoundary name="SetInfoCard">
          <Box sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}>
            <MtgSetCard set={setInfo} />
          </Box>
        </SectionErrorBoundary>
      )}
      
      {/* Fallback title if no set info */}
      {!setInfo && (
        <Typography variant="h3" component="h1" gutterBottom sx={{ mb: 4, textAlign: 'center' }}>
          {setName} ({setCode.toUpperCase()})
        </Typography>
      )}

      {/* Filters and Search - Hide completely if only 1 card */}
      {cards.length > 1 && (
      <FilterErrorBoundary name="SetPageFilters">
        <FilterPanel
        config={{
          search: {
            value: initialValues.search || '',
            onChange: setSearchTerm,
            placeholder: 'Search cards...',
            debounceMs: 300,
            minWidth: 300
          },
          multiSelects: [
            // Card Groups (if multiple groups exist)
            ...(cardGroups.filter(g => g.cards.length > 0).length > 1 && filters.showGroups !== false ? [{
              key: 'cardGroups',
              value: selectedGroupIds,
              onChange: (groupIds: string[]) => updateFilter('groups', groupIds),
              options: cardGroups
                .filter(g => g.cards.length > 0) // Only show groups with cards
                .map(g => ({
                  value: g.id,
                  label: `${g.displayName} (${g.cards.length})`
                })),
              label: 'Card Group',
              placeholder: 'All Groups',
              minWidth: 200
            }] : []),
            // Rarities filter (now using multiSelect for "Clear All" option)
            ...(allRarities.length > 1 ? [{
              key: 'rarities',
              value: selectedRarities,
              onChange: handleRarityChange,
              options: allRarities.map(rarity => ({ value: rarity, label: rarity })),
              label: 'Rarity',
              placeholder: 'All Rarities',
              minWidth: 150
            }] : []),
            // Artists filter (now using multiSelect for "Clear All" option)
            ...(allArtists.length > 1 ? [{
              key: 'artists',
              value: selectedArtists,
              onChange: (value: string[]) => updateFilter('artists', value),
              options: allArtists.map(artist => ({ value: artist, label: artist })),
              label: 'Artist',
              placeholder: 'All Artists',
              minWidth: 200
            }] : []),
            // Finishes filter (general filter, shown for all users)
            ...(allFinishes.length > 1 ? [{
              key: 'finishes',
              value: filters.finishes || [],
              onChange: (value: string[]) => updateFilter('finishes', value),
              options: allFinishes.map(finish => ({ value: finish, label: finish })),
              label: 'Finish',
              placeholder: 'All Finishes',
              minWidth: 150
            }] : [])
          ],
          sort: {
            value: sortBy,
            onChange: handleSortChange,
            options: (hasCollector ? SET_PAGE_COLLECTOR_SORT_OPTIONS : SET_PAGE_SORT_OPTIONS).map(opt => {
              // Add conditional display for release date options
              if (opt.value === 'release-desc' || opt.value === 'release-asc') {
                return {
                  ...opt,
                  condition: cards.some(c => c.releasedAt !== cards[0]?.releasedAt)
                };
              }
              return opt;
            }),
            minWidth: 200
          },
          customFilters: cardGroups.filter(g => g.cards.length > 0).length > 1 ? [
            <FormControlLabel
              key="show-groups-toggle"
              control={
                <Switch
                  checked={filters.showGroups !== false}
                  onChange={(e) => updateFilter('showGroups', e.target.checked)}
                  size="small"
                />
              }
              label="Show Card Groups"
              sx={{ minWidth: 150 }}
            />
          ] : [],
          collectorFilters: hasCollector ? {
            collectionCounts: {
              key: 'collectionCounts',
              value: filters.collectionCounts || [],
              onChange: (value: string[]) => updateFilter('collectionCounts', value),
              options: getCollectionCountOptions(),
              label: 'Collection Count',
              placeholder: 'All Counts',
              minWidth: 180
            },
            signedCards: {
              key: 'signedCards',
              value: filters.signedCards || [],
              onChange: (value: string[]) => updateFilter('signedCards', value),
              options: getSignedCardsOptions(),
              label: 'Signed Cards',
              placeholder: 'All Cards',
              minWidth: 150
            }
          } : undefined
        }}
          layout="compact"
          sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}
        />
      </FilterErrorBoundary>
      )}

      <ResultsSummary 
        current={filters.showGroups === false 
          ? sortedCards.length 
          : cardGroups
              .filter(g => g.cards.length > 0 && visibleGroupIds.has(g.id))
              .reduce((sum, g) => sum + g.cards.length, 0)} 
        total={cards.length} 
        label="cards"
        textAlign="center"
      />

      {/* Card Groups */}
      <CardGridErrorBoundary name="SetPageCardGroups">
        {cardsLoading && (
          <>
            {/* If we have setInfo with groupings, show loading skeleton for each group */}
            {setInfo?.groupings && setInfo.groupings.length > 0 ? (
              setInfo.groupings.map((grouping) => (
                <CardGroup
                  key={`loading-${grouping.id}`}
                  groupId={grouping.id}
                  groupName={grouping.displayName.toUpperCase()}
                  cards={[]}
                  isVisible={true}
                  showHeader={true}
                  isLoading={true}
                  context={{
                    isOnSetPage: true,
                    currentSet: setCode,
                    hideSetInfo: true,
                    hideReleaseDate: false,
                    hasCollector
                  }}
                />
              ))
            ) : (
              /* Otherwise show a single loading group */
              <CardGroup
                key="loading-default"
                groupId="default-cards"
                groupName="LOADING CARDS"
                cards={[]}
                isVisible={true}
                showHeader={false}
                isLoading={true}
                context={{
                  isOnSetPage: true,
                  currentSet: setCode,
                  hideSetInfo: true,
                  hideReleaseDate: false,
                  hasCollector
                }}
              />
            )}
          </>
        )}
        {!cardsLoading && (
          filters.showGroups === false ? (
            // Flat display: show all sorted cards in a single group
            <CardGroup
              key="all-cards"
              groupId="all-cards"
              groupName="ALL CARDS"
              cards={sortedCards}
              isVisible={true}
              showHeader={false}
              context={{
                isOnSetPage: true,
                currentSet: setCode,
                hideSetInfo: true,
                hideReleaseDate: allSameReleaseDate,
                hasCollector
              }}
            />
          ) : (
            // Grouped display: show cards organized by groups (filter out empty groups)
            cardGroups
              .filter(group => group.cards.length > 0) // Only show groups with cards
              .map((group) => (
                <CardGroup
                  key={group.id}
                  groupId={group.id}
                  groupName={group.displayName.toUpperCase()}
                  cards={group.cards}
                  totalCards={group.totalCards}
                  isVisible={visibleGroupIds.has(group.id)}
                  showHeader={cardGroups.length > 1}
                  context={{
                    isOnSetPage: true,
                    currentSet: setCode,
                    hideSetInfo: true,
                    hideReleaseDate: allSameReleaseDate,
                    hasCollector
                  }}
                />
              ))
          )
        )}
      </CardGridErrorBoundary>

      {filteredCards.length === 0 && (
        <SearchEmptyState
          itemType="cards"
          onClear={() => {
            setSearchTerm('');
            updateFilter('rarities', []);
            updateFilter('artists', []);
            updateFilter('groups', []);
            updateFilter('showGroups', true);
          }}
        />
      )}

      {/* Back to Top Button */}
      <BackToTopFab />
    </Container>
    </QueryStateContainer>
  );
};