import React from 'react';
import { Box, Typography, Divider, Skeleton } from '@mui/material';
import { MtgCard } from './MtgCard';
import { ResponsiveGridAutoFit } from '../atoms/layouts/ResponsiveGrid';
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
  onCardSelection: (cardId: string, selected: boolean) => void;
  selectedCardId: string | null;
  isLoading?: boolean;
}

export const CardGroup: React.FC<CardGroupProps> = ({
  groupId,
  groupName,
  cards,
  totalCards,
  isVisible,
  showHeader,
  context,
  onCardSelection,
  selectedCardId,
  isLoading = false
}) => {
  if (!isVisible) {
    return null;
  }

  // Show loading skeletons if loading
  if (isLoading) {
    return (
      <Box 
        data-card-group={groupId}
        sx={{ mb: showHeader ? 6 : 0 }}
      >
        {showHeader && (
          <Box sx={{ mb: 2, textAlign: 'center' }}>
            <Skeleton variant="text" width={200} height={30} sx={{ mx: 'auto' }} />
            <Divider sx={{ mt: 1, mb: 3 }} />
          </Box>
        )}
        
        <ResponsiveGridAutoFit 
          minItemWidth={280} 
          spacing={1.5}
          sx={{ margin: '0 auto' }}
        >
          {/* Show 8 skeleton cards as placeholders */}
          {Array.from({ length: 8 }).map((_, index) => (
            <Skeleton
              key={`skeleton-${index}`}
              variant="rounded"
              width={280}
              height={390}
              sx={{
                bgcolor: 'grey.800',
                borderRadius: '12px'
              }}
            />
          ))}
        </ResponsiveGridAutoFit>
      </Box>
    );
  }

  // Don't hide empty groups - show them for debugging
  // if (cards.length === 0) {
  //   return null;
  // }

  return (
    <Box 
      data-card-group={groupId}
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
        <ResponsiveGridAutoFit 
          minItemWidth={280} 
          spacing={1.5}
          sx={{ margin: '0 auto' }}
        >
          {cards.map((card) => (
            <MtgCard
              key={card.id}
              card={card}
              isSelected={selectedCardId === card.id}
              onSelectionChange={onCardSelection}
              context={context}
            />
          ))}
        </ResponsiveGridAutoFit>
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