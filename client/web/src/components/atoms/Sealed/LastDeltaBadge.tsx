import React from 'react';
import { Chip } from '@mui/material';
import type { SxProps, Theme } from '@mui/material';

interface LastDeltaBadgeProps {
  delta: number;
  sx?: SxProps<Theme>;
}

/**
 * Displays the last modification delta for a sealed product.
 * Shows +X (green) for additions, -X (red) for removals.
 * Only visible if the product was modified in the current session.
 */
export const LastDeltaBadge: React.FC<LastDeltaBadgeProps> = ({ delta, sx }) => {
  const isPositive = delta > 0;
  const displayText = isPositive ? `+${delta}` : `${delta}`;
  const bgColor = isPositive
    ? 'rgba(76, 175, 80, 0.9)'
    : 'rgba(244, 67, 54, 0.9)';

  return (
    <Chip
      label={displayText}
      size="small"
      sx={{
        height: { xs: 20, sm: 24 },
        fontSize: { xs: '0.65rem', sm: '0.75rem' },
        fontWeight: 700,
        backgroundColor: bgColor,
        color: 'white',
        backdropFilter: 'blur(4px)',
        border: '1px solid rgba(255, 255, 255, 0.2)',
        boxShadow: '0 2px 4px rgba(0,0,0,0.2)',
        '& .MuiChip-label': {
          px: 1
        },
        ...sx
      }}
    />
  );
};
