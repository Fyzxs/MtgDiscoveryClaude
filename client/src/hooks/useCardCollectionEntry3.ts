import { useState, useEffect, useCallback } from 'react';
import type { CollectionEntryState, CardCollectionUpdate, CardFinish } from '../types/collection';
import { globalCardEntry } from '../utils/globalCardEntryHandler';
import { domOverlay } from '../utils/directDomOverlay';

interface UseCardCollectionEntryOptions {
  cardId: string;
  isSelected: boolean;
  availableFinishes?: CardFinish[];
  onSubmit: (update: CardCollectionUpdate) => Promise<void>;
}

interface UseCardCollectionEntryReturn {
  isEntering: boolean;
  entryState: CollectionEntryState;
  invalidFinishFlash: boolean;
}

export function useCardCollectionEntry({
  cardId,
  isSelected,
  availableFinishes = ['non-foil', 'foil', 'etched'],
  onSubmit
}: UseCardCollectionEntryOptions): UseCardCollectionEntryReturn {
  // Just dummy state for React compatibility
  const [isEntering] = useState(false);
  const [entryState] = useState<CollectionEntryState>({
    count: '',
    finish: 'non-foil',
    special: 'none',
    isNegative: false
  });
  const [invalidFinishFlash, setInvalidFinishFlash] = useState(false);

  const flashInvalid = useCallback(() => {
    setInvalidFinishFlash(true);
    domOverlay.flash(cardId);
    setTimeout(() => setInvalidFinishFlash(false), 150);
  }, [cardId]);

  // Register with global handler
  useEffect(() => {
    globalCardEntry.register(cardId, {
      cardId,
      availableFinishes,
      onSubmit,
      onFlashInvalid: flashInvalid
    });

    return () => {
      globalCardEntry.unregister(cardId);
    };
  }, [cardId, availableFinishes, onSubmit, flashInvalid]);

  // Pre-create overlay when selected
  useEffect(() => {
    if (isSelected) {
      domOverlay.ensureOverlay(cardId);
    } else {
      globalCardEntry.reset(cardId);
    }
  }, [isSelected, cardId]);

  return {
    isEntering,
    entryState,
    invalidFinishFlash
  };
}