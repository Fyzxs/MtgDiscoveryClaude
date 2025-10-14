// Type aliases for all MUI component props
// This allows our application code to use clean prop names without "Mui" prefix

import type {
  AlertProps as MuiAlertProps,
  AppBarProps as MuiAppBarProps,
  AutocompleteProps as MuiAutocompleteProps,
  BoxProps as MuiBoxProps,
  ButtonProps as MuiButtonProps,
  CardProps as MuiCardProps,
  CardActionAreaProps as MuiCardActionAreaProps,
  CardContentProps as MuiCardContentProps,
  CardMediaProps as MuiCardMediaProps,
  CheckboxProps as MuiCheckboxProps,
  ChipProps as MuiChipProps,
  CircularProgressProps as MuiCircularProgressProps,
  CollapseProps as MuiCollapseProps,
  ContainerProps as MuiContainerProps,
  DialogProps as MuiDialogProps,
  DialogTitleProps as MuiDialogTitleProps,
  DialogContentProps as MuiDialogContentProps,
  DialogActionsProps as MuiDialogActionsProps,
  DividerProps as MuiDividerProps,
  FabProps as MuiFabProps,
  FormControlProps as MuiFormControlProps,
  FormControlLabelProps as MuiFormControlLabelProps,
  IconProps as MuiIconProps,
  IconButtonProps as MuiIconButtonProps,
  InputAdornmentProps as MuiInputAdornmentProps,
  InputLabelProps as MuiInputLabelProps,
  LinearProgressProps as MuiLinearProgressProps,
  LinkProps as MuiLinkProps,
  ListProps as MuiListProps,
  ListItemProps as MuiListItemProps,
  ListItemButtonProps as MuiListItemButtonProps,
  ListItemIconProps as MuiListItemIconProps,
  ListItemTextProps as MuiListItemTextProps,
  MenuProps as MuiMenuProps,
  MenuItemProps as MuiMenuItemProps,
  ModalProps as MuiModalProps,
  PaperProps as MuiPaperProps,
  PopoverProps as MuiPopoverProps,
  SelectProps as MuiSelectProps,
  SkeletonProps as MuiSkeletonProps,
  StackProps as MuiStackProps,
  SwitchProps as MuiSwitchProps,
  TextFieldProps as MuiTextFieldProps,
  ToolbarProps as MuiToolbarProps,
  TooltipProps as MuiTooltipProps,
  TypographyProps as MuiTypographyProps,
  ZoomProps as MuiZoomProps,
  GridProps as MuiGridProps,
  SxProps as MuiSxProps,
  Theme as MuiTheme,
} from '@mui/material';

// Layout Components
export type BoxProps = MuiBoxProps;
export type ContainerProps = MuiContainerProps;
export type StackProps = MuiStackProps;
export type PaperProps = MuiPaperProps;
export type DividerProps = MuiDividerProps;
export type GridProps = MuiGridProps;

// Typography
export type TypographyProps = MuiTypographyProps;

// Navigation
export type AppBarProps = MuiAppBarProps;
export type ToolbarProps = MuiToolbarProps;
export type MenuProps = MuiMenuProps;

// Buttons
export type ButtonProps = MuiButtonProps;
export type IconButtonProps = MuiIconButtonProps;
export type FabProps = MuiFabProps;

// Form Components
export type TextFieldProps = MuiTextFieldProps;
export type SelectProps = MuiSelectProps;
export type CheckboxProps = MuiCheckboxProps;
export type SwitchProps = MuiSwitchProps;
export type FormControlProps = MuiFormControlProps;
export type FormControlLabelProps = MuiFormControlLabelProps;
export type InputLabelProps = MuiInputLabelProps;
export type InputAdornmentProps = MuiInputAdornmentProps;
export type MenuItemProps = MuiMenuItemProps;
export type AutocompleteProps<T, Multiple extends boolean | undefined = undefined, DisableClearable extends boolean | undefined = undefined, FreeSolo extends boolean | undefined = undefined> = MuiAutocompleteProps<T, Multiple, DisableClearable, FreeSolo>;

// Feedback
export type AlertProps = MuiAlertProps;
export type CircularProgressProps = MuiCircularProgressProps;
export type LinearProgressProps = MuiLinearProgressProps;
export type SkeletonProps = MuiSkeletonProps;

// Dialog Components
export type DialogProps = MuiDialogProps;
export type DialogTitleProps = MuiDialogTitleProps;
export type DialogContentProps = MuiDialogContentProps;
export type DialogActionsProps = MuiDialogActionsProps;

// List Components
export type ListProps = MuiListProps;
export type ListItemProps = MuiListItemProps;
export type ListItemButtonProps = MuiListItemButtonProps;
export type ListItemIconProps = MuiListItemIconProps;
export type ListItemTextProps = MuiListItemTextProps;

// Card Components
export type CardProps = MuiCardProps;
export type CardMediaProps = MuiCardMediaProps;
export type CardContentProps = MuiCardContentProps;
export type CardActionAreaProps = MuiCardActionAreaProps;

// Other Components
export type ChipProps = MuiChipProps;
export type LinkProps = MuiLinkProps;
export type TooltipProps = MuiTooltipProps;
export type ModalProps = MuiModalProps;
export type CollapseProps = MuiCollapseProps;
export type IconProps = MuiIconProps;
export type ZoomProps = MuiZoomProps;
export type PopoverProps = MuiPopoverProps;

// Utility Types
export type SxProps = MuiSxProps;
export type Theme = MuiTheme;
