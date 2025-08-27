import React, { useState } from 'react';
import { useQuery } from '@apollo/client/react';
import { Box, Typography, CircularProgress, Alert, Collapse, IconButton, Chip } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import { GET_CARDS_BY_IDS } from '../../../graphql/queries/cards';
import { MtgCard } from '../../organisms/MtgCard';
import { ResponsiveGridAutoFit } from '../../atoms/layouts/ResponsiveGrid';
import type { Card } from '../../../types/card';

interface RelatedCardsDisplayProps {
  relatedCardIds: string[];
  currentCardId: string;
}

interface CardsResponse {
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
  const filteredIds = relatedCardIds.filter(id => id !== currentCardId);
  
  const { loading, error, data } = useQuery<CardsResponse>(GET_CARDS_BY_IDS, {
    variables: { ids: { cardIds: filteredIds } },
    skip: filteredIds.length === 0 || !expanded
  });

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
    if (!expanded) return `[${totalCount}]`;
    if (loading) return `[Loading ${totalCount}...]`;
    if (error || data?.cardsById?.__typename === 'FailureResponse') return `[Error]`;
    return `[${loadedCount}/${totalCount}]`;
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
          color={error || data?.cardsById?.__typename === 'FailureResponse' ? 'error' : 'default'}
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

          {error && (
            <Alert severity="error" sx={{ my: 1 }}>
              Failed to load related cards
            </Alert>
          )}

          {data?.cardsById?.__typename === 'FailureResponse' && (
            <Alert severity="error" sx={{ my: 1 }}>
              {data.cardsById.status?.message || 'Failed to load related cards'}
            </Alert>
          )}

          {!loading && !error && data?.cardsById?.__typename !== 'FailureResponse' && cards.length === 0 && (
            <Typography variant="body2" color="text.secondary" align="center">
              No related cards found
            </Typography>
          )}

          {!loading && cards.length > 0 && (
            <ResponsiveGridAutoFit 
              minItemWidth={280} 
              spacing={1.5}
            >
              {cards.map((card) => (
                <MtgCard 
                  key={card.id}
                  card={card}
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