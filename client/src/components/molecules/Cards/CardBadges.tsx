import React from 'react';
import { Box, Chip } from '../../atoms';
import { useCardBadges } from '../../../hooks/useCardBadges';
import { formatFinishText, formatPromoText, formatFrameEffectText, getFinishColor } from '../../../utils/badgeFormatters';

interface CardBadgesProps {
  foil?: boolean;
  nonfoil?: boolean;
  etched?: boolean;
  promoTypes?: string[];
  frameEffects?: string[] | null;
  isPromo?: boolean;
  digital?: boolean;
  excludePromoTypes?: string[];
  excludeFrameEffects?: string[];
  inline?: boolean;
}

/**
 * CardBadges - Displays badges for card finishes, promo types, and frame effects
 * Refactored to use useCardBadges hook and badge formatters for better separation of concerns
 */
export const CardBadges: React.FC<CardBadgesProps> = ({
  foil = false,
  nonfoil = false,
  etched = false,
  promoTypes,
  frameEffects,
  isPromo = false,
  digital = false,
  excludePromoTypes,
  excludeFrameEffects,
  inline = false
}) => {
  const badges = useCardBadges({
    foil,
    nonfoil,
    etched,
    promoTypes,
    frameEffects,
    isPromo,
    digital,
    excludePromoTypes,
    excludeFrameEffects
  });

  // If no badges to display, return null
  if (badges.length === 0) {
    return null;
  }

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: inline ? 'row' : 'column-reverse',
        flexWrap: inline ? 'wrap' : 'nowrap',
        gap: 0.5,
        ...(inline ? {
          alignItems: 'center'
        } : {
          position: 'absolute',
          bottom: 185,
          right: 8,
          zIndex: 15,
          alignItems: 'flex-end'
        })
      }}
    >
      {badges.map((badge) => {
        let formattedLabel: string;
        let bgColor: string;
        let textColor: string = 'white';
        let borderColor: string = '1px solid rgba(255, 255, 255, 0.2)';
        let boxShadow: string = '0 2px 4px rgba(0,0,0,0.2)';
        let fontWeight: number = 600;

        // Format label based on badge type
        if (badge.type === 'finish') {
          formattedLabel = formatFinishText(badge.label);
          const color = getFinishColor(badge.label);
          bgColor = badge.label.toLowerCase() === 'foil'
            ? 'rgba(33, 150, 243, 0.9)'
            : badge.label.toLowerCase() === 'etched'
              ? 'rgba(156, 39, 176, 0.9)'
              : badge.label.toLowerCase() === 'nonfoil'
                ? 'rgba(158, 158, 158, 0.9)'
                : 'rgba(97, 97, 97, 0.9)';
        } else if (badge.type === 'digital') {
          formattedLabel = 'Digital';
          bgColor = 'rgba(138, 43, 226, 0.9)';
        } else if (badge.type === 'promo') {
          formattedLabel = formatPromoText(badge.label);
          const isSerialized = badge.label.toLowerCase() === 'serialized';
          bgColor = isSerialized
            ? 'rgba(255, 215, 0, 0.95)'
            : 'rgba(255, 152, 0, 0.9)';
          textColor = isSerialized ? '#000' : 'white';
          borderColor = isSerialized
            ? '1px solid rgba(255, 215, 0, 0.5)'
            : '1px solid rgba(255, 255, 255, 0.2)';
          boxShadow = isSerialized
            ? '0 2px 8px rgba(255, 215, 0, 0.5)'
            : '0 2px 4px rgba(0,0,0,0.2)';
          fontWeight = isSerialized ? 700 : 600;
        } else { // frame effect
          formattedLabel = formatFrameEffectText(badge.label);
          bgColor = badge.label.toLowerCase() === 'showcase'
            ? 'rgba(76, 175, 80, 0.9)'
            : badge.label.toLowerCase() === 'extendedart'
              ? 'rgba(0, 188, 212, 0.9)'
              : badge.label.toLowerCase() === 'fullart'
                ? 'rgba(255, 87, 34, 0.9)'
                : 'rgba(121, 85, 72, 0.9)';
        }

        return (
          <Chip
            key={badge.id}
            label={formattedLabel}
            size="small"
            sx={{
              height: 20,
              fontSize: '0.625rem',
              fontWeight,
              '& .MuiChip-label': {
                px: 1
              },
              backdropFilter: 'blur(4px)',
              backgroundColor: bgColor,
              color: textColor,
              border: borderColor,
              boxShadow
            }}
          />
        );
      })}
    </Box>
  );
};
