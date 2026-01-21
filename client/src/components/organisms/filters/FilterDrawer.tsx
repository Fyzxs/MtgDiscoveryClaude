import React from 'react';
import {
  Box,
  Drawer,
  IconButton,
  Typography,
  Button
} from '../../atoms';
import { CloseIcon } from '../../atoms';
import { FilterPanel } from './FilterPanel';
import type { FilterPanelConfig } from '../../../types/filters';
import { useTheme } from '../../atoms';

interface FilterDrawerProps {
  open: boolean;
  onClose: () => void;
  config: FilterPanelConfig;
  /** Title shown in drawer header */
  title?: string;
  /** Callback when "Apply Filters" is clicked */
  onApply?: () => void;
  /** Callback when "Clear All" is clicked */
  onClear?: () => void;
  /** Number of active filters */
  activeFilterCount?: number;
}

/**
 * Mobile-friendly filter drawer that slides in from the bottom.
 * Contains a vertical FilterPanel layout optimized for touch.
 */
export const FilterDrawer: React.FC<FilterDrawerProps> = ({
  open,
  onClose,
  config,
  title = 'Filters',
  onApply,
  onClear,
  activeFilterCount = 0
}) => {
  const theme = useTheme();

  const handleApply = () => {
    onApply?.();
    onClose();
  };

  return (
    <Drawer
      anchor="bottom"
      open={open}
      onClose={onClose}
      PaperProps={{
        sx: {
          maxHeight: '85vh',
          borderTopLeftRadius: theme.mtg.mobile.sheetBorderRadius,
          borderTopRightRadius: theme.mtg.mobile.sheetBorderRadius,
          bgcolor: 'background.paper',
        }
      }}
    >
      {/* Drag handle */}
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          pt: 1,
          pb: 0.5
        }}
      >
        <Box
          sx={{
            width: 40,
            height: 4,
            bgcolor: 'grey.600',
            borderRadius: 2
          }}
        />
      </Box>

      {/* Header */}
      <Box sx={{
        px: 2,
        py: 1.5,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        borderBottom: 1,
        borderColor: 'divider',
        minHeight: 56,
      }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <Typography variant="h6" fontWeight="bold">
            {title}
          </Typography>
          {activeFilterCount > 0 && (
            <Box
              sx={{
                bgcolor: 'primary.main',
                color: 'primary.contrastText',
                borderRadius: '50%',
                width: 24,
                height: 24,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: '0.75rem',
                fontWeight: 'bold'
              }}
            >
              {activeFilterCount}
            </Box>
          )}
        </Box>
        <IconButton
          onClick={onClose}
          sx={{ minWidth: 44, minHeight: 44 }}
        >
          <CloseIcon />
        </IconButton>
      </Box>

      {/* Filter content */}
      <Box sx={{
        overflow: 'auto',
        flex: 1,
        px: 2,
        py: 2,
      }}>
        <FilterPanel
          config={config}
          layout="vertical"
          spacing={2}
          sx={{ mb: 0 }}
        />
      </Box>

      {/* Footer actions */}
      <Box sx={{
        px: 2,
        py: 2,
        borderTop: 1,
        borderColor: 'divider',
        display: 'flex',
        gap: 2,
        // Safe area padding for mobile
        pb: 'calc(16px + env(safe-area-inset-bottom, 0px))',
      }}>
        {onClear && (
          <Button
            variant="outlined"
            onClick={onClear}
            fullWidth
            sx={{ minHeight: 48 }}
            disabled={activeFilterCount === 0}
          >
            Clear All
          </Button>
        )}
        <Button
          variant="contained"
          onClick={handleApply}
          fullWidth
          sx={{ minHeight: 48 }}
        >
          Apply Filters
        </Button>
      </Box>
    </Drawer>
  );
};
