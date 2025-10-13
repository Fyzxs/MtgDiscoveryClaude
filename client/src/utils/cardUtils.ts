import type { CardLike } from '../config/cardSortOptions';
import { RARITY_ORDER } from '../config/cardSortOptions';
import { commonFilters } from '../hooks/useFilterState';

export const getUniqueArtists = (cards: CardLike[]): string[] => {
  const artistSet = new Set<string>();
  cards.forEach(card => {
    if (card.artist) {
      // Split multiple artists (e.g., "Artist 1 & Artist 2")
      const artists = card.artist.split(/\s+(?:&|and)\s+/i);
      artists.forEach(artist => artistSet.add(artist.trim()));
    }
  });
  return Array.from(artistSet).sort((a, b) => a.toLowerCase().localeCompare(b.toLowerCase()));
};

export const getUniqueRarities = (cards: CardLike[]): string[] => {
  const raritySet = new Set<string>();
  cards.forEach(card => {
    if (card.rarity) {
      raritySet.add(card.rarity);
    }
  });
  return Array.from(raritySet).sort((a, b) => 
    (RARITY_ORDER[a.toLowerCase()] ?? 99) - (RARITY_ORDER[b.toLowerCase()] ?? 99)
  );
};

export const getUniqueSets = (cards: CardLike[]): { value: string; label: string }[] => {
  const setMap = new Map<string, string>();
  cards.forEach(card => {
    if (card.setCode && card.setName) {
      setMap.set(card.setCode, card.setName);
    }
  });
  return Array.from(setMap.entries())
    .map(([code, name]) => ({ value: code, label: `${name} (${code.toUpperCase()})` }))
    .sort((a, b) => a.label.localeCompare(b.label));
};

/**
 * Extract unique finish types from cards
 */
export const getUniqueFinishes = (cards: CardLike[]): string[] => {
  const finishSet = new Set<string>();
  cards.forEach(card => {
    if (card.userCollection) {
      const collectionArray = Array.isArray(card.userCollection) ? card.userCollection : [card.userCollection];
      collectionArray.forEach(item => {
        if (item.finish) {
          finishSet.add(item.finish);
        }
      });
    }
  });
  return Array.from(finishSet).sort((a, b) => a.toLowerCase().localeCompare(b.toLowerCase()));
};

/**
 * Get total collection count for a card
 */
export const getCardCollectionCount = (card: CardLike): number => {
  if (!card.userCollection) return 0;
  const collectionArray = Array.isArray(card.userCollection) ? card.userCollection : [card.userCollection];
  return collectionArray.reduce((sum, item) => sum + item.count, 0);
};

/**
 * Check if a card has any signed copies
 */
export const hasSignedCopies = (card: CardLike): boolean => {
  if (!card.userCollection) return false;
  const collectionArray = Array.isArray(card.userCollection) ? card.userCollection : [card.userCollection];
  return collectionArray.some(item => item.special === 'signed');
};

/**
 * Get collection count options for filtering
 */
export const getCollectionCountOptions = () => [
  { value: '0', label: 'Not Owned (0)' },
  { value: '1', label: '1 copy' },
  { value: '2', label: '2 copies' },
  { value: '3', label: '3 copies' },
  { value: '4plus', label: '4+ copies' }
];

/**
 * Get signed cards options for filtering
 */
export const getSignedCardsOptions = () => [
  { value: 'signed', label: 'Signed' },
  { value: 'unsigned', label: 'Unsigned' }
];

/**
 * Get format options for filtering (Digital/Paper)
 */
export const getFormatOptions = () => [
  { value: 'paper', label: 'Paper' },
  { value: 'digital', label: 'Digital' }
];

/**
 * Extract unique formats from cards (Digital/Paper)
 * Returns array of format types present in the card collection
 */
export const getUniqueFormats = (cards: CardLike[]): string[] => {
  const hasDigital = cards.some(card => card.digital === true);
  const hasPaper = cards.some(card => card.digital === false || card.digital === undefined || card.digital === null);

  const formats: string[] = [];
  if (hasPaper) formats.push('paper');
  if (hasDigital) formats.push('digital');

  return formats;
};

export const createCardFilterFunctions = <T extends CardLike>() => ({
  rarities: commonFilters.multiSelect<T>('rarity'),
  sets: commonFilters.multiSelect<T>('setCode'),
  artists: (card: T, selectedArtists: string[]) => {
    if (!selectedArtists || selectedArtists.length === 0) return true;
    if (!card.artist) return false;
    const cardArtists = card.artist.split(/\s+(?:&|and)\s+/i).map(a => a.trim());
    const normalizedSelected = selectedArtists.map(a => a.toLowerCase());
    return cardArtists.some(artist => normalizedSelected.includes(artist.toLowerCase()));
  },
  formats: (card: T, selectedFormats: string[]) => {
    // If no formats selected, show all cards
    if (!selectedFormats || selectedFormats.length === 0) return true;

    // If both formats selected, show all cards
    if (selectedFormats.includes('paper') && selectedFormats.includes('digital')) return true;

    // If only digital selected, show only digital cards
    if (selectedFormats.includes('digital') && !selectedFormats.includes('paper')) {
      return card.digital === true;
    }

    // If only paper selected, show only non-digital cards
    if (selectedFormats.includes('paper') && !selectedFormats.includes('digital')) {
      return card.digital === false || card.digital === undefined || card.digital === null;
    }

    return true;
  },
  // Collector-specific filters
  collectionCounts: (card: T, selectedCounts: string[]) => {
    if (!selectedCounts || selectedCounts.length === 0) return true;
    const count = getCardCollectionCount(card);
    return selectedCounts.some(countOption => {
      if (countOption === '0') return count === 0;
      if (countOption === '1') return count === 1;
      if (countOption === '2') return count === 2;
      if (countOption === '3') return count === 3;
      if (countOption === '4plus') return count >= 4;
      return false;
    });
  },
  signedCards: (card: T, selectedOptions: string[]) => {
    if (!selectedOptions || selectedOptions.length === 0) return true;
    // Both signed and unsigned filters only apply to owned cards
    const count = getCardCollectionCount(card);
    if (count === 0) return false; // No cards owned = not included in either filter

    const hasSigned = hasSignedCopies(card);
    return selectedOptions.some(option => {
      if (option === 'signed') return hasSigned;
      if (option === 'unsigned') return !hasSigned;
      return false;
    });
  },
  finishes: (card: T, selectedFinishes: string[]) => {
    if (!selectedFinishes || selectedFinishes.length === 0) return true;
    if (!card.userCollection) return false;
    const collectionArray = Array.isArray(card.userCollection) ? card.userCollection : [card.userCollection];
    return collectionArray.some(item =>
      item.finish && selectedFinishes.includes(item.finish)
    );
  }
});