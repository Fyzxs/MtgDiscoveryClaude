import React from 'react';
import Typography from '../Typography';

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
        fontSize: '1.1rem',
      }}
    >
      {name}
    </Typography>
  );
};