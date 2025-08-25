import React, { useState } from 'react';
import { CardMedia, Box } from '@mui/material';

interface CardImageProps {
  imageUrl?: string;
  cardName?: string;
  className?: string;
}

export const CardImage: React.FC<CardImageProps> = ({ 
  imageUrl, 
  cardName,
  className 
}) => {
  const [imageLoaded, setImageLoaded] = useState(false);

  return (
    <Box sx={{ position: 'relative', pointerEvents: 'none' }} className={className}>
      {/* Card Back Placeholder - always visible initially */}
      <CardMedia
        component="img"
        image="/cardback.jpeg"
        alt="Magic card back"
        sx={{
          width: '100%',
          aspectRatio: '745/1040',
          objectFit: 'cover',
          position: 'absolute',
          top: 0,
          left: 0,
          zIndex: 1,
          pointerEvents: 'none'
        }}
      />
      
      {/* Actual Card Image - fades in over placeholder */}
      <CardMedia
        component="img"
        image={imageUrl || ''}
        alt={cardName || 'Magic card'}
        sx={{
          width: '100%',
          aspectRatio: '745/1040',
          objectFit: 'cover',
          position: 'relative',
          zIndex: 2,
          opacity: imageLoaded ? 1 : 0,
          transition: 'opacity 0.8s ease-in-out',
          pointerEvents: 'none'
        }}
        onLoad={() => {
          // Add delay to see the card back placeholder
          setTimeout(() => {
            setImageLoaded(true);
          }, 500);
        }}
        loading="lazy"
      />
    </Box>
  );
};