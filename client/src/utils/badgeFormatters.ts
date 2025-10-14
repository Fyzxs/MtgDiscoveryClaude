/**
 * Badge formatting utilities for MTG card badges
 * Extracted from CardBadges component for reusability
 */

/**
 * Format finish text for display
 */
export const formatFinishText = (finish: string): string => {
  const formatMap: Record<string, string> = {
    'foil': 'Foil',
    'etched': 'Etched',
    'glossy': 'Glossy',
    'nonfoil': 'Non-Foil',
    'galaxyfoil': 'Galaxy Foil',
  };
  return formatMap[finish.toLowerCase()] || finish;
};

/**
 * Format promo type text for display
 */
export const formatPromoText = (promoType: string): string => {
  const lowerType = promoType.toLowerCase();

  // Handle FF + Roman numerals pattern (e.g., "ffxiv" -> "FF XIV")
  const ffRomanMatch = lowerType.match(/^ff([ivxlcdm]+)$/);
  if (ffRomanMatch) {
    return `FF ${ffRomanMatch[1].toUpperCase()}`;
  }

  const formatMap: Record<string, string> = {
    'prerelease': 'Pre-Release',
    'promobundle': 'Promo Bundle',
    'buyabox': 'Buy-a-Box',
    'intro_pack': 'Intro Pack',
    'gameday': 'Game Day',
    'release': 'Release',
    'fnm': 'FNM',
    'judge_gift': 'Judge Gift',
    'arena_league': 'Arena League',
    'player_rewards': 'Player Rewards',
    'gateway': 'Gateway',
    'wizards_play_network': 'WPN',
    'duels': 'Duels',
    'premiere_shop': 'Premiere Shop',
    'grand_prix': 'Grand Prix',
    'pro_tour': 'Pro Tour',
    'worlds': 'Worlds',
    'convention': 'Convention',
    'treasure_chest': 'Treasure Chest',
    'boosterfun': 'Booster Fun',
    'serialized': 'Serialized',
    'surgefoil': 'Surge Foil',
    'raisedfoil': 'Raised Foil',
    'sldbonus': 'SLD Bonus',
    'datestamped': 'Date Stamped',
    'promopack': 'Promo Pack',
    'starterdeck': 'Starter Deck',
  };
  return formatMap[lowerType] || promoType.replace(/_/g, ' ')
    .split(' ')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1))
    .join(' ');
};

/**
 * Format frame effect text for display
 */
export const formatFrameEffectText = (effect: string): string => {
  const formatMap: Record<string, string> = {
    'legendary': 'Legendary',
    'miracle': 'Miracle',
    'nyxtouched': 'Nyxtouched',
    'draft': 'Draft',
    'devoid': 'Devoid',
    'tombstone': 'Tombstone',
    'colorshifted': 'Colorshifted',
    'inverted': 'Inverted',
    'sunmoondfc': 'Sun/Moon DFC',
    'compasslanddfc': 'Compass Land DFC',
    'originpwdfc': 'Origin PW DFC',
    'mooneldrazidfc': 'Moon Eldrazi DFC',
    'moonreversemoondfc': 'Moon Reverse Moon DFC',
    'showcase': 'Showcase',
    'extendedart': 'Extended Art',
    'companion': 'Companion',
    'etched': 'Etched',
    'snow': 'Snow',
    'lesson': 'Lesson',
    'shatteredglass': 'Shattered Glass',
    'convertdfc': 'Convert DFC',
    'fandfc': 'Fan DFC',
    'upsidedowndfc': 'Upside Down DFC',
    'fullart': 'Full Art',
    'textless': 'Textless',
  };
  return formatMap[effect.toLowerCase()] || effect.replace(/_/g, ' ')
    .split(' ')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1))
    .join(' ');
};

/**
 * Get finish color for badge
 */
export const getFinishColor = (finish: string): 'primary' | 'secondary' | 'default' | 'info' => {
  switch (finish.toLowerCase()) {
    case 'foil':
      return 'primary';
    case 'etched':
      return 'secondary';
    case 'nonfoil':
      return 'info';
    default:
      return 'default';
  }
};
