import React, { useRef, useCallback } from 'react';
import { Box, Typography, Chip, Stack, Card as MuiCard } from '@mui/material';
import { useTheme, alpha } from '@mui/material/styles';
import { ExternalLinkIcon } from '../../molecules/shared/ExternalLinkIcon';
import { useLazyLoad } from '../../../hooks/useLazyLoad';
import { LastDeltaBadge } from '../Cards/LastDeltaBadge';
import { useSealedCollection } from '../../../contexts/SealedCollectionContext';
import { useSealedProductCollectionActions } from '../../../hooks/useSealedProductCollectionActions';
import { useSealedProductInteractions } from '../../../hooks/useSealedProductInteractions';
import type { SealedProduct } from '../../../hooks/useSealedProductsData';

// COMING_SOON placeholder image - shown while product image loads
const COMING_SOON_URL = '/coming-soon.png';

interface SealedProductCardProps {
  product: SealedProduct;
  index: number;
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
  index,
  onProductClick,
}) => {
  const theme = useTheme();
  const categoryColor = getCategoryColor(product.category);
  const [imageLoaded, setImageLoaded] = React.useState(false);
  const productRef = useRef<HTMLDivElement>(null);
  const { getLastDelta, lastDeltaVersion } = useSealedCollection();

  // Lazy load images as they approach viewport
  const { ref: lazyLoadRef, hasBeenInView } = useLazyLoad({
    rootMargin: '100px',
    threshold: 0.01
  });

  // Combined ref callback for both lazy loading and product ref
  const handleRef = useCallback((node: HTMLDivElement | null) => {
    productRef.current = node;
    // lazyLoadRef is a ref object, not a callback - assign directly
    if (lazyLoadRef) {
      (lazyLoadRef as React.MutableRefObject<HTMLDivElement | null>).current = node;
    }
  }, [lazyLoadRef]);

  // Use interactions hook (matches card pattern exactly)
  const { isSelected, handleProductClick } = useSealedProductInteractions({
    productRef
  });

  // Use collection actions hook (registers keyboard handler)
  useSealedProductCollectionActions({
    productUuid: product.uuid,
    setId: product.setId,
    productName: product.name,
    isSelected,
    productRef
  });

  const hasPurchaseLinks = product.purchaseUrlTcgplayer || product.purchaseUrlCardmarket || product.purchaseUrlCardKingdom;

  // Determine badge label: use subtype if available and not "default", otherwise use category
  const badgeLabel = product.subtype && product.subtype.toLowerCase() !== 'default'
    ? formatCategory(product.subtype)
    : formatCategory(product.category);

  // Wrap handleProductClick to pass product and onProductClick
  const handleClick = useCallback((e: React.MouseEvent) => {
    handleProductClick(e, onProductClick, product);
  }, [handleProductClick, onProductClick, product]);

  // Get last delta for badge
  const lastDelta = getLastDelta(product.uuid);

  // Quantity for display
  const quantity = product.userQuantity || 0;

  // Debug logging
  React.useEffect(() => {
    console.log(`[SealedProductCard] ${product.name} - userQuantity:`, product.userQuantity, 'quantity:', quantity);
  }, [product.name, product.userQuantity, quantity]);

  return (
    <MuiCard
      ref={handleRef}
      elevation={4}
      onClick={handleClick}
      data-selected={isSelected}
      data-product-uuid={product.uuid}
      data-product-index={index}
      tabIndex={-1}
      role="button"
      aria-label={`${product.name}, quantity: ${quantity}`}
      sx={{
        position: 'relative',
        width: '100%',
        height: {
          xs: 240,
          sm: 260,
          md: 280,
          lg: 300
        },
        display: 'flex',
        flexDirection: 'column',
        bgcolor: 'grey.900',
        borderRadius: 2,
        overflow: 'hidden',
        cursor: 'pointer',
        border: `1px solid ${theme.palette.grey[800]}`,
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: theme.shadows[8],
          borderColor: alpha(categoryColor, 0.5),
        },
        // Selected state
        '&[data-selected="true"]': {
          borderColor: 'primary.main',
          borderWidth: 2,
          boxShadow: `0 0 0 2px ${theme.palette.primary.main}`,
        },
        // Submitting state
        '&[data-submitting="true"]': {
          opacity: 0.7,
        },
        // Flash animations
        '&[data-flash="success"]': {
          animation: 'flash-success 0.9s ease-out',
        },
        '&[data-flash="error"]': {
          animation: 'flash-error 0.9s ease-out',
        },
        transition: 'transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease',
        '@keyframes flash-success': {
          '0%': { backgroundColor: theme.palette.success.main },
          '100%': { backgroundColor: theme.palette.grey[900] }
        },
        '@keyframes flash-error': {
          '0%': { backgroundColor: theme.palette.error.main },
          '100%': { backgroundColor: theme.palette.grey[900] }
        }
      }}
    >
      {/* Product Image with COMING_SOON background */}
      <Box
        sx={{
          position: 'relative',
          width: '100%',
          paddingTop: '100%',
          flexShrink: 0,
          backgroundImage: `url(${COMING_SOON_URL})`,
          backgroundSize: 'cover',
          backgroundPosition: 'center',
          bgcolor: 'grey.800',
          overflow: 'hidden',
        }}
      >
        {/* Last Delta Badge - Top-left */}
        {lastDelta !== undefined && (
          <LastDeltaBadge
            delta={lastDelta}
            key={`${product.uuid}-${lastDeltaVersion}`}
            sx={{
              position: 'absolute',
              top: { xs: 6, sm: 8 },
              left: { xs: 6, sm: 8 },
              zIndex: 3,
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
              top: lastDelta !== undefined ? { xs: 32, sm: 36 } : { xs: 6, sm: 8 },
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

        {/* Product image - fades in over COMING_SOON background when loaded */}
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
      </Box>

      {/* Product Info */}
      <Box
        sx={{
          p: { xs: 1, sm: 1.5 },
          flexGrow: 1,
          display: 'flex',
          flexDirection: 'column',
          overflow: 'hidden',
          justifyContent: 'flex-start',
        }}
      >
        {/* Product Name - 2-line truncation */}
        <Typography
          sx={{
            fontSize: { xs: '0.75rem', sm: '0.8125rem' },
            fontWeight: 600,
            color: 'text.primary',
            lineHeight: 1.3,
            display: '-webkit-box',
            WebkitLineClamp: 2,
            WebkitBoxOrient: 'vertical',
            overflow: 'hidden',
            textAlign: 'center',
            minHeight: { xs: '1.95rem', sm: '2.1rem' },
          }}
        >
          {product.name}
        </Typography>

        {/* Quantity Display - Always show when userQuantity data is available */}
        <Typography
          sx={{
            fontSize: { xs: '0.7rem', sm: '0.75rem' },
            color: quantity > 0 ? 'success.main' : 'error.main',
            textAlign: 'center',
            mt: 0.5,
            fontWeight: 600,
          }}
        >
          {quantity > 0 ? `Qty: ${quantity}` : '⭕'}
        </Typography>
      </Box>

      {/* Purchase Links - Desktop only */}
      {hasPurchaseLinks && (
        <Stack
          direction="row"
          spacing={0.5}
          sx={{
            display: { xs: 'none', sm: 'none', md: 'flex' },
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
    </MuiCard>
  );
};
