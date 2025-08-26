import React from 'react';
import { Box, SxProps, Theme } from '@mui/material';

export interface RarityGlowStyles {
  boxShadow: string;
}

export interface RarityColors {
  background: string;
  color: string;
}

/**
 * Get rarity-based colors for badges and styling
 */
export const getRarityColors = (rarity?: string): RarityColors => {
  const normalizedRarity = rarity?.toLowerCase() || '';
  
  switch (normalizedRarity) {
    case 'common':
      return { background: '#4B5563', color: '#F3F4F6' };
    case 'uncommon':
      return { background: '#9CA3AF', color: '#111827' };
    case 'rare':
      return { background: '#D97706', color: '#FEF3C7' };
    case 'mythic':
      return { background: '#EA580C', color: '#FFEDD5' };
    case 'special':
    case 'bonus':
      return { background: '#9333EA', color: '#F3E8FF' };
    default:
      return { background: '#374151', color: '#D1D5DB' };
  }
};

/**
 * Get rarity symbol character
 */
export const getRaritySymbol = (rarity?: string): string => {
  const normalizedRarity = rarity?.toLowerCase() || '';
  
  switch (normalizedRarity) {
    case 'common':
      return 'C';
    case 'uncommon':
      return 'U';
    case 'rare':
      return 'R';
    case 'mythic':
      return 'M';
    case 'special':
      return 'S';
    case 'bonus':
      return 'B';
    default:
      return '?';
  }
};

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

interface RarityStyledBoxProps {
  children: React.ReactNode;
  rarity?: string;
  isSelected?: boolean;
  isHovered?: boolean;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * A styled Box component that applies rarity-based styling automatically
 */
export const RarityStyledBox: React.FC<RarityStyledBoxProps> = React.memo(({
  children,
  rarity,
  isSelected = false,
  isHovered = false,
  sx = {},
  className = '',
  ...props
}) => {
  const glowStyles = getRarityGlowStyles(rarity, isSelected, isHovered);
  const transform = getCardTransform(isSelected, isHovered);

  return (
    <Box
      className={className}
      sx={{
        ...glowStyles,
        transform,
        transition: 'transform 0.15s ease-in-out, box-shadow 0.15s ease-in-out',
        ...sx
      }}
      {...props}
    >
      {children}
    </Box>
  );
});

RarityStyledBox.displayName = 'RarityStyledBox';