import { useCallback } from 'react';
import { useApolloClient } from '@apollo/client/react';
import { GET_USER_SET_CARDS } from '../graphql/queries/userCards';
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
  }[];
  groups: {
    rarity: string;
    group: {
      nonFoil: { cards: string[] };
      foil: { cards: string[] };
      etched: { cards: string[] };
    };
  }[];
}

interface SetCollectionProgressHook {
  getCollectionProgress: (set: MtgSet, forceRefresh?: boolean) => Promise<SetCollectionProgress | undefined>;
}

export function useSetCollectionProgress(): SetCollectionProgressHook {
  const { hasCollector, collectorId } = useCollectorParam();
  const client = useApolloClient();

  const getCollectionProgress = useCallback(async (set: MtgSet, forceRefresh = false): Promise<SetCollectionProgress | undefined> => {
    if (!hasCollector || !collectorId) {
      return undefined;
    }

    try {
      const { data } = await client.query({
        query: GET_USER_SET_CARDS,
        variables: {
          setCardArgs: {
            userId: collectorId,
            setId: set.id
          }
        },
        errorPolicy: 'all',
        fetchPolicy: forceRefresh ? 'network-only' : 'cache-first'
      });

      if (!data || data.userSetCards.__typename === 'FailureResponse') {
        return undefined;
      }

      const userSetData: UserSetCardData = data.userSetCards.data;

      if (!userSetData) {
        return undefined;
      }

      // Build detailed group information for ALL groups (not just collecting ones)
      const groups: CollectionGroup[] = userSetData.collecting.map(collectingGroup => {
        const groupData = userSetData.groups.find(g => g.rarity === collectingGroup.setGroupId);

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
      const collectedInTrackingGroups = collectingGroups.reduce((sum, collectingGroup) => {
        const groupData = userSetData.groups.find(g => g.rarity === collectingGroup.setGroupId);
        if (!groupData) {
          return sum;
        }

        const nonFoilCount = groupData.group.nonFoil.cards.length;
        const foilCount = groupData.group.foil.cards.length;
        const etchedCount = groupData.group.etched.cards.length;

        return sum + nonFoilCount + foilCount + etchedCount;
      }, 0);

      // Total available cards in tracking groups (only those with collecting: true)
      const totalAvailableInTrackingGroups = collectingGroups.reduce((sum, g) => sum + g.count, 0);

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
      console.error('Error in getCollectionProgress:', error);
      return undefined;
    }
  }, [hasCollector, collectorId, client]);

  return {
    getCollectionProgress
  };
}