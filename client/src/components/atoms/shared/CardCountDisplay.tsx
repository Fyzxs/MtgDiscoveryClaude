import React from 'react';
import Typography from '../Typography';

interface CardCountDisplayProps {
  count: number;
  /** Compact mode for smaller cards */
  compact?: boolean;
}

export const CardCountDisplay: React.FC<CardCountDisplayProps> = ({ count, compact = false }) => {
  return (
    <Typography
      variant="body2"
      color="text.secondary"
      sx={{
        fontSize: compact ? '0.625rem' : '0.875rem',
      }}
    >
      {compact ? `${count}` : `${count} cards`}
    </Typography>
  );
};