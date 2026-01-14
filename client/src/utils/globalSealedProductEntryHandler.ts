import { logger } from './logger';
import { perfMonitor } from './performanceMonitor';
import type { SealedProductCollectionUpdate } from '../contexts/SealedCollectionContext';

interface SealedProductEntryState {
  count: string;
  isNegative: boolean;
}

interface SealedProductHandler {
  productUuid: string;
  setId: string;
  onSubmit: (update: SealedProductCollectionUpdate) => Promise<void>;
  onFlashInvalid: () => void;
  onOverlayUpdate: (state: { count: string; isNegative: boolean; visible: boolean }) => void;
}

class GlobalSealedProductEntryHandler {
  private handlers = new Map<string, SealedProductHandler>();
  private entryStates = new Map<string, SealedProductEntryState>();
  private isEntering = new Map<string, boolean>();
  private recentEnterKeydown = false;

  constructor() {
    // Install ONE global handler that never changes
    document.addEventListener('keydown', this.handleKeyDown.bind(this), true);
    document.addEventListener('keyup', this.handleKeyUp.bind(this), true);
  }

  register(productUuid: string, handler: SealedProductHandler) {
    this.handlers.set(productUuid, handler);
    if (!this.entryStates.has(productUuid)) {
      this.entryStates.set(productUuid, {
        count: '',
        isNegative: false
      });
    }
  }

  unregister(productUuid: string) {
    this.handlers.delete(productUuid);
    this.entryStates.delete(productUuid);
    this.isEntering.delete(productUuid);
  }

  reset(productUuid: string) {
    this.isEntering.set(productUuid, false);
    const state = this.entryStates.get(productUuid);
    if (state) {
      state.count = '';
      state.isNegative = false;
    }

    const handler = this.handlers.get(productUuid);
    if (handler) {
      handler.onOverlayUpdate({ count: '', isNegative: false, visible: false });
    }
  }

  private handleKeyUp(event: KeyboardEvent) {
    // Prevent Enter keyup from triggering button activation
    if (this.recentEnterKeydown && event.key.toLowerCase() === 'enter') {
      this.recentEnterKeydown = false;
      event.preventDefault();
      event.stopPropagation();
    }
  }

  private handleKeyDown(event: KeyboardEvent) {
    // Don't capture keys when user is typing in an input/textarea/select
    const target = event.target as HTMLElement;
    if (target.tagName === 'INPUT' || target.tagName === 'TEXTAREA' || target.tagName === 'SELECT') {
      return;
    }

    // Don't capture keys when a modal or drawer is open
    const modalOrDrawerOpen = document.querySelector('.MuiModal-root, .MuiDrawer-root');
    if (modalOrDrawerOpen) {
      return;
    }

    // CRITICAL: Block all collection keyboard shortcuts if no collector ID in URL
    const urlParams = new URLSearchParams(window.location.search);
    const collectorId = urlParams.get('ctor');
    if (!collectorId) {
      return;
    }

    // Find selected sealed product via DOM
    const selectedProduct = document.querySelector('[data-selected="true"][data-product-uuid]');
    if (!selectedProduct) return;

    const productUuid = selectedProduct.getAttribute('data-product-uuid');
    if (!productUuid) return;

    const handler = this.handlers.get(productUuid);
    if (!handler) return;

    const key = event.key.toLowerCase();
    const isShift = event.shiftKey;

    // Don't process if modifier keys (except shift) are pressed
    if (event.ctrlKey || event.altKey || event.metaKey) return;

    // Handle escape
    if (key === 'escape') {
      if (this.isEntering.get(productUuid)) {
        this.cancelEntry(productUuid);
        event.preventDefault();
        event.stopPropagation();
      }
      return;
    }

    // Handle enter
    if (key === 'enter') {
      if (this.isEntering.get(productUuid)) {
        this.submitEntry(productUuid);
        this.recentEnterKeydown = true;
        event.preventDefault();
        event.stopPropagation();
      }
      return;
    }

    // Valid entry keys for sealed products (simpler than cards - no finish/special)
    // Note: Arrow keys and WASD are NOT included - they're for navigation
    const validKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
      '+', '`', '-', '~', 'x', 'backspace'];

    if (validKeys.includes(key) || (isShift && key === '~')) {
      event.preventDefault();
      event.stopPropagation();

      if (!this.isEntering.get(productUuid)) {
        this.isEntering.set(productUuid, true);
      }

      // Process the key
      this.processKey(productUuid, key, isShift);
    }
  }

  private processKey(productUuid: string, key: string, isShift: boolean) {
    const handler = this.handlers.get(productUuid);
    if (!handler) return;

    const state = { ...this.entryStates.get(productUuid)! };

    // Number keys
    if (key >= '0' && key <= '9') {
      // Limit to 4 digits to prevent overflow
      if (state.count.length < 4) {
        state.count += key;
      }
    }
    // Increment (+/`)
    else if (key === '+' || key === '`') {
      const current = state.count === '' ? 0 : parseInt(state.count, 10);
      state.count = Math.min(current + 1, 9999).toString();
      state.isNegative = false;
    }
    // Decrement (-/~)
    else if (key === '-' || (isShift && key === '~')) {
      const current = state.count === '' ? 0 : parseInt(state.count, 10);
      const newValue = Math.max(current - 1, 0);
      state.count = newValue.toString();
      state.isNegative = false;
    }
    // Negate (x)
    else if (key === 'x') {
      state.isNegative = !state.isNegative;
    }
    // Backspace
    else if (key === 'backspace') {
      state.count = state.count.slice(0, -1);
    }

    // Update state
    this.entryStates.set(productUuid, state);

    // Update overlay via React callback
    handler.onOverlayUpdate({ ...state, visible: true });
  }

  private cancelEntry(productUuid: string) {
    this.reset(productUuid);
  }

  private async submitEntry(productUuid: string) {
    const handler = this.handlers.get(productUuid);
    const state = this.entryStates.get(productUuid);
    if (!handler || !state) return;

    // Validate count
    if (state.count === '') {
      handler.onFlashInvalid();
      return;
    }

    let count = parseInt(state.count, 10);
    if (state.isNegative) {
      count = -count;
    }

    // Submit the update
    perfMonitor.start('sealed-entry-submit');
    try {
      await handler.onSubmit({
        productUuid: handler.productUuid,
        setId: handler.setId,
        count
      });
    } catch (error) {
      logger.error('Failed to submit sealed product entry:', error);
    } finally {
      perfMonitor.end('sealed-entry-submit');
    }

    // Reset entry state
    this.reset(productUuid);
  }
}

// Export singleton instance
export const globalSealedProductEntry = new GlobalSealedProductEntryHandler();
