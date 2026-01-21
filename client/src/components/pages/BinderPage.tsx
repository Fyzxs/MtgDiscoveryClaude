import React, { useState } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import { Box, Typography, Button } from '../atoms';
import { ArrowBackIcon } from '../atoms/Icons';
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
  const navigate = useNavigate();
  const location = useLocation();
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
    setName,
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

  // Handle navigation back to set page
  const handleBackToSet = () => {
    navigate(`/set/${setCode}${location.search}`);
  };


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
    <Box sx={{ py: 2, px: { xs: 1, sm: 2, md: 3 } }}>
      {/* Header */}
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          gap: 2,
          mb: 3
        }}
      >
        {/* Back button */}
        <Button
          variant="text"
          startIcon={<ArrowBackIcon />}
          onClick={handleBackToSet}
          sx={{ alignSelf: 'flex-start' }}
        >
          Back to Set
        </Button>

        {/* Set card */}
        {setInfo && (
          <MtgSetCard set={setInfo} />
        )}

        {/* Title */}
        <Typography variant="h5" component="h1" sx={{ textAlign: 'center' }}>
          Binder View
        </Typography>

        {/* Stats */}
        <Typography variant="body2" color="text.secondary">
          {effectiveHasCollector
            ? `${collectedCardIds.size} of ${binderCards.length} cards collected`
            : `${binderCards.length} cards`}
          {' '}({totalPages} page{totalPages !== 1 ? 's' : ''})
        </Typography>
      </Box>

      {/* Controls */}
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

      {/* Binder view */}
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
  );
};

export default BinderPage;
