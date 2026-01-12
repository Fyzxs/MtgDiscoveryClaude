import React, { createContext, useContext, useRef, useCallback, useState } from 'react';
import { logger } from '../utils/logger';
import { NotificationToastStack } from '../components/organisms/shared/NotificationToastStack';
import type { NotificationToastStackRef } from '../components/organisms/shared/NotificationToastStack';
import type { WishlistCardUpdate } from '../types/collection';
import { useMutation, useApolloClient } from '@apollo/client/react';
import { gql } from '@apollo/client';
import { ADD_CARD_TO_WISHLIST } from '../graphql/mutations/wishlist';
import { useUser } from './UserContext';
import { useCollectorParam } from '../hooks/useCollectorParam';
import { perfMonitor } from '../utils/performanceMonitor';

interface WishlistContextValue {
  submitWishlistUpdate: (update: WishlistCardUpdate, cardName?: string) => Promise<void>;
  isAnyCardEntering: boolean;
  setIsAnyCardEntering: (isEntering: boolean) => void;
  getLastDelta: (cardId: string) => number | undefined;
  lastDeltaVersion: number;
}

interface UserWishlistItem {
  finish: string;
  special: string;
  count: number;
}

interface CachedCard {
  userWishlist?: UserWishlistItem[];
}

interface WishlistMutationResponse {
  addCardToWishlist?: {
    __typename: string;
    data?: Array<{
      userWishlist: UserWishlistItem[];
    }>;
    status?: {
      message: string;
    };
  };
}

const WishlistContext = createContext<WishlistContextValue | undefined>(undefined);

// eslint-disable-next-line react-refresh/only-export-components -- Standard pattern: export hook with Provider
export const useWishlist = () => {
  const context = useContext(WishlistContext);
  if (!context) {
    throw new Error('useWishlist must be used within WishlistProvider');
  }
  return context;
};

interface WishlistProviderProps {
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
  'artist-proof': 'proof',
  'altered': 'altered'
};

export const WishlistProvider: React.FC<WishlistProviderProps> = ({ children }) => {
  const apolloClient = useApolloClient();
  const toastStackRef = useRef<NotificationToastStackRef>(null);
  const [isAnyCardEntering, setIsAnyCardEntering] = useState(false);
  const { userProfile } = useUser();
  const { collectorId } = useCollectorParam();
  const [addCardToWishlist] = useMutation(ADD_CARD_TO_WISHLIST);

  // Track last modification delta per card
  const lastDeltaMapRef = useRef<Map<string, number>>(new Map());
  const [lastDeltaVersion, setLastDeltaVersion] = useState(0);

  const getLastDelta = useCallback((cardId: string): number | undefined => {
    return lastDeltaMapRef.current.get(cardId);
  }, []);

  const submitWishlistUpdate = useCallback(async (update: WishlistCardUpdate, cardName?: string) => {
    perfMonitor.start('wishlist-update-total');

    // Save previous delta for rollback on failure (before try block for catch access)
    const previousDelta = lastDeltaMapRef.current.get(update.cardId);
    let deltaWasSet = false;
    // Save original wishlist for rollback on failure
    let originalWishlist: UserWishlistItem[] | null = null;

    try {
      if (!userProfile?.id) {
        throw new Error('User not authenticated');
      }
      // Use ctor from URL if present, otherwise use logged-in user's ID
      const targetUserId = collectorId || userProfile.id;

      // Store last delta for this card
      lastDeltaMapRef.current.set(update.cardId, update.count);
      deltaWasSet = true;
      setLastDeltaVersion(v => v + 1);

      const variables = perfMonitor.measure('wishlist-prepare-variables', () => ({
        args: {
          cardId: update.cardId,
          setId: update.setId,
          userId: targetUserId,
          userWishlistCardDetails: {
            finish: FINISH_MAP[update.finish] || 'nonfoil',
            special: SPECIAL_MAP[update.special] || 'none',
            count: update.count
          }
        }
      }));

      // INSTANT UI UPDATE: Get current wishlist from cache and update optimistically
      const cacheId = apolloClient.cache.identify({ __typename: 'Card', id: update.cardId });
      const cachedCard = cacheId ? apolloClient.cache.readFragment({
        id: cacheId,
        fragment: gql`
          fragment CardWishlist on Card {
            userWishlist {
              finish
              special
              count
            }
          }
        `
      }) : null;

      const currentWishlist = (cachedCard as CachedCard)?.userWishlist || [];
      // Save for rollback
      originalWishlist = [...currentWishlist];
      const targetFinish = FINISH_MAP[update.finish] || 'nonfoil';
      const targetSpecial = SPECIAL_MAP[update.special] || 'none';

      // Find existing entry and ADD to its count
      let updated = false;
      const optimisticUserWishlist = currentWishlist.map((item: UserWishlistItem) => {
        if (item.finish === targetFinish && item.special === targetSpecial) {
          updated = true;
          return { ...item, count: item.count + update.count };
        }
        return item;
      });

      // If no matching entry was found, create new entry
      if (!updated) {
        optimisticUserWishlist.push({
          finish: targetFinish,
          special: targetSpecial,
          count: update.count
        });
      }

      window.dispatchEvent(new CustomEvent('wishlist-updated', {
        detail: {
          cardId: update.cardId,
          setId: update.setId,
          userWishlist: optimisticUserWishlist
        }
      }));

      // Execute mutation in background
      perfMonitor.start('wishlist-mutation');
      const mutationStart = performance.now();
      const result = await addCardToWishlist({ variables });
      const mutationEnd = performance.now();
      logger.debug(`[TIMING] Wishlist mutation took ${mutationEnd - mutationStart}ms`);
      perfMonitor.end('wishlist-mutation');

      perfMonitor.end('wishlist-update-total');

      if ((result.data as WishlistMutationResponse)?.addCardToWishlist?.__typename === 'CardsSuccessResponse') {
        const updatedCard = (result.data as WishlistMutationResponse).addCardToWishlist?.data?.[0];

        if (updatedCard) {
          queueMicrotask(() => {
            window.dispatchEvent(new CustomEvent('wishlist-updated', {
              detail: {
                cardId: update.cardId,
                setId: update.setId,
                userWishlist: updatedCard.userWishlist
              }
            }));
          });
        }

        queueMicrotask(() => {
          toastStackRef.current?.addToast({
            type: update.count < 0 ? 'wishlist-remove' : 'wishlist',
            count: update.count,
            finish: update.finish,
            special: update.special,
            cardName
          });
        });
      } else {
        const errorMessage = (result.data as WishlistMutationResponse)?.addCardToWishlist?.__typename === 'FailureResponse'
          ? (result.data as WishlistMutationResponse).addCardToWishlist?.status?.message || 'Failed to update wishlist'
          : 'Failed to update wishlist';
        throw new Error(errorMessage);
      }
    } catch (error) {
      perfMonitor.end('wishlist-update-total');
      logger.error('Wishlist update failed:', error);

      // Revert delta badge to previous state on failure (only if we set it)
      if (deltaWasSet) {
        if (previousDelta === undefined) {
          lastDeltaMapRef.current.delete(update.cardId);
        } else {
          lastDeltaMapRef.current.set(update.cardId, previousDelta);
        }
        setLastDeltaVersion(v => v + 1);
      }

      // Revert optimistic UI update by dispatching original wishlist
      if (originalWishlist !== null) {
        window.dispatchEvent(new CustomEvent('wishlist-updated', {
          detail: {
            cardId: update.cardId,
            setId: update.setId,
            userWishlist: originalWishlist
          }
        }));
      }

      let errorMessage = 'Update failed';
      if (error instanceof Error) {
        errorMessage = error.message;
        if (error.message.includes('401') || error.message.includes('Unauthorized')) {
          errorMessage = 'Authentication required. Please log in again.';
        } else if (error.message.includes('403') || error.message.includes('Forbidden')) {
          errorMessage = 'Access denied. Please check your permissions.';
        }
      }

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
  }, [addCardToWishlist, userProfile, collectorId, apolloClient.cache]);

  const value: WishlistContextValue = {
    submitWishlistUpdate,
    isAnyCardEntering,
    setIsAnyCardEntering,
    getLastDelta,
    lastDeltaVersion
  };

  return (
    <WishlistContext.Provider value={value}>
      {children}
      <NotificationToastStack ref={toastStackRef} />
    </WishlistContext.Provider>
  );
};
