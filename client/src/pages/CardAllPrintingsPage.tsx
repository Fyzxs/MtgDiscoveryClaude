import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useQuery } from '@apollo/client/react';
import {
  Container,
  Typography,
  Box,
  Button,
  CircularProgress,
  Alert
} from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import RefreshIcon from '@mui/icons-material/Refresh';
import { ResultsSummary } from '../components/molecules/shared/ResultsSummary';
import { GET_CARDS_BY_NAME } from '../graphql/queries/cards';
import { CardGrid } from '../components/organisms/CardGrid';
import { useCardFiltering } from '../hooks/useCardFiltering';
import { CardFilterPanel } from '../components/molecules/shared/CardFilterPanel';
import { CARD_DETAIL_SORT_OPTIONS, CARD_DETAIL_COLLECTOR_SORT_OPTIONS } from '../config/cardSortOptions';
import { CollectorFiltersSection } from '../components/molecules/shared/CollectorFiltersSection';
import {
  getCollectionCountOptions,
  getSignedCardsOptions,
  createCardFilterFunctions
} from '../utils/cardUtils';
import { handleGraphQLError, globalLoadingManager } from '../utils/networkErrorHandler';
import { AppErrorBoundary } from '../components/ErrorBoundaries';
import { useCollectorParam } from '../hooks/useCollectorParam';


interface CardData {
  id: string;
  name: string;
  setCode: string;
  setName: string;
  releasedAt: string;
  collectorNumber: string;
  rarity: string;
  artist: string;
  digital?: boolean;
  prices: {
    usd: string | null;
    usdFoil: string | null;
  };
  imageUris?: {
    normal: string;
    large: string;
    small: string;
  };
  cardFaces?: Array<{
    imageUris?: {
      normal: string;
      large: string;
      small: string;
    };
  }>;
  userCollection?: Array<{
    finish: string;
    special: string;
    count: number;
  }> | {
    finish: string;
    special: string;
    count: number;
  };
}

interface CardsResponse {
  cardsByName: {
    __typename: string;
    data?: CardData[];
    status?: {
      message: string;
    };
  };
}

export const CardAllPrintingsPage: React.FC = () => {
  const { cardName } = useParams<{ cardName: string }>();
  const navigate = useNavigate();
  const decodedCardName = decodeURIComponent(cardName || '');
  const [userFriendlyError, setUserFriendlyError] = useState<string | null>(null);
  const [retryCount, setRetryCount] = useState(0);

  // Check if we have a collector parameter
  const { hasCollector, collectorId } = useCollectorParam();

  const { loading, error, data, refetch } = useQuery<CardsResponse>(GET_CARDS_BY_NAME, {
    variables: {
      cardName: {
        cardName: decodedCardName,
        userId: collectorId || undefined
      }
    },
    skip: !cardName,
    errorPolicy: 'all'
  });

  useEffect(() => {
    const loadingKey = `card-detail-${decodedCardName}`;
    globalLoadingManager.setLoading(loadingKey, loading);
    
    return () => {
      globalLoadingManager.setLoading(loadingKey, false);
    };
  }, [loading, decodedCardName]);

  useEffect(() => {
    if (error) {
      try {
        const networkError = handleGraphQLError(error);
        setUserFriendlyError(networkError.userMessage);
      } catch {
        setUserFriendlyError('Failed to load card details. Please try again.');
      }
    } else {
      setUserFriendlyError(null);
    }
  }, [error]);

  const cards = data?.cardsByName?.data || [];
  const hasError = userFriendlyError || data?.cardsByName?.__typename === 'FailureResponse';
  const graphQLError = data?.cardsByName?.status?.message;



  // handleBackClick removed - using href directly on buttons

  const handleRetry = async () => {
    setRetryCount(prev => prev + 1);
    setUserFriendlyError(null);
    try {
      await refetch();
    } catch (retryError) {
      console.error('Retry failed:', retryError);
    }
  };

  const handleArtistClick = (artistName: string) => {
    navigate(`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`);
  };

  // Use the shared card filtering hook (no search or sets filter for card detail page)
  const {
    filteredCards,
    totalCards,
    sortBy,
    filters,
    setSortBy,
    updateFilter,
    uniqueArtists,
    uniqueRarities,
    hasMultipleArtists,
    hasMultipleRarities
  } = useCardFiltering(cards, {
    defaultSort: hasCollector ? 'collection-desc' : 'release-desc',
    includeSets: false,
    includeCollectorFilters: hasCollector
  });

  if (loading) {
    return (
      <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
        <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
          <CircularProgress />
        </Box>
      </Container>
    );
  }

  if (hasError) {
    return (
      <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
        <Button
          startIcon={<ArrowBackIcon />}
          component="a"
          href="/search/cards"
          sx={{ mb: 2, textDecoration: 'none' }}
        >
          Back to Search
        </Button>
        
        <Alert 
          severity="error" 
          sx={{ mb: 2 }}
          action={
            <Box sx={{ display: 'flex', gap: 1 }}>
              <Button 
                color="inherit" 
                size="small" 
                startIcon={<RefreshIcon />}
                onClick={handleRetry}
                disabled={loading}
              >
                {loading ? 'Retrying...' : 'Retry'}
              </Button>
            </Box>
          }
        >
          <Typography variant="body1">
            {userFriendlyError || graphQLError || 'Failed to load card details'}
          </Typography>
          {retryCount > 0 && (
            <Typography variant="caption" display="block" sx={{ mt: 1 }}>
              Retry attempts: {retryCount}
            </Typography>
          )}
        </Alert>
      </Container>
    );
  }

  if (cards.length === 0) {
    return (
      <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
        <Button
          startIcon={<ArrowBackIcon />}
          component="a"
          href="/search/cards"
          sx={{ mb: 2, textDecoration: 'none' }}
        >
          Back to Search
        </Button>
        <Typography>No cards found with name "{decodedCardName}"</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
      <Button
        startIcon={<ArrowBackIcon />}
        component="a"
        href="/search/cards"
        sx={{ mb: 3, textDecoration: 'none' }}
      >
        Back to Search
      </Button>

      {/* Centered card name */}
      <Box sx={{ textAlign: 'center', mb: 4 }}>
        <Typography variant="h2" fontWeight="bold">
          {decodedCardName}
        </Typography>
      </Box>

      {/* Card Filter Panel - Centered controls */}
      <CardFilterPanel
        sortBy={sortBy}
        filters={filters}
        onSortChange={setSortBy}
        onFilterChange={updateFilter}
        uniqueArtists={uniqueArtists}
        uniqueRarities={uniqueRarities}
        hasMultipleArtists={hasMultipleArtists}
        hasMultipleRarities={hasMultipleRarities}
        filteredCount={filteredCards.length}
        totalCount={totalCards}
        showSearch={false}
        sortOptions={hasCollector ? CARD_DETAIL_COLLECTOR_SORT_OPTIONS : CARD_DETAIL_SORT_OPTIONS}
        centerControls={true}
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
          sx={{ mt: 2, mb: 3, mx: 'auto', maxWidth: '800px' }}
        />
      )}

      {/* Results Summary */}
      <ResultsSummary 
        current={filteredCards.length}
        total={totalCards}
        label="printings"
        textAlign="center"
      />

      {/* Cards Grid */}
      <AppErrorBoundary variant="card-grid" name="CardDetailGrid">
        <CardGrid
          cards={filteredCards}
          groupId="all-printings"
          context={{
            isOnCardPage: true,
            hasCollector,
            showCollectorInfo: hasCollector
          }}
          isLoading={false}
          onArtistClick={handleArtistClick}
        />
      </AppErrorBoundary>
    </Container>
  );
};