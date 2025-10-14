import React from 'react';
import { Box, type BoxProps } from '..';

/**
 * FlexBetween - Layout atom for flex containers with space-between
 * Common pattern: display flex with space-between and centered alignment
 */
export const FlexBetween: React.FC<BoxProps> = ({ children, sx = {}, ...props }) => {
  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        ...sx
      }}
      {...props}
    >
      {children}
    </Box>
  );
};
