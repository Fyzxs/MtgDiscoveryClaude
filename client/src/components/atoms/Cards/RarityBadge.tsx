import React from 'react';
import { Chip } from '@mui/material';
import type { Rarity } from '../../../types/card';
import { getRarityColors, getRaritySymbol } from '../shared/RarityStyles';

interface RarityBadgeProps {
  rarity: Rarity | string;
  className?: string;
}

export const RarityBadge: React.FC<RarityBadgeProps> = React.memo(({ rarity, className = '' }) => {
  const colors = getRarityColors(rarity);

  return (
    <Chip 
      label={getRaritySymbol(rarity)}
      className={className}
      sx={{
        width: 24,
        height: 24,
        minWidth: 24,
        backgroundColor: colors.background,
        color: colors.color,
        fontSize: '0.75rem',
        fontWeight: 'bold',
        '& .MuiChip-label': {
          px: 0,
          py: 0
        }
      }}
      title={rarity}
    />
  );
});

RarityBadge.displayName = 'RarityBadge';