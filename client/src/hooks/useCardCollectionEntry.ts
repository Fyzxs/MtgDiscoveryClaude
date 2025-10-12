import { useState, useEffect, useCallback, useRef } from 'react';
import type {
  CollectionEntryState,
  CardCollectionUpdate,
  CardFinish,
  CardSpecial
} from '../types/collection';
import { domOverlay } from '../utils/directDomOverlay';
import { domHelpPanel } from '../utils/directDomHelpPanel';

interface UseCardCollectionEntryOptions {
  cardId: string;
  isSelected: boolean;
  availableFinishes?: CardFinish[];
  onSubmit: (update: CardCollectionUpdate) => Promise<void>;
  onEntryStateChange?: (isEntering: boolean) => void;
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
  onSubmit,
  onEntryStateChange
}: UseCardCollectionEntryOptions): UseCardCollectionEntryReturn {
  const [isEntering, setIsEntering] = useState(false);
  const [entryState, setEntryState] = useState<CollectionEntryState>({
    count: '',
    finish: 'non-foil',
    special: 'none',
    isNegative: false
  });
  const [invalidFinishFlash, setInvalidFinishFlash] = useState(false);

  // Use ref to track if this card is the currently active one
  const isActiveRef = useRef(false);
  // Use ref to track entry state without React delays
  const isEnteringRef = useRef(false);
  // Store entry state in ref for instant access
  const entryStateRef = useRef<CollectionEntryState>({
    count: '',
    finish: 'non-foil',
    special: 'none',
    isNegative: false
  });

  // Handle selection changes
  useEffect(() => {
    if (!isSelected) {
      setIsEntering(false);
      isEnteringRef.current = false;
      entryStateRef.current = {
        count: '',
        finish: 'non-foil',
        special: 'none',
        isNegative: false
      };
      setEntryState({
        count: '',
        finish: 'non-foil',
        special: 'none',
        isNegative: false
      });
      setInvalidFinishFlash(false);
      isActiveRef.current = false;
      onEntryStateChange?.(false);
      domOverlay.hide(cardId);
      domHelpPanel.hide();
    }
  }, [isSelected, cardId, onEntryStateChange]);

  // Flash invalid finish attempt
  const flashInvalid = useCallback(() => {
    setInvalidFinishFlash(true);
    domOverlay.flash(cardId);
    setTimeout(() => setInvalidFinishFlash(false), 150);
  }, [cardId]);

  // Process key input and update DOM immediately
  const processKey = useCallback((key: string, isShift: boolean) => {
    // Use ref state for instant access
    let newState = { ...entryStateRef.current };

    // Number keys (0-9)
    if (key >= '0' && key <= '9') {
      newState.count = newState.count === '0' ? key : newState.count + key;
    }
    // Increment (+, `)
    else if (key === '+' || key === '`') {
      const current = parseInt(newState.count || '0');
      newState.count = String(current + 1);
    }
    // Decrement (-, ~)
    else if (key === '-' || (isShift && key === '~') || key === '~') {
      const current = parseInt(newState.count || '0');
      if (current > 0) {
        newState.count = String(current - 1);
      }
    }
    // Negate (x)
    else if (key === 'x') {
      const count = newState.count || '0';
      if (count !== '0' && count !== '') {
        newState.isNegative = !newState.isNegative;
      }
    }
    // Finish keys
    else if (['z', 'n', 'f', 'o', 'e', 'h'].includes(key)) {
      const finishMap: Record<string, CardFinish> = {
        'z': 'non-foil',
        'n': 'non-foil',
        'f': 'foil',
        'o': 'foil',
        'e': 'etched',
        'h': 'etched'
      };
      const targetFinish = finishMap[key];
      if (availableFinishes.includes(targetFinish)) {
        newState.finish = targetFinish;
      } else {
        flashInvalid();
        return; // Don't update state for invalid finish
      }
    }
    // Special keys
    else if (['g', 'i', 'r', 'p', 't', 'm'].includes(key)) {
      const specialMap: Record<string, CardSpecial> = {
        'g': 'signed',
        'i': 'signed',
        'r': 'artist-proof',
        'p': 'artist-proof',
        't': 'altered',
        'm': 'altered'
      };
      const newSpecial = specialMap[key];
      newState.special = newState.special === newSpecial ? 'none' : newSpecial;
    }

    // Update ref immediately
    entryStateRef.current = newState;

    // Update DOM immediately
    domOverlay.show(cardId, newState, false);

    // Update React state (will catch up later)
    setEntryState(newState);
  }, [cardId, availableFinishes, flashInvalid]);

  // Cancel entry
  const cancelEntry = useCallback(() => {
    setIsEntering(false);
    isEnteringRef.current = false;
    entryStateRef.current = {
      count: '',
      finish: 'non-foil',
      special: 'none',
      isNegative: false
    };
    setEntryState({
      count: '',
      finish: 'non-foil',
      special: 'none',
      isNegative: false
    });
    setInvalidFinishFlash(false);
    isActiveRef.current = false;
    onEntryStateChange?.(false);
    // Hide overlay and help panel immediately via DOM
    domOverlay.hide(cardId);
    domHelpPanel.hide();
  }, [cardId, onEntryStateChange]);

  // Submit entry
  const submitEntry = useCallback(async () => {
    if (!isEnteringRef.current || !isActiveRef.current) return;

    const count = parseInt(entryStateRef.current.count || '0');
    const finalCount = entryStateRef.current.isNegative ? -count : count;

    try {
      await onSubmit({
        cardId,
        count: finalCount,
        finish: entryStateRef.current.finish,
        special: entryStateRef.current.special
      });

      // Reset state after successful submission
      cancelEntry();
    } catch (error) {
      // Don't reset on error - let user retry or cancel manually
      throw error;
    }
  }, [cardId, onSubmit, cancelEntry]);

  // Global keyboard handler
  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      // Check if THIS card is selected via DOM (instant check, no React state!)
      const cardElement = document.querySelector(`[data-card-id="${cardId}"]`);
      if (!cardElement || cardElement.getAttribute('data-selected') !== 'true') return;

      const key = event.key.toLowerCase();
      const isShift = event.shiftKey;

      // Don't process if modifier keys (except shift) are pressed
      if (event.ctrlKey || event.altKey || event.metaKey) return;

      // Handle escape - cancel entry (use ref for instant check)
      if (key === 'escape') {
        if (isEnteringRef.current) {
          cancelEntry();
          event.preventDefault();
          event.stopPropagation();
        }
        return;
      }

      // Handle enter - submit entry (use ref for instant check)
      if (key === 'enter') {
        if (isEnteringRef.current) {
          submitEntry();
          event.preventDefault();
          event.stopPropagation();
        }
        return;
      }

      // Check if key is valid for starting entry
      const validKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'z', 'n', 'f', 'o', 'e', 'h', 'g', 'i', 'r', 'p', 't', 'm',
        '+', '`', '-', '~', 'x'];

      if (validKeys.includes(key) || (isShift && key === '~')) {
        event.preventDefault();
        event.stopPropagation();

        // Use ref for instant check - no React state delay!
        if (!isEnteringRef.current) {
          // IMMEDIATELY show overlay with current state - don't wait for processing!
          domOverlay.show(cardId, entryStateRef.current, false);
          domHelpPanel.show();

          // Mark as entering
          isEnteringRef.current = true;
          isActiveRef.current = true;

          // NOW process the key
          processKey(key, isShift);

          // Update React state (will catch up later)
          setIsEntering(true);
          onEntryStateChange?.(true);
        } else {
          // Already entering, just process the key
          processKey(key, isShift);
        }
      }
    };

    // Use capture phase for immediate response
    document.addEventListener('keydown', handleKeyDown, true);
    return () => document.removeEventListener('keydown', handleKeyDown, true);
  }, [cardId, processKey, cancelEntry, submitEntry, onEntryStateChange]);

  // Cleanup on unmount
  useEffect(() => {
    return () => {
      domOverlay.cleanup(cardId);
    };
  }, [cardId]);

  return {
    isEntering,
    entryState,
    invalidFinishFlash
  };
}