import type { Card } from './card';

export interface CardGroupConfig {
  id: string;
  name: string;
  displayName: string;
  cards: Card[];
  isVisible: boolean;
  
  // Special handling flags
  isPromoType?: boolean;
  isFoilOnly?: boolean;
  isVariation?: boolean;
  isBooster?: boolean;
  isPromo?: boolean;
}

export interface CardGroupsState {
  groups: Map<string, CardGroupConfig>;
  visibleGroupIds: Set<string>;
  allGroupIds: string[];
}