import React, { useState, useEffect, useRef, useMemo } from 'react';
import { logger } from '../../utils/logger';
import { Box, Typography, CircularProgress, Alert, Collapse, IconButton, Chip } from '../../atoms';
import { MtgCard } from './MtgCard';
import { ResponsiveGridAutoFit } from '../molecules/layouts/ResponsiveGrid';
import { handleGraphQLError, globalLoadingManager } from '../../utils/networkErrorHandler';
import { useCardQueries } from '../../hooks/useCardQueries';
import type { Card } from '../../types/card';
import { ExpandMoreIcon, ExpandLessIcon } from '../atoms/Icons';

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
  const loadedCount = cards.length;
  const totalCount = filteredIds.length;

  // Determine the badge text
  const getBadgeText = () => {
    if (!expanded) return totalCount.toString();
    if (loading) return `Loading ${totalCount}...`;
    if (userFriendlyError || data?.cardsById?.__typename === 'FailureResponse') return 'Error';
    // Only show loaded/total if they're different
    return loadedCount === totalCount ? loadedCount.toString() : `${loadedCount}/${totalCount}`;
  };

  return (
    <Box>
      {/* Header with expand/collapse */}
      <Box 
        sx={{ 
          display: 'flex', 
          alignItems: 'center', 
          gap: 1,
          cursor: 'pointer',
          p: 1,
          borderRadius: 1,
          '&:hover': {
            bgcolor: 'action.hover'
          }
        }}
        onClick={() => setExpanded(!expanded)}
      >
        <IconButton size="small">
          {expanded ? <ExpandLessIcon /> : <ExpandMoreIcon />}
        </IconButton>
        <Typography variant="body2">
          Click to {expanded ? 'hide' : 'show'} related cards
        </Typography>
        <Chip 
          label={getBadgeText()} 
          size="small"
          sx={{ 
            height: 22,
            fontSize: '0.8rem',
            fontWeight: 'bold',
            bgcolor: userFriendlyError || data?.cardsById?.__typename === 'FailureResponse' ? 'error.main' : loading ? 'action.disabled' : 'primary.main',
            color: userFriendlyError || data?.cardsById?.__typename === 'FailureResponse' ? 'error.contrastText' : loading ? 'text.disabled' : 'primary.contrastText',
            '& .MuiChip-label': {
              px: 1.5
            }
          }}
        />
      </Box>

      {/* Collapsible content */}
      <Collapse in={expanded}>
        <Box sx={{ mt: 2 }}>
          {loading && (
            <Box sx={{ display: 'flex', justifyContent: 'center', p: 2 }}>
              <CircularProgress size={24} />
            </Box>
          )}

          {userFriendlyError && (
            <Alert severity="error" sx={{ my: 1 }}>
              {userFriendlyError}
            </Alert>
          )}

          {data?.cardsById?.__typename === 'FailureResponse' && (
            <Alert severity="error" sx={{ my: 1 }}>
              {data.cardsById.status?.message || 'Failed to load related cards'}
            </Alert>
          )}

          {!loading && !userFriendlyError && data?.cardsById?.__typename !== 'FailureResponse' && cards.length === 0 && (
            <Typography variant="body2" color="text.secondary" align="center">
              No related cards found
            </Typography>
          )}

          {!loading && cards.length > 0 && (
            <ResponsiveGridAutoFit 
              minItemWidth={280} 
              spacing={1.5}
            >
              {cards.map((card, index) => (
                <MtgCard
                  key={card.id}
                  card={card}
                  index={index}
                  groupId="related"
                  context={{
                    isOnCardPage: true
                  }}
                />
              ))}
            </ResponsiveGridAutoFit>
          )}
        </Box>
      </Collapse>
    </Box>
  );
};