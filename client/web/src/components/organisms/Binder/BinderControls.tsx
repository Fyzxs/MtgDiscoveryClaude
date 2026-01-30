import React from 'react';
import { Box, IconButton, Typography, ToggleButton, ToggleButtonGroup, Tooltip } from '../../atoms';
import { NavigateBeforeIcon, NavigateNextIcon, SortByAlphaIcon, FormatListNumberedIcon, LooksOneIcon, LooksTwoIcon } from '../../atoms';
import type { BinderSortBy } from '../../../hooks/useBinderPageData';

export type BinderViewMode = 'single' | 'book';

interface BinderControlsProps {
  /** Current sort order */
  sortBy: BinderSortBy;
  /** Callback when sort changes */
  onSortChange: (sort: BinderSortBy) => void;
  /** Current page number (1-indexed) */
  currentPage: number;
  /** Total number of pages */
  totalPages: number;
  /** Navigate to previous page */
  onPrev: () => void;
  /** Navigate to next page */
  onNext: () => void;
  /** Current view mode */
  viewMode: BinderViewMode;
  /** Callback when view mode changes */
  onViewModeChange: (mode: BinderViewMode) => void;
  /** Whether book mode is available (desktop only) */
  canUseBookMode?: boolean;
}

/**
 * Control bar for binder view with sort toggle and navigation.
 */
export const BinderControls: React.FC<BinderControlsProps> = ({
  sortBy,
  onSortChange,
  currentPage,
  totalPages,
  onPrev,
  onNext,
  viewMode,
  onViewModeChange,
  canUseBookMode = true
}) => {
  const canGoPrev = currentPage > 1;
  const canGoNext = currentPage < totalPages;

  const handleSortChange = (_event: React.MouseEvent<HTMLElement>, newSort: BinderSortBy | null) => {
    if (newSort !== null) {
      onSortChange(newSort);
    }
  };

  const handleViewModeChange = (_event: React.MouseEvent<HTMLElement>, newMode: BinderViewMode | null) => {
    if (newMode !== null) {
      onViewModeChange(newMode);
    }
  };

  return (
    <Box
      id="binder-controls"
      sx={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        gap: { xs: 1, sm: 2, md: 3 },
        flexWrap: 'wrap',
        py: 1
      }}
    >
      {/* Sort Toggle */}
      <Box id="binder-sort-controls" sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
        <Typography variant="caption" sx={{ color: 'text.secondary', display: { xs: 'none', sm: 'block' } }}>
          Sort:
        </Typography>
        <ToggleButtonGroup
          value={sortBy}
          exclusive
          onChange={handleSortChange}
          size="small"
          aria-label="Sort order"
        >
          <ToggleButton id="binder-sort-collector" value="collector" aria-label="Sort by collector number">
            <Tooltip title="Collector Number">
              <FormatListNumberedIcon fontSize="small" />
            </Tooltip>
          </ToggleButton>
          <ToggleButton id="binder-sort-alphabetical" value="alphabetical" aria-label="Sort alphabetically">
            <Tooltip title="Alphabetical">
              <SortByAlphaIcon fontSize="small" />
            </Tooltip>
          </ToggleButton>
        </ToggleButtonGroup>
      </Box>

      {/* View Mode Toggle (only show on desktop) */}
      {canUseBookMode && (
        <Box id="binder-view-mode-controls" sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <Typography variant="caption" sx={{ color: 'text.secondary', display: { xs: 'none', sm: 'block' } }}>
            View:
          </Typography>
          <ToggleButtonGroup
            value={viewMode}
            exclusive
            onChange={handleViewModeChange}
            size="small"
            aria-label="View mode"
          >
            <ToggleButton id="binder-view-single" value="single" aria-label="Single page view">
              <Tooltip title="Single Page">
                <LooksOneIcon fontSize="small" />
              </Tooltip>
            </ToggleButton>
            <ToggleButton id="binder-view-book" value="book" aria-label="Two page book view">
              <Tooltip title="Two Pages">
                <LooksTwoIcon fontSize="small" />
              </Tooltip>
            </ToggleButton>
          </ToggleButtonGroup>
        </Box>
      )}

      {/* Navigation */}
      <Box id="binder-navigation-controls" sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
        <Tooltip title={viewMode === 'book' ? 'Previous spread (←)' : 'Previous page (←)'}>
          <span>
            <IconButton
              id="binder-nav-prev"
              onClick={onPrev}
              disabled={!canGoPrev}
              size="small"
              aria-label="Previous page"
            >
              <NavigateBeforeIcon />
            </IconButton>
          </span>
        </Tooltip>

        <Typography
          id="binder-page-counter"
          variant="body2"
          sx={{
            minWidth: 80,
            textAlign: 'center',
            fontWeight: 500,
            color: 'text.primary'
          }}
        >
          {currentPage} / {totalPages}
        </Typography>

        <Tooltip title={viewMode === 'book' ? 'Next spread (→)' : 'Next page (→)'}>
          <span>
            <IconButton
              id="binder-nav-next"
              onClick={onNext}
              disabled={!canGoNext}
              size="small"
              aria-label="Next page"
            >
              <NavigateNextIcon />
            </IconButton>
          </span>
        </Tooltip>
      </Box>

      {/* Keyboard hint */}
      <Typography
        variant="caption"
        sx={{
          color: 'text.disabled',
          display: { xs: 'none', md: 'block' }
        }}
      >
        ←/→ z/x ,/. to navigate
      </Typography>
    </Box>
  );
};
