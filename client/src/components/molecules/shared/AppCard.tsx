import React from 'react';
import { Card, CardContent, useTheme } from '../../atoms';
import type { CardProps, SxProps, Theme } from '../../atoms';
import type { StyledComponentProps } from '../../../types/components';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';

interface AppCardProps extends Omit<CardProps, 'variant'>, StyledComponentProps {
  children: React.ReactNode;
  padding?: boolean | 'responsive' | 'none' | 'compact' | 'comfortable';
  responsiveElevation?: boolean; // Different elevation on mobile vs desktop
  touchOptimized?: boolean; // Enable touch-friendly styling
  variant?: 'default' | 'outlined' | 'glass' | 'floating'; // Different visual styles
}

export const AppCard: React.FC<AppCardProps> = ({
  children,
  padding = true,
  elevation = 2,
  responsiveElevation = true,
  touchOptimized = true,
  variant = 'default',
  sx,
  ...props
}) => {
  const theme = useTheme();
  const { isMobile } = useResponsiveBreakpoints();

  // Calculate responsive elevation
  const getElevation = () => {
    if (!responsiveElevation) return elevation;

    // Lower elevation on mobile for better performance and battery life
    if (isMobile) {
      return typeof elevation === 'number' ? Math.max(1, elevation - 1) : 1;
    }

    return elevation;
  };

  // Get padding configuration
  const getPaddingConfig = () => {
    if (padding === 'none' || padding === false) return null;

    const paddingConfigs = {
      responsive: {
        xs: theme.spacing(2), // 16px on mobile
        sm: theme.spacing(3), // 24px on tablet
        md: theme.spacing(4), // 32px on desktop
      },
      compact: theme.spacing(2), // 16px all around
      comfortable: theme.spacing(3), // 24px all around
      default: theme.spacing(3), // Default Material-UI CardContent padding
    };

    const paddingType = padding === true ? 'default' : padding;
    return paddingConfigs[paddingType] || paddingConfigs.default;
  };

  // Get variant-specific styling
  const getVariantSx = (): SxProps<Theme> => {
    const baseStyles = {
      borderRadius: 2,
      overflow: 'hidden',
      transition: theme.mtg.transitions.card,
    };

    switch (variant) {
      case 'outlined':
        return {
          ...baseStyles,
          border: `1px solid ${theme.palette.divider}`,
          backgroundColor: 'transparent',
        };

      case 'glass':
        return {
          ...baseStyles,
          backgroundColor: 'rgba(255, 255, 255, 0.1)',
          backdropFilter: 'blur(10px)',
          border: `1px solid rgba(255, 255, 255, 0.2)`,
        };

      case 'floating':
        return {
          ...baseStyles,
          boxShadow: theme.mtg.shadows.card.hover,
          transform: 'translateY(-2px)',
          '&:hover': {
            transform: 'translateY(-4px)',
            boxShadow: theme.mtg.shadows.card.selected,
          },
        };

      default:
        return baseStyles;
    }
  };

  // Get touch-optimized styling
  const getTouchOptimizedSx = (): SxProps<Theme> => {
    if (!touchOptimized) return {};

    return {
      // Remove iOS tap highlight
      WebkitTapHighlightColor: 'transparent',

      // Touch feedback for interactive cards
      cursor: props.onClick ? 'pointer' : 'default',

      // Active state for touch feedback
      ...(props.onClick && {
        '&:active': {
          transform: 'scale(0.98)',
          transition: theme.mtg.transitions.touch,
        },
      }),

      // Better touch target sizing
      minHeight: isMobile ? theme.mtg.dimensions.touch.minHeight : 'auto',

      // Responsive margin for better touch spacing
      margin: {
        xs: theme.spacing(0.5), // Tighter spacing on mobile
        sm: theme.spacing(1),   // More spacing on larger screens
      },

      // Enhanced accessibility
      '&:focus-visible': {
        outline: `2px solid ${theme.palette.primary.main}`,
        outlineOffset: '2px',
      },
    };
  };

  // Combine all styling
  const combinedSx = {
    ...getVariantSx(),
    ...getTouchOptimizedSx(),
    ...sx,
  } as SxProps<Theme>;

  const paddingConfig = getPaddingConfig();

  return (
    <Card
      elevation={getElevation()}
      sx={combinedSx}
      {...props}
    >
      {paddingConfig ? (
        <CardContent
          sx={{
            padding: paddingConfig,
            // Remove default bottom padding on last child
            '&:last-child': {
              paddingBottom: paddingConfig,
            },
          }}
        >
          {children}
        </CardContent>
      ) : (
        children
      )}
    </Card>
  );
};