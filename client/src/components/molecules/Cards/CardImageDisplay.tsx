import React, { useState } from 'react';
import { Box, IconButton, Tooltip } from '@mui/material';
import FlipIcon from '@mui/icons-material/Flip';
import type { Card } from '../../../types/card';

interface CardImageDisplayProps {
  card: Card;
  size?: 'small' | 'normal' | 'large';
  showFlipButton?: boolean;
  borderRadius?: string;
  maxWidth?: string | number;
  maxHeight?: string | number;
  className?: string;
  onClick?: () => void;
  sx?: any;
}

export const CardImageDisplay: React.FC<CardImageDisplayProps> = ({
  card,
  size = 'normal',
  showFlipButton = true,
  borderRadius = '4.55%',
  maxWidth = '100%',
  maxHeight = '100%',
  className,
  onClick,
  sx = {}
}) => {
  const [currentFaceIndex, setCurrentFaceIndex] = useState(0);
  const [isFlipping, setIsFlipping] = useState(false);
  
  // Check if this is a double-faced card
  const isDoubleFaced = card.cardFaces && card.cardFaces.length > 1;
  const currentFace = isDoubleFaced ? card.cardFaces[currentFaceIndex] : null;
  
  // Determine which image to show
  const getImageUrl = () => {
    if (isDoubleFaced && currentFace?.imageUris) {
      // Use the current face's image
      if (size === 'large') return currentFace.imageUris.large || currentFace.imageUris.normal || currentFace.imageUris.small || '';
      if (size === 'small') return currentFace.imageUris.small || currentFace.imageUris.normal || '';
      return currentFace.imageUris.normal || currentFace.imageUris.large || currentFace.imageUris.small || '';
    }
    
    // Use the main card image
    if (!card.imageUris) return '';
    if (size === 'large') return card.imageUris.large || card.imageUris.normal || card.imageUris.small || '';
    if (size === 'small') return card.imageUris.small || card.imageUris.normal || '';
    return card.imageUris.normal || card.imageUris.large || card.imageUris.small || '';
  };
  
  const handleFlip = (e: React.MouseEvent) => {
    e.stopPropagation(); // Prevent triggering parent onClick
    if (!isDoubleFaced || isFlipping) return;
    
    setIsFlipping(true);
    setTimeout(() => {
      setCurrentFaceIndex(prev => (prev + 1) % card.cardFaces!.length);
      setIsFlipping(false);
    }, 300); // Half of the animation duration
  };

  return (
    <Box
      className={className}
      sx={{
        position: 'relative',
        display: 'inline-block',
        maxWidth,
        maxHeight,
        perspective: '1000px',
        ...sx
      }}
      onClick={onClick}
    >
      <Box
        sx={{
          position: 'relative',
          transformStyle: 'preserve-3d',
          transition: 'transform 0.6s',
          transform: isFlipping ? 'rotateY(180deg)' : 'rotateY(0deg)',
          width: '100%',
          height: '100%'
        }}
      >
        <Box
          component="img"
          src={getImageUrl()}
          alt={card.name}
          loading="lazy"
          sx={{
            maxWidth: '100%',
            maxHeight: '100%',
            width: 'auto',
            height: 'auto',
            borderRadius,
            display: 'block',
            backfaceVisibility: 'hidden'
          }}
        />
      </Box>
      
      {/* Flip button for double-faced cards */}
      {isDoubleFaced && showFlipButton && (
        <Tooltip title={`Show ${currentFaceIndex === 0 ? 'back' : 'front'} face`}>
          <IconButton
            size="small"
            onClick={handleFlip}
            disabled={isFlipping}
            sx={{
              position: 'absolute',
              top: 16,
              left: 16,
              bgcolor: 'rgba(25, 118, 210, 0.8)',
              color: 'white',
              zIndex: 10,
              '&:hover': {
                bgcolor: 'rgba(25, 118, 210, 0.95)',
              },
              '&.Mui-disabled': {
                color: 'rgba(255, 255, 255, 0.5)',
              }
            }}
            aria-label="flip card"
          >
            <FlipIcon fontSize="small" />
          </IconButton>
        </Tooltip>
      )}
    </Box>
  );
};