import React from 'react';
import { Chip } from '../../../atoms';
import type { SxProps, Theme } from '@mui/material';
import { useLastDelta } from '../../../../hooks/useLastDelta';

interface DeltaBadgeProps {
  /** Card or item ID - when provided, uses useLastDelta hook */
  itemId?: string;
  /** Direct delta value - when provided, skips hook */
  delta?: number;
  /** Position styling - can be absolute positioned */
  positioned?: boolean;
  /** Additional styles */
  sx?: SxProps<Theme>;
}

/**
 * Unified delta badge for displaying modification deltas.
 * Shows +X (green) for additions, -X (red) for removals.
 *
 * Usage:
 * - With hook: <DeltaBadge itemId="card-123" positioned />
 * - Direct value: <DeltaBadge delta={5} />
 */
export const DeltaBadge: React.FC<DeltaBadgeProps> = ({
  itemId,
  delta: directDelta,
  positioned = false,
  sx
}) => {
  const hookDelta = useLastDelta(itemId ?? '');
  const delta = directDelta ?? hookDelta;

  if (delta === undefined) {
    return null;
  }

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
        ...(positioned && {
          position: 'absolute',
          top: 8,
          left: 8,
          zIndex: 15,
        }),
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
