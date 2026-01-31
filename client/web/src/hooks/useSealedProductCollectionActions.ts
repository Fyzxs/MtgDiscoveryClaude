import { useCallback, useRef, useEffect } from 'react';
import type { RefObject } from 'react';
import { useSealedCollection } from '../contexts/SealedCollectionContext';
import { useSealedProductCollectionEntry } from '../hooks/useSealedProductCollectionEntry';
import type { SealedProductCollectionUpdate } from '../contexts/SealedCollectionContext';
import type { SealedProductOverlayState } from '../hooks/useSealedProductCollectionEntry';

interface SealedProductCollectionActionsProps {
  productUuid: string;
  setId: string;
  productName: string;
  isSelected: boolean;
  productRef: RefObject<HTMLDivElement | null>;
}

export const useSealedProductCollectionActions = ({
  productUuid,
  setId,
  productName,
  isSelected,
  productRef
}: SealedProductCollectionActionsProps) => {
  const { submitSealedProductUpdate } = useSealedCollection();

  // Use refs to avoid callback identity changes
  const submitSealedProductUpdateRef = useRef(submitSealedProductUpdate);

  useEffect(() => {
    submitSealedProductUpdateRef.current = submitSealedProductUpdate;
  }, [submitSealedProductUpdate]);

  // Handle sealed product update submission
  const handleSealedProductSubmit = useCallback(async (update: SealedProductCollectionUpdate) => {
    const productElement = productRef.current;
    if (!productElement) return;

    // Mark as submitting for instant visual feedback
    productElement.setAttribute('data-submitting', 'true');

    try {
      await submitSealedProductUpdateRef.current(update, productName);
      // Success flash via DOM
      productElement.removeAttribute('data-submitting');
      productElement.setAttribute('data-flash', 'success');
      setTimeout(() => productElement.removeAttribute('data-flash'), 900);
    } catch {
      // Error flash via DOM
      productElement.removeAttribute('data-submitting');
      productElement.setAttribute('data-flash', 'error');
      setTimeout(() => productElement.removeAttribute('data-flash'), 900);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps -- productRef is a ref, refs are stable
  }, [productName]);

  // Collection entry hook
  const { overlayState } = useSealedProductCollectionEntry({
    productUuid,
    setId,
    isSelected,
    onSubmit: handleSealedProductSubmit
  });

  return {
    handleSealedProductSubmit,
    overlayState
  };
};
