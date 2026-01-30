import type { SxProps, Theme } from '@mui/material';

export const touchTargetStyles = {
  minimum: {
    minWidth: 44,
    minHeight: 44,
  } as SxProps<Theme>,

  comfortable: {
    minWidth: 48,
    minHeight: 48,
  } as SxProps<Theme>,

  large: {
    minWidth: 56,
    minHeight: 56,
  } as SxProps<Theme>,

  iconButton: {
    minWidth: 44,
    minHeight: 44,
    p: 1,
  } as SxProps<Theme>,
} as const;
