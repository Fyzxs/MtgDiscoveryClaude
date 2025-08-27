import React from 'react';
import { Chip } from '@mui/material';
import { formatReleaseDate } from '../../../utils/dateFormatters';

interface ReleaseDateBadgeProps {
  date: string;
}

export const ReleaseDateBadge: React.FC<ReleaseDateBadgeProps> = ({ date }) => {
  return (
    <Chip
      label={formatReleaseDate(date)}
      size="small"
      variant="filled"
      sx={{
        fontWeight: 600,
        backgroundColor: 'rgba(0, 0, 0, 0.9)',
        color: 'white',
        border: '2px solid rgba(255, 255, 255, 0.2)',
      }}
    />
  );
};