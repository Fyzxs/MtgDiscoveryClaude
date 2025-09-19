import React from 'react';
import { Box } from '@mui/material';
import { ResponsiveGridAutoFit } from '../atoms/layouts/ResponsiveGrid';
import { useGridNavigation } from '../../hooks/useGridNavigation';
import { MtgCard } from './MtgCard';
import type { Card, CardContext, UserCardData } from '../../types/card';

interface CardGridContainerProps {
  cards: Card[];
  groupId: string;
  context: CardContext;
  collectionLookup?: Map<string, UserCardData>;
  spacing?: number;
  minItemWidth?: number;
  sx?: any;
  enableNavigation?: boolean;
}

/**
 * Standardized container for card grids that ensures consistent DOM structure
 * and keyboard navigation support across all card display pages.
 *
 * DOM Structure:
 * <Box data-card-group={groupId}>
 *   <ResponsiveGridAutoFit data-grid-container="true">
 *     <MtgCard />...
 *   </ResponsiveGridAutoFit>
 * </Box>
 */
export const CardGridContainer: React.FC<CardGridContainerProps> = ({
  cards,
  groupId,
  context,
  collectionLookup,
  spacing = 1.5,
  minItemWidth = 280,
  sx = {},
  enableNavigation = true
}) => {
  // Grid navigation hook
  const { handleKeyDown } = useGridNavigation({
    totalItems: cards.length,
    groupId,
    enabled: enableNavigation && cards.length > 0
  });

  return (
    <Box data-card-group={groupId}>
      <ResponsiveGridAutoFit
        minItemWidth={minItemWidth}
        spacing={spacing}
        sx={sx}
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
          />
        ))}
      </ResponsiveGridAutoFit>
    </Box>
  );
};