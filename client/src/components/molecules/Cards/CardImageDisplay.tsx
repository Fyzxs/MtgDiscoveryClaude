import React, { useState, useEffect } from 'react';
import { Box, IconButton, Tooltip, Skeleton } from '@mui/material';
import FlipIcon from '@mui/icons-material/Flip';
import type { Card } from '../../../types/card';
import { imageCache } from '../../../utils/imageCache';

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

// Default Magic card back image URL - using local image
const CARD_BACK_URL = '/cardback.jpeg';

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
  
  const imageUrl = getImageUrl();
  const [imageLoaded, setImageLoaded] = useState(() => {
    // Check global cache if this image was already loaded
    return imageUrl ? imageCache.isLoaded(imageUrl) : true;
  });
  
  // Preload image when URL changes
  useEffect(() => {
    if (!imageUrl) {
      setImageLoaded(true);
      return;
    }

    // Check if already loaded in global cache
    if (imageCache.isLoaded(imageUrl)) {
      setImageLoaded(true);
      return;
    }

    // Preload the image
    imageCache.preload(imageUrl).then((success) => {
      if (success) {
        setImageLoaded(true);
      }
    });
  }, [imageUrl]);
  
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
        display: 'block',
        width: '100%',
        height: '100%',
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
          height: '100%',
          // Card back as background - ensure it shows
          backgroundImage: `url(${CARD_BACK_URL})`,
          backgroundSize: 'contain',
          backgroundPosition: 'center',
          backgroundRepeat: 'no-repeat',
          borderRadius,
          minHeight: '200px' // Ensure minimum height
        }}
      >
        
        {/* Actual image */}
        {imageUrl && (
          <Box
            component="img"
            src={imageUrl}
            alt={card.name}
            loading="eager" // Changed from lazy to eager to prevent reload issues
            onLoad={() => {
              imageCache.markLoaded(imageUrl);
              setImageLoaded(true);
            }}
            onError={() => {
              // Don't mark as loaded in cache on error
              setImageLoaded(true); // But still show card back
            }}
            sx={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            objectFit: 'contain',
            borderRadius,
            display: 'block',
            backfaceVisibility: 'hidden',
            opacity: imageLoaded ? 1 : 0,
            transition: 'opacity 0.3s ease-in-out',
            backgroundColor: 'transparent'
            }}
          />
        )}
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