export interface SetGroupingFilter {
  collectorNumberRange?: {
    min: string;
    max: string;
  };
  properties?: Record<string, any>;
}

export interface SetGrouping {
  id: string;
  displayName: string;
  order: number;
  cardCount: number;
  rawQuery: string;
  filters: SetGroupingFilter;
}

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
  printedSize?: number;
  digital: boolean;
  nonFoilOnly: boolean;
  foilOnly: boolean;
  block?: string;
  iconSvgUri: string;
  groupings?: SetGrouping[];
}

export interface SetContext {
  isOnSetListPage?: boolean;
  isOnBlockPage?: boolean;
  currentBlock?: string;
}