import React from 'react';
import { Box, Typography, Chip, IconButton, Stack } from '@mui/material';
import { useTheme, alpha } from '@mui/material/styles';
import OpenInNewIcon from '@mui/icons-material/OpenInNew';
import type { SealedProduct } from '../../../hooks/useSealedProductsData';

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

  const handlePurchaseClick = (url: string | undefined, e: React.MouseEvent) => {
    e.stopPropagation();
    if (url) {
      window.open(url, '_blank', 'noopener,noreferrer');
    }
  };

  const hasPurchaseLinks = product.purchaseUrlTcgplayer || product.purchaseUrlCardmarket || product.purchaseUrlCardKingdom;

  return (
    <Box
      sx={{
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
      {/* Product Image */}
      <Box
        sx={{
          position: 'relative',
          width: '100%',
          paddingTop: '100%',
          bgcolor: 'grey.800',
          overflow: 'hidden',
        }}
      >
        {product.imageUrl ? (
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
            }}
            onError={(e) => {
              const target = e.target as HTMLImageElement;
              target.style.display = 'none';
            }}
          />
        ) : (
          <Box
            sx={{
              position: 'absolute',
              top: '50%',
              left: '50%',
              transform: 'translate(-50%, -50%)',
              color: 'grey.600',
              textAlign: 'center',
            }}
          >
            <Typography variant="caption">No Image</Typography>
          </Box>
        )}

        {/* Category Badge */}
        {product.category && (
          <Chip
            label={formatCategory(product.category)}
            size="small"
            sx={{
              position: 'absolute',
              top: 8,
              left: 8,
              bgcolor: alpha(categoryColor, 0.9),
              color: 'white',
              fontSize: '0.65rem',
              fontWeight: 600,
              height: 20,
              '& .MuiChip-label': {
                px: 1,
              },
            }}
          />
        )}
      </Box>

      {/* Product Info */}
      <Box sx={{ p: 1.5 }}>
        <Typography
          sx={{
            fontSize: '0.8125rem',
            fontWeight: 600,
            color: 'text.primary',
            lineHeight: 1.3,
            mb: 0.5,
            display: '-webkit-box',
            WebkitLineClamp: 2,
            WebkitBoxOrient: 'vertical',
            overflow: 'hidden',
            minHeight: '2.1rem',
          }}
        >
          {product.name}
        </Typography>

        {product.subtype && (
          <Typography
            sx={{
              fontSize: '0.6875rem',
              color: 'text.secondary',
              mb: 0.5,
            }}
          >
            {formatCategory(product.subtype)}
          </Typography>
        )}

        {product.cardCount && product.cardCount > 0 && (
          <Typography
            sx={{
              fontSize: '0.6875rem',
              color: 'text.secondary',
            }}
          >
            {product.cardCount} cards
          </Typography>
        )}

        {/* Purchase Links */}
        {hasPurchaseLinks && (
          <Stack direction="row" spacing={0.5} sx={{ mt: 1 }}>
            {product.purchaseUrlTcgplayer && (
              <IconButton
                size="small"
                onClick={(e) => handlePurchaseClick(product.purchaseUrlTcgplayer, e)}
                sx={{
                  bgcolor: alpha('#00b0ff', 0.1),
                  color: '#00b0ff',
                  '&:hover': {
                    bgcolor: alpha('#00b0ff', 0.2),
                  },
                  width: 28,
                  height: 28,
                }}
                title="Buy on TCGPlayer"
              >
                <OpenInNewIcon sx={{ fontSize: 14 }} />
              </IconButton>
            )}
            {product.purchaseUrlCardmarket && (
              <IconButton
                size="small"
                onClick={(e) => handlePurchaseClick(product.purchaseUrlCardmarket, e)}
                sx={{
                  bgcolor: alpha('#ff9100', 0.1),
                  color: '#ff9100',
                  '&:hover': {
                    bgcolor: alpha('#ff9100', 0.2),
                  },
                  width: 28,
                  height: 28,
                }}
                title="Buy on Cardmarket"
              >
                <OpenInNewIcon sx={{ fontSize: 14 }} />
              </IconButton>
            )}
            {product.purchaseUrlCardKingdom && (
              <IconButton
                size="small"
                onClick={(e) => handlePurchaseClick(product.purchaseUrlCardKingdom, e)}
                sx={{
                  bgcolor: alpha('#76ff03', 0.1),
                  color: '#76ff03',
                  '&:hover': {
                    bgcolor: alpha('#76ff03', 0.2),
                  },
                  width: 28,
                  height: 28,
                }}
                title="Buy on Card Kingdom"
              >
                <OpenInNewIcon sx={{ fontSize: 14 }} />
              </IconButton>
            )}
          </Stack>
        )}
      </Box>
    </Box>
  );
};
