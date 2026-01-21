import React, { useEffect, useState, useMemo } from 'react';
import { useParams } from 'react-router-dom';
import { useCardCache } from '../../hooks/useCardCache';
import type { Card } from '../../types/card';
import {
  Container,
  Typography,
  Box,
  Button,
  CircularProgress,
  Alert
} from '@mui/material';
import RefreshIcon from '@mui/icons-material/Refresh';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { CardGrid } from '../organisms/CardGrid';
import { useCardFiltering } from '../../hooks/useCardFiltering';
import { CardFilterPanel } from '../organisms/CardFilterPanel';
import { CARD_DETAIL_SORT_OPTIONS, CARD_DETAIL_COLLECTOR_SORT_OPTIONS } from '../../config/cardSortOptions';
import { CollectorFiltersSection } from '../molecules/shared/CollectorFiltersSection';
import {
  getCollectionCountOptions,
  getSignedCardsOptions
} from '../../utils/cardUtils';
import { handleGraphQLError, globalLoadingManager } from '../../utils/networkErrorHandler';
import { AppErrorBoundary } from '../ErrorBoundaries';
import { useCollectorParam } from '../../hooks/useCollectorParam';
import { useCollectorNavigation } from '../../hooks/useCollectorNavigation';

// Stable empty array to prevent infinite re-renders
const EMPTY_CARDS_ARRAY: Card[] = [];

interface CardsSuccessResponse {
  cardsByName: {
    __typename: string;
    data?: Card[];
    status?: {
      message: string;
    };
  };
}

export const CardAllPrintingsPage: React.FC = () => {
  const { cardName } = useParams<{ cardName: string }>();
  const { navigateWithCollector } = useCollectorNavigation();
  const decodedCardName = decodeURIComponent(cardName || '');
  const [userFriendlyError, setUserFriendlyError] = useState<string | null>(null);
  const [retryCount, setRetryCount] = useState(0);

  // Check if we have a collector parameter
  const { hasCollector } = useCollectorParam();

  // Use card cache for fetching cards by name
  const { fetchCardsByName } = useCardCache();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);
  const [data, setData] = useState<CardsSuccessResponse | null>(null);

  const refetch = async () => {
    if (!cardName) return;

    setLoading(true);
    setError(null);
    try {
      const cards = await fetchCardsByName(decodedCardName);
      setData({
        cardsByName: {
          __typename: 'CardsSuccessResponse',
          data: cards
        }
      });
    } catch (err) {
      setError(err as Error);
      setData({
        cardsByName: {
          __typename: 'FailureResponse',
          status: {
            message: (err as Error).message
          }
        }
      });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    refetch();
  }, [cardName, decodedCardName]);

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

  const cards = data?.cardsByName?.data || EMPTY_CARDS_ARRAY;
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
    navigateWithCollector(`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`);
  };

  // Memoize filtering options to prevent infinite re-renders
  const filteringOptions = useMemo(() => ({
    defaultSort: 'release-desc', // Always default to newest first
    includeSets: false,
    includeCollectorFilters: hasCollector
  }), [hasCollector]);

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
    uniqueFormats,
    hasMultipleArtists,
    hasMultipleRarities,
    hasMultipleFormats
  } = useCardFiltering(cards, filteringOptions);

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
        <Typography>No cards found with name "{decodedCardName}"</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
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
        uniqueFormats={uniqueFormats}
        hasMultipleArtists={hasMultipleArtists}
        hasMultipleRarities={hasMultipleRarities}
        hasMultipleFormats={hasMultipleFormats}
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
              value: (filters.collectionCounts as string[]) || [],
              onChange: (value: string[]) => updateFilter('collectionCounts', value),
              options: getCollectionCountOptions(),
              label: 'Collection Count',
              placeholder: 'All Counts',
              minWidth: 180
            },
            signedCards: {
              key: 'signedCards',
              value: (filters.signedCards as string[]) || [],
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