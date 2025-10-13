import type { Card } from '../types/card';

// Cache key prefixes for different data types
const CacheType = {
  CARD: 'card',
  SET_CARDS: 'set',
  ARTIST_CARDS: 'artist',
  NAME_CARDS: 'name'
} as const;

type CacheType = typeof CacheType[keyof typeof CacheType];

// Cache entry structure with metadata
interface CacheEntry<T = unknown> {
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
  finish: 'nonfoil' | 'foil' | 'etched';
  special: 'none' | 'artist_proof' | 'signed' | 'altered';
  count: number;
}


// Fetcher function types for cache-through operations
type CardFetcher = () => Promise<Card>;
type CardsFetcher = (missingIds: string[]) => Promise<Card[]>;
type SetCardsFetcher = () => Promise<Card[]>;
type ArtistCardsFetcher = () => Promise<Card[]>;
type NameCardsFetcher = () => Promise<Card[]>;
type CollectorUpdater = () => Promise<Card>;

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
 * - fetchCardsByArtist() - Get artist cards with cache-through
 * - updateCollectorCardData() - Update collector data via API and cache result
 * - clear() - Clear all cache entries
 */
export class CardCacheManager {
  private cache = new Map<string, CacheEntry>();
  private accessOrder = new Map<string, number>();
  private accessCounter = 0;
  private config: CacheConfig;
  private instanceId = Math.random().toString(36).substring(2, 11);

  constructor(config: Partial<CacheConfig> = {}) {
    this.config = {
      defaultTtlMs: 5 * 60 * 1000, // 5 minutes default TTL
      maxEntries: 1000,
      enableLogging: false,
      ...config
    };

    if (this.config.enableLogging) {
      console.log(`[CardCache] New instance created: ${this.instanceId}`);
    }

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
      if (this.config.enableLogging) {
        console.log(`[CardCache-${this.instanceId}] Miss: looking for cardId=${cardId}, key=${key}, collectorId=${collectorId}`);
        const cardKeys = Array.from(this.cache.keys()).filter(k => k.includes('card:'));
        console.log(`[CardCache-${this.instanceId}] Available card keys (first 5): ${cardKeys.slice(0, 5).join(', ')}, total: ${cardKeys.length}`);
      }
      return null;
    }

    if (this.isExpired(entry)) {
      this.cache.delete(key);
      this.accessOrder.delete(key);
      if (this.config.enableLogging) {
        console.log(`[CardCache] Miss (expired): ${key}`);
      }
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
      console.log(`[CardCache] Stored card: cardId=${cardId}, key=${key}, collectorId=${collectorId}`);
    }
  }

  /**
   * Get cards for a set (internal use only)
   */
  private getSetCards(setId: string, collectorId?: string | null): Card[] | null {
    const key = this.generateKey(CacheType.SET_CARDS, setId, collectorId);
    const entry = this.cache.get(key);

    if (!entry) {
      if (this.config.enableLogging) {
        console.log(`[CardCache] Miss (set): ${key}`);
      }
      return null;
    }

    if (this.isExpired(entry)) {
      this.cache.delete(key);
      this.accessOrder.delete(key);
      if (this.config.enableLogging) {
        console.log(`[CardCache] Miss (set expired): ${key}`);
      }
      return null;
    }

    this.updateAccessOrder(key);

    if (this.config.enableLogging) {
      console.log(`[CardCache] Hit (set): ${key}`);
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
    if (this.config.enableLogging) {
      console.log(`[CardCache] Caching ${cards.length} individual cards from set ${setId}`);
    }
    cards.forEach(card => {
      if (card.id) {
        this.setCard(card.id, card, collectorId, ttl);
      }
    });
  }

  /**
   * Get cards for an artist (internal use only)
   */
  private getArtistCards(artistName: string, collectorId?: string | null): Card[] | null {
    const key = this.generateKey(CacheType.ARTIST_CARDS, artistName, collectorId);
    const entry = this.cache.get(key);

    if (!entry) {
      if (this.config.enableLogging) {
        console.log(`[CardCache] Miss (artist): ${key}`);
      }
      return null;
    }

    if (this.isExpired(entry)) {
      this.cache.delete(key);
      this.accessOrder.delete(key);
      if (this.config.enableLogging) {
        console.log(`[CardCache] Miss (artist expired): ${key}`);
      }
      return null;
    }

    this.updateAccessOrder(key);

    if (this.config.enableLogging) {
      console.log(`[CardCache] Hit (artist): ${key}`);
    }

    return entry.data as Card[];
  }

  /**
   * Set cards for an artist (internal use only)
   */
  private setArtistCards(
    artistName: string,
    cards: Card[],
    collectorId?: string | null,
    ttlMs?: number
  ): void {
    const key = this.generateKey(CacheType.ARTIST_CARDS, artistName, collectorId);
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

    // Also cache individual cards from the artist
    cards.forEach(card => {
      if (card.id) {
        this.setCard(card.id, card, collectorId, ttl);
      }
    });
  }

  /**
   * Get cards for a name (internal use only)
   */
  private getNameCards(cardName: string, collectorId?: string | null): Card[] | null {
    const key = this.generateKey(CacheType.NAME_CARDS, cardName, collectorId);
    const entry = this.cache.get(key);

    if (!entry) {
      if (this.config.enableLogging) {
        console.log(`[CardCache] Miss (name): ${key}`);
      }
      return null;
    }

    if (this.isExpired(entry)) {
      this.cache.delete(key);
      this.accessOrder.delete(key);
      if (this.config.enableLogging) {
        console.log(`[CardCache] Miss (name expired): ${key}`);
      }
      return null;
    }

    this.updateAccessOrder(key);

    if (this.config.enableLogging) {
      console.log(`[CardCache] Hit (name): ${key}`);
    }

    return entry.data as Card[];
  }

  /**
   * Set cards for a name (internal use only)
   */
  private setNameCards(
    cardName: string,
    cards: Card[],
    collectorId?: string | null,
    ttlMs?: number
  ): void {
    const key = this.generateKey(CacheType.NAME_CARDS, cardName, collectorId);
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

    // Also cache individual cards from the name search
    if (this.config.enableLogging) {
      console.log(`[CardCache] Caching ${cards.length} individual cards from name search: ${cardName}`);
    }
    cards.forEach(card => {
      if (card.id) {
        this.setCard(card.id, card, collectorId, ttl);
      } else {
        console.warn(`[CardCache] Card without ID in name search:`, card);
      }
    });
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
    if (this.config.enableLogging) {
      console.log(`[CardCache-${this.instanceId}] fetchCard called for: ${cardId}, collectorId: ${collectorId}`);
    }

    // Check cache first
    const cached = this.getCard(cardId, collectorId);
    if (cached) {
      if (this.config.enableLogging) {
        console.log(`[CardCache-${this.instanceId}] Returning cached card: ${cardId}`);
      }
      return cached;
    }

    // Cache miss - fetch from API
    if (this.config.enableLogging) {
      console.log(`[CardCache-${this.instanceId}] Cache miss, fetching from API: ${cardId}`);
    }
    const card = await fetcher();
    this.setCard(cardId, card, collectorId, ttlMs);
    if (this.config.enableLogging) {
      console.log(`[CardCache-${this.instanceId}] Stored card: ${cardId}, cache size: ${this.cache.size}`);
    }
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
    if (this.config.enableLogging) {
      console.log(`[CardCache-${this.instanceId}] fetchSetCards called for: ${setId}, collectorId: ${collectorId}, cache size: ${this.cache.size}`);
    }

    // Check cache first
    const cached = this.getSetCards(setId, collectorId);
    if (cached) {
      return cached;
    }

    // Cache miss - fetch from API
    const cards = await fetcher();

    this.setSetCards(setId, cards, collectorId, ttlMs);

    if (this.config.enableLogging) {
      console.log(`[CardCache] Stored set: ${setId} with ${cards.length} cards, cache size: ${this.cache.size}`);
    }
    return cards;
  }

  /**
   * Fetch cards by artist through cache
   */
  async fetchCardsByArtist(
    artistName: string,
    fetcher: ArtistCardsFetcher,
    collectorId?: string | null,
    ttlMs?: number
  ): Promise<Card[]> {
    // Check cache first
    const cached = this.getArtistCards(artistName, collectorId);
    if (cached) {
      return cached;
    }

    // Cache miss - fetch from API
    const cards = await fetcher();
    this.setArtistCards(artistName, cards, collectorId, ttlMs);
    return cards;
  }

  /**
   * Fetch cards by name through cache
   */
  async fetchCardsByName(
    cardName: string,
    fetcher: NameCardsFetcher,
    collectorId?: string | null,
    ttlMs?: number
  ): Promise<Card[]> {
    console.log(`[CardCache-${this.instanceId}] fetchCardsByName called for: "${cardName}", cache size: ${this.cache.size}`);

    // Check cache first
    const cached = this.getNameCards(cardName, collectorId);
    if (cached) {
      console.log(`[CardCache-${this.instanceId}] HIT! Returning cached name query for: "${cardName}"`);
      return cached;
    }
    console.log(`[CardCache-${this.instanceId}] MISS - will fetch from API for: "${cardName}"`);

    // Cache miss - fetch from API
    const cards = await fetcher();
    this.setNameCards(cardName, cards, collectorId, ttlMs);
    return cards;
  }

  /**
   * Update collector card data through the cache (fetch-through pattern)
   * This handles the API call and caches the response for the collector context only
   */
  async updateCollectorCardData(
    collectorData: CollectorCardData,
    collectorId: string,
    updater: () => Promise<Card>
  ): Promise<Card> {
    // Call the API to update the collector data
    const updatedCard = await updater();

    // Cache the updated card for the collector context only
    this.setCard(collectorData.cardId, updatedCard, collectorId);

    return updatedCard;
  }

  /**
   * Clear all cache entries
   */
  clear(): void {
    const previousSize = this.cache.size;
    this.cache.clear();
    this.accessOrder.clear();

    if (this.config.enableLogging) {
      console.log(`[CardCache-${this.instanceId}] Cleared all ${previousSize} entries`);
      console.trace('Cache clear called from:');
    }
  }

}

// Create a singleton that persists across HMR and navigations
// Store it on window to ensure it survives module reloads
declare global {
  interface Window {
    __cardCacheManager?: CardCacheManager;
  }
}

// Export a singleton instance for global use
export const cardCacheManager = (() => {
  if (!window.__cardCacheManager) {
    window.__cardCacheManager = new CardCacheManager({
      enableLogging: false // Disable logging
    });
  }
  return window.__cardCacheManager;
})();

// Export types for consumers
export type {
  CacheConfig,
  CollectorCardData,
  CardFetcher,
  CardsFetcher,
  SetCardsFetcher,
  ArtistCardsFetcher,
  NameCardsFetcher,
  CollectorUpdater
};