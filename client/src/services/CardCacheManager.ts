import type { Card } from '../types/card';

// Cache key prefixes for different data types
const CacheType = {
  CARD: 'card',
  SET_CARDS: 'set'
} as const;

type CacheType = typeof CacheType[keyof typeof CacheType];

// Cache entry structure with metadata
interface CacheEntry<T = any> {
  data: T;
  timestamp: number;
  expiresAt: number;
  collectorId?: string | null;
}

// Configuration for cache behavior
interface CacheConfig {
  defaultTtlMs: number;
  maxEntries: number;
  enableLogging: boolean;
}


// Collector-specific card data
interface CollectorCardData {
  cardId: string;
  finish: string;
  special: string;
  count: number;
}

// Update result for cache operations
interface CacheUpdateResult {
  updated: boolean;
  key: string;
  source: 'cache' | 'api';
}

// Fetcher function types for cache-through operations
type CardFetcher = () => Promise<Card>;
type CardsFetcher = (missingIds: string[]) => Promise<Card[]>;
type SetCardsFetcher = () => Promise<Card[]>;

/**
 * Comprehensive Card Cache Manager that handles all card data caching
 * with support for both anonymous and collector-specific contexts.
 *
 * Cache Key Strategy:
 * - Without collector: cardId, setId
 * - With collector: {ctor}:{cardId}, {ctor}:{setId}
 *
 * Features:
 * - Fetch-through-cache API for automatic cache population
 * - Intelligent cache invalidation
 * - TTL-based expiration
 * - LRU eviction when size limits reached
 * - Collector context awareness
 * - Type-safe operations
 * - Encapsulated cache population (no public setters)
 *
 * Public API:
 * - fetchCard() - Get single card, fetch if needed
 * - fetchCards() - Bulk card fetch with cache-through
 * - fetchSetCards() - Get set cards with cache-through
 * - updateCollectorCard() - Handle collector updates
 * - clear() - Clear all cache entries
 */
export class CardCacheManager {
  private cache = new Map<string, CacheEntry>();
  private accessOrder = new Map<string, number>();
  private accessCounter = 0;
  private config: CacheConfig;

  constructor(config: Partial<CacheConfig> = {}) {
    this.config = {
      defaultTtlMs: 5 * 60 * 1000, // 5 minutes default TTL
      maxEntries: 1000,
      enableLogging: false,
      ...config
    };

    // Set up periodic cleanup
    this.startCleanupTimer();
  }

  /**
   * Generate cache key based on context
   */
  private generateKey(
    type: CacheType,
    id: string,
    collectorId?: string | null
  ): string {
    return collectorId ? `${type}:${collectorId}:${id}` : `${type}:${id}`;
  }

  /**
   * Update access order for LRU eviction
   */
  private updateAccessOrder(key: string): void {
    this.accessOrder.set(key, ++this.accessCounter);
  }

  /**
   * Check if cache entry is expired
   */
  private isExpired(entry: CacheEntry): boolean {
    return Date.now() > entry.expiresAt;
  }

  /**
   * Evict least recently used entry
   */
  private evictLRU(): void {
    if (this.accessOrder.size === 0) return;

    let oldestKey = '';
    let oldestAccess = Infinity;

    for (const [key, accessTime] of this.accessOrder.entries()) {
      if (accessTime < oldestAccess) {
        oldestAccess = accessTime;
        oldestKey = key;
      }
    }

    if (oldestKey) {
      this.cache.delete(oldestKey);
      this.accessOrder.delete(oldestKey);

      if (this.config.enableLogging) {
        console.log(`[CardCache] Evicted LRU entry: ${oldestKey}`);
      }
    }
  }

  /**
   * Ensure cache doesn't exceed max size
   */
  private enforceMaxSize(): void {
    while (this.cache.size >= this.config.maxEntries) {
      this.evictLRU();
    }
  }

  /**
   * Clean up expired entries
   */
  private cleanupExpired(): void {
    const now = Date.now();
    const expiredKeys: string[] = [];

    for (const [key, entry] of this.cache.entries()) {
      if (now > entry.expiresAt) {
        expiredKeys.push(key);
      }
    }

    for (const key of expiredKeys) {
      this.cache.delete(key);
      this.accessOrder.delete(key);
    }

    if (this.config.enableLogging && expiredKeys.length > 0) {
      console.log(`[CardCache] Cleaned up ${expiredKeys.length} expired entries`);
    }
  }

  /**
   * Start periodic cleanup timer
   */
  private startCleanupTimer(): void {
    setInterval(() => {
      this.cleanupExpired();
    }, 60000); // Cleanup every minute
  }

  /**
   * Get a single card by ID (internal use only)
   */
  private getCard(cardId: string, collectorId?: string | null): Card | null {
    const key = this.generateKey(CacheType.CARD, cardId, collectorId);
    const entry = this.cache.get(key);

    if (!entry) {
      return null;
    }

    if (this.isExpired(entry)) {
      this.cache.delete(key);
      this.accessOrder.delete(key);
      return null;
    }

    this.updateAccessOrder(key);

    if (this.config.enableLogging) {
      console.log(`[CardCache] Hit: ${key}`);
    }

    return entry.data as Card;
  }

  /**
   * Set a single card in cache (internal use only)
   */
  private setCard(
    cardId: string,
    card: Card,
    collectorId?: string | null,
    ttlMs?: number
  ): void {
    const key = this.generateKey(CacheType.CARD, cardId, collectorId);
    const ttl = ttlMs ?? this.config.defaultTtlMs;

    this.enforceMaxSize();

    const entry: CacheEntry<Card> = {
      data: card,
      timestamp: Date.now(),
      expiresAt: Date.now() + ttl,
      collectorId
    };

    this.cache.set(key, entry);
    this.updateAccessOrder(key);

    if (this.config.enableLogging) {
      console.log(`[CardCache] Set: ${key}`);
    }
  }

  /**
   * Get cards for a set (internal use only)
   */
  private getSetCards(setId: string, collectorId?: string | null): Card[] | null {
    const key = this.generateKey(CacheType.SET_CARDS, setId, collectorId);
    const entry = this.cache.get(key);

    if (!entry) {
      return null;
    }

    if (this.isExpired(entry)) {
      this.cache.delete(key);
      this.accessOrder.delete(key);
      return null;
    }

    this.updateAccessOrder(key);

    if (this.config.enableLogging) {
      console.log(`[CardCache] Set hit: ${key}`);
    }

    return entry.data as Card[];
  }

  /**
   * Set cards for a set (internal use only)
   */
  private setSetCards(
    setId: string,
    cards: Card[],
    collectorId?: string | null,
    ttlMs?: number
  ): void {
    const key = this.generateKey(CacheType.SET_CARDS, setId, collectorId);
    const ttl = ttlMs ?? this.config.defaultTtlMs;

    this.enforceMaxSize();

    const entry: CacheEntry<Card[]> = {
      data: cards,
      timestamp: Date.now(),
      expiresAt: Date.now() + ttl,
      collectorId
    };

    this.cache.set(key, entry);
    this.updateAccessOrder(key);

    // Also cache individual cards from the set
    cards.forEach(card => {
      if (card.id) {
        this.setCard(card.id, card, collectorId, ttl);
      }
    });

    if (this.config.enableLogging) {
      console.log(`[CardCache] Set cards: ${key} (${cards.length} cards)`);
    }
  }

  /**
   * Get multiple cards by IDs (internal use only)
   */
  private getCards(cardIds: string[], collectorId?: string | null): Partial<Record<string, Card>> {
    const result: Partial<Record<string, Card>> = {};

    for (const cardId of cardIds) {
      const card = this.getCard(cardId, collectorId);
      if (card) {
        result[cardId] = card;
      }
    }

    return result;
  }

  /**
   * Set multiple cards at once (internal use only)
   */
  private setCards(
    cards: Card[],
    collectorId?: string | null,
    ttlMs?: number
  ): void {
    cards.forEach(card => {
      if (card.id) {
        this.setCard(card.id, card, collectorId, ttlMs);
      }
    });
  }

  /**
   * Fetch a single card through cache (check cache first, fetch if needed)
   */
  async fetchCard(
    cardId: string,
    fetcher: CardFetcher,
    collectorId?: string | null,
    ttlMs?: number
  ): Promise<Card> {
    // Check cache first
    const cached = this.getCard(cardId, collectorId);
    if (cached) {
      return cached;
    }

    // Cache miss - fetch from API
    const card = await fetcher();
    this.setCard(cardId, card, collectorId, ttlMs);
    return card;
  }

  /**
   * Fetch multiple cards through cache (bulk operation)
   */
  async fetchCards(
    cardIds: string[],
    fetcher: CardsFetcher,
    collectorId?: string | null,
    ttlMs?: number
  ): Promise<Card[]> {
    // Check which cards we already have
    const cachedCards = this.getCards(cardIds, collectorId);
    const missingIds = cardIds.filter(id => !cachedCards[id]);

    // If we have everything, return cached data
    if (missingIds.length === 0) {
      return cardIds.map(id => cachedCards[id]!).filter(Boolean);
    }

    // Fetch missing cards
    const fetchedCards = await fetcher(missingIds);
    this.setCards(fetchedCards, collectorId, ttlMs);

    // Return combined results
    const allCards = this.getCards(cardIds, collectorId);
    return cardIds.map(id => allCards[id]!).filter(Boolean);
  }

  /**
   * Fetch cards for a set through cache
   */
  async fetchSetCards(
    setId: string,
    fetcher: SetCardsFetcher,
    collectorId?: string | null,
    ttlMs?: number
  ): Promise<Card[]> {
    // Check cache first
    const cached = this.getSetCards(setId, collectorId);
    if (cached) {
      return cached;
    }

    // Cache miss - fetch from API
    const cards = await fetcher();
    this.setSetCards(setId, cards, collectorId, ttlMs);
    return cards;
  }

  /**
   * Update card with new collector data (e.g., after collection update)
   * This updates the userCollection field on cached card entries
   */
  updateCollectorCard(
    collectorData: CollectorCardData,
    collectorId: string
  ): CacheUpdateResult {
    const key = this.generateKey(CacheType.CARD, collectorData.cardId, collectorId);
    const existingEntry = this.cache.get(key);

    if (existingEntry && !this.isExpired(existingEntry)) {
      // Update existing entry's userCollection field
      const card = existingEntry.data as Card;
      if (card) {
        card.userCollection = {
          finish: collectorData.finish,
          special: collectorData.special,
          count: collectorData.count
        };
        existingEntry.timestamp = Date.now();
        this.updateAccessOrder(key);

        if (this.config.enableLogging) {
          console.log(`[CardCache] Updated collector data: ${key}`);
        }

        return { updated: true, key, source: 'cache' };
      }
    }

    // No existing entry to update - would need to fetch full card first
    return { updated: false, key, source: 'api' };
  }

  /**
   * Clear all cache entries
   */
  clear(): void {
    const previousSize = this.cache.size;
    this.cache.clear();
    this.accessOrder.clear();

    if (this.config.enableLogging) {
      console.log(`[CardCache] Cleared all ${previousSize} entries`);
    }
  }

}

// Export a singleton instance for global use
export const cardCacheManager = new CardCacheManager({
  enableLogging: process.env.NODE_ENV === 'development'
});

// Export types for consumers
export type {
  CacheConfig,
  CacheUpdateResult,
  CollectorCardData,
  CardFetcher,
  CardsFetcher,
  SetCardsFetcher
};