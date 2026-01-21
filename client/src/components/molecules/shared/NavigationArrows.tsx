import React from 'react';
import { Box, IconButton } from '@mui/material';
import NavigateBeforeIcon from '@mui/icons-material/NavigateBefore';
import NavigateNextIcon from '@mui/icons-material/NavigateNext';
import { touchTargetStyles } from '../../../styles/touchTargets';

interface NavigationArrowsProps {
  onPrevious?: () => void;
  onNext?: () => void;
  hasPrevious?: boolean;
  hasNext?: boolean;
  size?: 'small' | 'medium';
}

export const NavigationArrows: React.FC<NavigationArrowsProps> = ({
  onPrevious,
  onNext,
  hasPrevious = true,
  hasNext = true,
  size = 'small'
}) => {
  if (!onPrevious && !onNext) {
    return null;
  }

  return (
    <Box sx={{ display: 'flex', gap: 1 }}>
      {onPrevious && (
        <IconButton
          onClick={onPrevious}
          disabled={!hasPrevious}
          size={size}
          sx={touchTargetStyles.iconButton}
        >
          <NavigateBeforeIcon />
        </IconButton>
      )}
      {onNext && (
        <IconButton
          onClick={onNext}
          disabled={!hasNext}
          size={size}
          sx={touchTargetStyles.iconButton}
        >
          <NavigateNextIcon />
        </IconButton>
      )}
    </Box>
  );
};
