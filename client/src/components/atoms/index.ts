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
export { default as Grid } from './Grid';

// Typography
export { default as Typography } from './Typography';

// Navigation
export { default as AppBar } from './AppBar';
export { default as Toolbar } from './Toolbar';
export { default as Menu } from './Menu';

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
export { default as Autocomplete } from './Autocomplete';

// Feedback
export { default as Alert } from './Alert';
export { default as CircularProgress } from './CircularProgress';
export { default as LinearProgress } from './LinearProgress';
export { default as Skeleton } from './Skeleton';

// Dialog Components
export { default as Dialog } from './Dialog';
export { default as DialogTitle } from './DialogTitle';
export { default as DialogContent } from './DialogContent';
export { default as DialogActions } from './DialogActions';

// List Components
export { default as List } from './List';
export { default as ListItem } from './ListItem';
export { default as ListItemButton } from './ListItemButton';
export { default as ListItemIcon } from './ListItemIcon';
export { default as ListItemText } from './ListItemText';

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
export { default as Popover } from './Popover';

// Icons
export * from './Icons';

// Export all prop types
export type {
  // Layout
  BoxProps,
  ContainerProps,
  StackProps,
  PaperProps,
  DividerProps,
  GridProps,

  // Typography
  TypographyProps,

  // Navigation
  AppBarProps,
  ToolbarProps,
  MenuProps,

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
  AutocompleteProps,

  // Feedback
  AlertProps,
  CircularProgressProps,
  LinearProgressProps,
  SkeletonProps,

  // Dialog
  DialogProps,
  DialogTitleProps,
  DialogContentProps,
  DialogActionsProps,

  // List
  ListProps,
  ListItemProps,
  ListItemButtonProps,
  ListItemIconProps,
  ListItemTextProps,

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
  PopoverProps,
} from './types';

// Re-export utilities and types that are commonly used
// These don't need wrapping as they're not components
export { useTheme, keyframes, useMediaQuery, alpha } from '@mui/material';
export type { SxProps, Theme, SelectChangeEvent } from '@mui/material';

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
