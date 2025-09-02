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
  showDigital: (card: T, show: boolean) => show || !card.digital
});