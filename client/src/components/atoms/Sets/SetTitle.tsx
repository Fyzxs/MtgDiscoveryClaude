import React from 'react';
import { Typography } from '@mui/material';

interface SetTitleProps {
  name: string;
}

export const SetTitle: React.FC<SetTitleProps> = ({ name }) => {
  return (
    <Typography
      variant="h6"
      component="div"
      sx={{
        fontWeight: 600,
        color: 'text.primary',
        mb: 2,
      }}
    >
      {name}
    </Typography>
  );
};