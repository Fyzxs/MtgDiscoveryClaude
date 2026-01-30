import React from 'react';
import { Box } from '../../atoms';
import { CollectorNumber } from '../../atoms';
import { AppBadge } from '../shared/badges';
import { ReservedListShield } from '../../atoms/Cards/ReservedListShield';

interface CollectorInfoProps {
  collectorNumber?: string;
  setCode?: string;
  rarity?: string;
  reserved?: boolean;
  className?: string;
}

export const CollectorInfo: React.FC<CollectorInfoProps> = ({
  collectorNumber,
  setCode,
  rarity,
  reserved = false,
  className = ''
}) => {
  if (!collectorNumber && !rarity) return null;

  return (
    <Box className={className} sx={{ display: 'flex', alignItems: 'center', gap: 3 }}>
      {collectorNumber && (
        <CollectorNumber number={collectorNumber} setCode={setCode} />
      )}
      {rarity && (
        <AppBadge variant="rarity" rarity={rarity} />
      )}
      {reserved && (
        <ReservedListShield />
      )}
    </Box>
  );
};