import type { Card, UserCardData } from '../types/card';

export const formatCollectionCount = (count: number): string | number => {
  return count > 0 ? count : '⭕';
};

export const getCollectionCount = (card: Card): number => {
  const userCollection = card.userCollection;

  if (!userCollection) {
    return 0;
  }

  if (Array.isArray(userCollection)) {
    return userCollection.reduce((sum: number, item: UserCardData) => sum + item.count, 0);
  }

  return userCollection.count;
};
