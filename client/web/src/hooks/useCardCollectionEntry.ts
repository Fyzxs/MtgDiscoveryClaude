import { useState, useEffect, useCallback, useRef } from 'react';
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

  // Refs for stable registration — avoid re-registration when mutation responses
  // cause new array/callback references (re-registration calls unregister → hideOverlay,
  // which stomps on a visible overlay the user is actively typing into)
  const availableFinishesRef = useRef(availableFinishes);
  availableFinishesRef.current = availableFinishes;
  const onSubmitRef = useRef(onSubmit);
  onSubmitRef.current = onSubmit;

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
    console.log(`[OVERLAY] React setState — visible=${state.visible} count=${state.count}`);
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

  // Only register with globalCardEntry when selected.
  // Unregister on deselect/unmount — avoids 200+ registrations on page load.
  // Uses refs for onSubmit and availableFinishes so the effect only re-runs
  // on isSelected/cardId changes, NOT on mutation-triggered re-renders
  // (which would cause unregister → hideOverlay, stomping on a visible overlay).
  useEffect(() => {
    if (!isSelected) return;
    globalCardEntry.register(cardId, {
      cardId,
      get availableFinishes() { return availableFinishesRef.current; },
      onSubmit: (update) => onSubmitRef.current(update),
      onFlashInvalid: flashInvalid,
      onOverlayUpdate: updateOverlayState
    });
    return () => {
      globalCardEntry.unregister(cardId);
    };
  }, [isSelected, cardId, flashInvalid, updateOverlayState]);

  // Reset local React state when deselected (no globalCardEntry call needed —
  // unregister above already hides the overlay via DOM)
  useEffect(() => {
    if (isSelected === false) {
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
  }, [isSelected, availableFinishes]);

  return {
    isEntering,
    invalidFinishFlash,
    overlayState
  };
}