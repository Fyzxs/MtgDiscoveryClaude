import React from 'react';
import { Box } from '@mui/material';

interface SetIconDisplayProps {
  iconSvgUri?: string;
  setName: string;
  borderColor: string;
}

export const SetIconDisplay: React.FC<SetIconDisplayProps> = ({ 
  iconSvgUri, 
  setName, 
  borderColor 
}) => {
  if (!iconSvgUri) {
    return null;
  }

  return (
    <Box
      sx={{
        width: '200px',
        height: '200px',
        border: `3px solid ${borderColor}`,
        borderRadius: '8px',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        backgroundColor: 'rgba(255, 255, 255, 0.02)',
        mb: 1,
        transition: 'border-color 0.3s ease',
      }}
    >
      <Box
        component="img"
        src={iconSvgUri}
        alt={`${setName} icon`}
        sx={{
          maxWidth: '180px',
          maxHeight: '180px',
          width: 'auto',
          height: 'auto',
          objectFit: 'contain',
          filter: 'brightness(0) invert(1) drop-shadow(0 4px 8px rgba(0,0,0,0.5))',
        }}
      />
    </Box>
  );
};