/**
 * Context information for sealed product display
 * Controls conditional rendering of information based on viewing context
 */
export interface SealedProductContext {
  /** Hide category badge (e.g., when filtering by category) */
  hideCategory?: boolean;
  /** Hide release date (e.g., when sorting by release date) */
  hideReleaseDate?: boolean;
  /** User has collector data available */
  hasCollector?: boolean;
  /** Currently on a set page (may affect info display) */
  isOnSetPage?: boolean;
}
