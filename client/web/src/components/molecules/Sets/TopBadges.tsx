import React from 'react';
import { Box } from '../../atoms';
import { AppBadge, DateBadge } from '../shared/badges';

interface TopBadgesProps {
  setCode: string;
  releaseDate: string;
  /** Compact mode for smaller cards - shows only set code */
  compact?: boolean;
}

export const TopBadges: React.FC<TopBadgesProps> = ({ setCode, releaseDate, compact = false }) => {
  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: compact ? 'center' : 'space-between',
        alignItems: 'center',
        width: '100%',
        mb: 0,
        gap: compact ? 1 : 0,
      }}
    >
      <AppBadge variant="code" label={setCode} compact={compact} />
      {!compact && <DateBadge variant="chip" date={releaseDate} />}
    </Box>
  );
};