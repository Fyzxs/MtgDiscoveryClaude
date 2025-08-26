import React from 'react';
import { MtgCard } from './MtgCard';
import type { Card, CardContext } from '../../types/card';
import { CardGridSection } from '../molecules/shared/CardGrid';

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

export const CardGroup: React.FC<CardGroupProps> = React.memo(({
  groupId,
  groupName,
  cards,
  isVisible,
  showHeader,
  context,
  onCardSelection,
  selectedCardId
}) => {
  return (
    <CardGridSection
      title={groupName}
      items={cards}
      renderItem={(card) => (
        <MtgCard
          key={card.id}
          card={card}
          isSelected={selectedCardId === card.id}
          onSelectionChange={onCardSelection}
          context={context}
        />
      )}
      showHeader={showHeader}
      showCount={true}
      isVisible={isVisible}
      sx={{
        '[data-card-group]': groupId
      }}
    />
  );
});

CardGroup.displayName = 'CardGroup';