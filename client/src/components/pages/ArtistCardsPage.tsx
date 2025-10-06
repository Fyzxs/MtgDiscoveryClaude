import React, { useState, useMemo, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Alert } from '@mui/material';
import { BrowseTemplate } from '../templates/pages/BrowseTemplate';
import { ArtistPageHeader } from '../organisms/ArtistPageHeader';
import { ArtistPageFilters } from '../organisms/ArtistPageFilters';
import { ArtistPageCardDisplay } from '../organisms/ArtistPageCardDisplay';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { useUrlState } from '../../hooks/useUrlState';
import { useFilterState } from '../../hooks/useFilterState';
import { QueryStateContainer } from '../molecules/shared/QueryStateContainer';
import { RARITY_ORDER } from '../../config/cardSortOptions';
import {
  getUniqueRarities,
  getUniqueSets,
  createCardFilterFunctions
} from '../../utils/cardUtils';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { SectionErrorBoundary } from '../ErrorBoundaries';
import { useCollectorParam } from '../../hooks/useCollectorParam';
import { useCardCache } from '../../hooks/useCardCache';
import { toPascalCase, getArtistNameInfo } from '../../utils/artistUtils';
import type { Card } from '../../types/card';

interface CardsResponse {
  cardsByArtistName: {
    __typename: string;
    data?: Card[];
    status?: {
      message: string;
    };
  };
}


export const ArtistCardsPage: React.FC = () => {
  // Get artist name from route params and decode it
  const { artistName } = useParams<{ artistName: string }>();
  const decodedArtistName = decodeURIComponent(artistName || '').replace(/-/g, ' ');
  const pascalCasedName = toPascalCase(decodedArtistName);

  // Check for collector parameter
  const { hasCollector } = useCollectorParam();

  // URL state configuration for query parameters
  const urlStateConfig = {
    search: { default: '' },
    rarities: { default: [] },
    sets: { default: [] },
    sort: { default: 'release-desc' },
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
            __typename: 'CardsByArtistSuccessResponse',
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

  // Configure filter state with sort options
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
        collectionCounts: Array.isArray(initialValues.counts) ? initialValues.counts : (initialValues.counts ? [initialValues.counts] : []),
        signedCards: Array.isArray(initialValues.signed) ? initialValues.signed : (initialValues.signed ? [initialValues.signed] : [])
      }
    }
  );

  const selectedRarities = filters.rarities || [];
  const selectedSets = filters.sets || [];

  // Memoize cards array to prevent unnecessary re-renders
  const cards = useMemo(() => cardsData?.cardsByArtistName?.data || [], [cardsData]);

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
      debounceMs: 0
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
      debounceMs: 300
    }
  );

  const handleClearFilters = () => {
    setSearchTerm('');
    updateFilter('rarities', []);
    updateFilter('sets', []);
    if (hasCollector) {
      updateFilter('collectionCounts', []);
      updateFilter('signedCards', []);
    }
  };

  // Early returns for error states
  if (!artistName) {
    return (
      <Alert severity="error" sx={{ m: 4 }}>
        No artist name provided. Please provide an artist name in the URL.
      </Alert>
    );
  }

  if (cardsData?.cardsByArtistName?.__typename === 'FailureResponse') {
    return (
      <Alert severity="error" sx={{ m: 4 }}>
        {cardsData.cardsByArtistName.status?.message || 'Failed to load cards'}
      </Alert>
    );
  }

  return (
    <QueryStateContainer
      loading={cardsLoading}
      error={cardsError}
      containerProps={{ maxWidth: false }}
    >
      <BrowseTemplate
        header={
          <SectionErrorBoundary name="ArtistPageHeader">
            <ArtistPageHeader
              displayArtistName={displayArtistName}
              alternateNames={alternateNames}
            />
          </SectionErrorBoundary>
        }
        filters={
          <ArtistPageFilters
            totalCards={cards.length}
            displayArtistName={displayArtistName}
            hasCollector={hasCollector}
            filters={{
              search: {
                value: initialValues.search || '',
                onChange: setSearchTerm
              },
              rarities: {
                value: selectedRarities,
                onChange: (value: string[]) => updateFilter('rarities', value),
                options: allRarities,
                shouldShow: allRarities.length > 1
              },
              sets: {
                value: selectedSets,
                onChange: (value: string[]) => updateFilter('sets', value),
                options: allSets,
                shouldShow: allSets.length > 1,
                getOptionLabel: (setCode: string) => setLabelMap.get(setCode) || setCode.toUpperCase()
              },
              sort: {
                value: sortBy,
                onChange: setSortBy
              },
              collectionCounts: hasCollector ? {
                value: filters.collectionCounts || [],
                onChange: (value: string[]) => updateFilter('collectionCounts', value)
              } : undefined,
              signedCards: hasCollector ? {
                value: filters.signedCards || [],
                onChange: (value: string[]) => updateFilter('signedCards', value)
              } : undefined
            }}
          />
        }
        summary={
          <ResultsSummary
            current={filteredCards.length}
            total={cards.length}
            label="cards"
            textAlign="center"
          />
        }
        content={
          <ArtistPageCardDisplay
            filteredCards={filteredCards}
            displayArtistName={displayArtistName}
            hasCollector={hasCollector}
            isLoading={cardsLoading}
            onClearFilters={handleClearFilters}
          />
        }
      />
      <BackToTopFab />
    </QueryStateContainer>
  );
};