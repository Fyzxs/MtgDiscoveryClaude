import React, { ReactNode } from 'react';
import { Card as MuiCard, SxProps, Theme, CardProps } from '@mui/material';
import { RarityStyledBox } from '../../atoms/shared/RarityStyles';

interface BaseCardLayoutProps extends Omit<CardProps, 'onClick'> {
  children: ReactNode;
  onClick?: (e: React.MouseEvent) => void;
  isSelected?: boolean;
  isHovered?: boolean;
  rarity?: string;
  useRarityStyles?: boolean;
  width?: number | string;
  height?: number | string;
  hoverEffect?: 'scale' | 'lift' | 'glow' | 'custom' | 'none';
  borderStyle?: 'default' | 'rounded' | 'sharp';
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Base card layout component that provides common card styling patterns
 * Can be used as foundation for both MtgCard and MtgSetCard
 */
export const BaseCardLayout: React.FC<BaseCardLayoutProps> = React.memo(({
  children,
  onClick,
  isSelected = false,
  isHovered = false,
  rarity,
  useRarityStyles = false,
  width = 280,
  height = 'auto',
  hoverEffect = 'scale',
  borderStyle = 'rounded',
  sx = {},
  className = '',
  ...cardProps
}) => {
  const getHoverStyles = () => {
    if (hoverEffect === 'none') return {};
    
    switch (hoverEffect) {
      case 'scale':
        return {
          '&:hover': {
            transform: 'scale(1.02)',
            transition: 'transform 0.2s ease-in-out'
          }
        };
      case 'lift':
        return {
          '&:hover': {
            transform: 'translateY(-4px)',
            boxShadow: (theme: Theme) => theme.shadows[8]
          }
        };
      case 'glow':
        return {
          '&:hover': {
            boxShadow: '0 0 20px rgba(25, 118, 210, 0.4)'
          }
        };
      default:
        return {};
    }
  };

  const getBorderRadius = () => {
    switch (borderStyle) {
      case 'sharp':
        return 0;
      case 'rounded':
        return 6;
      default:
        return 4;
    }
  };

  if (useRarityStyles && rarity) {
    return (
      <RarityStyledBox
        rarity={rarity}
        isSelected={isSelected}
        isHovered={isHovered}
        className={className}
        sx={sx}
      >
        <MuiCard
          {...cardProps}
          onClick={onClick}
          sx={{
            width,
            height,
            borderRadius: getBorderRadius(),
            cursor: onClick ? 'pointer' : 'default',
            position: 'relative',
            ...getHoverStyles(),
          }}
        >
          {children}
        </MuiCard>
      </RarityStyledBox>
    );
  }

  return (
    <MuiCard
      {...cardProps}
      onClick={onClick}
      className={className}
      sx={{
        width,
        height,
        borderRadius: getBorderRadius(),
        cursor: onClick ? 'pointer' : 'default',
        position: 'relative',
        transition: 'all 0.2s ease-in-out',
        ...getHoverStyles(),
        ...sx
      }}
    >
      {children}
    </MuiCard>
  );
});

interface InteractiveCardLayoutProps extends BaseCardLayoutProps {
  onMouseEnter?: () => void;
  onMouseLeave?: () => void;
  onFocus?: () => void;
  onBlur?: () => void;
  tabIndex?: number;
  ariaLabel?: string;
}

/**
 * Interactive card layout with built-in hover and focus states
 */
export const InteractiveCardLayout: React.FC<InteractiveCardLayoutProps> = React.memo(({
  onMouseEnter,
  onMouseLeave,
  onFocus,
  onBlur,
  tabIndex = 0,
  ariaLabel,
  onClick,
  ...props
}) => {
  const [isHovered, setIsHovered] = React.useState(false);
  const [isFocused, setIsFocused] = React.useState(false);

  const handleMouseEnter = () => {
    setIsHovered(true);
    onMouseEnter?.();
  };

  const handleMouseLeave = () => {
    setIsHovered(false);
    onMouseLeave?.();
  };

  const handleFocus = () => {
    setIsFocused(true);
    onFocus?.();
  };

  const handleBlur = () => {
    setIsFocused(false);
    onBlur?.();
  };

  return (
    <BaseCardLayout
      {...props}
      isHovered={isHovered}
      onClick={onClick}
      onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
      onFocus={handleFocus}
      onBlur={handleBlur}
      tabIndex={onClick ? tabIndex : undefined}
      aria-label={ariaLabel}
      sx={{
        ...(isFocused && {
          outline: '2px solid',
          outlineColor: 'primary.light',
          outlineOffset: '2px'
        }),
        ...props.sx
      }}
    />
  );
});

BaseCardLayout.displayName = 'BaseCardLayout';
InteractiveCardLayout.displayName = 'InteractiveCardLayout';