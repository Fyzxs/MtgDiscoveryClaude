import React, { useState, useEffect, useRef, useMemo } from 'react';
import { Box, type SxProps, type Theme } from '../../atoms';
import { ResponsiveGridAutoFit } from '../../molecules/layouts/ResponsiveGrid';
import { useGridNavigation } from '../../../hooks/useGridNavigation';
import { MtgCard } from './MtgCard';
import type { Card, CardContext } from '../../../types/card';
import type { Breakpoint } from '@mui/material/styles';

type ResponsiveValue<T> = T | Partial<Record<Breakpoint, T>>;

// Default responsive card widths for mobile-first design
const DEFAULT_MIN_ITEM_WIDTH: Partial<Record<Breakpoint, number>> = {
  xs: 120,  // 2-3 cards on mobile
  sm: 140,  // 3-4 cards on tablet
  md: 180,  // auto-fill on medium
  lg: 220,  // auto-fill on large
  xl: 250,  // auto-fill on extra large
};

interface CardGridProps {
  cards: Card[];
  groupId: string;
  context: CardContext;
  spacing?: ResponsiveValue<number>;
  minItemWidth?: ResponsiveValue<number>;
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
  spacing = { xs: 1, sm: 1.5 },
  minItemWidth = DEFAULT_MIN_ITEM_WIDTH,
  sx = {},
  enableNavigation = true,
  isLoading = false,
  onArtistClick,
  onSetClick
}) => {
  // Create a stable fingerprint of the card LIST (just IDs, not data)
  // This only changes when the actual cards change, not when collection data updates
  const cardListFingerprint = useMemo(
    () => cards.map(c => c.id).join(','),
    // eslint-disable-next-line react-hooks/exhaustive-deps -- only care about ID changes
    [cards.length, cards[0]?.id, cards[cards.length - 1]?.id]
  );

  // Progressive rendering: render cards in batches for better performance
  const [visibleCount, setVisibleCount] = useState(0);
  const prevFingerprintRef = useRef<string | null>(null);

  useEffect(() => {
    // Only run progressive loading when the card LIST changes (not just data)
    // null means first run, empty string means no cards yet
    if (prevFingerprintRef.current !== null && cardListFingerprint === prevFingerprintRef.current) {
      return;
    }
    prevFingerprintRef.current = cardListFingerprint;

    // Render in batches to keep UI responsive
    const batchSize = 50;
    const cardsLength = cards.length;

    if (cardsLength === 0) {
      setVisibleCount(0);
      return;
    }

    // First batch immediately
    setVisibleCount(Math.min(batchSize, cardsLength));

    // Remaining batches progressively
    if (cardsLength > batchSize) {
      const timeoutIds: NodeJS.Timeout[] = [];

      for (let i = batchSize; i < cardsLength; i += batchSize) {
        const timeout = setTimeout(() => {
          setVisibleCount(Math.min(i + batchSize, cardsLength));
        }, ((i / batchSize) - 1) * 50); // 50ms between batches
        timeoutIds.push(timeout);
      }

      return () => timeoutIds.forEach(clearTimeout);
    }
  }, [cardListFingerprint, cards.length]);

  // Use cards directly (not deferred) so collection updates show immediately
  // Progressive loading is controlled by visibleCount
  const cardsToRender = cards.slice(0, visibleCount);

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
  if (cards.length === 0) {
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
            onArtistClick={onArtistClick}
            onSetClick={onSetClick}
          />
        ))}
      </ResponsiveGridAutoFit>
    </Box>
  );
};