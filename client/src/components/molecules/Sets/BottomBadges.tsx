import React from 'react';
import { Box } from '@mui/material';
import { SetTypeBadge } from '../../atoms/Sets/SetTypeBadge';
import { DigitalBadge } from '../../atoms/Sets/DigitalBadge';
import { FoilOnlyBadge } from '../../atoms/Sets/FoilOnlyBadge';

interface BottomBadgesProps {
  setType: string;
  digital: boolean;
  foilOnly: boolean;
}

export const BottomBadges: React.FC<BottomBadgesProps> = ({ 
  setType, 
  digital, 
  foilOnly 
}) => {
  return (
    <Box 
      sx={{ 
        display: 'flex', 
        alignItems: 'center', 
        gap: 1, 
        flexWrap: 'wrap', 
        justifyContent: 'flex-start', 
        width: '100%', 
        maxWidth: '180px', 
        mb: 1 
      }}
    >
      <SetTypeBadge setType={setType} />
      <DigitalBadge show={digital} />
      <FoilOnlyBadge show={foilOnly} />
    </Box>
  );
};