import React from 'react';
import { Box } from '@mui/material';
import { ResponsiveGridAutoFit } from '../atoms/layouts/ResponsiveGrid';
import { useGridNavigation } from '../../hooks/useGridNavigation';
import { MtgCard } from './MtgCard';
import type { Card, CardContext, UserCardData } from '../../types/card';

interface CardGridProps {
  cards: Card[];
  groupId: string;
  context: CardContext;
  collectionLookup?: Map<string, UserCardData>;
  spacing?: number;
  minItemWidth?: number;
  sx?: any;
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
  collectionLookup,
  spacing = 1.5,
  minItemWidth = 280,
  sx = {},
  enableNavigation = true,
  isLoading = false,
  onArtistClick,
  onSetClick
}) => {
  // Grid navigation hook
  const { handleKeyDown } = useGridNavigation({
    totalItems: cards.length,
    groupId,
    enabled: enableNavigation && !isLoading && cards.length > 0
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
        {cards.map((card, index) => (
          <MtgCard
            key={card.id}
            card={card}
            index={index}
            groupId={groupId}
            context={context}
            collectionData={collectionLookup?.get(card.id)}
            onArtistClick={onArtistClick}
            onSetClick={onSetClick}
          />
        ))}
      </ResponsiveGridAutoFit>
    </Box>
  );
};