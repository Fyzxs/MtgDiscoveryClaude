import React, { createContext, useContext, useRef, useCallback, useState } from 'react';
import { logger } from '../utils/logger';
import { NotificationToastStack } from '../components/organisms/shared/NotificationToastStack';
import type { NotificationToastStackRef } from '../components/organisms/shared/NotificationToastStack';
import type { CardCollectionUpdate } from '../types/collection';
import { useMutation, useApolloClient } from '@apollo/client/react';
import { gql } from '@apollo/client';
import { ADD_CARD_TO_COLLECTION } from '../graphql/mutations/addCardToCollection';
import { GET_SET_BY_CODE_WITH_GROUPINGS } from '../graphql/queries/sets';
import { useUser } from './UserContext';
import { useCollectorParam } from '../hooks/useCollectorParam';
import { perfMonitor } from '../utils/performanceMonitor';

interface CollectionContextValue {
  submitCollectionUpdate: (update: CardCollectionUpdate, cardName?: string) => Promise<void>;
  isAnyCardEntering: boolean;
  setIsAnyCardEntering: (isEntering: boolean) => void;
  getLastDelta: (cardId: string) => number | undefined;
  lastDeltaVersion: number;
}

interface UserCollectionItem {
  finish: string;
  special: string;
  count: number;
}

interface CachedCard {
  userCollection?: UserCollectionItem[];
}

interface AddCardMutationResponse {
  addCardToCollection?: {
    __typename: string;
    data?: Array<{
      userCollection: UserCollectionItem[];
    }>;
    status?: {
      message: string;
    };
  };
}

const CollectionContext = createContext<CollectionContextValue | undefined>(undefined);

// eslint-disable-next-line react-refresh/only-export-components -- Standard pattern: export hook with Provider
export const useCollection = () => {
  const context = useContext(CollectionContext);
  if (!context) {
    throw new Error('useCollection must be used within CollectionProvider');
  }
  return context;
};

interface CollectionProviderProps {
  children: React.ReactNode;
}

// OPTIMIZATION: Static maps lifted to module scope to prevent recreation
const FINISH_MAP: Record<string, string> = {
  'non-foil': 'nonfoil',
  'foil': 'foil',
  'etched': 'etched'
};

const SPECIAL_MAP: Record<string, string> = {
  'none': 'none',
  'signed': 'signed',
  'proof': 'proof',
  'altered': 'altered'
};

export const CollectionProvider: React.FC<CollectionProviderProps> = ({ children }) => {
  const apolloClient = useApolloClient();
  const toastStackRef = useRef<NotificationToastStackRef>(null);
  const [isAnyCardEntering, setIsAnyCardEntering] = useState(false);
  const { userProfile } = useUser();
  const { collectorId } = useCollectorParam();
  const [addCardToCollection] = useMutation(ADD_CARD_TO_COLLECTION);

  // Track last modification delta per card (for LastDeltaBadge)
  const lastDeltaMapRef = useRef<Map<string, number>>(new Map());
  const [lastDeltaVersion, setLastDeltaVersion] = useState(0);

  const getLastDelta = useCallback((cardId: string): number | undefined => {
    return lastDeltaMapRef.current.get(cardId);
  }, []);

  // REMOVED: Flash animation - now handled directly in MtgCard via DOM for performance

  const submitCollectionUpdate = useCallback(async (update: CardCollectionUpdate, cardName?: string) => {
    perfMonitor.start('collection-update-total');

    // Save previous delta for rollback on failure (before try block for catch access)
    const previousDelta = lastDeltaMapRef.current.get(update.cardId);
    let deltaWasSet = false;
    // Save original collection for rollback on failure
    let originalCollection: UserCollectionItem[] | null = null;

    try {
      // Fast-path validation - moved before any processing
      if (!userProfile?.id) {
        throw new Error('User not authenticated');
      }
      // Use ctor from URL if present, otherwise use logged-in user's ID
      const targetUserId = collectorId || userProfile.id;

      // Store last delta for this card (for LastDeltaBadge display)
      lastDeltaMapRef.current.set(update.cardId, update.count);
      deltaWasSet = true;
      setLastDeltaVersion(v => v + 1);

      // OPTIMIZATION: Use static maps from module scope
      const variables = perfMonitor.measure('collection-prepare-variables', () => ({
        args: {
          cardId: update.cardId,
          setId: update.setId,
          userId: targetUserId,
          userCardDetails: {
            finish: FINISH_MAP[update.finish] || 'nonfoil',
            special: SPECIAL_MAP[update.special] || 'none',
            count: update.count,
            setGroupId: update.setGroupId
          }
        }
      }));

      // INSTANT UI UPDATE: Get current collection from cache and update it optimistically
      // This preserves other finish types (foil, etched, etc.) while updating the target one
      const cacheId = apolloClient.cache.identify({ __typename: 'Card', id: update.cardId });
      const cachedCard = cacheId ? apolloClient.cache.readFragment({
        id: cacheId,
        fragment: gql`
          fragment CardCollection on Card {
            userCollection {
              finish
              special
              count
            }
          }
        `
      }) : null;

      const currentCollection = (cachedCard as CachedCard)?.userCollection || [];
      // Save for rollback
      originalCollection = [...currentCollection];
      const targetFinish = FINISH_MAP[update.finish] || 'nonfoil';
      const targetSpecial = SPECIAL_MAP[update.special] || 'none';

      // Find existing entry and ADD to its count (update.count is a delta, not absolute)
      let updated = false;
      const optimisticUserCollection = currentCollection.map((item: UserCollectionItem) => {
        if (item.finish === targetFinish && item.special === targetSpecial) {
          updated = true;
          // Add the delta to the existing count
          return { ...item, count: item.count + update.count };
        }
        return item;
      });

      // If no matching entry was found, create new entry with the delta as initial count
      if (!updated) {
        optimisticUserCollection.push({
          finish: targetFinish,
          special: targetSpecial,
          count: update.count
        });
      }

      window.dispatchEvent(new CustomEvent('collection-updated', {
        detail: {
          cardId: update.cardId,
          setId: update.setId,
          userCollection: optimisticUserCollection
        }
      }));

      // Execute mutation in background (UI already updated above)
      perfMonitor.start('collection-mutation');
      // console.time('APOLLO_MUTATION');
      const mutationStart = performance.now();
      const result = await addCardToCollection({ variables });
      const mutationEnd = performance.now();
      // // console.timeEnd('APOLLO_MUTATION');
      logger.debug(`[TIMING] Mutation took ${mutationEnd - mutationStart}ms`);
      perfMonitor.end('collection-mutation');

      perfMonitor.end('collection-update-total');

      // Check if mutation was successful
      if ((result.data as AddCardMutationResponse)?.addCardToCollection?.__typename === 'CardsSuccessResponse') {
        const updatedCard = (result.data as AddCardMutationResponse).addCardToCollection?.data?.[0];

        if (updatedCard) {
          // Dispatch again with real data from server (in case it differs from optimistic)
          queueMicrotask(() => {
            window.dispatchEvent(new CustomEvent('collection-updated', {
              detail: {
                cardId: update.cardId,
                setId: update.setId,
                userCollection: updatedCard.userCollection
              }
            }));
          });
        }

        // Refetch set data so MtgSetCard updates with new collection counts
        if (update.setCode) {
          queueMicrotask(() => {
            apolloClient.query({
              query: GET_SET_BY_CODE_WITH_GROUPINGS,
              variables: {
                codes: {
                  setCodes: [update.setCode],
                  userId: targetUserId
                }
              },
              fetchPolicy: 'network-only'
            }).catch(err => logger.error('Failed to refetch set data:', err));
          });
        }

        // OPTIMIZATION: Defer toast to prevent blocking
        queueMicrotask(() => {
          toastStackRef.current?.addToast({
            type: 'success',
            count: update.count,
            finish: update.finish,
            special: update.special,
            cardName
          });
        });
      } else {
        const errorMessage = (result.data as AddCardMutationResponse)?.addCardToCollection?.__typename === 'FailureResponse'
          ? (result.data as AddCardMutationResponse).addCardToCollection?.status?.message || 'Failed to update collection'
          : 'Failed to update collection';
        throw new Error(errorMessage);
      }
    } catch (error) {
      perfMonitor.end('collection-update-total');
      logger.error('Collection update failed:', error);

      // Revert delta badge to previous state on failure (only if we set it)
      if (deltaWasSet) {
        if (previousDelta === undefined) {
          lastDeltaMapRef.current.delete(update.cardId);
        } else {
          lastDeltaMapRef.current.set(update.cardId, previousDelta);
        }
        setLastDeltaVersion(v => v + 1);
      }

      // Revert optimistic UI update by dispatching original collection
      if (originalCollection !== null) {
        window.dispatchEvent(new CustomEvent('collection-updated', {
          detail: {
            cardId: update.cardId,
            setId: update.setId,
            userCollection: originalCollection
          }
        }));
      }

      let errorMessage = 'Update failed';
      if (error instanceof Error) {
        errorMessage = error.message;
        // Check for authentication-related errors
        if (error.message.includes('401') || error.message.includes('Unauthorized')) {
          errorMessage = 'Authentication required. Please log in again.';
        } else if (error.message.includes('403') || error.message.includes('Forbidden')) {
          errorMessage = 'Access denied. Please check your permissions.';
        }
      }

      // OPTIMIZATION: Defer error toast
      queueMicrotask(() => {
        toastStackRef.current?.addToast({
          type: 'error',
          count: update.count,
          finish: update.finish,
          special: update.special,
          cardName,
          errorMessage
        });
      });
      throw error;
    }
  }, [addCardToCollection, userProfile, collectorId, apolloClient.cache]);

  const value: CollectionContextValue = {
    submitCollectionUpdate,
    isAnyCardEntering,
    setIsAnyCardEntering,
    getLastDelta,
    lastDeltaVersion
  };

  return (
    <CollectionContext.Provider value={value}>
      {children}
      <NotificationToastStack ref={toastStackRef} />
    </CollectionContext.Provider>
  );
};