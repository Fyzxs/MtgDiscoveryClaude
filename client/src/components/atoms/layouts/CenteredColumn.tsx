import React from 'react';
import { Box, type BoxProps } from '..';

/**
 * CenteredColumn - Layout atom for centered column layouts
 * Common pattern: flex column with centered alignment
 */
export const CenteredColumn: React.FC<BoxProps> = ({ children, sx = {}, ...props }) => {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        ...sx
      }}
      {...props}
    >
      {children}
    </Box>
  );
};
