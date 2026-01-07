import React from 'react';
import { Box, Typography } from '../../atoms';
import { BinderSlot } from '../../molecules/Binder';
import type { Card } from '../../../types/card';

interface BinderPageGridProps {
  /** Cards for this page (9 items, may include nulls for empty slots) */
  cards: (Card | null)[];
  /** Set of collected card IDs for determining opacity */
  collectedCardIds: Set<string>;
  /** Page number for display */
  pageNumber: number;
  /** Whether to show page number */
  showPageNumber?: boolean;
  /** Whether there is a collector viewing (affects card opacity) */
  hasCollector?: boolean;
}

/**
 * A single binder page displaying a 3x3 grid of card slots.
 * Mimics the layout of a physical binder page.
 */
export const BinderPageGrid: React.FC<BinderPageGridProps> = ({
  cards,
  collectedCardIds,
  pageNumber,
  showPageNumber = true,
  hasCollector = true
}) => {
  // Ensure we always have exactly 9 slots
  const slots = [...cards];
  while (slots.length < 9) {
    slots.push(null);
  }

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        width: '100%',
        maxWidth: { xs: 360, sm: 450, md: 550, lg: 600 }
      }}
    >
      {/* Binder page background */}
      <Box
        sx={{
          bgcolor: 'grey.900',
          borderRadius: 2,
          p: { xs: 1, sm: 1.5, md: 2 },
          border: '2px solid',
          borderColor: 'grey.800',
          boxShadow: 3,
          width: '100%'
        }}
      >
        {/* 3x3 Grid */}
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: 'repeat(3, 1fr)',
            gridTemplateRows: 'repeat(3, 1fr)',
            gap: { xs: 0.5, sm: 0.75, md: 1 },
            aspectRatio: '3 / 4' // Approximate binder page ratio
          }}
        >
          {slots.map((card, index) => (
            <BinderSlot
              key={card?.id ?? `empty-${pageNumber}-${index}`}
              card={card}
              isCollected={card ? collectedCardIds.has(card.id) : false}
              index={index}
              hasCollector={hasCollector}
            />
          ))}
        </Box>
      </Box>

      {/* Page number */}
      {showPageNumber && (
        <Typography
          variant="caption"
          sx={{
            mt: 1,
            color: 'text.secondary',
            fontWeight: 500
          }}
        >
          Page {pageNumber}
        </Typography>
      )}
    </Box>
  );
};
