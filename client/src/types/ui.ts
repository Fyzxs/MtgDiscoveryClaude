/**
 * UI-specific type definitions for common UI patterns and states
 */

/**
 * Common UI states for async operations
 */
export type LoadingState = 'idle' | 'loading' | 'success' | 'error';

/**
 * Visibility state for UI elements
 */
export type VisibilityState = 'visible' | 'hidden' | 'collapsed';

/**
 * Display modes for components
 */
export type DisplayMode = 'compact' | 'comfortable' | 'expanded';

/**
 * Common sizes used across UI components
 */
export type Size = 'small' | 'medium' | 'large';

/**
 * Color variants for UI components
 */
export type ColorVariant = 'default' | 'primary' | 'secondary' | 'error' | 'warning' | 'info' | 'success';

/**
 * Positions for floating elements
 */
export interface Position {
  top?: number | string;
  right?: number | string;
  bottom?: number | string;
  left?: number | string;
}

/**
 * Spacing configuration
 */
export interface Spacing {
  top?: number | string;
  right?: number | string;
  bottom?: number | string;
  left?: number | string;
  x?: number | string; // horizontal
  y?: number | string; // vertical
  all?: number | string; // all sides
}

/**
 * Breakpoint values for responsive design
 */
export interface Breakpoints {
  xs?: unknown;
  sm?: unknown;
  md?: unknown;
  lg?: unknown;
  xl?: unknown;
}

/**
 * Common animation configuration
 */
export interface AnimationConfig {
  duration?: number;
  easing?: string;
  delay?: number;
}

/**
 * Tooltip configuration
 */
export interface TooltipConfig {
  title: string;
  placement?: 'top' | 'right' | 'bottom' | 'left';
  arrow?: boolean;
  enterDelay?: number;
  leaveDelay?: number;
}

/**
 * Modal/Dialog configuration
 */
export interface DialogConfig {
  open: boolean;
  onClose: () => void;
  title?: string;
  fullWidth?: boolean;
  maxWidth?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | false;
  fullScreen?: boolean;
}

/**
 * Pagination configuration
 */
export interface PaginationConfig {
  page: number;
  pageSize: number;
  total: number;
  onPageChange: (page: number) => void;
  onPageSizeChange?: (size: number) => void;
  pageSizeOptions?: number[];
}

/**
 * Common form field props
 */
export interface FormFieldProps {
  name: string;
  label?: string;
  placeholder?: string;
  required?: boolean;
  disabled?: boolean;
  error?: boolean;
  helperText?: string;
  fullWidth?: boolean;
}

/**
 * Tab configuration for tab panels
 */
export interface TabConfig {
  label: string;
  value: string | number;
  disabled?: boolean;
  icon?: React.ReactNode;
  content?: React.ReactNode;
}

/**
 * Common list item configuration
 */
export interface ListItemConfig<T = unknown> {
  id: string | number;
  label: string;
  value: T;
  disabled?: boolean;
  selected?: boolean;
  icon?: React.ReactNode;
  secondary?: string;
}