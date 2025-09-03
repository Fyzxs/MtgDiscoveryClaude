import React, { useState, useEffect, useCallback, useMemo } from 'react';
import { useQuery } from '@apollo/client/react';
import { useParams } from 'react-router-dom';
import { 
  Container, 
  Typography, 
  Box, 
  Alert
} from '@mui/material';
import { GET_CARDS_BY_SET_CODE } from '../graphql/queries/cards';
import { GET_SET_BY_CODE_WITH_GROUPINGS } from '../graphql/queries/sets';
import { MtgSetCard } from '../components/organisms/MtgSetCard';
import { CardGroup } from '../components/organisms/CardGroup';
import { ResultsSummary } from '../components/molecules/shared/ResultsSummary';
import { SearchEmptyState } from '../components/atoms/shared/EmptyState';
import { useUrlState } from '../hooks/useUrlState';
import { useFilterState, commonFilters } from '../hooks/useFilterState';
import { QueryStateContainer, useQueryStates } from '../components/molecules/shared/QueryStateContainer';
import { FilterPanel } from '../components/molecules/shared/FilterPanel';
import { RARITY_ORDER, parseCollectorNumber } from '../config/cardSortOptions';
import { getUniqueArtists, getUniqueRarities } from '../utils/cardUtils';
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
  // Get set code from route params
  const { setCode } = useParams<{ setCode: string }>();

  // URL state configuration for query parameters
  const urlStateConfig = {
    search: { default: '' },
    rarities: { default: [] },
    artists: { default: [] },
    groups: { default: [] },
    sort: { default: 'collector-asc' }
  };

  // Get initial values from URL only once on mount
  const { getInitialValues, updateUrl } = useUrlState({}, urlStateConfig);
  const [initialValues] = useState(() => getInitialValues());
  const { loading: cardsLoading, error: cardsError, data: cardsData } = useQuery<CardsResponse>(GET_CARDS_BY_SET_CODE, {
    variables: { setCode: { setCode } },
    skip: !setCode
  });
  
  // Get unique artists and rarities from data
  const allArtists = useMemo(() => getUniqueArtists(cardsData?.cardsBySetCode?.data || []), [cardsData]);
  const allRarities = useMemo(() => getUniqueRarities(cardsData?.cardsBySetCode?.data || []), [cardsData]);

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

  // Configure filter state (memoized to prevent recreating on every render)
  const filterConfig = useMemo(() => ({
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
      }
    },
    filterFunctions: {
      rarities: commonFilters.multiSelect<Card>('rarity'),
      artists: (card: Card, selectedArtists: string[]) => {
        if (!selectedArtists || selectedArtists.length === 0) return true;
        if (!card.artist) return false;
        const cardArtists = card.artist.split(/\s+(?:&|and)\s+/i).map(a => a.trim());
        // Case-insensitive comparison to handle URL parameters that might have different casing
        const normalizedSelected = selectedArtists.map(a => a.toLowerCase());
        return cardArtists.some(artist => normalizedSelected.includes(artist.toLowerCase()));
      },
      showDigital: (card: Card, show: boolean) => show || !card.digital
    },
    defaultSort: 'collector-asc'
  }), []);

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
        showDigital: false
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
      groups: filters.groups?.length > 0 ? filters.groups : null
    };
    
    updateUrl(urlUpdates);
  }, [searchTerm, sortBy, filters.rarities, filters.artists, filters.groups, updateUrl]);
  
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
  
  const [selectedCardId, setSelectedCardId] = useState<string | null>(null);




  
  // Example: When adding to collection (to be implemented)
  // const handleAddToCollection = () => {
  //   const selectedId = getSelectedCardId();
  //   if (selectedId) {
  //     // Add card with selectedId to collection
  //   }
  // };
  
  // This is no longer used for click handling, but kept for potential future use
  const handleCardSelection = useCallback((cardId: string, selected: boolean) => {
    setSelectedCardId(selected ? cardId : null);
  }, []);

  // Sync non-search filters with URL immediately
  useUrlState(
    {
      rarities: selectedRarities,
      artists: filters.artists || [],  // Use raw filter values to preserve URL state
      sort: sortBy
    },
    {
      rarities: { default: [] },
      artists: { default: [] },
      sort: { default: 'collector-asc' }
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

  // Define setInfo before using it in useEffect
  const setInfo = setData?.setsByCode?.data?.[0];
  const cards = cardsData?.cardsBySetCode?.data || [];

  // Stable computation of all card groups (independent of sorting/filtering)
  const allCardGroups = useMemo(() => {
    if (!setInfo || !cards) {
      return [];
    }

    // Calculate total cards per group from ALL cards (unfiltered)
    const totalCardsPerGroup = groupCardsOptimized(cards, setInfo.groupings);
    
    // Convert to CardGroupConfig format for compatibility
    const groupsArray: CardGroupConfig[] = [];
    
    // Get groupings for proper ordering
    const groupings = setInfo.groupings || [];
    
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
  const visibleGroupIds = useMemo(() => {
    if (selectedGroupIds.length === 0) {
      // No selection = show all groups
      return new Set(cardGroups.map(g => g.id));
    }
    // Show only selected groups
    return new Set(selectedGroupIds);
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

  // Check for GraphQL failure
  if (cardsData?.cardsBySetCode?.__typename === 'FailureResponse') {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          {cardsData.cardsBySetCode.status?.message || 'Failed to load cards'}
        </Alert>
      </Container>
    );
  }

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
          multiSelects: cardGroups.length > 1 ? [
            {
              key: 'cardGroups',
              value: selectedGroupIds,
              onChange: (groupIds: string[]) => updateFilter('groups', groupIds),
              options: cardGroups.map(g => ({ 
                value: g.id, 
                label: `${g.displayName} (${g.cards.length})` 
              })),
              label: 'Card Group',
              placeholder: 'All Groups',
              minWidth: 200
            }
          ] : [],
          autocompletes: [
            ...(allRarities.length > 1 ? [{
              key: 'rarities',
              value: selectedRarities,
              onChange: handleRarityChange,
              options: allRarities,
              label: 'Rarity',
              placeholder: 'All Rarities',
              minWidth: 180
            }] : []),
            ...(allArtists.length > 1 ? [{
              key: 'artists',
              // Use selectedArtists which are already validated
              value: selectedArtists,
              onChange: (value: string[]) => updateFilter('artists', value),
              options: allArtists,
              label: 'Artist',
              placeholder: 'All Artists',
              minWidth: 250,
              maxTagsToShow: 1
            }] : [])
          ],
          sort: {
            value: sortBy,
            onChange: handleSortChange,
            options: [
              { value: 'collector-asc', label: 'Collector # (Low-High)' },
              { value: 'collector-desc', label: 'Collector # (High-Low)' },
              { value: 'name-asc', label: 'Name (A-Z)' },
              { value: 'name-desc', label: 'Name (Z-A)' },
              { value: 'rarity', label: 'Rarity' },
              { value: 'price-desc', label: 'Price (High-Low)' },
              { value: 'price-asc', label: 'Price (Low-High)' },
              { 
                value: 'release-desc', 
                label: 'Release Date (Newest)',
                condition: cards.some(c => c.releasedAt !== cards[0]?.releasedAt)
              },
              { 
                value: 'release-asc', 
                label: 'Release Date (Oldest)',
                condition: cards.some(c => c.releasedAt !== cards[0]?.releasedAt)
              }
            ],
            minWidth: 180
          }
        }}
          layout="compact"
          sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}
        />
      </FilterErrorBoundary>
      )}

      <ResultsSummary 
        current={cardGroups
          .filter(g => visibleGroupIds.has(g.id))
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
                    hideReleaseDate: false
                  }}
                  onCardSelection={() => {}}
                  selectedCardId={null}
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
                  hideReleaseDate: false
                }}
                onCardSelection={() => {}}
                selectedCardId={null}
              />
            )}
          </>
        )}
        {!cardsLoading && cardGroups.map((group) => (
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
            hideReleaseDate: allSameReleaseDate
          }}
          onCardSelection={handleCardSelection}
          selectedCardId={selectedCardId}
        />
        ))}
      </CardGridErrorBoundary>

      {filteredCards.length === 0 && (
        <SearchEmptyState
          itemType="cards"
          onClear={() => {
            setSearchTerm('');
            updateFilter('rarities', []);
            updateFilter('artists', []);
            setSelectedGroupIds([]);
          }}
        />
      )}

      {/* Back to Top Button */}
      <BackToTopFab />
    </Container>
    </QueryStateContainer>
  );
};