import React from 'react';
import { Chip } from '@mui/material';

interface FoilOnlyBadgeProps {
  show: boolean;
}

export const FoilOnlyBadge: React.FC<FoilOnlyBadgeProps> = ({ show }) => {
  if (!show) {
    return null;
  }

  return (
    <Chip
      label="Foil Only"
      size="small"
      color="warning"
      variant="filled"
      sx={{ fontWeight: 500 }}
    />
  );
};