import React from 'react';
import { Box, Typography } from '../atoms';

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
  return (
    <Box sx={{ mb: 4, textAlign: 'center' }}>
      <Typography variant="h3" component="h1" gutterBottom>
        {displayArtistName} Cards
      </Typography>
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