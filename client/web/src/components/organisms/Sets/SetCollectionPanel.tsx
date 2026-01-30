import React, { useMemo, useState } from 'react';
import { logger } from '../../../utils/logger';
import { Box, IconButton, Collapse, Checkbox, FormControlLabel, Typography, CircularProgress } from '../../atoms';
import { useTheme } from '../../atoms';
import type { MtgSet } from '../../../types/set';
import type { GroupFinishProgress, CollectionGroup } from '../../../types/collection';
import { useSetGroupToggle } from '../../../hooks/useSetGroupToggle';
import { useCollectorParam } from '../../../hooks/useCollectorParam';
import { ChevronLeftIcon } from '../../atoms';
import { getDefaultCollectingFinishes } from '../../../utils/mockFinishCounts';
import { useUser } from '../../../contexts/UserContext';

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
  const { toggleSetGroup, loading } = useSetGroupToggle();
  const { isAuthenticated } = useUser();

  // Local state to track finish selections for optimistic UI updates
  // Overrides backend state until mutation completes and refetch occurs
  const [localFinishSelections, setLocalFinishSelections] = useState<Record<string, ('nonFoil' | 'foil' | 'etched')[]>>({});

  // Calculate collection progress from embedded userCollection data
  const collectionProgress = useMemo(() => {
    if (!hasCollector || !set.userCollection) {
      return undefined;
    }

    // Build detailed group information
    const groups: CollectionGroup[] = set.userCollection.collecting.map(collectingGroup => {
      const groupData = set.userCollection?.groups?.find(g => g.setGroupId === collectingGroup.setGroupId);
      const grouping = set.groupings?.find(g => g.id === collectingGroup.setGroupId);

      const nonFoilCollected = groupData?.group.nonFoil.cards.length || 0;
      const foilCollected = groupData?.group.foil.cards.length || 0;
      const etchedCollected = groupData?.group.etched.cards.length || 0;

      // Use actual finish counts from backend set metadata (grouping.cardCounts is source of truth)
      // Fall back to user collection counts if set metadata unavailable
      const finishCounts = grouping?.cardCounts || collectingGroup.counts || {
        total: 0,
        nonFoil: 0,
        foil: 0,
        etched: 0
      };

      // Get which finishes are being collected
      // Use local state if available, otherwise use from backend, otherwise default to available finishes
      const availableFinishes = getDefaultCollectingFinishes(finishCounts);
      const collectingFinishes = localFinishSelections[collectingGroup.setGroupId]
        || collectingGroup.collectingFinishes
        || availableFinishes;

      const finishes: GroupFinishProgress[] = [];

      // Only show finishes that are available (count > 0)
      if (finishCounts.nonFoil > 0) {
        finishes.push({
          finishType: 'nonFoil',
          collected: nonFoilCollected,
          total: finishCounts.nonFoil,
          percentage: finishCounts.nonFoil > 0 ? (nonFoilCollected / finishCounts.nonFoil) * 100 : 0,
          emoji: '🔹',
          isSelected: collectingFinishes.includes('nonFoil')
        });
      }

      if (finishCounts.foil > 0) {
        finishes.push({
          finishType: 'foil',
          collected: foilCollected,
          total: finishCounts.foil,
          percentage: finishCounts.foil > 0 ? (foilCollected / finishCounts.foil) * 100 : 0,
          emoji: '✨',
          isSelected: collectingFinishes.includes('foil')
        });
      }

      if (finishCounts.etched > 0) {
        finishes.push({
          finishType: 'etched',
          collected: etchedCollected,
          total: finishCounts.etched,
          percentage: finishCounts.etched > 0 ? (etchedCollected / finishCounts.etched) * 100 : 0,
          emoji: '⚡',
          isSelected: collectingFinishes.includes('etched')
        });
      }

      return {
        setGroupId: collectingGroup.setGroupId,
        displayName: grouping?.displayName || collectingGroup.setGroupId.charAt(0).toUpperCase() + collectingGroup.setGroupId.slice(1),
        isCollecting: collectingGroup.collecting,
        count: finishCounts.total,
        finishes
      };
    });

    return { groups };
  }, [hasCollector, set.userCollection, set.groupings, set.code, localFinishSelections]);

  const handleGroupToggle = async (e: React.ChangeEvent<HTMLInputElement>, groupId: string) => {
    const isCollecting = e.target.checked;

    // Get the finish counts from the set's groupings metadata (for determining available finishes)
    const grouping = set.groupings?.find(g => g.id === groupId);
    const availableFinishCounts = grouping?.cardCounts || { total: 0, nonFoil: 0, foil: 0, etched: 0 };

    // Get the collected counts from user collection data
    const groupData = set.userCollection?.groups?.find(g => g.setGroupId === groupId);
    const collectedCounts = {
      total: (groupData?.group.nonFoil.cards.length || 0) + (groupData?.group.foil.cards.length || 0) + (groupData?.group.etched.cards.length || 0),
      nonFoil: groupData?.group.nonFoil.cards.length || 0,
      foil: groupData?.group.foil.cards.length || 0,
      etched: groupData?.group.etched.cards.length || 0
    };

    // When checking: select all available finishes by default
    // When unchecking: clear all finishes
    const selectedFinishes = isCollecting
      ? getDefaultCollectingFinishes(availableFinishCounts)
      : [];

    // Update local state
    setLocalFinishSelections(prev => ({
      ...prev,
      [groupId]: selectedFinishes
    }));

    // Call mutation with collected counts (not available counts)
    await toggleSetGroup(set.id, set.code, groupId, isCollecting, selectedFinishes, collectedCounts);

    // Notify parent that group was toggled (to refresh set card)
    onGroupToggled?.();
  };

  const handleFinishToggle = async (groupId: string, finishType: 'nonFoil' | 'foil' | 'etched', isChecked: boolean) => {
    // Get current selections for this group
    const currentSelections = localFinishSelections[groupId] || [];

    // Update selections
    const newSelections = isChecked
      ? [...currentSelections, finishType]
      : currentSelections.filter(f => f !== finishType);

    // Update local state
    setLocalFinishSelections(prev => ({
      ...prev,
      [groupId]: newSelections
    }));

    // Get the collected counts from user collection data
    const groupData = set.userCollection?.groups?.find(g => g.setGroupId === groupId);
    const collectedCounts = {
      total: (groupData?.group.nonFoil.cards.length || 0) + (groupData?.group.foil.cards.length || 0) + (groupData?.group.etched.cards.length || 0),
      nonFoil: groupData?.group.nonFoil.cards.length || 0,
      foil: groupData?.group.foil.cards.length || 0,
      etched: groupData?.group.etched.cards.length || 0
    };

    // If all finishes are unchecked, auto-uncheck the group
    if (newSelections.length === 0) {
      await toggleSetGroup(set.id, set.code, groupId, false, [], collectedCounts);
      onGroupToggled?.();
    } else {
      // Just update the finish selections
      await toggleSetGroup(set.id, set.code, groupId, true, newSelections, collectedCounts);
      onGroupToggled?.();
    }

    logger.info('[SetCollectionPanel] Finish toggled:', { groupId, finishType, isChecked, newSelections });
  };

  // Only show panel when collector ID is present in URL
  if (hasCollector === null || hasCollector === undefined) {
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

      // Otherwise, create a default group (not collecting, but may have collected cards)
      // Get finish counts from backend grouping data
      const finishCounts = grouping?.cardCounts || { total: 0, nonFoil: 0, foil: 0, etched: 0 };

      // Check if there are any collected cards for this group (even if not actively collecting)
      const groupData = set.userCollection?.groups?.find(g => g.setGroupId === groupId);
      const nonFoilCollected = groupData?.group.nonFoil.cards.length || 0;
      const foilCollected = groupData?.group.foil.cards.length || 0;
      const etchedCollected = groupData?.group.etched.cards.length || 0;

      const defaultFinishes: GroupFinishProgress[] = [];

      if (finishCounts.nonFoil > 0) {
        defaultFinishes.push({
          finishType: 'nonFoil',
          collected: nonFoilCollected,
          total: finishCounts.nonFoil,
          percentage: finishCounts.nonFoil > 0 ? (nonFoilCollected / finishCounts.nonFoil) * 100 : 0,
          emoji: '🔹',
          isSelected: false
        });
      }

      if (finishCounts.foil > 0) {
        defaultFinishes.push({
          finishType: 'foil',
          collected: foilCollected,
          total: finishCounts.foil,
          percentage: finishCounts.foil > 0 ? (foilCollected / finishCounts.foil) * 100 : 0,
          emoji: '✨',
          isSelected: false
        });
      }

      if (finishCounts.etched > 0) {
        defaultFinishes.push({
          finishType: 'etched',
          collected: etchedCollected,
          total: finishCounts.etched,
          percentage: finishCounts.etched > 0 ? (etchedCollected / finishCounts.etched) * 100 : 0,
          emoji: '⚡',
          isSelected: false
        });
      }

      return {
        setGroupId: groupId,
        displayName,
        isCollecting: false,
        count: finishCounts.total,
        finishes: defaultFinishes,
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
        height: '260px'
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
            height: '260px',
            bgcolor: 'background.paper',
            border: `1px solid ${theme.palette.mtg.cardBorder}`,
            borderLeft: 'none',
            borderRadius: '0 4px 4px 0',
            p: 2,
            overflowY: 'auto',
            position: 'relative'
          }}
        >
          {/* Loading Overlay */}
          {loading && (
            <Box
              sx={{
                position: 'absolute',
                top: 0,
                left: 0,
                right: 0,
                bottom: 0,
                bgcolor: 'rgba(0, 0, 0, 0.5)',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                borderRadius: '0 4px 4px 0',
                zIndex: 1000
              }}
            >
              <CircularProgress size={48} />
            </Box>
          )}

          <Typography variant="h6" sx={{ mb: 2, fontSize: '1rem' }}>
            Collection Groups
          </Typography>

          {displayGroups.map((group) => (
            <Box key={group.setGroupId} sx={{ mb: 1 }}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={group.isCollecting}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleGroupToggle(e, group.setGroupId)}
                    size="small"
                    disabled={!isAuthenticated || loading}
                    sx={{ py: 0 }}
                  />
                }
                label={
                  <Typography variant="body2" sx={{ fontWeight: 600, fontSize: '0.875rem' }}>
                    {group.displayName}
                  </Typography>
                }
                sx={{ m: 0, py: 0 }}
              />
              {/* Nested finish checkboxes - always show, but disable when group is unchecked */}
              <Box sx={{ ml: 1 }}>
                {group.finishes.map((finish) => (
                  <FormControlLabel
                    key={finish.finishType}
                    control={
                      <Checkbox
                        checked={finish.isSelected}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                          handleFinishToggle(group.setGroupId, finish.finishType, e.target.checked)
                        }
                        size="small"
                        disabled={!isAuthenticated || !group.isCollecting || loading}
                        sx={{ py: 0 }}
                      />
                    }
                    label={
                      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <Typography variant="caption" sx={{ fontSize: '0.75rem' }}>
                          {finish.emoji} {finish.finishType === 'nonFoil' ? 'Non-foil' : finish.finishType.charAt(0).toUpperCase() + finish.finishType.slice(1)}
                        </Typography>
                        <Typography variant="caption" sx={{ fontSize: '0.75rem', color: 'text.secondary' }}>
                          {finish.collected}/{finish.total} ({Math.round(finish.percentage)}%)
                        </Typography>
                      </Box>
                    }
                    sx={{ m: 0, py: 0, ml: 1 }}
                  />
                ))}
              </Box>
            </Box>
          ))}
        </Box>
      </Collapse>
    </Box>
  );
};
