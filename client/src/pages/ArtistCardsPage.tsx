import React, { useState, useMemo } from 'react';
import { useQuery } from '@apollo/client/react';
import { useParams } from 'react-router-dom';
import { 
  Container, 
  Typography, 
  Box,
  Alert
} from '@mui/material';
import { GET_CARDS_BY_ARTIST } from '../graphql/queries/cards';
import { ResponsiveGridAutoFit } from '../components/atoms/layouts/ResponsiveGrid';
import { MtgCard } from '../components/organisms/MtgCard';
import { ResultsSummary } from '../components/molecules/shared/ResultsSummary';
import { SearchEmptyState } from '../components/atoms/shared/EmptyState';
import { useUrlState } from '../hooks/useUrlState';
import { useFilterState, commonFilters } from '../hooks/useFilterState';
import { QueryStateContainer } from '../components/molecules/shared/QueryStateContainer';
import { FilterPanel } from '../components/molecules/shared/FilterPanel';
import { RARITY_ORDER } from '../config/cardSortOptions';
import { getUniqueRarities, getUniqueSets } from '../utils/cardUtils';
import { BackToTopFab } from '../components/molecules/shared/BackToTopFab';
import { 
  SectionErrorBoundary, 
  FilterErrorBoundary, 
  CardGridErrorBoundary 
} from '../components/ErrorBoundaries';
import type { Card } from '../types/card';

interface CardsResponse {
  cardsByArtistName: {
    __typename: string;
    data?: Card[];
    status?: {
      message: string;
    };
  };
}

// Helper function to Pascal case names
const toPascalCase = (str: string): string => {
  return str.split(' ').map(word => 
    word.charAt(0).toUpperCase() + word.slice(1).toLowerCase()
  ).join(' ');
};

// Helper function to find most common artist name and alternates
const getArtistNameInfo = (cards: Card[], initialName: string) => {
  const artistCounts = new Map<string, number>();
  const allArtistNames = new Set<string>();
  
  cards.forEach(card => {
    if (card.artist) {
      // Split multiple artists and count each variation
      const artists = card.artist.split(/\s+(?:&|and)\s+/i).map(a => a.trim());
      artists.forEach(artist => {
        allArtistNames.add(artist);
        // Only count artists that match our target (case-insensitive)
        if (artist.toLowerCase() === initialName.toLowerCase()) {
          artistCounts.set(artist, (artistCounts.get(artist) || 0) + 1);
        }
      });
    }
  });
  
  // Find most common variation of our target artist
  let mostCommonName = initialName;
  let maxCount = 0;
  
  for (const [name, count] of artistCounts.entries()) {
    if (count > maxCount) {
      maxCount = count;
      mostCommonName = name;
    }
  }
  
  // Get all variations that match our target (excluding the most common)
  const alternates = Array.from(artistCounts.keys())
    .filter(name => name !== mostCommonName)
    .sort();
  
  return {
    primaryName: mostCommonName,
    alternateNames: alternates
  };
};

export const ArtistCardsPage: React.FC = () => {
  // Get artist name from route params and decode it
  const { artistName } = useParams<{ artistName: string }>();
  const decodedArtistName = decodeURIComponent(artistName || '').replace(/-/g, ' ');
  const pascalCasedName = toPascalCase(decodedArtistName);

  // URL state configuration for query parameters
  const urlStateConfig = {
    search: { default: '' },
    rarities: { default: [] },
    sets: { default: [] },
    sort: { default: 'release-desc' }
  };

  // Get initial values from URL only once on mount
  const { getInitialValues } = useUrlState({}, urlStateConfig);
  const [initialValues] = useState(() => getInitialValues());

  const { loading: cardsLoading, error: cardsError, data: cardsData } = useQuery<CardsResponse>(GET_CARDS_BY_ARTIST, {
    variables: { artistName: { artistName: decodedArtistName } },
    skip: !artistName
  });
  
  // Get unique rarities and sets from data
  const allRarities = useMemo(() => getUniqueRarities(cardsData?.cardsByArtistName?.data || []), [cardsData]);
  const allSetObjects = useMemo(() => getUniqueSets(cardsData?.cardsByArtistName?.data || []), [cardsData]);
  const allSets = useMemo(() => allSetObjects.map(set => set.value), [allSetObjects]);
  
  // Create a map for set code to display label
  const setLabelMap = useMemo(() => {
    const map = new Map<string, string>();
    allSetObjects.forEach(set => map.set(set.value, set.label));
    return map;
  }, [allSetObjects]);

  // Configure filter state (memoized to prevent recreating on every render)
  const filterConfig = useMemo(() => ({
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
      'set-asc': (a: Card, b: Card) => (a.setName || '').localeCompare(b.setName || '')
    },
    filterFunctions: {
      rarities: commonFilters.multiSelect<Card>('rarity'),
      sets: commonFilters.multiSelect<Card>('setCode'),
      showDigital: (card: Card, show: boolean) => show || !card.digital
    },
    defaultSort: 'release-desc'
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
    cardsData?.cardsByArtistName?.data,
    filterConfig,
    {
      search: initialValues.search || '',
      sort: initialValues.sort || 'release-desc',
      filters: {
        rarities: Array.isArray(initialValues.rarities) ? initialValues.rarities : (initialValues.rarities ? [initialValues.rarities] : []),
        sets: Array.isArray(initialValues.sets) ? initialValues.sets : (initialValues.sets ? [initialValues.sets] : []),
        showDigital: false
      }
    }
  );

  const selectedRarities = filters.rarities || [];
  const selectedSets = filters.sets || [];
  
  const cards = cardsData?.cardsByArtistName?.data || [];
  
  // Get the most common artist name variation and alternates from loaded cards
  const artistNameInfo = useMemo(() => {
    if (cards.length === 0) {
      return { primaryName: pascalCasedName, alternateNames: [] };
    }
    return getArtistNameInfo(cards, decodedArtistName);
  }, [cards, decodedArtistName, pascalCasedName]);
  
  const displayArtistName = artistNameInfo.primaryName;
  const alternateNames = artistNameInfo.alternateNames;

  // Sync non-search filters with URL immediately
  useUrlState(
    {
      rarities: selectedRarities,
      sets: selectedSets,
      sort: sortBy
    },
    {
      rarities: { default: [] },
      sets: { default: [] },
      sort: { default: 'release-desc' }
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

  const handleRarityChange = (value: string[]) => {
    updateFilter('rarities', value);
  };

  const handleSetChange = (value: string[]) => {
    updateFilter('sets', value);
  };

  const handleSortChange = (value: string) => {
    setSortBy(value);
  };

  if (!artistName) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          No artist name provided. Please provide an artist name in the URL.
        </Alert>
      </Container>
    );
  }

  if (cardsData?.cardsByArtistName?.__typename === 'FailureResponse') {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">
          {cardsData.cardsByArtistName.status?.message || 'Failed to load cards'}
        </Alert>
      </Container>
    );
  }

  return (
    <QueryStateContainer
      loading={cardsLoading}
      error={cardsError}
      containerProps={{ maxWidth: false }}
    >
      <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
        {/* Artist Page Title */}
        <SectionErrorBoundary name="ArtistPageHeader">
          <Box sx={{ mb: 4, textAlign: 'center' }}>
            <Typography variant="h3" component="h1" gutterBottom>
              {displayArtistName} Cards
            </Typography>
            {alternateNames.length > 0 && (
              <Typography 
                variant="body2" 
                color="text.secondary" 
                sx={{ mt: 1, fontSize: '0.9rem' }}
              >
                Alternate Names: {alternateNames.join(', ')}
              </Typography>
            )}
          </Box>
        </SectionErrorBoundary>

        {/* Filters and Search - Hide completely if only 1 card */}
        {cards.length > 1 && (
          <FilterErrorBoundary name="ArtistPageFilters">
            <FilterPanel
              config={{
                search: {
                  value: initialValues.search || '',
                  onChange: setSearchTerm,
                  placeholder: `Search ${displayArtistName}'s cards...`,
                  debounceMs: 300,
                  minWidth: 300
                },
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
                  ...(allSets.length > 1 ? [{
                    key: 'sets',
                    value: selectedSets,
                    onChange: handleSetChange,
                    options: allSets,
                    label: 'Set',
                    placeholder: 'All Sets',
                    minWidth: 200,
                    getOptionLabel: (setCode: string) => setLabelMap.get(setCode) || setCode.toUpperCase()
                  }] : [])
                ],
                sort: {
                  value: sortBy,
                  onChange: handleSortChange,
                  options: [
                    { value: 'release-desc', label: 'Release Date (Newest)' },
                    { value: 'release-asc', label: 'Release Date (Oldest)' },
                    { value: 'name-asc', label: 'Name (A-Z)' },
                    { value: 'name-desc', label: 'Name (Z-A)' },
                    { value: 'rarity', label: 'Rarity' },
                    { value: 'price-desc', label: 'Price (High-Low)' },
                    { value: 'price-asc', label: 'Price (Low-High)' },
                    { value: 'set-asc', label: 'Set (A-Z)' }
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
          current={filteredCards.length} 
          total={cards.length} 
          label="cards"
          textAlign="center"
        />

        {/* Card Grid */}
        <CardGridErrorBoundary name="ArtistPageCardGrid">
          <ResponsiveGridAutoFit sx={{ mt: 3 }}>
            {filteredCards.map((card) => (
              <MtgCard
                key={card.id}
                card={card}
                context={{
                  isOnArtistPage: true,
                  currentArtist: displayArtistName,
                  hideSetInfo: false,
                  showCollectorInfo: true
                }}
              />
            ))}
          </ResponsiveGridAutoFit>
        </CardGridErrorBoundary>

        {!cardsLoading && filteredCards.length === 0 && (
          <SearchEmptyState
            itemType="cards"
            onClear={() => {
              setSearchTerm('');
              updateFilter('rarities', []);
              updateFilter('sets', []);
            }}
          />
        )}

        {/* Back to Top Button */}
        <BackToTopFab />
      </Container>
    </QueryStateContainer>
  );
};