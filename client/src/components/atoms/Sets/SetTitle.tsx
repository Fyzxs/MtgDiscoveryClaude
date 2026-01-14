import React from 'react';
import Typography from '../mui-wrappers/Typography';
import type { SxProps, Theme } from '@mui/material';

interface SetTitleProps {
  name: string;
  /** Optional sx overrides for responsive sizing */
  sx?: SxProps<Theme>;
}

export const SetTitle: React.FC<SetTitleProps> = ({ name, sx = {} }) => {
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
        fontSize: '1rem',
        ...sx
      }}
    >
      {name}
    </Typography>
  );
};