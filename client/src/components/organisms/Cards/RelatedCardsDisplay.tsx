import React, { useState, useEffect, useRef, useMemo } from 'react';
import { logger } from '../../../utils/logger';
import { Typography } from '../../atoms';
import { ExpandableSection } from '../../molecules';
import { LoadingIndicator, ErrorAlert } from '../../molecules/feedback';
import { CardGrid } from './CardGrid';
import { handleGraphQLError, globalLoadingManager } from '../../../utils/networkErrorHandler';
import { useCardQueries } from '../../../hooks/useCardQueries';
import type { Card } from '../../../types/card';

interface RelatedCardsDisplayProps {
  relatedCardIds: string[];
  currentCardId: string;
}

interface CardsSuccessResponse {
  cardsById: {
    __typename: string;
    data?: Card[];
    status?: {
      message: string;
    };
  };
}

export const RelatedCardsDisplay: React.FC<RelatedCardsDisplayProps> = ({
  relatedCardIds,
  currentCardId
}) => {
  const [expanded, setExpanded] = useState(false);
  const [userFriendlyError, setUserFriendlyError] = useState<string | null>(null);

  // Memoize filteredIds to prevent infinite re-renders
  const filteredIds = useMemo(() =>
    relatedCardIds.filter(id => id !== currentCardId),
    [relatedCardIds, currentCardId]
  );

  // Use card cache for fetching cards by IDs
  const { fetchCards } = useCardQueries();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);
  const [data, setData] = useState<CardsSuccessResponse | null>(null);

  // Use ref to store latest fetchCards function to avoid dependency issues
  const fetchCardsRef = useRef(fetchCards);
  fetchCardsRef.current = fetchCards;

  useEffect(() => {
    logger.debug('[RelatedCards] useEffect triggered:', { expanded, filteredIdsLength: filteredIds.length });

    if (!expanded || filteredIds.length === 0) return;

    const loadCards = async () => {
      logger.debug('[RelatedCards] Loading cards:', filteredIds);
      setLoading(true);
      setError(null);
      try {
        const cards = await fetchCardsRef.current(filteredIds);
        logger.debug('[RelatedCards] Cards loaded:', cards.length);
        setData({
          cardsById: {
            __typename: 'CardsSuccessResponse',
            data: cards
          }
        });
      } catch (err) {
        logger.error('[RelatedCards] Error loading cards:', err);
        setError(err as Error);
        setData({
          cardsById: {
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

    loadCards();
  }, [expanded, filteredIds]);

  useEffect(() => {
    const loadingKey = `related-cards-${currentCardId}`;
    globalLoadingManager.setLoading(loadingKey, loading);
    
    return () => {
      globalLoadingManager.setLoading(loadingKey, false);
    };
  }, [loading, currentCardId]);

  useEffect(() => {
    if (error) {
      const networkError = handleGraphQLError(error);
      setUserFriendlyError(networkError.userMessage);
    } else {
      setUserFriendlyError(null);
    }
  }, [error]);

  if (filteredIds.length === 0) return null;

  // Sort cards alphabetically by name (A-Z)
  // Create a new array to avoid mutating the original
  const cards = [...(data?.cardsById?.data || [])].sort((a, b) =>
    (a.name || '').localeCompare(b.name || '')
  );

  const hasError = userFriendlyError || data?.cardsById?.__typename === 'FailureResponse';

  return (
    <ExpandableSection
      title="Related Cards"
      count={filteredIds.length}
      isLoading={loading}
      isError={Boolean(hasError)}
      expanded={expanded}
      onExpandedChange={setExpanded}
    >
      {loading && (
        <LoadingIndicator withContainer={false} />
      )}

      {userFriendlyError && (
        <ErrorAlert message={userFriendlyError} />
      )}

      {data?.cardsById?.__typename === 'FailureResponse' && (
        <ErrorAlert message={data.cardsById.status?.message || 'Failed to load related cards'} />
      )}

      {!loading && !hasError && cards.length > 0 && (
        <CardGrid
          cards={cards}
          groupId="related-cards"
          context={{}}
          spacing={1.5}
          sx={{ mt: 1 }}
        />
      )}

      {!loading && !hasError && cards.length === 0 && (
        <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'center', py: 2 }}>
          No related cards found
        </Typography>
      )}
    </ExpandableSection>
  );
};