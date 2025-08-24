import React, { useState } from 'react';
import { Box, Typography, Skeleton, CardMedia } from '@mui/material';
import type { ImageUris } from '../../types/card';

interface CardImageProps {
  imageUris?: ImageUris;
  cardName: string;
  size?: 'small' | 'normal' | 'large';
  className?: string;
  onClick?: () => void;
}

export const CardImage: React.FC<CardImageProps> = ({ 
  imageUris, 
  cardName,
  size = 'normal',
  className = '',
  onClick
}) => {
  const [isLoading, setIsLoading] = useState(true);
  const [hasError, setHasError] = useState(false);

  const getImageUrl = (): string => {
    if (!imageUris) return '';
    
    switch (size) {
      case 'small':
        return imageUris.small || imageUris.normal || '';
      case 'large':
        return imageUris.large || imageUris.normal || '';
      default:
        return imageUris.normal || '';
    }
  };

  const getSizeValue = () => {
    switch (size) {
      case 'small': return 128; // w-32
      case 'normal': return 256; // w-64
      case 'large': return 384; // w-96
      default: return 256;
    }
  };

  const imageUrl = getImageUrl();
  const sizeValue = getSizeValue();

  if (!imageUrl || hasError) {
    return (
      <Box 
        className={className}
        sx={{
          width: sizeValue,
          aspectRatio: '745/1040',
          bgcolor: 'grey.800',
          borderRadius: 2,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          color: 'grey.500'
        }}
      >
        <Box sx={{ textAlign: 'center', p: 2 }}>
          <Typography variant="h3" sx={{ mb: 1 }}>🎴</Typography>
          <Typography variant="body2" sx={{ mb: 0.5 }}>{cardName}</Typography>
          <Typography variant="caption">Image not available</Typography>
        </Box>
      </Box>
    );
  }

  return (
    <Box className={className} sx={{ position: 'relative', width: sizeValue }}>
      {isLoading && (
        <Skeleton 
          variant="rectangular" 
          sx={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            aspectRatio: '745/1040',
            borderRadius: 2
          }}
        />
      )}
      <CardMedia
        component="img"
        image={imageUrl}
        alt={cardName}
        sx={{
          width: sizeValue,
          aspectRatio: '745/1040',
          borderRadius: 2,
          boxShadow: 3,
          cursor: onClick ? 'pointer' : 'default',
          transition: 'transform 0.2s ease',
          opacity: isLoading ? 0 : 1,
          '&:hover': onClick ? {
            transform: 'scale(1.05)'
          } : {}
        }}
        onLoad={() => setIsLoading(false)}
        onError={() => {
          setIsLoading(false);
          setHasError(true);
        }}
        onClick={onClick}
      />
    </Box>
  );
};