/* eslint-disable */
import type { TypedDocumentNode as DocumentNode } from '@graphql-typed-document-node/core';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  Any: { input: any; output: any; }
};

/** Input for adding cards to a user's collection */
export type AddCardToCollectionInput = {
  /** The unique identifier of the card */
  cardId: Scalars['String']['input'];
  /** The unique identifier of the set */
  setId: Scalars['String']['input'];
  /** The collected item with its finish and count */
  userCardDetails: CollectedItemInput;
  /** The user Id of the user adding the card */
  userId: Scalars['String']['input'];
};

/** Union type for different response types from AddCardToCollection mutation */
export type AddCardToCollectionResponse = CardsSuccessResponse | FailureResponse;

/** Input for adding a set group to a user's set cards collection */
export type AddSetGroupToUserSetCardInput = {
  /** Whether the user is collecting this set group */
  collecting: Scalars['Boolean']['input'];
  /** The finishes being collected for this set group (e.g., 'nonFoil', 'foil', 'etched') */
  collectingFinishes: Array<Scalars['String']['input']>;
  /** The card counts by finish type for this set group */
  counts: FinishCountsInput;
  /** The unique identifier of the set group */
  setGroupId: Scalars['String']['input'];
  /** The unique identifier of the set */
  setId: Scalars['String']['input'];
};

/** Related card parts */
export type AllParts = {
  __typename?: 'AllParts';
  /** The component type */
  component?: Maybe<Scalars['String']['output']>;
  /** The card ID */
  id?: Maybe<Scalars['String']['output']>;
  /** The card name */
  name?: Maybe<Scalars['String']['output']>;
  /** The type line */
  typeLine?: Maybe<Scalars['String']['output']>;
  /** The Scryfall API URI */
  uri?: Maybe<Scalars['String']['output']>;
};

export type AllSetsArgEntityInput = {
  userId?: InputMaybe<Scalars['String']['input']>;
};

export type ApiMutation = {
  __typename?: 'ApiMutation';
  addCardToCollection?: Maybe<AddCardToCollectionResponse>;
  addSetGroupToUserSetCard?: Maybe<UserSetCardResponse>;
  registerUserInfo?: Maybe<UserRegistrationResponse>;
  testMutation?: Maybe<Scalars['String']['output']>;
};


export type ApiMutationAddCardToCollectionArgs = {
  args?: InputMaybe<AddCardToCollectionInput>;
};


export type ApiMutationAddSetGroupToUserSetCardArgs = {
  input?: InputMaybe<AddSetGroupToUserSetCardInput>;
};

export type ApiQuery = {
  __typename?: 'ApiQuery';
  allSets?: Maybe<SetResponse>;
  artistSearch?: Maybe<ArtistSearchResponse>;
  cardNameSearch?: Maybe<CardNameSearchResponse>;
  cardsByArtist?: Maybe<CardsByArtistResponse>;
  cardsByArtistName?: Maybe<CardsByArtistResponse>;
  cardsById?: Maybe<CardResponse>;
  cardsByName?: Maybe<CardResponse>;
  cardsBySetCode?: Maybe<CardResponse>;
  setsByCode?: Maybe<SetResponse>;
  setsById?: Maybe<SetResponse>;
  test?: Maybe<Scalars['String']['output']>;
  testSet?: Maybe<Scalars['String']['output']>;
  userInfo?: Maybe<UserInfo>;
};


export type ApiQueryAllSetsArgs = {
  args?: InputMaybe<AllSetsArgEntityInput>;
};


export type ApiQueryArtistSearchArgs = {
  searchTerm?: InputMaybe<ArtistSearchTermArgEntityInput>;
};


export type ApiQueryCardNameSearchArgs = {
  searchTerm?: InputMaybe<CardSearchTermArgEntityInput>;
};


export type ApiQueryCardsByArtistArgs = {
  artistId?: InputMaybe<ArtistIdArgEntityInput>;
};


export type ApiQueryCardsByArtistNameArgs = {
  artistName?: InputMaybe<ArtistNameArgEntityInput>;
};


export type ApiQueryCardsByIdArgs = {
  ids?: InputMaybe<CardIdsArgEntityInput>;
};


export type ApiQueryCardsByNameArgs = {
  cardName?: InputMaybe<CardNameArgEntityInput>;
};


export type ApiQueryCardsBySetCodeArgs = {
  setCode?: InputMaybe<SetCodeArgEntityInput>;
};


export type ApiQuerySetsByCodeArgs = {
  codes?: InputMaybe<SetCodesArgEntityInput>;
};


export type ApiQuerySetsByIdArgs = {
  ids?: InputMaybe<SetIdsArgEntityInput>;
};

/** Defines when a policy shall be executed. */
export enum ApplyPolicy {
  /** After the resolver was executed. */
  AfterResolver = 'AFTER_RESOLVER',
  /** Before the resolver was executed. */
  BeforeResolver = 'BEFORE_RESOLVER',
  /** The policy is applied in the validation step before the execution. */
  Validation = 'VALIDATION'
}

export type ArtistIdArgEntityInput = {
  artistId?: InputMaybe<Scalars['String']['input']>;
  userId?: InputMaybe<Scalars['String']['input']>;
};

export type ArtistNameArgEntityInput = {
  artistName?: InputMaybe<Scalars['String']['input']>;
  userId?: InputMaybe<Scalars['String']['input']>;
};

/** Union type for artist search response */
export type ArtistSearchResponse = ArtistSearchResultsSuccessResponse | FailureResponse;

/** An artist search result */
export type ArtistSearchResult = {
  __typename?: 'ArtistSearchResult';
  /** The unique identifier for the artist */
  artistId: Scalars['String']['output'];
  /** The name of the artist */
  name: Scalars['String']['output'];
};

/** Response returned when artist search is successful */
export type ArtistSearchResultsSuccessResponse = {
  __typename?: 'ArtistSearchResultsSuccessResponse';
  /** The list of artist search results */
  data: Array<ArtistSearchResult>;
  /** Metadata about the response */
  metaData?: Maybe<MetaData>;
  /** Status information about the success */
  status?: Maybe<StatusData>;
};

export type ArtistSearchTermArgEntityInput = {
  searchTerm?: InputMaybe<Scalars['String']['input']>;
};

/** Represents a Magic: The Gathering card from Scryfall */
export type Card = {
  __typename?: 'Card';
  /** Related card parts */
  allParts?: Maybe<Array<Maybe<AllParts>>>;
  /** The MTG Arena ID */
  arenaId?: Maybe<Scalars['Int']['output']>;
  /** The artist name */
  artist?: Maybe<Scalars['String']['output']>;
  /** The artist IDs */
  artistIds?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** Attraction lights */
  attractions?: Maybe<Array<Scalars['Int']['output']>>;
  /** Whether this card appears in boosters */
  booster?: Maybe<Scalars['Boolean']['output']>;
  /** The border color */
  borderColor?: Maybe<Scalars['String']['output']>;
  /** The card back ID */
  cardBackId?: Maybe<Scalars['String']['output']>;
  /** Card faces for multi-faced cards */
  cardFaces?: Maybe<Array<Maybe<CardFace>>>;
  /** The Cardmarket ID */
  cardMarketId?: Maybe<Scalars['Int']['output']>;
  /** The converted mana cost */
  cmc?: Maybe<Scalars['Float']['output']>;
  /** The collector number */
  collectorNumber?: Maybe<Scalars['String']['output']>;
  /** The color identity */
  colorIdentity?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** The color indicator */
  colorIndicator?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** The card colors */
  colors?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** Content warning flag */
  contentWarning?: Maybe<Scalars['Boolean']['output']>;
  /** The defense (battles only) */
  defense?: Maybe<Scalars['String']['output']>;
  /** Whether this is a digital card */
  digital?: Maybe<Scalars['Boolean']['output']>;
  /** The EDHREC rank */
  edhRecRank?: Maybe<Scalars['Int']['output']>;
  /** Available finishes */
  finishes?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** The flavor name of the card */
  flavorName?: Maybe<Scalars['String']['output']>;
  /** The flavor text */
  flavorText?: Maybe<Scalars['String']['output']>;
  /** Whether this card is available in foil */
  foil?: Maybe<Scalars['Boolean']['output']>;
  /** The frame version */
  frame?: Maybe<Scalars['String']['output']>;
  /** Frame effects */
  frameEffects?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** Whether this card is full art */
  fullArt?: Maybe<Scalars['Boolean']['output']>;
  /** Whether this card is a game changer */
  gameChanger?: Maybe<Scalars['Boolean']['output']>;
  /** Games this card appears in */
  games?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** The hand modifier (Vanguard only) */
  handModifier?: Maybe<Scalars['String']['output']>;
  /** Whether high-resolution images are available */
  highResImage?: Maybe<Scalars['Boolean']['output']>;
  /** The unique Scryfall identifier of the card */
  id: Scalars['String']['output'];
  /** The illustration ID */
  illustrationId?: Maybe<Scalars['String']['output']>;
  /** The status of image availability */
  imageStatus?: Maybe<Scalars['String']['output']>;
  /** URIs to images of the card */
  imageUris?: Maybe<ImageUris>;
  /** Keywords on this card */
  keywords?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** The language code for this printing */
  lang?: Maybe<Scalars['String']['output']>;
  /** The card layout */
  layout?: Maybe<Scalars['String']['output']>;
  /** Format legalities */
  legalities?: Maybe<Legalities>;
  /** The life modifier (Vanguard only) */
  lifeModifier?: Maybe<Scalars['String']['output']>;
  /** The loyalty (planeswalkers only) */
  loyalty?: Maybe<Scalars['String']['output']>;
  /** The mana cost */
  manaCost?: Maybe<Scalars['String']['output']>;
  /** The Magic Online foil ID */
  mtgoFoilId?: Maybe<Scalars['Int']['output']>;
  /** The Magic Online ID */
  mtgoId?: Maybe<Scalars['Int']['output']>;
  /** The multiverse IDs on Gatherer */
  multiverseIds?: Maybe<Array<Maybe<Scalars['Int']['output']>>>;
  /** The name of the card */
  name?: Maybe<Scalars['String']['output']>;
  /** Whether this card is available in non-foil */
  nonFoil?: Maybe<Scalars['Boolean']['output']>;
  /** The Oracle ID for this card */
  oracleId?: Maybe<Scalars['String']['output']>;
  /** The Oracle text */
  oracleText?: Maybe<Scalars['String']['output']>;
  /** Whether this card is oversized */
  oversized?: Maybe<Scalars['Boolean']['output']>;
  /** The Penny Dreadful rank */
  pennyRank?: Maybe<Scalars['Int']['output']>;
  /** The power (creatures only) */
  power?: Maybe<Scalars['String']['output']>;
  /** Preview information */
  preview?: Maybe<Preview>;
  /** Price information */
  prices?: Maybe<Prices>;
  /** The printed name */
  printedName?: Maybe<Scalars['String']['output']>;
  /** The printed text */
  printedText?: Maybe<Scalars['String']['output']>;
  /** The printed type line */
  printedTypeLine?: Maybe<Scalars['String']['output']>;
  /** The URI to search for prints */
  printsSearchUri?: Maybe<Scalars['String']['output']>;
  /** Mana produced by this card */
  producedMana?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** Whether this card is a promotional printing */
  promo?: Maybe<Scalars['Boolean']['output']>;
  /** The types of promotional printing */
  promoTypes?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** Purchase URIs */
  purchaseUris?: Maybe<PurchaseUris>;
  /** The rarity */
  rarity?: Maybe<Scalars['String']['output']>;
  /** Related URIs */
  relatedUris?: Maybe<RelatedUris>;
  /** The date this card was released */
  releasedAt?: Maybe<Scalars['String']['output']>;
  /** Whether this card is a reprint */
  reprint?: Maybe<Scalars['Boolean']['output']>;
  /** Whether this card is on the reserved list */
  reserved?: Maybe<Scalars['Boolean']['output']>;
  /** The URI for rulings */
  rulingsUri?: Maybe<Scalars['String']['output']>;
  /** The Scryfall web page for the set */
  scryfallSetUri?: Maybe<Scalars['String']['output']>;
  /** The Scryfall web page for this card */
  scryfallUri?: Maybe<Scalars['String']['output']>;
  /** The security stamp */
  securityStamp?: Maybe<Scalars['String']['output']>;
  /** The set code */
  setCode?: Maybe<Scalars['String']['output']>;
  /** The set grouping ID for collector-focused categorization (e.g., borderless, showcase, extended art) */
  setGroupId?: Maybe<Scalars['String']['output']>;
  /** The set ID */
  setId?: Maybe<Scalars['String']['output']>;
  /** The set name */
  setName?: Maybe<Scalars['String']['output']>;
  /** The Scryfall search URI for the set */
  setSearchUri?: Maybe<Scalars['String']['output']>;
  /** The set type */
  setType?: Maybe<Scalars['String']['output']>;
  /** The Scryfall API URI for the set */
  setUri?: Maybe<Scalars['String']['output']>;
  /** Whether this card has a story spotlight */
  storySpotlight?: Maybe<Scalars['Boolean']['output']>;
  /** The TCGPlayer etched foil ID */
  tcgPlayerEtchedId?: Maybe<Scalars['Int']['output']>;
  /** The TCGPlayer ID */
  tcgPlayerId?: Maybe<Scalars['Int']['output']>;
  /** Whether this card is textless */
  textless?: Maybe<Scalars['Boolean']['output']>;
  /** The toughness (creatures only) */
  toughness?: Maybe<Scalars['String']['output']>;
  /** The type line */
  typeLine?: Maybe<Scalars['String']['output']>;
  /** The Scryfall API URI for this card */
  uri?: Maybe<Scalars['String']['output']>;
  /** User's collection information for this card (populated when userId provided) */
  userCollection?: Maybe<Array<Maybe<CollectedItem>>>;
  /** Whether this card is a variation */
  variation?: Maybe<Scalars['Boolean']['output']>;
  /** The watermark */
  watermark?: Maybe<Scalars['String']['output']>;
};

/** A face of a multi-faced card */
export type CardFace = {
  __typename?: 'CardFace';
  /** The artist */
  artist?: Maybe<Scalars['String']['output']>;
  /** The artist ID */
  artistId?: Maybe<Scalars['String']['output']>;
  /** The converted mana cost */
  cmc?: Maybe<Scalars['Float']['output']>;
  /** The color indicator */
  colorIndicator?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** The colors */
  colors?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
  /** The defense */
  defense?: Maybe<Scalars['String']['output']>;
  /** The flavor text */
  flavorText?: Maybe<Scalars['String']['output']>;
  /** The illustration ID */
  illustrationId?: Maybe<Scalars['String']['output']>;
  /** Image URIs */
  imageUris?: Maybe<ImageUris>;
  /** The layout */
  layout?: Maybe<Scalars['String']['output']>;
  /** The loyalty */
  loyalty?: Maybe<Scalars['String']['output']>;
  /** The mana cost */
  manaCost?: Maybe<Scalars['String']['output']>;
  /** The face name */
  name?: Maybe<Scalars['String']['output']>;
  objectString?: Maybe<Scalars['String']['output']>;
  /** The Oracle text */
  oracleText?: Maybe<Scalars['String']['output']>;
  /** The power */
  power?: Maybe<Scalars['String']['output']>;
  /** The printed name */
  printedName?: Maybe<Scalars['String']['output']>;
  /** The printed text */
  printedText?: Maybe<Scalars['String']['output']>;
  /** The printed type line */
  printedTypeLine?: Maybe<Scalars['String']['output']>;
  /** The toughness */
  toughness?: Maybe<Scalars['String']['output']>;
  /** The type line */
  typeLine?: Maybe<Scalars['String']['output']>;
  /** The watermark */
  watermark?: Maybe<Scalars['String']['output']>;
};

export type CardIdsArgEntityInput = {
  cardIds?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  userId?: InputMaybe<Scalars['String']['input']>;
};

export type CardNameArgEntityInput = {
  cardName?: InputMaybe<Scalars['String']['input']>;
  userId?: InputMaybe<Scalars['String']['input']>;
};

/** Union type for card name search response */
export type CardNameSearchResponse = CardNameSearchResultsSuccessResponse | FailureResponse;

export type CardNameSearchResultOutEntity = {
  __typename?: 'CardNameSearchResultOutEntity';
  name?: Maybe<Scalars['String']['output']>;
};

/** Successful response for a card name search */
export type CardNameSearchResultsSuccessResponse = {
  __typename?: 'CardNameSearchResultsSuccessResponse';
  data: Array<CardNameSearchResultOutEntity>;
  metaData?: Maybe<MetaData>;
  status?: Maybe<StatusData>;
};

/** Union type for different response types from CardsById query */
export type CardResponse = CardsSuccessResponse | FailureResponse;

export type CardSearchTermArgEntityInput = {
  searchTerm?: InputMaybe<Scalars['String']['input']>;
};

/** Union type for cards by artist response */
export type CardsByArtistResponse = CardsByArtistSuccessResponse | FailureResponse;

/** Response returned when cards by artist are successfully retrieved */
export type CardsByArtistSuccessResponse = {
  __typename?: 'CardsByArtistSuccessResponse';
  /** The list of cards illustrated by the artist */
  data?: Maybe<Array<Maybe<Card>>>;
  /** Metadata about the response */
  metaData?: Maybe<MetaData>;
  /** Status information about the success */
  status?: Maybe<StatusData>;
};

/** Response returned when cards are successfully retrieved */
export type CardsSuccessResponse = {
  __typename?: 'CardsSuccessResponse';
  /** The list of cards retrieved */
  data?: Maybe<Array<Maybe<Card>>>;
  /** Metadata about the response */
  metaData?: Maybe<MetaData>;
  /** Status information about the success */
  status?: Maybe<StatusData>;
};

/** A collected item variant with finish and special treatment */
export type CollectedItem = {
  __typename?: 'CollectedItem';
  /** The number of this variant owned */
  count: Scalars['Int']['output'];
  /** The finish type (nonfoil, foil, etched) */
  finish: Scalars['String']['output'];
  /** The special treatment (none, showcase, extended, retro, promo, altered, serialized) */
  special: Scalars['String']['output'];
};

/** Represents a collected item with its finish and count */
export type CollectedItemInput = {
  /** The number of copies of this variant */
  count: Scalars['Int']['input'];
  /** The finish type of the card (e.g., 'foil', 'nonfoil') */
  finish: Scalars['String']['input'];
  /** The set grouping ID (e.g., 'borderless', 'showcase'). Null if card has no grouping. */
  setGroupId?: InputMaybe<Scalars['String']['input']>;
  /** Special treatment or version of the card */
  special?: InputMaybe<Scalars['String']['input']>;
};

/** Range of collector numbers */
export type CollectorNumberRange = {
  __typename?: 'CollectorNumberRange';
  max?: Maybe<Scalars['String']['output']>;
  min?: Maybe<Scalars['String']['output']>;
  orConditions?: Maybe<Array<Maybe<Scalars['String']['output']>>>;
};

/** Response returned when the query fails */
export type FailureResponse = {
  __typename?: 'FailureResponse';
  /** Metadata about the response */
  metaData?: Maybe<MetaData>;
  /** Status information about the failure */
  status?: Maybe<StatusData>;
};

/** Card finish counts by type */
export type FinishCounts = {
  __typename?: 'FinishCounts';
  /** Count of etched cards */
  etched: Scalars['Int']['output'];
  /** Count of foil cards */
  foil: Scalars['Int']['output'];
  /** Count of non-foil cards */
  nonFoil: Scalars['Int']['output'];
  /** Total count across all finishes */
  total: Scalars['Int']['output'];
};

/** Card finish counts by type */
export type FinishCountsInput = {
  /** Count of etched cards */
  etched: Scalars['Int']['input'];
  /** Count of foil cards */
  foil: Scalars['Int']['input'];
  /** Count of non-foil cards */
  nonFoil: Scalars['Int']['input'];
  /** Total count across all finishes */
  total: Scalars['Int']['input'];
};

/** Filters for card groupings */
export type GroupingFilters = {
  __typename?: 'GroupingFilters';
  collectorNumberRange?: Maybe<CollectorNumberRange>;
  properties?: Maybe<Scalars['Any']['output']>;
};

/** URIs to various image formats */
export type ImageUris = {
  __typename?: 'ImageUris';
  /** Art crop image */
  artCrop?: Maybe<Scalars['String']['output']>;
  /** Border crop image */
  borderCrop?: Maybe<Scalars['String']['output']>;
  /** Large JPG image */
  large?: Maybe<Scalars['String']['output']>;
  /** Normal JPG image */
  normal?: Maybe<Scalars['String']['output']>;
  /** PNG image */
  png?: Maybe<Scalars['String']['output']>;
  /** Small JPG image */
  small?: Maybe<Scalars['String']['output']>;
};

/** Format legalities for the card */
export type Legalities = {
  __typename?: 'Legalities';
  /** Alchemy legality */
  alchemy?: Maybe<Scalars['String']['output']>;
  /** Brawl legality */
  brawl?: Maybe<Scalars['String']['output']>;
  /** Commander legality */
  commander?: Maybe<Scalars['String']['output']>;
  /** Duel Commander legality */
  duel?: Maybe<Scalars['String']['output']>;
  /** Explorer legality */
  explorer?: Maybe<Scalars['String']['output']>;
  /** Future Standard legality */
  future?: Maybe<Scalars['String']['output']>;
  /** Gladiator legality */
  gladiator?: Maybe<Scalars['String']['output']>;
  /** Historic legality */
  historic?: Maybe<Scalars['String']['output']>;
  /** Legacy legality */
  legacy?: Maybe<Scalars['String']['output']>;
  /** Modern legality */
  modern?: Maybe<Scalars['String']['output']>;
  /** Oathbreaker legality */
  oathbreaker?: Maybe<Scalars['String']['output']>;
  /** Old School legality */
  oldschool?: Maybe<Scalars['String']['output']>;
  /** Pauper legality */
  pauper?: Maybe<Scalars['String']['output']>;
  /** Pauper Commander legality */
  pauperCommander?: Maybe<Scalars['String']['output']>;
  /** Penny Dreadful legality */
  penny?: Maybe<Scalars['String']['output']>;
  /** Pioneer legality */
  pioneer?: Maybe<Scalars['String']['output']>;
  /** PreDH legality */
  predh?: Maybe<Scalars['String']['output']>;
  /** Premodern legality */
  premodern?: Maybe<Scalars['String']['output']>;
  /** Standard legality */
  standard?: Maybe<Scalars['String']['output']>;
  /** Standard Brawl legality */
  standardBrawl?: Maybe<Scalars['String']['output']>;
  /** Timeless legality */
  timeless?: Maybe<Scalars['String']['output']>;
  /** Vintage legality */
  vintage?: Maybe<Scalars['String']['output']>;
};

/** Metadata for the response */
export type MetaData = {
  __typename?: 'MetaData';
  /** Time taken to process the request */
  elapsedTime?: Maybe<Scalars['String']['output']>;
  /** Unique ID for this invocation */
  invocationId?: Maybe<Scalars['String']['output']>;
  /** Timestamp of the response */
  timeStamp?: Maybe<Scalars['String']['output']>;
};

/** Preview information */
export type Preview = {
  __typename?: 'Preview';
  /** The preview date */
  previewedAt?: Maybe<Scalars['String']['output']>;
  /** The preview source */
  source?: Maybe<Scalars['String']['output']>;
  /** The preview source URI */
  sourceUri?: Maybe<Scalars['String']['output']>;
};

/** Price information in various currencies */
export type Prices = {
  __typename?: 'Prices';
  /** EUR price */
  eur?: Maybe<Scalars['String']['output']>;
  /** EUR foil price */
  eurFoil?: Maybe<Scalars['String']['output']>;
  /** MTGO Tix price */
  tix?: Maybe<Scalars['String']['output']>;
  /** USD price */
  usd?: Maybe<Scalars['String']['output']>;
  /** USD etched price */
  usdEtched?: Maybe<Scalars['String']['output']>;
  /** USD foil price */
  usdFoil?: Maybe<Scalars['String']['output']>;
};

/** URIs for purchasing the card */
export type PurchaseUris = {
  __typename?: 'PurchaseUris';
  /** Cardhoarder purchase link */
  cardhoarder?: Maybe<Scalars['String']['output']>;
  /** Cardmarket purchase link */
  cardmarket?: Maybe<Scalars['String']['output']>;
  /** TCGPlayer purchase link */
  tcgplayer?: Maybe<Scalars['String']['output']>;
};

/** Related URIs for additional information */
export type RelatedUris = {
  __typename?: 'RelatedUris';
  /** EDHREC page */
  edhrec?: Maybe<Scalars['String']['output']>;
  /** Gatherer page */
  gatherer?: Maybe<Scalars['String']['output']>;
  /** TCGPlayer articles */
  tcgplayerInfiniteArticles?: Maybe<Scalars['String']['output']>;
  /** TCGPlayer decks */
  tcgplayerInfiniteDecks?: Maybe<Scalars['String']['output']>;
};

/** Artist data from Scryfall */
export type ScryfallArtist = {
  __typename?: 'ScryfallArtist';
  /** The unique identifier for the artist */
  artistId: Scalars['String']['output'];
  /** All known names for this artist */
  artistNames: Array<Scalars['String']['output']>;
  /** Combined searchable artist names */
  artistNamesSearch: Scalars['String']['output'];
  /** IDs of cards illustrated by this artist */
  cardIds: Array<Scalars['String']['output']>;
  /** IDs of sets containing cards by this artist */
  setIds: Array<Scalars['String']['output']>;
};

/** Represents a Magic: The Gathering set from Scryfall */
export type Set = {
  __typename?: 'Set';
  /** The block name for this set, if applicable */
  block?: Maybe<Scalars['String']['output']>;
  /** The block code for this set, if applicable */
  blockCode?: Maybe<Scalars['String']['output']>;
  /** The number of cards in this set */
  cardCount?: Maybe<Scalars['Int']['output']>;
  /** The three to five-letter set code */
  code: Scalars['String']['output'];
  /** Whether this set was only released in a digital format */
  digital?: Maybe<Scalars['Boolean']['output']>;
  /** Whether this set contains only foil cards */
  foilOnly?: Maybe<Scalars['Boolean']['output']>;
  /** Card groupings as displayed on the Scryfall set page */
  groupings?: Maybe<Array<Maybe<SetGrouping>>>;
  /** A URI to an SVG file for this set's icon */
  iconSvgUri: Scalars['String']['output'];
  /** The unique Scryfall identifier of the set */
  id: Scalars['String']['output'];
  /** The English name of the set */
  name: Scalars['String']['output'];
  /** Whether this set contains only nonfoil cards */
  nonFoilOnly?: Maybe<Scalars['Boolean']['output']>;
  printedSize: Scalars['Int']['output'];
  /** The date the set was released or will be released */
  releasedAt?: Maybe<Scalars['String']['output']>;
  /** The Scryfall web page for this set */
  scryfallUri: Scalars['String']['output'];
  /** A Scryfall API URI that you can request to search for cards in this set */
  searchUri: Scalars['String']['output'];
  /** The type of set (core, expansion, masters, etc.) */
  setType: Scalars['String']['output'];
  /** The TCGPlayer group ID for this set */
  tcgPlayerId?: Maybe<Scalars['Int']['output']>;
  /** The Scryfall API URI for this set */
  uri: Scalars['String']['output'];
  /** User-specific collection information for this set (populated when userId provided) */
  userCollection?: Maybe<SetInformationOutEntity>;
};

export type SetCodeArgEntityInput = {
  setCode?: InputMaybe<Scalars['String']['input']>;
  userId?: InputMaybe<Scalars['String']['input']>;
};

export type SetCodesArgEntityInput = {
  setCodes?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  userId?: InputMaybe<Scalars['String']['input']>;
};

/** Card grouping within a set */
export type SetGrouping = {
  __typename?: 'SetGrouping';
  cardCounts?: Maybe<SetGroupingFinishCounts>;
  displayName: Scalars['String']['output'];
  filters?: Maybe<GroupingFilters>;
  id: Scalars['String']['output'];
  order: Scalars['Int']['output'];
  rawQuery: Scalars['String']['output'];
};

/** Card finish counts within a set grouping */
export type SetGroupingFinishCounts = {
  __typename?: 'SetGroupingFinishCounts';
  /** Number of cards available in etched finish */
  etched: Scalars['Int']['output'];
  /** Number of cards available in foil finish */
  foil: Scalars['Int']['output'];
  /** Number of cards available in non-foil finish */
  nonFoil: Scalars['Int']['output'];
  /** Total number of cards in this grouping */
  total: Scalars['Int']['output'];
};

export type SetIdsArgEntityInput = {
  setIds?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  userId?: InputMaybe<Scalars['String']['input']>;
};

export type SetInformationOutEntity = {
  __typename?: 'SetInformationOutEntity';
  collecting?: Maybe<Array<Maybe<UserSetCardCollecting>>>;
  groups?: Maybe<Array<Maybe<UserSetCardCollectionGroup>>>;
  totalCards: Scalars['Int']['output'];
  uniqueCards: Scalars['Int']['output'];
};

/** Union type for different response types from SetsById query */
export type SetResponse = FailureResponse | SetsSuccessResponse;

/** Response returned when sets are successfully retrieved */
export type SetsSuccessResponse = {
  __typename?: 'SetsSuccessResponse';
  /** The list of sets retrieved */
  data?: Maybe<Array<Maybe<Set>>>;
  /** Metadata about the response */
  metaData?: Maybe<MetaData>;
  /** Status information about the success */
  status?: Maybe<StatusData>;
};

/** Status information for the response */
export type StatusData = {
  __typename?: 'StatusData';
  /** Status message */
  message?: Maybe<Scalars['String']['output']>;
  /** HTTP status code */
  statusCode?: Maybe<Scalars['Int']['output']>;
};

/** A user's card collection entry */
export type UserCardCollection = {
  __typename?: 'UserCardCollection';
  /** The unique identifier for the card */
  cardId: Scalars['String']['output'];
  /** The list of collected item variations */
  collectedList: Array<CollectedItem>;
  /** The unique identifier for the set */
  setId: Scalars['String']['output'];
  /** The unique identifier for the user */
  userId: Scalars['String']['output'];
};

/** Input for querying a specific user card */
export type UserCardInput = {
  /** The unique identifier of the card */
  cardId: Scalars['String']['input'];
  /** The unique identifier of the user */
  userId: Scalars['String']['input'];
};

/** Input for querying user cards by card IDs */
export type UserCardsByIdsInput = {
  /** The collection of card IDs to query */
  cardIds: Array<Scalars['String']['input']>;
  /** The unique identifier of the user */
  userId: Scalars['String']['input'];
};

/** Input for querying user cards by set */
export type UserCardsBySetInput = {
  /** The unique identifier of the set */
  setId: Scalars['String']['input'];
  /** The unique identifier of the user */
  userId: Scalars['String']['input'];
};

/** Response for user cards collection queries */
export type UserCardsCollectionResponse = FailureResponse | UserCardsCollectionSuccessResponse;

/** Response returned when querying user cards collection is successful */
export type UserCardsCollectionSuccessResponse = {
  __typename?: 'UserCardsCollectionSuccessResponse';
  /** The user card collection results */
  data: Array<UserCardCollection>;
  /** Metadata about the response */
  metaData?: Maybe<MetaData>;
  /** Status information about the success */
  status?: Maybe<StatusData>;
};

/** Basic user information extracted from JWT */
export type UserInfo = {
  __typename?: 'UserInfo';
  /** The user's email address from JWT claims */
  email: Scalars['String']['output'];
  /** The unique user ID generated from JWT claims */
  userId: Scalars['String']['output'];
};

/** A user registration result */
export type UserRegistration = {
  __typename?: 'UserRegistration';
  /** The display name of the user */
  displayName: Scalars['String']['output'];
  /** The unique identifier for the user */
  userId: Scalars['String']['output'];
};

/** Union type for user registration response */
export type UserRegistrationResponse = FailureResponse | UserRegistrationSuccessResponse;

/** Response returned when user registration is successful */
export type UserRegistrationSuccessResponse = {
  __typename?: 'UserRegistrationSuccessResponse';
  /** The user registration result */
  data: UserRegistration;
  /** Metadata about the response */
  metaData?: Maybe<MetaData>;
  /** Status information about the success */
  status?: Maybe<StatusData>;
};

/** A user's collection summary for a specific set */
export type UserSetCard = {
  __typename?: 'UserSetCard';
  /** Set groups that the user is actively collecting with counts */
  collecting: Array<UserSetCardCollecting>;
  /** Card collection groups organized by set group */
  groups: Array<UserSetCardCollectionGroup>;
  /** The unique identifier for the set */
  setId: Scalars['String']['output'];
  /** The total number of cards in this set */
  totalCards: Scalars['Int']['output'];
  /** The number of unique cards the user has from this set */
  uniqueCards: Scalars['Int']['output'];
  /** The unique identifier for the user */
  userId: Scalars['String']['output'];
};

/** Collecting status for a specific set group */
export type UserSetCardCollecting = {
  __typename?: 'UserSetCardCollecting';
  /** Whether the user is actively collecting this set group */
  collecting: Scalars['Boolean']['output'];
  /** The finishes being collected for this set group (e.g., 'nonFoil', 'foil', 'etched') */
  collectingFinishes: Array<Scalars['String']['output']>;
  /** The card counts by finish type for this collecting group */
  counts: FinishCounts;
  /** The set group ID (e.g., 'normal', 'borderless') */
  setGroupId: Scalars['String']['output'];
};

/** A card group for a set group (e.g., 'booster', 'extended-art', 'promo') */
export type UserSetCardCollectionGroup = {
  __typename?: 'UserSetCardCollectionGroup';
  /** The card group data for this set group */
  group: UserSetCardGroup;
  /** The set group identifier (e.g., 'booster', 'extended-art', 'promo', 'showcase') */
  setGroupId: Scalars['String']['output'];
};

/** A collection of card IDs for a specific finish type */
export type UserSetCardFinishGroup = {
  __typename?: 'UserSetCardFinishGroup';
  /** List of card IDs in this finish group */
  cards: Array<Scalars['String']['output']>;
};

/** A group of cards organized by finish type */
export type UserSetCardGroup = {
  __typename?: 'UserSetCardGroup';
  /** Etched foil cards in this group */
  etched: UserSetCardFinishGroup;
  /** Foil cards in this group */
  foil: UserSetCardFinishGroup;
  /** Non-foil cards in this group */
  nonFoil: UserSetCardFinishGroup;
};

/** Response for user set card queries */
export type UserSetCardResponse = FailureResponse | UserSetCardSuccessResponse;

/** Response returned when querying user set card is successful */
export type UserSetCardSuccessResponse = {
  __typename?: 'UserSetCardSuccessResponse';
  /** The user set card collection result */
  data: UserSetCard;
  /** Metadata about the response */
  metaData?: Maybe<MetaData>;
  /** Status information about the success */
  status?: Maybe<StatusData>;
};

export type CardCollectionFragment = { __typename?: 'Card', userCollection?: Array<{ __typename?: 'CollectedItem', finish: string, special: string, count: number } | null> | null } & { ' $fragmentName'?: 'CardCollectionFragment' };

export type AddCardToCollectionMutationVariables = Exact<{
  args: AddCardToCollectionInput;
}>;


export type AddCardToCollectionMutation = { __typename?: 'ApiMutation', addCardToCollection?: { __typename: 'CardsSuccessResponse', data?: Array<{ __typename?: 'Card', id: string, userCollection?: Array<{ __typename?: 'CollectedItem', finish: string, special: string, count: number } | null> | null } | null> | null } | { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null, statusCode?: number | null } | null } | null };

export type RegisterUserMutationVariables = Exact<{ [key: string]: never; }>;


export type RegisterUserMutation = { __typename?: 'ApiMutation', registerUserInfo?: { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null, statusCode?: number | null } | null } | { __typename: 'UserRegistrationSuccessResponse', data: { __typename?: 'UserRegistration', userId: string, displayName: string }, status?: { __typename?: 'StatusData', message?: string | null, statusCode?: number | null } | null } | null };

export type GetUserInfoQueryVariables = Exact<{ [key: string]: never; }>;


export type GetUserInfoQuery = { __typename?: 'ApiQuery', userInfo?: { __typename?: 'UserInfo', userId: string, email: string } | null };

export type ArtistSearchQueryVariables = Exact<{
  searchTerm: ArtistSearchTermArgEntityInput;
}>;


export type ArtistSearchQuery = { __typename?: 'ApiQuery', artistSearch?: { __typename: 'ArtistSearchResultsSuccessResponse', data: Array<{ __typename?: 'ArtistSearchResult', artistId: string, name: string }> } | { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null, statusCode?: number | null } | null } | null };

export type CardNameSearchQueryVariables = Exact<{
  searchTerm: CardSearchTermArgEntityInput;
}>;


export type CardNameSearchQuery = { __typename?: 'ApiQuery', cardNameSearch?: { __typename: 'CardNameSearchResultsSuccessResponse', data: Array<{ __typename?: 'CardNameSearchResultOutEntity', name?: string | null }> } | { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null } | null } | null };

export type GetCardsByNameQueryVariables = Exact<{
  cardName: CardNameArgEntityInput;
}>;


export type GetCardsByNameQuery = { __typename?: 'ApiQuery', cardsByName?: { __typename: 'CardsSuccessResponse', data?: Array<{ __typename?: 'Card', id: string, oracleId?: string | null, multiverseIds?: Array<number | null> | null, tcgPlayerId?: number | null, cardMarketId?: number | null, name?: string | null, lang?: string | null, releasedAt?: string | null, uri?: string | null, scryfallUri?: string | null, layout?: string | null, highResImage?: boolean | null, imageStatus?: string | null, manaCost?: string | null, cmc?: number | null, typeLine?: string | null, oracleText?: string | null, power?: string | null, toughness?: string | null, loyalty?: string | null, defense?: string | null, lifeModifier?: string | null, handModifier?: string | null, colors?: Array<string | null> | null, colorIdentity?: Array<string | null> | null, colorIndicator?: Array<string | null> | null, keywords?: Array<string | null> | null, games?: Array<string | null> | null, reserved?: boolean | null, foil?: boolean | null, nonFoil?: boolean | null, finishes?: Array<string | null> | null, oversized?: boolean | null, promo?: boolean | null, promoTypes?: Array<string | null> | null, reprint?: boolean | null, variation?: boolean | null, setId?: string | null, setCode?: string | null, setName?: string | null, setType?: string | null, setUri?: string | null, setSearchUri?: string | null, scryfallSetUri?: string | null, rulingsUri?: string | null, printsSearchUri?: string | null, collectorNumber?: string | null, digital?: boolean | null, rarity?: string | null, flavorText?: string | null, cardBackId?: string | null, artist?: string | null, artistIds?: Array<string | null> | null, illustrationId?: string | null, borderColor?: string | null, frame?: string | null, frameEffects?: Array<string | null> | null, securityStamp?: string | null, fullArt?: boolean | null, textless?: boolean | null, booster?: boolean | null, storySpotlight?: boolean | null, edhRecRank?: number | null, pennyRank?: number | null, printedName?: string | null, printedTypeLine?: string | null, printedText?: string | null, watermark?: string | null, contentWarning?: boolean | null, producedMana?: Array<string | null> | null, attractions?: Array<number> | null, setGroupId?: string | null, imageUris?: { __typename?: 'ImageUris', small?: string | null, normal?: string | null, large?: string | null, png?: string | null, artCrop?: string | null, borderCrop?: string | null } | null, legalities?: { __typename?: 'Legalities', standard?: string | null, future?: string | null, historic?: string | null, timeless?: string | null, gladiator?: string | null, pioneer?: string | null, explorer?: string | null, modern?: string | null, legacy?: string | null, pauper?: string | null, vintage?: string | null, penny?: string | null, commander?: string | null, oathbreaker?: string | null, standardBrawl?: string | null, brawl?: string | null, alchemy?: string | null, pauperCommander?: string | null, duel?: string | null, oldschool?: string | null, premodern?: string | null, predh?: string | null } | null, prices?: { __typename?: 'Prices', usd?: string | null, usdFoil?: string | null, usdEtched?: string | null, eur?: string | null, eurFoil?: string | null, tix?: string | null } | null, relatedUris?: { __typename?: 'RelatedUris', gatherer?: string | null, tcgplayerInfiniteArticles?: string | null, tcgplayerInfiniteDecks?: string | null, edhrec?: string | null } | null, purchaseUris?: { __typename?: 'PurchaseUris', tcgplayer?: string | null, cardmarket?: string | null, cardhoarder?: string | null } | null, cardFaces?: Array<{ __typename?: 'CardFace', name?: string | null, manaCost?: string | null, typeLine?: string | null, oracleText?: string | null, colors?: Array<string | null> | null, colorIndicator?: Array<string | null> | null, power?: string | null, toughness?: string | null, loyalty?: string | null, defense?: string | null, artist?: string | null, artistId?: string | null, illustrationId?: string | null, flavorText?: string | null, printedName?: string | null, printedTypeLine?: string | null, printedText?: string | null, watermark?: string | null, layout?: string | null, cmc?: number | null, imageUris?: { __typename?: 'ImageUris', small?: string | null, normal?: string | null, large?: string | null, png?: string | null, artCrop?: string | null, borderCrop?: string | null } | null } | null> | null, allParts?: Array<{ __typename?: 'AllParts', id?: string | null, component?: string | null, name?: string | null, typeLine?: string | null, uri?: string | null } | null> | null, preview?: { __typename?: 'Preview', source?: string | null, sourceUri?: string | null, previewedAt?: string | null } | null, userCollection?: Array<{ __typename?: 'CollectedItem', finish: string, special: string, count: number } | null> | null } | null> | null } | { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null } | null } | null };

export type GetCardsBySetCodeQueryVariables = Exact<{
  setCode: SetCodeArgEntityInput;
}>;


export type GetCardsBySetCodeQuery = { __typename?: 'ApiQuery', cardsBySetCode?: { __typename: 'CardsSuccessResponse', data?: Array<{ __typename?: 'Card', id: string, oracleId?: string | null, multiverseIds?: Array<number | null> | null, tcgPlayerId?: number | null, cardMarketId?: number | null, name?: string | null, lang?: string | null, releasedAt?: string | null, uri?: string | null, scryfallUri?: string | null, layout?: string | null, highResImage?: boolean | null, imageStatus?: string | null, manaCost?: string | null, cmc?: number | null, typeLine?: string | null, oracleText?: string | null, power?: string | null, toughness?: string | null, loyalty?: string | null, defense?: string | null, lifeModifier?: string | null, handModifier?: string | null, colors?: Array<string | null> | null, colorIdentity?: Array<string | null> | null, colorIndicator?: Array<string | null> | null, keywords?: Array<string | null> | null, games?: Array<string | null> | null, reserved?: boolean | null, foil?: boolean | null, nonFoil?: boolean | null, finishes?: Array<string | null> | null, oversized?: boolean | null, promo?: boolean | null, promoTypes?: Array<string | null> | null, reprint?: boolean | null, variation?: boolean | null, setId?: string | null, setCode?: string | null, setName?: string | null, setType?: string | null, setUri?: string | null, setSearchUri?: string | null, scryfallSetUri?: string | null, rulingsUri?: string | null, printsSearchUri?: string | null, collectorNumber?: string | null, digital?: boolean | null, rarity?: string | null, flavorText?: string | null, cardBackId?: string | null, artist?: string | null, artistIds?: Array<string | null> | null, illustrationId?: string | null, borderColor?: string | null, frame?: string | null, frameEffects?: Array<string | null> | null, securityStamp?: string | null, fullArt?: boolean | null, textless?: boolean | null, booster?: boolean | null, storySpotlight?: boolean | null, edhRecRank?: number | null, pennyRank?: number | null, printedName?: string | null, printedTypeLine?: string | null, printedText?: string | null, watermark?: string | null, contentWarning?: boolean | null, producedMana?: Array<string | null> | null, attractions?: Array<number> | null, setGroupId?: string | null, imageUris?: { __typename?: 'ImageUris', small?: string | null, normal?: string | null, large?: string | null, png?: string | null, artCrop?: string | null, borderCrop?: string | null } | null, legalities?: { __typename?: 'Legalities', standard?: string | null, future?: string | null, historic?: string | null, timeless?: string | null, gladiator?: string | null, pioneer?: string | null, explorer?: string | null, modern?: string | null, legacy?: string | null, pauper?: string | null, vintage?: string | null, penny?: string | null, commander?: string | null, oathbreaker?: string | null, standardBrawl?: string | null, brawl?: string | null, alchemy?: string | null, pauperCommander?: string | null, duel?: string | null, oldschool?: string | null, premodern?: string | null, predh?: string | null } | null, prices?: { __typename?: 'Prices', usd?: string | null, usdFoil?: string | null, usdEtched?: string | null, eur?: string | null, eurFoil?: string | null, tix?: string | null } | null, relatedUris?: { __typename?: 'RelatedUris', gatherer?: string | null, tcgplayerInfiniteArticles?: string | null, tcgplayerInfiniteDecks?: string | null, edhrec?: string | null } | null, purchaseUris?: { __typename?: 'PurchaseUris', tcgplayer?: string | null, cardmarket?: string | null, cardhoarder?: string | null } | null, cardFaces?: Array<{ __typename?: 'CardFace', name?: string | null, manaCost?: string | null, typeLine?: string | null, oracleText?: string | null, colors?: Array<string | null> | null, colorIndicator?: Array<string | null> | null, power?: string | null, toughness?: string | null, loyalty?: string | null, defense?: string | null, artist?: string | null, artistId?: string | null, illustrationId?: string | null, flavorText?: string | null, printedName?: string | null, printedTypeLine?: string | null, printedText?: string | null, watermark?: string | null, layout?: string | null, cmc?: number | null, imageUris?: { __typename?: 'ImageUris', small?: string | null, normal?: string | null, large?: string | null, png?: string | null, artCrop?: string | null, borderCrop?: string | null } | null } | null> | null, allParts?: Array<{ __typename?: 'AllParts', id?: string | null, component?: string | null, name?: string | null, typeLine?: string | null, uri?: string | null } | null> | null, preview?: { __typename?: 'Preview', source?: string | null, sourceUri?: string | null, previewedAt?: string | null } | null, userCollection?: Array<{ __typename?: 'CollectedItem', finish: string, special: string, count: number } | null> | null } | null> | null } | { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null } | null } | null };

export type GetCardsBatchQueryVariables = Exact<{
  ids: CardIdsArgEntityInput;
}>;


export type GetCardsBatchQuery = { __typename?: 'ApiQuery', cardsById?: { __typename: 'CardsSuccessResponse', data?: Array<{ __typename?: 'Card', id: string, oracleId?: string | null, name?: string | null, lang?: string | null, releasedAt?: string | null, uri?: string | null, scryfallUri?: string | null, layout?: string | null, highResImage?: boolean | null, imageStatus?: string | null, manaCost?: string | null, cmc?: number | null, typeLine?: string | null, oracleText?: string | null, power?: string | null, toughness?: string | null, colors?: Array<string | null> | null, colorIdentity?: Array<string | null> | null, keywords?: Array<string | null> | null, reserved?: boolean | null, setId?: string | null, setCode?: string | null, setName?: string | null, setType?: string | null, collectorNumber?: string | null, digital?: boolean | null, rarity?: string | null, artist?: string | null, artistIds?: Array<string | null> | null, illustrationId?: string | null, borderColor?: string | null, frame?: string | null, imageUris?: { __typename?: 'ImageUris', small?: string | null, normal?: string | null, large?: string | null, png?: string | null, artCrop?: string | null, borderCrop?: string | null } | null, prices?: { __typename?: 'Prices', usd?: string | null, usdFoil?: string | null, usdEtched?: string | null, eur?: string | null, eurFoil?: string | null, tix?: string | null } | null, relatedUris?: { __typename?: 'RelatedUris', gatherer?: string | null, tcgplayerInfiniteArticles?: string | null, tcgplayerInfiniteDecks?: string | null, edhrec?: string | null } | null, purchaseUris?: { __typename?: 'PurchaseUris', tcgplayer?: string | null, cardmarket?: string | null, cardhoarder?: string | null } | null, userCollection?: Array<{ __typename?: 'CollectedItem', finish: string, special: string, count: number } | null> | null } | null> | null } | { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null } | null } | null };

export type CardsByArtistNameQueryVariables = Exact<{
  artistName: ArtistNameArgEntityInput;
}>;


export type CardsByArtistNameQuery = { __typename?: 'ApiQuery', cardsByArtistName?: { __typename: 'CardsByArtistSuccessResponse', data?: Array<{ __typename?: 'Card', id: string, oracleId?: string | null, multiverseIds?: Array<number | null> | null, tcgPlayerId?: number | null, cardMarketId?: number | null, name?: string | null, lang?: string | null, releasedAt?: string | null, uri?: string | null, scryfallUri?: string | null, layout?: string | null, highResImage?: boolean | null, imageStatus?: string | null, manaCost?: string | null, cmc?: number | null, typeLine?: string | null, oracleText?: string | null, power?: string | null, toughness?: string | null, loyalty?: string | null, defense?: string | null, lifeModifier?: string | null, handModifier?: string | null, colors?: Array<string | null> | null, colorIdentity?: Array<string | null> | null, colorIndicator?: Array<string | null> | null, keywords?: Array<string | null> | null, games?: Array<string | null> | null, reserved?: boolean | null, foil?: boolean | null, nonFoil?: boolean | null, finishes?: Array<string | null> | null, oversized?: boolean | null, promo?: boolean | null, promoTypes?: Array<string | null> | null, reprint?: boolean | null, variation?: boolean | null, setId?: string | null, setCode?: string | null, setName?: string | null, setType?: string | null, setUri?: string | null, setSearchUri?: string | null, scryfallSetUri?: string | null, rulingsUri?: string | null, printsSearchUri?: string | null, collectorNumber?: string | null, digital?: boolean | null, rarity?: string | null, flavorText?: string | null, cardBackId?: string | null, artist?: string | null, artistIds?: Array<string | null> | null, illustrationId?: string | null, borderColor?: string | null, frame?: string | null, frameEffects?: Array<string | null> | null, securityStamp?: string | null, fullArt?: boolean | null, textless?: boolean | null, booster?: boolean | null, storySpotlight?: boolean | null, edhRecRank?: number | null, pennyRank?: number | null, printedName?: string | null, printedTypeLine?: string | null, printedText?: string | null, watermark?: string | null, contentWarning?: boolean | null, producedMana?: Array<string | null> | null, attractions?: Array<number> | null, setGroupId?: string | null, imageUris?: { __typename?: 'ImageUris', small?: string | null, normal?: string | null, large?: string | null, png?: string | null, artCrop?: string | null, borderCrop?: string | null } | null, legalities?: { __typename?: 'Legalities', standard?: string | null, future?: string | null, historic?: string | null, timeless?: string | null, gladiator?: string | null, pioneer?: string | null, explorer?: string | null, modern?: string | null, legacy?: string | null, pauper?: string | null, vintage?: string | null, penny?: string | null, commander?: string | null, oathbreaker?: string | null, standardBrawl?: string | null, brawl?: string | null, alchemy?: string | null, pauperCommander?: string | null, duel?: string | null, oldschool?: string | null, premodern?: string | null, predh?: string | null } | null, prices?: { __typename?: 'Prices', usd?: string | null, usdFoil?: string | null, usdEtched?: string | null, eur?: string | null, eurFoil?: string | null, tix?: string | null } | null, relatedUris?: { __typename?: 'RelatedUris', gatherer?: string | null, tcgplayerInfiniteArticles?: string | null, tcgplayerInfiniteDecks?: string | null, edhrec?: string | null } | null, purchaseUris?: { __typename?: 'PurchaseUris', tcgplayer?: string | null, cardmarket?: string | null, cardhoarder?: string | null } | null, cardFaces?: Array<{ __typename?: 'CardFace', name?: string | null, manaCost?: string | null, typeLine?: string | null, oracleText?: string | null, colors?: Array<string | null> | null, colorIndicator?: Array<string | null> | null, power?: string | null, toughness?: string | null, loyalty?: string | null, defense?: string | null, artist?: string | null, artistId?: string | null, illustrationId?: string | null, flavorText?: string | null, printedName?: string | null, printedTypeLine?: string | null, printedText?: string | null, watermark?: string | null, layout?: string | null, cmc?: number | null, imageUris?: { __typename?: 'ImageUris', small?: string | null, normal?: string | null, large?: string | null, png?: string | null, artCrop?: string | null, borderCrop?: string | null } | null } | null> | null, allParts?: Array<{ __typename?: 'AllParts', id?: string | null, component?: string | null, name?: string | null, typeLine?: string | null, uri?: string | null } | null> | null, preview?: { __typename?: 'Preview', source?: string | null, sourceUri?: string | null, previewedAt?: string | null } | null, userCollection?: Array<{ __typename?: 'CollectedItem', finish: string, special: string, count: number } | null> | null } | null> | null } | { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null } | null } | null };

export type GetAllSetsQueryVariables = Exact<{
  args?: InputMaybe<AllSetsArgEntityInput>;
}>;


export type GetAllSetsQuery = { __typename?: 'ApiQuery', allSets?: { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null, statusCode?: number | null } | null } | { __typename: 'SetsSuccessResponse', data?: Array<{ __typename?: 'Set', id: string, code: string, tcgPlayerId?: number | null, name: string, uri: string, scryfallUri: string, searchUri: string, releasedAt?: string | null, setType: string, cardCount?: number | null, printedSize: number, digital?: boolean | null, nonFoilOnly?: boolean | null, foilOnly?: boolean | null, block?: string | null, iconSvgUri: string, groupings?: Array<{ __typename?: 'SetGrouping', id: string, cardCounts?: { __typename?: 'SetGroupingFinishCounts', nonFoil: number, foil: number, etched: number } | null } | null> | null, userCollection?: { __typename?: 'SetInformationOutEntity', totalCards: number, uniqueCards: number, collecting?: Array<{ __typename?: 'UserSetCardCollecting', setGroupId: string, collecting: boolean, collectingFinishes: Array<string>, counts: { __typename?: 'FinishCounts', total: number, nonFoil: number, foil: number, etched: number } } | null> | null, groups?: Array<{ __typename?: 'UserSetCardCollectionGroup', setGroupId: string, group: { __typename?: 'UserSetCardGroup', nonFoil: { __typename?: 'UserSetCardFinishGroup', cards: Array<string> }, foil: { __typename?: 'UserSetCardFinishGroup', cards: Array<string> }, etched: { __typename?: 'UserSetCardFinishGroup', cards: Array<string> } } } | null> | null } | null } | null> | null } | null };

export type GetMultipleSetsByCodeQueryVariables = Exact<{
  codes: SetCodesArgEntityInput;
}>;


export type GetMultipleSetsByCodeQuery = { __typename?: 'ApiQuery', setsByCode?: { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null, statusCode?: number | null } | null } | { __typename: 'SetsSuccessResponse', data?: Array<{ __typename?: 'Set', id: string, code: string, tcgPlayerId?: number | null, name: string, uri: string, scryfallUri: string, searchUri: string, releasedAt?: string | null, setType: string, cardCount?: number | null, printedSize: number, digital?: boolean | null, nonFoilOnly?: boolean | null, foilOnly?: boolean | null, block?: string | null, iconSvgUri: string } | null> | null } | null };

export type GetSetByCodeWithGroupingsQueryVariables = Exact<{
  codes: SetCodesArgEntityInput;
}>;


export type GetSetByCodeWithGroupingsQuery = { __typename?: 'ApiQuery', setsByCode?: { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null, statusCode?: number | null } | null } | { __typename: 'SetsSuccessResponse', data?: Array<{ __typename?: 'Set', id: string, code: string, tcgPlayerId?: number | null, name: string, uri: string, scryfallUri: string, searchUri: string, setType: string, cardCount?: number | null, printedSize: number, releasedAt?: string | null, digital?: boolean | null, nonFoilOnly?: boolean | null, foilOnly?: boolean | null, block?: string | null, iconSvgUri: string, groupings?: Array<{ __typename?: 'SetGrouping', id: string, displayName: string, order: number, rawQuery: string, cardCounts?: { __typename?: 'SetGroupingFinishCounts', total: number, nonFoil: number, foil: number, etched: number } | null, filters?: { __typename?: 'GroupingFilters', properties?: any | null, collectorNumberRange?: { __typename?: 'CollectorNumberRange', min?: string | null, max?: string | null, orConditions?: Array<string | null> | null } | null } | null } | null> | null, userCollection?: { __typename?: 'SetInformationOutEntity', totalCards: number, uniqueCards: number, collecting?: Array<{ __typename?: 'UserSetCardCollecting', setGroupId: string, collecting: boolean, collectingFinishes: Array<string>, counts: { __typename?: 'FinishCounts', total: number, nonFoil: number, foil: number, etched: number } } | null> | null, groups?: Array<{ __typename?: 'UserSetCardCollectionGroup', setGroupId: string, group: { __typename?: 'UserSetCardGroup', nonFoil: { __typename?: 'UserSetCardFinishGroup', cards: Array<string> }, foil: { __typename?: 'UserSetCardFinishGroup', cards: Array<string> }, etched: { __typename?: 'UserSetCardFinishGroup', cards: Array<string> } } } | null> | null } | null } | null> | null } | null };

export type AddSetGroupToUserSetCardMutationVariables = Exact<{
  input: AddSetGroupToUserSetCardInput;
}>;


export type AddSetGroupToUserSetCardMutation = { __typename?: 'ApiMutation', addSetGroupToUserSetCard?: { __typename: 'FailureResponse', status?: { __typename?: 'StatusData', message?: string | null, statusCode?: number | null } | null } | { __typename: 'UserSetCardSuccessResponse', data: { __typename?: 'UserSetCard', userId: string, setId: string, totalCards: number, uniqueCards: number, collecting: Array<{ __typename?: 'UserSetCardCollecting', setGroupId: string, collecting: boolean, collectingFinishes: Array<string>, counts: { __typename?: 'FinishCounts', total: number, nonFoil: number, foil: number, etched: number } }> }, status?: { __typename?: 'StatusData', message?: string | null, statusCode?: number | null } | null } | null };

export const CardCollectionFragmentDoc = {"kind":"Document","definitions":[{"kind":"FragmentDefinition","name":{"kind":"Name","value":"CardCollection"},"typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"Card"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"userCollection"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"finish"}},{"kind":"Field","name":{"kind":"Name","value":"special"}},{"kind":"Field","name":{"kind":"Name","value":"count"}}]}}]}}]} as unknown as DocumentNode<CardCollectionFragment, unknown>;
export const AddCardToCollectionDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"mutation","name":{"kind":"Name","value":"AddCardToCollection"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"args"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"AddCardToCollectionInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"addCardToCollection"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"args"},"value":{"kind":"Variable","name":{"kind":"Name","value":"args"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"CardsSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"userCollection"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"finish"}},{"kind":"Field","name":{"kind":"Name","value":"special"}},{"kind":"Field","name":{"kind":"Name","value":"count"}}]}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}},{"kind":"Field","name":{"kind":"Name","value":"statusCode"}}]}}]}}]}}]}}]} as unknown as DocumentNode<AddCardToCollectionMutation, AddCardToCollectionMutationVariables>;
export const RegisterUserDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"mutation","name":{"kind":"Name","value":"RegisterUser"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"registerUserInfo"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"UserRegistrationSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"userId"}},{"kind":"Field","name":{"kind":"Name","value":"displayName"}}]}},{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}},{"kind":"Field","name":{"kind":"Name","value":"statusCode"}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}},{"kind":"Field","name":{"kind":"Name","value":"statusCode"}}]}}]}}]}}]}}]} as unknown as DocumentNode<RegisterUserMutation, RegisterUserMutationVariables>;
export const GetUserInfoDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetUserInfo"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"userInfo"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"userId"}},{"kind":"Field","name":{"kind":"Name","value":"email"}}]}}]}}]} as unknown as DocumentNode<GetUserInfoQuery, GetUserInfoQueryVariables>;
export const ArtistSearchDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"ArtistSearch"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"searchTerm"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"ArtistSearchTermArgEntityInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"artistSearch"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"searchTerm"},"value":{"kind":"Variable","name":{"kind":"Name","value":"searchTerm"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"ArtistSearchResultsSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"artistId"}},{"kind":"Field","name":{"kind":"Name","value":"name"}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}},{"kind":"Field","name":{"kind":"Name","value":"statusCode"}}]}}]}}]}}]}}]} as unknown as DocumentNode<ArtistSearchQuery, ArtistSearchQueryVariables>;
export const CardNameSearchDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"CardNameSearch"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"searchTerm"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"CardSearchTermArgEntityInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cardNameSearch"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"searchTerm"},"value":{"kind":"Variable","name":{"kind":"Name","value":"searchTerm"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"CardNameSearchResultsSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"name"}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}}]}}]}}]}}]}}]} as unknown as DocumentNode<CardNameSearchQuery, CardNameSearchQueryVariables>;
export const GetCardsByNameDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetCardsByName"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"cardName"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"CardNameArgEntityInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cardsByName"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"cardName"},"value":{"kind":"Variable","name":{"kind":"Name","value":"cardName"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"CardsSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"oracleId"}},{"kind":"Field","name":{"kind":"Name","value":"multiverseIds"}},{"kind":"Field","name":{"kind":"Name","value":"tcgPlayerId"}},{"kind":"Field","name":{"kind":"Name","value":"cardMarketId"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"lang"}},{"kind":"Field","name":{"kind":"Name","value":"releasedAt"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallUri"}},{"kind":"Field","name":{"kind":"Name","value":"layout"}},{"kind":"Field","name":{"kind":"Name","value":"highResImage"}},{"kind":"Field","name":{"kind":"Name","value":"imageStatus"}},{"kind":"Field","name":{"kind":"Name","value":"imageUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"small"}},{"kind":"Field","name":{"kind":"Name","value":"normal"}},{"kind":"Field","name":{"kind":"Name","value":"large"}},{"kind":"Field","name":{"kind":"Name","value":"png"}},{"kind":"Field","name":{"kind":"Name","value":"artCrop"}},{"kind":"Field","name":{"kind":"Name","value":"borderCrop"}}]}},{"kind":"Field","name":{"kind":"Name","value":"manaCost"}},{"kind":"Field","name":{"kind":"Name","value":"cmc"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"oracleText"}},{"kind":"Field","name":{"kind":"Name","value":"power"}},{"kind":"Field","name":{"kind":"Name","value":"toughness"}},{"kind":"Field","name":{"kind":"Name","value":"loyalty"}},{"kind":"Field","name":{"kind":"Name","value":"defense"}},{"kind":"Field","name":{"kind":"Name","value":"lifeModifier"}},{"kind":"Field","name":{"kind":"Name","value":"handModifier"}},{"kind":"Field","name":{"kind":"Name","value":"colors"}},{"kind":"Field","name":{"kind":"Name","value":"colorIdentity"}},{"kind":"Field","name":{"kind":"Name","value":"colorIndicator"}},{"kind":"Field","name":{"kind":"Name","value":"keywords"}},{"kind":"Field","name":{"kind":"Name","value":"legalities"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"standard"}},{"kind":"Field","name":{"kind":"Name","value":"future"}},{"kind":"Field","name":{"kind":"Name","value":"historic"}},{"kind":"Field","name":{"kind":"Name","value":"timeless"}},{"kind":"Field","name":{"kind":"Name","value":"gladiator"}},{"kind":"Field","name":{"kind":"Name","value":"pioneer"}},{"kind":"Field","name":{"kind":"Name","value":"explorer"}},{"kind":"Field","name":{"kind":"Name","value":"modern"}},{"kind":"Field","name":{"kind":"Name","value":"legacy"}},{"kind":"Field","name":{"kind":"Name","value":"pauper"}},{"kind":"Field","name":{"kind":"Name","value":"vintage"}},{"kind":"Field","name":{"kind":"Name","value":"penny"}},{"kind":"Field","name":{"kind":"Name","value":"commander"}},{"kind":"Field","name":{"kind":"Name","value":"oathbreaker"}},{"kind":"Field","name":{"kind":"Name","value":"standardBrawl"}},{"kind":"Field","name":{"kind":"Name","value":"brawl"}},{"kind":"Field","name":{"kind":"Name","value":"alchemy"}},{"kind":"Field","name":{"kind":"Name","value":"pauperCommander"}},{"kind":"Field","name":{"kind":"Name","value":"duel"}},{"kind":"Field","name":{"kind":"Name","value":"oldschool"}},{"kind":"Field","name":{"kind":"Name","value":"premodern"}},{"kind":"Field","name":{"kind":"Name","value":"predh"}}]}},{"kind":"Field","name":{"kind":"Name","value":"games"}},{"kind":"Field","name":{"kind":"Name","value":"reserved"}},{"kind":"Field","name":{"kind":"Name","value":"foil"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoil"}},{"kind":"Field","name":{"kind":"Name","value":"finishes"}},{"kind":"Field","name":{"kind":"Name","value":"oversized"}},{"kind":"Field","name":{"kind":"Name","value":"promo"}},{"kind":"Field","name":{"kind":"Name","value":"promoTypes"}},{"kind":"Field","name":{"kind":"Name","value":"reprint"}},{"kind":"Field","name":{"kind":"Name","value":"variation"}},{"kind":"Field","name":{"kind":"Name","value":"setId"}},{"kind":"Field","name":{"kind":"Name","value":"setCode"}},{"kind":"Field","name":{"kind":"Name","value":"setName"}},{"kind":"Field","name":{"kind":"Name","value":"setType"}},{"kind":"Field","name":{"kind":"Name","value":"setUri"}},{"kind":"Field","name":{"kind":"Name","value":"setSearchUri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallSetUri"}},{"kind":"Field","name":{"kind":"Name","value":"rulingsUri"}},{"kind":"Field","name":{"kind":"Name","value":"printsSearchUri"}},{"kind":"Field","name":{"kind":"Name","value":"collectorNumber"}},{"kind":"Field","name":{"kind":"Name","value":"digital"}},{"kind":"Field","name":{"kind":"Name","value":"rarity"}},{"kind":"Field","name":{"kind":"Name","value":"flavorText"}},{"kind":"Field","name":{"kind":"Name","value":"cardBackId"}},{"kind":"Field","name":{"kind":"Name","value":"artist"}},{"kind":"Field","name":{"kind":"Name","value":"artistIds"}},{"kind":"Field","name":{"kind":"Name","value":"illustrationId"}},{"kind":"Field","name":{"kind":"Name","value":"borderColor"}},{"kind":"Field","name":{"kind":"Name","value":"frame"}},{"kind":"Field","name":{"kind":"Name","value":"frameEffects"}},{"kind":"Field","name":{"kind":"Name","value":"securityStamp"}},{"kind":"Field","name":{"kind":"Name","value":"fullArt"}},{"kind":"Field","name":{"kind":"Name","value":"textless"}},{"kind":"Field","name":{"kind":"Name","value":"booster"}},{"kind":"Field","name":{"kind":"Name","value":"storySpotlight"}},{"kind":"Field","name":{"kind":"Name","value":"edhRecRank"}},{"kind":"Field","name":{"kind":"Name","value":"pennyRank"}},{"kind":"Field","name":{"kind":"Name","value":"prices"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"usd"}},{"kind":"Field","name":{"kind":"Name","value":"usdFoil"}},{"kind":"Field","name":{"kind":"Name","value":"usdEtched"}},{"kind":"Field","name":{"kind":"Name","value":"eur"}},{"kind":"Field","name":{"kind":"Name","value":"eurFoil"}},{"kind":"Field","name":{"kind":"Name","value":"tix"}}]}},{"kind":"Field","name":{"kind":"Name","value":"relatedUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"gatherer"}},{"kind":"Field","name":{"kind":"Name","value":"tcgplayerInfiniteArticles"}},{"kind":"Field","name":{"kind":"Name","value":"tcgplayerInfiniteDecks"}},{"kind":"Field","name":{"kind":"Name","value":"edhrec"}}]}},{"kind":"Field","name":{"kind":"Name","value":"purchaseUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"tcgplayer"}},{"kind":"Field","name":{"kind":"Name","value":"cardmarket"}},{"kind":"Field","name":{"kind":"Name","value":"cardhoarder"}}]}},{"kind":"Field","name":{"kind":"Name","value":"cardFaces"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"manaCost"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"oracleText"}},{"kind":"Field","name":{"kind":"Name","value":"colors"}},{"kind":"Field","name":{"kind":"Name","value":"colorIndicator"}},{"kind":"Field","name":{"kind":"Name","value":"power"}},{"kind":"Field","name":{"kind":"Name","value":"toughness"}},{"kind":"Field","name":{"kind":"Name","value":"loyalty"}},{"kind":"Field","name":{"kind":"Name","value":"defense"}},{"kind":"Field","name":{"kind":"Name","value":"artist"}},{"kind":"Field","name":{"kind":"Name","value":"artistId"}},{"kind":"Field","name":{"kind":"Name","value":"illustrationId"}},{"kind":"Field","name":{"kind":"Name","value":"imageUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"small"}},{"kind":"Field","name":{"kind":"Name","value":"normal"}},{"kind":"Field","name":{"kind":"Name","value":"large"}},{"kind":"Field","name":{"kind":"Name","value":"png"}},{"kind":"Field","name":{"kind":"Name","value":"artCrop"}},{"kind":"Field","name":{"kind":"Name","value":"borderCrop"}}]}},{"kind":"Field","name":{"kind":"Name","value":"flavorText"}},{"kind":"Field","name":{"kind":"Name","value":"printedName"}},{"kind":"Field","name":{"kind":"Name","value":"printedTypeLine"}},{"kind":"Field","name":{"kind":"Name","value":"printedText"}},{"kind":"Field","name":{"kind":"Name","value":"watermark"}},{"kind":"Field","name":{"kind":"Name","value":"layout"}},{"kind":"Field","name":{"kind":"Name","value":"cmc"}}]}},{"kind":"Field","name":{"kind":"Name","value":"allParts"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"component"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}}]}},{"kind":"Field","name":{"kind":"Name","value":"printedName"}},{"kind":"Field","name":{"kind":"Name","value":"printedTypeLine"}},{"kind":"Field","name":{"kind":"Name","value":"printedText"}},{"kind":"Field","name":{"kind":"Name","value":"watermark"}},{"kind":"Field","name":{"kind":"Name","value":"contentWarning"}},{"kind":"Field","name":{"kind":"Name","value":"preview"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"source"}},{"kind":"Field","name":{"kind":"Name","value":"sourceUri"}},{"kind":"Field","name":{"kind":"Name","value":"previewedAt"}}]}},{"kind":"Field","name":{"kind":"Name","value":"producedMana"}},{"kind":"Field","name":{"kind":"Name","value":"attractions"}},{"kind":"Field","name":{"kind":"Name","value":"setGroupId"}},{"kind":"Field","name":{"kind":"Name","value":"userCollection"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"finish"}},{"kind":"Field","name":{"kind":"Name","value":"special"}},{"kind":"Field","name":{"kind":"Name","value":"count"}}]}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}}]}}]}}]}}]}}]} as unknown as DocumentNode<GetCardsByNameQuery, GetCardsByNameQueryVariables>;
export const GetCardsBySetCodeDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetCardsBySetCode"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"setCode"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"SetCodeArgEntityInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cardsBySetCode"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"setCode"},"value":{"kind":"Variable","name":{"kind":"Name","value":"setCode"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"CardsSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"oracleId"}},{"kind":"Field","name":{"kind":"Name","value":"multiverseIds"}},{"kind":"Field","name":{"kind":"Name","value":"tcgPlayerId"}},{"kind":"Field","name":{"kind":"Name","value":"cardMarketId"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"lang"}},{"kind":"Field","name":{"kind":"Name","value":"releasedAt"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallUri"}},{"kind":"Field","name":{"kind":"Name","value":"layout"}},{"kind":"Field","name":{"kind":"Name","value":"highResImage"}},{"kind":"Field","name":{"kind":"Name","value":"imageStatus"}},{"kind":"Field","name":{"kind":"Name","value":"imageUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"small"}},{"kind":"Field","name":{"kind":"Name","value":"normal"}},{"kind":"Field","name":{"kind":"Name","value":"large"}},{"kind":"Field","name":{"kind":"Name","value":"png"}},{"kind":"Field","name":{"kind":"Name","value":"artCrop"}},{"kind":"Field","name":{"kind":"Name","value":"borderCrop"}}]}},{"kind":"Field","name":{"kind":"Name","value":"manaCost"}},{"kind":"Field","name":{"kind":"Name","value":"cmc"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"oracleText"}},{"kind":"Field","name":{"kind":"Name","value":"power"}},{"kind":"Field","name":{"kind":"Name","value":"toughness"}},{"kind":"Field","name":{"kind":"Name","value":"loyalty"}},{"kind":"Field","name":{"kind":"Name","value":"defense"}},{"kind":"Field","name":{"kind":"Name","value":"lifeModifier"}},{"kind":"Field","name":{"kind":"Name","value":"handModifier"}},{"kind":"Field","name":{"kind":"Name","value":"colors"}},{"kind":"Field","name":{"kind":"Name","value":"colorIdentity"}},{"kind":"Field","name":{"kind":"Name","value":"colorIndicator"}},{"kind":"Field","name":{"kind":"Name","value":"keywords"}},{"kind":"Field","name":{"kind":"Name","value":"legalities"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"standard"}},{"kind":"Field","name":{"kind":"Name","value":"future"}},{"kind":"Field","name":{"kind":"Name","value":"historic"}},{"kind":"Field","name":{"kind":"Name","value":"timeless"}},{"kind":"Field","name":{"kind":"Name","value":"gladiator"}},{"kind":"Field","name":{"kind":"Name","value":"pioneer"}},{"kind":"Field","name":{"kind":"Name","value":"explorer"}},{"kind":"Field","name":{"kind":"Name","value":"modern"}},{"kind":"Field","name":{"kind":"Name","value":"legacy"}},{"kind":"Field","name":{"kind":"Name","value":"pauper"}},{"kind":"Field","name":{"kind":"Name","value":"vintage"}},{"kind":"Field","name":{"kind":"Name","value":"penny"}},{"kind":"Field","name":{"kind":"Name","value":"commander"}},{"kind":"Field","name":{"kind":"Name","value":"oathbreaker"}},{"kind":"Field","name":{"kind":"Name","value":"standardBrawl"}},{"kind":"Field","name":{"kind":"Name","value":"brawl"}},{"kind":"Field","name":{"kind":"Name","value":"alchemy"}},{"kind":"Field","name":{"kind":"Name","value":"pauperCommander"}},{"kind":"Field","name":{"kind":"Name","value":"duel"}},{"kind":"Field","name":{"kind":"Name","value":"oldschool"}},{"kind":"Field","name":{"kind":"Name","value":"premodern"}},{"kind":"Field","name":{"kind":"Name","value":"predh"}}]}},{"kind":"Field","name":{"kind":"Name","value":"games"}},{"kind":"Field","name":{"kind":"Name","value":"reserved"}},{"kind":"Field","name":{"kind":"Name","value":"foil"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoil"}},{"kind":"Field","name":{"kind":"Name","value":"finishes"}},{"kind":"Field","name":{"kind":"Name","value":"oversized"}},{"kind":"Field","name":{"kind":"Name","value":"promo"}},{"kind":"Field","name":{"kind":"Name","value":"promoTypes"}},{"kind":"Field","name":{"kind":"Name","value":"reprint"}},{"kind":"Field","name":{"kind":"Name","value":"variation"}},{"kind":"Field","name":{"kind":"Name","value":"setId"}},{"kind":"Field","name":{"kind":"Name","value":"setCode"}},{"kind":"Field","name":{"kind":"Name","value":"setName"}},{"kind":"Field","name":{"kind":"Name","value":"setType"}},{"kind":"Field","name":{"kind":"Name","value":"setUri"}},{"kind":"Field","name":{"kind":"Name","value":"setSearchUri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallSetUri"}},{"kind":"Field","name":{"kind":"Name","value":"rulingsUri"}},{"kind":"Field","name":{"kind":"Name","value":"printsSearchUri"}},{"kind":"Field","name":{"kind":"Name","value":"collectorNumber"}},{"kind":"Field","name":{"kind":"Name","value":"digital"}},{"kind":"Field","name":{"kind":"Name","value":"rarity"}},{"kind":"Field","name":{"kind":"Name","value":"flavorText"}},{"kind":"Field","name":{"kind":"Name","value":"cardBackId"}},{"kind":"Field","name":{"kind":"Name","value":"artist"}},{"kind":"Field","name":{"kind":"Name","value":"artistIds"}},{"kind":"Field","name":{"kind":"Name","value":"illustrationId"}},{"kind":"Field","name":{"kind":"Name","value":"borderColor"}},{"kind":"Field","name":{"kind":"Name","value":"frame"}},{"kind":"Field","name":{"kind":"Name","value":"frameEffects"}},{"kind":"Field","name":{"kind":"Name","value":"securityStamp"}},{"kind":"Field","name":{"kind":"Name","value":"fullArt"}},{"kind":"Field","name":{"kind":"Name","value":"textless"}},{"kind":"Field","name":{"kind":"Name","value":"booster"}},{"kind":"Field","name":{"kind":"Name","value":"storySpotlight"}},{"kind":"Field","name":{"kind":"Name","value":"edhRecRank"}},{"kind":"Field","name":{"kind":"Name","value":"pennyRank"}},{"kind":"Field","name":{"kind":"Name","value":"prices"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"usd"}},{"kind":"Field","name":{"kind":"Name","value":"usdFoil"}},{"kind":"Field","name":{"kind":"Name","value":"usdEtched"}},{"kind":"Field","name":{"kind":"Name","value":"eur"}},{"kind":"Field","name":{"kind":"Name","value":"eurFoil"}},{"kind":"Field","name":{"kind":"Name","value":"tix"}}]}},{"kind":"Field","name":{"kind":"Name","value":"relatedUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"gatherer"}},{"kind":"Field","name":{"kind":"Name","value":"tcgplayerInfiniteArticles"}},{"kind":"Field","name":{"kind":"Name","value":"tcgplayerInfiniteDecks"}},{"kind":"Field","name":{"kind":"Name","value":"edhrec"}}]}},{"kind":"Field","name":{"kind":"Name","value":"purchaseUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"tcgplayer"}},{"kind":"Field","name":{"kind":"Name","value":"cardmarket"}},{"kind":"Field","name":{"kind":"Name","value":"cardhoarder"}}]}},{"kind":"Field","name":{"kind":"Name","value":"cardFaces"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"manaCost"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"oracleText"}},{"kind":"Field","name":{"kind":"Name","value":"colors"}},{"kind":"Field","name":{"kind":"Name","value":"colorIndicator"}},{"kind":"Field","name":{"kind":"Name","value":"power"}},{"kind":"Field","name":{"kind":"Name","value":"toughness"}},{"kind":"Field","name":{"kind":"Name","value":"loyalty"}},{"kind":"Field","name":{"kind":"Name","value":"defense"}},{"kind":"Field","name":{"kind":"Name","value":"artist"}},{"kind":"Field","name":{"kind":"Name","value":"artistId"}},{"kind":"Field","name":{"kind":"Name","value":"illustrationId"}},{"kind":"Field","name":{"kind":"Name","value":"imageUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"small"}},{"kind":"Field","name":{"kind":"Name","value":"normal"}},{"kind":"Field","name":{"kind":"Name","value":"large"}},{"kind":"Field","name":{"kind":"Name","value":"png"}},{"kind":"Field","name":{"kind":"Name","value":"artCrop"}},{"kind":"Field","name":{"kind":"Name","value":"borderCrop"}}]}},{"kind":"Field","name":{"kind":"Name","value":"flavorText"}},{"kind":"Field","name":{"kind":"Name","value":"printedName"}},{"kind":"Field","name":{"kind":"Name","value":"printedTypeLine"}},{"kind":"Field","name":{"kind":"Name","value":"printedText"}},{"kind":"Field","name":{"kind":"Name","value":"watermark"}},{"kind":"Field","name":{"kind":"Name","value":"layout"}},{"kind":"Field","name":{"kind":"Name","value":"cmc"}}]}},{"kind":"Field","name":{"kind":"Name","value":"allParts"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"component"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}}]}},{"kind":"Field","name":{"kind":"Name","value":"printedName"}},{"kind":"Field","name":{"kind":"Name","value":"printedTypeLine"}},{"kind":"Field","name":{"kind":"Name","value":"printedText"}},{"kind":"Field","name":{"kind":"Name","value":"watermark"}},{"kind":"Field","name":{"kind":"Name","value":"contentWarning"}},{"kind":"Field","name":{"kind":"Name","value":"preview"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"source"}},{"kind":"Field","name":{"kind":"Name","value":"sourceUri"}},{"kind":"Field","name":{"kind":"Name","value":"previewedAt"}}]}},{"kind":"Field","name":{"kind":"Name","value":"producedMana"}},{"kind":"Field","name":{"kind":"Name","value":"attractions"}},{"kind":"Field","name":{"kind":"Name","value":"setGroupId"}},{"kind":"Field","name":{"kind":"Name","value":"userCollection"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"finish"}},{"kind":"Field","name":{"kind":"Name","value":"special"}},{"kind":"Field","name":{"kind":"Name","value":"count"}}]}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}}]}}]}}]}}]}}]} as unknown as DocumentNode<GetCardsBySetCodeQuery, GetCardsBySetCodeQueryVariables>;
export const GetCardsBatchDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetCardsBatch"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"ids"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"CardIdsArgEntityInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cardsById"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"ids"},"value":{"kind":"Variable","name":{"kind":"Name","value":"ids"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"CardsSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"oracleId"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"lang"}},{"kind":"Field","name":{"kind":"Name","value":"releasedAt"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallUri"}},{"kind":"Field","name":{"kind":"Name","value":"layout"}},{"kind":"Field","name":{"kind":"Name","value":"highResImage"}},{"kind":"Field","name":{"kind":"Name","value":"imageStatus"}},{"kind":"Field","name":{"kind":"Name","value":"imageUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"small"}},{"kind":"Field","name":{"kind":"Name","value":"normal"}},{"kind":"Field","name":{"kind":"Name","value":"large"}},{"kind":"Field","name":{"kind":"Name","value":"png"}},{"kind":"Field","name":{"kind":"Name","value":"artCrop"}},{"kind":"Field","name":{"kind":"Name","value":"borderCrop"}}]}},{"kind":"Field","name":{"kind":"Name","value":"manaCost"}},{"kind":"Field","name":{"kind":"Name","value":"cmc"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"oracleText"}},{"kind":"Field","name":{"kind":"Name","value":"power"}},{"kind":"Field","name":{"kind":"Name","value":"toughness"}},{"kind":"Field","name":{"kind":"Name","value":"colors"}},{"kind":"Field","name":{"kind":"Name","value":"colorIdentity"}},{"kind":"Field","name":{"kind":"Name","value":"keywords"}},{"kind":"Field","name":{"kind":"Name","value":"reserved"}},{"kind":"Field","name":{"kind":"Name","value":"setId"}},{"kind":"Field","name":{"kind":"Name","value":"setCode"}},{"kind":"Field","name":{"kind":"Name","value":"setName"}},{"kind":"Field","name":{"kind":"Name","value":"setType"}},{"kind":"Field","name":{"kind":"Name","value":"collectorNumber"}},{"kind":"Field","name":{"kind":"Name","value":"digital"}},{"kind":"Field","name":{"kind":"Name","value":"rarity"}},{"kind":"Field","name":{"kind":"Name","value":"artist"}},{"kind":"Field","name":{"kind":"Name","value":"artistIds"}},{"kind":"Field","name":{"kind":"Name","value":"illustrationId"}},{"kind":"Field","name":{"kind":"Name","value":"borderColor"}},{"kind":"Field","name":{"kind":"Name","value":"frame"}},{"kind":"Field","name":{"kind":"Name","value":"prices"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"usd"}},{"kind":"Field","name":{"kind":"Name","value":"usdFoil"}},{"kind":"Field","name":{"kind":"Name","value":"usdEtched"}},{"kind":"Field","name":{"kind":"Name","value":"eur"}},{"kind":"Field","name":{"kind":"Name","value":"eurFoil"}},{"kind":"Field","name":{"kind":"Name","value":"tix"}}]}},{"kind":"Field","name":{"kind":"Name","value":"relatedUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"gatherer"}},{"kind":"Field","name":{"kind":"Name","value":"tcgplayerInfiniteArticles"}},{"kind":"Field","name":{"kind":"Name","value":"tcgplayerInfiniteDecks"}},{"kind":"Field","name":{"kind":"Name","value":"edhrec"}}]}},{"kind":"Field","name":{"kind":"Name","value":"purchaseUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"tcgplayer"}},{"kind":"Field","name":{"kind":"Name","value":"cardmarket"}},{"kind":"Field","name":{"kind":"Name","value":"cardhoarder"}}]}},{"kind":"Field","name":{"kind":"Name","value":"userCollection"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"finish"}},{"kind":"Field","name":{"kind":"Name","value":"special"}},{"kind":"Field","name":{"kind":"Name","value":"count"}}]}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}}]}}]}}]}}]}}]} as unknown as DocumentNode<GetCardsBatchQuery, GetCardsBatchQueryVariables>;
export const CardsByArtistNameDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"CardsByArtistName"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"artistName"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"ArtistNameArgEntityInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cardsByArtistName"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"artistName"},"value":{"kind":"Variable","name":{"kind":"Name","value":"artistName"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"CardsByArtistSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"oracleId"}},{"kind":"Field","name":{"kind":"Name","value":"multiverseIds"}},{"kind":"Field","name":{"kind":"Name","value":"tcgPlayerId"}},{"kind":"Field","name":{"kind":"Name","value":"cardMarketId"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"lang"}},{"kind":"Field","name":{"kind":"Name","value":"releasedAt"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallUri"}},{"kind":"Field","name":{"kind":"Name","value":"layout"}},{"kind":"Field","name":{"kind":"Name","value":"highResImage"}},{"kind":"Field","name":{"kind":"Name","value":"imageStatus"}},{"kind":"Field","name":{"kind":"Name","value":"imageUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"small"}},{"kind":"Field","name":{"kind":"Name","value":"normal"}},{"kind":"Field","name":{"kind":"Name","value":"large"}},{"kind":"Field","name":{"kind":"Name","value":"png"}},{"kind":"Field","name":{"kind":"Name","value":"artCrop"}},{"kind":"Field","name":{"kind":"Name","value":"borderCrop"}}]}},{"kind":"Field","name":{"kind":"Name","value":"manaCost"}},{"kind":"Field","name":{"kind":"Name","value":"cmc"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"oracleText"}},{"kind":"Field","name":{"kind":"Name","value":"power"}},{"kind":"Field","name":{"kind":"Name","value":"toughness"}},{"kind":"Field","name":{"kind":"Name","value":"loyalty"}},{"kind":"Field","name":{"kind":"Name","value":"defense"}},{"kind":"Field","name":{"kind":"Name","value":"lifeModifier"}},{"kind":"Field","name":{"kind":"Name","value":"handModifier"}},{"kind":"Field","name":{"kind":"Name","value":"colors"}},{"kind":"Field","name":{"kind":"Name","value":"colorIdentity"}},{"kind":"Field","name":{"kind":"Name","value":"colorIndicator"}},{"kind":"Field","name":{"kind":"Name","value":"keywords"}},{"kind":"Field","name":{"kind":"Name","value":"legalities"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"standard"}},{"kind":"Field","name":{"kind":"Name","value":"future"}},{"kind":"Field","name":{"kind":"Name","value":"historic"}},{"kind":"Field","name":{"kind":"Name","value":"timeless"}},{"kind":"Field","name":{"kind":"Name","value":"gladiator"}},{"kind":"Field","name":{"kind":"Name","value":"pioneer"}},{"kind":"Field","name":{"kind":"Name","value":"explorer"}},{"kind":"Field","name":{"kind":"Name","value":"modern"}},{"kind":"Field","name":{"kind":"Name","value":"legacy"}},{"kind":"Field","name":{"kind":"Name","value":"pauper"}},{"kind":"Field","name":{"kind":"Name","value":"vintage"}},{"kind":"Field","name":{"kind":"Name","value":"penny"}},{"kind":"Field","name":{"kind":"Name","value":"commander"}},{"kind":"Field","name":{"kind":"Name","value":"oathbreaker"}},{"kind":"Field","name":{"kind":"Name","value":"standardBrawl"}},{"kind":"Field","name":{"kind":"Name","value":"brawl"}},{"kind":"Field","name":{"kind":"Name","value":"alchemy"}},{"kind":"Field","name":{"kind":"Name","value":"pauperCommander"}},{"kind":"Field","name":{"kind":"Name","value":"duel"}},{"kind":"Field","name":{"kind":"Name","value":"oldschool"}},{"kind":"Field","name":{"kind":"Name","value":"premodern"}},{"kind":"Field","name":{"kind":"Name","value":"predh"}}]}},{"kind":"Field","name":{"kind":"Name","value":"games"}},{"kind":"Field","name":{"kind":"Name","value":"reserved"}},{"kind":"Field","name":{"kind":"Name","value":"foil"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoil"}},{"kind":"Field","name":{"kind":"Name","value":"finishes"}},{"kind":"Field","name":{"kind":"Name","value":"oversized"}},{"kind":"Field","name":{"kind":"Name","value":"promo"}},{"kind":"Field","name":{"kind":"Name","value":"promoTypes"}},{"kind":"Field","name":{"kind":"Name","value":"reprint"}},{"kind":"Field","name":{"kind":"Name","value":"variation"}},{"kind":"Field","name":{"kind":"Name","value":"setId"}},{"kind":"Field","name":{"kind":"Name","value":"setCode"}},{"kind":"Field","name":{"kind":"Name","value":"setName"}},{"kind":"Field","name":{"kind":"Name","value":"setType"}},{"kind":"Field","name":{"kind":"Name","value":"setUri"}},{"kind":"Field","name":{"kind":"Name","value":"setSearchUri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallSetUri"}},{"kind":"Field","name":{"kind":"Name","value":"rulingsUri"}},{"kind":"Field","name":{"kind":"Name","value":"printsSearchUri"}},{"kind":"Field","name":{"kind":"Name","value":"collectorNumber"}},{"kind":"Field","name":{"kind":"Name","value":"digital"}},{"kind":"Field","name":{"kind":"Name","value":"rarity"}},{"kind":"Field","name":{"kind":"Name","value":"flavorText"}},{"kind":"Field","name":{"kind":"Name","value":"cardBackId"}},{"kind":"Field","name":{"kind":"Name","value":"artist"}},{"kind":"Field","name":{"kind":"Name","value":"artistIds"}},{"kind":"Field","name":{"kind":"Name","value":"illustrationId"}},{"kind":"Field","name":{"kind":"Name","value":"borderColor"}},{"kind":"Field","name":{"kind":"Name","value":"frame"}},{"kind":"Field","name":{"kind":"Name","value":"frameEffects"}},{"kind":"Field","name":{"kind":"Name","value":"securityStamp"}},{"kind":"Field","name":{"kind":"Name","value":"fullArt"}},{"kind":"Field","name":{"kind":"Name","value":"textless"}},{"kind":"Field","name":{"kind":"Name","value":"booster"}},{"kind":"Field","name":{"kind":"Name","value":"storySpotlight"}},{"kind":"Field","name":{"kind":"Name","value":"edhRecRank"}},{"kind":"Field","name":{"kind":"Name","value":"pennyRank"}},{"kind":"Field","name":{"kind":"Name","value":"prices"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"usd"}},{"kind":"Field","name":{"kind":"Name","value":"usdFoil"}},{"kind":"Field","name":{"kind":"Name","value":"usdEtched"}},{"kind":"Field","name":{"kind":"Name","value":"eur"}},{"kind":"Field","name":{"kind":"Name","value":"eurFoil"}},{"kind":"Field","name":{"kind":"Name","value":"tix"}}]}},{"kind":"Field","name":{"kind":"Name","value":"relatedUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"gatherer"}},{"kind":"Field","name":{"kind":"Name","value":"tcgplayerInfiniteArticles"}},{"kind":"Field","name":{"kind":"Name","value":"tcgplayerInfiniteDecks"}},{"kind":"Field","name":{"kind":"Name","value":"edhrec"}}]}},{"kind":"Field","name":{"kind":"Name","value":"purchaseUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"tcgplayer"}},{"kind":"Field","name":{"kind":"Name","value":"cardmarket"}},{"kind":"Field","name":{"kind":"Name","value":"cardhoarder"}}]}},{"kind":"Field","name":{"kind":"Name","value":"cardFaces"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"manaCost"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"oracleText"}},{"kind":"Field","name":{"kind":"Name","value":"colors"}},{"kind":"Field","name":{"kind":"Name","value":"colorIndicator"}},{"kind":"Field","name":{"kind":"Name","value":"power"}},{"kind":"Field","name":{"kind":"Name","value":"toughness"}},{"kind":"Field","name":{"kind":"Name","value":"loyalty"}},{"kind":"Field","name":{"kind":"Name","value":"defense"}},{"kind":"Field","name":{"kind":"Name","value":"artist"}},{"kind":"Field","name":{"kind":"Name","value":"artistId"}},{"kind":"Field","name":{"kind":"Name","value":"illustrationId"}},{"kind":"Field","name":{"kind":"Name","value":"imageUris"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"small"}},{"kind":"Field","name":{"kind":"Name","value":"normal"}},{"kind":"Field","name":{"kind":"Name","value":"large"}},{"kind":"Field","name":{"kind":"Name","value":"png"}},{"kind":"Field","name":{"kind":"Name","value":"artCrop"}},{"kind":"Field","name":{"kind":"Name","value":"borderCrop"}}]}},{"kind":"Field","name":{"kind":"Name","value":"flavorText"}},{"kind":"Field","name":{"kind":"Name","value":"printedName"}},{"kind":"Field","name":{"kind":"Name","value":"printedTypeLine"}},{"kind":"Field","name":{"kind":"Name","value":"printedText"}},{"kind":"Field","name":{"kind":"Name","value":"watermark"}},{"kind":"Field","name":{"kind":"Name","value":"layout"}},{"kind":"Field","name":{"kind":"Name","value":"cmc"}}]}},{"kind":"Field","name":{"kind":"Name","value":"allParts"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"component"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"typeLine"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}}]}},{"kind":"Field","name":{"kind":"Name","value":"printedName"}},{"kind":"Field","name":{"kind":"Name","value":"printedTypeLine"}},{"kind":"Field","name":{"kind":"Name","value":"printedText"}},{"kind":"Field","name":{"kind":"Name","value":"watermark"}},{"kind":"Field","name":{"kind":"Name","value":"contentWarning"}},{"kind":"Field","name":{"kind":"Name","value":"preview"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"source"}},{"kind":"Field","name":{"kind":"Name","value":"sourceUri"}},{"kind":"Field","name":{"kind":"Name","value":"previewedAt"}}]}},{"kind":"Field","name":{"kind":"Name","value":"producedMana"}},{"kind":"Field","name":{"kind":"Name","value":"attractions"}},{"kind":"Field","name":{"kind":"Name","value":"setGroupId"}},{"kind":"Field","name":{"kind":"Name","value":"userCollection"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"finish"}},{"kind":"Field","name":{"kind":"Name","value":"special"}},{"kind":"Field","name":{"kind":"Name","value":"count"}}]}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}}]}}]}}]}}]}}]} as unknown as DocumentNode<CardsByArtistNameQuery, CardsByArtistNameQueryVariables>;
export const GetAllSetsDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetAllSets"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"args"}},"type":{"kind":"NamedType","name":{"kind":"Name","value":"AllSetsArgEntityInput"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"allSets"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"args"},"value":{"kind":"Variable","name":{"kind":"Name","value":"args"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"SetsSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"code"}},{"kind":"Field","name":{"kind":"Name","value":"tcgPlayerId"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallUri"}},{"kind":"Field","name":{"kind":"Name","value":"searchUri"}},{"kind":"Field","name":{"kind":"Name","value":"releasedAt"}},{"kind":"Field","name":{"kind":"Name","value":"setType"}},{"kind":"Field","name":{"kind":"Name","value":"cardCount"}},{"kind":"Field","name":{"kind":"Name","value":"printedSize"}},{"kind":"Field","name":{"kind":"Name","value":"digital"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoilOnly"}},{"kind":"Field","name":{"kind":"Name","value":"foilOnly"}},{"kind":"Field","name":{"kind":"Name","value":"block"}},{"kind":"Field","name":{"kind":"Name","value":"iconSvgUri"}},{"kind":"Field","name":{"kind":"Name","value":"groupings"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"cardCounts"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"nonFoil"}},{"kind":"Field","name":{"kind":"Name","value":"foil"}},{"kind":"Field","name":{"kind":"Name","value":"etched"}}]}}]}},{"kind":"Field","name":{"kind":"Name","value":"userCollection"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"totalCards"}},{"kind":"Field","name":{"kind":"Name","value":"uniqueCards"}},{"kind":"Field","name":{"kind":"Name","value":"collecting"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"setGroupId"}},{"kind":"Field","name":{"kind":"Name","value":"collecting"}},{"kind":"Field","name":{"kind":"Name","value":"counts"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"total"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoil"}},{"kind":"Field","name":{"kind":"Name","value":"foil"}},{"kind":"Field","name":{"kind":"Name","value":"etched"}}]}},{"kind":"Field","name":{"kind":"Name","value":"collectingFinishes"}}]}},{"kind":"Field","name":{"kind":"Name","value":"groups"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"setGroupId"}},{"kind":"Field","name":{"kind":"Name","value":"group"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"nonFoil"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cards"}}]}},{"kind":"Field","name":{"kind":"Name","value":"foil"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cards"}}]}},{"kind":"Field","name":{"kind":"Name","value":"etched"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cards"}}]}}]}}]}}]}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}},{"kind":"Field","name":{"kind":"Name","value":"statusCode"}}]}}]}}]}}]}}]} as unknown as DocumentNode<GetAllSetsQuery, GetAllSetsQueryVariables>;
export const GetMultipleSetsByCodeDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetMultipleSetsByCode"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"codes"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"SetCodesArgEntityInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"setsByCode"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"codes"},"value":{"kind":"Variable","name":{"kind":"Name","value":"codes"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"SetsSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"code"}},{"kind":"Field","name":{"kind":"Name","value":"tcgPlayerId"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallUri"}},{"kind":"Field","name":{"kind":"Name","value":"searchUri"}},{"kind":"Field","name":{"kind":"Name","value":"releasedAt"}},{"kind":"Field","name":{"kind":"Name","value":"setType"}},{"kind":"Field","name":{"kind":"Name","value":"cardCount"}},{"kind":"Field","name":{"kind":"Name","value":"printedSize"}},{"kind":"Field","name":{"kind":"Name","value":"digital"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoilOnly"}},{"kind":"Field","name":{"kind":"Name","value":"foilOnly"}},{"kind":"Field","name":{"kind":"Name","value":"block"}},{"kind":"Field","name":{"kind":"Name","value":"iconSvgUri"}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}},{"kind":"Field","name":{"kind":"Name","value":"statusCode"}}]}}]}}]}}]}}]} as unknown as DocumentNode<GetMultipleSetsByCodeQuery, GetMultipleSetsByCodeQueryVariables>;
export const GetSetByCodeWithGroupingsDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetSetByCodeWithGroupings"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"codes"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"SetCodesArgEntityInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"setsByCode"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"codes"},"value":{"kind":"Variable","name":{"kind":"Name","value":"codes"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"SetsSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"code"}},{"kind":"Field","name":{"kind":"Name","value":"tcgPlayerId"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"uri"}},{"kind":"Field","name":{"kind":"Name","value":"scryfallUri"}},{"kind":"Field","name":{"kind":"Name","value":"searchUri"}},{"kind":"Field","name":{"kind":"Name","value":"setType"}},{"kind":"Field","name":{"kind":"Name","value":"cardCount"}},{"kind":"Field","name":{"kind":"Name","value":"printedSize"}},{"kind":"Field","name":{"kind":"Name","value":"releasedAt"}},{"kind":"Field","name":{"kind":"Name","value":"digital"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoilOnly"}},{"kind":"Field","name":{"kind":"Name","value":"foilOnly"}},{"kind":"Field","name":{"kind":"Name","value":"block"}},{"kind":"Field","name":{"kind":"Name","value":"iconSvgUri"}},{"kind":"Field","name":{"kind":"Name","value":"groupings"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"displayName"}},{"kind":"Field","name":{"kind":"Name","value":"order"}},{"kind":"Field","name":{"kind":"Name","value":"cardCounts"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"total"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoil"}},{"kind":"Field","name":{"kind":"Name","value":"foil"}},{"kind":"Field","name":{"kind":"Name","value":"etched"}}]}},{"kind":"Field","name":{"kind":"Name","value":"rawQuery"}},{"kind":"Field","name":{"kind":"Name","value":"filters"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"collectorNumberRange"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"min"}},{"kind":"Field","name":{"kind":"Name","value":"max"}},{"kind":"Field","name":{"kind":"Name","value":"orConditions"}}]}},{"kind":"Field","name":{"kind":"Name","value":"properties"}}]}}]}},{"kind":"Field","name":{"kind":"Name","value":"userCollection"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"totalCards"}},{"kind":"Field","name":{"kind":"Name","value":"uniqueCards"}},{"kind":"Field","name":{"kind":"Name","value":"collecting"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"setGroupId"}},{"kind":"Field","name":{"kind":"Name","value":"collecting"}},{"kind":"Field","name":{"kind":"Name","value":"counts"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"total"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoil"}},{"kind":"Field","name":{"kind":"Name","value":"foil"}},{"kind":"Field","name":{"kind":"Name","value":"etched"}}]}},{"kind":"Field","name":{"kind":"Name","value":"collectingFinishes"}}]}},{"kind":"Field","name":{"kind":"Name","value":"groups"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"setGroupId"}},{"kind":"Field","name":{"kind":"Name","value":"group"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"nonFoil"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cards"}}]}},{"kind":"Field","name":{"kind":"Name","value":"foil"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cards"}}]}},{"kind":"Field","name":{"kind":"Name","value":"etched"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cards"}}]}}]}}]}}]}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}},{"kind":"Field","name":{"kind":"Name","value":"statusCode"}}]}}]}}]}}]}}]} as unknown as DocumentNode<GetSetByCodeWithGroupingsQuery, GetSetByCodeWithGroupingsQueryVariables>;
export const AddSetGroupToUserSetCardDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"mutation","name":{"kind":"Name","value":"AddSetGroupToUserSetCard"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"input"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"AddSetGroupToUserSetCardInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"addSetGroupToUserSetCard"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"input"},"value":{"kind":"Variable","name":{"kind":"Name","value":"input"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"__typename"}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"UserSetCardSuccessResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"data"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"userId"}},{"kind":"Field","name":{"kind":"Name","value":"setId"}},{"kind":"Field","name":{"kind":"Name","value":"totalCards"}},{"kind":"Field","name":{"kind":"Name","value":"uniqueCards"}},{"kind":"Field","name":{"kind":"Name","value":"collecting"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"setGroupId"}},{"kind":"Field","name":{"kind":"Name","value":"collecting"}},{"kind":"Field","name":{"kind":"Name","value":"counts"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"total"}},{"kind":"Field","name":{"kind":"Name","value":"nonFoil"}},{"kind":"Field","name":{"kind":"Name","value":"foil"}},{"kind":"Field","name":{"kind":"Name","value":"etched"}}]}},{"kind":"Field","name":{"kind":"Name","value":"collectingFinishes"}}]}}]}},{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}},{"kind":"Field","name":{"kind":"Name","value":"statusCode"}}]}}]}},{"kind":"InlineFragment","typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"FailureResponse"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"status"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"message"}},{"kind":"Field","name":{"kind":"Name","value":"statusCode"}}]}}]}}]}}]}}]} as unknown as DocumentNode<AddSetGroupToUserSetCardMutation, AddSetGroupToUserSetCardMutationVariables>;