import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import { Box } from '../atoms';
import { MtgSetCard } from '../molecules/Sets/MtgSetCard';
import { BinderView, BinderControls } from '../organisms/Binder';
import type { BinderViewMode } from '../organisms/Binder/BinderControls';
import { LoadingIndicator } from '../molecules/feedback/LoadingIndicator';
import { StatusMessage } from '../molecules/feedback/StatusMessage';
import { useBinderPageData } from '../../hooks/useBinderPageData';
import { useBinderNavigation } from '../../hooks/useBinderNavigation';
import { useResponsiveBreakpoints } from '../../hooks/useResponsiveBreakpoints';
import { useCollectorParam } from '../../hooks/useCollectorParam';

/**
 * Binder page displaying cards in a 3x3 grid format mimicking physical binder pages.
 * Only shows cards from collection groups the user is actively collecting.
 */
export const BinderPage: React.FC = () => {
  const { setCode } = useParams<{ setCode: string }>();
  const { hasCollector } = useCollectorParam();
  const { isMobile, isTablet } = useResponsiveBreakpoints();

  // Book mode is only available on desktop
  const canUseBookMode = !isMobile && !isTablet;

  // View mode state (default to book on desktop, single on mobile)
  const [viewMode, setViewMode] = useState<BinderViewMode>('book');

  // Effective book mode: only if on desktop AND user selected book mode
  const useBookMode = canUseBookMode && viewMode === 'book';

  const {
    setInfo,
    binderCards,
    collectedCardIds,
    getPageCards,
    currentPage,
    totalPages,
    sortBy,
    setSortBy,
    goToPage,
    nextPage,
    prevPage,
    nextSpread,
    prevSpread,
    isLoading,
    firstError,
    hasCollectingGroups
  } = useBinderPageData(setCode);

  // Effective collector: only treat as collector if tracking groups for this set
  // If not tracking anything, show all cards at full opacity (like no collector)
  const effectiveHasCollector = hasCollector && hasCollectingGroups;

  // Use spread navigation in book mode, single page navigation otherwise
  const handleNext = useBookMode ? nextSpread : nextPage;
  const handlePrev = useBookMode ? prevSpread : prevPage;

  // Keyboard navigation
  useBinderNavigation({
    currentPage,
    totalPages,
    nextPage: handleNext,
    prevPage: handlePrev,
    goToPage,
    enabled: true
  });


  // Loading state
  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
        <LoadingIndicator message="Loading binder..." />
      </Box>
    );
  }

  // Error state
  if (firstError) {
    return (
      <Box sx={{ py: 4 }}>
        <StatusMessage
          type="error"
          message="Failed to load binder data"
          details={firstError.message}
        />
      </Box>
    );
  }

  return (
    <Box
      data-component="binder-page"
      sx={{
        // Use CSS Grid for precise viewport fitting on desktop
        // Height: 100dvh - header(64px) - footer(48px) = 100dvh - 112px
        display: { xs: 'block', lg: 'grid' },
        gridTemplateRows: { lg: 'auto auto 1fr' },
        height: { xs: 'auto', lg: 'calc(100dvh - 112px)' },
        maxHeight: { xs: 'none', lg: 'calc(100dvh - 112px)' },
        overflow: 'hidden',
        py: 2,
        px: { xs: 1, sm: 2, md: 3 }
      }}
    >
      {/* Header - fixed size */}
      <Box
        data-component="binder-header"
        sx={{
          display: 'flex',
          justifyContent: 'center',
          mb: 1
        }}
      >
        {/* Set card - click navigates back to set */}
        {setInfo && (
          <MtgSetCard set={setInfo} expanded />
        )}
      </Box>

      {/* Controls - fixed size */}
      <Box data-component="binder-controls">
        <BinderControls
          sortBy={sortBy}
          onSortChange={setSortBy}
          currentPage={currentPage}
          totalPages={totalPages}
          onPrev={handlePrev}
          onNext={handleNext}
          viewMode={viewMode}
          onViewModeChange={setViewMode}
          canUseBookMode={canUseBookMode}
        />
      </Box>

      {/* Binder view - fills remaining grid row */}
      <Box
        data-component="binder-view-container"
        sx={{
          minHeight: { xs: 500, lg: 0 },
          height: { lg: '100%' },
          overflow: 'hidden'
        }}
      >
        <BinderView
          cards={binderCards}
          collectedCardIds={collectedCardIds}
          currentPage={currentPage}
          totalPages={totalPages}
          bookMode={useBookMode}
          onPageChange={goToPage}
          onNext={handleNext}
          onPrev={handlePrev}
          getPageCards={getPageCards}
          hasCollector={effectiveHasCollector}
        />
      </Box>
    </Box>
  );
};

export default BinderPage;
