import React from 'react';
import Chip from '../Chip';
import { formatReleaseDate } from '../../../utils/dateFormatters';

interface SetDateBadgeProps {
  date: string;
  /** Compact mode for smaller cards */
  compact?: boolean;
}

/**
 * Displays a formatted release date as a prominent chip badge.
 * Used on Set cards to show when a set was released.
 */
export const SetDateBadge: React.FC<SetDateBadgeProps> = ({ date, compact = false }) => {
  return (
    <Chip
      label={formatReleaseDate(date)}
      size="small"
      variant="filled"
      sx={{
        fontWeight: 600,
        fontSize: compact ? '0.625rem' : undefined,
        height: compact ? 20 : undefined,
        backgroundColor: 'rgba(0, 0, 0, 0.9)',
        color: 'white',
        border: compact ? '1px solid rgba(255, 255, 255, 0.2)' : '2px solid rgba(255, 255, 255, 0.2)',
        '& .MuiChip-label': {
          px: compact ? 0.75 : 1,
        },
      }}
    />
  );
};
