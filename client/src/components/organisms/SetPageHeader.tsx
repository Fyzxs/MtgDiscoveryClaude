import React from 'react';
import { Box, Typography } from '@mui/material';
import { MtgSetCard } from '../molecules/Sets/MtgSetCard';
import { SectionErrorBoundary } from '../ErrorBoundaries';
import type { MtgSet } from '../../types/set';

interface SetPageHeaderProps {
  setInfo?: MtgSet;
  setName: string;
  setCode: string;
}

export const SetPageHeader: React.FC<SetPageHeaderProps> = ({
  setInfo,
  setName,
  setCode
}) => {
  return (
    <>
      {/* Set Information Card */}
      {setInfo && (
        <SectionErrorBoundary name="SetInfoCard">
          <Box sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}>
            <MtgSetCard set={setInfo} />
          </Box>
        </SectionErrorBoundary>
      )}

      {/* Fallback title if no set info */}
      {!setInfo && (
        <Typography variant="h3" component="h1" gutterBottom sx={{ mb: 4, textAlign: 'center' }}>
          {setName} ({setCode.toUpperCase()})
        </Typography>
      )}
    </>
  );
};