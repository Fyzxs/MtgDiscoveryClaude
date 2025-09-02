import React from 'react';
import { Typography } from '@mui/material';
import { DarkBadge } from './DarkBadge';
import type { PriceDisplayProps as StandardPriceProps } from '../../../types/components';

interface PriceDisplayProps extends Omit<StandardPriceProps, 'price' | 'currency'> {
  price: string | number | null | undefined;
  currency?: 'USD' | 'EUR' | 'usd' | 'eur';
  label?: string;
}

export const PriceDisplay: React.FC<PriceDisplayProps> = ({ 
  price, 
  currency = 'usd',
  label,
  className = '' 
}) => {
  const getPriceValue = (): number => {
    if (!price) return 0;
    const val = typeof price === 'string' ? parseFloat(price) : price;
    return isNaN(val) ? 0 : val;
  };

  const getPriceColor = (value: number): string => {
    if (value === 0) return '#B91C1C'; // dark red for $0.00 or unknown
    if (value < 5) return '#FB923C'; // light orange for under $5
    if (value < 10) return '#F97316'; // orange for $5-$10
    if (value < 100) return '#22C55E'; // green for $10-$100
    if (value < 500) return '#60A5FA'; // light blue for $100-$500
    return '#86EFAC'; // light green for over $500
  };

  const formatPrice = (value: number): string => {
    const symbol = currency.toLowerCase() === 'eur' ? '€' : '$';
    return `${symbol}${value.toFixed(2)}`;
  };

  const priceValue = getPriceValue();
  const colorTheme = getPriceColor(priceValue);

  return (
    <DarkBadge
      component="span"
      className={className}
      sx={{
        display: 'inline-flex',
        flexDirection: 'column',
        alignItems: 'flex-start'
      }}
    >
      {label && (
        <Typography component="span" variant="caption" sx={{ color: 'text.secondary' }}>
          {label}
        </Typography>
      )}
      <Typography 
        component="span"
        variant="body2"
        sx={{ 
          fontWeight: 'bold',
          color: colorTheme
        }}
      >
        {formatPrice(priceValue)}
      </Typography>
    </DarkBadge>
  );
};