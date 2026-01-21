import { useEffect, useRef } from 'react';
import type { Card } from '../types/card';

/**
 * Hook to listen for wishlist-updated events and update local card state.
 *
 * When a card's wishlist data changes (via the global wishlist entry system),
 * this hook ensures the local cards array is updated with the new userWishlist data.
 *
 * @param cards - Current cards array
 * @param setCards - State setter for the cards array
 */
export function useWishlistUpdates(
  cards: Card[],
  setCards: (cards: Card[]) => void
): void {
  const cardsRef = useRef<Card[]>(cards);

  // Keep ref in sync with current cards
  useEffect(() => {
    cardsRef.current = cards;
  }, [cards]);

  useEffect(() => {
    const handleWishlistUpdate = (event: Event) => {
      const { cardId, userWishlist } = (event as CustomEvent).detail;

      queueMicrotask(() => {
        const currentCards = cardsRef.current;
        if (!currentCards || currentCards.length === 0) return;

        // Only update if the card exists in our current list
        const cardIndex = currentCards.findIndex(card => card.id === cardId);
        if (cardIndex === -1) return;

        const updatedCards = currentCards.map(card =>
          card.id === cardId ? { ...card, userWishlist } : card
        );

        setCards(updatedCards);
      });
    };

    window.addEventListener('wishlist-updated', handleWishlistUpdate as EventListener);
    return () => window.removeEventListener('wishlist-updated', handleWishlistUpdate as EventListener);
  }, [setCards]);
}
