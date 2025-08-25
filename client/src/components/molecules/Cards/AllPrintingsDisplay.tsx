import React, { useState, useEffect } from 'react';
import { useQuery } from '@apollo/client/react';
import { Box, Typography, IconButton, CircularProgress, Grid } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
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
  // Filter out the current card
  const otherCards = allCards.filter(card => card.id !== currentCardId);
  const hasError = error || data?.cardsByName?.__typename === 'FailureResponse';

  const getBadgeText = () => {
    if (loading) return '[Loading...]';
    if (hasError) return '[Error]';
    return `[${otherCards.length}]`;
  };

  // Don't render if there are no other printings
  if (!loading && !hasError && otherCards.length === 0) {
    return null;
  }

  return (
    <Box>
      <Box sx={{ 
        display: 'flex', 
        alignItems: 'center', 
        gap: 1,
        cursor: 'pointer',
        '&:hover': {
          '& .expand-icon': {
            color: 'primary.main'
          }
        }
      }}
      onClick={() => setExpanded(!expanded)}
      >
        <Typography variant="subtitle1" fontWeight="bold">
          Other Printings
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {getBadgeText()}
        </Typography>
        <IconButton
          size="small"
          className="expand-icon"
          sx={{ ml: 'auto' }}
        >
          {expanded ? <ExpandLessIcon /> : <ExpandMoreIcon />}
        </IconButton>
      </Box>

      {expanded && (
        <Box sx={{ mt: 2 }}>
          {loading && (
            <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
              <CircularProgress />
            </Box>
          )}
          
          {hasError && (
            <Typography variant="body2" color="error" sx={{ textAlign: 'center', py: 2 }}>
              Failed to load printings
            </Typography>
          )}
          
          {!loading && !hasError && otherCards.length > 0 && (
            <Grid container spacing={2}>
              {otherCards.map((card) => (
                <Grid item key={card.id} xs={12} sm={6} md={4}>
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
        </Box>
      )}
    </Box>
  );
};