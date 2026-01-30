export const COLLECTION_EMOJIS = {
  empty: '⭕',
  nonfoil: '🔹',
  foil: '✨',
  etched: '🌟',
  proof: '📜',
  signed: '✍️',
  altered: '🎨',
} as const;

export const COLLECTION_EMOJI_LABELS: Record<string, string> = {
  [COLLECTION_EMOJIS.empty]: 'Not collected',
  [COLLECTION_EMOJIS.nonfoil]: 'Nonfoil',
  [COLLECTION_EMOJIS.foil]: 'Foil',
  [COLLECTION_EMOJIS.etched]: 'Etched',
  [COLLECTION_EMOJIS.proof]: 'Proof',
  [COLLECTION_EMOJIS.signed]: 'Signed',
  [COLLECTION_EMOJIS.altered]: 'Altered',
};
