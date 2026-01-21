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
        display: 'grid',
        gridTemplateColumns: {
          xs: 'repeat(auto-fill, minmax(140px, 1fr))',
          sm: 'repeat(auto-fill, minmax(160px, 1fr))',
          md: 'repeat(auto-fill, minmax(180px, 1fr))',
          lg: 'repeat(auto-fill, minmax(200px, 1fr))',
        },
        gap: { xs: 1.5, sm: 2 },
        justifyContent: 'center',
      }}
    >
      {products.map((product) => (
        <SealedProductCard
          key={product.uuid}
          product={product}
          onProductClick={onProductClick}
        />
      ))}
    </Box>
  );
};
