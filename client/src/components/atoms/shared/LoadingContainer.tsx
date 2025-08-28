import React from 'react';
import { Box, CircularProgress, Typography } from '@mui/material';

interface LoadingContainerProps {
  size?: 'small' | 'medium' | 'large';
  message?: string;
  centerVertically?: boolean;
  py?: number;
}

/**
 * Standardized loading spinner container with consistent styling
 */
export const LoadingContainer: React.FC<LoadingContainerProps> = ({
  size = 'medium',
  message,
  centerVertically = true,
  py = 2
}) => {
  const getSpinnerSize = () => {
    switch (size) {
      case 'small': return 20;
      case 'large': return 48;
      default: return 24;
    }
  };

  const containerSx = {
    display: 'flex',
    flexDirection: 'column' as const,
    alignItems: 'center',
    ...(centerVertically && { justifyContent: 'center' }),
    py: py,
    gap: message ? 1 : 0
  };

  return (
    <Box sx={containerSx}>
      <CircularProgress size={getSpinnerSize()} />
      {message && (
        <Typography variant="body2" color="text.secondary" textAlign="center">
          {message}
        </Typography>
      )}
    </Box>
  );
};