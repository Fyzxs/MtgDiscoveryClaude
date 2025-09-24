import { domOverlay } from './directDomOverlay';
import { domHelpPanel } from './directDomHelpPanel';
import type { CardFinish, CardSpecial, CollectionEntryState } from '../types/collection';

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
        domHelpPanel.show();
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
    domHelpPanel.hide();
  }

  private async submitEntry(cardId: string) {
    const handler = this.handlers.get(cardId);
    const state = this.entryStates.get(cardId);
    if (!handler || !state) return;

    const count = parseInt(state.count || '0');
    const finalCount = state.isNegative ? -count : count;

    try {
      await handler.onSubmit({
        cardId,
        count: finalCount,
        finish: state.finish,
        special: state.special
      });
      this.cancelEntry(cardId);
    } catch (error) {
      // Let user retry
    }
  }

  reset(cardId: string) {
    this.isEntering.set(cardId, false);
    this.entryStates.set(cardId, {
      count: '',
      finish: 'non-foil',
      special: 'none',
      isNegative: false
    });
    domOverlay.hide(cardId);
    domHelpPanel.hide();
  }
}

// Global singleton
export const globalCardEntry = new GlobalCardEntryHandler();