import React from 'react';
import { Box } from '../../atoms';
import { ItemDateBadge } from '../../atoms';
import type { SealedProductContext } from '../../../types/sealedProduct';

interface SealedProductOverlayProps {
  releaseDate?: string;
  context?: SealedProductContext;
  className?: string;
  // Responsive props
  variant?: 'minimal' | 'compact' | 'full';
}

/**
 * SealedProductOverlay - simplified to show only release date
 * Category badge is shown at top of image
 * Product name and collection info are shown in grey box below image
 */
export const SealedProductOverlay: React.FC<SealedProductOverlayProps> = React.memo(({
  releaseDate,
  context = {},
  className,
  variant = 'full',
}) => {

  // If no release date and context says to hide it, don't render overlay
  if (!releaseDate || context.hideReleaseDate) {
    return null;
  }

  // Simple overlay with just release date badge
  return (
    <Box
      sx={{
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        zIndex: 10,
      }}
      className={className}
    >
      <Box sx={{ p: 1, display: 'flex', justifyContent: 'flex-end' }}>
        <ItemDateBadge date={releaseDate} />
      </Box>
    </Box>
  );
});
