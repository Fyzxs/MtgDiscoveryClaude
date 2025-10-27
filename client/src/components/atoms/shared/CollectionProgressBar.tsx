import React from 'react';
import Box from '../Box';
import LinearProgress from '../LinearProgress';
import Typography from '../Typography';
import { useTheme, alpha } from '@mui/material';

interface CollectionProgressBarProps {
  collected: number;
  total: number;
  percentage: number;
}

export const CollectionProgressBar: React.FC<CollectionProgressBarProps> = ({
  collected,
  total,
  percentage
}) => {
  const theme = useTheme();
  const isNotCollecting = total === 0 && percentage === 0;

  const getProgressColor = () => {
    if (percentage >= 100) {
      return {
        start: theme.palette.success.dark,
        end: theme.palette.success.light
      };
    }
    if (percentage > 75) {
      return {
        start: theme.palette.secondary.dark,
        end: theme.palette.secondary.light
      };
    }
    return {
      start: theme.palette.primary.main,
      end: theme.palette.primary.light
    };
  };

  const progressColor = getProgressColor();

  return (
    <Box sx={{ width: '100%', mt: 1 }}>
      <Typography
        variant="body2"
        color="text.secondary"
        sx={{ fontSize: '0.75rem', mb: 0.5, textAlign: 'center' }}
      >
        {isNotCollecting ? 'No groups selected' : `${collected} of ${total} set cards`}
      </Typography>
      <Box sx={{ position: 'relative', width: '100%' }}>
        <LinearProgress
          variant="determinate"
          value={percentage}
          sx={{
            height: 32,
            borderRadius: 1,
            bgcolor: isNotCollecting ? theme.palette.grey[800] : alpha(theme.palette.primary.main, 0.15),
            '& .MuiLinearProgress-bar': {
              borderRadius: 1,
              background: `linear-gradient(90deg, ${progressColor.start} 0%, ${progressColor.end} 100%)`,
            }
          }}
        />
        <Typography
          variant="h6"
          sx={{
            position: 'absolute',
            top: '50%',
            left: '50%',
            transform: 'translate(-50%, -50%)',
            fontSize: isNotCollecting ? '0.875rem' : '1rem',
            fontWeight: 700,
            color: 'text.primary',
            textShadow: '0 1px 3px rgba(0,0,0,0.8)'
          }}
        >
          {isNotCollecting ? 'Not Collecting' : `${Math.round(percentage)}%`}
        </Typography>
      </Box>
    </Box>
  );
};
