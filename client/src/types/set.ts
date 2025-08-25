export interface MtgSet {
  id: string;
  code: string;
  tcgPlayerId?: number;
  name: string;
  uri: string;
  scryfallUri: string;
  searchUri: string;
  releasedAt: string;
  setType: string;
  cardCount: number;
  digital: boolean;
  nonFoilOnly: boolean;
  foilOnly: boolean;
  block?: string;
  iconSvgUri: string;
}

export interface SetContext {
  isOnSetListPage?: boolean;
  isOnBlockPage?: boolean;
  currentBlock?: string;
}