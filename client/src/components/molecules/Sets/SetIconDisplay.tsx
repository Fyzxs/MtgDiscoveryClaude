import React from 'react';
import { Box } from '../../atoms';

interface SetIconDisplayProps {
  iconSvgUri?: string;
  setName: string;
  borderColor: string;
  /** Size of the icon container in pixels - defaults to 140 */
  size?: number;
}

export const SetIconDisplay: React.FC<SetIconDisplayProps> = ({
  iconSvgUri,
  setName,
  borderColor,
  size = 140
}) => {
  if (!iconSvgUri) {
    return null;
  }

  // Scale settings based on size
  const isCompact = size < 60;
  const borderWidth = isCompact ? 2 : 3;
  const borderRadius = isCompact ? 4 : 8;
  // Tighter inner size for compact (92% vs 85%)
  const innerSize = Math.floor(size * (isCompact ? 0.92 : 0.85));

  return (
    <Box
      sx={{
        width: size,
        height: size,
        border: `${borderWidth}px solid ${borderColor}`,
        borderRadius: `${borderRadius}px`,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        backgroundColor: 'rgba(255, 255, 255, 0.02)',
        transition: 'border-color 0.3s ease',
        flexShrink: 0,
      }}
    >
      <Box
        component="img"
        src={iconSvgUri}
        alt={`${setName} icon`}
        loading="lazy"
        sx={{
          maxWidth: innerSize,
          maxHeight: innerSize,
          width: 'auto',
          height: 'auto',
          objectFit: 'contain',
          filter: `brightness(0) invert(1) drop-shadow(0 ${isCompact ? 2 : 4}px ${isCompact ? 4 : 8}px rgba(0,0,0,0.5))`,
        }}
      />
    </Box>
  );
};