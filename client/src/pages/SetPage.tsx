import React, { useState, useEffect, useCallback, useMemo } from 'react';
import { useQuery } from '@apollo/client/react';
import { 
  Container, 
  Typography, 
  Box, 
  Alert
} from '@mui/material';
import { GET_CARDS_BY_SET_CODE } from '../graphql/queries/cards';
import { GET_SETS_BY_CODE } from '../graphql/queries/sets';
import { MtgSetCard } from '../components/organisms/MtgSetCard';
import { CardGroup } from '../components/organisms/CardGroup';
import { ResultsSummary } from '../components/atoms/shared/ResultsSummary';
import { SearchEmptyState } from '../components/atoms/shared/EmptyState';
import { useUrlState } from '../hooks/useUrlState';
import { useFilterState, commonFilters } from '../hooks/useFilterState';
import { QueryStateContainer, useQueryStates } from '../components/molecules/shared/QueryStateContainer';
import { FilterPanel } from '../components/molecules/shared/FilterPanel';
import { BackToTopFab } from '../components/molecules/shared/BackToTopFab';
import { 
  SectionErrorBoundary, 
  FilterErrorBoundary, 
  CardGridErrorBoundary 
} from '../components/ErrorBoundaries';
import type { CardGroupConfig } from '../types/cardGroup';
import type { Card } from '../types/card';
import type { MtgSet } from '../types/set';

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

const RARITIES = ['common', 'uncommon', 'rare', 'mythic', 'special', 'bonus'];

// Get unique artists from cards
const getUniqueArtists = (cards: Card[]): string[] => {
  const artistSet = new Set<string>();
  cards.forEach(card => {
    if (card.artist) {
      // Split multiple artists (e.g., "Artist 1 & Artist 2")
      const artists = card.artist.split(/\s+(?:&|and)\s+/i);
      artists.forEach(artist => artistSet.add(artist.trim()));
    }
  });
  return Array.from(artistSet).sort((a, b) => a.toLowerCase().localeCompare(b.toLowerCase()));
};

// Format card type labels for display
const formatCardTypeLabel = (type: string): string => {
  // Handle null/undefined
  if (!type) return 'Unknown';
  
  // Normalize the type for comparison (remove spaces, underscores, make uppercase)
  const normalizedType = type.replace(/[\s_-]/g, '').toUpperCase();
  
  // Special cases mapping - use normalized keys
  const specialCases: Record<string, string> = {
    'INBOOSTERS': 'In Boosters',
    'CARDVARIATIONS': 'Card Variations', 
    'FOILONLYBOOSTER': 'Foil Only Booster Cards',
    'PROMOCARDS': 'Promo Cards',
    'OTHERCARDS': 'Other Cards'
  };
  
  // Check normalized version
  if (specialCases[normalizedType]) {
    return specialCases[normalizedType];
  }
  
  // General formatting: replace underscores with spaces and capitalize words
  return type.replace(/[_-]/g, ' ').replace(/\b\w/g, c => c.toUpperCase());
};

// Determine which type group a card belongs to (matching the grouping logic)
const getCardType = (card: Card): string => {
  // Check for foil-only booster cards (7th Edition specific)
  if (card.booster && card.foil && !card.nonFoil) {
    return 'FOIL_ONLY_BOOSTER';
  }
  // Check for variations
  else if (card.variation) {
    return 'CARD_VARIATIONS';
  }
  // Check if it's in boosters (this is the "regular" cards)
  else if (card.booster) {
    return 'IN_BOOSTERS';
  }
  // Check for promos
  else if (card.promo) {
    return 'PROMO_CARDS';
  }
  // Default for cards that don't fit other categories
  return 'OTHER_CARDS';
};

// Get unique card types from cards (includes promo types, boosters, variations, etc.)
const getUniqueCardTypes = (cards: Card[]): string[] => {
  const cardTypeSet = new Set<string>();
  
  cards.forEach(card => {
    const cardType = getCardType(card);
    if (cardType !== 'OTHER_CARDS' || cards.some(c => getCardType(c) === 'OTHER_CARDS')) {
      // Only add OTHER_CARDS if at least one card actually belongs to it
      cardTypeSet.add(cardType);
    }
  });
  
  return Array.from(cardTypeSet)
    .sort((a, b) => formatCardTypeLabel(a).localeCompare(formatCardTypeLabel(b)));
};

export const SetPage: React.FC = () => {
  // Get set code from URL first
  const urlParams = new URLSearchParams(window.location.search);
  const setCode = urlParams.get('set') || '';

  // URL state configuration (don't manage 'page' or 'set' as they're routing params)
  const urlStateConfig = {
    search: { default: '' },
    rarities: { default: [] },
    artists: { default: [] },
    sort: { default: 'collector-asc' }
  };

  // Get initial values from URL
  const { getInitialValues } = useUrlState({}, urlStateConfig);
  const initialValues = getInitialValues();
  const { loading: cardsLoading, error: cardsError, data: cardsData } = useQuery<CardsResponse>(GET_CARDS_BY_SET_CODE, {
    variables: { setCode: { setCode } },
    skip: !setCode
  });
  
  // Get all artists from the data for normalization (moved up to be available for initial state)
  const allArtists = useMemo(() => {
    const allCards = cardsData?.cardsBySetCode?.data || [];
    return getUniqueArtists(allCards);
  }, [cardsData]);
  
  // Create artist map early for initial value normalization
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

  const { loading: setLoading, error: setError, data: setData } = useQuery<SetsResponse>(GET_SETS_BY_CODE, {
    variables: { codes: { setCodes: [setCode] } },
    skip: !setCode
  });

  // Configure filter state (memoized to prevent recreating on every render)
  const parseCollectorNumber = useCallback((num: string): number => {
    const match = num.match(/^(\d+)/);
    return match ? parseInt(match[1], 10) : 999999;
  }, []);

  const filterConfig = useMemo(() => ({
    searchFields: ['name', 'oracleText', 'typeLine', 'artist'] as (keyof Card)[],
    sortOptions: {
      'collector-asc': (a: Card, b: Card) => parseCollectorNumber(a.collectorNumber || '') - parseCollectorNumber(b.collectorNumber || ''),
      'collector-desc': (a: Card, b: Card) => parseCollectorNumber(b.collectorNumber || '') - parseCollectorNumber(a.collectorNumber || ''),
      'name-asc': (a: Card, b: Card) => a.name.localeCompare(b.name),
      'name-desc': (a: Card, b: Card) => b.name.localeCompare(a.name),
      'rarity': (a: Card, b: Card) => {
        const rarityOrder: Record<string, number> = { common: 0, uncommon: 1, rare: 2, mythic: 3, special: 4, bonus: 5 };
        return (rarityOrder[a.rarity || ''] || 99) - (rarityOrder[b.rarity || ''] || 99);
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
  }), [parseCollectorNumber]);

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
        showDigital: false
      }
    }
  );

  const selectedRarities = filters.rarities || [];
  
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
  const [selectedCardTypes, setSelectedCardTypes] = useState<string[]>([]);
  const [cardGroups, setCardGroups] = useState<CardGroupConfig[]>([]);
  const [visibleGroupIds, setVisibleGroupIds] = useState<Set<string>>(new Set());
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

  useEffect(() => {
    if (filteredCards) {
      
      // Group cards by their characteristics
      const groupsMap = new Map<string, CardGroupConfig>();
      
      filteredCards.forEach(card => {
        const cardType = getCardType(card);
        const groupId = cardType;
        const displayName = formatCardTypeLabel(cardType);
        
        if (!groupsMap.has(groupId)) {
          groupsMap.set(groupId, {
            id: groupId,
            name: cardType,
            displayName: displayName,
            cards: [],
            isVisible: true,
            isFoilOnly: cardType === 'FOIL_ONLY_BOOSTER',
            isVariation: cardType === 'CARD_VARIATIONS',
            isBooster: cardType === 'IN_BOOSTERS',
            isPromo: cardType === 'PROMO_CARDS'
          });
        }
        
        groupsMap.get(groupId)!.cards.push(card);
      });
      
      // Convert to array and sort by display name
      const groupsArray = Array.from(groupsMap.values())
        .sort((a, b) => {
          // Put "In Boosters" first as it's the regular cards
          if (a.id === 'IN_BOOSTERS') return -1;
          if (b.id === 'IN_BOOSTERS') return 1;
          return a.displayName.localeCompare(b.displayName);
        });
      
      setCardGroups(groupsArray);
      
      // Initially all groups are visible
      const allGroupIds = new Set(groupsArray.map(g => g.id));
      setVisibleGroupIds(allGroupIds);
    }
  }, [filteredCards]);
  
  // Handle card type filtering by showing/hiding groups
  useEffect(() => {
    if (selectedCardTypes.length === 0) {
      // Show all groups if no filter selected
      setVisibleGroupIds(new Set(cardGroups.map(g => g.id)));
    } else {
      // Only show selected groups
      setVisibleGroupIds(new Set(selectedCardTypes));
    }
  }, [selectedCardTypes, cardGroups]);

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

  const cards = cardsData?.cardsBySetCode?.data || [];
  const setInfo = setData?.setsByCode?.data?.[0];
  const setName = setInfo?.name || cards[0]?.setName || setCode.toUpperCase();
  // Note: allArtists and artistMap are now created earlier for initial value normalization
  const uniqueCardTypes = getUniqueCardTypes(cards);
  
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

      {/* Filters and Search */}
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
          multiSelects: [],
          autocompletes: [
            {
              key: 'rarities',
              value: selectedRarities,
              onChange: handleRarityChange,
              options: RARITIES,
              label: 'Rarity',
              placeholder: 'All Rarities',
              minWidth: 180
            },
            {
              key: 'artists',
              // Use selectedArtists which are already validated
              value: selectedArtists,
              onChange: (value) => updateFilter('artists', value),
              options: allArtists,
              label: 'Artist',
              placeholder: 'All Artists',
              minWidth: 250,
              maxTagsToShow: 1
            },
            ...(uniqueCardTypes.length > 1 ? [{
              key: 'cardTypes',
              value: selectedCardTypes,
              onChange: setSelectedCardTypes,
              options: uniqueCardTypes,
              label: 'Card Group',
              placeholder: 'All Types',
              minWidth: 250,
              getOptionLabel: formatCardTypeLabel
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

      <ResultsSummary 
        showing={cardGroups
          .filter(g => visibleGroupIds.has(g.id))
          .reduce((sum, g) => sum + g.cards.length, 0)} 
        total={filteredCards.length} 
        itemType="cards"
        textAlign="center"
      />

      {/* Card Groups */}
      <CardGridErrorBoundary name="SetPageCardGroups">
        {cardsLoading && (
          <CardGroup
            key="loading"
            groupId="loading"
            groupName="Loading"
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
        {!cardsLoading && cardGroups.map((group) => (
        <CardGroup
          key={group.id}
          groupId={group.id}
          groupName={group.displayName.toUpperCase()}
          cards={group.cards}
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
            setSelectedCardTypes([]);
          }}
        />
      )}

      {/* Back to Top Button */}
      <BackToTopFab />
    </Container>
    </QueryStateContainer>
  );
};