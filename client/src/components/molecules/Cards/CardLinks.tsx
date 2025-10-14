import React from 'react';
import { Box } from '../../atoms';
import type { SxProps, Theme } from '../../atoms';
import { ExternalLinkIcon } from '../shared/ExternalLinkIcon';

interface CardLinksProps {
  scryfallUrl?: string;
  tcgplayerUrl?: string;
  cardName?: string;
  className?: string;
  sx?: SxProps<Theme>;
}

export const CardLinks: React.FC<CardLinksProps> = ({
  scryfallUrl,
  tcgplayerUrl,
  cardName,
  className,
  sx
}) => {
  const fallbackTcgplayerUrl = cardName
    ? `https://www.tcgplayer.com/search/magic/product?productLineName=magic&q=${encodeURIComponent(cardName)}`
    : undefined;

  // Generate Card Kingdom search URL
  const generateCardKingdomUrl = (name: string): string => {
    const params = new URLSearchParams({
      'search': 'mtg_advanced',
      'filter[search]': 'mtg_advanced',
      'filter[tab]': 'mtg_card',
      'filter[name]': name
    });
    return `https://www.cardkingdom.com/catalog/search?${params.toString()}`;
  };

  const cardKingdomUrl = cardName ? generateCardKingdomUrl(cardName) : undefined;

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5, ...sx }} className={className}>
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
      <ExternalLinkIcon
        type="cardkingdom"
        url={cardKingdomUrl}
        size="small"
      />
    </Box>
  );
};