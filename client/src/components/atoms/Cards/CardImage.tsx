import React from 'react';
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
  return (
    <Box 
      sx={{ 
        position: 'relative', 
        pointerEvents: 'none',
        backgroundImage: 'url(/cardback.jpeg)',
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        aspectRatio: '745/1040'
      }} 
      className={className}
    >
      {/* Actual Card Image - only render if we have a URL */}
      {imageUrl && (
        <CardMedia
          component="img"
          image={imageUrl}
          alt={cardName || 'Magic card'}
          sx={{
            width: '100%',
            height: '100%',
            objectFit: 'cover',
            position: 'absolute',
            top: 0,
            left: 0,
            pointerEvents: 'none'
          }}
          loading="lazy"
        />
      )}
    </Box>
  );
};