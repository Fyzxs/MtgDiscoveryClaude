/**
 * Badge formatting utilities for MTG card badges
 * Extracted from CardBadges component for reusability
 */

/**
 * Format finish text for display
 */
export const formatFinishText = (finish: string): string => {
  const formatMap: Record<string, string> = {
    'etched': 'Etched',
    'foil': 'Foil',
    'galaxyfoil': 'Galaxy Foil',
    'glossy': 'Glossy',
    'nonfoil': 'Non-Foil',
    'oilslick': 'Oil Slick',
    'ripplefoil': 'Ripple Foil',
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
    'arena_league': 'Arena League',
    'arenaleague': 'Arena League',
    'beginnerbox': 'Beginner Box',
    'boosterfun': 'Booster Fun',
    'boxtopper': 'Box Topper',
    'buyabox': 'Buy-a-Box',
    'convention': 'Convention',
    'datestamped': 'Date Stamped',
    'duels': 'Duels',
    'firstplacefoil': 'First Place Foil',
    'fnm': 'FNM',
    'gameday': 'Game Day',
    'gateway': 'Gateway',
    'giftbox': 'Gift Box',
    'grand_prix': 'Grand Prix',
    'instore': 'In Store',
    'intropack': 'Intro Pack',
    'judgegift': 'Judge Gift',
    'mediainsert': 'Media Insert',
    'planeswalkerdeck': 'Planeswalker Deck',
    'player_rewards': 'Player Rewards',
    'premiereshop': 'Premiere Shop',
    'prerelease': 'Pre-Release',
    'pro_tour': 'Pro Tour',
    'promobundle': 'Promo Bundle',
    'promopack': 'Promo Pack',
    'raisedfoil': 'Raised Foil',
    'release': 'Release',
    'schinesealtart': 'Chinese Alt Art',
    'serialized': 'Serialized',
    'sldbonus': 'SLD Bonus',
    'startercollection': 'Starter Collection',
    'starterdeck': 'Starter Deck',
    'surgefoil': 'Surge Foil',
    'treasurechest': 'Treasure Chest',
    'wizardsplaynetwork': 'WPN',
    'worlds': 'Worlds',
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
    'colorshifted': 'Colorshifted',
    'companion': 'Companion',
    'compasslanddfc': 'Compass Land DFC',
    'convertdfc': 'Convert DFC',
    'devoid': 'Devoid',
    'draft': 'Draft',
    'etched': 'Etched',
    'extendedart': 'Extended Art',
    'fandfc': 'Fan DFC',
    'fullart': 'Full Art',
    'inverted': 'Inverted',
    'legendary': 'Legendary',
    'lesson': 'Lesson',
    'miracle': 'Miracle',
    'mooneldrazidfc': 'Moon Eldrazi DFC',
    'moonlitland': 'Moonlit Land',
    'moonreversemoondfc': 'Moon Reverse Moon DFC',
    'nyxtouched': 'Nyxtouched',
    'originpwdfc': 'Origin PW DFC',
    'shatteredglass': 'Shattered Glass',
    'showcase': 'Showcase',
    'snow': 'Snow',
    'sunmoondfc': 'Sun/Moon DFC',
    'textless': 'Textless',
    'tombstone': 'Tombstone',
    'universesbeyond': 'Universes Beyond',
    'upsidedowndfc': 'Upside Down DFC',
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
