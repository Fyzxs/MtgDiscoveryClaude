import React from 'react';
import Typography from '../Typography';
import type { SxProps, Theme } from '@mui/material';
import { formatReleaseDate } from '../../../utils/dateFormatters';

interface ItemDateBadgeProps {
  date: string;
  sx?: SxProps<Theme>;
}

/**
 * Displays a formatted release date with subtle styling.
 * Used on card and sealed product overlays to show when an item was released.
 * Can be styled via sx prop for different contexts.
 */
export const ItemDateBadge: React.FC<ItemDateBadgeProps> = ({ date, sx = {} }) => {
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
      {formatReleaseDate(date)}
    </Typography>
  );
};
