import React from 'react';
import { Box, LinearProgress, Typography } from '@mui/material';
import { useTheme } from '@mui/material/styles';

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

  return (
    <Box sx={{ width: '100%', mt: 1 }}>
      <Typography
        variant="body2"
        color="text.secondary"
        sx={{ fontSize: '0.75rem', mb: 0.5, textAlign: 'center' }}
      >
        {collected} of {total} set cards
      </Typography>
      <Box sx={{ position: 'relative', width: '100%' }}>
        <LinearProgress
          variant="determinate"
          value={percentage}
          sx={{
            height: 32,
            borderRadius: 1,
            bgcolor: theme.palette.grey[800],
            '& .MuiLinearProgress-bar': {
              borderRadius: 1,
              background: `linear-gradient(90deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.light} 100%)`,
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
            fontSize: '1rem',
            fontWeight: 700,
            color: 'text.primary',
            textShadow: '0 1px 3px rgba(0,0,0,0.8)'
          }}
        >
          {Math.round(percentage)}%
        </Typography>
      </Box>
    </Box>
  );
};
