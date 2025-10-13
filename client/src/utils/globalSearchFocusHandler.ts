/**
 * Global keyboard handler for double-tap Shift to focus and clear search input
 *
 * Pattern: Press Shift twice quickly (within 400ms) to focus the search box and clear it
 */

interface HTMLInputElementWithClear extends HTMLInputElement {
  __clearSearch?: () => void;
}

class GlobalSearchFocusHandler {
  private lastShiftTime: number = 0;
  private readonly DOUBLE_TAP_THRESHOLD_MS = 400;

  constructor() {
    // Install ONE global handler that never changes
    document.addEventListener('keydown', this.handleKeyDown.bind(this), true);
  }

  private handleKeyDown(event: KeyboardEvent) {
    // Only care about Shift key
    if (event.key !== 'Shift') {
      return;
    }

    // Don't trigger if user is already in an input/textarea/select (except our search input)
    const target = event.target as HTMLElement;
    const isSearchInput = target.getAttribute('data-search-input') === 'true';

    if (
      (target.tagName === 'INPUT' || target.tagName === 'TEXTAREA' || target.tagName === 'SELECT') &&
      !isSearchInput
    ) {
      return;
    }

    const now = Date.now();
    const timeSinceLastShift = now - this.lastShiftTime;

    // Check if this is a double-tap
    if (timeSinceLastShift < this.DOUBLE_TAP_THRESHOLD_MS) {
      // Double-tap detected!
      this.focusAndClearSearch();
      // Reset the timer
      this.lastShiftTime = 0;
    } else {
      // First tap, record the time
      this.lastShiftTime = now;
    }
  }

  private focusAndClearSearch() {
    // Find the card search input by data attribute (DebouncedSearchInput)
    const searchInput = document.querySelector('[data-search-input="true"]') as HTMLInputElement;

    if (searchInput) {
      // Clear input and focus
      searchInput.focus();
      searchInput.select();

      // Clear any selected cards
      const allSelected = document.querySelectorAll('[data-selected="true"]');
      allSelected.forEach(selected => {
        selected.setAttribute('data-selected', 'false');
      });

      // Call the clear function which will defer the state update
      // If user starts typing within 200ms, the state update will be cancelled
      const clearFn = (searchInput as HTMLInputElementWithClear).__clearSearch;
      if (clearFn && typeof clearFn === 'function') {
        clearFn();
      }
    }
  }
}

// Global singleton
export const globalSearchFocus = new GlobalSearchFocusHandler();
