import React from 'react';
import { Typography, Box } from '@mui/material';

interface ResultsSummaryProps {
  current: number;
  total: number;
  label: string;
  sx?: any;
  textAlign?: 'left' | 'center' | 'right';
  variant?: 'body1' | 'body2' | 'subtitle1' | 'subtitle2';
  color?: string;
  showWhenEqual?: boolean;
  customFormat?: (current: number, total: number, label: string) => string;
}

const ResultsSummaryComponent: React.FC<ResultsSummaryProps> = ({
  current,
  total,
  label,
  sx = {},
  textAlign = 'left',
  variant = 'body1',
  color = 'text.secondary',
  showWhenEqual = true,
  customFormat
}) => {
  // Don't show if current equals total and showWhenEqual is false
  if (!showWhenEqual && current === total) {
    return null;
  }

  // Get the display text
  const getText = () => {
    if (customFormat) {
      return customFormat(current, total, label);
    }
    
    // Default format: "Showing X of Y label"
    if (current === total) {
      return `${total} ${label}`;
    }
    return `Showing ${current} of ${total} ${label}`;
  };

  return (
    <Box sx={{ mb: 3, textAlign, ...sx }}>
      <Typography 
        variant={variant} 
        color={color}
        sx={{ 
          ...(variant === 'caption' && {
            letterSpacing: 2,
            fontSize: '0.875rem',
            textTransform: 'uppercase'
          })
        }}
      >
        {getText()}
      </Typography>
    </Box>
  );
};

/**
 * Memoized ResultsSummary component
 * Displays a summary of filtered results in a consistent format
 * 
 * @example
 * // Basic usage
 * <ResultsSummary current={10} total={50} label="cards" />
 * // Output: "Showing 10 of 50 cards"
 * 
 * @example
 * // When all items shown
 * <ResultsSummary current={50} total={50} label="sets" />
 * // Output: "50 sets"
 * 
 * @example
 * // Custom format
 * <ResultsSummary 
 *   current={3} 
 *   total={10} 
 *   label="printings"
 *   customFormat={(c, t, l) => `${c}/${t} ${l} visible`}
 * />
 * // Output: "3/10 printings visible"
 */
export const ResultsSummary = React.memo(ResultsSummaryComponent);