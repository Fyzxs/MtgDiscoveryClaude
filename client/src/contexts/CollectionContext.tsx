import React, { createContext, useContext, useRef, useCallback, useState } from 'react';
import { NotificationToastStack } from '../components/organisms/NotificationToastStack';
import type { NotificationToastStackRef } from '../components/organisms/NotificationToastStack';
import { QuickEntryKeysFab } from '../components/molecules/Cards/QuickEntryKeysFab';
import type { CardCollectionUpdate } from '../types/collection';
import { useMutation, useApolloClient } from '@apollo/client/react';
import { ADD_CARD_TO_COLLECTION } from '../graphql/mutations/addCardToCollection';
import { useUser } from './UserContext';
import { cardCacheManager } from '../services/CardCacheManager';
import { perfMonitor } from '../utils/performanceMonitor';

interface CollectionContextValue {
  submitCollectionUpdate: (update: CardCollectionUpdate, cardName?: string) => Promise<void>;
  isAnyCardEntering: boolean;
  setIsAnyCardEntering: (isEntering: boolean) => void;
}

const CollectionContext = createContext<CollectionContextValue | undefined>(undefined);

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
  'artist-proof': 'proof',
  'altered': 'altered'
};

export const CollectionProvider: React.FC<CollectionProviderProps> = ({ children }) => {
  const apolloClient = useApolloClient();
  const toastStackRef = useRef<NotificationToastStackRef>(null);
  const [isAnyCardEntering, setIsAnyCardEntering] = useState(false);
  const { userProfile } = useUser();
  const [addCardToCollection] = useMutation(ADD_CARD_TO_COLLECTION);

  // REMOVED: Flash animation - now handled directly in MtgCard via DOM for performance

  const submitCollectionUpdate = useCallback(async (update: CardCollectionUpdate, cardName?: string) => {
    perfMonitor.start('collection-update-total');

    try {
      // Fast-path validation - moved before any processing
      if (!userProfile?.id) {
        throw new Error('User not authenticated');
      }

      // OPTIMIZATION: Use static maps from module scope
      const variables = perfMonitor.measure('collection-prepare-variables', () => ({
        args: {
          cardId: update.cardId,
          setId: update.setId || '',
          userId: userProfile.id,
          userCardDetails: {
            finish: FINISH_MAP[update.finish] || 'nonfoil',
            special: SPECIAL_MAP[update.special] || 'none',
            count: update.count
          }
        }
      }));

      // Execute mutation
      perfMonitor.start('collection-mutation');
      const result = await addCardToCollection({ variables });
      perfMonitor.end('collection-mutation');

      perfMonitor.end('collection-update-total');

      // Check if mutation was successful
      if ((result.data as any)?.addCardToCollection?.__typename === 'SuccessCardsResponse') {
        const updatedCard = (result.data as any).addCardToCollection.data?.[0];

        if (updatedCard) {
          // OPTIMIZATION: Use requestIdleCallback for non-critical cache updates
          // This ensures these operations happen when the browser is idle
          const idleCallback = (window as any).requestIdleCallback || queueMicrotask;
          idleCallback(() => {
            // Don't clear cache - Apollo cache update should be sufficient
            // Clearing cache was causing Related Cards to reload unnecessarily

            // Update Apollo cache with new collection data
            apolloClient.cache.modify({
              id: apolloClient.cache.identify({ __typename: 'Card', id: update.cardId }),
              fields: {
                userCollection: () => updatedCard.userCollection
              }
            });

            // Trigger event to notify pages to update the card in place
            window.dispatchEvent(new CustomEvent('collection-updated', {
              detail: {
                cardId: update.cardId,
                setId: update.setId,
                userCollection: updatedCard.userCollection
              }
            }));
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
        const errorMessage = (result.data as any)?.addCardToCollection?.__typename === 'FailureResponse'
          ? (result.data as any).addCardToCollection.status?.message || 'Failed to update collection'
          : 'Failed to update collection';
        throw new Error(errorMessage);
      }
    } catch (error) {
      perfMonitor.end('collection-update-total');
      console.error('Collection update failed:', error);

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
  }, [addCardToCollection, userProfile, apolloClient.cache]);

  const value: CollectionContextValue = {
    submitCollectionUpdate,
    isAnyCardEntering,
    setIsAnyCardEntering
  };

  return (
    <CollectionContext.Provider value={value}>
      {children}
      <NotificationToastStack ref={toastStackRef} />
      <QuickEntryKeysFab />
    </CollectionContext.Provider>
  );
};