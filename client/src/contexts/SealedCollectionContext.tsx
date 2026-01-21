import React, { createContext, useContext, useRef, useCallback, useState } from 'react';
import { logger } from '../utils/logger';
import { NotificationToastStack } from '../components/organisms/shared/NotificationToastStack';
import type { NotificationToastStackRef } from '../components/organisms/shared/NotificationToastStack';
import { useMutation, useApolloClient } from '@apollo/client/react';
import { gql } from '@apollo/client';
import { ADD_SEALED_PRODUCT_TO_COLLECTION } from '../graphql/mutations/sealedProducts';
import { useUser } from './UserContext';
import { useCollectorParam } from '../hooks/useCollectorParam';
import { perfMonitor } from '../utils/performanceMonitor';

export interface SealedProductCollectionUpdate {
  productUuid: string;
  setId: string;
  count: number; // Delta value (+1, -1, +5, etc.)
}

interface SealedCollectionContextValue {
  submitSealedProductUpdate: (update: SealedProductCollectionUpdate, productName?: string) => Promise<void>;
  isAnyProductEntering: boolean;
  setIsAnyProductEntering: (isEntering: boolean) => void;
  getLastDelta: (productUuid: string) => number | undefined;
  lastDeltaVersion: number;
}

interface CachedSealedProduct {
  userQuantity?: number;
}

interface AddSealedProductMutationResponse {
  addUserSealedProduct?: {
    __typename: string;
    data?: Array<{
      uuid: string;
      userQuantity: number;
    }>;
    status?: {
      message: string;
    };
  };
}

const SealedCollectionContext = createContext<SealedCollectionContextValue | undefined>(undefined);

// eslint-disable-next-line react-refresh/only-export-components -- Standard pattern: export hook with Provider
export const useSealedCollection = () => {
  const context = useContext(SealedCollectionContext);
  if (!context) {
    throw new Error('useSealedCollection must be used within SealedCollectionProvider');
  }
  return context;
};

interface SealedCollectionProviderProps {
  children: React.ReactNode;
}

export const SealedCollectionProvider: React.FC<SealedCollectionProviderProps> = ({ children }) => {
  const apolloClient = useApolloClient();
  const toastStackRef = useRef<NotificationToastStackRef>(null);
  const [isAnyProductEntering, setIsAnyProductEntering] = useState(false);
  const { userProfile } = useUser();
  const { collectorId } = useCollectorParam();
  const [addSealedProduct] = useMutation(ADD_SEALED_PRODUCT_TO_COLLECTION);

  // Track last modification delta per product (for LastDeltaBadge)
  const lastDeltaMapRef = useRef<Map<string, number>>(new Map());
  const [lastDeltaVersion, setLastDeltaVersion] = useState(0);

  const getLastDelta = useCallback((productUuid: string): number | undefined => {
    return lastDeltaMapRef.current.get(productUuid);
  }, []);

  const submitSealedProductUpdate = useCallback(async (update: SealedProductCollectionUpdate, productName?: string) => {
    perfMonitor.start('sealed-collection-update-total');

    // Save previous delta for rollback on failure
    const previousDelta = lastDeltaMapRef.current.get(update.productUuid);
    let deltaWasSet = false;
    // Save original quantity for rollback on failure
    let originalQuantity: number | null = null;

    try {
      // Fast-path validation
      if (!userProfile?.id) {
        throw new Error('User not authenticated');
      }
      // Use ctor from URL if present, otherwise use logged-in user's ID
      const targetUserId = collectorId || userProfile.id;

      // Store last delta for this product (for LastDeltaBadge display)
      lastDeltaMapRef.current.set(update.productUuid, update.count);
      deltaWasSet = true;
      setLastDeltaVersion(v => v + 1);

      const variables = perfMonitor.measure('sealed-collection-prepare-variables', () => ({
        args: {
          productUuid: update.productUuid,
          setId: update.setId,
          userId: targetUserId,
          userSealedProductDetails: {
            count: update.count
          }
        }
      }));

      // INSTANT UI UPDATE: Get current quantity from cache and update it optimistically
      const cacheId = apolloClient.cache.identify({ __typename: 'SealedProduct', uuid: update.productUuid });
      const cachedProduct = cacheId ? apolloClient.cache.readFragment({
        id: cacheId,
        fragment: gql`
          fragment SealedProductQuantity on SealedProduct {
            userQuantity
          }
        `
      }) : null;

      const currentQuantity = (cachedProduct as CachedSealedProduct)?.userQuantity || 0;
      // Save for rollback
      originalQuantity = currentQuantity;

      // Add the delta to the existing quantity (update.count is a delta, not absolute)
      const optimisticQuantity = Math.max(0, currentQuantity + update.count);

      // Optimistically update Apollo cache
      if (cacheId) {
        apolloClient.cache.modify({
          id: cacheId,
          fields: {
            userQuantity() {
              return optimisticQuantity;
            }
          }
        });
      }

      // Dispatch custom event for cross-component synchronization
      window.dispatchEvent(new CustomEvent('collection-updated', {
        detail: {
          productUuid: update.productUuid,
          setId: update.setId,
          userQuantity: optimisticQuantity
        }
      }));

      // Execute mutation in background (UI already updated above)
      perfMonitor.start('sealed-collection-mutation');
      const mutationStart = performance.now();
      const result = await addSealedProduct({ variables });
      const mutationEnd = performance.now();
      logger.debug(`[TIMING] Sealed product mutation took ${mutationEnd - mutationStart}ms`);
      perfMonitor.end('sealed-collection-mutation');

      perfMonitor.end('sealed-collection-update-total');

      // Check if mutation was successful
      if ((result.data as AddSealedProductMutationResponse)?.addUserSealedProduct?.__typename === 'AddUserSealedProductSuccessResponse') {
        const updatedProduct = (result.data as AddSealedProductMutationResponse).addUserSealedProduct?.data?.[0];

        if (updatedProduct) {
          // Dispatch again with real data from server (in case it differs from optimistic)
          queueMicrotask(() => {
            window.dispatchEvent(new CustomEvent('collection-updated', {
              detail: {
                productUuid: update.productUuid,
                setId: update.setId,
                userQuantity: updatedProduct.userQuantity
              }
            }));
          });
        }

        // OPTIMIZATION: Defer toast to prevent blocking
        queueMicrotask(() => {
          toastStackRef.current?.addToast({
            type: 'success',
            count: update.count,
            finish: 'non-foil', // Sealed products don't have finishes, use default
            special: 'none',
            cardName: productName
          });
        });
      } else {
        const errorMessage = (result.data as AddSealedProductMutationResponse)?.addUserSealedProduct?.__typename === 'FailureResponse'
          ? (result.data as AddSealedProductMutationResponse).addUserSealedProduct?.status?.message || 'Failed to update collection'
          : 'Failed to update collection';
        throw new Error(errorMessage);
      }
    } catch (error) {
      perfMonitor.end('sealed-collection-update-total');
      logger.error('Sealed collection update failed:', error);

      // Revert delta badge to previous state on failure (only if we set it)
      if (deltaWasSet) {
        if (previousDelta === undefined) {
          lastDeltaMapRef.current.delete(update.productUuid);
        } else {
          lastDeltaMapRef.current.set(update.productUuid, previousDelta);
        }
        setLastDeltaVersion(v => v + 1);
      }

      // Revert optimistic UI update by dispatching original quantity
      if (originalQuantity !== null) {
        // Revert Apollo cache
        const cacheId = apolloClient.cache.identify({ __typename: 'SealedProduct', uuid: update.productUuid });
        if (cacheId) {
          apolloClient.cache.modify({
            id: cacheId,
            fields: {
              userQuantity() {
                return originalQuantity;
              }
            }
          });
        }

        // Dispatch event with original quantity
        window.dispatchEvent(new CustomEvent('collection-updated', {
          detail: {
            productUuid: update.productUuid,
            setId: update.setId,
            userQuantity: originalQuantity
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
          finish: 'non-foil',
          special: 'none',
          cardName: productName,
          errorMessage
        });
      });
      throw error;
    }
  }, [addSealedProduct, userProfile, collectorId, apolloClient.cache]);

  const value: SealedCollectionContextValue = {
    submitSealedProductUpdate,
    isAnyProductEntering,
    setIsAnyProductEntering,
    getLastDelta,
    lastDeltaVersion
  };

  return (
    <SealedCollectionContext.Provider value={value}>
      {children}
      <NotificationToastStack ref={toastStackRef} />
    </SealedCollectionContext.Provider>
  );
};
