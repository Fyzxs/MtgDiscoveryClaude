import { useFragment } from '@apollo/client/react';
import { gql } from '@apollo/client';
import type { UserCardData } from '../types/card';

interface CardCollectionCacheData {
  userCollection?: UserCardData[];
  userWishlist?: UserCardData[];
}

const CARD_COLLECTION_CACHE_FRAGMENT = gql`
  fragment CardCollectionCache on Card {
    userCollection {
      finish
      special
      count
    }
    userWishlist {
      finish
      special
      count
    }
  }
`;

export function useCardCollectionFromCache(cardId: string) {
  const { data } = useFragment<CardCollectionCacheData>({
    fragment: CARD_COLLECTION_CACHE_FRAGMENT,
    from: cardId ? { __typename: 'Card', id: cardId } : null
  });

  // Return partial data even when fragment is incomplete — one field may be
  // in the cache (e.g. userCollection written by optimistic update) while the
  // other is not yet present. Callers fall back to props via ?? operator.
  const typed = data as CardCollectionCacheData | undefined;
  return {
    userCollection: typed?.userCollection as UserCardData[] | undefined,
    userWishlist: typed?.userWishlist as UserCardData[] | undefined
  };
}
