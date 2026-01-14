import { useState, useEffect, useCallback } from 'react';
import { globalSealedProductEntry } from '../utils/globalSealedProductEntryHandler';
import { sealedProductOverlay } from '../utils/directSealedProductOverlay';
import type { SealedProductCollectionUpdate } from '../contexts/SealedCollectionContext';

interface UseSealedProductCollectionEntryOptions {
  productUuid: string;
  setId: string;
  isSelected: boolean;
  onSubmit: (update: SealedProductCollectionUpdate) => Promise<void>;
}

interface UseSealedProductCollectionEntryReturn {
  isEntering: boolean;
  invalidFlash: boolean;
}

export function useSealedProductCollectionEntry({
  productUuid,
  setId,
  isSelected,
  onSubmit
}: UseSealedProductCollectionEntryOptions): UseSealedProductCollectionEntryReturn {
  // Dummy state for React compatibility
  const [isEntering] = useState(false);
  const [invalidFlash, setInvalidFlash] = useState(false);

  const flashInvalid = useCallback(() => {
    setInvalidFlash(true);
    sealedProductOverlay.flash(productUuid);
    setTimeout(() => setInvalidFlash(false), 150);
  }, [productUuid]);

  // Register with global handler
  useEffect(() => {
    globalSealedProductEntry.register(productUuid, {
      productUuid,
      setId,
      onSubmit,
      onFlashInvalid: flashInvalid
    });

    return () => {
      globalSealedProductEntry.unregister(productUuid);
    };
  }, [productUuid, setId, onSubmit, flashInvalid]);

  // Pre-create overlay when selected
  useEffect(() => {
    if (isSelected) {
      sealedProductOverlay.ensureOverlay(productUuid);
    } else {
      globalSealedProductEntry.reset(productUuid);
    }
  }, [isSelected, productUuid]);

  return {
    isEntering,
    invalidFlash
  };
}
