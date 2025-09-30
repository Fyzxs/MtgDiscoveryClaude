import { domOverlay } from './directDomOverlay';
import type { CardFinish, CardSpecial, CollectionEntryState } from '../types/collection';
import { perfMonitor } from './performanceMonitor';

interface CardHandler {
  cardId: string;
  availableFinishes: CardFinish[];
  onSubmit: (update: any) => Promise<void>;
  onFlashInvalid: () => void;
}

class GlobalCardEntryHandler {
  private handlers = new Map<string, CardHandler>();
  private entryStates = new Map<string, CollectionEntryState>();
  private isEntering = new Map<string, boolean>();

  constructor() {
    // Install ONE global handler that never changes
    document.addEventListener('keydown', this.handleKeyDown.bind(this), true);
  }

  register(cardId: string, handler: CardHandler) {
    this.handlers.set(cardId, handler);
    if (!this.entryStates.has(cardId)) {
      this.entryStates.set(cardId, {
        count: '',
        finish: 'non-foil',
        special: 'none',
        isNegative: false
      });
    }
  }

  unregister(cardId: string) {
    this.handlers.delete(cardId);
    this.entryStates.delete(cardId);
    this.isEntering.delete(cardId);
    domOverlay.cleanup(cardId);
  }

  private handleKeyDown(event: KeyboardEvent) {
    // Don't capture keys when user is typing in an input/textarea/select
    const target = event.target as HTMLElement;
    if (target.tagName === 'INPUT' || target.tagName === 'TEXTAREA' || target.tagName === 'SELECT') {
      return;
    }

    // Find selected card via DOM
    const selectedCard = document.querySelector('[data-selected="true"][data-card-id]');
    if (!selectedCard) return;

    const cardId = selectedCard.getAttribute('data-card-id');
    if (!cardId) return;

    const handler = this.handlers.get(cardId);
    if (!handler) return;

    const key = event.key.toLowerCase();
    const isShift = event.shiftKey;

    // Don't process if modifier keys (except shift) are pressed
    if (event.ctrlKey || event.altKey || event.metaKey) return;

    // Handle escape
    if (key === 'escape') {
      if (this.isEntering.get(cardId)) {
        this.cancelEntry(cardId);
        event.preventDefault();
        event.stopPropagation();
      }
      return;
    }

    // Handle enter
    if (key === 'enter') {
      if (this.isEntering.get(cardId)) {
        this.submitEntry(cardId);
        event.preventDefault();
        event.stopPropagation();
      }
      return;
    }

    // Valid entry keys
    const validKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
      'z', 'n', 'f', 'o', 'e', 'h', 'g', 'i', 'r', 'p', 't', 'm',
      '+', '`', '-', '~', 'x'];

    if (validKeys.includes(key) || (isShift && key === '~')) {
      event.preventDefault();
      event.stopPropagation();

      if (!this.isEntering.get(cardId)) {
        // IMMEDIATELY show overlay
        const state = this.entryStates.get(cardId)!;
        domOverlay.show(cardId, state, false);
        this.isEntering.set(cardId, true);
      }

      // Process the key
      this.processKey(cardId, key, isShift);
    }
  }

  private processKey(cardId: string, key: string, isShift: boolean) {
    const handler = this.handlers.get(cardId);
    if (!handler) return;

    let state = { ...this.entryStates.get(cardId)! };

    // Number keys
    if (key >= '0' && key <= '9') {
      state.count = state.count === '0' ? key : state.count + key;
    }
    // Increment
    else if (key === '+' || key === '`') {
      const current = parseInt(state.count || '0');
      state.count = String(current + 1);
    }
    // Decrement
    else if (key === '-' || (isShift && key === '~') || key === '~') {
      const current = parseInt(state.count || '0');
      if (current > 0) {
        state.count = String(current - 1);
      }
    }
    // Negate
    else if (key === 'x') {
      const count = state.count || '0';
      if (count !== '0' && count !== '') {
        state.isNegative = !state.isNegative;
      }
    }
    // Finish keys
    else if (['z', 'n', 'f', 'o', 'e', 'h'].includes(key)) {
      const finishMap: Record<string, CardFinish> = {
        'z': 'non-foil',
        'n': 'non-foil',
        'f': 'foil',
        'o': 'foil',
        'e': 'etched',
        'h': 'etched'
      };
      const targetFinish = finishMap[key];
      if (handler.availableFinishes.includes(targetFinish)) {
        state.finish = targetFinish;
      } else {
        handler.onFlashInvalid();
        return;
      }
    }
    // Special keys
    else if (['g', 'i', 'r', 'p', 't', 'm'].includes(key)) {
      const specialMap: Record<string, CardSpecial> = {
        'g': 'signed',
        'i': 'signed',
        'r': 'artist-proof',
        'p': 'artist-proof',
        't': 'altered',
        'm': 'altered'
      };
      const newSpecial = specialMap[key];
      state.special = state.special === newSpecial ? 'none' : newSpecial;
    }

    // Update state
    this.entryStates.set(cardId, state);
    // Update DOM
    domOverlay.show(cardId, state, false);
  }

  private cancelEntry(cardId: string) {
    this.isEntering.set(cardId, false);
    this.entryStates.set(cardId, {
      count: '',
      finish: 'non-foil',
      special: 'none',
      isNegative: false
    });
    domOverlay.hide(cardId);
  }

  private async submitEntry(cardId: string) {
    perfMonitor.start('submit-entry-total');

    const handler = this.handlers.get(cardId);
    const state = this.entryStates.get(cardId);
    if (!handler || !state) return;

    // Pre-calculate BEFORE any DOM operations
    perfMonitor.measure('submit-prepare-data', () => {
      const count = parseInt(state.count || '0');
      const finalCount = state.isNegative ? -count : count;
      (window as any).__tempUpdateData = {
        cardId,
        count: finalCount,
        finish: state.finish,
        special: state.special
      };
    });

    const updateData = (window as any).__tempUpdateData;
    delete (window as any).__tempUpdateData;

    // Get card element for instant state updates
    const cardElement = document.querySelector(`[data-card-id="${cardId}"]`);

    // CRITICAL: Instant visual feedback - no animations
    perfMonitor.measure('submit-hide-ui', () => {
      // 1. Mark card as submitting (disables transform transition) but KEEP selected
      if (cardElement) {
        cardElement.setAttribute('data-submitting', 'true');
        // Don't deselect - user wants to stay on same card for rapid entry
      }

      // 2. Hide overlay immediately
      domOverlay.hide(cardId);

      // 3. Reset state immediately after
      this.isEntering.set(cardId, false);
      this.entryStates.set(cardId, {
        count: '',
        finish: 'non-foil',
        special: 'none',
        isNegative: false
      });
    });

    perfMonitor.end('submit-entry-total');

    // Fire-and-forget the submission - don't block UI
    handler.onSubmit(updateData).catch(error => {
      console.error('[GlobalCardEntry] Submission failed:', error);
      // Error handling happens in CollectionContext (toast)
    });
  }

  reset(cardId: string) {
    // Only hide if we're actually entering (navigating away from entry mode)
    // Don't hide if submitEntry already did it
    const wasEntering = this.isEntering.get(cardId);

    this.isEntering.set(cardId, false);
    this.entryStates.set(cardId, {
      count: '',
      finish: 'non-foil',
      special: 'none',
      isNegative: false
    });

    // Hide overlay if user was entering (navigating away), but submitEntry already hides it
    if (wasEntering) {
      domOverlay.hide(cardId);
    }
  }
}

// Global singleton
export const globalCardEntry = new GlobalCardEntryHandler();