import React from 'react';
import { Box, Chip, Typography } from '../../atoms';
import { MobileFilterButton } from './MobileFilterButton';
import { ViewListIcon, ViewModuleIcon } from '../../atoms/Icons';

interface MobileFilterBarProps {
  /** Number of active filters */
  activeFilterCount: number;
  /** Click handler to open filter drawer */
  onFilterClick: () => void;
  /** Whether to show grouped card layout (optional - only needed if showGroupsToggle is true) */
  showGroups?: boolean;
  /** Handler for toggling grouped layout (optional - only needed if showGroupsToggle is true) */
  onShowGroupsChange?: (showGroups: boolean) => void;
  /** Whether the groups toggle should be visible (only when multiple groups exist) */
  showGroupsToggle?: boolean;
  /** Results summary text */
  resultsSummary?: string;
}

/**
 * Sticky filter bar for mobile/tablet views.
 * Contains filter button with badge and card groups toggle.
 * Stays fixed at top as user scrolls.
 */
export const MobileFilterBar: React.FC<MobileFilterBarProps> = ({
  activeFilterCount,
  onFilterClick,
  showGroups = false,
  onShowGroupsChange,
  showGroupsToggle = false,
  resultsSummary
}) => {
  return (
    <Box
      sx={{
        position: 'sticky',
        top: 0,
        zIndex: 1100,
        bgcolor: 'background.default',
        borderBottom: 1,
        borderColor: 'divider',
        px: 2,
        py: 1.5,
        mb: 2,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        gap: 2,
        // Ensure it's above cards but below modals/drawers
        backdropFilter: 'blur(8px)',
        backgroundColor: 'rgba(18, 18, 18, 0.95)',
      }}
    >
      {/* Left side: Results summary */}
      <Box sx={{ flex: 1, minWidth: 0 }}>
        {resultsSummary && (
          <Typography
            variant="body2"
            color="text.secondary"
            sx={{
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              whiteSpace: 'nowrap'
            }}
          >
            {resultsSummary}
          </Typography>
        )}
      </Box>

      {/* Right side: Controls */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, flexShrink: 0 }}>
        {/* Groups toggle chip */}
        {showGroupsToggle && onShowGroupsChange && (
          <Chip
            icon={showGroups ? <ViewModuleIcon /> : <ViewListIcon />}
            label={showGroups ? 'Grouped' : 'Flat'}
            onClick={() => onShowGroupsChange(!showGroups)}
            variant={showGroups ? 'filled' : 'outlined'}
            color={showGroups ? 'primary' : 'default'}
            size="small"
            sx={{
              minHeight: 36,
              '& .MuiChip-icon': {
                fontSize: '1.1rem'
              }
            }}
          />
        )}

        {/* Filter button */}
        <MobileFilterButton
          activeCount={activeFilterCount}
          onClick={onFilterClick}
          showLabel={false}
        />
      </Box>
    </Box>
  );
};
