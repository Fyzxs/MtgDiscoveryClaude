import React, { useMemo, useCallback } from 'react';
import { Grid } from 'react-window';
import { Box, useTheme } from '@mui/material';
import { MtgCard } from './MtgCard';
import type { Card, CardContext } from '../../types/card';

interface VirtualizedCardGridProps {
  cards: Card[];
  context: CardContext;
  onCardSelection: (cardId: string, selected: boolean) => void;
  selectedCardId: string | null;
  containerWidth?: number;
  containerHeight?: number;
}

const CARD_WIDTH = 280;
const CARD_HEIGHT = 420; // Increased to account for card + margins
const SPACING = 12;

export const VirtualizedCardGrid: React.FC<VirtualizedCardGridProps> = ({
  cards,
  context,
  onCardSelection,
  selectedCardId,
  containerWidth = 1200,
  containerHeight
}) => {
  const theme = useTheme();
  
  // Calculate columns based on container width
  const columnCount = useMemo(() => {
    const availableWidth = containerWidth - SPACING;
    return Math.max(1, Math.floor(availableWidth / (CARD_WIDTH + SPACING)));
  }, [containerWidth]);

  const rowCount = useMemo(() => 
    Math.ceil(cards.length / columnCount), 
    [cards.length, columnCount]
  );

  // Calculate total height needed to show all cards
  const totalHeight = useMemo(() => 
    rowCount * CARD_HEIGHT,
    [rowCount]
  );

  // Use provided containerHeight or calculated totalHeight
  const gridHeight = containerHeight || totalHeight;

  // Memoized cell renderer
  const Cell = useCallback(({ columnIndex, rowIndex, style }: any) => {
    const cardIndex = rowIndex * columnCount + columnIndex;
    const card = cards[cardIndex];

    // Ensure style is defined to prevent spreading undefined
    const safeStyle = style || {};

    if (!card) {
      return <div style={safeStyle} />;
    }

    return (
      <div 
        style={{ 
          ...safeStyle, 
          padding: SPACING / 2,
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'flex-start'
        }}
      >
        <MtgCard
          key={card.id}
          card={card}
          isSelected={selectedCardId === card.id}
          onSelectionChange={onCardSelection}
          context={context}
        />
      </div>
    );
  }, [cards, columnCount, selectedCardId, onCardSelection, context]);

  if (cards.length === 0) return null;

  return (
    <Box sx={{ width: '100%', height: gridHeight }}>
      <Grid
        columnCount={columnCount}
        columnWidth={CARD_WIDTH + SPACING}
        height={gridHeight}
        rowCount={rowCount}
        rowHeight={CARD_HEIGHT}
        width={containerWidth}
        style={{ 
          overflowX: 'hidden',
          backgroundColor: theme.palette.background.default
        }}
        cellComponent={Cell}
        cellProps={{}}
      />
    </Box>
  );
};