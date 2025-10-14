import { useMemo } from 'react';

/**
 * Badge data structure
 */
export interface Badge {
  id: string;
  type: 'finish' | 'promo' | 'frame' | 'digital';
  label: string;
  value: string;
}

interface UseCardBadgesProps {
  foil?: boolean;
  nonfoil?: boolean;
  etched?: boolean;
  promoTypes?: string[];
  frameEffects?: string[] | null;
  isPromo?: boolean;
  digital?: boolean;
  excludePromoTypes?: string[];
  excludeFrameEffects?: string[];
}

// Default exclusions
const DEFAULT_EXCLUDE_PROMO_TYPES: string[] = ['boosterfun'];
const DEFAULT_EXCLUDE_FRAME_EFFECTS: string[] = ['inverted', 'legendary', 'enchantment', 'etched'];

/**
 * Hook to calculate which badges should be displayed for a card
 * Extracted from CardBadges component for reusability and testability
 */
export const useCardBadges = ({
  foil = false,
  nonfoil = false,
  etched = false,
  promoTypes = [],
  frameEffects = [],
  isPromo = false,
  digital = false,
  excludePromoTypes = DEFAULT_EXCLUDE_PROMO_TYPES,
  excludeFrameEffects = DEFAULT_EXCLUDE_FRAME_EFFECTS,
}: UseCardBadgesProps): Badge[] => {
  return useMemo(() => {
    const badges: Badge[] = [];
    const safePromoTypes = promoTypes || [];
    const safeFrameEffects = frameEffects || [];

    // Check for special foil promo types
    const hasSpecialFoilPromo = safePromoTypes.some(promo => {
      const lowerPromo = promo.toLowerCase();
      return lowerPromo.endsWith('foil') ||
        lowerPromo.endsWith('galaxyfoil') ||
        lowerPromo.endsWith('ripplefoil') ||
        lowerPromo === 'oilslick';
    });

    // Check if card is serialized - if so, only show that badge
    const isSerialized = safePromoTypes.some(promo => promo.toLowerCase() === 'serialized');

    if (!isSerialized) {
      // Non-foil badge: only show if card is BOTH non-foil AND foil (hybrid cards)
      if (nonfoil && foil) {
        badges.push({
          id: 'nonfoil',
          type: 'finish',
          label: 'nonfoil',
          value: 'nonfoil'
        });
      }

      // Foil badge: only show if card is foil BUT no special foil promo types
      if (foil && !hasSpecialFoilPromo) {
        badges.push({
          id: 'foil',
          type: 'finish',
          label: 'foil',
          value: 'foil'
        });
      }

      // Etched badge: show if card has etched finish
      if (etched) {
        badges.push({
          id: 'etched',
          type: 'finish',
          label: 'etched',
          value: 'etched'
        });
      }
    }

    // Digital badge
    if (digital) {
      badges.push({
        id: 'digital',
        type: 'digital',
        label: 'digital',
        value: 'digital'
      });
    }

    // Promo type badges
    const displayPromoTypes = isSerialized
      ? ['serialized']
      : safePromoTypes.filter(
          promoType => !excludePromoTypes.includes(promoType.toLowerCase())
        );

    displayPromoTypes.forEach((promoType, index) => {
      badges.push({
        id: `promo-${index}`,
        type: 'promo',
        label: promoType,
        value: promoType
      });
    });

    // Generic promo badge if isPromo is true but no specific types
    if (isPromo && displayPromoTypes.length === 0 && !isSerialized) {
      badges.push({
        id: 'promo-generic',
        type: 'promo',
        label: 'promo',
        value: 'promo'
      });
    }

    // Frame effect badges
    const displayFrameEffects = isSerialized
      ? []
      : safeFrameEffects.filter(
          effect => !excludeFrameEffects.includes(effect.toLowerCase())
        );

    displayFrameEffects.forEach((effect, index) => {
      badges.push({
        id: `effect-${index}`,
        type: 'frame',
        label: effect,
        value: effect
      });
    });

    return badges;
  }, [foil, nonfoil, etched, promoTypes, frameEffects, isPromo, digital, excludePromoTypes, excludeFrameEffects]);
};
