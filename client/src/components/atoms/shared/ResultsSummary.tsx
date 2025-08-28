import React from 'react';
import { Typography, Box } from '@mui/material';

interface ResultsSummaryProps {
  showing: number;
  total: number;
  itemType?: string;
  sx?: any;
  textAlign?: 'left' | 'center' | 'right';
}

const ResultsSummaryComponent: React.FC<ResultsSummaryProps> = ({
  showing,
  total,
  itemType = 'items',
  sx = {},
  textAlign = 'left'
}) => {
  // Pluralize the item type
  const getItemLabel = () => {
    if (showing === 1 && total === 1) {
      return itemType.replace(/s$/, ''); // Remove trailing 's' for singular
    }
    return itemType;
  };

  return (
    <Box sx={{ mb: 3, textAlign, ...sx }}>
      <Typography variant="body1" color="text.secondary">
        Showing {showing} of {total} {getItemLabel()}
      </Typography>
    </Box>
  );
};

/**
 * Memoized ResultsSummary component
 * Only re-renders when counts change
 */
export const ResultsSummary = React.memo(ResultsSummaryComponent);