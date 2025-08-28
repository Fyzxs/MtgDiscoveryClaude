import React from 'react';
import { Box, Typography } from '@mui/material';

interface RarityCollectorBadgeProps {
  rarity?: string;
  collectorNumber?: string;
  className?: string;
}

export const RarityCollectorBadge: React.FC<RarityCollectorBadgeProps> = ({ 
  rarity,
  collectorNumber,
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
        <Box
          sx={{
            display: 'inline-flex',
            alignItems: 'center',
            px: 0.75,
            py: 0.25,
            borderTopLeftRadius: '4px',
            borderBottomLeftRadius: '4px',
            borderTopRightRadius: 0,
            borderBottomRightRadius: 0,
            background: colors.rarity,
            color: 'white',
            fontSize: '0.75rem',
            fontWeight: 'bold',
            textTransform: 'uppercase',
            boxShadow: '0 1px 3px rgba(0, 0, 0, 0.3)'
          }}
        >
          <Typography variant="caption" sx={{ fontSize: 'inherit', fontWeight: 'inherit' }}>
            {rarity.charAt(0)}
          </Typography>
        </Box>
      )}
      {collectorNumber && (
        <Box
          sx={{
            display: 'inline-flex',
            alignItems: 'center',
            px: 0.75,
            py: 0.25,
            borderTopLeftRadius: rarity ? 0 : '4px',
            borderBottomLeftRadius: rarity ? 0 : '4px',
            borderTopRightRadius: '4px',
            borderBottomRightRadius: '4px',
            background: colors.collector,
            color: 'white',
            fontSize: '0.75rem',
            fontFamily: 'monospace',
            boxShadow: '0 1px 3px rgba(0, 0, 0, 0.3)'
          }}
        >
          <Typography variant="caption" sx={{ fontSize: 'inherit', fontFamily: 'inherit' }}>
            #<span style={{ fontWeight: 'bold' }}>{collectorNumber}</span>
          </Typography>
        </Box>
      )}
    </Box>
  );
};