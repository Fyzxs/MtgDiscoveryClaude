import { useCallback } from 'react';
import { logger } from '../utils/logger';
import { useMutation } from '@apollo/client/react';
import { ADD_SET_GROUP_TO_USER_SET_CARD } from '../graphql/queries/userCards';
import { GET_SET_BY_CODE_WITH_GROUPINGS } from '../graphql/queries/sets';
import { useCollectorParam } from './useCollectorParam';

interface FinishCounts {
  total: number;
  nonFoil: number;
  foil: number;
  etched: number;
}

interface UseSetGroupToggleResult {
  toggleSetGroup: (
    setId: string,
    setCode: string,
    setGroupId: string,
    collecting: boolean,
    selectedFinishes: ('nonFoil' | 'foil' | 'etched')[],
    finishCounts: FinishCounts
  ) => Promise<void>;
  loading: boolean;
  error: Error | undefined;
}

export function useSetGroupToggle(): UseSetGroupToggleResult {
  const { collectorId } = useCollectorParam();

  const [addSetGroupMutation, { loading, error }] = useMutation(ADD_SET_GROUP_TO_USER_SET_CARD);

  const toggleSetGroup = useCallback(async (
    setId: string,
    setCode: string,
    setGroupId: string,
    collecting: boolean,
    selectedFinishes: ('nonFoil' | 'foil' | 'etched')[],
    finishCounts: FinishCounts
  ) => {
    if (!collectorId) {
      logger.error('No collector ID available');
      return;
    }

    // Use the collected card counts passed from the caller
    const counts = finishCounts;

    logger.info('toggleSetGroup called:', {
      setId,
      setCode,
      setGroupId,
      collecting,
      selectedFinishes,
      counts,
      collectorId
    });

    try {
      await addSetGroupMutation({
        variables: {
          input: {
            userId: collectorId,
            setId,
            setGroupId,
            collecting,
            counts,
            collectingFinishes: selectedFinishes
          }
        },
        refetchQueries: [
          {
            query: GET_SET_BY_CODE_WITH_GROUPINGS,
            variables: {
              codes: {
                setCodes: [setCode],
                userId: collectorId
              }
            }
          }
        ],
        awaitRefetchQueries: true
      });
    } catch (err) {
      logger.error('Error toggling set group:', err);
    }
  }, [collectorId, addSetGroupMutation]);

  return {
    toggleSetGroup,
    loading,
    error: error as Error | undefined
  };
}
