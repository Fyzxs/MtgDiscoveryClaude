import { useCallback } from 'react';
import { logger } from '../utils/logger';
import { useMutation } from '@apollo/client/react';
import { ADD_SET_GROUP_TO_USER_SET_CARD } from '../graphql/queries/userCards';
import { GET_SET_BY_CODE_WITH_GROUPINGS } from '../graphql/queries/sets';
import { useCollectorParam } from './useCollectorParam';

interface UseSetGroupToggleResult {
  toggleSetGroup: (
    setId: string,
    setCode: string,
    setGroupId: string,
    collecting: boolean,
    selectedFinishes: ('nonFoil' | 'foil' | 'etched')[]
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
    selectedFinishes: ('nonFoil' | 'foil' | 'etched')[]
  ) => {
    if (!collectorId) {
      logger.error('No collector ID available');
      return;
    }

    // PHASE 1: STUBBED - Log the call but don't actually mutate
    logger.warn('[STUB] toggleSetGroup called with finishes:', {
      setId,
      setCode,
      setGroupId,
      collecting,
      selectedFinishes,
      collectorId
    });

    // Get count from the mutation - the backend still needs it
    // TODO Phase 3: Backend will calculate this or we'll pass it differently
    const count = selectedFinishes.length > 0 ? 1 : 0; // Stub value

    // TODO Phase 3: Replace stub with real mutation that includes selectedFinishes
    try {
      await addSetGroupMutation({
        variables: {
          input: {
            setId,
            setGroupId,
            collecting,
            count
            // TODO Phase 3: Add selectedFinishes to input when backend is ready
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
