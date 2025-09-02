import React from 'react';
import { Button as MuiButtonBase, CircularProgress } from '@mui/material';
import type { ButtonProps as MuiButtonProps } from '@mui/material';

interface AppButtonProps extends Omit<MuiButtonProps, 'size'> {
  isLoading?: boolean;
  size?: 'small' | 'medium' | 'large';
}

export const AppButton: React.FC<AppButtonProps> = ({
  children,
  isLoading = false,
  disabled,
  size = 'medium',
  variant = 'contained',
  ...props
}) => {
  return (
    <MuiButtonBase
      variant={variant}
      size={size}
      disabled={disabled || isLoading}
      startIcon={isLoading ? <CircularProgress size={16} /> : undefined}
      {...props}
    >
      {isLoading ? 'Loading...' : children}
    </MuiButtonBase>
  );
};