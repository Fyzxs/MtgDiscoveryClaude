import React from 'react';
import { ManaSymbol } from '../../atoms';
import type { StyledComponentProps } from '../../../types/components';

interface ManaCostProps extends StyledComponentProps {
  manaCost?: string;
  size?: 'small' | 'medium' | 'large';
}

export const ManaCost: React.FC<ManaCostProps> = ({ 
  manaCost, 
  size = 'medium',
  className
}) => {
  if (!manaCost) return null;

  // Parse mana cost string like "{2}{W}{W}" into individual symbols
  const parseManaSymbols = (cost: string): string[] => {
    const matches = cost.match(/{[^}]+}/g);
    return matches || [];
  };

  const symbols = parseManaSymbols(manaCost);

  return (
    <div className={`flex items-center gap-1 ${className}`}>
      {symbols.map((symbol, index) => (
        <ManaSymbol key={index} symbol={symbol} size={size} />
      ))}
    </div>
  );
};