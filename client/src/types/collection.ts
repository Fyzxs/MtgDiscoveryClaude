export type CardFinish = 'non-foil' | 'foil' | 'etched';
export type CardSpecial = 'none' | 'signed' | 'artist-proof' | 'altered';

export interface CollectionEntryState {
  count: string;
  finish: CardFinish;
  special: CardSpecial;
  isNegative: boolean;
}

export interface CardCollectionUpdate {
  cardId: string;
  setId?: string;
  count: number;
  finish: CardFinish;
  special: CardSpecial;
}

export interface CollectionUpdateResult {
  success: boolean;
  card?: any; // Will be replaced with proper Card type
  error?: string;
}

export const FINISH_DISPLAY_NAMES: Record<CardFinish, string> = {
  'non-foil': 'Non-foil',
  'foil': 'Foil',
  'etched': 'Etched'
};

export const SPECIAL_DISPLAY_NAMES: Record<CardSpecial, string> = {
  'none': '',
  'signed': 'Signed',
  'artist-proof': 'Artist Proof',
  'altered': 'Altered'
};