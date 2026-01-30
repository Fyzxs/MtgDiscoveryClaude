import React from 'react';
import { Box } from '../../atoms';
import { BinderSlot } from '../../molecules/Binder';
import type { Card } from '../../../types/card';

interface BinderPageGridProps {
  /** Cards for this page (9 items, may include nulls for empty slots) */
  cards: (Card | null)[];
  /** Set of collected card IDs for determining opacity */
  collectedCardIds: Set<string>;
  /** Page number (used for keys) */
  pageNumber: number;
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
  hasCollector = true
}) => {
  // Ensure we always have exactly 9 slots
  const slots = [...cards];
  while (slots.length < 9) {
    slots.push(null);
  }

  // Grid aspect ratio: 3 cards wide x 3 cards tall (each card 745:1040) + gaps
  const gridAspectRatio = (3 * 745 + 16) / (3 * 1040 + 16); // ~0.718

  return (
    <Box
      data-component="binder-page-grid"
      sx={{
        bgcolor: 'grey.900',
        borderRadius: 2,
        p: { xs: 1, sm: 1.5, md: 2 },
        border: '2px solid',
        borderColor: 'grey.800',
        boxShadow: 3,
        // Width fills parent, height calculated from aspect ratio, capped at viewport
        width: '100%',
        maxWidth: { xs: '100%', sm: '600px', md: '700px', lg: '800px' },
        aspectRatio: `${gridAspectRatio}`,
        maxHeight: 'calc(100vh - 500px)',
        overflow: 'hidden'
      }}
    >
      {/* 3x3 Grid - fills page */}
      <Box
        data-component="binder-3x3-grid"
        sx={{
          display: 'grid',
          gridTemplateColumns: 'repeat(3, 1fr)',
          gridTemplateRows: 'repeat(3, 1fr)',
          gap: 1, // fixed 8px gap
          height: '100%',
          width: '100%'
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
  );
};
