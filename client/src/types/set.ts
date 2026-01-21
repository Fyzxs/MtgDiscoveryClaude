export interface SetGroupingFilter {
  collectorNumberRange?: {
    min: string | null;
    max: string | null;
    orConditions?: string[];
  };
  properties?: Record<string, unknown>;
}

export interface SetGrouping {
  id: string;
  displayName: string;
  order: number;
  cardCount: number;
  rawQuery: string;
  filters: SetGroupingFilter;
  finishCounts?: { // NEW: Actual finish availability (stubbed for Phase 1)
    nonFoil: number;
    foil: number;
    etched: number;
  };
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
  userCollection?: import('./collection').UserSetCardCollection;
}

export interface SetContext {
  isOnSetListPage?: boolean;
  isOnBlockPage?: boolean;
  currentBlock?: string;
}