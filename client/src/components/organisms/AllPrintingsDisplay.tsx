import React, { useState, useEffect } from 'react';
import { Typography } from '../../atoms';
import { ExpandableSection } from '../molecules';
import { LoadingContainer, ErrorAlert } from '../atoms';
import { MtgCard } from './MtgCard';
import { ResponsiveGridAutoFit } from '../molecules/layouts/ResponsiveGrid';
import { handleGraphQLError, globalLoadingManager } from '../../utils/networkErrorHandler';
import { useCollectorParam } from '../../hooks/useCollectorParam';
import { useCardQueries } from '../../hooks/useCardQueries';
import type { Card } from '../../types/card';

interface AllPrintingsDisplayProps {
  cardName: string;
  currentCardId: string;
}

interface CardsSuccessResponse {
  cardsByName: {
    __typename: string;
    data?: Card[];
    status?: {
      message: string;
    };
  };
}

export const AllPrintingsDisplay: React.FC<AllPrintingsDisplayProps> = ({ cardName, currentCardId }) => {
  const [expanded, setExpanded] = useState(false);
  const [userFriendlyError, setUserFriendlyError] = useState<string | null>(null);

  // Check for collector parameter
  const { hasCollector } = useCollectorParam();

  // Use card cache for fetching cards by name
  const { fetchCardsByName } = useCardQueries();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);
  const [data, setData] = useState<CardsSuccessResponse | null>(null);

  useEffect(() => {
    if (!cardName) return;

    const loadCards = async () => {
      setLoading(true);
      setError(null);
      try {
        const cards = await fetchCardsByName(cardName);
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

    loadCards();
  }, [cardName, fetchCardsByName]);

  useEffect(() => {
    const loadingKey = `all-printings-${currentCardId}`;
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

  const allCards = data?.cardsByName?.data || [];
  // Filter out the current card and sort by release date (oldest first)
  const otherCards = allCards
    .filter(card => card.id !== currentCardId)
    .sort((a, b) => {
      const dateA = a.releasedAt ? new Date(a.releasedAt).getTime() : 0;
      const dateB = b.releasedAt ? new Date(b.releasedAt).getTime() : 0;
      return dateA - dateB; // Oldest first
    });


  const hasError = userFriendlyError || data?.cardsByName?.__typename === 'FailureResponse';

  // Don't render if there are no other printings
  if (!loading && !hasError && otherCards.length === 0) {
    return null;
  }

  return (
    <ExpandableSection
      title="Other Printings"
      count={otherCards.length}
      isLoading={loading}
      isError={Boolean(hasError)}
      expanded={expanded}
      onExpandedChange={setExpanded}
    >
      {loading && (
        <LoadingContainer py={4} />
      )}
      
      {userFriendlyError && (
        <ErrorAlert message={userFriendlyError} />
      )}

      {data?.cardsByName?.__typename === 'FailureResponse' && (
        <ErrorAlert message={data.cardsByName.status?.message || 'Failed to load printings'} />
      )}
      
      {!loading && !hasError && otherCards.length > 0 && (
        <ResponsiveGridAutoFit 
          minItemWidth={280} 
          spacing={1.5}
          sx={{ mt: 1 }}
        >
          {otherCards.map((card, index) => (
            <MtgCard
              key={card.id}
              card={card}
              index={index}
              groupId="all-printings"
              context={{
                isOnCardPage: true,
                hasCollector,
                showCollectorInfo: hasCollector
              }}
              collectionData={card.userCollection}
            />
          ))}
        </ResponsiveGridAutoFit>
      )}

      {!loading && !hasError && otherCards.length === 0 && (
        <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'center', py: 2 }}>
          No other printings found
        </Typography>
      )}
    </ExpandableSection>
  );
};