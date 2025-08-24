import React from 'react';
import { TextField } from '@mui/material';
import type { TextFieldProps } from '@mui/material';

interface MuiInputProps extends Omit<TextFieldProps, 'variant'> {
  variant?: 'outlined' | 'filled' | 'standard';
}

export const MuiInput: React.FC<MuiInputProps> = ({
  variant = 'outlined',
  size = 'medium',
  fullWidth = true,
  ...props
}) => {
  return (
    <TextField
      variant={variant}
      size={size}
      fullWidth={fullWidth}
      {...props}
    />
  );
};