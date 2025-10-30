import React, { useMemo } from 'react';
import { logger } from '../../utils/logger';
import { Box, IconButton, Collapse, Checkbox, FormControlLabel, Typography } from '../atoms';
import { useTheme } from '../atoms';
import type { MtgSet } from '../../types/set';
import { useSetGroupToggle } from '../../hooks/useSetGroupToggle';
import { useCollectorParam } from '../../hooks/useCollectorParam';
import { ChevronLeftIcon } from '../atoms';

// Collection group types
interface GroupFinishProgress {
  finishType: 'nonFoil' | 'foil' | 'etched';
  collected: number;
  total: number;
  percentage: number;
  emoji: string;
}

interface CollectionGroup {
  setGroupId: string;
  displayName: string;
  isCollecting: boolean;
  count: number;
  finishes: GroupFinishProgress[];
}

// Extended type for display groups with sorting index
interface DisplayCollectionGroup extends CollectionGroup {
  originalIndex: number;
}

interface SetCollectionPanelProps {
  set: MtgSet;
  isExpanded: boolean;
  onToggle: () => void;
  availableGroupIds?: string[];
  onGroupToggled?: () => void;
}

export const SetCollectionPanel: React.FC<SetCollectionPanelProps> = ({
  set,
  isExpanded,
  onToggle,
  availableGroupIds,
  onGroupToggled
}) => {
  const theme = useTheme();
  const { hasCollector } = useCollectorParam();
  const { toggleSetGroup } = useSetGroupToggle();

  // Calculate collection progress from embedded userCollection data
  const collectionProgress = useMemo(() => {
    if (!hasCollector || !set.userCollection) {
      return undefined;
    }

    // Build detailed group information
    const groups: CollectionGroup[] = set.userCollection.collecting.map(collectingGroup => {
      const groupData = set.userCollection?.groups.find(g => g.rarity === collectingGroup.setGroupId);
      const grouping = set.groupings?.find(g => g.id === collectingGroup.setGroupId);

      const nonFoilCollected = groupData?.group.nonFoil.cards.length || 0;
      const foilCollected = groupData?.group.foil.cards.length || 0;
      const etchedCollected = groupData?.group.etched.cards.length || 0;

      // TODO: Get actual totals per finish from set metadata
      // For now, distribute the count evenly across finishes
      const totalPerFinish = Math.ceil(collectingGroup.count / 3);

      const finishes: GroupFinishProgress[] = [
        {
          finishType: 'nonFoil',
          collected: nonFoilCollected,
          total: totalPerFinish,
          percentage: totalPerFinish > 0 ? (nonFoilCollected / totalPerFinish) * 100 : 0,
          emoji: '🔹'
        },
        {
          finishType: 'foil',
          collected: foilCollected,
          total: totalPerFinish,
          percentage: totalPerFinish > 0 ? (foilCollected / totalPerFinish) * 100 : 0,
          emoji: '✨'
        },
        {
          finishType: 'etched',
          collected: etchedCollected,
          total: totalPerFinish,
          percentage: totalPerFinish > 0 ? (etchedCollected / totalPerFinish) * 100 : 0,
          emoji: '⚡'
        }
      ];

      return {
        setGroupId: collectingGroup.setGroupId,
        displayName: grouping?.displayName || collectingGroup.setGroupId.charAt(0).toUpperCase() + collectingGroup.setGroupId.slice(1),
        isCollecting: collectingGroup.collecting,
        count: collectingGroup.count,
        finishes
      };
    });

    return { groups };
  }, [hasCollector, set.userCollection, set.groupings]);

  const handleGroupToggle = async (e: React.ChangeEvent<HTMLInputElement>, groupId: string) => {
    const isCollecting = e.target.checked;

    // Get the count from the set's groupings metadata
    const grouping = set.groupings?.find(g => g.id === groupId);
    const count = grouping?.cardCount || 0;

    // Mutation will handle refetchQueries and update the cache
    await toggleSetGroup(set.id, set.code, groupId, isCollecting, count);

    // Notify parent that group was toggled (to refresh set card)
    onGroupToggled?.();
  };

  if (hasCollector === false) {
    return null;
  }

  // If no availableGroupIds provided, don't show anything
  if (!availableGroupIds || availableGroupIds.length === 0) {
    return null;
  }

  // Build display groups from available groups, merging with collection progress
  let displayGroups: DisplayCollectionGroup[] = [];

  try {
    displayGroups = availableGroupIds.map((groupId, originalIndex) => {
      // Find this group in collection progress (if it exists)
      const progressGroup = collectionProgress?.groups.find(g => g.setGroupId === groupId);

      // Get display name from set groupings metadata
      const grouping = set.groupings?.find(g => g.id === groupId);
      const displayName = grouping?.displayName || groupId.charAt(0).toUpperCase() + groupId.slice(1);

      // If we have progress data for this group, use it with proper display name
      if (progressGroup) {
        return {
          ...progressGroup,
          displayName,
          originalIndex
        };
      }

      // Otherwise, create a default group (not collecting, no progress)
      return {
        setGroupId: groupId,
        displayName,
        isCollecting: false,
        count: 0,
        finishes: [
          { finishType: 'nonFoil' as const, collected: 0, total: 0, percentage: 0, emoji: '🔹' },
          { finishType: 'foil' as const, collected: 0, total: 0, percentage: 0, emoji: '✨' },
          { finishType: 'etched' as const, collected: 0, total: 0, percentage: 0, emoji: '⚡' }
        ],
        originalIndex
      };
    }).sort((a, b) => {
      // Checked items go to the top
      if (a.isCollecting && !b.isCollecting) return -1;
      if (!a.isCollecting && b.isCollecting) return 1;

      // If both checked or both unchecked, maintain original order
      return a.originalIndex - b.originalIndex;
    });
  } catch (error) {
    logger.error('[SetCollectionPanel] Error building display groups:', error);
    logger.error('[SetCollectionPanel] availableGroupIds:', availableGroupIds);
    logger.error('[SetCollectionPanel] collectionProgress:', collectionProgress);
    logger.error('[SetCollectionPanel] set.groupings:', set.groupings);
    return null;
  }

  return (
    <Box
      sx={{
        display: 'flex',
        alignItems: 'flex-start',
        height: '360px'
      }}
    >
      {/* Expand/Collapse Button */}
      <IconButton
        onClick={onToggle}
        sx={{
          bgcolor: 'background.paper',
          border: `1px solid ${theme.palette.mtg.cardBorder}`,
          borderRadius: '0 4px 4px 0',
          borderLeft: 'none',
          height: '48px',
          width: '24px',
          padding: 0,
          '&:hover': {
            bgcolor: theme.palette.action.hover
          }
        }}
      >
        <ChevronLeftIcon
          sx={{
            transform: isExpanded ? 'rotate(180deg)' : 'rotate(0deg)',
            transition: 'transform 0.2s'
          }}
        />
      </IconButton>

      {/* Expanded Panel */}
      <Collapse orientation="horizontal" in={isExpanded} timeout={300}>
        <Box
          sx={{
            width: '300px',
            height: '360px',
            bgcolor: 'background.paper',
            border: `1px solid ${theme.palette.mtg.cardBorder}`,
            borderLeft: 'none',
            borderRadius: '0 4px 4px 0',
            p: 2,
            overflowY: 'auto'
          }}
        >
          <Typography variant="h6" sx={{ mb: 2, fontSize: '1rem' }}>
            Collection Groups
          </Typography>

          {displayGroups.map((group) => (
            <Box key={group.setGroupId} sx={{ mb: 3 }}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={group.isCollecting}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleGroupToggle(e, group.setGroupId)}
                    size="small"
                  />
                }
                label={
                  <Typography variant="body2" sx={{ fontWeight: 600, fontSize: '0.875rem' }}>
                    {group.displayName}
                  </Typography>
                }
                sx={{ mb: 1 }}
              />
              <Box sx={{ ml: 4 }}>
                {group.finishes.map((finish) => (
                  <Box
                    key={finish.finishType}
                    sx={{
                      display: 'flex',
                      alignItems: 'center',
                      gap: 1,
                      mb: 0.5
                    }}
                  >
                    <Typography variant="caption" sx={{ fontSize: '0.75rem', minWidth: '80px' }}>
                      {finish.emoji} {finish.finishType === 'nonFoil' ? 'Non-foil' : finish.finishType.charAt(0).toUpperCase() + finish.finishType.slice(1)}
                    </Typography>
                    <Typography variant="caption" sx={{ fontSize: '0.75rem', color: 'text.secondary' }}>
                      {finish.collected}/{finish.total} ({Math.round(finish.percentage)}%)
                    </Typography>
                  </Box>
                ))}
              </Box>
            </Box>
          ))}
        </Box>
      </Collapse>
    </Box>
  );
};
