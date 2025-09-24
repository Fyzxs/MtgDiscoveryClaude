import React, { createContext, useContext, useRef, useCallback, useState } from 'react';
import { NotificationToastStack } from '../components/organisms/NotificationToastStack';
import type { NotificationToastStackRef } from '../components/organisms/NotificationToastStack';
import { GlobalCardEntryHelp } from '../components/molecules/Cards/GlobalCardEntryHelp';
import type { CardCollectionUpdate } from '../types/collection';
import { useMutation, useApolloClient } from '@apollo/client/react';
import { ADD_CARD_TO_COLLECTION } from '../graphql/mutations/addCardToCollection';
import { useUser } from './UserContext';
import { cardCacheManager } from '../services/CardCacheManager';

interface CollectionContextValue {
  submitCollectionUpdate: (update: CardCollectionUpdate, cardName?: string) => Promise<void>;
  setCardFlashing: (cardId: string, type: 'success' | 'error') => void;
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

export const CollectionProvider: React.FC<CollectionProviderProps> = ({ children }) => {
  const apolloClient = useApolloClient();
  const toastStackRef = useRef<NotificationToastStackRef>(null);
  const [, setFlashingCards] = useState<Set<string>>(new Set());
  const [isAnyCardEntering, setIsAnyCardEntering] = useState(false);
  const { userProfile } = useUser();
  const [addCardToCollection] = useMutation(ADD_CARD_TO_COLLECTION);

  const setCardFlashing = useCallback((cardId: string) => {
    setFlashingCards(prev => new Set(prev).add(cardId));

    // Flash 3 times (150ms on, 150ms off = 900ms total)
    let flashCount = 0;
    const flashInterval = setInterval(() => {
      flashCount++;
      if (flashCount >= 6) {
        clearInterval(flashInterval);
        setFlashingCards(prev => {
          const next = new Set(prev);
          next.delete(cardId);
          return next;
        });
      }
    }, 150);
  }, []);

  const submitCollectionUpdate = useCallback(async (update: CardCollectionUpdate, cardName?: string) => {
    try {
      if (!userProfile?.id) {
        throw new Error('User not authenticated');
      }

      // Map finish values to backend format
      const finishMap: Record<string, string> = {
        'non-foil': 'nonfoil',
        'foil': 'foil',
        'etched': 'etched'
      };

      // Map special values to backend format
      const specialMap: Record<string, string> = {
        'none': 'none',
        'signed': 'signed',
        'artist-proof': 'proof',
        'altered': 'altered'
      };

      console.log('Sending collection update mutation:', {
        cardId: update.cardId,
        setId: update.setId,
        userId: userProfile.id,
        userCardDetails: {
          finish: finishMap[update.finish] || 'nonfoil',
          special: specialMap[update.special] || 'none',
          count: update.count
        }
      });

      const result = await addCardToCollection({
        variables: {
          args: {
            cardId: update.cardId,
            setId: update.setId || '', // Need to get setId from card data
            userId: userProfile.id,
            userCardDetails: {
              finish: finishMap[update.finish] || 'nonfoil',
              special: specialMap[update.special] || 'none',
              count: update.count
            }
          }
        }
      });

      console.log('Collection update mutation result:', result);

      // Check if mutation was successful
      if (result.data?.addCardToCollection?.__typename === 'SuccessCardsResponse') {
        const updatedCard = result.data.addCardToCollection.data?.[0];

        if (updatedCard) {
          // Clear the cache to ensure fresh data is fetched on next query
          // In the future, we could selectively invalidate specific cache entries
          // For now, clear all to ensure consistency
          cardCacheManager.clear();

          // Update Apollo cache with new collection data
          apolloClient.cache.modify({
            id: apolloClient.cache.identify({ __typename: 'Card', id: update.cardId }),
            fields: {
              userCollection: () => updatedCard.userCollection
            }
          });
        }

        toastStackRef.current?.addToast({
          type: 'success',
          count: update.count,
          finish: update.finish,
          special: update.special,
          cardName
        });
        setCardFlashing(update.cardId);
      } else {
        const errorMessage = result.data?.addCardToCollection?.__typename === 'FailureResponse'
          ? result.data.addCardToCollection.status?.message || 'Failed to update collection'
          : 'Failed to update collection';
        throw new Error(errorMessage);
      }
    } catch (error) {
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

      toastStackRef.current?.addToast({
        type: 'error',
        count: update.count,
        finish: update.finish,
        special: update.special,
        cardName,
        errorMessage
      });
      setCardFlashing(update.cardId);
      throw error;
    }
  }, [addCardToCollection, userProfile, setCardFlashing]);

  const value: CollectionContextValue = {
    submitCollectionUpdate,
    setCardFlashing,
    isAnyCardEntering,
    setIsAnyCardEntering
  };

  return (
    <CollectionContext.Provider value={value}>
      {children}
      <NotificationToastStack ref={toastStackRef} />
      <GlobalCardEntryHelp />
    </CollectionContext.Provider>
  );
};