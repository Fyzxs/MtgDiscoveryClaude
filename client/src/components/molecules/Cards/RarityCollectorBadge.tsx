import React from 'react';
import { Box } from '../../atoms';
import { BadgePill } from '../../atoms/Cards/BadgePill';
import { ReservedListShield } from '../../atoms/Cards/ReservedListShield';

interface RarityCollectorBadgeProps {
  rarity?: string;
  collectorNumber?: string;
  reserved?: boolean;
  className?: string;
}

export const RarityCollectorBadge: React.FC<RarityCollectorBadgeProps> = ({
  rarity,
  collectorNumber,
  reserved = false,
  className
}) => {
  if (!rarity && !collectorNumber) return null;

  const getRarityColors = (rarity?: string) => {
    const rarityLower = rarity?.toLowerCase();
    switch (rarityLower) {
      case 'common':
        return {
          rarity: 'linear-gradient(135deg, #4B5563 0%, #374151 100%)',
          collector: 'linear-gradient(135deg, #374151 0%, #1F2937 100%)'
        };
      case 'uncommon':
        return {
          rarity: 'linear-gradient(135deg, #9CA3AF 0%, #6B7280 100%)',
          collector: 'linear-gradient(135deg, #6B7280 0%, #4B5563 100%)'
        };
      case 'rare':
        return {
          rarity: 'linear-gradient(135deg, #F59E0B 0%, #D97706 100%)',
          collector: 'linear-gradient(135deg, #D97706 0%, #B45309 100%)'
        };
      case 'mythic':
        return {
          rarity: 'linear-gradient(135deg, #EA580C 0%, #DC2626 100%)',
          collector: 'linear-gradient(135deg, #DC2626 0%, #991B1B 100%)'
        };
      case 'special':
      case 'bonus':
        return {
          rarity: 'linear-gradient(135deg, #A855F7 0%, #9333EA 100%)',
          collector: 'linear-gradient(135deg, #9333EA 0%, #7C3AED 100%)'
        };
      default:
        return {
          rarity: 'linear-gradient(135deg, #4B5563 0%, #374151 100%)',
          collector: 'linear-gradient(135deg, #374151 0%, #1F2937 100%)'
        };
    }
  };

  const colors = getRarityColors(rarity);

  return (
    <Box sx={{ display: 'flex', alignItems: 'center' }} className={className}>
      {rarity && (
        <BadgePill
          content={rarity.charAt(0).toUpperCase()}
          background={colors.rarity}
          borderRadiusLeft={true}
          borderRadiusRight={false}
        />
      )}
      {reserved && (
        <BadgePill
          content={<ReservedListShield size="small" iconSx={{ filter: 'none', width: 12, height: 12, display: 'block' }} />}
          background="linear-gradient(135deg, rgba(181, 166, 66, 0.9) 0%, rgba(139, 115, 85, 0.9) 100%)"
          borderRadiusLeft={false}
          borderRadiusRight={false}
          sx={{ px: 0.5 }}
        />
      )}
      {collectorNumber && (
        <BadgePill
          content={<>#<span style={{ fontWeight: 'bold' }}>{collectorNumber}</span></>}
          background={colors.collector}
          borderRadiusLeft={(rarity || reserved) ? false : true}
          borderRadiusRight={true}
          fontFamily="monospace"
        />
      )}
    </Box>
  );
};