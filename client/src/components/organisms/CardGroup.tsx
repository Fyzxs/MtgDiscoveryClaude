import React from 'react';
import { Box, Typography, Divider } from '@mui/material';
import { CardGrid } from './CardGrid';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import type { Card, CardContext } from '../../types/card';

interface CardGroupProps {
  groupId: string;
  groupName: string;
  cards: Card[];
  totalCards?: number; // Total cards in this group before filtering
  isVisible: boolean;
  showHeader: boolean;
  context: CardContext;
  isLoading?: boolean;
}

const CardGroupComponent: React.FC<CardGroupProps> = ({
  groupId,
  groupName,
  cards,
  totalCards,
  isVisible,
  showHeader,
  context,
  isLoading = false
}) => {
  if (!isVisible) {
    return null;
  }


  // Don't hide empty groups - show them for debugging
  // if (cards.length === 0) {
  //   return null;
  // }

  return (
    <Box
      sx={{ mb: showHeader ? 6 : 0 }}
    >
      {showHeader && (
        <Box sx={{ mb: 2, textAlign: 'center' }}>
          <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: 1 }}>
            <Typography 
              variant="overline" 
              sx={{ 
                color: 'text.secondary',
                letterSpacing: 2,
                fontSize: '0.875rem'
              }}
            >
              {groupName}
            </Typography>
            <Typography 
              variant="overline" 
              sx={{ 
                color: 'text.secondary',
                letterSpacing: 2,
                fontSize: '0.875rem'
              }}
            >
              •
            </Typography>
            <ResultsSummary 
              current={cards.length}
              total={totalCards || cards.length}
              label="CARDS"
              sx={{ mb: 0 }}
              customFormat={(current, total, label) => 
                current === total 
                  ? `${total} ${label}`
                  : `${current} OF ${total} ${label}`
              }
              variant="caption"
              color="text.secondary"
            />
          </Box>
          <Divider sx={{ mt: 1, mb: 3 }} />
        </Box>
      )}
      
      {cards.length > 0 ? (
        <CardGrid
          cards={cards}
          groupId={groupId}
          context={context}
          isLoading={isLoading}
          spacing={1.5}
          minItemWidth={280}
        />
      ) : (
        <Box sx={{
          textAlign: 'center',
          py: 4,
          color: 'text.secondary',
          fontStyle: 'italic'
        }}>
          No cards match the current filters
        </Box>
      )}
    </Box>
  );
};

// Memoize CardGroup to prevent unnecessary re-renders when group selection changes
export const CardGroup = React.memo(CardGroupComponent, (prevProps, nextProps) => {
  // If both are invisible, don't re-render
  if (!prevProps.isVisible && !nextProps.isVisible) {
    return true; // Props are "equal" - skip re-render
  }

  // If visibility changed, re-render (this is expected)
  if (prevProps.isVisible !== nextProps.isVisible) {
    return false; // Props are "different" - re-render
  }

  // If both visible, check other important props
  if (prevProps.isVisible && nextProps.isVisible) {
    // Use array reference equality - if the array reference changed, re-render
    // This allows parent components to trigger re-renders by creating new arrays
    // when individual card data changes (like userCollection updates)
    if (prevProps.cards !== nextProps.cards) {
      return false; // Array reference changed - re-render
    }

    return (
      prevProps.groupId === nextProps.groupId &&
      prevProps.groupName === nextProps.groupName &&
      prevProps.totalCards === nextProps.totalCards &&
      prevProps.showHeader === nextProps.showHeader &&
      prevProps.isLoading === nextProps.isLoading
    );
  }

  return false; // Default to re-render if unsure
});

