import React from 'react';
import { Box, CircularProgress, Typography } from '@mui/material';

interface AccessibleLoadingIndicatorProps {
  message?: string;
  size?: number | 'small' | 'medium' | 'large';
  showMessage?: boolean;
  'aria-label'?: string;
}

/**
 * Accessible loading indicator with proper ARIA attributes and screen reader support
 */
export const AccessibleLoadingIndicator: React.FC<AccessibleLoadingIndicatorProps> = ({
  message = 'Loading content, please wait...',
  size = 'medium',
  showMessage = true,
  'aria-label': ariaLabel
}) => {
  const loadingId = React.useId();
  
  return (
    <Box
      role="status"
      aria-live="polite"
      aria-busy="true"
      aria-labelledby={showMessage ? `${loadingId}-text` : undefined}
      aria-label={!showMessage ? (ariaLabel || message) : undefined}
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        gap: 2,
        py: 4
      }}
    >
      <CircularProgress 
        size={typeof size === 'string' ? size : size} 
        aria-hidden="true"
        sx={{
          color: 'primary.main'
        }}
      />
      {showMessage && (
        <Typography 
          id={`${loadingId}-text`}
          variant="body2" 
          color="text.secondary"
          sx={{ textAlign: 'center' }}
        >
          {message}
        </Typography>
      )}
    </Box>
  );
};