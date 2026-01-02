import React from 'react';
import { Box, Typography } from '../../atoms';
import type { SxProps, Theme } from '@mui/material';

interface BadgePillProps {
  content: React.ReactNode;
  background: string;
  borderRadiusLeft?: boolean;
  borderRadiusRight?: boolean;
  fontFamily?: string;
  textAlign?: 'left' | 'center' | 'right';
  sx?: SxProps<Theme>;
}

export const BadgePill: React.FC<BadgePillProps> = ({
  content,
  background,
  borderRadiusLeft = false,
  borderRadiusRight = false,
  fontFamily = 'inherit',
  textAlign = 'center',
  sx = {}
}) => {
  return (
    <Box
      sx={{
        display: 'inline-flex',
        alignItems: 'center',
        justifyContent: 'center',
        px: 0.75,
        py: 0.25,
        borderTopLeftRadius: borderRadiusLeft ? '4px' : 0,
        borderBottomLeftRadius: borderRadiusLeft ? '4px' : 0,
        borderTopRightRadius: borderRadiusRight ? '4px' : 0,
        borderBottomRightRadius: borderRadiusRight ? '4px' : 0,
        background,
        color: 'white',
        fontSize: '0.75rem',
        fontWeight: 'bold',
        fontFamily,
        textAlign,
        boxShadow: '0 1px 3px rgba(0, 0, 0, 0.3)',
        lineHeight: 1,
        minHeight: '18px',
        ...sx
      }}
    >
      {typeof content === 'string' ? (
        <Typography variant="caption" sx={{ fontSize: 'inherit', fontWeight: 'inherit', fontFamily: 'inherit', lineHeight: 1, display: 'block' }}>
          {content}
        </Typography>
      ) : (
        content
      )}
    </Box>
  );
};
