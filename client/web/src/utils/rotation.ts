/**
 * Utilities for time-based content rotation
 */

const MILLISECONDS_PER_DAY = 86400000;

/**
 * Gets the index for daily rotation of an array of items.
 * The index changes once per day (at midnight UTC) and cycles through all items.
 *
 * @param items - Array of items to rotate through
 * @returns Index into the array for the current day
 *
 * @example
 * const artists = [artist1, artist2, artist3];
 * const todayArtist = artists[getDailyRotationIndex(artists)];
 */
export function getDailyRotationIndex<T>(items: T[]): number {
  if (items.length === 0) {
    return 0;
  }
  return Math.floor(Date.now() / MILLISECONDS_PER_DAY) % items.length;
}

/**
 * Gets the item from an array based on daily rotation.
 * Convenience wrapper around getDailyRotationIndex.
 *
 * @param items - Array of items to rotate through
 * @returns The item for the current day, or undefined if array is empty
 *
 * @example
 * const proTip = getDailyRotatedItem(proTips);
 */
export function getDailyRotatedItem<T>(items: T[]): T | undefined {
  if (items.length === 0) {
    return undefined;
  }
  return items[getDailyRotationIndex(items)];
}
