import { GET_USER_CARDS_BY_SET, GET_USER_CARDS_BATCH } from '../graphql/queries/userCards';
import type { UserCardData } from '../types/card';

interface PendingRequest {
  cardId: string;
  userId: string;
  resolve: (data: UserCardData | null) => void;
  reject: (error: Error) => void;
}


export class CollectionManager {
  private static instance: CollectionManager;
  private apolloClient: any;

  // Cache for individual card collection data
  private cache = new Map<string, UserCardData>();

  // Cache for set collection data
  private setCache = new Map<string, Map<string, UserCardData>>();

  // Pending individual card requests for batching
  private pendingRequests: PendingRequest[] = [];
  private batchTimer: NodeJS.Timeout | null = null;

  // Configuration
  private readonly BATCH_SIZE = 25; // Reduced batch size to avoid rate limits
  private readonly BATCH_DELAY_MS = 500; // Increased delay to reduce request rate
  private readonly MAX_CACHE_SIZE = 2000;

  constructor(apolloClient: any) {
    this.apolloClient = apolloClient;
  }

  public static getInstance(apolloClient?: any): CollectionManager {
    if (!CollectionManager.instance) {
      if (!apolloClient) {
        throw new Error('Apollo client must be provided when creating CollectionManager instance');
      }
      CollectionManager.instance = new CollectionManager(apolloClient);
    }
    return CollectionManager.instance;
  }

  /**
   * Request collection data for a single card
   * This will be batched with other requests automatically
   */
  public requestCollectionData(cardId: string, userId: string): Promise<UserCardData | null> {
    const cacheKey = this.getCacheKey(cardId, userId);

    // Check individual cache only
    const cached = this.cache.get(cacheKey);
    if (cached) {
      return Promise.resolve(cached);
    }

    // Create promise for batched request
    return new Promise<UserCardData | null>((resolve, reject) => {
      this.pendingRequests.push({
        cardId,
        userId,
        resolve,
        reject
      });

      // Start batch timer if not already running
      if (!this.batchTimer) {
        this.batchTimer = setTimeout(() => {
          this.processBatch();
        }, this.BATCH_DELAY_MS);
      }

      // Process immediately if batch is full
      if (this.pendingRequests.length >= this.BATCH_SIZE) {
        this.processBatch();
      }
    });
  }

  /**
   * Request collection data for an entire set
   * This bypasses batching and makes a direct set request
   */
  public async requestSetCollectionData(setId: string, userId: string): Promise<Map<string, UserCardData>> {
    const setCacheKey = this.getSetCacheKey(setId, userId);

    // Check set cache first
    const cached = this.setCache.get(setCacheKey);
    if (cached) {
      return cached;
    }

    try {
      const response = await this.apolloClient.query({
        query: GET_USER_CARDS_BY_SET,
        variables: {
          setArgs: {
            setId,
            userId
          }
        },
        fetchPolicy: 'network-only' // Always fetch fresh data for sets
      });

      if (response.data?.userCardsBySet?.__typename === 'SuccessUserCardsCollectionResponse') {
        const collectionData = response.data.userCardsBySet.data;
        const setMap = new Map<string, UserCardData>();


        // Cache each card individually and in the set
        collectionData.forEach((card: any) => {
          const userData: UserCardData = {
            userId: card.userId,
            cardId: card.cardId,
            setId: card.setId,
            collectedList: card.collectedList.map((item: any) => ({
              finish: item.finish as 'nonfoil' | 'foil' | 'etched',
              special: (item.special as 'none' | 'artist_proof' | 'signed' | 'altered') || 'none',
              count: item.count
            }))
          };

          // Cache in set map
          setMap.set(card.cardId, userData);

          // Cache in individual cache
          const cacheKey = this.getCacheKey(card.cardId, userId);
          this.cache.set(cacheKey, userData);
        });

        // Cache the entire set
        this.setCache.set(setCacheKey, setMap);
        this.manageCacheSize();

        return setMap;
      } else {
        console.warn('Failed to fetch set collection data:', response.data?.userCardsBySet);
        return new Map();
      }
    } catch (error) {
      console.error('Error fetching set collection data:', error);
      return new Map();
    }
  }

  /**
   * Invalidate cache for a specific card
   */
  public invalidateCard(cardId: string, userId: string): void {
    const cacheKey = this.getCacheKey(cardId, userId);
    this.cache.delete(cacheKey);

    // Also remove from any set caches
    for (const [setKey, setMap] of this.setCache.entries()) {
      if (setKey.includes(userId)) {
        setMap.delete(cardId);
      }
    }
  }

  /**
   * Invalidate all cache for a user
   */
  public invalidateUser(userId: string): void {
    // Remove from individual cache
    for (const key of this.cache.keys()) {
      if (key.includes(userId)) {
        this.cache.delete(key);
      }
    }

    // Remove from set cache
    for (const key of this.setCache.keys()) {
      if (key.includes(userId)) {
        this.setCache.delete(key);
      }
    }
  }

  /**
   * Smart bulk request that uses set caching when possible
   * Groups cards by set and fetches missing data efficiently
   */
  public async requestCollectionDataForCards(cards: Array<{cardId: string, setId?: string}>, userId: string): Promise<Map<string, UserCardData>> {
    const resultMap = new Map<string, UserCardData>();
    const missingCards: string[] = [];


    // First, try to get data from individual cache only
    for (const card of cards) {
      const cacheKey = this.getCacheKey(card.cardId, userId);
      const cached = this.cache.get(cacheKey);

      if (cached) {
        resultMap.set(card.cardId, cached);
      } else {
        missingCards.push(card.cardId);
      }
    }

    // If we have all data cached, return immediately
    if (missingCards.length === 0) {
      return resultMap;
    }


    // For missing cards, make individual requests (they will be batched automatically)
    const missingPromises = missingCards.map(cardId =>
      this.requestCollectionData(cardId, userId)
    );

    const missingResults = await Promise.all(missingPromises);

    // Add missing results to the map
    missingCards.forEach((cardId, index) => {
      const data = missingResults[index];
      if (data) {
        resultMap.set(cardId, data);
      }
    });

    return resultMap;
  }

  /**
   * Get cached collection data synchronously (does not trigger fetch)
   */
  public getCachedCollectionData(cardId: string, userId: string): UserCardData | null {
    const cacheKey = this.getCacheKey(cardId, userId);
    return this.cache.get(cacheKey) || null;
  }

  /**
   * Get current cache statistics
   */
  public getCacheStats() {
    return {
      individualCards: this.cache.size,
      sets: this.setCache.size,
      totalSetCards: Array.from(this.setCache.values()).reduce((sum, setMap) => sum + setMap.size, 0),
      pendingRequests: this.pendingRequests.length
    };
  }

  private async processBatch(): Promise<void> {
    if (this.batchTimer) {
      clearTimeout(this.batchTimer);
      this.batchTimer = null;
    }

    if (this.pendingRequests.length === 0) {
      return;
    }

    const currentBatch = this.pendingRequests.splice(0);

    // Group by userId for efficiency
    const requestsByUser = new Map<string, PendingRequest[]>();
    currentBatch.forEach(request => {
      const userRequests = requestsByUser.get(request.userId) || [];
      userRequests.push(request);
      requestsByUser.set(request.userId, userRequests);
    });

    // Process each user's requests using bulk endpoint
    for (const [userId, userRequests] of requestsByUser.entries()) {
      try {
        await this.processBulkRequest(userId, userRequests);
      } catch (error) {
        console.error(`Failed to process bulk request for user ${userId}:`, error);
        // Reject all requests in this batch
        userRequests.forEach(request => {
          request.reject(error as Error);
        });
      }
    }
  }

  private async processBulkRequest(userId: string, requests: PendingRequest[]): Promise<void> {
    const cardIds = requests.map(req => req.cardId);

    try {
      const response = await this.apolloClient.query({
        query: GET_USER_CARDS_BATCH,
        variables: {
          cardsArgs: {
            userId,
            cardIds
          }
        },
        fetchPolicy: 'network-only' // Always fetch fresh data for bulk requests
      });

      // Log only failures and successes, not every response
      if (response.data?.userCardsByIds?.__typename !== 'SuccessUserCardsCollectionResponse') {
      }

      if (response.data?.userCardsByIds?.__typename === 'SuccessUserCardsCollectionResponse') {
        const collectionDataArray = response.data.userCardsByIds.data;


        // Create a map for quick lookup
        const collectionMap = new Map<string, UserCardData>();
        collectionDataArray.forEach((data: any) => {
          const userData: UserCardData = {
            userId: data.userId,
            cardId: data.cardId,
            setId: data.setId,
            collectedList: data.collectedList.map((item: any) => ({
              finish: item.finish as 'nonfoil' | 'foil' | 'etched',
              special: (item.special as 'none' | 'artist_proof' | 'signed' | 'altered') || 'none',
              count: item.count
            }))
          };

          collectionMap.set(data.cardId, userData);

          // Cache each result in individual cache
          const cacheKey = this.getCacheKey(data.cardId, userId);
          this.cache.set(cacheKey, userData);
        });


        // Resolve all requests
        requests.forEach(request => {
          const collectionData = collectionMap.get(request.cardId) || null;
          request.resolve(collectionData);
        });

        // Manage cache size after bulk update
        this.manageCacheSize();
      } else if (response.data?.userCardsByIds?.__typename === 'FailureResponse') {
        // GraphQL returned a failure response
        console.warn('GraphQL failure response:', response.data.userCardsByIds.status);
        requests.forEach(request => {
          request.resolve(null);
        });
      } else {
        // Unexpected response structure
        console.warn('Unexpected response structure:', response.data?.userCardsByIds);
        requests.forEach(request => {
          request.resolve(null);
        });
      }
    } catch (error: any) {
      // Check if it's a 429 rate limiting error
      if (error.graphQLErrors?.some((err: any) =>
          err.extensions?.message?.includes('TooManyRequests') ||
          err.extensions?.message?.includes('429'))) {
        console.warn('Rate limited by Cosmos DB, retrying after delay...');

        // Retry after a delay (start with 1 second, could be made configurable)
        setTimeout(async () => {
          try {
            await this.processBulkRequest(userId, requests);
          } catch (retryError) {
            console.error('Retry failed after rate limit:', retryError);
            requests.forEach(request => {
              request.reject(retryError as Error);
            });
          }
        }, 1000);
      } else {
        // Other network or GraphQL error - reject all requests
        console.error('Error in bulk collection request:', error);
        requests.forEach(request => {
          request.reject(error as Error);
        });
      }
    }
  }

  private findInSetCache(cardId: string, userId: string): UserCardData | null {

    for (const [setKey, setMap] of this.setCache.entries()) {
      if (setKey.includes(userId)) {
        const data = setMap.get(cardId);
        if (data) {
          return data;
        }
      }
    }
    return null;
  }

  private getCacheKey(cardId: string, userId: string): string {
    return `${userId}:${cardId}`;
  }

  private getSetCacheKey(setId: string, userId: string): string {
    return `${userId}:set:${setId}`;
  }

  private manageCacheSize(): void {
    // Simple LRU eviction - remove oldest entries if cache gets too large
    if (this.cache.size > this.MAX_CACHE_SIZE) {
      const entries = Array.from(this.cache.entries());
      const toRemove = entries.slice(0, entries.length - this.MAX_CACHE_SIZE);
      toRemove.forEach(([key]) => this.cache.delete(key));
    }
  }
}

// Export singleton accessor
export const getCollectionManager = (apolloClient?: any) => {
  return CollectionManager.getInstance(apolloClient);
};