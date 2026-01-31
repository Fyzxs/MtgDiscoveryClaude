import React from 'react';
import { Chip } from '../../../atoms';
import type { SxProps, Theme } from '@mui/material';
import { getSetTypeColor } from '../../../../constants/setTypeColors';

type StatusVariant = 'digital' | 'foil' | 'setType';

interface StatusBadgeProps {
  variant: StatusVariant;
  /** Set type value - required when variant is 'setType' */
  setType?: string;
  /** Controls visibility */
  show?: boolean;
  /** Additional styles */
  sx?: SxProps<Theme>;
}

/**
 * Unified status badge for displaying set/card status indicators.
 *
 * Variants:
 * - 'digital': Digital-only indicator (formerly DigitalBadge)
 * - 'foil': Foil-only indicator (formerly FoilOnlyBadge)
 * - 'setType': Dynamic set type with custom colors (formerly SetTypeBadge)
 *
 * Usage:
 * - <StatusBadge variant="digital" show={isDigital} />
 * - <StatusBadge variant="foil" show={isFoilOnly} />
 * - <StatusBadge variant="setType" setType="expansion" />
 */
export const StatusBadge: React.FC<StatusBadgeProps> = ({
  variant,
  setType,
  show = true,
  sx
}) => {
  if (!show) {
    return null;
  }

  const formatSetType = (type: string) => {
    return type.replace(/_/g, ' ').replace(/\b\w/g, (l) => l.toUpperCase());
  };

  // Digital variant
  if (variant === 'digital') {
    return (
      <Chip
        label="Digital"
        size="small"
        color="info"
        variant="filled"
        sx={sx}
      />
    );
  }

  // Foil variant
  if (variant === 'foil') {
    return (
      <Chip
        label="Foil"
        size="small"
        variant="filled"
        sx={{
          fontWeight: 500,
          backgroundColor: '#ffe96aff',
          color: '#000000',
          ...sx
        }}
      />
    );
  }

  // Set type variant
  if (variant === 'setType') {
    if (!setType) {
      return null;
    }

    const color = getSetTypeColor(setType);

    return (
      <Chip
        label={formatSetType(setType)}
        size="small"
        variant="filled"
        sx={{
          backgroundColor: color,
          color: 'white',
          fontWeight: 600,
          '&:hover': {
            backgroundColor: color,
          },
          ...sx
        }}
      />
    );
  }

  return null;
};
