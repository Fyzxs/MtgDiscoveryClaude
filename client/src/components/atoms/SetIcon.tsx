import React from 'react';
import { Icon } from '@mui/material';

interface SetIconProps {
  setCode?: string;
  rarity?: string;
  size?: 'small' | 'medium' | 'large' | 'xlarge';
  className?: string;
}

export const SetIcon: React.FC<SetIconProps> = ({ 
  setCode, 
  rarity = 'common',
  size = 'medium',
  className = '' 
}) => {
  if (!setCode) return null;

  const getSizeValue = () => {
    switch (size) {
      case 'small': return '1rem';
      case 'medium': return '1.5rem';
      case 'large': return '2rem';
      case 'xlarge': return '2.5rem';
      default: return '1.5rem';
    }
  };

  const sizeClasses = {
    small: 'ss-fw ss-1x',
    medium: 'ss-fw ss-2x',
    large: 'ss-fw ss-3x',
    xlarge: 'ss-fw ss-4x'
  };

  // Keyrune uses 'ss' prefix and lowercase set codes
  const keyruneClass = `ss ss-${setCode.toLowerCase()}`;
  
  // Rarity gradient classes - use white/light grey for common
  const rarityClass = rarity && rarity.toLowerCase() !== 'common' ? `ss-${rarity.toLowerCase()}` : '';

  return (
    <Icon 
      className={`${keyruneClass} ${sizeClasses[size]} ${rarityClass} ${className}`}
      sx={{ 
        fontSize: getSizeValue(),
        display: 'inline-flex',
        alignItems: 'center',
        justifyContent: 'center',
        // Force light color for common cards
        ...(rarity && rarity.toLowerCase() === 'common' ? {
          color: 'rgba(255, 255, 255, 0.8) !important',
          filter: 'brightness(1.2)'
        } : {})
      }}
      title={setCode.toUpperCase()}
    />
  );
};