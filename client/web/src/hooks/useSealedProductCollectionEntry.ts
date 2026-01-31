import { useState, useEffect, useCallback } from 'react';
import { globalSealedProductEntry } from '../utils/globalSealedProductEntryHandler';
import type { SealedProductCollectionUpdate } from '../contexts/SealedCollectionContext';

export interface SealedProductOverlayState {
  visible: boolean;
  count: number;
  isNegative: boolean;
  flash: boolean;
}

interface UseSealedProductCollectionEntryOptions {
  productUuid: string;
  setId: string;
  isSelected: boolean;
  onSubmit: (update: SealedProductCollectionUpdate) => Promise<void>;
}

interface UseSealedProductCollectionEntryReturn {
  isEntering: boolean;
  invalidFlash: boolean;
  overlayState: SealedProductOverlayState;
}

export function useSealedProductCollectionEntry({
  productUuid,
  setId,
  isSelected,
  onSubmit
}: UseSealedProductCollectionEntryOptions): UseSealedProductCollectionEntryReturn {
  // React state for overlay display
  const [overlayState, setOverlayState] = useState<SealedProductOverlayState>({
    visible: false,
    count: 0,
    isNegative: false,
    flash: false
  });

  // Track entering state
  const [isEntering, setIsEntering] = useState(false);
  const [invalidFlash, setInvalidFlash] = useState(false);

  const flashInvalid = useCallback(() => {
    setInvalidFlash(true);
    setOverlayState(prev => ({ ...prev, flash: true }));
    setTimeout(() => {
      setInvalidFlash(false);
      setOverlayState(prev => ({ ...prev, flash: false }));
    }, 150);
  }, []);

  // Callback for global handler to update overlay state
  const updateOverlayState = useCallback((state: { count: string; isNegative: boolean; visible: boolean }) => {
    const count = state.count === '' ? 0 : parseInt(state.count, 10);
    setOverlayState({
      visible: state.visible,
      count,
      isNegative: state.isNegative,
      flash: false
    });
    setIsEntering(state.visible);
  }, []);

  // Register with global handler
  useEffect(() => {
    globalSealedProductEntry.register(productUuid, {
      productUuid,
      setId,
      onSubmit,
      onFlashInvalid: flashInvalid,
      onOverlayUpdate: updateOverlayState
    });

    return () => {
      globalSealedProductEntry.unregister(productUuid);
    };
  }, [productUuid, setId, onSubmit, flashInvalid, updateOverlayState]);

  // Reset overlay when not selected
  useEffect(() => {
    if (isSelected === false) {
      globalSealedProductEntry.reset(productUuid);
      setOverlayState({
        visible: false,
        count: 0,
        isNegative: false,
        flash: false
      });
      setIsEntering(false);
    }
  }, [isSelected, productUuid]);

  return {
    isEntering,
    invalidFlash,
    overlayState
  };
}
