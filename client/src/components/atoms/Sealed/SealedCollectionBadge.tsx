import React from 'react';
import { Box, Typography, CircularProgress, alpha, useTheme } from '@mui/material';

interface SealedCollectionBadgeProps {
  quantity: number;
  onClick?: () => void;
  isUpdating?: boolean;
}

export const SealedCollectionBadge: React.FC<SealedCollectionBadgeProps> = ({
  quantity,
  onClick,
  isUpdating = false
}) => {
  const theme = useTheme();

  if (quantity === 0 && isUpdating === false) {
    return null;
  }

  const handleKeyDown = (event: React.KeyboardEvent): void => {
    if (onClick && (event.key === 'Enter' || event.key === ' ')) {
      event.preventDefault();
      onClick();
    }
  };

  return (
    <Box
      onClick={onClick}
      onKeyDown={onClick ? handleKeyDown : undefined}
      role={onClick ? 'button' : undefined}
      tabIndex={onClick ? 0 : undefined}
      aria-label={onClick ? 'Update collection quantity' : `${quantity} in collection`}
      sx={{
        position: 'absolute',
        top: { xs: 6, sm: 8 },
        left: { xs: 6, sm: 8 },
        zIndex: 3,
        bgcolor: alpha(theme.palette.success.main, 0.9),
        color: 'white',
        borderRadius: 1,
        px: 0.75,
        py: 0.25,
        fontSize: { xs: '0.65rem', sm: '0.7rem' },
        fontWeight: 700,
        cursor: onClick ? 'pointer' : 'default',
        display: 'flex',
        alignItems: 'center',
        gap: 0.5,
        minWidth: 28,
        justifyContent: 'center',
        transition: 'all 0.2s ease',
        '&:hover': onClick ? {
          bgcolor: alpha(theme.palette.success.dark, 0.95),
          transform: 'scale(1.05)'
        } : {},
        '&:focus': onClick ? {
          outline: `2px solid ${theme.palette.success.light}`,
          outlineOffset: 2
        } : {}
      }}
    >
      {isUpdating ? (
        <CircularProgress size={12} sx={{ color: 'white' }} />
      ) : (
        <Typography
          component="span"
          sx={{
            fontSize: 'inherit',
            fontWeight: 'inherit',
            lineHeight: 1
          }}
        >
          [{quantity}]
        </Typography>
      )}
    </Box>
  );
};
