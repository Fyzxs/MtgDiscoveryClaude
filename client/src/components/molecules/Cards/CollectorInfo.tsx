import React from 'react';
import { Box } from '../../atoms';
import { CollectorNumber, RarityBadge } from '../../atoms';

interface CollectorInfoProps {
  collectorNumber?: string;
  setCode?: string;
  rarity?: string;
  className?: string;
}

export const CollectorInfo: React.FC<CollectorInfoProps> = ({ 
  collectorNumber,
  setCode,
  rarity,
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
    </Box>
  );
};