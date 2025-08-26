import React, { useState, useRef, useEffect } from 'react';
import { CardMedia, Box, CircularProgress } from '@mui/material';

interface CardImageProps {
  imageUrl?: string;
  cardName?: string;
  className?: string;
  lazyLoad?: boolean;
  placeholder?: string;
  onLoad?: () => void;
  onError?: () => void;
}

export const CardImage: React.FC<CardImageProps> = React.memo(({ 
  imageUrl, 
  cardName,
  className,
  lazyLoad = true,
  placeholder = '/cardback.jpeg',
  onLoad,
  onError
}) => {
  const [isVisible, setIsVisible] = useState(!lazyLoad);
  const [isLoading, setIsLoading] = useState(false);
  const [hasLoaded, setHasLoaded] = useState(false);
  const [hasError, setHasError] = useState(false);
  const imgRef = useRef<HTMLDivElement>(null);

  // Intersection Observer for lazy loading
  useEffect(() => {
    if (!lazyLoad || isVisible || !imgRef.current) return;

    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            setIsVisible(true);
            setIsLoading(true);
            observer.unobserve(entry.target);
          }
        });
      },
      {
        rootMargin: '50px', // Start loading 50px before the image comes into view
        threshold: 0.1
      }
    );

    observer.observe(imgRef.current);

    return () => {
      if (imgRef.current) {
        observer.unobserve(imgRef.current);
      }
    };
  }, [lazyLoad, isVisible]);

  const handleImageLoad = () => {
    setIsLoading(false);
    setHasLoaded(true);
    onLoad?.();
  };

  const handleImageError = () => {
    setIsLoading(false);
    setHasError(true);
    onError?.();
  };

  return (
    <Box 
      ref={imgRef}
      sx={{ 
        position: 'relative', 
        pointerEvents: 'none',
        backgroundImage: `url(${placeholder})`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        aspectRatio: '745/1040',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center'
      }} 
      className={className}
    >
      {/* Loading indicator */}
      {isLoading && !hasLoaded && !hasError && (
        <CircularProgress 
          size={24} 
          sx={{ 
            position: 'absolute',
            color: 'rgba(255, 255, 255, 0.7)',
            zIndex: 1
          }} 
        />
      )}

      {/* Actual Card Image - only render if visible and we have a URL */}
      {isVisible && imageUrl && !hasError && (
        <CardMedia
          component="img"
          image={imageUrl}
          alt={cardName || 'Magic card'}
          onLoad={handleImageLoad}
          onError={handleImageError}
          sx={{
            width: '100%',
            height: '100%',
            objectFit: 'cover',
            position: 'absolute',
            top: 0,
            left: 0,
            pointerEvents: 'none',
            opacity: hasLoaded ? 1 : 0,
            transition: 'opacity 0.3s ease-in-out'
          }}
        />
      )}
    </Box>
  );
});

CardImage.displayName = 'CardImage';