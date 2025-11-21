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
