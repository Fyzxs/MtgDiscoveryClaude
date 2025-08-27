/**
 * Common component prop type definitions used across the application
 */

import type { SxProps, Theme } from '@mui/material';
import type { ReactNode } from 'react';

/**
 * Common props for components that support sx styling
 */
export interface StyledComponentProps {
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Props for components that display a summary
 */
export interface SummaryDisplayProps extends StyledComponentProps {
  showing: number;
  total: number;
  itemType?: string;
  textAlign?: 'left' | 'center' | 'right';
}

/**
 * Props for empty state components
 */
export interface EmptyStateProps extends StyledComponentProps {
  message?: string;
  description?: string;
  icon?: ReactNode;
  action?: {
    label: string;
    onClick: () => void;
  };
}

/**
 * Props for search components
 */
export interface SearchInputProps extends StyledComponentProps {
  value?: string;
  onChange: (value: string) => void;
  placeholder?: string;
  debounceMs?: number;
  minWidth?: number | string;
  fullWidth?: boolean;
  showClear?: boolean;
}

/**
 * Props for grid layout components
 */
export interface GridLayoutProps extends StyledComponentProps {
  children: ReactNode;
  minItemWidth?: number | string;
  spacing?: number | string;
  justifyContent?: 'start' | 'center' | 'end' | 'space-between' | 'space-around' | 'space-evenly';
  alignItems?: 'start' | 'center' | 'end' | 'stretch';
  component?: React.ElementType;
}

/**
 * Props for loading/error state containers
 */
export interface StateContainerProps {
  loading?: boolean;
  error?: Error | null;
  errorMessage?: string;
  children: ReactNode;
  loadingComponent?: ReactNode;
  errorComponent?: ReactNode;
  containerProps?: any;
  showContainer?: boolean;
}

/**
 * Props for FAB (Floating Action Button) components
 */
export interface FabComponentProps extends StyledComponentProps {
  onClick?: () => void;
  color?: 'inherit' | 'default' | 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning';
  size?: 'small' | 'medium' | 'large';
  disabled?: boolean;
  icon?: ReactNode;
  ariaLabel?: string;
}

/**
 * Props for badge components
 */
export interface BadgeProps extends StyledComponentProps {
  label: string;
  color?: 'default' | 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning';
  size?: 'small' | 'medium' | 'large';
  variant?: 'standard' | 'outlined' | 'filled';
}

/**
 * Props for price display components
 */
export interface PriceDisplayProps extends StyledComponentProps {
  price?: number | null;
  currency?: string;
  showFree?: boolean;
  freeText?: string;
  prefix?: string;
  suffix?: string;
}

/**
 * Props for date display components  
 */
export interface DateDisplayProps extends StyledComponentProps {
  date: string | Date;
  format?: 'short' | 'long' | 'relative';
  showTime?: boolean;
}

/**
 * Props for link components
 */
export interface LinkProps extends StyledComponentProps {
  href: string;
  target?: '_self' | '_blank' | '_parent' | '_top';
  rel?: string;
  children: ReactNode;
  underline?: 'none' | 'hover' | 'always';
}

/**
 * Common card selection props
 */
export interface SelectionProps {
  isSelected?: boolean;
  onSelectionChange?: (id: string, selected: boolean) => void;
  selectionMode?: 'none' | 'single' | 'multiple';
}