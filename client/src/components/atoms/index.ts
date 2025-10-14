// Barrel file for all atoms
// This exports both MUI wrappers AND domain-specific atoms

// === MUI Component Wrappers ===
// Import these instead of @mui/material directly

// Layout Components
export { default as Box } from './Box';
export { default as Container } from './Container';
export { default as Stack } from './Stack';
export { default as Paper } from './Paper';
export { default as Divider } from './Divider';

// Typography
export { default as Typography } from './Typography';

// Buttons
export { default as Button } from './Button';
export { default as IconButton } from './IconButton';
export { default as Fab } from './Fab';

// Form Components
export { default as TextField } from './TextField';
export { default as Select } from './Select';
export { default as Checkbox } from './Checkbox';
export { default as Switch } from './Switch';
export { default as FormControl } from './FormControl';
export { default as FormControlLabel } from './FormControlLabel';
export { default as InputLabel } from './InputLabel';
export { default as InputAdornment } from './InputAdornment';
export { default as MenuItem } from './MenuItem';

// Feedback
export { default as Alert } from './Alert';
export { default as CircularProgress } from './CircularProgress';
export { default as LinearProgress } from './LinearProgress';
export { default as Skeleton } from './Skeleton';

// Card Components
export { default as Card } from './Card';
export { default as CardMedia } from './CardMedia';
export { default as CardContent } from './CardContent';
export { default as CardActionArea } from './CardActionArea';

// Other Components
export { default as Chip } from './Chip';
export { default as Link } from './Link';
export { default as Tooltip } from './Tooltip';
export { default as Modal } from './Modal';
export { default as Collapse } from './Collapse';
export { default as Icon } from './Icon';
export { default as Zoom } from './Zoom';

// Export all prop types
export type {
  // Layout
  BoxProps,
  ContainerProps,
  StackProps,
  PaperProps,
  DividerProps,

  // Typography
  TypographyProps,

  // Buttons
  ButtonProps,
  IconButtonProps,
  FabProps,

  // Form
  TextFieldProps,
  SelectProps,
  CheckboxProps,
  SwitchProps,
  FormControlProps,
  FormControlLabelProps,
  InputLabelProps,
  InputAdornmentProps,
  MenuItemProps,

  // Feedback
  AlertProps,
  CircularProgressProps,
  LinearProgressProps,
  SkeletonProps,

  // Card
  CardProps,
  CardMediaProps,
  CardContentProps,
  CardActionAreaProps,

  // Other
  ChipProps,
  LinkProps,
  TooltipProps,
  ModalProps,
  CollapseProps,
  IconProps,
  ZoomProps,
} from './types';

// Re-export utilities and types that are commonly used
// These don't need wrapping as they're not components
export { useTheme, keyframes } from '@mui/material';
export type { SxProps, Theme } from '@mui/material';

// === Domain-Specific Atoms ===
// Re-export all domain atoms from subfolders

// Card Atoms
export * from './Cards';

// Set Atoms
export * from './Sets';

// Shared Atoms
export * from './shared';

// Layout Atoms
export * from './layouts';
