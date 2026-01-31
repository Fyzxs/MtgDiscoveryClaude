import { useState, useCallback, useEffect } from 'react';
import type { RefObject } from 'react';

interface SealedProductInteractionsProps {
  productRef: RefObject<HTMLDivElement | null>;
}

/**
 * Handles sealed product interactions matching card pattern exactly
 * - Always selects (never toggles)
 * - Clears other selections
 * - Manages focus for keyboard navigation
 * - Uses MutationObserver to track selection state
 */
export const useSealedProductInteractions = ({
  productRef
}: SealedProductInteractionsProps) => {
  const [isSelected, setIsSelected] = useState(false);

  // Track selection state from DOM attribute using MutationObserver
  useEffect(() => {
    if (!productRef.current) return;

    const observer = new MutationObserver((mutations) => {
      mutations.forEach((mutation) => {
        if (mutation.attributeName === 'data-selected') {
          const selected = productRef.current?.getAttribute('data-selected') === 'true';
          setIsSelected(selected || false);
        }
      });
    });

    observer.observe(productRef.current, { attributes: true, attributeFilter: ['data-selected'] });

    return () => observer.disconnect();
  }, [productRef]);

  // Deselect when clicking outside any sealed product
  useEffect(() => {
    const handleDocumentClick = (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      // Check if click was inside any sealed product
      const clickedProduct = target.closest('[data-product-uuid]');
      if (clickedProduct) return;

      // Click was outside all products - deselect all
      const allSelected = document.querySelectorAll('[data-selected="true"][data-product-uuid]');
      allSelected.forEach(selected => {
        selected.setAttribute('data-selected', 'false');
      });
    };

    document.addEventListener('click', handleDocumentClick);
    return () => document.removeEventListener('click', handleDocumentClick);
  }, []);

  const handleProductClick = useCallback((e: React.MouseEvent, onProductClick?: (product: any) => void, product?: any) => {
    // Don't trigger selection if clicking on links
    const target = e.target as HTMLElement;
    const clickedLink = target.closest('a');

    if (clickedLink) {
      return;
    }

    // Only prevent default for product selection clicks
    e.preventDefault();
    e.stopPropagation();

    const productElement = e.currentTarget as HTMLElement;

    // Clear ALL selections across ALL products on the page
    const allSelected = document.querySelectorAll('[data-selected="true"][data-product-uuid]');
    allSelected.forEach(selected => {
      if (selected !== productElement) {
        selected.setAttribute('data-selected', 'false');
      }
    });

    // ALWAYS select the clicked product (don't toggle - matching card behavior)
    productElement.setAttribute('data-selected', 'true');

    // Remove focus from any other focused element to clear blue borders
    const focused = document.querySelector(':focus');
    if (focused && focused !== productElement) {
      (focused as HTMLElement).blur();
    }

    // Focus this product to enable keyboard navigation from here
    productElement.focus();

    // Optionally call external click handler
    if (onProductClick && product) {
      onProductClick(product);
    }
  }, []);

  return {
    isSelected,
    handleProductClick
  };
};
