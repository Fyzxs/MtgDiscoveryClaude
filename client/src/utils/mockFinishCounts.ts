// Mock finish counts for Phase 1 (stubbed backend)
// This will be replaced with real data from GraphQL in Phase 3

export interface FinishCounts {
  nonFoil: number;
  foil: number;
  etched: number;
}

// Pre-foil era sets (before Urza's Legacy 1999 when foils were introduced)
const PRE_FOIL_SETS = ['lea', 'leb', 'arn', 'atq', 'leg', '2ed', 'drk', 'fem', '3ed', '4ed', 'ice', 'chr', 'hml', 'all', 'mir', 'vis', 'wth', '5ed', 'por', 'tmp', 'sth', 'exo'];

// Sets that are non-foil only (modern reprints, etc)
const NON_FOIL_ONLY_SETS = ['ha1', 'ha2', 'ha3', 'ha4', 'ha5', 'ha6', 'ha7'];

// Sets with etched foils (typically premium products from 2020+)
const ETCHED_FOIL_SETS = ['cmr', 'stx', 'mh2', 'afr', 'mid', 'vow', 'neo', 'snc', 'clb', 'dmu', 'bro', 'one', 'mom', 'mat', 'woe', 'lci'];

/**
 * Get mock finish counts for a set group
 * @param setCode - The set code (e.g., 'mir', 'neo', 'khm')
 * @param groupId - The group ID (e.g., 'common', 'rare', 'booster', 'variation')
 * @param baseCount - The total unique cards in this group
 * @returns Mock finish counts
 */
export function getMockFinishCounts(setCode: string, groupId: string, baseCount: number): FinishCounts {
  const lowerSetCode = setCode.toLowerCase();

  // Pre-foil era: only non-foil
  if (PRE_FOIL_SETS.includes(lowerSetCode)) {
    return {
      nonFoil: baseCount,
      foil: 0,
      etched: 0
    };
  }

  // Non-foil only sets
  if (NON_FOIL_ONLY_SETS.includes(lowerSetCode)) {
    return {
      nonFoil: baseCount,
      foil: 0,
      etched: 0
    };
  }

  // Modern sets with etched foils
  if (ETCHED_FOIL_SETS.includes(lowerSetCode)) {
    // Etched typically only on rares/mythics
    const hasEtched = ['rare', 'mythic'].includes(groupId.toLowerCase());
    const etchedCount = hasEtched ? Math.floor(baseCount * 0.3) : 0; // ~30% of rares/mythics have etched

    return {
      nonFoil: baseCount,
      foil: baseCount,
      etched: etchedCount
    };
  }

  // Default modern set: non-foil and foil available for all cards
  return {
    nonFoil: baseCount,
    foil: baseCount,
    etched: 0
  };
}

/**
 * Get mock collecting finishes for a group
 * By default, when a group is first checked, all available finishes are selected
 */
export function getDefaultCollectingFinishes(finishCounts: FinishCounts): ('nonFoil' | 'foil' | 'etched')[] {
  const finishes: ('nonFoil' | 'foil' | 'etched')[] = [];

  if (finishCounts.nonFoil > 0) {
    finishes.push('nonFoil');
  }

  if (finishCounts.foil > 0) {
    finishes.push('foil');
  }

  if (finishCounts.etched > 0) {
    finishes.push('etched');
  }

  return finishes;
}
