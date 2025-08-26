import React from 'react';
import { Button, ButtonProps, SxProps, Theme } from '@mui/material';

interface ToggleButtonProps extends Omit<ButtonProps, 'onClick'> {
  isActive: boolean;
  onToggle: (active: boolean) => void;
  activeColor?: 'primary' | 'secondary' | 'success' | 'warning' | 'error' | 'info';
  inactiveColor?: 'inherit' | 'primary' | 'secondary';
  activeVariant?: 'contained' | 'outlined' | 'text';
  inactiveVariant?: 'contained' | 'outlined' | 'text';
  children: React.ReactNode;
  ariaLabel?: string;
  sx?: SxProps<Theme>;
}

/**
 * Reusable toggle button component for filters and other toggleable states
 * Provides consistent styling and behavior across the application
 */
export const ToggleButton: React.FC<ToggleButtonProps> = React.memo(({
  isActive,
  onToggle,
  activeColor = 'primary',
  inactiveColor = 'inherit',
  activeVariant = 'contained',
  inactiveVariant = 'outlined',
  children,
  ariaLabel,
  sx = {},
  disabled = false,
  ...props
}) => {
  const handleClick = () => {
    if (!disabled) {
      onToggle(!isActive);
    }
  };

  return (
    <Button
      {...props}
      onClick={handleClick}
      color={isActive ? activeColor : inactiveColor}
      variant={isActive ? activeVariant : inactiveVariant}
      disabled={disabled}
      aria-pressed={isActive}
      aria-label={ariaLabel}
      sx={{
        minWidth: 'fit-content',
        transition: 'all 0.2s ease-in-out',
        '&:hover': {
          transform: 'scale(1.02)',
        },
        '&:active': {
          transform: 'scale(0.98)',
        },
        ...sx
      }}
    >
      {children}
    </Button>
  );
});

interface ToggleButtonGroupProps {
  children: React.ReactNode;
  orientation?: 'horizontal' | 'vertical';
  spacing?: number;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Container for grouping multiple toggle buttons with consistent spacing
 */
export const ToggleButtonGroup: React.FC<ToggleButtonGroupProps> = React.memo(({
  children,
  orientation = 'horizontal',
  spacing = 1,
  sx = {},
  className = ''
}) => {
  return (
    <div
      className={className}
      style={{
        display: 'flex',
        flexDirection: orientation === 'horizontal' ? 'row' : 'column',
        gap: `${spacing * 8}px`, // MUI spacing unit
        flexWrap: 'wrap',
        alignItems: 'flex-start',
        ...sx
      }}
    >
      {children}
    </div>
  );
});

interface FilterToggleProps {
  label: string;
  count?: number;
  isActive: boolean;
  onToggle: (active: boolean) => void;
  showCount?: boolean;
  disabled?: boolean;
}

/**
 * Specialized toggle button for filtering with optional count display
 */
export const FilterToggle: React.FC<FilterToggleProps> = React.memo(({
  label,
  count,
  isActive,
  onToggle,
  showCount = false,
  disabled = false
}) => {
  const displayText = showCount && count !== undefined 
    ? `${label} (${count})`
    : label;

  return (
    <ToggleButton
      isActive={isActive}
      onToggle={onToggle}
      activeColor="primary"
      inactiveColor="inherit"
      activeVariant="contained"
      inactiveVariant="outlined"
      disabled={disabled}
      ariaLabel={`Filter by ${label}${showCount && count !== undefined ? `, ${count} items` : ''}`}
      size="small"
    >
      {displayText}
    </ToggleButton>
  );
});

ToggleButton.displayName = 'ToggleButton';
ToggleButtonGroup.displayName = 'ToggleButtonGroup';
FilterToggle.displayName = 'FilterToggle';