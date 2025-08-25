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
      label="Foil"
      size="small"
      variant="filled"
      sx={{
        fontWeight: 500,
        backgroundColor: '#ffe96aff',
        color: '#000000'
      }}
    />
  );
};