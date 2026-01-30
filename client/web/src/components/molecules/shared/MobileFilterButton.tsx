import React from 'react';
import { Box, IconButton, Badge, Typography } from '../../atoms';
import { TuneIcon } from '../../atoms';

interface MobileFilterButtonProps {
  /** Number of active filters */
  activeCount?: number;
  /** Click handler to open filter drawer */
  onClick: () => void;
  /** Whether the button is disabled */
  disabled?: boolean;
  /** Show label text next to icon */
  showLabel?: boolean;
  /** Custom label text */
  label?: string;
}

/**
 * Mobile-friendly filter button that shows active filter count.
 * Designed for fixed positioning in mobile layouts.
 */
export const MobileFilterButton: React.FC<MobileFilterButtonProps> = ({
  activeCount = 0,
  onClick,
  disabled = false,
  showLabel = false,
  label = 'Filters'
}) => {

  // Simple icon button with badge
  if (!showLabel) {
    return (
      <IconButton
        onClick={onClick}
        disabled={disabled}
        sx={{
          minWidth: 48,
          minHeight: 48,
          bgcolor: activeCount > 0 ? 'primary.main' : 'grey.800',
          color: 'white',
          '&:hover': {
            bgcolor: activeCount > 0 ? 'primary.dark' : 'grey.700',
          },
          '&:disabled': {
            bgcolor: 'grey.900',
            color: 'grey.600',
          }
        }}
      >
        <Badge
          badgeContent={activeCount}
          color="error"
          invisible={activeCount === 0}
          sx={{
            '& .MuiBadge-badge': {
              right: -3,
              top: -3,
              fontSize: '0.625rem',
              minWidth: 16,
              height: 16,
            }
          }}
        >
          <TuneIcon />
        </Badge>
      </IconButton>
    );
  }

  // Button with label
  return (
    <Box
      component="button"
      onClick={onClick}
      disabled={disabled}
      sx={{
        display: 'flex',
        alignItems: 'center',
        gap: 1,
        px: 2,
        py: 1,
        minHeight: 44,
        bgcolor: activeCount > 0 ? 'primary.main' : 'grey.800',
        color: 'white',
        border: 'none',
        borderRadius: 2,
        cursor: disabled ? 'not-allowed' : 'pointer',
        '&:hover:not(:disabled)': {
          bgcolor: activeCount > 0 ? 'primary.dark' : 'grey.700',
        },
        '&:disabled': {
          bgcolor: 'grey.900',
          color: 'grey.600',
          opacity: 0.6,
        }
      }}
    >
      <Badge
        badgeContent={activeCount}
        color="error"
        invisible={activeCount === 0}
        sx={{
          '& .MuiBadge-badge': {
            right: -6,
            top: -6,
            fontSize: '0.625rem',
            minWidth: 18,
            height: 18,
          }
        }}
      >
        <TuneIcon />
      </Badge>
      <Typography
        variant="body2"
        sx={{
          fontWeight: 500,
          fontSize: '0.875rem'
        }}
      >
        {label}
        {activeCount > 0 && ` (${activeCount})`}
      </Typography>
    </Box>
  );
};
