export const setTypeColors: Record<string, string> = {
  // Core & Expansion sets (Green family)
  'core': '#388E3C',
  'expansion': '#2E7D32',

  // Reprint/Masters/Starter sets (Orange family)
  'masters': '#FF6F00',
  'masterpiece': '#FF8F00',
  'starter': '#FFA726',

  // Commander sets (Purple family)
  'commander': '#7B1FA2',
  'planechase': '#5E35B1',
  'archenemy': '#6A1B9A',
  'vanguard': '#4A148C',

  // Gift/Premium sets (Blue family)
  'arsenal': '#0D47A1',
  'from_the_vault': '#1565C0',
  'spellbook': '#1976D2',
  'premium_deck': '#1E88E5',
  'box': '#2196F3',

  // Versus/Draft sets (Teal family)
  'duel_deck': '#00695C',
  'draft_innovation': '#00796B',

  // Special/Promo sets (Pink-Purple gradient)
  'promo': '#E91E63',
  'token': '#EC407A',
  'funny': '#AB47BC',
  'memorabilia': '#BA68C8',
  'minigame': '#CE93D8',

  // Default for unknown types
  'default': '#616161'
};

export const getSetTypeColor = (setType: string): string => {
  return setTypeColors[setType] || setTypeColors['default'];
};