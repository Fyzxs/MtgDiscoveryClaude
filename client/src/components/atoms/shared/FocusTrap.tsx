import React, { useEffect, useRef, ReactNode } from 'react';

interface FocusTrapProps {
  children: ReactNode;
  active?: boolean;
  autoFocus?: boolean;
  restoreFocus?: boolean;
}

/**
 * Focus trap component for modals and other overlay components
 * Traps focus within the component when active
 */
export const FocusTrap: React.FC<FocusTrapProps> = React.memo(({
  children,
  active = true,
  autoFocus = true,
  restoreFocus = true
}) => {
  const containerRef = useRef<HTMLDivElement>(null);
  const previousActiveElement = useRef<HTMLElement | null>(null);

  // Get all focusable elements within the container
  const getFocusableElements = (): HTMLElement[] => {
    if (!containerRef.current) return [];

    const focusableSelectors = [
      'a[href]',
      'button:not([disabled])',
      'textarea:not([disabled])',
      'input:not([disabled])',
      'select:not([disabled])',
      '[tabindex]:not([tabindex="-1"])',
      '[contenteditable="true"]',
      'audio[controls]',
      'video[controls]',
      'iframe',
      'object',
      'embed',
      'area[href]',
      'summary'
    ];

    const elements = containerRef.current.querySelectorAll<HTMLElement>(
      focusableSelectors.join(', ')
    );

    return Array.from(elements).filter((element) => {
      // Check if element is actually visible and interactable
      const style = window.getComputedStyle(element);
      return (
        style.display !== 'none' &&
        style.visibility !== 'hidden' &&
        style.opacity !== '0' &&
        !element.hasAttribute('disabled') &&
        !element.hasAttribute('aria-hidden')
      );
    });
  };

  // Handle tab key navigation
  const handleKeyDown = (event: KeyboardEvent) => {
    if (!active || event.key !== 'Tab' || !containerRef.current) return;

    const focusableElements = getFocusableElements();
    if (focusableElements.length === 0) return;

    const firstFocusable = focusableElements[0];
    const lastFocusable = focusableElements[focusableElements.length - 1];
    const currentElement = document.activeElement as HTMLElement;

    // If we're not within the trap, focus the first element
    if (!containerRef.current.contains(currentElement)) {
      event.preventDefault();
      firstFocusable.focus();
      return;
    }

    if (event.shiftKey) {
      // Shift + Tab (backwards)
      if (currentElement === firstFocusable) {
        event.preventDefault();
        lastFocusable.focus();
      }
    } else {
      // Tab (forwards)
      if (currentElement === lastFocusable) {
        event.preventDefault();
        firstFocusable.focus();
      }
    }
  };

  // Set up focus trap
  useEffect(() => {
    if (!active) return;

    // Store the previously focused element
    previousActiveElement.current = document.activeElement as HTMLElement;

    // Focus the first focusable element if autoFocus is enabled
    if (autoFocus) {
      const focusableElements = getFocusableElements();
      if (focusableElements.length > 0) {
        focusableElements[0].focus();
      }
    }

    // Add event listener for Tab key
    document.addEventListener('keydown', handleKeyDown);

    return () => {
      document.removeEventListener('keydown', handleKeyDown);

      // Restore focus to the previously focused element
      if (restoreFocus && previousActiveElement.current) {
        previousActiveElement.current.focus();
      }
    };
  }, [active, autoFocus, restoreFocus]);

  return (
    <div ref={containerRef} style={{ outline: 'none' }}>
      {children}
    </div>
  );
});

FocusTrap.displayName = 'FocusTrap';