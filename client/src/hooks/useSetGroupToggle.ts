import { useCallback } from 'react';
import { logger } from '../utils/logger';
import { useMutation } from '@apollo/client/react';
import { ADD_SET_GROUP_TO_USER_SET_CARD, GET_USER_SET_CARDS } from '../graphql/queries/userCards';
import { useCollectorParam } from './useCollectorParam';

interface UseSetGroupToggleResult {
  toggleSetGroup: (setId: string, setGroupId: string, collecting: boolean, count: number) => Promise<void>;
  loading: boolean;
  error: Error | undefined;
}

export function useSetGroupToggle(): UseSetGroupToggleResult {
  const { collectorId } = useCollectorParam();

  const [addSetGroupMutation, { loading, error }] = useMutation(ADD_SET_GROUP_TO_USER_SET_CARD);

  const toggleSetGroup = useCallback(async (
    setId: string,
    setGroupId: string,
    collecting: boolean,
    count: number
  ) => {
    if (!collectorId) {
      logger.error('No collector ID available');
      return;
    }

    try {
      await addSetGroupMutation({
        variables: {
          input: {
            setId,
            setGroupId,
            collecting,
            count
          }
        },
        refetchQueries: [
          {
            query: GET_USER_SET_CARDS,
            variables: {
              setCardArgs: {
                userId: collectorId,
                setId
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
