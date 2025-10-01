/**
 * Translation key constants for type safety and consistency
 * These keys map to the JSON files in /public/locales/
 */

export const TRANSLATION_KEYS = {
  // Common UI elements
  COMMON: {
    SEARCH: 'search',
    LOADING: 'loading',
    ERROR: 'error',
    SUCCESS: 'success',
    CANCEL: 'cancel',
    SAVE: 'save',
    DELETE: 'delete',
    EDIT: 'edit',
    VIEW: 'view',
    CLEAR_ALL: 'clearAll',
    SELECT_ALL: 'selectAll',
    SHOW_MORE: 'showMore',
    SHOW_LESS: 'showLess',
    BACK_TO_TOP: 'backToTop',
    DOUBLE_CLICK_TO_EXPAND: 'doubleClickToExpand'
  },

  // Navigation
  NAVIGATION: {
    HOME: 'home',
    SETS: 'sets',
    CARDS: 'cards',
    COLLECTION: 'collection',
    ARTISTS: 'artists',
    SEARCH: 'search',
    SETTINGS: 'settings',
    PROFILE: 'profile'
  },

  // Card-related translations
  CARDS: {
    CARD: 'card',
    CARDS: 'cards',
    NAME: 'name',
    TYPE: 'type',
    MANA_COST: 'manaCost',
    COLLECTOR_NUMBER: 'collectorNumber',
    ARTIST: 'artist',
    RARITY: {
      COMMON: 'rarity.common',
      UNCOMMON: 'rarity.uncommon',
      RARE: 'rarity.rare',
      MYTHIC: 'rarity.mythic',
      SPECIAL: 'rarity.special',
      BONUS: 'rarity.bonus'
    },
    FINISH: {
      NONFOIL: 'finish.nonfoil',
      FOIL: 'finish.foil',
      ETCHED: 'finish.etched'
    },
    SPECIAL: {
      NONE: 'special.none',
      ARTIST_PROOF: 'special.artistProof',
      SIGNED: 'special.signed',
      ALTERED: 'special.altered'
    },
    LEGALITY: {
      LEGAL: 'legality.legal',
      NOT_LEGAL: 'legality.notLegal',
      BANNED: 'legality.banned',
      RESTRICTED: 'legality.restricted'
    }
  },

  // Set-related translations
  SETS: {
    SET: 'set',
    SETS: 'sets',
    SET_NAME: 'setName',
    SET_CODE: 'setCode',
    RELEASE_DATE: 'releaseDate',
    CARD_COUNT: 'cardCount',
    SET_TYPE: {
      CORE: 'setType.core',
      EXPANSION: 'setType.expansion',
      MASTERS: 'setType.masters',
      COMMANDER: 'setType.commander',
      DRAFT: 'setType.draft',
      SUPPLEMENTAL: 'setType.supplemental'
    }
  },

  // Collection management
  COLLECTION: {
    COLLECTION: 'collection',
    ADD_TO_COLLECTION: 'addToCollection',
    REMOVE_FROM_COLLECTION: 'removeFromCollection',
    COLLECTION_COUNT: 'collectionCount',
    TOTAL_CARDS: 'totalCards',
    UNIQUE_CARDS: 'uniqueCards',
    PROGRESS: 'progress',
    COMPLETION: 'completion',
    COLLECTED: 'collected',
    NOT_COLLECTED: 'notCollected',
    SHOW_GROUPS: 'showGroups',
    CARD_GROUPS: 'cardGroups',
    GROUP_SELECTION: 'groupSelection'
  },

  // Cultural symbols and emojis
  SYMBOLS: {
    FINISH: {
      NONFOIL: 'finish.nonfoil',
      FOIL: 'finish.foil',
      ETCHED: 'finish.etched'
    },
    SPECIAL: {
      ARTIST_PROOF: 'special.artistProof',
      SIGNED: 'special.signed',
      ALTERED: 'special.altered'
    },
    STATUS: {
      COLLECTED: 'status.collected',
      NOT_COLLECTED: 'status.notCollected',
      PARTIAL: 'status.partial'
    }
  },

  // Error messages
  ERRORS: {
    GENERIC: 'generic',
    NETWORK: 'network',
    NOT_FOUND: 'notFound',
    UNAUTHORIZED: 'unauthorized',
    FORBIDDEN: 'forbidden',
    CARD_NOT_FOUND: 'cardNotFound',
    SET_NOT_FOUND: 'setNotFound',
    COLLECTION_UPDATE_FAILED: 'collectionUpdateFailed',
    AUTHENTICATION_REQUIRED: 'authenticationRequired'
  }
} as const;

// Helper type for extracting nested keys
export type TranslationKey<T> = T extends string
  ? T
  : T extends Record<string, any>
  ? {
      [K in keyof T]: T[K] extends string
        ? T[K]
        : T[K] extends Record<string, any>
        ? `${K & string}.${TranslationKey<T[K]>}`
        : never;
    }[keyof T]
  : never;

// Extract all possible translation keys
export type CommonKey = TranslationKey<typeof TRANSLATION_KEYS.COMMON>;
export type NavigationKey = TranslationKey<typeof TRANSLATION_KEYS.NAVIGATION>;
export type CardsKey = TranslationKey<typeof TRANSLATION_KEYS.CARDS>;
export type SetsKey = TranslationKey<typeof TRANSLATION_KEYS.SETS>;
export type CollectionKey = TranslationKey<typeof TRANSLATION_KEYS.COLLECTION>;
export type SymbolsKey = TranslationKey<typeof TRANSLATION_KEYS.SYMBOLS>;
export type ErrorsKey = TranslationKey<typeof TRANSLATION_KEYS.ERRORS>;