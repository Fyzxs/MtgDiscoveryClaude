import type { Card } from '../types/card';
import type { SetGrouping } from '../types/set';

// Pre-compiled grouping matcher for better performance
interface GroupingMatcher {
  id: string;
  order: number;
  displayName: string;
  matcher: (card: Card) => boolean;
}

// Cache for compiled matchers to avoid recompiling
const matcherCache = new Map<string, GroupingMatcher[]>();

// Helper function to convert snake_case to camelCase
function snakeToCamel(str: string): string {
  return str.replace(/_([a-z])/g, (_, letter) => letter.toUpperCase());
}

// Extract numeric value from collector number for range comparisons
//
// COLLECTOR NUMBER PARSING STRATEGY:
// - This function determines how collector numbers are treated in range matching
// - Current strategy: STRICT NUMERIC ONLY (^(\d+)$)
//
// KNOWN ISSUES BY SET:
// - SET:40K - Has special surge foil cards with symbols: "317★", "42★"
//   These should NOT match numeric ranges and be treated as special variants
//   Solution: Strict matching prevents "317★" from being parsed as "317"
//
// - FUTURE CONSIDERATION: Some sets might need hybrid numbers like "42a" to match
//   ranges based on their numeric prefix. If encountered, we may need:
//   * Set-specific parsing strategies 
//   * Separate functions for different parsing modes
//   * Configuration-based approach per set
//
// CURRENT BEHAVIOR:
// - "317"   -> 317 (matches ranges)
// - "317★"  -> 0   (doesn't match ranges, treated as special)
// - "42a"   -> 0   (doesn't match ranges, treated as special) 
// - "DMR-271" -> 0 (doesn't match ranges, uses OR conditions)
function getNumericValue(cn: string): number {
  // Only treat as numeric if it's purely digits (no special characters)
  const match = cn.match(/^(\d+)$/);
  return match ? parseInt(match[1], 10) : 0;
}

// Pre-compile a grouping into an optimized matcher function
function compileGroupingMatcher(grouping: SetGrouping): GroupingMatcher {
  const { filters } = grouping;
  
  // Pre-compile collector number range logic
  let collectorNumberCheck: ((card: Card) => boolean) | null = null;
  
  if (filters.collectorNumberRange) {
    const { min, max, orConditions } = filters.collectorNumberRange;
    const hasRange = min !== null && max !== null;
    const minNum = hasRange ? getNumericValue(min!) : 0;
    const maxNum = hasRange ? getNumericValue(max!) : 0;
    const normalizedOrConditions = orConditions ? 
      orConditions.map(cn => cn.toLowerCase().trim()) : [];
    
    collectorNumberCheck = (card: Card) => {
      const cardNumber = card.collectorNumber;
      if (!cardNumber) return false;
      
      // Check OR conditions first
      if (normalizedOrConditions.length > 0) {
        const normalized = cardNumber.toLowerCase().trim();
        if (normalizedOrConditions.includes(normalized)) {
          return true;
        }
        // If we have OR conditions but no match, and no range to check, return false
        if (!hasRange) {
          return false;
        }
      }
      
      // Range check (only if we have a range)
      if (hasRange) {
        const cardNum = getNumericValue(cardNumber);
        return cardNum >= minNum && cardNum <= maxNum;
      }
      
      // If we get here, there are no OR conditions and no range, so it matches nothing
      return false;
    };
  }
  
  // Pre-compile property checks
  let propertyChecks: ((card: Card) => boolean)[] = [];
  
  if (filters.properties && Object.keys(filters.properties).length > 0) {
    for (const [key, value] of Object.entries(filters.properties)) {
      if (key.endsWith('_excludes')) {
        const propName = snakeToCamel(key.replace('_excludes', ''));
        const searchValue = String(value).toLowerCase();
        
        propertyChecks.push((card: Card) => {
          const cardValue = (card as any)[propName];
          if (cardValue && typeof cardValue === 'string') {
            return !cardValue.toLowerCase().includes(searchValue);
          }
          return true;
        });
      } else if (key.endsWith('_contains')) {
        const propName = snakeToCamel(key.replace('_contains', ''));
        const searchValue = String(value).toLowerCase();
        
        propertyChecks.push((card: Card) => {
          const cardValue = (card as any)[propName];
          return cardValue && 
                 typeof cardValue === 'string' && 
                 cardValue.toLowerCase().includes(searchValue);
        });
      } else {
        // Direct property match or array contains
        propertyChecks.push((card: Card) => {
          // Get card value
          let cardValue: any;
          
          if (key === 'date') {
            cardValue = card.releasedAt;
          } else {
            cardValue = (card as any)[key];
            if (cardValue === undefined) {
              const camelKey = snakeToCamel(key);
              cardValue = (card as any)[camelKey];
            }
          }
          
          // Direct property check
          if (cardValue !== undefined) {
            if (typeof value === 'boolean') {
              return cardValue === value;
            } else if (typeof cardValue === 'string' && typeof value === 'string') {
              return cardValue.toLowerCase() === value.toLowerCase();
            } else {
              return cardValue === value;
            }
          }
          
          // Array property check
          let found = false;
          const searchValue = String(value).toLowerCase();
          const searchKey = key.toLowerCase();
          
          if (typeof value === 'boolean' && value === true) {
            // Check if key exists in arrays
            found = [card.finishes, card.frameEffects, card.promoTypes]
              .some(arr => arr && Array.isArray(arr) && 
                          arr.some(item => item.toLowerCase() === searchKey));
          } else {
            // Check if value exists in arrays
            found = [card.finishes, card.frameEffects, card.promoTypes]
              .some(arr => arr && Array.isArray(arr) && 
                          arr.some(item => item.toLowerCase() === searchValue));
          }
          
          // Special property mappings
          if (!found) {
            if (key === 'frame' && card.frameEffects) {
              found = card.frameEffects.some(f => f.toLowerCase() === searchValue);
            } else if (key === 'border' && card.borderColor) {
              found = card.borderColor.toLowerCase() === searchValue;
            }
          }
          
          return typeof value === 'boolean' && !value ? !found : found;
        });
      }
    }
  }
  
  // Create the optimized matcher function
  const matcher = (card: Card): boolean => {
    // Collector number check (most common filter, check first)
    if (collectorNumberCheck && !collectorNumberCheck(card)) {
      return false;
    }
    
    // Property checks (use early return for performance)
    for (const check of propertyChecks) {
      if (!check(card)) {
        return false;
      }
    }
    
    return true;
  };
  
  return {
    id: grouping.id || 'no-id',
    order: grouping.order,
    displayName: grouping.displayName || 'No Name',
    matcher
  };
}

// Generate cache key for groupings
function generateGroupingsKey(groupings: SetGrouping[]): string {
  return groupings
    .map(g => `${g.id || 'no-id'}-${g.order}-${JSON.stringify(g.filters)}`)
    .join('|');
}

// Get compiled matchers with caching
function getCompiledMatchers(groupings: SetGrouping[]): GroupingMatcher[] {
  const cacheKey = generateGroupingsKey(groupings);
  const cached = matcherCache.get(cacheKey);
  
  if (cached) {
    return cached;
  }
  
  const matchers = groupings.map(compileGroupingMatcher);
  
  // Separate "In Boosters" groups from specific groups
  // "In Boosters" groups should display first (low order) but process last
  const inBoosterGroups: GroupingMatcher[] = [];
  const specificGroups: GroupingMatcher[] = [];
  
  matchers.forEach(matcher => {
    const isInBoosterGroup = matcher.id.toLowerCase().includes('booster') || 
                            matcher.displayName.toLowerCase().includes('booster') ||
                            matcher.displayName.toLowerCase().includes('in boosters');
    
    if (isInBoosterGroup) {
      inBoosterGroups.push(matcher);
    } else {
      specificGroups.push(matcher);
    }
  });
  
  // Sort each group by display order
  specificGroups.sort((a, b) => a.order - b.order);
  inBoosterGroups.sort((a, b) => a.order - b.order);
  
  // Processing order: specific groups first, then "In Boosters" groups
  const processOrderMatchers = [...specificGroups, ...inBoosterGroups];
  
  matcherCache.set(cacheKey, processOrderMatchers);
  
  // Limit cache size
  if (matcherCache.size > 10) {
    const firstKey = matcherCache.keys().next().value;
    if (firstKey) {
      matcherCache.delete(firstKey);
    }
  }
  
  return processOrderMatchers;
}

/**
 * Optimized function to group cards by set groupings
 * Uses pre-compiled matchers and single-pass algorithm for better performance
 */
export function groupCardsOptimized(
  cards: Card[],
  groupings?: SetGrouping[]
): Map<string, Card[]> {
  
  if (!groupings || groupings.length === 0) {
    return new Map([['default-cards', cards]]);
  }
  
  const startTime = performance.now();
  
  // Get compiled matchers
  const matchers = getCompiledMatchers(groupings);
  
  // Initialize result map
  const grouped = new Map<string, Card[]>();
  const assignedCards = new Set<string>();
  
  // Pre-allocate arrays for each group
  matchers.forEach(matcher => {
    grouped.set(matcher.id, []);
  });
  
  // Single pass through cards with early assignment
  for (const card of cards) {
    if (assignedCards.has(card.id)) continue;
    
    // Find first matching group
    for (const matcher of matchers) {
      if (matcher.matcher(card)) {
        grouped.get(matcher.id)!.push(card);
        assignedCards.add(card.id);
        break; // Early exit once assigned
      }
    }
  }
  
  // Add unassigned cards to default group
  const unassignedCards = cards.filter(card => !assignedCards.has(card.id));
  if (unassignedCards.length > 0) {
    grouped.set('default-cards', unassignedCards);
  }
  
  const endTime = performance.now();
  if (cards.length > 1000) {
    console.log(`Optimized grouping of ${cards.length} cards completed in ${(endTime - startTime).toFixed(2)}ms`);
  }
  
  return grouped;
}

/**
 * Get display name for a group (unchanged from original)
 */
export function getGroupDisplayName(groupId: string, groupings?: SetGrouping[]): string {
  if (groupId === 'default-cards') return 'Cards';
  
  const grouping = groupings?.find(g => g.id === groupId);
  return grouping?.displayName || 'Cards';
}

/**
 * Get sort order for a group (unchanged from original)
 */
export function getGroupOrder(groupId: string, groupings?: SetGrouping[]): number {
  if (groupId === 'default-cards') return 999999;
  
  const grouping = groupings?.find(g => g.id === groupId);
  return grouping?.order ?? 999999;
}

/**
 * Clear the matcher cache (useful for memory management)
 */
export function clearGroupingCache(): void {
  matcherCache.clear();
}