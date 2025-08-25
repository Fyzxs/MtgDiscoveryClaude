import React from 'react';
import { Box, Typography, Divider } from '@mui/material';
import { MtgCard } from './MtgCard';
import type { Card, CardContext } from '../../types/card';

interface CardGroupProps {
  groupId: string;
  groupName: string;
  cards: Card[];
  isVisible: boolean;
  showHeader: boolean;
  context: CardContext;
  onCardSelection: (cardId: string, selected: boolean) => void;
  selectedCardId: string | null;
}

export const CardGroup: React.FC<CardGroupProps> = ({
  groupId,
  groupName,
  cards,
  isVisible,
  showHeader,
  context,
  onCardSelection,
  selectedCardId
}) => {
  if (!isVisible || cards.length === 0) {
    return null;
  }

  return (
    <Box 
      data-card-group={groupId}
      sx={{ mb: showHeader ? 6 : 0 }}
    >
      {showHeader && (
        <Box sx={{ mb: 2, textAlign: 'center' }}>
          <Typography 
            variant="overline" 
            sx={{ 
              color: 'text.secondary',
              letterSpacing: 2,
              fontSize: '0.875rem'
            }}
          >
            {groupName} • {cards.length} CARDS
          </Typography>
          <Divider sx={{ mt: 1, mb: 3 }} />
        </Box>
      )}
      
      <Box sx={{ 
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fill, 280px)',
        gap: 3,
        justifyContent: 'center'
      }}>
        {cards.map((card) => (
          <MtgCard
            key={card.id}
            card={card}
            isSelected={selectedCardId === card.id}
            onSelectionChange={onCardSelection}
            context={context}
          />
        ))}
      </Box>
    </Box>
  );
};