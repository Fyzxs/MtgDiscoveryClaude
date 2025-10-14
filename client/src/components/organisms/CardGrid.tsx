import React, { useDeferredValue, useState, useEffect } from 'react';
import { Box, type SxProps, type Theme } from '@mui/material';
import { ResponsiveGridAutoFit } from '../molecules/layouts/ResponsiveGrid';
import { useGridNavigation } from '../../hooks/useGridNavigation';
import { MtgCard } from './MtgCard';
import type { Card, CardContext } from '../../types/card';

interface CardGridProps {
  cards: Card[];
  groupId: string;
  context: CardContext;
  spacing?: number;
  minItemWidth?: number;
  sx?: SxProps<Theme>;
  enableNavigation?: boolean;
  isLoading?: boolean;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  onSetClick?: (setCode?: string) => void;
}

/**
 * Standardized card grid component that ensures consistent:
 * - DOM structure for navigation
 * - Keyboard navigation support
 * - Visual layout and spacing
 * - Loading states
 *
 * This should be used by ALL pages that display card grids to ensure
 * consistency across the application.
 *
 * DOM Structure (guaranteed):
 * <Box data-card-group={groupId}>
 *   <ResponsiveGridAutoFit data-grid-container="true">
 *     <MtgCard data-mtg-card="true" />...
 *   </ResponsiveGridAutoFit>
 * </Box>
 */
export const CardGrid: React.FC<CardGridProps> = ({
  cards,
  groupId,
  context,
  spacing = 1.5,
  minItemWidth = 280,
  sx = {},
  enableNavigation = true,
  isLoading = false,
  onArtistClick,
  onSetClick
}) => {
  // Defer card rendering to keep UI responsive during filter changes
  // When showing MORE cards (filter removal), React can render them in background
  const deferredCards = useDeferredValue(cards);

  // Progressive rendering: render cards in batches for better performance
  const [visibleCount, setVisibleCount] = useState(0);

  useEffect(() => {
    // Reset when cards change
    setVisibleCount(0);

    // Render in batches to keep UI responsive
    const batchSize = 50;
    const totalCards = deferredCards.length;

    if (totalCards === 0) return;

    // First batch immediately
    setVisibleCount(Math.min(batchSize, totalCards));

    // Remaining batches progressively
    if (totalCards > batchSize) {
      const timeoutIds: NodeJS.Timeout[] = [];

      for (let i = batchSize; i < totalCards; i += batchSize) {
        const timeout = setTimeout(() => {
          setVisibleCount(Math.min(i + batchSize, totalCards));
        }, ((i / batchSize) - 1) * 50); // 50ms between batches
        timeoutIds.push(timeout);
      }

      return () => timeoutIds.forEach(clearTimeout);
    }
  }, [deferredCards]);

  // Use visible cards for rendering
  const cardsToRender = deferredCards.slice(0, visibleCount);

  // Grid navigation hook
  const { handleKeyDown } = useGridNavigation({
    totalItems: cardsToRender.length,
    groupId,
    enabled: enableNavigation && !isLoading && cardsToRender.length > 0
  });

  // Loading state with skeletons
  if (isLoading) {
    return (
      <Box data-card-group={groupId}>
        <ResponsiveGridAutoFit
          minItemWidth={minItemWidth}
          spacing={spacing}
          sx={sx}
          data-grid-container="true"
        >
          {/* Show 8 skeleton cards as placeholders */}
          {Array.from({ length: 8 }).map((_, index) => (
            <Box
              key={`skeleton-${index}`}
              sx={{
                width: 280,
                height: 390,
                bgcolor: 'grey.800',
                borderRadius: '12px',
                animation: 'pulse 1.5s ease-in-out infinite'
              }}
            />
          ))}
        </ResponsiveGridAutoFit>
      </Box>
    );
  }

  // Empty state
  if (deferredCards.length === 0) {
    return null;
  }

  // Main card grid
  return (
    <Box data-card-group={groupId}>
      <ResponsiveGridAutoFit
        minItemWidth={minItemWidth}
        spacing={spacing}
        sx={{
          margin: '0 auto',
          transition: 'grid-template-columns 0.15s ease-out',
          ...sx
        }}
        data-grid-container="true"
        onKeyDown={enableNavigation ? handleKeyDown : undefined}
        tabIndex={enableNavigation ? 0 : -1}
      >
        {cardsToRender.map((card, index) => (
          <MtgCard
            key={card.id}
            card={card}
            index={index}
            groupId={groupId}
            context={context}
            collectionData={card.userCollection}
            onArtistClick={onArtistClick}
            onSetClick={onSetClick}
          />
        ))}
      </ResponsiveGridAutoFit>
    </Box>
  );
};