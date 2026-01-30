import React from 'react';
import { Box } from '../../atoms';
import { StatusBadge } from '../shared/badges';

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
        mt: 0,
        mb: 0
      }}
    >
      <StatusBadge variant="setType" setType={setType} />
      <StatusBadge variant="digital" show={digital} />
      <StatusBadge variant="foil" show={foilOnly} />
    </Box>
  );
};