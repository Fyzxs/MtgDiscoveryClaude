import React from 'react';
import Typography from '../Typography';
import type { SxProps, Theme } from '@mui/material';

interface SetTitleProps {
  name: string;
  /** Optional sx overrides for responsive sizing */
  sx?: SxProps<Theme>;
}

export const SetTitle: React.FC<SetTitleProps> = ({ name, sx }) => {
  // Calculate font size based on name length
  const getFontSize = () => {
    const length = name.length;
    if (length < 20) return '1.1rem';
    if (length < 30) return '1.05rem';
    if (length < 40) return '1rem';
    if (length < 50) return '0.95rem';
    return '0.9rem';
  };

  return (
    <Typography
      variant="h6"
      component="div"
      sx={{
        fontWeight: 600,
        color: 'text.primary',
        mb: 1,
        minHeight: '48px',
        display: '-webkit-box',
        alignItems: 'center',
        justifyContent: 'center',
        overflow: 'hidden',
        textOverflow: 'ellipsis',
        WebkitLineClamp: 2,
        WebkitBoxOrient: 'vertical',
        lineHeight: 1.2,
        fontSize: getFontSize(),
        ...sx,
      }}
    >
      {name}
    </Typography>
  );
};