import { useCallback, useMemo, useRef, useEffect } from 'react';
import type { RefObject } from 'react';
import { useCollection } from '../contexts/CollectionContext';
import { useWishlist } from '../contexts/WishlistContext';
import { useEntryMode } from '../contexts/EntryModeContext';
import { useCardCollectionEntry } from '../hooks/useCardCollectionEntry';
import type { Card } from '../types/card';
import type { CardFinish, CardSpecial } from '../types/collection';

interface CollectionUpdate {
  cardId: string;
  count: number;
  finish: CardFinish;
  special: CardSpecial;
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
  const { submitWishlistUpdate } = useWishlist();
  const { isWishlistMode, mode } = useEntryMode();

  // Use refs to avoid callback identity changes when mode toggles
  // This prevents the overlay from being destroyed during mode switch
  const modeRef = useRef(mode);
  const isWishlistModeRef = useRef(isWishlistMode);
  const submitCollectionUpdateRef = useRef(submitCollectionUpdate);
  const submitWishlistUpdateRef = useRef(submitWishlistUpdate);

  useEffect(() => {
    modeRef.current = mode;
    isWishlistModeRef.current = isWishlistMode;
    submitCollectionUpdateRef.current = submitCollectionUpdate;
    submitWishlistUpdateRef.current = submitWishlistUpdate;
  }, [mode, isWishlistMode, submitCollectionUpdate, submitWishlistUpdate]);

  // Determine available finishes
  const availableFinishes = useMemo<CardFinish[]>(() => {
    const finishes: CardFinish[] = [];
    if (card.nonFoil) finishes.push('non-foil');
    if (card.foil) finishes.push('foil');
    if (card.finishes?.includes('etched')) finishes.push('etched');
    return finishes.length > 0 ? finishes : ['foil']; // Default to foil if no finishes specified (should not happen)
  }, [card.nonFoil, card.foil, card.finishes]);

  // Handle collection/wishlist update submission based on mode
  // Uses refs to keep callback identity stable across mode changes
  const handleCollectionSubmit = useCallback(async (update: CollectionUpdate) => {
    const cardElement = cardRef.current;
    if (!cardElement) return;

    // Mark as submitting for instant visual feedback (but keep selected for rapid entry)
    cardElement.setAttribute('data-submitting', 'true');
    // Mark the mode for styling purposes
    cardElement.setAttribute('data-entry-mode', modeRef.current);

    try {
      if (isWishlistModeRef.current) {
        // Wishlist submission
        await submitWishlistUpdateRef.current({
          ...update,
          setId: card.setId || ''
        }, card.name);
        // Wishlist flash - light blue with heart indicator
        cardElement.removeAttribute('data-submitting');
        cardElement.setAttribute('data-flash', 'wishlist');
        setTimeout(() => cardElement.removeAttribute('data-flash'), 900);
      } else {
        // Collection submission
        await submitCollectionUpdateRef.current({
          ...update,
          setId: card.setId,
          setGroupId: card.setGroupId || undefined
        }, card.name);
        // Success flash via DOM (after mutation succeeds)
        cardElement.removeAttribute('data-submitting');
        cardElement.setAttribute('data-flash', 'success');
        setTimeout(() => cardElement.removeAttribute('data-flash'), 900);
      }
    } catch {
      // Error flash via DOM
      cardElement.removeAttribute('data-submitting');
      cardElement.setAttribute('data-flash', 'error');
      setTimeout(() => cardElement.removeAttribute('data-flash'), 900);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps -- cardRef is a ref, refs are stable
  }, [card.name, card.setId, card.setGroupId]);

  // Collection entry hook
  useCardCollectionEntry({
    cardId: card.id,
    isSelected,
    availableFinishes,
    onSubmit: handleCollectionSubmit
  });

  return {
    availableFinishes,
    handleCollectionSubmit,
    isWishlistMode,
    entryMode: mode
  };
};
