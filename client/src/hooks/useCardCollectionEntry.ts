import { useState, useEffect, useCallback } from 'react';
import type { CardCollectionUpdate, CardFinish, CardSpecial } from '../types/collection';
import { globalCardEntry } from '../utils/globalCardEntryHandler';

export interface CardOverlayState {
  visible: boolean;
  count: number;
  isNegative: boolean;
  finish: CardFinish;
  special: CardSpecial;
  flash: boolean;
}

interface UseCardCollectionEntryOptions {
  cardId: string;
  isSelected: boolean;
  availableFinishes?: CardFinish[];
  onSubmit: (update: CardCollectionUpdate) => Promise<void>;
}

interface UseCardCollectionEntryReturn {
  isEntering: boolean;
  invalidFinishFlash: boolean;
  overlayState: CardOverlayState;
}

export function useCardCollectionEntry({
  cardId,
  isSelected,
  availableFinishes = ['non-foil', 'foil', 'etched'],
  onSubmit
}: UseCardCollectionEntryOptions): UseCardCollectionEntryReturn {
  // React state for overlay display
  const [overlayState, setOverlayState] = useState<CardOverlayState>({
    visible: false,
    count: 0,
    isNegative: false,
    finish: availableFinishes[0] || 'non-foil',
    special: 'none',
    flash: false
  });

  // Track entering state
  const [isEntering, setIsEntering] = useState(false);
  const [invalidFinishFlash, setInvalidFinishFlash] = useState(false);

  const flashInvalid = useCallback(() => {
    setInvalidFinishFlash(true);
    setOverlayState(prev => ({ ...prev, flash: true }));
    setTimeout(() => {
      setInvalidFinishFlash(false);
      setOverlayState(prev => ({ ...prev, flash: false }));
    }, 150);
  }, []);

  // Callback for global handler to update overlay state
  const updateOverlayState = useCallback((state: {
    count: string;
    isNegative: boolean;
    visible: boolean;
    finish: CardFinish;
    special: CardSpecial;
  }) => {
    const count = state.count === '' ? 0 : parseInt(state.count, 10);
    setOverlayState({
      visible: state.visible,
      count,
      isNegative: state.isNegative,
      finish: state.finish,
      special: state.special,
      flash: false
    });
    setIsEntering(state.visible);
  }, []);

  // Register with global handler
  useEffect(() => {
    globalCardEntry.register(cardId, {
      cardId,
      availableFinishes,
      onSubmit,
      onFlashInvalid: flashInvalid,
      onOverlayUpdate: updateOverlayState
    });

    return () => {
      globalCardEntry.unregister(cardId);
    };
  }, [cardId, availableFinishes, onSubmit, flashInvalid, updateOverlayState]);

  // Reset overlay when not selected
  useEffect(() => {
    if (isSelected === false) {
      globalCardEntry.reset(cardId);
      setOverlayState({
        visible: false,
        count: 0,
        isNegative: false,
        finish: availableFinishes[0] || 'non-foil',
        special: 'none',
        flash: false
      });
      setIsEntering(false);
    }
  }, [isSelected, cardId, availableFinishes]);

  return {
    isEntering,
    invalidFinishFlash,
    overlayState
  };
}