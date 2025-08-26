import React from 'react';
import { Box, Typography, SxProps, Theme } from '@mui/material';

interface StatsRowProps {
  label?: string;
  value?: React.ReactNode;
  children?: React.ReactNode;
  variant?: 'h6' | 'subtitle1' | 'body1' | 'body2';
  align?: 'left' | 'center' | 'right';
  showWhen?: boolean;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Reusable component for displaying statistical data in rows
 * Used across modal displays for consistent formatting
 */
export const StatsRow: React.FC<StatsRowProps> = React.memo(({
  label,
  value,
  children,
  variant = 'body1',
  align = 'left',
  showWhen = true,
  sx = {},
  className = ''
}) => {
  if (!showWhen) return null;

  // If children are provided, render them directly
  if (children) {
    return (
      <Box 
        className={className}
        sx={{ 
          display: 'flex', 
          flexDirection: 'column',
          gap: 1,
          ...sx 
        }}
      >
        {children}
      </Box>
    );
  }

  // Standard label-value display
  return (
    <Box 
      className={className}
      sx={{ 
        display: 'flex', 
        flexDirection: 'column',
        gap: label ? 1 : 0,
        ...sx 
      }}
    >
      {label && (
        <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
          {label}
        </Typography>
      )}
      {value && (
        <Box sx={{ 
          display: 'flex', 
          justifyContent: align === 'left' ? 'flex-start' : align === 'right' ? 'flex-end' : 'center',
          alignItems: 'center',
          gap: 1,
          pl: label ? 2 : 0
        }}>
          {typeof value === 'string' || typeof value === 'number' ? (
            <Typography variant={variant}>
              {value}
            </Typography>
          ) : (
            value
          )}
        </Box>
      )}
    </Box>
  );
});

interface PowerToughnessProps {
  power?: string;
  toughness?: string;
  loyalty?: string;
  defense?: string;
}

/**
 * Specialized component for Power/Toughness and similar combat stats
 */
export const PowerToughnessRow: React.FC<PowerToughnessProps> = React.memo(({
  power,
  toughness,
  loyalty,
  defense
}) => {
  const hasStats = power || loyalty || defense;
  
  if (!hasStats) return null;

  return (
    <Box sx={{ 
      display: 'flex', 
      gap: 3, 
      justifyContent: power ? 'flex-end' : 'flex-start',
      alignItems: 'center'
    }}>
      {power && (
        <Typography variant="h6">
          {power}/{toughness}
        </Typography>
      )}
      {loyalty && (
        <Typography variant="h6">
          Loyalty: {loyalty}
        </Typography>
      )}
      {defense && (
        <Typography variant="h6">
          Defense: {defense}
        </Typography>
      )}
    </Box>
  );
});

interface StatsGroupProps {
  title: string;
  children: React.ReactNode;
  showWhen?: boolean;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Groups multiple stats under a common heading with consistent spacing
 */
export const StatsGroup: React.FC<StatsGroupProps> = React.memo(({
  title,
  children,
  showWhen = true,
  sx = {},
  className = ''
}) => {
  if (!showWhen) return null;

  return (
    <Box 
      className={className}
      sx={{ 
        display: 'flex', 
        flexDirection: 'column',
        gap: 1,
        ...sx 
      }}
    >
      <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
        {title}
      </Typography>
      <Box sx={{ pl: 2 }}>
        {children}
      </Box>
    </Box>
  );
});

StatsRow.displayName = 'StatsRow';
PowerToughnessRow.displayName = 'PowerToughnessRow';
StatsGroup.displayName = 'StatsGroup';