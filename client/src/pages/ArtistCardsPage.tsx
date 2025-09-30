import React, { useState, useMemo, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import {
  Container,
  Typography,
  Box,
  Alert
} from '@mui/material';
import { CardGrid } from '../components/organisms/CardGrid';
import { ResultsSummary } from '../components/molecules/shared/ResultsSummary';
import { SearchEmptyState } from '../components/atoms/shared/EmptyState';
import { useUrlState } from '../hooks/useUrlState';
import { useFilterState } from '../hooks/useFilterState';
import { QueryStateContainer } from '../components/molecules/shared/QueryStateContainer';
import { FilterPanel } from '../components/molecules/shared/FilterPanel';
import { RARITY_ORDER, ARTIST_PAGE_SORT_OPTIONS, ARTIST_PAGE_COLLECTOR_SORT_OPTIONS } from '../config/cardSortOptions';
import {
  getUniqueRarities,
  getUniqueSets,
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
import { CollectorFiltersSection } from '../components/molecules/shared/CollectorFiltersSection';
import { useCollectorParam } from '../hooks/useCollectorParam';
import { useCardCache } from '../hooks/useCardCache';
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

  // Check for collector parameter
  const { hasCollector, collectorId: _collectorId } = useCollectorParam();

  // URL state configuration for query parameters
  const urlStateConfig = {
    search: { default: '' },
    rarities: { default: [] },
    sets: { default: [] },
    sort: { default: 'release-desc' },
    // Collector-specific filters
    counts: { default: [] },
    signed: { default: [] }
  };

  // Get initial values from URL only once on mount
  const { getInitialValues } = useUrlState({}, urlStateConfig);
  const [initialValues] = useState(() => getInitialValues());

  // Use card cache for fetching cards by artist
  const { fetchCardsByArtist } = useCardCache();
  const [cardsLoading, setCardsLoading] = useState(true);
  const [cardsError, setCardsError] = useState<Error | null>(null);
  const [cardsData, setCardsData] = useState<CardsResponse | null>(null);

  useEffect(() => {
    if (!artistName) return;

    const loadCards = async () => {
      setCardsLoading(true);
      setCardsError(null);
      try {
        const cards = await fetchCardsByArtist(decodedArtistName);
        setCardsData({
          cardsByArtistName: {
            __typename: 'SuccessCardsByArtistResponse',
            data: cards
          }
        });
      } catch (err) {
        setCardsError(err as Error);
        setCardsData({
          cardsByArtistName: {
            __typename: 'FailureResponse',
            status: {
              message: (err as Error).message
            }
          }
        });
      } finally {
        setCardsLoading(false);
      }
    };

    loadCards();
  }, [artistName, decodedArtistName, fetchCardsByArtist]);
  
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
      'set-asc': (a: Card, b: Card) => (a.setName || '').localeCompare(b.setName || ''),
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
    defaultSort: 'release-desc'
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
    cardsData?.cardsByArtistName?.data,
    filterConfig,
    {
      search: initialValues.search || '',
      sort: initialValues.sort || 'release-desc',
      filters: {
        rarities: Array.isArray(initialValues.rarities) ? initialValues.rarities : (initialValues.rarities ? [initialValues.rarities] : []),
        sets: Array.isArray(initialValues.sets) ? initialValues.sets : (initialValues.sets ? [initialValues.sets] : []),
        showDigital: false,
        // Collector-specific filters
        collectionCounts: Array.isArray(initialValues.counts) ? initialValues.counts : (initialValues.counts ? [initialValues.counts] : []),
        signedCards: Array.isArray(initialValues.signed) ? initialValues.signed : (initialValues.signed ? [initialValues.signed] : [])
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
      sort: sortBy,
      // Collector-specific filters
      counts: filters.collectionCounts || [],
      signed: filters.signedCards || []
    },
    {
      rarities: { default: [] },
      sets: { default: [] },
      sort: { default: 'release-desc' },
      counts: { default: [] },
      signed: { default: [] }
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
                  options: hasCollector ? ARTIST_PAGE_COLLECTOR_SORT_OPTIONS : ARTIST_PAGE_SORT_OPTIONS,
                  minWidth: 180
                }
              }}
              layout="compact"
              sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}
            />

            {/* Collector-specific filters */}
            {hasCollector && (
              <CollectorFiltersSection
                config={{
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
                }}
                title="Collection Filters"
                sx={{ mt: 2 }}
              />
            )}
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
          <CardGrid
            cards={filteredCards}
            groupId="artist-cards"
            context={{
              isOnArtistPage: true,
              currentArtist: displayArtistName,
              hideSetInfo: false,
              showCollectorInfo: true,
              hasCollector
            }}
            isLoading={cardsLoading}
            sx={{ mt: 3 }}
          />
        </CardGridErrorBoundary>

        {!cardsLoading && filteredCards.length === 0 && (
          <SearchEmptyState
            itemType="cards"
            onClear={() => {
              setSearchTerm('');
              updateFilter('rarities', []);
              updateFilter('sets', []);
              if (hasCollector) {
                updateFilter('collectionCounts', []);
                updateFilter('signedCards', []);
              }
            }}
          />
        )}

        {/* Back to Top Button */}
        <BackToTopFab />
      </Container>
    </QueryStateContainer>
  );
};