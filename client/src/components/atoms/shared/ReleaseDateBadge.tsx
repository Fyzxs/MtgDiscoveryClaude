import React from 'react';
import { Chip } from '@mui/material';
import { format } from 'date-fns';

interface ReleaseDateBadgeProps {
  date: string;
}

export const ReleaseDateBadge: React.FC<ReleaseDateBadgeProps> = ({ date }) => {
  const formatReleaseDate = (dateString: string) => {
    try {
      const dateObj = new Date(dateString);
      return format(dateObj, 'MMM yyyy');
    } catch {
      return dateString;
    }
  };

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