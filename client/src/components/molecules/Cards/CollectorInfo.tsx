import React from 'react';
import { Box } from '../../atoms';
import { CollectorNumber, RarityBadge } from '../../atoms';
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
        <RarityBadge rarity={rarity} />
      )}
      {reserved && (
        <ReservedListShield />
      )}
    </Box>
  );
};