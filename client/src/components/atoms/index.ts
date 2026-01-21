// Barrel file for all atoms
// This exports both MUI wrappers AND domain-specific atoms

// === MUI Component Wrappers ===
// Import these instead of @mui/material directly

// Layout Components
export { default as Box } from './mui-wrappers/Box';
export { default as Container } from './mui-wrappers/Container';
export { default as Stack } from './mui-wrappers/Stack';
export { default as Paper } from './mui-wrappers/Paper';
export { default as Divider } from './mui-wrappers/Divider';
export { default as Grid } from './mui-wrappers/Grid';

// Typography
export { default as Typography } from './mui-wrappers/Typography';

// Navigation
export { default as AppBar } from './mui-wrappers/AppBar';
export { default as Toolbar } from './mui-wrappers/Toolbar';
export { default as Menu } from './mui-wrappers/Menu';

// Buttons
export { default as Button } from './mui-wrappers/Button';
export { default as IconButton } from './mui-wrappers/IconButton';
export { default as Fab } from './mui-wrappers/Fab';
export { default as ToggleButton } from './mui-wrappers/ToggleButton';
export { default as ToggleButtonGroup } from './mui-wrappers/ToggleButtonGroup';

// Form Components
export { default as TextField } from './mui-wrappers/TextField';
export { default as Select } from './mui-wrappers/Select';
export { default as Checkbox } from './mui-wrappers/Checkbox';
export { default as Switch } from './mui-wrappers/Switch';
export { default as FormControl } from './mui-wrappers/FormControl';
export { default as FormControlLabel } from './mui-wrappers/FormControlLabel';
export { default as InputLabel } from './mui-wrappers/InputLabel';
export { default as InputAdornment } from './mui-wrappers/InputAdornment';
export { default as MenuItem } from './mui-wrappers/MenuItem';
export { default as Autocomplete } from './mui-wrappers/Autocomplete';

// Feedback
export { default as Alert } from './mui-wrappers/Alert';
export { default as CircularProgress } from './mui-wrappers/CircularProgress';
export { default as LinearProgress } from './mui-wrappers/LinearProgress';
export { default as Skeleton } from './mui-wrappers/Skeleton';

// Dialog Components
export { default as Dialog } from './mui-wrappers/Dialog';
export { default as DialogTitle } from './mui-wrappers/DialogTitle';
export { default as DialogContent } from './mui-wrappers/DialogContent';
export { default as DialogActions } from './mui-wrappers/DialogActions';

// List Components
export { default as List } from './mui-wrappers/List';
export { default as ListItem } from './mui-wrappers/ListItem';
export { default as ListItemButton } from './mui-wrappers/ListItemButton';
export { default as ListItemIcon } from './mui-wrappers/ListItemIcon';
export { default as ListItemText } from './mui-wrappers/ListItemText';

// Card Components
export { default as Card } from './mui-wrappers/Card';
export { default as CardMedia } from './mui-wrappers/CardMedia';
export { default as CardContent } from './mui-wrappers/CardContent';
export { default as CardActionArea } from './mui-wrappers/CardActionArea';

// Other Components
export { default as Badge } from './mui-wrappers/Badge';
export { default as Chip } from './mui-wrappers/Chip';
export { default as Drawer } from './mui-wrappers/Drawer';
export { default as SwipeableDrawer } from './mui-wrappers/SwipeableDrawer';
export { default as Link } from './mui-wrappers/Link';
export { default as Tooltip } from './mui-wrappers/Tooltip';
export { default as Modal } from './mui-wrappers/Modal';
export { default as Collapse } from './mui-wrappers/Collapse';
export { default as Icon } from './mui-wrappers/Icon';
export { default as Zoom } from './mui-wrappers/Zoom';
export { default as Popover } from './mui-wrappers/Popover';

// Icons
export * from './mui-wrappers/Icons';

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
  ToggleButtonProps,
  ToggleButtonGroupProps,

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
  BadgeProps,
  ChipProps,
  DrawerProps,
  SwipeableDrawerProps,
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

// Accessibility Atoms
export * from './accessibility';

// Binder Atoms
export * from './Binder';
