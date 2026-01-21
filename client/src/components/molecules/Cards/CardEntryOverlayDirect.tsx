import React, { useEffect, useRef } from 'react';
import { Box, Typography } from '@mui/material';
import {
  FINISH_DISPLAY_NAMES,
  SPECIAL_DISPLAY_NAMES
} from '../../../types/collection';
import type { CollectionEntryState } from '../../../types/collection';

interface CardEntryOverlayDirectProps {
  cardId: string;
  entryState: CollectionEntryState;
  invalidFinishFlash?: boolean;
}

export const CardEntryOverlayDirect: React.FC<CardEntryOverlayDirectProps> = ({
  cardId,
  entryState,
  invalidFinishFlash = false
}) => {
  const overlayRef = useRef<HTMLDivElement>(null);

  const displayCount = entryState.isNegative
    ? `-${entryState.count || '0'}`
    : entryState.count || '0';

  const finishText = FINISH_DISPLAY_NAMES[entryState.finish];
  const specialText = SPECIAL_DISPLAY_NAMES[entryState.special];

  // Use data attributes to control visibility via CSS
  useEffect(() => {
    if (overlayRef.current) {
      overlayRef.current.style.backgroundColor = invalidFinishFlash
        ? 'rgba(211, 47, 47, 0.95)'  // error.dark
        : 'rgba(0, 0, 0, 0.95)';

      overlayRef.current.style.borderColor = invalidFinishFlash
        ? '#f44336'  // error.main
        : '#1976d2'; // primary.main
    }
  }, [invalidFinishFlash]);

  return (
    <Box
      ref={overlayRef}
      className="card-entry-overlay"
      sx={{
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        height: '33%',
        bgcolor: 'rgba(0, 0, 0, 0.95)',
        backdropFilter: 'blur(8px)',
        display: 'none', // Hidden by default, shown via CSS
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        borderTop: '2px solid',
        borderColor: 'primary.main',
        transition: 'none', // No transition for instant show/hide
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