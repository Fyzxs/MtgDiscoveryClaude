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
}

export interface UserSetCardRarityGroup {
  rarity: string;
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
  groups: UserSetCardRarityGroup[];
}