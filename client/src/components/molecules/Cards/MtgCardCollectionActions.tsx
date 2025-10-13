import { useCallback, useMemo } from 'react';
import type { RefObject } from 'react';
import { useCollection } from '../../../contexts/CollectionContext';
import { useCardCollectionEntry } from '../../../hooks/useCardCollectionEntry3';
import type { Card } from '../../../types/card';
import type { CardFinish } from '../../../types/collection';

interface CollectionUpdate {
  cardId: string;
  count: number;
  finish: CardFinish;
  special: string;
}

interface MtgCardCollectionActionsProps {
  card: Card;
  isSelected: boolean;
  cardRef: RefObject<HTMLDivElement | null>;
}

export const useMtgCardCollectionActions = ({
  card,
  isSelected,
  cardRef
}: MtgCardCollectionActionsProps) => {
  const { submitCollectionUpdate } = useCollection();

  // Determine available finishes
  const availableFinishes = useMemo<CardFinish[]>(() => {
    const finishes: CardFinish[] = [];
    if (card.nonFoil) finishes.push('non-foil');
    if (card.foil) finishes.push('foil');
    if (card.finishes?.includes('etched')) finishes.push('etched');
    return finishes.length > 0 ? finishes : ['foil']; // Default to foil if no finishes specified (should not happen)
  }, [card.nonFoil, card.foil, card.finishes]);

  // Handle collection update submission
  const handleCollectionSubmit = useCallback(async (update: CollectionUpdate) => {
    const cardElement = cardRef.current;
    if (!cardElement) return;

    // Mark as submitting for instant visual feedback (but keep selected for rapid entry)
    cardElement.setAttribute('data-submitting', 'true');

    try {
      // Fire-and-forget submission
      await submitCollectionUpdate({
        ...update,
        setId: card.setId,
        setGroupId: card.setGroupId || undefined
      }, card.name);
      // Success flash via DOM (after mutation succeeds)
      cardElement.removeAttribute('data-submitting');
      cardElement.setAttribute('data-flash', 'success');
      setTimeout(() => cardElement.removeAttribute('data-flash'), 900);
    } catch {
      // Error flash via DOM
      cardElement.removeAttribute('data-submitting');
      cardElement.setAttribute('data-flash', 'error');
      setTimeout(() => cardElement.removeAttribute('data-flash'), 900);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps -- cardRef is a ref and doesn't need to be in dependencies
  }, [submitCollectionUpdate, card.name, card.setId, card.setGroupId]);

  // Collection entry hook
  useCardCollectionEntry({
    cardId: card.id,
    isSelected,
    availableFinishes,
    onSubmit: handleCollectionSubmit
  });

  return {
    availableFinishes,
    handleCollectionSubmit
  };
};