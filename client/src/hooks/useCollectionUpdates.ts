import { useEffect, useRef } from 'react';
import type { Card } from '../types/card';

/**
 * Hook to listen for collection-updated events and update local card state.
 *
 * When a card's collection data changes (via the global collection entry system),
 * this hook ensures the local cards array is updated with the new userCollection data.
 *
 * @param cards - Current cards array
 * @param setCards - State setter for the cards array
 */
export function useCollectionUpdates(
  cards: Card[],
  setCards: (cards: Card[]) => void
): void {
  const cardsRef = useRef<Card[]>(cards);

  // Keep ref in sync with current cards
  useEffect(() => {
    cardsRef.current = cards;
  }, [cards]);

  useEffect(() => {
    const handleCollectionUpdate = (event: Event) => {
      const { cardId, userCollection } = (event as CustomEvent).detail;

      queueMicrotask(() => {
        const currentCards = cardsRef.current;
        if (!currentCards || currentCards.length === 0) return;

        // Only update if the card exists in our current list
        const cardIndex = currentCards.findIndex(card => card.id === cardId);
        if (cardIndex === -1) return;

        const updatedCards = currentCards.map(card =>
          card.id === cardId ? { ...card, userCollection } : card
        );

        setCards(updatedCards);
      });
    };

    window.addEventListener('collection-updated', handleCollectionUpdate as EventListener);
    return () => window.removeEventListener('collection-updated', handleCollectionUpdate as EventListener);
  }, [setCards]);
}
