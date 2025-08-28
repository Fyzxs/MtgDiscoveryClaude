import React from 'react';
import { CollectorNumber } from '../../atoms/Cards/CollectorNumber';
import { RarityBadge } from '../../atoms/Cards/RarityBadge';

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
    <div className={`flex items-center gap-3 ${className}`}>
      {collectorNumber && (
        <CollectorNumber number={collectorNumber} setCode={setCode} />
      )}
      {rarity && (
        <RarityBadge rarity={rarity} />
      )}
    </div>
  );
};