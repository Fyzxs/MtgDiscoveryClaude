import React from 'react';
import { Box, Typography } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import type { CollectionEntryState } from '../../../types/collection';
import { FINISH_DISPLAY_NAMES, SPECIAL_DISPLAY_NAMES } from '../../../types/collection';

interface CollectionEntryOverlayProps {
  isVisible: boolean;
  entryState: CollectionEntryState;
  invalidFinishFlash?: boolean;
}

export const CollectionEntryOverlay: React.FC<CollectionEntryOverlayProps> = React.memo(({
  isVisible,
  entryState,
  invalidFinishFlash = false
}) => {
  const theme = useTheme();

  // Calculate display values
  const displayCount = entryState.isNegative
    ? `-${entryState.count || '0'}`
    : entryState.count || '0';

  const countColor = entryState.isNegative ? 'error.main' : 'primary.main';
  const specialText = SPECIAL_DISPLAY_NAMES[entryState.special];
  const showSpecial = specialText && specialText !== '';

  return (
    <Box
      className="card-entry-overlay"
      sx={{
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        width: '100%',
        height: '33%',
        background: invalidFinishFlash
          ? 'rgba(211, 47, 47, 0.95)'
          : 'rgba(0, 0, 0, 0.95)',
        backdropFilter: 'blur(8px)',
        borderTop: '2px solid',
        borderTopColor: invalidFinishFlash ? 'error.main' : 'primary.main',
        zIndex: 1000,
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        fontFamily: '"Roboto","Helvetica","Arial",sans-serif',
        pointerEvents: 'auto',
        boxSizing: 'border-box',
        // Use display for instant show/hide - no transitions
        display: isVisible ? 'flex' : 'none',
        // Only transition the flash effect colors
        transition: invalidFinishFlash
          ? 'background 0.15s ease, border-color 0.15s ease'
          : 'none'
      }}
    >
      {/* Count Display */}
      <Typography
        variant="h1"
        sx={{
          fontSize: '3rem',
          fontWeight: 'bold',
          color: countColor,
          lineHeight: 1,
          mb: 1,
          textAlign: 'center'
        }}
      >
        {displayCount}
      </Typography>

      {/* Finish and Special Details */}
      <Box
        sx={{
          display: 'flex',
          alignItems: 'center',
          gap: 2,
          height: '2rem'
        }}
      >
        {/* Finish Display */}
        <Typography
          variant="h6"
          sx={{
            fontSize: '1.25rem',
            fontWeight: 500,
            color: 'text.primary'
          }}
        >
          {FINISH_DISPLAY_NAMES[entryState.finish]}
        </Typography>

        {/* Divider and Special (only show if special is set) */}
        {showSpecial && (
          <>
            <Box
              sx={{
                width: 1,
                height: '1.5rem',
                bgcolor: 'divider'
              }}
            />
            <Typography
              variant="h6"
              sx={{
                fontSize: '1.25rem',
                fontWeight: 500,
                color: 'warning.main'
              }}
            >
              {specialText}
            </Typography>
          </>
        )}
      </Box>
    </Box>
  );
});