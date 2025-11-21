import React, { useMemo } from 'react';
import { Box, Typography, Paper, type SxProps, type Theme } from '../../atoms';
import type { MtgSet } from '../../../types/set';

interface CollectionStats {
  totalCards: number;
  uniqueOwned: number;
  totalAvailable: number;
  completedSets: number;
  totalSets: number;
}

interface CollectionStatsSummaryProps {
  sets: MtgSet[];
  sx?: SxProps<Theme>;
}

const formatNumber = (num: number): string => {
  return num.toLocaleString();
};

const formatPercentage = (value: number, total: number): string => {
  if (total === 0) return '0';
  return Math.round((value / total) * 100).toString();
};

const computeCollectionStats = (sets: MtgSet[]): CollectionStats => {
  return sets.reduce<CollectionStats>(
    (acc, set) => {
      const userCollection = set.userCollection;

      acc.totalCards += userCollection?.totalCards ?? 0;
      acc.uniqueOwned += userCollection?.uniqueCards ?? 0;

      const totalAvailable = (set.groupings ?? []).reduce((sum, g) => {
        const counts = g.cardCounts;
        if (counts) {
          return sum + counts.nonFoil + counts.foil + counts.etched;
        }
        return sum;
      }, 0);

      acc.totalAvailable += totalAvailable;

      const userUnique = userCollection?.uniqueCards ?? 0;
      if (userUnique >= totalAvailable && totalAvailable > 0) {
        acc.completedSets++;
      }

      if (totalAvailable > 0) {
        acc.totalSets++;
      }

      return acc;
    },
    { totalCards: 0, uniqueOwned: 0, totalAvailable: 0, completedSets: 0, totalSets: 0 }
  );
};

interface StatItemProps {
  value: string;
  label: string;
  subtext?: string;
}

const StatItem: React.FC<StatItemProps> = ({ value, label, subtext }) => (
  <Box sx={{ textAlign: 'center', px: { xs: 1, sm: 2 } }}>
    <Typography
      variant="h5"
      component="div"
      sx={{ fontWeight: 'bold', color: 'primary.main' }}
    >
      {value}
    </Typography>
    <Typography variant="body2" color="text.secondary">
      {label}
    </Typography>
    {subtext && (
      <Typography variant="caption" color="text.disabled">
        {subtext}
      </Typography>
    )}
  </Box>
);

const CollectionStatsSummaryComponent: React.FC<CollectionStatsSummaryProps> = ({
  sets,
  sx = {}
}) => {
  const stats = useMemo(() => computeCollectionStats(sets), [sets]);

  return (
    <Paper
      elevation={0}
      sx={{
        p: 2,
        mb: 2,
        bgcolor: 'background.paper',
        border: '1px solid',
        borderColor: 'divider',
        borderRadius: 2,
        ...sx
      }}
    >
      <Box
        sx={{
          display: 'flex',
          flexDirection: { xs: 'column', sm: 'row' },
          justifyContent: 'center',
          alignItems: 'center',
          gap: { xs: 2, sm: 3 },
          flexWrap: 'wrap'
        }}
      >
        <StatItem
          value={formatNumber(stats.totalCards)}
          label="cards in collection"
        />

        <Box
          sx={{
            display: { xs: 'none', sm: 'block' },
            width: '1px',
            height: 40,
            bgcolor: 'divider'
          }}
        />

        <StatItem
          value={`${formatNumber(stats.uniqueOwned)} of ${formatNumber(stats.totalAvailable)}`}
          label="unique cards"
          subtext={`${formatPercentage(stats.uniqueOwned, stats.totalAvailable)}%`}
        />

        <Box
          sx={{
            display: { xs: 'none', sm: 'block' },
            width: '1px',
            height: 40,
            bgcolor: 'divider'
          }}
        />

        <StatItem
          value={`${formatNumber(stats.completedSets)} of ${formatNumber(stats.totalSets)}`}
          label="completed sets"
          subtext={`${formatPercentage(stats.completedSets, stats.totalSets)}%`}
        />
      </Box>
    </Paper>
  );
};

export const CollectionStatsSummary = React.memo(CollectionStatsSummaryComponent);
