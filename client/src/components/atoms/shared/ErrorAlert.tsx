import React from 'react';
import Alert from '../Alert';
import Typography from '../Typography';
import Box from '../Box';

interface ErrorAlertProps {
  message: string;
  severity?: 'error' | 'warning' | 'info';
  variant?: 'filled' | 'outlined' | 'standard';
  centered?: boolean;
  fullWidth?: boolean;
  sx?: object;
}

/**
 * Standardized error alert component with consistent styling and messaging
 */
export const ErrorAlert: React.FC<ErrorAlertProps> = ({
  message,
  severity = 'error',
  variant = 'standard',
  centered = false,
  fullWidth = true,
  sx = {}
}) => {
  const content = (
    <Alert 
      severity={severity} 
      variant={variant}
      sx={{ 
        mb: 2,
        ...(fullWidth && { width: '100%' }),
        ...sx 
      }}
    >
      {message}
    </Alert>
  );

  if (centered) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 2 }}>
        {content}
      </Box>
    );
  }

  return content;
};

/**
 * Simplified error text component for inline error messages
 */
export const ErrorText: React.FC<{ 
  message: string; 
  centered?: boolean;
  variant?: 'body1' | 'body2' | 'caption';
}> = ({ 
  message, 
  centered = true,
  variant = 'body2' 
}) => (
  <Typography 
    variant={variant} 
    color="error" 
    sx={{ 
      textAlign: centered ? 'center' : 'left', 
      py: 2 
    }}
  >
    {message}
  </Typography>
);