import React from 'react';
import { Chip } from '@mui/material';
import { getSetTypeColor } from '../../../constants/setTypeColors';

interface SetTypeBadgeProps {
  setType: string;
}

export const SetTypeBadge: React.FC<SetTypeBadgeProps> = ({ setType }) => {
  const formatSetType = (type: string) => {
    return type.replace(/_/g, ' ').replace(/\b\w/g, (l) => l.toUpperCase());
  };

  const color = getSetTypeColor(setType);

  return (
    <Chip
      label={formatSetType(setType)}
      size="small"
      variant="filled"
      sx={{
        backgroundColor: color,
        color: 'white',
        fontWeight: 600,
        '&:hover': {
          backgroundColor: color,
        }
      }}
    />
  );
};