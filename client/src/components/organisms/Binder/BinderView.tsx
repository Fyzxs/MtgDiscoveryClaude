import React, { useRef, useEffect, useMemo } from 'react';
import { Box } from '../../atoms';
import { BinderPageGrid } from './BinderPageGrid';
import { BinderPageTabs } from './BinderPageTabs';
import { useSwipeGesture } from '../../../hooks/useSwipeGesture';
import type { Card } from '../../../types/card';

interface BinderViewProps {
  /** All cards in the binder, sorted */
  cards: Card[];
  /** Set of collected card IDs */
  collectedCardIds: Set<string>;
  /** Current page number (1-indexed) */
  currentPage: number;
  /** Total number of pages */
  totalPages: number;
  /** Whether to show back-to-back book mode */
  bookMode: boolean;
  /** Navigate to specific page */
  onPageChange: (page: number) => void;
  /** Navigate to next page/spread */
  onNext: () => void;
  /** Navigate to previous page/spread */
  onPrev: () => void;
  /** Function to get cards for a specific page */
  getPageCards: (pageNum: number) => (Card | null)[];
  /** Whether there is a collector viewing (affects card opacity) */
  hasCollector?: boolean;
}

/**
 * Main binder view container.
 * Handles single page vs book mode display and swipe navigation.
 */
export const BinderView: React.FC<BinderViewProps> = ({
  collectedCardIds,
  currentPage,
  totalPages,
  bookMode,
  onPageChange,
  onNext,
  onPrev,
  getPageCards,
  hasCollector = true
}) => {
  const containerRef = useRef<HTMLDivElement>(null);

  // Swipe gesture handling
  const swipeHandlers = useSwipeGesture({
    onSwipeLeft: () => onNext(),
    onSwipeRight: () => onPrev(),
    threshold: 50,
    velocityThreshold: 0.2
  });

  // Attach swipe handlers to container
  useEffect(() => {
    const container = containerRef.current;
    if (!container) return;

    const handleTouchStart = (e: TouchEvent) => swipeHandlers.onTouchStart(e);
    const handleTouchMove = (e: TouchEvent) => swipeHandlers.onTouchMove(e);
    const handleTouchEnd = (e: TouchEvent) => swipeHandlers.onTouchEnd(e);
    const handleTouchCancel = (e: TouchEvent) => swipeHandlers.onTouchCancel(e);

    container.addEventListener('touchstart', handleTouchStart, { passive: true });
    container.addEventListener('touchmove', handleTouchMove, { passive: true });
    container.addEventListener('touchend', handleTouchEnd, { passive: true });
    container.addEventListener('touchcancel', handleTouchCancel, { passive: true });

    return () => {
      container.removeEventListener('touchstart', handleTouchStart);
      container.removeEventListener('touchmove', handleTouchMove);
      container.removeEventListener('touchend', handleTouchEnd);
      container.removeEventListener('touchcancel', handleTouchCancel);
    };
  }, [swipeHandlers]);

  // Calculate which pages to display
  const { leftPageNum, rightPageNum } = useMemo(() => {
    if (!bookMode) {
      // Single page mode - just show current page on right
      return { leftPageNum: null, rightPageNum: currentPage };
    }

    // Book mode
    if (currentPage === 1) {
      // Page 1 is always solo (like a title page)
      return { leftPageNum: null, rightPageNum: 1 };
    }

    // After page 1, show even-odd pairs
    // Page 2-3, 4-5, 6-7, etc.
    // Even pages go on left, odd on right
    if (currentPage % 2 === 0) {
      // Current is even - it's the left page
      return {
        leftPageNum: currentPage,
        rightPageNum: currentPage + 1 <= totalPages ? currentPage + 1 : null
      };
    } else {
      // Current is odd - it's the right page
      return {
        leftPageNum: currentPage - 1,
        rightPageNum: currentPage
      };
    }
  }, [bookMode, currentPage, totalPages]);

  // Get tab elements
  const { leftTabs: leftTabsElement, rightTabs: rightTabsElement } = BinderPageTabs({
    currentPage,
    totalPages,
    onPageSelect: onPageChange,
    bookMode
  });

  return (
    <Box
      ref={containerRef}
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'flex-start',
        gap: 1,
        py: 2,
        px: { xs: 1, sm: 2 },
        minHeight: { xs: 500, sm: 600, md: 700 },
        touchAction: 'pan-y' // Allow vertical scroll, capture horizontal swipes
      }}
    >
      {/* Left tabs - next to left card */}
      {leftTabsElement}

      {/* Book mode: always show two-page layout */}
      {bookMode && (
        <>
          {/* Left page or empty placeholder */}
          {leftPageNum !== null ? (
            <BinderPageGrid
              cards={getPageCards(leftPageNum)}
              collectedCardIds={collectedCardIds}
              pageNumber={leftPageNum}
              showPageNumber={true}
              hasCollector={hasCollector}
            />
          ) : (
            <Box
              sx={{
                width: '100%',
                maxWidth: { xs: 360, sm: 450, md: 550, lg: 600 },
                aspectRatio: '3 / 4',
                bgcolor: 'grey.900',
                borderRadius: 2,
                border: '2px dashed',
                borderColor: 'grey.800',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                opacity: 0.5
              }}
            />
          )}

          {/* Spine/divider */}
          <Box
            sx={{
              width: 4,
              minHeight: { xs: 500, sm: 600, md: 700 },
              bgcolor: 'grey.700',
              borderRadius: 1,
              alignSelf: 'stretch',
              display: { xs: 'none', md: 'block' }
            }}
          />

          {/* Right page or empty placeholder */}
          {rightPageNum !== null && rightPageNum <= totalPages ? (
            <BinderPageGrid
              cards={getPageCards(rightPageNum)}
              collectedCardIds={collectedCardIds}
              pageNumber={rightPageNum}
              showPageNumber={true}
              hasCollector={hasCollector}
            />
          ) : (
            <Box
              sx={{
                width: '100%',
                maxWidth: { xs: 360, sm: 450, md: 550, lg: 600 },
                aspectRatio: '3 / 4',
                bgcolor: 'grey.900',
                borderRadius: 2,
                border: '2px dashed',
                borderColor: 'grey.800',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                opacity: 0.5
              }}
            />
          )}
        </>
      )}

      {/* Single page mode (non-book) */}
      {!bookMode && rightPageNum !== null && rightPageNum <= totalPages && (
        <BinderPageGrid
          cards={getPageCards(rightPageNum)}
          collectedCardIds={collectedCardIds}
          pageNumber={rightPageNum}
          showPageNumber={true}
          hasCollector={hasCollector}
        />
      )}

      {/* Right tabs - next to right card */}
      {rightTabsElement}
    </Box>
  );
};
