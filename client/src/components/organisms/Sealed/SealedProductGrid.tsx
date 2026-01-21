import React from 'react';
import { Box, Typography, CircularProgress } from '@mui/material';
import { SealedProductCard } from '../../atoms/Sealed/SealedProductCard';
import type { SealedProduct } from '../../../hooks/useSealedProductsData';

interface SealedProductGridProps {
  products: SealedProduct[];
  loading?: boolean;
  error?: Error | null;
  onProductClick?: (product: SealedProduct) => void;
}

export const SealedProductGrid: React.FC<SealedProductGridProps> = ({
  products,
  loading = false,
  error = null,
  onProductClick,
}) => {
  if (loading) {
    return (
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          minHeight: 200,
          py: 4,
        }}
      >
        <CircularProgress size={40} />
      </Box>
    );
  }

  if (error) {
    return (
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          minHeight: 200,
          py: 4,
        }}
      >
        <Typography color="error" variant="body2">
          {error.message || 'Failed to load sealed products'}
        </Typography>
      </Box>
    );
  }

  if (products.length === 0) {
    return (
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          minHeight: 200,
          py: 4,
        }}
      >
        <Typography color="text.secondary" variant="body2">
          No sealed products found for this set
        </Typography>
      </Box>
    );
  }

  return (
    <Box
      sx={{
        display: 'flex',
        flexWrap: 'wrap',
        justifyContent: 'center',
        gap: { xs: 1, sm: 1.5, md: 2, lg: 2.5 },
      }}
    >
      {products.map((product) => (
        <Box
          key={product.uuid}
          sx={{
            width: { xs: 150, sm: 170, md: 190, lg: 210 },
            flexShrink: 0,
          }}
        >
          <SealedProductCard
            product={product}
            onProductClick={onProductClick}
          />
        </Box>
      ))}
    </Box>
  );
};
