import React from 'react';
import { Chip, Typography } from '../../../atoms';
import type { SxProps, Theme } from '@mui/material';
import { formatReleaseDate } from '../../../../utils/dateFormatters';

interface DateBadgeProps {
  date: string;
  /** Variant determines styling */
  variant?: 'chip' | 'text';
  /** Compact mode for smaller display */
  compact?: boolean;
  /** Additional styles */
  sx?: SxProps<Theme>;
}

/**
 * Unified date badge for displaying formatted release dates.
 *
 * Variants:
 * - 'chip': Prominent Chip badge (formerly SetDateBadge)
 * - 'text': Subtle Typography overlay (formerly ItemDateBadge)
 *
 * Usage:
 * - Set cards: <DateBadge date="2023-01-01" variant="chip" />
 * - Item overlays: <DateBadge date="2023-01-01" variant="text" />
 */
export const DateBadge: React.FC<DateBadgeProps> = ({
  date,
  variant = 'chip',
  compact = false,
  sx
}) => {
  const formattedDate = formatReleaseDate(date);

  if (variant === 'text') {
    return (
      <Typography
        variant="caption"
        sx={{
          fontSize: '0.625rem',
          color: 'grey.300',
          bgcolor: 'rgba(0, 0, 0, 0.6)',
          px: 0.75,
          py: 0.25,
          borderRadius: 1,
          ...sx
        }}
      >
        {formattedDate}
      </Typography>
    );
  }

  return (
    <Chip
      label={formattedDate}
      size="small"
      variant="filled"
      sx={{
        fontWeight: 600,
        fontSize: compact ? '0.5625rem' : undefined,
        height: compact ? 18 : undefined,
        backgroundColor: 'rgba(0, 0, 0, 0.9)',
        color: 'white',
        border: compact ? '1px solid rgba(255, 255, 255, 0.2)' : '2px solid rgba(255, 255, 255, 0.2)',
        '& .MuiChip-label': {
          px: compact ? 0.5 : 1,
        },
        ...sx
      }}
    />
  );
};
