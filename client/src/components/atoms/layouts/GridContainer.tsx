import React from 'react';
import { Box, type BoxProps } from '..';

interface GridContainerProps extends BoxProps {
  gap?: number | string;
  columns?: string | number;
}

/**
 * GridContainer - Layout atom for CSS grid containers
 * Common pattern: display grid with configurable gap and columns
 */
export const GridContainer: React.FC<GridContainerProps> = ({
  children,
  gap = 2,
  columns = 'auto',
  sx = {},
  ...props
}) => {
  return (
    <Box
      sx={{
        display: 'grid',
        gap,
        gridTemplateColumns: typeof columns === 'number' ? `repeat(${columns}, 1fr)` : columns,
        ...sx
      }}
      {...props}
    >
      {children}
    </Box>
  );
};
