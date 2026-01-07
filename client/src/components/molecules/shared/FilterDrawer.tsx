import React from 'react';
import {
  SwipeableDrawer,
  Box,
  Typography,
  IconButton,
  Button,
  Divider,
  Badge,
} from '../../atoms';
import { CloseIcon, FilterListIcon } from '../../atoms/Icons';
import { touchTargetStyles } from '../../../styles/touchTargets';

interface FilterDrawerProps {
  open: boolean;
  onOpen: () => void;
  onClose: () => void;
  children: React.ReactNode;
  activeFilterCount?: number;
  title?: string;
}

/**
 * Mobile filter drawer that slides up from bottom
 * Provides a clean mobile UX for filter panels
 */
export const FilterDrawer: React.FC<FilterDrawerProps> = ({
  open,
  onOpen,
  onClose,
  children,
  activeFilterCount = 0,
  title = 'Filters',
}) => {
  return (
    <SwipeableDrawer
      anchor="bottom"
      open={open}
      onClose={onClose}
      onOpen={onOpen}
      disableSwipeToOpen={false}
      swipeAreaWidth={20}
      ModalProps={{
        keepMounted: true, // Better mobile performance
      }}
      PaperProps={{
        sx: {
          maxHeight: '85vh',
          borderTopLeftRadius: 16,
          borderTopRightRadius: 16,
        },
      }}
    >
      {/* Drag handle */}
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          pt: 1.5,
          pb: 0.5,
        }}
      >
        <Box
          sx={{
            width: 32,
            height: 4,
            bgcolor: 'grey.400',
            borderRadius: 2,
          }}
        />
      </Box>

      {/* Header */}
      <Box
        sx={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          px: 2,
          py: 1,
        }}
      >
        <Typography variant="h6" sx={{ fontWeight: 600 }}>
          {title}
          {activeFilterCount > 0 && (
            <Typography
              component="span"
              sx={{ ml: 1, color: 'primary.main', fontSize: '0.875rem' }}
            >
              ({activeFilterCount} active)
            </Typography>
          )}
        </Typography>
        <IconButton
          onClick={onClose}
          aria-label="Close filters"
          sx={{ ...touchTargetStyles.minimum }}
        >
          <CloseIcon />
        </IconButton>
      </Box>

      <Divider />

      {/* Filter content */}
      <Box
        sx={{
          overflow: 'auto',
          flex: 1,
          p: 2,
          pb: 'calc(16px + env(safe-area-inset-bottom, 0px))',
        }}
      >
        {children}
      </Box>

      {/* Apply button */}
      <Box
        sx={{
          p: 2,
          pt: 1,
          borderTop: '1px solid',
          borderColor: 'divider',
          pb: 'calc(16px + env(safe-area-inset-bottom, 0px))',
        }}
      >
        <Button
          fullWidth
          variant="contained"
          onClick={onClose}
          sx={{ ...touchTargetStyles.comfortable }}
        >
          Apply Filters
        </Button>
      </Box>
    </SwipeableDrawer>
  );
};

interface FilterDrawerTriggerProps {
  onClick: () => void;
  activeFilterCount?: number;
  label?: string;
}

/**
 * Button to trigger the filter drawer
 */
export const FilterDrawerTrigger: React.FC<FilterDrawerTriggerProps> = ({
  onClick,
  activeFilterCount = 0,
  label = 'Filter',
}) => {
  return (
    <Button
      variant="outlined"
      onClick={onClick}
      startIcon={
        activeFilterCount > 0 ? (
          <Badge badgeContent={activeFilterCount} color="primary" sx={{ '& .MuiBadge-badge': { fontSize: '0.625rem' } }}>
            <FilterListIcon />
          </Badge>
        ) : (
          <FilterListIcon />
        )
      }
      sx={{
        ...touchTargetStyles.minimum,
        minWidth: 'auto',
        px: 2,
      }}
    >
      {label}
    </Button>
  );
};
