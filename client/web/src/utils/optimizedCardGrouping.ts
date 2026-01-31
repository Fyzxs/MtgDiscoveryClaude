import type { Card } from '../types/card';
import type { SetGrouping } from '../types/set';

/**
 * Optimized function to group cards by setGroupId (primary method)
 * Uses setGroupId from cards to assign them to groups defined in setInfo.groupings
 */
export function groupCardsBySetGroupId(cards: Card[]): Map<string, Card[]> {
  const grouped = new Map<string, Card[]>();

  for (const card of cards) {
    // Handle both null/undefined and string "undefined"
    const rawGroupId = card.setGroupId;
    const groupId = (!rawGroupId || rawGroupId === 'undefined' || rawGroupId === 'null')
      ? 'default-cards'
      : rawGroupId;

    if (!grouped.has(groupId)) {
      grouped.set(groupId, []);
    }
    grouped.get(groupId)!.push(card);
  }

  return grouped;
}

/**
 * Optimized function to group cards using setGroupId and groupings metadata
 * - setInfo.groupings defines what groups exist (metadata, display names, order)
 * - card.setGroupId determines which group each card belongs to
 * - We match card.setGroupId to setInfo.groupings[].id to assign cards to groups
 */
export function groupCardsOptimized(
  cards: Card[],
  groupings?: SetGrouping[]
): Map<string, Card[]> {
  // Use setGroupId-based grouping - cards know which group they belong to
  const grouped = groupCardsBySetGroupId(cards);

  // If we have groupings metadata, ensure all defined groups exist (even if empty)
  if (groupings && groupings.length > 0) {
    for (const grouping of groupings) {
      if (!grouped.has(grouping.id)) {
        grouped.set(grouping.id, []);
      }
    }
  }

  return grouped;
}

/**
 * Get display name for a group
 */
export function getGroupDisplayName(groupId: string, groupings?: SetGrouping[]): string {
  if (groupId === 'default-cards') return 'Cards';

  const grouping = groupings?.find(g => g.id === groupId);
  return grouping?.displayName || groupId; // Use groupId as fallback if no displayName found
}

/**
 * Get sort order for a group
 */
export function getGroupOrder(groupId: string, groupings?: SetGrouping[]): number {
  if (groupId === 'default-cards') return 999999;

  const grouping = groupings?.find(g => g.id === groupId);
  return grouping?.order ?? 999999;
}

/**
 * Clear the grouping cache (placeholder for backwards compatibility)
 */
export function clearGroupingCache(): void {
  // No cache to clear since we're using direct setGroupId lookup
}