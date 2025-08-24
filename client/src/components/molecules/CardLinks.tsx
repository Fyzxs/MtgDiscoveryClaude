import React from 'react';
import { ExternalLinkIcon } from '../atoms/ExternalLinkIcon';
import type { PurchaseUris, RelatedUris } from '../../types/card';

interface CardLinksProps {
  scryfallUri?: string;
  purchaseUris?: PurchaseUris;
  relatedUris?: RelatedUris;
  setCode?: string;
  collectorNumber?: string;
  cardName?: string;
  className?: string;
}

export const CardLinks: React.FC<CardLinksProps> = ({ 
  scryfallUri,
  purchaseUris,
  relatedUris,
  cardName,
  className = ''
}) => {
  // Generate direct links when not provided
  const getCardKingdomUrl = (): string | null => {
    if (purchaseUris?.tcgplayer) return purchaseUris.tcgplayer;
    
    // Fallback to search if we have card name
    if (cardName) {
      const searchQuery = encodeURIComponent(cardName);
      return `https://www.cardkingdom.com/catalog/search?search=header&filter%5Bname%5D=${searchQuery}`;
    }
    return null;
  };

  const getCrystalCommerceUrl = (): string | null => {
    if (purchaseUris?.cardmarket) return purchaseUris.cardmarket;
    
    // Fallback to search
    if (cardName) {
      const searchQuery = encodeURIComponent(cardName);
      return `https://www.cardmarket.com/en/Magic/Products/Search?searchString=${searchQuery}`;
    }
    return null;
  };

  return (
    <div className={`flex items-center gap-2 ${className}`}>
      {scryfallUri && (
        <ExternalLinkIcon type="scryfall" url={scryfallUri} />
      )}
      {relatedUris?.gatherer && (
        <ExternalLinkIcon type="gatherer" url={relatedUris.gatherer} />
      )}
      {relatedUris?.edhrec && (
        <ExternalLinkIcon type="edhrec" url={relatedUris.edhrec} />
      )}
      <ExternalLinkIcon type="tcgplayer" url={getCardKingdomUrl()} />
      <ExternalLinkIcon type="cardmarket" url={getCrystalCommerceUrl()} />
    </div>
  );
};