import { useState, useEffect } from 'react';
import { useApolloClient } from '@apollo/client/react';
import { GET_SEALED_PRODUCTS_BY_SET_CODE } from '../graphql/queries/sealedProducts';

export interface SealedProduct {
  uuid: string;
  setId: string;
  setCode: string;
  setName?: string;
  name: string;
  category?: string;
  subtype?: string;
  cardCount?: number;
  releaseDate?: string;
  tcgplayerProductId?: string;
  imageUrl?: string;
  purchaseUrlTcgplayer?: string;
  purchaseUrlCardmarket?: string;
  purchaseUrlCardKingdom?: string;
  userQuantity?: number;
}

interface SealedProductsResponse {
  sealedProductsBySetCode: {
    __typename: string;
    data?: SealedProduct[];
    status?: {
      message: string;
      statusCode?: number;
    };
  };
}

interface UseSealedProductsDataResult {
  sealedProducts: SealedProduct[];
  loading: boolean;
  error: Error | null;
}

export const useSealedProductsData = (
  setCode: string | undefined,
  isActive: boolean
): UseSealedProductsDataResult => {
  const apolloClient = useApolloClient();
  const [sealedProducts, setSealedProducts] = useState<SealedProduct[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    if (isActive === false || !setCode) {
      return;
    }

    const fetchSealedProducts = async () => {
      setLoading(true);
      setError(null);

      try {
        const response = await apolloClient.query<SealedProductsResponse>({
          query: GET_SEALED_PRODUCTS_BY_SET_CODE,
          variables: { args: { setCode } },
          fetchPolicy: 'cache-first',
        });

        const data = response.data?.sealedProductsBySetCode;
        if (data?.__typename === 'SealedProductsSuccessResponse') {
          setSealedProducts(data.data || []);
        } else if (data?.__typename === 'FailureResponse') {
          setError(new Error(data.status?.message || 'Failed to fetch sealed products'));
        }
      } catch (err) {
        setError(err as Error);
      } finally {
        setLoading(false);
      }
    };

    fetchSealedProducts();
  }, [setCode, isActive, apolloClient]);

  return { sealedProducts, loading, error };
};
