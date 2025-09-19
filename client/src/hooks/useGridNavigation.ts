import { useState, useEffect, useCallback, useRef } from 'react';

interface UseGridNavigationOptions {
  totalItems: number;
  groupId: string;
  enabled?: boolean;
}

interface UseGridNavigationResult {
  handleKeyDown: (event: React.KeyboardEvent) => void;
}

export const useGridNavigation = ({
  totalItems,
  groupId,
  enabled = true
}: UseGridNavigationOptions): UseGridNavigationResult => {
  const gridColumnsRef = useRef(0);

  // Calculate number of columns based on rendered grid
  const calculateGridColumns = useCallback((container: HTMLElement) => {
    const cards = container.querySelectorAll('[data-mtg-card="true"]');
    if (cards.length < 2) return 1;

    // Find first two cards in different rows
    const firstCard = cards[0] as HTMLElement;
    const firstTop = firstCard.offsetTop;

    let columns = 1;
    for (let i = 1; i < cards.length; i++) {
      const card = cards[i] as HTMLElement;
      if (card.offsetTop > firstTop) {
        columns = i;
        break;
      }
      // If we reach the end without finding a new row, all cards are in one row
      if (i === cards.length - 1) {
        columns = cards.length;
      }
    }

    return columns;
  }, []);

  // Get current selected index from DOM
  const getCurrentSelectedIndex = useCallback((container: HTMLElement): number => {
    // First check if there's a selected card in THIS container
    const selected = container.querySelector('[data-selected="true"]');
    if (selected) {
      const index = selected.getAttribute('data-card-index');
      if (index !== null) {
        return parseInt(index, 10);
      }
    }

    // If no selection in this container, check if there's one globally
    // This handles the case where user clicked in a different group
    const globalSelected = document.querySelector('[data-selected="true"]');
    if (globalSelected) {
      // Check if it's in our container
      if (container.contains(globalSelected)) {
        const index = globalSelected.getAttribute('data-card-index');
        if (index !== null) {
          return parseInt(index, 10);
        }
      } else {
        // Clear it since we're navigating in a different group
        globalSelected.setAttribute('data-selected', 'false');
      }
    }

    return -1;
  }, []);

  // Select card by index using DOM
  const selectCardByIndex = useCallback((container: HTMLElement, index: number) => {
    // Clear ALL current selections GLOBALLY (across all card groups)
    const allSelected = document.querySelectorAll('[data-selected="true"]');
    allSelected.forEach(selected => {
      selected.setAttribute('data-selected', 'false');
    });

    // Clear any focus to remove blue borders
    const focused = document.querySelector(':focus');
    if (focused) {
      (focused as HTMLElement).blur();
    }

    // Set new selection - cards might be nested inside the container
    const cards = container.querySelectorAll('[data-mtg-card="true"]');

    if (index >= 0 && index < cards.length) {
      const card = cards[index] as HTMLElement;
      card.setAttribute('data-selected', 'true');

      // Focus the card for accessibility
      card.focus();

      // Scroll into view
      card.scrollIntoView({
        behavior: 'smooth',
        block: 'nearest',
        inline: 'nearest'
      });
    }
  }, []);


  // Find next/previous card group
  const findAdjacentGroup = useCallback((currentGroup: HTMLElement, direction: 'next' | 'previous'): HTMLElement | null => {
    // Get all card groups that actually contain cards
    const allGroups = Array.from(document.querySelectorAll('[data-card-group]')) as HTMLElement[];
    const visibleGroups = allGroups.filter(group => {
      const cards = group.querySelectorAll('[data-mtg-card="true"]');
      return cards.length > 0;
    });

    const currentIndex = visibleGroups.indexOf(currentGroup);

    if (currentIndex === -1) {
      return null;
    }

    if (direction === 'next' && currentIndex < visibleGroups.length - 1) {
      return visibleGroups[currentIndex + 1];
    } else if (direction === 'previous' && currentIndex > 0) {
      return visibleGroups[currentIndex - 1];
    }

    return null;
  }, []);

  const navigate = useCallback((container: HTMLElement, direction: 'up' | 'down' | 'left' | 'right' | 'tab' | 'shift-tab') => {
    if (!enabled || totalItems === 0) return;

    const selectedIndex = getCurrentSelectedIndex(container);
    const columns = gridColumnsRef.current || calculateGridColumns(container);
    let newIndex = selectedIndex;
    let shouldJumpToNextGroup = false;
    let shouldJumpToPreviousGroup = false;

    // Initialize if no selection
    if (selectedIndex === -1) {
      newIndex = 0;
    } else {
      switch (direction) {
        case 'up':
          newIndex = selectedIndex - columns;
          if (newIndex < 0) {
            // Would go above first row, try to jump to previous group
            shouldJumpToPreviousGroup = true;
          }
          break;
        case 'down':
          newIndex = selectedIndex + columns;
          if (newIndex >= totalItems) {
            // Would go below last row, try to jump to next group
            shouldJumpToNextGroup = true;
          }
          break;
        case 'left':
          newIndex = selectedIndex - 1;
          if (newIndex < 0) {
            // Would go before first card, try to jump to previous group
            shouldJumpToPreviousGroup = true;
          }
          break;
        case 'right':
          newIndex = selectedIndex + 1;
          if (newIndex >= totalItems) {
            // Would go after last card, try to jump to next group
            shouldJumpToNextGroup = true;
          }
          break;
        case 'tab':
          newIndex = selectedIndex + 1;
          if (newIndex >= totalItems) {
            // Would go after last card, try to jump to next group
            shouldJumpToNextGroup = true;
          }
          break;
        case 'shift-tab':
          newIndex = selectedIndex - 1;
          if (newIndex < 0) {
            // Would go before first card, try to jump to previous group
            shouldJumpToPreviousGroup = true;
          }
          break;
      }
    }

    // Handle jumping between groups
    if (shouldJumpToNextGroup) {
      const nextGroup = findAdjacentGroup(container, 'next');
      if (nextGroup) {
        const nextCards = nextGroup.querySelectorAll('[data-mtg-card="true"]');
        if (nextCards.length > 0) {
          // Clear current selection
          const allSelected = document.querySelectorAll('[data-selected="true"]');
          allSelected.forEach(selected => {
            selected.setAttribute('data-selected', 'false');
          });

          // Select first card in next group
          const firstCard = nextCards[0] as HTMLElement;
          firstCard.setAttribute('data-selected', 'true');
          firstCard.focus();
          firstCard.scrollIntoView({
            behavior: 'smooth',
            block: 'nearest',
            inline: 'nearest'
          });
        }
      }
    } else if (shouldJumpToPreviousGroup) {
      const previousGroup = findAdjacentGroup(container, 'previous');
      if (previousGroup) {
        const previousCards = previousGroup.querySelectorAll('[data-mtg-card="true"]');
        if (previousCards.length > 0) {
          // Clear current selection
          const allSelected = document.querySelectorAll('[data-selected="true"]');
          allSelected.forEach(selected => {
            selected.setAttribute('data-selected', 'false');
          });

          // Select last card in previous group (or appropriate card based on direction)
          let targetIndex = previousCards.length - 1;

          // For up arrow, try to maintain column position
          if (direction === 'up') {
            const prevColumns = calculateGridColumns(previousGroup);
            const currentColumn = selectedIndex % columns;
            const lastRowStart = Math.floor((previousCards.length - 1) / prevColumns) * prevColumns;
            targetIndex = Math.min(lastRowStart + currentColumn, previousCards.length - 1);
          }

          const targetCard = previousCards[targetIndex] as HTMLElement;
          targetCard.setAttribute('data-selected', 'true');
          targetCard.focus();
          targetCard.scrollIntoView({
            behavior: 'smooth',
            block: 'nearest',
            inline: 'nearest'
          });
        }
      }
    } else if (newIndex !== selectedIndex) {
      // Normal navigation within the same group
      selectCardByIndex(container, newIndex);
    }
  }, [totalItems, enabled, calculateGridColumns, getCurrentSelectedIndex, selectCardByIndex, findAdjacentGroup]);

  const handleKeyDown = useCallback((event: React.KeyboardEvent) => {
    if (!enabled) return;

    // Check if we're in an input field
    const target = event.target as HTMLElement;
    if (target.tagName === 'INPUT' || target.tagName === 'TEXTAREA') {
      return;
    }

    // Find the container for this group - it might be the target itself or a parent
    let container = target.closest(`[data-card-group="${groupId}"]`) as HTMLElement;
    if (!container) {
      container = document.querySelector(`[data-card-group="${groupId}"]`) as HTMLElement;
    }
    if (!container) {
      return;
    }

    // Update columns count if needed
    if (gridColumnsRef.current === 0) {
      gridColumnsRef.current = calculateGridColumns(container);
    }

    let handled = false;

    switch (event.key) {
      // Arrow keys
      case 'ArrowUp':
        navigate(container, 'up');
        handled = true;
        break;
      case 'ArrowDown':
        navigate(container, 'down');
        handled = true;
        break;
      case 'ArrowLeft':
        navigate(container, 'left');
        handled = true;
        break;
      case 'ArrowRight':
        navigate(container, 'right');
        handled = true;
        break;

      // WASD keys
      case 'w':
      case 'W':
        navigate(container, 'up');
        handled = true;
        break;
      case 's':
      case 'S':
        navigate(container, 'down');
        handled = true;
        break;
      case 'a':
      case 'A':
        navigate(container, 'left');
        handled = true;
        break;
      case 'd':
      case 'D':
        navigate(container, 'right');
        handled = true;
        break;

      // Tab navigation
      case 'Tab':
        if (event.shiftKey) {
          navigate(container, 'shift-tab');
        } else {
          navigate(container, 'tab');
        }
        handled = true;
        break;
    }

    if (handled) {
      event.preventDefault();
      event.stopPropagation();
    }
  }, [enabled, navigate, groupId, calculateGridColumns]);

  // Update columns on resize
  useEffect(() => {
    const updateColumns = () => {
      const container = document.querySelector(`[data-card-group="${groupId}"]`) as HTMLElement;
      if (container) {
        gridColumnsRef.current = calculateGridColumns(container);
      }
    };

    window.addEventListener('resize', updateColumns);
    return () => window.removeEventListener('resize', updateColumns);
  }, [groupId, calculateGridColumns]);

  return {
    handleKeyDown
  };
};