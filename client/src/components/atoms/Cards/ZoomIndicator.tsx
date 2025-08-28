import React from 'react';
import { IconButton } from '@mui/material';
import { ZoomIn } from '@mui/icons-material';

interface ZoomIndicatorProps {
  onZoomClick: (e: React.MouseEvent) => void;
  className?: string;
}

export const ZoomIndicator: React.FC<ZoomIndicatorProps> = ({ 
  onZoomClick,
  className 
}) => {
  return (
    <IconButton
      tabIndex={-1}
      onClick={onZoomClick}
      onMouseDown={(e) => {
        e.stopPropagation();
      }}
      sx={{
        position: 'absolute',
        top: 12,
        right: 12,
        bgcolor: 'rgba(0, 0, 0, 0.15)',
        color: 'rgba(255, 255, 255, 0.4)',
        zIndex: 15,
        opacity: 0,
        transform: 'scale(0.9)',
        transition: 'all 0.3s ease-in-out',
        width: 70, // Quarter of the card width (280px)
        height: 70,
        borderRadius: 3,
        '&:hover': {
          bgcolor: 'rgba(0, 0, 0, 0.6)',
          color: 'white',
          transform: 'scale(1.02)',
        }
      }}
      className={`zoom-indicator ${className || ''}`}
    >
      <ZoomIn sx={{ fontSize: '3rem' }} />
    </IconButton>
  );
};