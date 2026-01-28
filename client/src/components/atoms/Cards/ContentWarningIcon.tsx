import React from 'react';
import Box from '../mui-wrappers/Box';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import type { SxProps, Theme } from '@mui/material';

interface ContentWarningIconProps {
  size?: 'small' | 'medium';
  sx?: SxProps<Theme>;
  iconSx?: SxProps<Theme>;
}

export const ContentWarningIcon: React.FC<ContentWarningIconProps> = ({
  size = 'medium',
  sx = {},
  iconSx = {},
}) => {
  const iconSize = size === 'small' ? 18 : 20;

  return (
    <Box
      component="span"
      sx={{
        display: 'inline-flex',
        alignItems: 'center',
        justifyContent: 'center',
        ...sx,
      }}
      title="Content Warning"
      aria-label="Content Warning"
    >
      <VisibilityOffIcon
        sx={{
          width: iconSize,
          height: iconSize,
          color: '#d97706',
          filter: 'drop-shadow(0 1px 3px rgba(0, 0, 0, 0.8)) drop-shadow(0 0 6px rgba(217, 119, 6, 0.4))',
          transition: 'all 0.2s ease-in-out',
          '&:hover': {
            filter: 'drop-shadow(0 2px 4px rgba(0, 0, 0, 0.9)) drop-shadow(0 0 10px rgba(217, 119, 6, 0.6))',
          },
          ...iconSx,
        }}
      />
    </Box>
  );
};
