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
    bookMode,
    getPageCards,
    collectedCardIds,
    hasCollector
  });

  return (
    <Box
      ref={containerRef}
      data-component="binder-view"
      sx={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'flex-start',
        gap: 0,
        py: 1,
        px: { xs: 1, sm: 2 },
        maxWidth: '100vw',
        overflow: 'hidden',
        touchAction: 'pan-y' // Allow vertical scroll, capture horizontal swipes
      }}
    >
      {/* Book mode: always show two-page layout */}
      {bookMode && (
        <>
          {/* Left section: tabs + page */}
          <Box
            data-component="binder-left-section"
            sx={{
              display: 'flex',
              alignItems: 'flex-start',
              gap: 0.5,
              flex: 1,
              minWidth: 0,
              maxWidth: { xs: '100%', sm: 'calc(600px + 32px)', md: 'calc(700px + 32px)', lg: 'calc(800px + 32px)' }
            }}
          >
            {leftTabsElement}
            {/* Page wrapper for divider positioning */}
            <Box data-component="binder-left-page-wrapper" sx={{ position: 'relative', flex: 1, minWidth: 0 }}>
              <BinderPageGrid
                cards={leftPageNum !== null ? getPageCards(leftPageNum) : Array(9).fill(null)}
                collectedCardIds={collectedCardIds}
                pageNumber={leftPageNum ?? 0}
                hasCollector={hasCollector}
              />
              {/* Spine/divider */}
              <Box
                sx={{
                  position: 'absolute',
                  right: -2,
                  top: 0,
                  bottom: 0,
                  width: 4,
                  bgcolor: 'grey.700',
                  borderRadius: 1,
                  display: { xs: 'none', md: 'block' }
                }}
              />
            </Box>
          </Box>

          {/* Right section: page + tabs */}
          <Box
            data-component="binder-right-section"
            sx={{
              display: 'flex',
              alignItems: 'flex-start',
              gap: 0.5,
              flex: 1,
              minWidth: 0,
              maxWidth: { xs: '100%', sm: 'calc(600px + 32px)', md: 'calc(700px + 32px)', lg: 'calc(800px + 32px)' }
            }}
          >
            <Box data-component="binder-right-page-wrapper" sx={{ position: 'relative', flex: 1, minWidth: 0 }}>
              <BinderPageGrid
                cards={rightPageNum !== null && rightPageNum <= totalPages ? getPageCards(rightPageNum) : Array(9).fill(null)}
                collectedCardIds={collectedCardIds}
                pageNumber={rightPageNum ?? 0}
                hasCollector={hasCollector}
              />
            </Box>
            {rightTabsElement}
          </Box>
        </>
      )}

      {/* Single page mode (non-book) */}
      {!bookMode && rightPageNum !== null && rightPageNum <= totalPages && (
        <Box
          sx={{
            flex: 1,
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'flex-start',
            gap: 0.5,
            width: '100%',
            overflow: 'hidden'
          }}
        >
          {leftTabsElement}
          <BinderPageGrid
            cards={getPageCards(rightPageNum)}
            collectedCardIds={collectedCardIds}
            pageNumber={rightPageNum}
            hasCollector={hasCollector}
          />
          {rightTabsElement}
        </Box>
      )}
    </Box>
  );
};
