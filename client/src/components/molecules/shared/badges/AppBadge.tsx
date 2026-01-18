import React from 'react';
import { Chip } from '../../../atoms';
import type { SxProps, Theme } from '@mui/material';
import type { Rarity } from '../../../../types/card';

type AppBadgeVariant = 'code' | 'rarity' | 'default';

interface AppBadgeProps {
  variant: AppBadgeVariant;
  /** Label text - required for 'default' and 'code' variants */
  label?: string;
  /** Rarity value - required for 'rarity' variant */
  rarity?: Rarity | string;
  /** Compact mode for smaller display */
  compact?: boolean;
  /** Size variant */
  size?: 'small' | 'medium';
  /** Additional styles */
  sx?: SxProps<Theme>;
  /** Additional className */
  className?: string;
}

/**
 * Unified app badge component with multiple variants.
 *
 * Variants:
 * - 'code': Monospace set code badge (formerly SetCodeBadge)
 * - 'rarity': Rarity symbol badge with color coding (formerly RarityBadge)
 * - 'default': Generic badge with custom label
 *
 * Usage:
 * - <AppBadge variant="code" label="MH3" />
 * - <AppBadge variant="rarity" rarity="mythic" />
 * - <AppBadge variant="default" label="New" />
 */
export const AppBadge: React.FC<AppBadgeProps> = ({
  variant,
  label,
  rarity,
  compact = false,
  size = 'medium',
  sx,
  className
}) => {
  // Rarity variant
  if (variant === 'rarity') {
    if (!rarity) {
      return null;
    }

    const getRarityColors = (r: string): { background: string; color: string } => {
      switch (r.toLowerCase()) {
        case 'common':
          return { background: '#4B5563', color: '#F3F4F6' };
        case 'uncommon':
          return { background: '#9CA3AF', color: '#111827' };
        case 'rare':
          return { background: '#D97706', color: '#FEF3C7' };
        case 'mythic':
          return { background: '#EA580C', color: '#FFEDD5' };
        case 'special':
        case 'bonus':
          return { background: '#9333EA', color: '#F3E8FF' };
        default:
          return { background: '#374151', color: '#D1D5DB' };
      }
    };

    const getRaritySymbol = (r: string): string => {
      switch (r.toLowerCase()) {
        case 'common':
          return 'C';
        case 'uncommon':
          return 'U';
        case 'rare':
          return 'R';
        case 'mythic':
          return 'M';
        case 'special':
          return 'S';
        case 'bonus':
          return 'B';
        default:
          return '?';
      }
    };

    const colors = getRarityColors(rarity);
    const chipSize = size === 'small' ? 20 : 24;
    const fontSize = size === 'small' ? '0.625rem' : '0.75rem';

    return (
      <Chip
        label={getRaritySymbol(rarity)}
        className={className}
        sx={{
          width: chipSize,
          height: chipSize,
          minWidth: chipSize,
          backgroundColor: colors.background,
          color: colors.color,
          fontSize,
          fontWeight: 'bold',
          '& .MuiChip-label': {
            px: 0,
            py: 0
          },
          ...sx
        }}
        title={rarity}
      />
    );
  }

  // Code variant
  if (variant === 'code') {
    if (!label) {
      return null;
    }

    return (
      <Chip
        label={label.toUpperCase()}
        size="small"
        variant="filled"
        color="primary"
        className={className}
        sx={{
          fontFamily: 'monospace',
          fontWeight: 600,
          fontSize: compact ? '0.5625rem' : '0.875rem',
          height: compact ? 18 : undefined,
          '& .MuiChip-label': {
            px: compact ? 0.5 : 1,
          },
          ...sx
        }}
      />
    );
  }

  // Default variant
  if (!label) {
    return null;
  }

  return (
    <Chip
      label={label}
      size={size}
      className={className}
      sx={sx}
    />
  );
};
