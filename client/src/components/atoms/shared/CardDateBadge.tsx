import React from 'react';
import Typography from '../Typography';
import { formatReleaseDate } from '../../../utils/dateFormatters';

interface CardDateBadgeProps {
  date: string;
}

/**
 * Displays a formatted release date with subtle styling.
 * Used on card overlays to show when a card was released.
 */
export const CardDateBadge: React.FC<CardDateBadgeProps> = ({ date }) => {
  return (
    <Typography
      variant="caption"
      sx={{
        fontSize: '0.625rem',
        color: 'grey.300',
        bgcolor: 'rgba(0, 0, 0, 0.6)',
        px: 0.75,
        py: 0.25,
        borderRadius: 1
      }}
    >
      {formatReleaseDate(date)}
    </Typography>
  );
};
