import React, { useState } from 'react';
import { useQuery } from '@apollo/client/react';
import { Typography, Grid } from '@mui/material';
import { ExpandableSection } from '../../molecules/shared/ExpandableSection';
import { LoadingContainer } from '../../atoms/shared/LoadingContainer';
import { ErrorText } from '../../atoms/shared/ErrorAlert';
import { MtgCard } from '../../organisms/MtgCard';
import { GET_CARDS_BY_NAME } from '../../../graphql/queries/cards';
import type { Card } from '../../../types/card';

interface AllPrintingsDisplayProps {
  cardName: string;
  currentCardId: string;
}

interface CardsResponse {
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

  const { loading, error, data } = useQuery<CardsResponse>(GET_CARDS_BY_NAME, {
    variables: { 
      cardName: { 
        cardName: cardName 
      } 
    },
    skip: !cardName
  });

  const allCards = data?.cardsByName?.data || [];
  // Filter out the current card and sort by release date (oldest first)
  const otherCards = allCards
    .filter(card => card.id !== currentCardId)
    .sort((a, b) => {
      const dateA = a.releasedAt ? new Date(a.releasedAt).getTime() : 0;
      const dateB = b.releasedAt ? new Date(b.releasedAt).getTime() : 0;
      return dateA - dateB; // Oldest first
    });
  const hasError = error || data?.cardsByName?.__typename === 'FailureResponse';

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
      
      {hasError && (
        <ErrorText message="Failed to load printings" />
      )}
      
      {!loading && !hasError && otherCards.length > 0 && (
        <Grid container spacing={2} sx={{ mt: 1 }}>
          {otherCards.map((card) => (
            <Grid key={card.id} size={{ xs: 12, sm: 6, md: 4 }}>
              <MtgCard 
                card={card}
                context={{
                  isOnCardPage: true
                }}
              />
            </Grid>
          ))}
        </Grid>
      )}

      {!loading && !hasError && otherCards.length === 0 && (
        <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'center', py: 2 }}>
          No other printings found
        </Typography>
      )}
    </ExpandableSection>
  );
};