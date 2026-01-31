import { useState, useEffect, useMemo, useCallback } from 'react';
import type { Card } from '../types/card';

const INITIAL_BATCH_SIZE = 100;
const BATCH_SIZE = 200;
const SCROLL_THRESHOLD = 1000; // Load more when within 1000px of bottom

export function useProgressiveCardLoading(allCards: Card[] | undefined) {
  const [loadedCount, setLoadedCount] = useState(INITIAL_BATCH_SIZE);
  const [isLoadingMore, setIsLoadingMore] = useState(false);
  
  const visibleCards = useMemo(() => {
    if (!allCards) return [];
    return allCards.slice(0, Math.min(loadedCount, allCards.length));
  }, [allCards, loadedCount]);
  
  const hasMore = useMemo(() => {
    if (!allCards) return false;
    return loadedCount < allCards.length;
  }, [allCards, loadedCount]);
  
  const loadMore = useCallback(() => {
    if (!hasMore || isLoadingMore) return;
    
    setIsLoadingMore(true);
    
    // Use requestAnimationFrame for smoother loading experience
    requestAnimationFrame(() => {
      setTimeout(() => {
        setLoadedCount(prev => Math.min(prev + BATCH_SIZE, allCards?.length || 0));
        setIsLoadingMore(false);
      }, 50); // Small delay to prevent overwhelming the UI
    });
  }, [hasMore, isLoadingMore, allCards?.length]);
  
  // Auto-load more when scrolling near bottom
  const handleScroll = useCallback(() => {
    if (!hasMore || isLoadingMore) return;
    
    const scrollPosition = window.innerHeight + window.scrollY;
    const threshold = document.body.offsetHeight - SCROLL_THRESHOLD;
    
    if (scrollPosition >= threshold) {
      loadMore();
    }
  }, [hasMore, isLoadingMore, loadMore]);
  
  useEffect(() => {
    const throttledScroll = throttle(handleScroll, 100);
    window.addEventListener('scroll', throttledScroll, { passive: true });
    return () => window.removeEventListener('scroll', throttledScroll);
  }, [handleScroll]);
  
  // Reset when cards change
  useEffect(() => {
    setLoadedCount(INITIAL_BATCH_SIZE);
    setIsLoadingMore(false);
  }, [allCards]);
  
  return {
    visibleCards,
    hasMore,
    isLoadingMore,
    loadMore,
    totalCards: allCards?.length || 0,
    loadedCards: visibleCards.length,
    progressPercentage: allCards ? Math.round((visibleCards.length / allCards.length) * 100) : 0
  };
}

// Simple throttle function to limit scroll event frequency
function throttle<T extends (...args: never[]) => unknown>(
  func: T,
  limit: number
): (...args: Parameters<T>) => void {
  let inThrottle: boolean;
  return function(this: unknown, ...args: Parameters<T>) {
    if (!inThrottle) {
      func.apply(this, args);
      inThrottle = true;
      setTimeout(() => inThrottle = false, limit);
    }
  };
}

// Alternative hook for virtual scrolling scenarios where we don't need progressive loading
export function useVirtualCardLoading(allCards: Card[] | undefined) {
  const [isLoading, setIsLoading] = useState(true);
  
  useEffect(() => {
    if (allCards) {
      // Small delay to let UI update
      const timeout = setTimeout(() => {
        setIsLoading(false);
      }, 100);
      
      return () => clearTimeout(timeout);
    }
  }, [allCards]);
  
  return {
    cards: allCards || [],
    isLoading,
    totalCards: allCards?.length || 0
  };
}