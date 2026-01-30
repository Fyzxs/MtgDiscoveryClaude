/**
 * Utility functions for rarity-based styling
 */

export interface RarityGlowStyles {
  boxShadow: string;
}

/**
 * Get glow styles based on card rarity
 */
export const getRarityGlowStyles = (
  rarity?: string,
  isSelected: boolean = false,
  isHovered: boolean = false
): RarityGlowStyles => {
  // If selected, always use blue glow
  if (isSelected) {
    return {
      boxShadow: isHovered
        ? '0 0 60px rgba(25, 118, 210, 1), 0 0 40px rgba(33, 150, 243, 0.8), 0 0 20px rgba(33, 150, 243, 0.6), 0 25px 50px -12px rgba(0, 0, 0, 0.5)'
        : '0 0 60px rgba(25, 118, 210, 1), 0 0 40px rgba(33, 150, 243, 0.8), 0 0 20px rgba(33, 150, 243, 0.6), 0 25px 50px -12px rgba(0, 0, 0, 0.25)'
    };
  }

  // Default shadow for non-hover states
  const defaultShadow = '0 25px 50px -12px rgba(0, 0, 0, 0.25)';
  
  // If not hovering, return default shadow
  if (!isHovered) {
    return { boxShadow: defaultShadow };
  }

  // Rarity-based hover glow
  if (!rarity) {
    return { boxShadow: '0 0 40px rgba(156, 163, 175, 0.4), 0 25px 50px -12px rgba(0, 0, 0, 0.5)' };
  }

  const normalizedRarity = rarity.toLowerCase();
  
  switch (normalizedRarity) {
    case 'common':
      return { boxShadow: '0 0 40px rgba(156, 163, 175, 0.5), 0 0 20px rgba(156, 163, 175, 0.3)' };
    
    case 'uncommon':
      return { boxShadow: '0 0 40px rgba(156, 163, 175, 0.4), 0 0 20px rgba(156, 163, 175, 0.2)' };
    
    case 'rare':
      return { boxShadow: '0 0 40px rgba(217, 119, 6, 0.5), 0 0 20px rgba(217, 119, 6, 0.3)' };
    
    case 'mythic':
      return { boxShadow: '0 0 40px rgba(234, 88, 12, 0.6), 0 0 20px rgba(234, 88, 12, 0.4)' };
    
    case 'special':
    case 'bonus':
      return { boxShadow: '0 0 40px rgba(147, 51, 234, 0.5), 0 0 20px rgba(147, 51, 234, 0.3)' };
    
    default:
      return { boxShadow: '0 0 40px rgba(156, 163, 175, 0.4), 0 0 20px rgba(156, 163, 175, 0.2)' };
  }
};

/**
 * Get transform scale based on selection and hover state
 */
export const getCardTransform = (isSelected: boolean, isHovered: boolean): string => {
  if (isSelected) {
    return 'scale(1.05)';
  }
  
  if (isHovered) {
    return 'scale(1.01)';
  }
  
  return 'scale(1)';
};