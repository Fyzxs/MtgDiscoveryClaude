import React from 'react';
import { Box, Typography } from '@mui/material';

interface CollectorNumberProps {
  number?: string;
  setCode?: string;
  className?: string;
}

export const CollectorNumber: React.FC<CollectorNumberProps> = ({ 
  number, 
  setCode,
  className = '' 
}) => {
  if (!number) return null;

  return (
    <Box 
      className={className}
      sx={{ 
        display: 'flex', 
        alignItems: 'center', 
        gap: 0.5, 
        color: 'grey.400',
        fontSize: '0.875rem'
      }}
    >
      <Typography 
        variant="caption" 
        sx={{ 
          fontFamily: 'monospace',
          fontSize: 'inherit'
        }}
      >
        #{number}
      </Typography>
      {setCode && (
        <Typography 
          variant="caption" 
          sx={{ 
            fontSize: '0.75rem', 
            textTransform: 'uppercase', 
            color: 'grey.500'
          }}
        >
          • {setCode}
        </Typography>
      )}
    </Box>
  );
};