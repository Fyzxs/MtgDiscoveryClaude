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
export interface UserSetCardCollectingCounts {
  total: number;
  nonFoil: number;
  foil: number;
  etched: number;
}

export interface UserSetCardCollecting {
  setGroupId: string;
  collecting: boolean;
  counts?: UserSetCardCollectingCounts;
  collectingFinishes?: ('nonFoil' | 'foil' | 'etched')[];
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
  groups?: UserSetCardCollectionGroup[];
}

// Shared types for collection group progress display
export type SetGroupFinishType = 'nonFoil' | 'foil' | 'etched';

export interface GroupFinishProgress {
  finishType: SetGroupFinishType;
  collected: number;
  total: number;
  percentage: number;
  emoji: string;
  isSelected?: boolean;
}

export interface CollectionGroup {
  setGroupId: string;
  displayName: string;
  isCollecting: boolean;
  count?: number;
  finishes: GroupFinishProgress[];
}

// Wishlist types
export interface WishlistCardUpdate {
  cardId: string;
  setId: string;
  count: number;
  finish: CardFinish;
  special: CardSpecial;
}

export interface WishlistUpdateResult {
  success: boolean;
  card?: unknown;
  error?: string;
}

// Entry mode for toggling between collection and wishlist
export type EntryMode = 'collection' | 'wishlist';

// Wishlist entry from the /wishlist page query
export interface WishlistEntry {
  userId: string;
  cardId: string;
  cardName: string;
  setId: string;
  setCode: string;
  setName: string;
  artistIds: string[];
  cardNameGuid: string;
  createdAt: string;
  updatedAt: string;
  wishlistItems: WishlistEntryItem[];
}

export interface WishlistEntryItem {
  finish: string;
  special: string;
  count: number;
}