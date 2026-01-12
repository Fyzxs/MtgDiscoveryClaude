import React, { useState } from 'react';
import { Box, Typography, Chip, Stack, Skeleton } from '@mui/material';
import { useTheme, alpha } from '@mui/material/styles';
import { ExternalLinkIcon } from '../../molecules/shared/ExternalLinkIcon';
import { CardDateBadge } from '../shared/CardDateBadge';
import { useLazyLoad } from '../../../hooks/useLazyLoad';
import type { SealedProduct } from '../../../hooks/useSealedProductsData';

// COMING_SOON placeholder image - shown while product image loads
const COMING_SOON_URL = '/coming-soon.png';

interface SealedProductCardProps {
  product: SealedProduct;
  onProductClick?: (product: SealedProduct) => void;
}

const formatCategory = (category: string | undefined): string => {
  if (!category) {
    return '';
  }
  return category
    .replace(/_/g, ' ')
    .replace(/\b\w/g, (char) => char.toUpperCase());
};

const getCategoryColor = (category: string | undefined): string => {
  switch (category?.toLowerCase()) {
    case 'booster_box':
      return '#7c4dff';
    case 'booster_pack':
      return '#536dfe';
    case 'bundle':
      return '#00bfa5';
    case 'commander_deck':
      return '#ff6d00';
    case 'starter_deck':
      return '#00c853';
    case 'precon':
      return '#ffab00';
    default:
      return '#78909c';
  }
};

export const SealedProductCard: React.FC<SealedProductCardProps> = ({
  product,
  onProductClick,
}) => {
  const theme = useTheme();
  const categoryColor = getCategoryColor(product.category);
  const [imageLoaded, setImageLoaded] = useState(false);

  // Lazy load images as they approach viewport
  const { ref: lazyRef, hasBeenInView } = useLazyLoad({
    rootMargin: '100px',
    threshold: 0.01
  });

  const hasPurchaseLinks = product.purchaseUrlTcgplayer || product.purchaseUrlCardmarket || product.purchaseUrlCardKingdom;

  // Determine badge label: use subtype if available and not "default", otherwise use category
  const badgeLabel = product.subtype && product.subtype.toLowerCase() !== 'default'
    ? formatCategory(product.subtype)
    : formatCategory(product.category);

  return (
    <Box
      ref={lazyRef}
      sx={{
        position: 'relative',
        width: '100%',
        // Mobile/tablet: image-only height, Desktop: include info section
        height: {
          xs: 150,  // Mobile: just image
          sm: 170,  // Tablet: just image
          md: 280,  // Desktop: image + info
          lg: 300   // Large desktop: image + info
        },
        display: 'flex',
        flexDirection: 'column',
        bgcolor: 'grey.900',
        borderRadius: 2,
        overflow: 'hidden',
        cursor: onProductClick ? 'pointer' : 'default',
        border: `1px solid ${theme.palette.grey[800]}`,
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: theme.shadows[8],
          borderColor: alpha(categoryColor, 0.5),
        },
        transition: 'transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease',
      }}
      onClick={() => onProductClick?.(product)}
    >
      {/* Product Image with COMING_SOON background */}
      <Box
        sx={{
          position: 'relative',
          width: '100%',
          // Mobile/tablet: fill entire card, Desktop: square aspect ratio
          paddingTop: { xs: '100%', sm: '100%', md: '100%' },
          flexGrow: { xs: 1, sm: 1, md: 0 }, // Fill on mobile, fixed on desktop
          flexShrink: 0,
          backgroundImage: `url(${COMING_SOON_URL})`,
          backgroundSize: 'cover',
          backgroundPosition: 'center',
          bgcolor: 'grey.800',
          overflow: 'hidden',
        }}
      >
        {/* Loading skeleton */}
        {!imageLoaded && hasBeenInView && (
          <Skeleton
            variant="rectangular"
            sx={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '100%',
              bgcolor: 'rgba(0, 0, 0, 0.3)',
            }}
          />
        )}

        {/* Product image */}
        {product.imageUrl && hasBeenInView && (
          <Box
            component="img"
            src={product.imageUrl}
            alt={product.name}
            sx={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '100%',
              objectFit: 'contain',
              p: 1,
              opacity: imageLoaded ? 1 : 0,
              transition: 'opacity 0.3s ease',
            }}
            onLoad={() => setImageLoaded(true)}
            onError={(e) => {
              const target = e.target as HTMLImageElement;
              target.style.display = 'none';
            }}
          />
        )}

        {/* Type Badge (subtype or category) */}
        {badgeLabel && (
          <Chip
            label={badgeLabel}
            size="small"
            sx={{
              position: 'absolute',
              top: { xs: 6, sm: 8 },
              left: { xs: 6, sm: 8 },
              bgcolor: alpha(categoryColor, 0.9),
              color: 'white',
              fontSize: { xs: '0.6rem', sm: '0.65rem' },
              fontWeight: 600,
              height: { xs: 18, sm: 20 },
              zIndex: 2,
              '& .MuiChip-label': {
                px: 1,
              },
            }}
          />
        )}

        {/* Release Date Badge */}
        {product.releaseDate && (
          <Box
            sx={{
              position: 'absolute',
              top: { xs: 6, sm: 8 },
              right: { xs: 6, sm: 8 },
              zIndex: 2,
            }}
          >
            <CardDateBadge date={product.releaseDate} />
          </Box>
        )}

        {/* Product Name Badge - Mobile/Tablet only */}
        <Box
          sx={{
            display: { xs: 'block', sm: 'block', md: 'none' },
            position: 'absolute',
            bottom: 0,
            left: 0,
            right: 0,
            bgcolor: 'rgba(0, 0, 0, 0.8)',
            backdropFilter: 'blur(8px)',
            p: { xs: 0.75, sm: 1 },
            zIndex: 2,
          }}
        >
          <Typography
            sx={{
              fontSize: { xs: '0.7rem', sm: '0.75rem' },
              fontWeight: 600,
              color: 'white',
              lineHeight: 1.2,
              textAlign: 'center',
              display: '-webkit-box',
              WebkitLineClamp: 2,
              WebkitBoxOrient: 'vertical',
              overflow: 'hidden',
            }}
          >
            {product.name}
          </Typography>
        </Box>
      </Box>

      {/* Product Info - Desktop only */}
      <Box
        sx={{
          display: { xs: 'none', sm: 'none', md: 'flex' },
          p: { md: 1.5 },
          flexGrow: 1,
          flexDirection: 'column',
          overflow: 'hidden',
          justifyContent: 'flex-start',
        }}
      >
        {/* Product Name - 2-line truncation */}
        <Typography
          sx={{
            fontSize: { md: '0.8125rem' },
            fontWeight: 600,
            color: 'text.primary',
            lineHeight: 1.3,
            display: '-webkit-box',
            WebkitLineClamp: 2,
            WebkitBoxOrient: 'vertical',
            overflow: 'hidden',
            textAlign: 'center',
          }}
        >
          {product.name}
        </Typography>
      </Box>

      {/* Purchase Links - Bottom right of entire card (all breakpoints) */}
      {hasPurchaseLinks && (
        <Stack
          direction="row"
          spacing={0.5}
          sx={{
            position: 'absolute',
            bottom: { xs: 6, sm: 8 },
            right: { xs: 6, sm: 8 },
            zIndex: 3,
          }}
        >
          <ExternalLinkIcon
            type="tcgplayer"
            url={product.purchaseUrlTcgplayer}
            size="small"
          />
          <ExternalLinkIcon
            type="cardmarket"
            url={product.purchaseUrlCardmarket}
            size="small"
          />
          <ExternalLinkIcon
            type="cardkingdom"
            url={product.purchaseUrlCardKingdom}
            size="small"
          />
        </Stack>
      )}
    </Box>
  );
};
