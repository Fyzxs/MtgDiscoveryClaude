import React from 'react';
import { Box, useTheme } from '@mui/material';

interface ProgressRingProps {
  /** Progress percentage (0-100) */
  percentage: number;
  /** Size of the ring in pixels */
  size: number;
  /** Stroke width in pixels (default: 8) */
  strokeWidth?: number;
  /** Show dashed background ring (default: true) */
  showDashes?: boolean;
  /** Children to render in the center of the ring */
  children?: React.ReactNode;
}

export const ProgressRing: React.FC<ProgressRingProps> = ({
  percentage,
  size,
  strokeWidth = 8,
  showDashes = true,
  children,
}) => {
  const theme = useTheme();

  // Calculate SVG parameters
  const radius = (size - strokeWidth) / 2;
  const circumference = 2 * Math.PI * radius;
  const strokeDashoffset = circumference - (percentage / 100) * circumference;

  // Determine color based on percentage
  const getProgressColor = (): string => {
    if (percentage >= 100) {
      return theme.palette.success.main;
    }
    if (percentage > 75) {
      return theme.palette.secondary.main;
    }
    return theme.palette.primary.main;
  };

  // Background ring color
  const getBgRingColor = (): string => {
    if (percentage >= 100) {
      return theme.palette.success.dark;
    }
    return theme.palette.grey[700];
  };

  return (
    <Box
      sx={{
        position: 'relative',
        width: size,
        height: size,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
      }}
    >
      <svg
        width={size}
        height={size}
        style={{
          position: 'absolute',
          top: 0,
          left: 0,
          transform: 'rotate(-90deg)',
        }}
      >
        {/* Background ring */}
        <circle
          cx={size / 2}
          cy={size / 2}
          r={radius}
          fill="none"
          stroke={getBgRingColor()}
          strokeWidth={strokeWidth}
          strokeDasharray={showDashes && percentage < 100 ? '6 4' : undefined}
          opacity={0.6}
        />
        {/* Progress ring */}
        <circle
          cx={size / 2}
          cy={size / 2}
          r={radius}
          fill="none"
          stroke={getProgressColor()}
          strokeWidth={strokeWidth}
          strokeLinecap="round"
          strokeDasharray={circumference}
          strokeDashoffset={strokeDashoffset}
          style={{
            transition: 'stroke-dashoffset 0.5s ease, stroke 0.3s ease',
          }}
        />
      </svg>
      {/* Center content */}
      <Box sx={{ position: 'relative', zIndex: 1 }}>
        {children}
      </Box>
    </Box>
  );
};
