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
  setGroupId?: string;
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