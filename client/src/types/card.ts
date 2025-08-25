export interface ImageUris {
  small?: string;
  normal?: string;
  large?: string;
  png?: string;
  artCrop?: string;
  borderCrop?: string;
}

export interface Legalities {
  standard?: string;
  future?: string;
  historic?: string;
  timeless?: string;
  gladiator?: string;
  pioneer?: string;
  explorer?: string | null;
  modern?: string;
  legacy?: string;
  pauper?: string;
  vintage?: string;
  penny?: string;
  commander?: string;
  oathbreaker?: string;
  standardBrawl?: string;
  brawl?: string;
  alchemy?: string;
  pauperCommander?: string;
  duel?: string;
  oldschool?: string;
  premodern?: string;
  predh?: string;
}

export interface Prices {
  usd?: string | null;
  usdFoil?: string | null;
  usdEtched?: string | null;
  eur?: string | null;
  eurFoil?: string | null;
  tix?: string | null;
}

export interface RelatedUris {
  gatherer?: string;
  tcgplayerInfiniteArticles?: string;
  tcgplayerInfiniteDecks?: string;
  edhrec?: string;
}

export interface PurchaseUris {
  tcgplayer?: string;
  cardmarket?: string;
  cardhoarder?: string | null;
}

export interface CardFace {
  objectString?: string;
  name?: string;
  manaCost?: string;
  typeLine?: string;
  oracleText?: string;
  colors?: string[];
  colorIndicator?: string[] | null;
  power?: string | null;
  toughness?: string | null;
  loyalty?: string | null;
  defense?: string | null;
  artist?: string;
  artistId?: string;
  illustrationId?: string;
  imageUris?: ImageUris;
  flavorText?: string | null;
  printedName?: string | null;
  printedTypeLine?: string | null;
  printedText?: string | null;
  watermark?: string | null;
  layout?: string;
  cmc?: number;
}

export interface AllPart {
  objectString?: string;
  id?: string;
  component?: string;
  name?: string;
  typeLine?: string;
  uri?: string;
}

export interface Preview {
  source?: string;
  sourceUri?: string;
  previewedAt?: string;
}

export interface Card {
  id: string;
  oracleId?: string;
  multiverseIds?: number[];
  tcgPlayerId?: number;
  cardMarketId?: number;
  name: string;
  lang?: string;
  releasedAt?: string;
  uri?: string;
  scryfallUri?: string;
  layout?: string;
  highResImage?: boolean;
  imageStatus?: string;
  imageUris?: ImageUris;
  manaCost?: string;
  cmc?: number;
  typeLine?: string;
  oracleText?: string;
  power?: string | null;
  toughness?: string | null;
  loyalty?: string | null;
  defense?: string | null;
  lifeModifier?: string | null;
  handModifier?: string | null;
  colors?: string[];
  colorIdentity?: string[];
  colorIndicator?: string[] | null;
  keywords?: string[];
  legalities?: Legalities;
  games?: string[];
  reserved?: boolean;
  foil?: boolean;
  nonFoil?: boolean;
  finishes?: string[];
  oversized?: boolean;
  promo?: boolean;
  reprint?: boolean;
  variation?: boolean;
  setId?: string;
  setCode?: string;
  setName?: string;
  setType?: string;
  setUri?: string;
  setSearchUri?: string;
  scryfallSetUri?: string;
  rulingsUri?: string;
  printsSearchUri?: string;
  collectorNumber?: string;
  digital?: boolean;
  rarity?: string;
  flavorText?: string | null;
  cardBackId?: string;
  artist?: string;
  artistIds?: string[];
  illustrationId?: string;
  borderColor?: string;
  frame?: string;
  frameEffects?: string[] | null;
  securityStamp?: string | null;
  fullArt?: boolean;
  textless?: boolean;
  booster?: boolean;
  storySpotlight?: boolean;
  edhRecRank?: number;
  pennyRank?: number;
  prices?: Prices;
  relatedUris?: RelatedUris;
  purchaseUris?: PurchaseUris;
  cardFaces?: CardFace[] | null;
  allParts?: AllPart[] | null;
  printedName?: string | null;
  printedTypeLine?: string | null;
  printedText?: string | null;
  watermark?: string | null;
  contentWarning?: boolean | null;
  preview?: Preview | null;
  producedMana?: string[] | null;
  attractions?: string[] | null;
}

export type Rarity = 'common' | 'uncommon' | 'rare' | 'mythic' | 'special' | 'bonus';

export interface CardContext {
  isOnSetPage?: boolean;
  isOnArtistPage?: boolean;
  isOnCardPage?: boolean;
  currentArtist?: string;
  currentSet?: string;
  currentSetCode?: string;
  showCollectorInfo?: boolean;
  hideSetInfo?: boolean;
  hideReleaseDate?: boolean;
}