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
  setId: string;
  setGroupId: string | null;
  count: number;
  finish: CardFinish;
  special: CardSpecial;
}

export interface CollectionUpdateResult {
  success: boolean;
  card?: unknown;
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

// UserSetCards types based on GraphQL response
export interface UserSetCardFinishGroup {
  cards: string[];
}

export interface UserSetCardGroupValue {
  nonFoil: UserSetCardFinishGroup;
  foil: UserSetCardFinishGroup;
  etched: UserSetCardFinishGroup;
}

export interface UserSetCardGroup {
  key: string;
  value: UserSetCardGroupValue;
}

export interface UserSetCard {
  userId: string;
  setId: string;
  totalCards: number;
  uniqueCards: number;
  groupsCollecting: string[];
  groups: UserSetCardGroup[];
}

export interface UserSetCardResponse {
  userSetCard: {
    __typename: string;
    data?: UserSetCard;
    status?: {
      message: string;
      statusCode: number;
    };
  };
}

// UserSetCard Collection data embedded in Set queries
export interface UserSetCardCollecting {
  setGroupId: string;
  collecting: boolean;
  count: number;
  collectingFinishes?: ('nonFoil' | 'foil' | 'etched')[]; // NEW: Which finishes are being collected (stubbed for Phase 1)
}

/**
 * User's card collection data for a specific set group.
 *
 * IMPORTANT: This represents a SET GROUP (e.g., "booster", "extended-art", "promo"),
 * NOT card rarity (common/uncommon/rare/mythic). Card rarity is a property on individual cards.
 */
export interface UserSetCardCollectionGroup {
  /** Set group identifier like "booster", "extended-art", "promo", "showcase" (NOT rarity) */
  setGroupId: string;
  group: {
    nonFoil: { cards: string[] };
    foil: { cards: string[] };
    etched: { cards: string[] };
  };
}

export interface UserSetCardCollection {
  totalCards: number;
  uniqueCards: number;
  collecting: UserSetCardCollecting[];
  groups: UserSetCardCollectionGroup[];
}