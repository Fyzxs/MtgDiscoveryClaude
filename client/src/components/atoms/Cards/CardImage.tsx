import React, { useState } from 'react';
import { CardMedia, Box, Alert, Typography } from '@mui/material';
import ImageNotSupportedIcon from '@mui/icons-material/ImageNotSupported';

interface CardImageProps {
  imageUrl?: string;
  cardName?: string;
  className?: string;
  showErrorDetails?: boolean;
}

export const CardImage: React.FC<CardImageProps> = ({ 
  imageUrl, 
  cardName,
  className,
  showErrorDetails = false
}) => {
  const [imageError, setImageError] = useState(false);
  const [imageLoading, setImageLoading] = useState(Boolean(imageUrl));

  const handleImageError = () => {
    setImageError(true);
    setImageLoading(false);
  };

  const handleImageLoad = () => {
    setImageLoading(false);
  };

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
      {/* Loading State */}
      {imageLoading && (
        <Box
          sx={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            backgroundColor: 'rgba(0, 0, 0, 0.1)',
            zIndex: 1
          }}
        >
          <Typography variant="caption" color="text.secondary">
            Loading...
          </Typography>
        </Box>
      )}

      {/* Error State */}
      {imageError && (
        <Box
          sx={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            backgroundColor: 'rgba(0, 0, 0, 0.05)',
            zIndex: 1,
            p: 2
          }}
        >
          <ImageNotSupportedIcon 
            sx={{ 
              fontSize: 48, 
              color: 'text.disabled', 
              mb: 1 
            }} 
          />
          <Typography 
            variant="caption" 
            color="text.secondary" 
            align="center"
          >
            Image unavailable
          </Typography>
          {showErrorDetails && (
            <Typography 
              variant="caption" 
              color="text.disabled" 
              align="center"
              sx={{ mt: 1 }}
            >
              Failed to load: {cardName}
            </Typography>
          )}
        </Box>
      )}

      {/* Actual Card Image - only render if we have a URL and no error */}
      {imageUrl && !imageError && (
        <CardMedia
          component="img"
          image={imageUrl}
          alt={cardName || 'Magic card'}
          onError={handleImageError}
          onLoad={handleImageLoad}
          sx={{
            width: '100%',
            height: '100%',
            objectFit: 'cover',
            position: 'absolute',
            top: 0,
            left: 0,
            pointerEvents: 'none',
            opacity: imageLoading ? 0 : 1,
            transition: 'opacity 0.3s ease-in-out'
          }}
          loading="lazy"
        />
      )}
    </Box>
  );
};