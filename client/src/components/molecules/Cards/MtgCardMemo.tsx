import { useMemo } from 'react';
import type { Card, CardContext } from '../../../types/card';

interface CardWithCollection extends Card {
  userCollection?: unknown;
}

interface MtgCardMemoProps {
  card: Card;
  context?: CardContext;
  index: number;
  groupId: string;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  className?: string;
}

export const useMtgCardMemo = ({ card }: { card: Card }) => {
  // Memoize aria label to avoid recalculating on every render
  const ariaLabel = useMemo(() => {
    return `${card.name} - ${card.rarity} ${card.typeLine || 'card'} from ${card.setName}. Artist: ${card.artist}`;
  }, [card.name, card.rarity, card.typeLine, card.setName, card.artist]);

  return {
    ariaLabel
  };
};

// Custom comparison function for React.memo optimization
export const mtgCardPropsComparison = (
  prevProps: MtgCardMemoProps,
  nextProps: MtgCardMemoProps
): boolean => {
  // Custom comparison to prevent unnecessary re-renders

  // Card ID is the most important - if different, always re-render
  if (prevProps.card.id !== nextProps.card.id) return false;

  // Index or group changes should re-render
  if (prevProps.index !== nextProps.index) return false;
  if (prevProps.groupId !== nextProps.groupId) return false;

  // Context changes that affect display
  if (prevProps.context?.isOnSetPage !== nextProps.context?.isOnSetPage) return false;
  if (prevProps.context?.isOnArtistPage !== nextProps.context?.isOnArtistPage) return false;
  if (prevProps.context?.currentArtist !== nextProps.context?.currentArtist) return false;
  if (prevProps.context?.currentSet !== nextProps.context?.currentSet) return false;
  if (prevProps.context?.showCollectorInfo !== nextProps.context?.showCollectorInfo) return false;

  // Callback function references (usually stable but worth checking)
  if (prevProps.onSetClick !== nextProps.onSetClick) return false;
  if (prevProps.onArtistClick !== nextProps.onArtistClick) return false;

  // Class name changes
  if (prevProps.className !== nextProps.className) return false;

  // Card properties that affect visual display
  if (prevProps.card.imageUris !== nextProps.card.imageUris) return false;
  if (prevProps.card.cardFaces !== nextProps.card.cardFaces) return false;
  if (prevProps.card.rarity !== nextProps.card.rarity) return false;
  if (prevProps.card.finishes !== nextProps.card.finishes) return false;
  if (prevProps.card.promoTypes !== nextProps.card.promoTypes) return false;
  if (prevProps.card.frameEffects !== nextProps.card.frameEffects) return false;
  if (prevProps.card.promo !== nextProps.card.promo) return false;
  if (prevProps.card.name !== nextProps.card.name) return false;
  if (prevProps.card.collectorNumber !== nextProps.card.collectorNumber) return false;
  if (prevProps.card.artist !== nextProps.card.artist) return false;
  if (prevProps.card.setCode !== nextProps.card.setCode) return false;
  if (prevProps.card.setName !== nextProps.card.setName) return false;
  if (prevProps.card.scryfallUri !== nextProps.card.scryfallUri) return false;

  // Price changes (for display)
  if (prevProps.card.prices?.usd !== nextProps.card.prices?.usd) return false;
  if (prevProps.card.purchaseUris?.tcgplayer !== nextProps.card.purchaseUris?.tcgplayer) return false;

  // Collection data changes (critical for collector view)
  if ((prevProps.card as CardWithCollection).userCollection !== (nextProps.card as CardWithCollection).userCollection) return false;

  // If we get here, props are effectively the same
  return true;
};