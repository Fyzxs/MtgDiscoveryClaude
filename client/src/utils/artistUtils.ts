import type { Card } from '../types/card';

/**
 * Parse artist string into individual artist names.
 * Artists can be separated by "&" or "and".
 *
 * @param artistString - The artist string to parse (e.g., "John Doe & Jane Smith")
 * @returns Array of individual artist names
 */
export const parseArtistNames = (artistString: string): string[] => {
  return artistString.split(/\s+(?:&|and)\s+/i);
};

/**
 * Convert a string to Pascal Case (capitalize first letter of each word).
 *
 * @param str - The string to convert
 * @returns The Pascal Case version of the string
 */
export const toPascalCase = (str: string): string => {
  return str
    .split(/[\s-]+/)
    .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
    .join(' ');
};

/**
 * Get artist name information from a collection of cards.
 * Returns the primary name (most common) and any alternate names found.
 *
 * @param cards - Array of cards to analyze
 * @param searchArtistName - The artist name being searched for
 * @returns Object with primaryName and alternateNames array
 */
export const getArtistNameInfo = (
  cards: Card[],
  searchArtistName: string
): { primaryName: string; alternateNames: string[] } => {
  const nameCounts = new Map<string, number>();
  const lowerSearchName = searchArtistName.toLowerCase();

  // Count occurrences of each artist name variation
  cards.forEach(card => {
    if (card.artist) {
      const artistNames = parseArtistNames(card.artist);
      artistNames.forEach(name => {
        if (name.toLowerCase().includes(lowerSearchName) || lowerSearchName.includes(name.toLowerCase())) {
          const count = nameCounts.get(name) || 0;
          nameCounts.set(name, count + 1);
        }
      });
    }
  });

  // Sort by count to find primary name
  const sortedNames = Array.from(nameCounts.entries())
    .sort((a, b) => b[1] - a[1])
    .map(([name]) => name);

  if (sortedNames.length === 0) {
    return { primaryName: toPascalCase(searchArtistName), alternateNames: [] };
  }

  return {
    primaryName: sortedNames[0],
    alternateNames: sortedNames.slice(1)
  };
};
