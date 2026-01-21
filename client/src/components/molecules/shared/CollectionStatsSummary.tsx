import React, { useMemo } from 'react';
import { Box, Typography, Paper, type SxProps, type Theme } from '../../atoms';
import type { MtgSet } from '../../../types/set';

interface CollectionStats {
  totalCards: number;
  uniqueOwned: number;
  totalAvailable: number;
  collectingSetsCompleted: number;
  collectingSetsTotal: number;
  setsWithAllUnique: number;
  setsWithCards: number;
  totalSets: number;
}

interface CollectionStatsSummaryProps {
  sets: MtgSet[];
  sx?: SxProps<Theme>;
}

const formatNumber = (num: number): string => {
  return num.toLocaleString();
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

      // Check if user is actively collecting this set (any group has collecting: true)
      const collectingGroups = userCollection?.collecting?.filter(c => c.collecting === true) ?? [];

      if (collectingGroups.length > 0) {
        // Calculate completion based on collected finishes in collecting groups
        const collected = collectingGroups.reduce((sum, cg) => {
          const groupData = userCollection?.groups?.find(g => g.setGroupId === cg.setGroupId);
          if (!groupData) return sum;
          const finishes = cg.collectingFinishes ?? [];
          let groupTotal = 0;
          if (finishes.includes('nonFoil')) groupTotal += groupData.group.nonFoil.cards.length;
          if (finishes.includes('foil')) groupTotal += groupData.group.foil.cards.length;
          if (finishes.includes('etched')) groupTotal += groupData.group.etched.cards.length;
          return sum + groupTotal;
        }, 0);

        const total = collectingGroups.reduce((sum, g) => {
          const finishes = g.collectingFinishes ?? [];
          let groupTotal = 0;
          if (finishes.includes('nonFoil')) groupTotal += g.counts.nonFoil;
          if (finishes.includes('foil')) groupTotal += g.counts.foil;
          if (finishes.includes('etched')) groupTotal += g.counts.etched;
          return sum + groupTotal;
        }, 0);

        // Only count as collecting if total > 0 (matches progress bar display logic)
        if (total > 0) {
          acc.collectingSetsTotal++;
          if (collected >= total) {
            acc.collectingSetsCompleted++;
          }
        }
      }

      const userUnique = userCollection?.uniqueCards ?? 0;
      if (userUnique > 0) {
        acc.setsWithCards++;
      }

      // Check if user owns all unique cards in the set (regardless of tracking)
      const setCardCount = set.cardCount ?? 0;
      if (userUnique >= setCardCount && setCardCount > 0) {
        acc.setsWithAllUnique++;
      }

      if (totalAvailable > 0) {
        acc.totalSets++;
      }

      return acc;
    },
    { totalCards: 0, uniqueOwned: 0, totalAvailable: 0, collectingSetsCompleted: 0, collectingSetsTotal: 0, setsWithAllUnique: 0, setsWithCards: 0, totalSets: 0 }
  );
};

interface StatItemProps {
  value: string;
  label: string;
  subtext?: string;
}

const StatItem: React.FC<StatItemProps> = ({ value, label, subtext }) => (
  <Box sx={{ textAlign: 'center', px: { xs: 1, sm: 1, md: 0.5, lg: 2 } }}>
    <Typography
      variant="h5"
      component="div"
      sx={{ fontWeight: 'bold', color: 'primary.main', fontSize: { md: '1.25rem', lg: '1.5rem' } }}
    >
      {value}
    </Typography>
    <Typography variant="body2" color="text.secondary" sx={{ fontSize: { md: '0.8rem', lg: '0.875rem' } }}>
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
        p: { xs: 2, md: 1.5, lg: 2 },
        mb: 2,
        mx: 'auto',
        width: 'fit-content',
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
          gap: { xs: 2, sm: 2, md: 1, lg: 3 },
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
            height: { sm: 40, md: 32, lg: 40 },
            bgcolor: 'divider'
          }}
        />

        <StatItem
          value={`${formatNumber(stats.uniqueOwned)} of ${formatNumber(stats.totalAvailable)}`}
          label="unique cards"
        />

        <Box
          sx={{
            display: { xs: 'none', sm: 'block' },
            width: '1px',
            height: { sm: 40, md: 32, lg: 40 },
            bgcolor: 'divider'
          }}
        />

        <StatItem
          value={`${formatNumber(stats.collectingSetsCompleted)} of ${formatNumber(stats.collectingSetsTotal)}`}
          label="tracked sets complete"
        />
      </Box>
    </Paper>
  );
};

export const CollectionStatsSummary = React.memo(CollectionStatsSummaryComponent);
