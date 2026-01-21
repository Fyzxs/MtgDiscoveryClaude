import React from 'react';
import { Box, Typography, CircularProgress } from '@mui/material';
import { SealedProductCard } from '../../atoms/Sealed/SealedProductCard';
import { useSealedProductGridNavigation } from '../../../hooks/useSealedProductGridNavigation';
import type { SealedProduct } from '../../../hooks/useSealedProductsData';
import type { SealedProductContext } from '../../../types/sealedProduct';
import { useCollectorParam } from '../../../hooks/useCollectorParam';

interface SealedProductGridProps {
  products: SealedProduct[];
  loading?: boolean;
  error?: Error | null;
  context?: SealedProductContext;
  onProductClick?: (product: SealedProduct) => void;
}

export const SealedProductGrid: React.FC<SealedProductGridProps> = ({
  products,
  loading = false,
  error = null,
  context: providedContext,
  onProductClick,
}) => {
  const { collectorId } = useCollectorParam();
  // Enable keyboard navigation
  const { handleKeyDown } = useSealedProductGridNavigation({
    totalItems: products.length,
    enabled: !loading && !error && products.length > 0
  });

  // Build context with user collector status
  const context: SealedProductContext = {
    ...providedContext,
    hasCollector: Boolean(collectorId)
  };

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
      data-sealed-products-grid
      onKeyDown={handleKeyDown}
      tabIndex={-1}
      sx={{
        display: 'flex',
        flexWrap: 'wrap',
        justifyContent: 'center',
        gap: { xs: 1, sm: 1.5, md: 2, lg: 2.5 },
        outline: 'none', // Remove focus outline on container
      }}
    >
      {products.map((product, index) => (
        <Box
          key={product.uuid}
          sx={{
            width: { xs: 150, sm: 170, md: 190, lg: 210 },
            flexShrink: 0,
          }}
        >
          <SealedProductCard
            product={product}
            index={index}
            context={context}
            onProductClick={onProductClick}
          />
        </Box>
      ))}
    </Box>
  );
};
