import React from 'react';
import { Button as MuiButtonBase, CircularProgress, useTheme } from '../../atoms';
import type { ButtonProps as MuiButtonProps, SxProps, Theme } from '../../atoms';
import type { StyledComponentProps } from '../../../types/components';
import { useLongPress } from '../../../hooks/useLongPress';
import { useHapticFeedback } from '../../../hooks/useHapticFeedback';

interface AppButtonProps extends Omit<MuiButtonProps, 'size'>, StyledComponentProps {
  isLoading?: boolean;
  size?: 'small' | 'medium' | 'large';
  enableHaptic?: boolean;
  enableLongPress?: boolean;
  onLongPress?: () => void;
  touchOptimized?: boolean; // Enable touch-optimized styling
}

export const AppButton: React.FC<AppButtonProps> = ({
  children,
  isLoading = false,
  disabled,
  size = 'medium',
  variant = 'contained',
  enableHaptic = true,
  enableLongPress = false,
  onLongPress,
  touchOptimized = true,
  onClick,
  sx,
  ...props
}) => {
  const theme = useTheme();
  const { triggerHaptic } = useHapticFeedback({ enabled: enableHaptic });

  // Handle haptic feedback on click
  const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    if (enableHaptic && !disabled && !isLoading) {
      triggerHaptic('light');
    }
    onClick?.(event);
  };

  // Handle long press with haptic feedback
  const handleLongPress = () => {
    if (enableHaptic) {
      triggerHaptic('medium');
    }
    onLongPress?.();
  };

  const longPressHandlers = useLongPress({
    onLongPress: handleLongPress,
    onClick: (event) => handleClick(event as unknown as React.MouseEvent<HTMLButtonElement>),
    threshold: 500,
    onStart: () => enableHaptic && triggerHaptic('selection'),
  });

  // Touch-optimized styling
  const getTouchOptimizedSx = (): SxProps<Theme> => {
    if (!touchOptimized) return {};

    const sizeConfig = {
      small: {
        minHeight: theme.mtg.dimensions.touch.minHeight,
        minWidth: theme.mtg.dimensions.touch.minHeight,
        fontSize: '0.8125rem',
      },
      medium: {
        minHeight: theme.mtg.dimensions.touch.comfortableHeight,
        minWidth: theme.mtg.dimensions.touch.comfortableHeight,
        fontSize: '0.875rem',
      },
      large: {
        minHeight: theme.mtg.dimensions.touch.largeHeight,
        minWidth: theme.mtg.dimensions.touch.largeHeight,
        fontSize: '1rem',
      },
    };

    return {
      ...sizeConfig[size],
      borderRadius: 2,
      transition: theme.mtg.transitions.touch,
      WebkitTapHighlightColor: 'transparent', // Remove iOS tap highlight

      // Touch feedback states
      '&:active': {
        transform: 'scale(0.95)',
        boxShadow: theme.mtg.shadows.card.touch,
      },

      // Enhanced focus styles for accessibility
      '&:focus-visible': {
        outline: `2px solid ${theme.palette.primary.main}`,
        outlineOffset: '2px',
      },

      // Loading state styling
      ...(isLoading && {
        pointerEvents: 'none',
        opacity: 0.7,
      }),

      // Responsive font sizes
      fontSize: {
        xs: sizeConfig[size].fontSize,
        sm: size === 'large' ? '1.125rem' : sizeConfig[size].fontSize,
      },

      // Enhanced hover on non-touch devices
      '@media (hover: hover)': {
        '&:hover': {
          transform: 'translateY(-1px)',
          boxShadow: theme.mtg.shadows.card.hover,
        },
      },
    };
  };

  const combinedSx = {
    ...getTouchOptimizedSx(),
    ...sx,
  } as SxProps<Theme>;

  // Use long press handlers if enabled, otherwise use regular click
  const eventHandlers = enableLongPress && onLongPress && !disabled && !isLoading
    ? {
      onTouchStart: longPressHandlers.onTouchStart,
      onTouchEnd: longPressHandlers.onTouchEnd,
      onTouchCancel: longPressHandlers.onTouchCancel,
      onMouseDown: longPressHandlers.onMouseDown,
      onMouseUp: longPressHandlers.onMouseUp,
      onMouseLeave: longPressHandlers.onMouseLeave,
    }
    : {
      onClick: handleClick,
    };

  return (
    <MuiButtonBase
      variant={variant}
      size={size}
      disabled={disabled || isLoading}
      startIcon={isLoading ? <CircularProgress size={size === 'small' ? 14 : size === 'large' ? 20 : 16} /> : undefined}
      sx={combinedSx}
      {...(eventHandlers as Partial<React.DOMAttributes<HTMLButtonElement>>)}
      {...props}
    >
      {isLoading ? 'Loading...' : children}
    </MuiButtonBase>
  );
};