import React from 'react';
import { Box } from '@mui/material';
import { getSetTypeColor } from '../../../constants/setTypeColors';

interface SetTypeAccentProps {
  /** The set type (e.g., 'expansion', 'commander', 'core') */
  setType: string;
  /** Height of the accent bar in pixels (default: 4) */
  height?: number;
}

export const SetTypeAccent: React.FC<SetTypeAccentProps> = ({
  setType,
  height = 4,
}) => {
  const color = getSetTypeColor(setType);

  return (
    <Box
      sx={{
        position: 'absolute',
        top: 0,
        left: 0,
        right: 0,
        height,
        backgroundColor: color,
        zIndex: 2,
      }}
    />
  );
};
