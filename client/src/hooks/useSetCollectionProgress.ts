import { useState, useCallback, useMemo } from 'react';
import type { MtgSet } from '../types/set';
import { useCollectorParam } from './useCollectorParam';

interface FinishProgress {
  finishType: 'non-foil' | 'foil' | 'etched';
  collectedCards: number;
  totalCards: number;
  percentage: number;
  emoji: string;
}

interface GroupProgress {
  groupId: string;
  groupName: string;
  displayName: string;
  isSelected: boolean;
  finishes: FinishProgress[];
}

interface SetCollectionProgress {
  setId: string;
  setName: string;
  setTotalCards: number;
  overallUniqueCards: number;
  overallPercentage: number;
  overallTotalCards: number;
  groups: GroupProgress[];
}

interface UserSetCards {
  id: string; // setId
  partition: string; // userId
  total_cards: number;
  unique_cards: number;
  groups: {
    [groupName: string]: {
      "non-foil": string[]; // Array of cardIds
      "foil": string[];
      "etched": string[];
    }
  }
}

interface SetCollectionProgressHook {
  getCollectionProgress: (set: MtgSet) => SetCollectionProgress | undefined;
  updateGroupSelection: (setId: string, groupId: string, isSelected: boolean) => Promise<void>;
  refreshCollectionData: (setId: string) => Promise<void>;
}

// Mock data structure - in real implementation this would come from GraphQL/API
const mockUserSetCards: Record<string, UserSetCards> = {};

// Mock group selections - in real implementation this would be stored in user preferences
const mockGroupSelections: Record<string, Record<string, boolean>> = {};

export function useSetCollectionProgress(): SetCollectionProgressHook {
  const { hasCollector, collectorId } = useCollectorParam();
  const [, forceUpdate] = useState({});

  // Force re-render when state changes
  const triggerUpdate = useCallback(() => {
    forceUpdate({});
  }, []);

  const getCollectionProgress = useCallback((set: MtgSet): SetCollectionProgress | undefined => {
    if (!hasCollector || !collectorId) {
      return undefined;
    }

    // Mock data - in real implementation, fetch from GraphQL/API
    const userSetData = mockUserSetCards[set.id];
    const groupSelections = mockGroupSelections[set.id] || {};

    if (!userSetData) {
      // Return default structure if no collection data exists yet
      return {
        setId: set.id,
        setName: set.name,
        setTotalCards: set.cardCount || 0,
        overallUniqueCards: 0,
        overallPercentage: 0,
        overallTotalCards: 0,
        groups: []
      };
    }

    // Process groups and calculate progress
    const groups: GroupProgress[] = [];
    let totalSelectedUniqueCards = 0;
    let totalSelectedTotalCards = 0;

    for (const [groupName, finishData] of Object.entries(userSetData.groups)) {
      const isSelected = groupSelections[groupName] !== false; // Default to selected

      const finishes: FinishProgress[] = [
        {
          finishType: 'non-foil',
          collectedCards: finishData['non-foil'].length,
          totalCards: 100, // Mock - should come from set grouping metadata
          percentage: (finishData['non-foil'].length / 100) * 100,
          emoji: '🔹'
        },
        {
          finishType: 'foil',
          collectedCards: finishData['foil'].length,
          totalCards: 100, // Mock - should come from set grouping metadata
          percentage: (finishData['foil'].length / 100) * 100,
          emoji: '✨'
        },
        {
          finishType: 'etched',
          collectedCards: finishData['etched'].length,
          totalCards: 50, // Mock - should come from set grouping metadata
          percentage: (finishData['etched'].length / 50) * 100,
          emoji: '⚡'
        }
      ];

      groups.push({
        groupId: groupName,
        groupName,
        displayName: groupName.charAt(0).toUpperCase() + groupName.slice(1).replace('-', ' '),
        isSelected,
        finishes
      });

      // Add to totals if group is selected
      if (isSelected) {
        const groupUniqueCards = finishes.reduce((sum, finish) => sum + finish.collectedCards, 0);
        const groupTotalCards = finishes.reduce((sum, finish) => sum + finish.totalCards, 0);
        totalSelectedUniqueCards += groupUniqueCards;
        totalSelectedTotalCards += groupTotalCards;
      }
    }

    return {
      setId: set.id,
      setName: set.name,
      setTotalCards: set.cardCount || 0,
      overallUniqueCards: totalSelectedUniqueCards,
      overallPercentage: totalSelectedTotalCards > 0 ? (totalSelectedUniqueCards / totalSelectedTotalCards) * 100 : 0,
      overallTotalCards: userSetData.total_cards,
      groups
    };
  }, [hasCollector, collectorId]);

  const updateGroupSelection = useCallback(async (setId: string, groupId: string, isSelected: boolean) => {
    if (!hasCollector || !collectorId) {
      return;
    }

    // Mock implementation - in real app, this would call GraphQL mutation
    if (!mockGroupSelections[setId]) {
      mockGroupSelections[setId] = {};
    }
    mockGroupSelections[setId][groupId] = isSelected;

    // TODO: Implement GraphQL mutation to update user's group preferences
    // await updateUserSetGroupPreferences({
    //   variables: {
    //     userId: collectorId,
    //     setId,
    //     groupId,
    //     isSelected
    //   }
    // });

    triggerUpdate();
  }, [hasCollector, collectorId, triggerUpdate]);

  const refreshCollectionData = useCallback(async (setId: string) => {
    if (!hasCollector || !collectorId) {
      return;
    }

    // TODO: Implement GraphQL query to refresh collection data
    // const { data } = await refetchUserSetCards({
    //   variables: {
    //     userId: collectorId,
    //     setId
    //   }
    // });

    triggerUpdate();
  }, [hasCollector, collectorId, triggerUpdate]);

  return {
    getCollectionProgress,
    updateGroupSelection,
    refreshCollectionData
  };
}