import React from 'react';
import { Box } from '@mui/material';
import { ExternalLinkIcon } from '../../atoms/shared/ExternalLinkIcon';

interface CardLinksProps {
  scryfallUrl?: string;
  tcgplayerUrl?: string;
  cardName?: string;
  className?: string;
}

export const CardLinks: React.FC<CardLinksProps> = ({ 
  scryfallUrl,
  tcgplayerUrl,
  cardName,
  className 
}) => {
  const fallbackTcgplayerUrl = cardName 
    ? `https://www.tcgplayer.com/search/magic/product?productLineName=magic&q=${encodeURIComponent(cardName)}`
    : undefined;

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }} className={className}>
      <ExternalLinkIcon 
        type="scryfall" 
        url={scryfallUrl} 
        size="small"
      />
      <ExternalLinkIcon 
        type="tcgplayer" 
        url={tcgplayerUrl || fallbackTcgplayerUrl}
        size="small"
      />
    </Box>
  );
};