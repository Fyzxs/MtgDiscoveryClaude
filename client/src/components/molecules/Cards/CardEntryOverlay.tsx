import React from 'react';
import { Box, Typography } from '@mui/material';
import {
  FINISH_DISPLAY_NAMES,
  SPECIAL_DISPLAY_NAMES
} from '../../../types/collection';
import type { CollectionEntryState } from '../../../types/collection';

interface CardEntryOverlayProps {
  isVisible: boolean;
  entryState: CollectionEntryState;
  invalidFinishFlash?: boolean;
}

export const CardEntryOverlay: React.FC<CardEntryOverlayProps> = ({
  isVisible,
  entryState,
  invalidFinishFlash = false
}) => {

  const displayCount = entryState.isNegative
    ? `-${entryState.count || '0'}`
    : entryState.count || '0';

  const finishText = FINISH_DISPLAY_NAMES[entryState.finish];
  const specialText = SPECIAL_DISPLAY_NAMES[entryState.special];

  if (!isVisible) return null;

  return (
    <Box
      sx={{
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        height: '33%',
        bgcolor: invalidFinishFlash
          ? 'error.dark'
          : 'rgba(0, 0, 0, 0.95)',
        backdropFilter: 'blur(8px)',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        borderTop: '2px solid',
        borderColor: invalidFinishFlash
          ? 'error.main'
          : 'primary.main',
        transition: 'background-color 0.15s ease',
        zIndex: 100
      }}
    >
        {/* Count Display */}
        <Typography
          variant="h1"
          sx={{
            fontSize: { xs: '3rem', sm: '4rem' },
            fontWeight: 'bold',
            color: entryState.isNegative ? 'error.main' : 'primary.main',
            lineHeight: 1,
            mb: 1
          }}
        >
          {displayCount}
        </Typography>

        {/* Finish and Special Display */}
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            gap: 2,
            height: '2rem'
          }}
        >
          <Typography
            variant="h6"
            sx={{
              color: 'text.primary',
              fontWeight: 500
            }}
          >
            {finishText}
          </Typography>

          {specialText && (
            <>
              <Box
                sx={{
                  width: '1px',
                  height: '1.5rem',
                  bgcolor: 'divider'
                }}
              />
              <Typography
                variant="h6"
                sx={{
                  color: 'warning.main',
                  fontWeight: 500
                }}
              >
                {specialText}
              </Typography>
            </>
          )}
        </Box>
      </Box>
  );
};