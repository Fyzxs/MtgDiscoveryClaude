import React from 'react';
import Box from '../mui-wrappers/Box';
import { useTheme } from '@mui/material';
import ShieldIcon from '@mui/icons-material/Shield';
import type { SxProps, Theme } from '@mui/material';

interface ReservedListShieldProps {
  size?: 'small' | 'medium';
  sx?: SxProps<Theme>;
  iconSx?: SxProps<Theme>;
}

export const ReservedListShield: React.FC<ReservedListShieldProps> = ({
  size = 'medium',
  sx = {},
  iconSx = {},
}) => {
  const theme = useTheme();

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
      title="Reserved List"
      aria-label="Reserved List"
    >
      <ShieldIcon
        sx={{
          width: iconSize,
          height: iconSize,
          color: theme.palette.reservedList.brass,
          filter: `drop-shadow(0 1px 3px rgba(0, 0, 0, 0.8)) drop-shadow(0 0 6px ${theme.palette.reservedList.gold})`,
          transition: 'all 0.2s ease-in-out',
          '&:hover': {
            filter: `drop-shadow(0 2px 4px rgba(0, 0, 0, 0.9)) drop-shadow(0 0 10px ${theme.palette.reservedList.gold})`,
          },
          ...iconSx,
        }}
      />
    </Box>
  );
};
