import React from 'react';
import Box from '../mui-wrappers/Box';
import LinearProgress from '../mui-wrappers/LinearProgress';
import Typography from '../mui-wrappers/Typography';
import { useTheme, alpha } from '@mui/material';

interface CollectionProgressBarProps {
  collected: number;
  total: number;
  percentage: number;
  /** Compact mode for smaller cards */
  compact?: boolean;
  /** Show tick marks at 5% and 10% intervals */
  showTicks?: boolean;
}

export const CollectionProgressBar: React.FC<CollectionProgressBarProps> = ({
  collected,
  total,
  percentage,
  compact = false,
  showTicks = false,
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

  // Compact sizing
  const barHeight = compact ? 20 : 32;
  const labelFontSize = compact ? '0.625rem' : '0.75rem';
  const percentFontSize = compact ? '0.75rem' : (isNotCollecting ? '0.875rem' : '1rem');

  return (
    <Box sx={{ width: '100%', mt: compact ? 0.5 : 1 }}>
      {!compact && (
        <Typography
          variant="body2"
          color="text.secondary"
          sx={{ fontSize: labelFontSize, mb: 0.5, textAlign: 'center' }}
        >
          {isNotCollecting ? `${collected} unique` : `${collected} of ${total} set cards`}
        </Typography>
      )}
      <Box sx={{ position: 'relative', width: '100%' }}>
        <LinearProgress
          variant="determinate"
          value={percentage}
          sx={{
            height: barHeight,
            borderRadius: 1,
            bgcolor: isNotCollecting ? theme.palette.grey[800] : alpha(theme.palette.primary.main, 0.15),
            '& .MuiLinearProgress-bar': {
              borderRadius: 1,
              background: `linear-gradient(90deg, ${progressColor.start} 0%, ${progressColor.end} 100%)`,
            }
          }}
        />
        {showTicks && (
          <Box
            sx={{
              position: 'absolute',
              top: 0,
              left: 0,
              right: 0,
              bottom: 0,
              pointerEvents: 'none',
              zIndex: 1,
            }}
          >
            {/* Generate tick marks at 5% intervals */}
            {Array.from({ length: 19 }, (_, i) => {
              const pct = (i + 1) * 5;
              const isMajor = pct % 10 === 0;
              return (
                <Box
                  key={pct}
                  sx={{
                    position: 'absolute',
                    left: `${pct}%`,
                    top: 0,
                    width: 2,
                    height: isMajor ? barHeight * 0.8 : barHeight * 0.4,
                    backgroundColor: 'rgba(0, 0, 0, 0.5)',
                    boxShadow: '1px 0 0 rgba(255, 255, 255, 0.3)',
                  }}
                />
              );
            })}
          </Box>
        )}
        <Typography
          variant="h6"
          sx={{
            position: 'absolute',
            top: '50%',
            left: '50%',
            transform: 'translate(-50%, -50%)',
            fontSize: percentFontSize,
            fontWeight: 700,
            color: 'text.primary',
            textShadow: '0 1px 3px rgba(0,0,0,0.8)',
            whiteSpace: 'nowrap',
            zIndex: 2,
          }}
        >
          {isNotCollecting
            ? (compact ? '—' : 'Not Tracked')
            : `${Math.round(percentage)}%`
          }
        </Typography>
      </Box>
    </Box>
  );
};
