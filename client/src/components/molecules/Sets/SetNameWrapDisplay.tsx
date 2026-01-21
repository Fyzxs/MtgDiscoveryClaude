import React from 'react';
import { Box, Typography } from '../../atoms';
import { useTextDistribution } from '../../../hooks/useTextDistribution';
import type { SxProps, Theme } from '@mui/material';

interface SetNameWrapDisplayProps {
  /** The set name to display */
  setName: string;
  /** The center image/icon element to wrap text around */
  centerElement: React.ReactNode;
  /** Width of the center element in pixels - used to calculate text distribution */
  centerWidth: number;
  /** Height of the center element in pixels - used for layout */
  centerHeight: number;
  /** Total container width in pixels - defaults to screen width if not provided */
  containerWidth?: number;
  /** Font size in pixels for text - defaults to 14 */
  fontSize?: number;
  /** Font weight for text - defaults to 600 */
  fontWeight?: number | string;
  /** Optional sx overrides */
  sx?: SxProps<Theme>;
}

/**
 * SetNameWrapDisplay - Displays MTG set names wrapped around a center image/icon
 *
 * Layout:
 * - Top: horizontal text centered above the image (as many words as fit)
 * - Left: vertical text (rotated 90° counter-clockwise, reads bottom-to-top)
 * - Right: vertical text (rotated 90° clockwise, reads top-to-bottom)
 * - Center: the set image/icon
 *
 * Text Distribution Algorithm:
 * 1. Fill the TOP with as many words as possible (horizontal text)
 * 2. Split remaining words EVENLY between LEFT and RIGHT sides
 * 3. ALWAYS split on word boundaries (spaces), never mid-word
 * 4. Use Canvas API to measure actual text width for accurate fitting
 *
 * Example:
 * Set name: "Avatar: The Last Airbender Beginner Box Front Cards"
 * Distribution:
 * - Top: "Avatar: The Last Airbender" (as much as fits)
 * - Left: "Beginner Box"
 * - Right: "Front Cards"
 */
export const SetNameWrapDisplay: React.FC<SetNameWrapDisplayProps> = ({
  setName,
  centerElement,
  centerWidth,
  centerHeight,
  containerWidth,
  fontSize = 14,
  fontWeight = 600,
  sx,
}) => {
  // Side text needs enough width for rotated text (line height becomes visual width)
  const sideTextWidth = fontSize * 1.4;

  // Calculate max width for top text - account for potential side text areas
  const maxTopWidth = (containerWidth ?? window.innerWidth) - (sideTextWidth * 2) - 16;

  // Get text distribution using custom hook
  const { top, left, right } = useTextDistribution({
    text: setName,
    maxTopWidth,
    fontSize,
    fontWeight,
  });

  // Calculate layout dimensions
  const topTextHeight = top ? fontSize * 1.5 : 0;
  const totalHeight = topTextHeight + centerHeight;

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        width: '100%',
        minHeight: totalHeight,
        position: 'relative',
        ...sx,
      }}
    >
      {/* Top Text - Horizontal */}
      {top && (
        <Box
          sx={{
            width: '100%',
            textAlign: 'center',
            mb: 1,
          }}
        >
          <Typography
            variant="body2"
            sx={{
              fontSize: `${fontSize}px`,
              fontWeight,
              lineHeight: 1.2,
              color: 'text.primary',
            }}
          >
            {top}
          </Typography>
        </Box>
      )}

      {/* Middle Section - Left Text, Center Image, Right Text */}
      <Box
        sx={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          gap: 0.5,
          width: '100%',
        }}
      >
        {/* Left Text - Vertical (uses writing-mode, reads bottom-to-top) */}
        {left && (
          <Box
            sx={{
              height: centerHeight,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              flexShrink: 0,
              overflow: 'hidden',
            }}
          >
            <Typography
              variant="body2"
              sx={{
                fontSize: `${fontSize}px`,
                fontWeight,
                lineHeight: 1.2,
                color: 'text.primary',
                writingMode: 'vertical-rl',
                transform: 'rotate(180deg)',
                whiteSpace: 'nowrap',
                maxHeight: centerHeight,
                overflow: 'hidden',
                textOverflow: 'ellipsis',
              }}
            >
              {left}
            </Typography>
          </Box>
        )}

        {/* Center Element */}
        <Box
          sx={{
            width: centerWidth,
            height: centerHeight,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            flexShrink: 0,
          }}
        >
          {centerElement}
        </Box>

        {/* Right Text - Vertical (uses writing-mode, reads top-to-bottom) */}
        {right && (
          <Box
            sx={{
              height: centerHeight,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              flexShrink: 0,
              overflow: 'hidden',
            }}
          >
            <Typography
              variant="body2"
              sx={{
                fontSize: `${fontSize}px`,
                fontWeight,
                lineHeight: 1.2,
                color: 'text.primary',
                writingMode: 'vertical-rl',
                whiteSpace: 'nowrap',
                maxHeight: centerHeight,
                overflow: 'hidden',
                textOverflow: 'ellipsis',
              }}
            >
              {right}
            </Typography>
          </Box>
        )}
      </Box>
    </Box>
  );
};
