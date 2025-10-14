import type { Card } from '../types/card';
import type { SetGrouping } from '../types/set';

/**
 * Check if a card matches a grouping's filters
 */
export function cardMatchesGrouping(card: Card, grouping: SetGrouping): boolean {
  const { filters } = grouping;


  // Check collector number range if specified
  if (filters.collectorNumberRange) {
    const { min, max, orConditions } = filters.collectorNumberRange;
    const cardNumber = card.collectorNumber;

    if (!cardNumber) return false;

    let matchesRange = false;

    // Check if card matches the min-max range
    if (min !== null && max !== null) {
      // For range matching, we ALWAYS extract just the numeric part
      // This allows cards like "30s", "118†s" to match against numeric ranges
      const getNumericValue = (cn: string): number => {
        // Extract leading digits from the string
        const match = cn.match(/^(\d+)/);
        return match ? parseInt(match[1], 10) : 0;
      };

      const cardNum = getNumericValue(cardNumber);
      const minNum = getNumericValue(min);
      const maxNum = getNumericValue(max);

      matchesRange = cardNum >= minNum && cardNum <= maxNum;
    }

    // Check if card matches any of the OR conditions (specific collector numbers)
    let matchesOrCondition = false;
    if (orConditions && orConditions.length > 0) {
      // Normalize the card's collector number for comparison
      const normalizedCardNumber = cardNumber.toLowerCase().trim();

      // Check if the card's collector number matches any of the OR conditions
      matchesOrCondition = orConditions.some(cn => {
        const normalizedCondition = cn.toLowerCase().trim();
        const matches = normalizedCardNumber === normalizedCondition;
        return matches;
      });
    }

    // Card must match either the range OR one of the specific collector numbers
    const passesCollectorCheck = matchesRange || matchesOrCondition;

    if (!passesCollectorCheck) {
      return false;
    }
  }

  // Helper function to convert snake_case to camelCase
  const snakeToCamel = (str: string): string => {
    return str.replace(/_([a-z])/g, (_, letter) => letter.toUpperCase());
  };

  // Check properties if specified
  if (filters.properties && Object.keys(filters.properties).length > 0) {
    for (const [key, value] of Object.entries(filters.properties)) {
      // Handle different types of property checks
      if (key.endsWith('_excludes')) {
        // Check that the property does NOT contain the value
        let propName = key.replace('_excludes', '');
        // Convert snake_case to camelCase for property lookup
        propName = snakeToCamel(propName);
        const cardValue = (card as unknown as Record<string, unknown>)[propName];
        if (cardValue && typeof cardValue === 'string') {
          if (cardValue.toLowerCase().includes(String(value).toLowerCase())) {
            return false;
          }
        }
      } else if (key.endsWith('_contains')) {
        // Check that the property contains the value
        let propName = key.replace('_contains', '');
        // Convert snake_case to camelCase for property lookup
        propName = snakeToCamel(propName);
        const cardValue = (card as unknown as Record<string, unknown>)[propName];
        if (!cardValue || typeof cardValue !== 'string') {
          return false;
        }
        if (!cardValue.toLowerCase().includes(String(value).toLowerCase())) {
          return false;
        }
      } else {
        // Direct property match or array contains
        // Handle special property mappings first
        let cardValue: unknown;

        if (key === 'date') {
          // Map 'date' property to card's releasedAt field
          cardValue = card.releasedAt;
        } else if (key === 'frame') {
          // Frame filters should always check frameEffects array, not the direct frame property
          // The card's frame property (e.g., "2015") is unrelated to frame filters (e.g., "extendedart")
          cardValue = undefined;
        } else {
          // Try both the original key and the camelCase version
          cardValue = (card as unknown as Record<string, unknown>)[key];
          if (cardValue === undefined) {
            // Try converting snake_case to camelCase
            const camelKey = snakeToCamel(key);
            cardValue = (card as unknown as Record<string, unknown>)[camelKey];
          }
        }

        // Check direct property
        if (cardValue !== undefined) {
          // Boolean comparison
          if (typeof value === 'boolean') {
            if (cardValue !== value) return false;
          }
          // String comparison (case insensitive)
          else if (typeof cardValue === 'string' && typeof value === 'string') {
            if (cardValue.toLowerCase() !== value.toLowerCase()) return false;
          }
          // Direct value comparison
          else if (cardValue !== value) {
            return false;
          }
        }
        // Check in array properties (finishes, frameEffects, promoTypes)
        else {
          let found = false;

          // When value is boolean true, check if the key exists in arrays
          // When value is string, check if the value exists in arrays
          if (typeof value === 'boolean' && value === true) {
            // For boolean true, check if the property name exists in the arrays
            // Check in finishes array
            if (card.finishes && Array.isArray(card.finishes)) {
              found = card.finishes.some(f => f.toLowerCase() === key.toLowerCase());
            }

            // Check in frameEffects array
            if (!found && card.frameEffects && Array.isArray(card.frameEffects)) {
              found = card.frameEffects.some(f => f.toLowerCase() === key.toLowerCase());
            }

            // Check in promoTypes array
            if (!found && card.promoTypes && Array.isArray(card.promoTypes)) {
              found = card.promoTypes.some(p => p.toLowerCase() === key.toLowerCase());
            }
          } else {
            // For string values, check if the value exists in arrays
            // Check in finishes array
            if (card.finishes && Array.isArray(card.finishes)) {
              found = card.finishes.some(f => f.toLowerCase() === String(value).toLowerCase());
            }

            // Check in frameEffects array
            if (!found && card.frameEffects && Array.isArray(card.frameEffects)) {
              found = card.frameEffects.some(f => f.toLowerCase() === String(value).toLowerCase());
            }

            // Check in promoTypes array
            if (!found && card.promoTypes && Array.isArray(card.promoTypes)) {
              found = card.promoTypes.some(p => p.toLowerCase() === String(value).toLowerCase());
            }
          }

          // Check special properties that map to arrays
          if (!found) {
            // Map special property names to array fields
            if (key === 'frame' && card.frameEffects) {
              found = card.frameEffects.some(f => f.toLowerCase() === String(value).toLowerCase());
            } else if (key === 'border' && card.borderColor) {
              found = card.borderColor.toLowerCase() === String(value).toLowerCase();
            }
          }

          // For boolean false values, we want NOT found
          if (typeof value === 'boolean' && !value) {
            if (found) return false;
          } else if (!found) {
            return false;
          }
        }
      }
    }
  }

  // If we reach here, the card passed all filters (or there were no filters)
  return true;
}

/**
 * Group cards according to set groupings
 */
export function groupCardsBySetGroupings(
  cards: Card[],
  groupings?: SetGrouping[]
): Map<string, Card[]> {
  const grouped = new Map<string, Card[]>();
  const assignedCards = new Set<string>();

  if (groupings && groupings.length > 0) {
    // Sort groupings by order (reverse - process from highest to lowest)
    const sortedGroupings = [...groupings].sort((a, b) => b.order - a.order);

    // Assign cards to groups (processing backwards through the groups)
    for (const grouping of sortedGroupings) {
      const groupCards: Card[] = [];

      for (const card of cards) {
        // Skip if card already assigned to a group
        if (assignedCards.has(card.id)) continue;

        if (cardMatchesGrouping(card, grouping)) {
          groupCards.push(card);
          assignedCards.add(card.id);
        }
      }

      // Add group even if empty (for debugging)
      grouped.set(grouping.id, groupCards);
    }
  }

  // Add remaining unassigned cards to default "Cards" group
  const unassignedCards = cards.filter(card => !assignedCards.has(card.id));
  if (unassignedCards.length > 0 || grouped.size === 0) {
    grouped.set('default-cards', unassignedCards);
  }

  return grouped;
}

/**
 * Get display name for a group
 */
export function getGroupDisplayName(groupId: string, groupings?: SetGrouping[]): string {
  if (groupId === 'default-cards') return 'Cards';

  const grouping = groupings?.find(g => g.id === groupId);
  return grouping?.displayName || 'Cards';
}

/**
 * Get sort order for a group
 */
export function getGroupOrder(groupId: string, groupings?: SetGrouping[]): number {
  if (groupId === 'default-cards') return 999999; // Put default group last

  const grouping = groupings?.find(g => g.id === groupId);
  return grouping?.order ?? 999999;
}