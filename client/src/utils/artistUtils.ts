import type { Card } from '../types/card';

/**
 * Converts a string to Pascal case (Title Case)
 * @param str The string to convert
 * @returns Pascal cased string
 */
export const toPascalCase = (str: string): string => {
  return str.split(' ').map(word =>
    word.charAt(0).toUpperCase() + word.slice(1).toLowerCase()
  ).join(' ');
};

/**
 * Analyzes cards to find the most common artist name variation and alternates
 * @param cards Array of cards to analyze
 * @param initialName The initial artist name from the URL
 * @returns Object with primary name and alternate names
 */
export const getArtistNameInfo = (cards: Card[], initialName: string) => {
  const artistCounts = new Map<string, number>();
  const allArtistNames = new Set<string>();

  cards.forEach(card => {
    if (card.artist) {
      // Split multiple artists and count each variation
      const artists = card.artist.split(/\s+(?:&|and)\s+/i).map(a => a.trim());
      artists.forEach(artist => {
        allArtistNames.add(artist);
        // Only count artists that match our target (case-insensitive)
        if (artist.toLowerCase() === initialName.toLowerCase()) {
          artistCounts.set(artist, (artistCounts.get(artist) || 0) + 1);
        }
      });
    }
  });

  // Find most common variation of our target artist
  let mostCommonName = initialName;
  let maxCount = 0;

  for (const [name, count] of artistCounts.entries()) {
    if (count > maxCount) {
      maxCount = count;
      mostCommonName = name;
    }
  }

  // Get all variations that match our target (excluding the most common)
  const alternates = Array.from(artistCounts.keys())
    .filter(name => name !== mostCommonName)
    .sort();

  return {
    primaryName: mostCommonName,
    alternateNames: alternates
  };
};