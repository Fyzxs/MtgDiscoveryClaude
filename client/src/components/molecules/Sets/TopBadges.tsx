import React from 'react';
import { Box } from '@mui/material';
import { SetCodeBadge } from '../../atoms/Sets/SetCodeBadge';
import { ReleaseDateBadge } from '../../atoms/shared/ReleaseDateBadge';

interface TopBadgesProps {
  setCode: string;
  releaseDate: string;
}

export const TopBadges: React.FC<TopBadgesProps> = ({ setCode, releaseDate }) => {
  return (
    <Box 
      sx={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center', 
        width: '100%', 
        maxWidth: '200px', 
        mb: 1 
      }}
    >
      <SetCodeBadge code={setCode} />
      <ReleaseDateBadge date={releaseDate} />
    </Box>
  );
};