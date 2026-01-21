import { useCallback } from 'react';
import { logger } from '../utils/logger';
import type { MtgSet } from '../types/set';
import { useCollectorParam } from './useCollectorParam';

export interface GroupFinishProgress {
  finishType: 'nonFoil' | 'foil' | 'etched';
  collected: number;
  total: number;
  percentage: number;
  emoji: string;
}

export interface CollectionGroup {
  setGroupId: string;
  displayName: string;
  isCollecting: boolean;
  count: number;
  finishes: GroupFinishProgress[];
}

export interface SetCollectionProgress {
  setId: string;
  setName: string;
  setTotalCards: number;
  uniqueCards: number;
  totalCards: number;
  percentage: number;
  groups: CollectionGroup[];
}

interface UserSetCardData {
  userId: string;
  setId: string;
  totalCards: number;
  uniqueCards: number;
  collecting: {
    setGroupId: string;
    collecting: boolean;
    count: number;
    collectingFinishes: string[];
  }[];
  groups: {
    setGroupId: string;
    group: {
      nonFoil: { cards: string[] };
      foil: { cards: string[] };
      etched: { cards: string[] };
    };
  }[];
}

interface UserSetCardResponse {
  userSetCards: {
    __typename: string;
    data?: UserSetCardData;
    status?: {
      message: string;
      statusCode: number;
    };
  };
}

interface SetCollectionProgressHook {
  getCollectionProgress: (set: MtgSet, userCollection?: UserSetCardData) => SetCollectionProgress | undefined;
}

export function useSetCollectionProgress(): SetCollectionProgressHook {
  const { hasCollector } = useCollectorParam();

  const getCollectionProgress = useCallback((set: MtgSet, userCollection?: UserSetCardData): SetCollectionProgress | undefined => {
    if (!hasCollector || !userCollection) {
      return undefined;
    }

    try {
      const userSetData = userCollection;

      // Build detailed group information for ALL groups (not just collecting ones)
      const groups: CollectionGroup[] = userSetData.collecting.map(collectingGroup => {
        const groupData = userSetData.groups.find(g => g.setGroupId === collectingGroup.setGroupId);

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
          displayName: collectingGroup.setGroupId.charAt(0).toUpperCase() + collectingGroup.setGroupId.slice(1),
          isCollecting: collectingGroup.collecting,
          count: collectingGroup.count,
          finishes
        };
      });

      // Filter to only groups that are being collected
      const collectingGroups = userSetData.collecting.filter(g => g.collecting === true);

      // Calculate actual cards collected in tracking groups (only those with collecting: true)
      // Only count finishes that the user is collecting
      const collectedInTrackingGroups = collectingGroups.reduce((sum, collectingGroup) => {
        const groupData = userSetData.groups.find(g => g.setGroupId === collectingGroup.setGroupId);
        if (!groupData) {
          return sum;
        }

        const collectingFinishes = collectingGroup.collectingFinishes || [];
        let groupCollected = 0;

        if (collectingFinishes.includes('nonFoil')) {
          groupCollected += groupData.group.nonFoil.cards.length;
        }
        if (collectingFinishes.includes('foil')) {
          groupCollected += groupData.group.foil.cards.length;
        }
        if (collectingFinishes.includes('etched')) {
          groupCollected += groupData.group.etched.cards.length;
        }

        return sum + groupCollected;
      }, 0);

      // Total available cards in tracking groups (only those with collecting: true)
      // Multiply count by the number of finishes being collected
      const totalAvailableInTrackingGroups = collectingGroups.reduce((sum, g) => {
        const finishCount = (g.collectingFinishes || []).length;
        return sum + (g.count * finishCount);
      }, 0);

      // If no groups are being collected, show 0% but still return the groups
      const percentage = totalAvailableInTrackingGroups > 0
        ? (collectedInTrackingGroups / totalAvailableInTrackingGroups) * 100
        : 0;

      return {
        setId: set.id,
        setName: set.name,
        setTotalCards: totalAvailableInTrackingGroups,
        uniqueCards: collectedInTrackingGroups,
        totalCards: userSetData.totalCards,
        percentage,
        groups
      };
    } catch (error) {
      logger.error('Error in getCollectionProgress:', error);
      return undefined;
    }
  }, [hasCollector]);

  return {
    getCollectionProgress
  };
}