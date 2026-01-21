export const EXTERNAL_URLS = {
  cardKingdom: 'https://www.cardkingdom.com',
  tcgplayer: 'https://www.tcgplayer.com',
  scryfall: 'https://scryfall.com',
} as const;

export const generateCardKingdomSearchUrl = (cardName: string): string => {
  const params = new URLSearchParams({
    'search': 'mtg_advanced',
    'filter[search]': 'mtg_advanced',
    'filter[tab]': 'mtg_card',
    'filter[name]': cardName
  });

  return `${EXTERNAL_URLS.cardKingdom}/catalog/search?${params.toString()}`;
};

export const generateTCGPlayerSearchUrl = (cardName: string): string => {
  const encodedName = encodeURIComponent(cardName);
  return `${EXTERNAL_URLS.tcgplayer}/search/magic/product?productLineName=magic&q=${encodedName}`;
};

export const generateScryfallSearchUrl = (cardName: string): string => {
  const encodedName = encodeURIComponent(cardName);
  return `${EXTERNAL_URLS.scryfall}/search?q=${encodedName}`;
};
