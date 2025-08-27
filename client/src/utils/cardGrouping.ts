import type { Card } from '../types/card';
import type { SetGrouping } from '../types/set';

/**
 * Check if a card matches a grouping's filters
 */
export function cardMatchesGrouping(card: Card, grouping: SetGrouping): boolean {
  const { filters } = grouping;
  
  // Check collector number range if specified
  if (filters.collectorNumberRange) {
    const { min, max } = filters.collectorNumberRange;
    const cardNumber = card.collectorNumber;
    
    if (!cardNumber) return false;
    
    // Extract numeric part for comparison
    const getNumericValue = (cn: string): number => {
      const match = cn.match(/^(\d+)/);
      return match ? parseInt(match[1], 10) : 0;
    };
    
    const cardNum = getNumericValue(cardNumber);
    const minNum = getNumericValue(min);
    const maxNum = getNumericValue(max);
    
    if (cardNum < minNum || cardNum > maxNum) {
      return false;
    }
  }
  
  // Check properties if specified
  if (filters.properties && Object.keys(filters.properties).length > 0) {
    for (const [key, value] of Object.entries(filters.properties)) {
      // Handle exclusion rules - if there's an excludes, ignore the contains
      const excludesKey = key.replace('_contains', '_excludes');
      const containsKey = key.replace('_excludes', '_contains');
      
      // Skip if this is a contains and there's an excludes for the same property
      if (key.endsWith('_contains') && filters.properties[excludesKey] !== undefined) {
        continue;
      }
      
      // Handle different types of property checks
      if (key.endsWith('_excludes')) {
        // Check that the property does NOT contain the value
        const propName = key.replace('_excludes', '');
        const cardValue = (card as any)[propName];
        if (cardValue && typeof cardValue === 'string') {
          if (cardValue.toLowerCase().includes(String(value).toLowerCase())) {
            return false;
          }
        }
      } else if (key.endsWith('_contains')) {
        // Check that the property contains the value
        const propName = key.replace('_contains', '');
        const cardValue = (card as any)[propName];
        if (!cardValue || typeof cardValue !== 'string') {
          return false;
        }
        if (!cardValue.toLowerCase().includes(String(value).toLowerCase())) {
          return false;
        }
      } else {
        // Direct property match or array contains
        const cardValue = (card as any)[key];
        
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
    // Sort groupings by order
    const sortedGroupings = [...groupings].sort((a, b) => a.order - b.order);
    
    // Assign cards to groups
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