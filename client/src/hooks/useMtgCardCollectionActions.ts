import { useCallback, useMemo, useRef, useEffect } from 'react';
import type { RefObject } from 'react';
import { useCollection } from '../contexts/CollectionContext';
import { useWishlist } from '../contexts/WishlistContext';
import { useCardCollectionEntry } from '../hooks/useCardCollectionEntry';
import type { CardOverlayState } from '../hooks/useCardCollectionEntry';
import type { Card } from '../types/card';
import type { CardFinish, CardSpecial, EntryMode } from '../types/collection';

interface CollectionUpdate {
  cardId: string;
  count: number;
  finish: CardFinish;
  special: CardSpecial;
}

interface MtgCardCollectionActionsProps {
  card?: Card;
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

  // Use refs to avoid callback identity changes when submit functions or card data updates.
  // Mode is read from document.body at submit time (synced by EntryModeContext)
  // so this hook does NOT subscribe to EntryModeContext — preventing mass re-renders
  // of every MtgCard/BinderSlot when the user toggles mode.
  const submitCollectionUpdateRef = useRef(submitCollectionUpdate);
  const submitWishlistUpdateRef = useRef(submitWishlistUpdate);
  const cardDataRef = useRef(card);

  useEffect(() => {
    submitCollectionUpdateRef.current = submitCollectionUpdate;
    submitWishlistUpdateRef.current = submitWishlistUpdate;
    cardDataRef.current = card;
  }, [submitCollectionUpdate, submitWishlistUpdate, card]);

  // Determine available finishes
  const availableFinishes = useMemo<CardFinish[]>(() => {
    if (!card) return [];
    const finishes: CardFinish[] = [];
    if (card.nonFoil) finishes.push('non-foil');
    if (card.foil) finishes.push('foil');
    if (card.finishes?.includes('etched')) finishes.push('etched');
    return finishes.length > 0 ? finishes : ['foil']; // Default to foil if no finishes specified (should not happen)
  }, [card]);

  // Handle collection/wishlist update submission based on mode
  // Uses refs to keep callback identity stable across mode changes and card data updates
  const handleCollectionSubmit = useCallback(async (update: CollectionUpdate) => {
    const currentCard = cardDataRef.current;
    if (!currentCard) return;
    const cardElement = cardRef.current;
    if (!cardElement) return;

    // Read current mode from DOM (synced by EntryModeContext on every toggle)
    const currentMode = (document.body.getAttribute('data-entry-mode') as EntryMode) || 'collection';
    const isWishlist = currentMode === 'wishlist';

    // Mark as submitting for instant visual feedback (but keep selected for rapid entry)
    cardElement.setAttribute('data-submitting', 'true');
    // Mark the mode for styling purposes
    cardElement.setAttribute('data-entry-mode', currentMode);

    try {
      if (isWishlist) {
        // Wishlist submission
        await submitWishlistUpdateRef.current({
          ...update,
          setId: currentCard.setId || ''
        }, currentCard.name);
        // Wishlist flash - light blue with heart indicator
        cardElement.removeAttribute('data-submitting');
        cardElement.setAttribute('data-flash', 'wishlist');
        setTimeout(() => cardElement.removeAttribute('data-flash'), 900);
      } else {
        // Collection submission
        await submitCollectionUpdateRef.current({
          ...update,
          setId: currentCard.setId,
          setCode: currentCard.setCode || '',
          setGroupId: currentCard.setGroupId || null
        }, currentCard.name);
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
    // eslint-disable-next-line react-hooks/exhaustive-deps -- all dependencies accessed via refs for stable callback identity
  }, []);

  // Collection entry hook - only register when card exists
  const { overlayState, isEntering, invalidFinishFlash } = useCardCollectionEntry({
    cardId: card?.id ?? '',
    isSelected: card ? isSelected : false,
    availableFinishes,
    onSubmit: handleCollectionSubmit
  });

  return {
    availableFinishes,
    handleCollectionSubmit,
    overlayState,
    isEntering,
    invalidFinishFlash
  };
};
