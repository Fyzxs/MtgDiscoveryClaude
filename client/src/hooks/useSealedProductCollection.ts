import { useCallback } from 'react';
import { useApolloClient } from '@apollo/client';

// TODO: Replace with actual generated types after running codegen
// import { useAddUserSealedProductMutation } from '../generated/graphql';

// Placeholder type until codegen runs
type AddUserSealedProductMutationVariables = {
  variables: {
    args: {
      productUuid: string;
      setId: string;
      countDelta: number;
    };
  };
};

type AddUserSealedProductMutationResult = {
  data?: {
    addUserSealedProduct?: {
      __typename: string;
      data?: {
        count: number;
      };
    };
  };
};

type AddUserSealedProductMutationFn = (
  options: AddUserSealedProductMutationVariables
) => Promise<AddUserSealedProductMutationResult>;

// Placeholder hook until codegen runs
const useAddUserSealedProductMutation = (): [
  AddUserSealedProductMutationFn,
  { loading: boolean }
] => {
  // This will be replaced by the actual generated hook
  return [async () => ({ data: undefined }), { loading: false }];
};

/**
 * Custom event detail type for sealed collection updates
 */
export interface SealedCollectionUpdatedDetail {
  productUuid: string;
  userQuantity: number;
}

/**
 * Return type for useSealedProductCollection hook
 */
export interface UseSealedProductCollectionReturn {
  updateQuantity: (
    productUuid: string,
    setId: string,
    delta: number
  ) => Promise<boolean>;
  isUpdating: boolean;
}

/**
 * Hook to manage sealed product collection state with optimistic updates.
 *
 * Provides methods to update sealed product quantities with:
 * - Optimistic UI updates for instant feedback
 * - Apollo cache management
 * - Custom event dispatching for cross-component synchronization
 * - Error handling with automatic rollback
 *
 * @example
 * ```typescript
 * const { updateQuantity, isUpdating } = useSealedProductCollection();
 *
 * const handleQuantityChange = async (delta: number) => {
 *   const success = await updateQuantity(
 *     product.uuid,
 *     product.setId,
 *     delta
 *   );
 *
 *   if (!success) {
 *     showErrorToast('Failed to update quantity');
 *   }
 * };
 * ```
 */
export function useSealedProductCollection(): UseSealedProductCollectionReturn {
  const [addMutation, { loading }] = useAddUserSealedProductMutation();
  const apolloClient = useApolloClient();

  const updateQuantity = useCallback(
    async (
      productUuid: string,
      setId: string,
      delta: number
    ): Promise<boolean> => {
      // Track original value for rollback on error
      let originalQuantity: number | null = null;

      try {
        // Step 1: Optimistically update Apollo cache
        const cacheId = apolloClient.cache.identify({
          __typename: 'SealedProduct',
          uuid: productUuid,
        });

        if (cacheId) {
          apolloClient.cache.modify({
            id: cacheId,
            fields: {
              userQuantity(existingQuantity = 0) {
                // Store original value for potential rollback
                originalQuantity = existingQuantity;

                // Calculate new quantity (ensure non-negative)
                const newQuantity = Math.max(0, existingQuantity + delta);

                return newQuantity;
              },
            },
          });
        }

        // Step 2: Execute mutation
        const result = await addMutation({
          variables: {
            args: {
              productUuid,
              setId,
              countDelta: delta,
            },
          },
        });

        // Step 3: Check if mutation succeeded
        if (
          result?.data?.addUserSealedProduct?.__typename ===
          'AddUserSealedProductSuccessResponse'
        ) {
          const newCount = result.data.addUserSealedProduct.data?.count ?? 0;

          // Step 4: Dispatch custom event for cross-component synchronization
          window.dispatchEvent(
            new CustomEvent<SealedCollectionUpdatedDetail>(
              'sealed-collection-updated',
              {
                detail: {
                  productUuid,
                  userQuantity: newCount,
                },
              }
            )
          );

          return true;
        }

        // Mutation failed - rollback optimistic update
        if (cacheId && originalQuantity !== null) {
          apolloClient.cache.modify({
            id: cacheId,
            fields: {
              userQuantity() {
                return originalQuantity;
              },
            },
          });
        }

        return false;
      } catch (error) {
        console.error('Failed to update sealed product collection:', error);

        // Rollback optimistic update on error
        if (cacheId && originalQuantity !== null) {
          apolloClient.cache.modify({
            id: cacheId,
            fields: {
              userQuantity() {
                return originalQuantity;
              },
            },
          });
        }

        return false;
      }
    },
    [addMutation, apolloClient]
  );

  return {
    updateQuantity,
    isUpdating: loading,
  };
}
