import React from 'react';
import { Box, Typography } from '../../atoms';
import { ExternalLinkIcon } from '../../molecules/shared/ExternalLinkIcon';

interface ArtistPageHeaderProps {
  /** The primary artist name to display */
  displayArtistName: string;

  /** Array of alternate name variations for this artist */
  alternateNames: string[];
}

/**
 * ArtistPageHeader - Displays artist name and alternate variations
 *
 * Shows the primary artist name as the page title and lists any
 * alternate name variations found in the card data.
 */
export const ArtistPageHeader: React.FC<ArtistPageHeaderProps> = ({
  displayArtistName,
  alternateNames
}) => {
  // Build ManaPool artist search URL (case-sensitive, spaces as +)
  const manaPoolArtistUrl = `https://manapool.com/cards?artist=${encodeURIComponent(displayArtistName).replace(/%20/g, '+')}`;

  return (
    <Box sx={{ mb: 4, textAlign: 'center' }}>
      <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1.5 }}>
        <Typography variant="h3" component="h1">
          {displayArtistName} Cards
        </Typography>
        <ExternalLinkIcon
          type="manapool-artist"
          url={manaPoolArtistUrl}
          size="medium"
        />
      </Box>
      {alternateNames.length > 0 && (
        <Typography
          variant="body2"
          color="text.secondary"
          sx={{ mt: 1, fontSize: '0.9rem' }}
        >
          Alternate Names: {alternateNames.join(', ')}
        </Typography>
      )}
    </Box>
  );
};