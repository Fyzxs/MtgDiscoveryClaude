import { useState, useCallback, useEffect } from 'react';
import type { RefObject } from 'react';
import type { OverlayBehavior } from './useCardDisplaySettings';

interface MtgCardInteractionsProps {
  cardRef: RefObject<HTMLDivElement | null>;
  overlayBehavior?: OverlayBehavior;
}

export const useMtgCardInteractions = ({
  cardRef,
  overlayBehavior = 'hover'
}: MtgCardInteractionsProps) => {
  const [modalOpen, setModalOpen] = useState(false);
  const [isSelected, setIsSelected] = useState(false);
  const [overlayExpanded, setOverlayExpanded] = useState(false);

  // Track selection state from DOM attribute
  useEffect(() => {
    if (!cardRef.current) return;

    const observer = new MutationObserver((mutations) => {
      mutations.forEach((mutation) => {
        if (mutation.attributeName === 'data-selected') {
          const selected = cardRef.current?.getAttribute('data-selected') === 'true';
          setIsSelected(selected || false);
        }
      });
    });

    observer.observe(cardRef.current, { attributes: true, attributeFilter: ['data-selected'] });

    return () => observer.disconnect();
  }, [cardRef]);

  // Reset overlay expanded state when card is deselected or another card is selected
  useEffect(() => {
    if (!isSelected && overlayExpanded) {
      setOverlayExpanded(false);
    }
  }, [isSelected, overlayExpanded]);

  const handleCardClick = useCallback((e: React.MouseEvent) => {
    // Don't trigger selection if clicking on links or zoom indicator
    const target = e.target as HTMLElement;
    const clickedLink = target.closest('a');
    const clickedZoom = target.closest('.zoom-indicator');

    // If clicking a link or zoom, let it handle itself - don't prevent default
    if (clickedLink || clickedZoom) {
      return;
    }

    // Only prevent default for card selection clicks
    e.preventDefault();
    e.stopPropagation();

    const cardElement = e.currentTarget as HTMLElement;

    // Handle tap behavior on mobile/tablet - single tap opens modal
    if (overlayBehavior === 'tap') {
      setModalOpen(true);
      return;
    }

    // Desktop behavior - select card
    // Clear ALL selections across ALL card groups on the page
    const allSelected = document.querySelectorAll('[data-selected="true"]');
    allSelected.forEach(selected => {
      if (selected !== cardElement) {
        selected.setAttribute('data-selected', 'false');
      }
    });

    // Always select the clicked card (don't toggle)
    cardElement.setAttribute('data-selected', 'true');

    // Remove focus from any other focused element to clear blue borders
    const focused = document.querySelector(':focus');
    if (focused && focused !== cardElement) {
      (focused as HTMLElement).blur();
    }

    // Focus this card to enable keyboard navigation from here
    cardElement.focus();
  }, [overlayBehavior, overlayExpanded]);

  const handleZoomClick = useCallback((e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setModalOpen(true);
  }, []);

  const handleModalClose = useCallback(() => {
    setModalOpen(false);
    // Reset overlay state so next tap starts fresh
    setOverlayExpanded(false);
  }, []);

  const handleOverlayToggle = useCallback(() => {
    setOverlayExpanded(prev => !prev);
  }, []);

  return {
    modalOpen,
    isSelected,
    overlayExpanded,
    handleCardClick,
    handleZoomClick,
    handleModalClose,
    handleOverlayToggle
  };
};
