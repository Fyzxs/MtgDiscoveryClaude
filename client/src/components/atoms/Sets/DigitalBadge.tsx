import React from 'react';
import { Chip } from '@mui/material';

interface DigitalBadgeProps {
  show: boolean;
}

export const DigitalBadge: React.FC<DigitalBadgeProps> = ({ show }) => {
  if (!show) {
    return null;
  }

  return (
    <Chip
      label="Digital"
      size="small"
      color="info"
      variant="filled"
    />
  );
};