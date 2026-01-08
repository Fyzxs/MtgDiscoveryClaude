import { useCollection } from '../contexts/CollectionContext';

/**
 * Hook to get the last modification delta for a specific card.
 * Returns undefined if the card hasn't been modified in this session.
 */
export function useLastDelta(cardId: string): number | undefined {
  const { getLastDelta, lastDeltaVersion } = useCollection();
  // lastDeltaVersion ensures this hook re-evaluates when deltas change
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const _version = lastDeltaVersion;
  return getLastDelta(cardId);
}
