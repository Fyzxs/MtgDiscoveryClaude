import { useEffect, useCallback, useRef } from 'react';

interface UseSealedProductGridNavigationOptions {
  totalItems: number;
  enabled?: boolean;
}

interface UseSealedProductGridNavigationResult {
  handleKeyDown: (event: React.KeyboardEvent) => void;
}

export const useSealedProductGridNavigation = ({
  totalItems,
  enabled = true
}: UseSealedProductGridNavigationOptions): UseSealedProductGridNavigationResult => {
  const gridColumnsRef = useRef(0);

  // Calculate number of columns based on rendered grid
  const calculateGridColumns = useCallback((container: HTMLElement) => {
    const products = container.querySelectorAll('[data-product-uuid]');
    if (products.length < 2) return 1;

    // Find first two products in different rows
    const firstProduct = products[0] as HTMLElement;
    const firstTop = firstProduct.offsetTop;

    let columns = 1;
    for (let i = 1; i < products.length; i++) {
      const product = products[i] as HTMLElement;
      if (product.offsetTop > firstTop) {
        columns = i;
        break;
      }
      // If we reach the end without finding a new row, all products are in one row
      if (i === products.length - 1) {
        columns = products.length;
      }
    }

    return columns;
  }, []);

  // Get current selected index from DOM
  const getCurrentSelectedIndex = useCallback((container: HTMLElement): number => {
    const selected = container.querySelector('[data-selected="true"][data-product-uuid]');
    if (selected) {
      const index = selected.getAttribute('data-product-index');
      if (index !== null) {
        return parseInt(index, 10);
      }
    }
    return -1;
  }, []);

  // Select product by index using DOM
  const selectProductByIndex = useCallback((container: HTMLElement, index: number) => {
    // Clear ALL current selections
    const allSelected = document.querySelectorAll('[data-selected="true"]');
    allSelected.forEach(selected => {
      selected.setAttribute('data-selected', 'false');
    });

    // Clear any focus to remove blue borders
    const focused = document.querySelector(':focus');
    if (focused) {
      (focused as HTMLElement).blur();
    }

    // Set new selection
    const products = container.querySelectorAll('[data-product-uuid]');

    if (index >= 0 && index < products.length) {
      const product = products[index] as HTMLElement;
      product.setAttribute('data-selected', 'true');

      // Focus the product for accessibility
      product.focus();

      // Scroll into view
      product.scrollIntoView({
        behavior: 'smooth',
        block: 'nearest',
        inline: 'nearest'
      });
    }
  }, []);

  const navigate = useCallback((container: HTMLElement, direction: 'up' | 'down' | 'left' | 'right' | 'tab' | 'shift-tab') => {
    if (!enabled || totalItems === 0) return;

    const selectedIndex = getCurrentSelectedIndex(container);
    const columns = gridColumnsRef.current || calculateGridColumns(container);
    let newIndex = selectedIndex;

    // Initialize if no selection
    if (selectedIndex === -1) {
      newIndex = 0;
    } else {
      switch (direction) {
        case 'up':
          newIndex = selectedIndex - columns;
          // Stay on current row if at top
          if (newIndex < 0) return;
          break;
        case 'down':
          newIndex = selectedIndex + columns;
          // Stay on current row if at bottom
          if (newIndex >= totalItems) return;
          break;
        case 'left':
          // Don't wrap to previous row - stop at left edge
          if (selectedIndex % columns === 0) return;
          newIndex = selectedIndex - 1;
          break;
        case 'right':
          // Don't wrap to next row - stop at right edge
          if ((selectedIndex + 1) % columns === 0 || selectedIndex === totalItems - 1) return;
          newIndex = selectedIndex + 1;
          break;
        case 'tab':
          newIndex = selectedIndex + 1;
          // Stop at last item - no wrap
          if (newIndex >= totalItems) return;
          break;
        case 'shift-tab':
          newIndex = selectedIndex - 1;
          // Stop at first item - no wrap
          if (newIndex < 0) return;
          break;
      }
    }

    if (newIndex !== selectedIndex) {
      selectProductByIndex(container, newIndex);
    }
  }, [totalItems, enabled, calculateGridColumns, getCurrentSelectedIndex, selectProductByIndex]);

  const handleKeyDown = useCallback((event: React.KeyboardEvent) => {
    if (!enabled) return;

    // Check if we're in an input field or if entry mode is active
    const target = event.target as HTMLElement;
    if (target.tagName === 'INPUT' || target.tagName === 'TEXTAREA') {
      return;
    }

    // Don't handle navigation if user is in entry mode (overlay visible)
    const isEnteringData = document.querySelector('[data-selected="true"] .collection-entry-overlay');
    if (isEnteringData) {
      return;
    }

    // Find the sealed products container
    const container = document.querySelector('[data-sealed-products-grid]') as HTMLElement;
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
  }, [enabled, navigate, calculateGridColumns]);

  // Update columns on resize
  useEffect(() => {
    const updateColumns = () => {
      const container = document.querySelector('[data-sealed-products-grid]') as HTMLElement;
      if (container) {
        gridColumnsRef.current = calculateGridColumns(container);
      }
    };

    window.addEventListener('resize', updateColumns);
    return () => window.removeEventListener('resize', updateColumns);
  }, [calculateGridColumns]);

  return {
    handleKeyDown
  };
};
