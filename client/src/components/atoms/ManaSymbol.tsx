import React from 'react';
import { Chip } from '@mui/material';

interface ManaSymbolProps {
  symbol: string;
  size?: 'small' | 'medium' | 'large';
  className?: string;
}

export const ManaSymbol: React.FC<ManaSymbolProps> = ({ 
  symbol, 
  size = 'medium',
  className = '' 
}) => {
  const getSizeValue = () => {
    switch (size) {
      case 'small': return { width: 16, height: 16, fontSize: '0.75rem' };
      case 'medium': return { width: 20, height: 20, fontSize: '0.875rem' };
      case 'large': return { width: 24, height: 24, fontSize: '1rem' };
      default: return { width: 20, height: 20, fontSize: '0.875rem' };
    }
  };

  const getSymbolColors = (s: string): { backgroundColor: string; color: string; borderColor: string } => {
    const sym = s.replace(/{|}/g, '').toUpperCase();
    switch (sym) {
      case 'W':
        return { backgroundColor: '#fef3c7', color: '#1f2937', borderColor: '#1f2937' };
      case 'U':
        return { backgroundColor: '#3b82f6', color: '#ffffff', borderColor: '#1d4ed8' };
      case 'B':
        return { backgroundColor: '#111827', color: '#f3f4f6', borderColor: '#374151' };
      case 'R':
        return { backgroundColor: '#ef4444', color: '#ffffff', borderColor: '#dc2626' };
      case 'G':
        return { backgroundColor: '#10b981', color: '#ffffff', borderColor: '#059669' };
      case 'C':
        return { backgroundColor: '#9ca3af', color: '#1f2937', borderColor: '#4b5563' };
      default:
        if (!isNaN(Number(sym))) {
          return { backgroundColor: '#d1d5db', color: '#1f2937', borderColor: '#6b7280' };
        }
        return { backgroundColor: '#fbbf24', color: '#1f2937', borderColor: '#f59e0b' };
    }
  };

  const formatSymbol = (s: string): string => {
    return s.replace(/{|}/g, '');
  };

  const sizeConfig = getSizeValue();
  const colors = getSymbolColors(symbol);

  return (
    <Chip
      label={formatSymbol(symbol)}
      className={className}
      sx={{
        width: sizeConfig.width,
        height: sizeConfig.height,
        minWidth: sizeConfig.width,
        backgroundColor: colors.backgroundColor,
        color: colors.color,
        border: `1px solid ${colors.borderColor}`,
        borderRadius: '50%',
        fontSize: sizeConfig.fontSize,
        fontWeight: 'bold',
        '& .MuiChip-label': {
          px: 0,
          py: 0
        }
      }}
      title={`Mana: ${formatSymbol(symbol)}`}
    />
  );
};