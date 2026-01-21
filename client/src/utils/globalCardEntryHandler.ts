import { logger } from './logger';
import { perfMonitor } from './performanceMonitor';
import type { CardFinish, CardSpecial, CardCollectionUpdate } from '../types/collection';

interface CollectionEntryState {
  count: string;
  finish: CardFinish;
  special: CardSpecial;
  isNegative: boolean;
}

interface CardHandler {
  cardId: string;
  availableFinishes: CardFinish[];
  onSubmit: (update: CardCollectionUpdate) => Promise<void>;
  onFlashInvalid: () => void;
  onOverlayUpdate: (state: { count: string; isNegative: boolean; visible: boolean; finish: CardFinish; special: CardSpecial }) => void;
}

class GlobalCardEntryHandler {
  private handlers = new Map<string, CardHandler>();
  private entryStates = new Map<string, CollectionEntryState>();
  private isEntering = new Map<string, boolean>();
  private recentEnterKeydown = false;

  constructor() {
    // Install ONE global handler that never changes
    document.addEventListener('keydown', this.handleKeyDown.bind(this), true);
    document.addEventListener('keyup', this.handleKeyUp.bind(this), true);
  }

  private getDefaultFinish(cardId: string): CardFinish {
    const handler = this.handlers.get(cardId);
    return handler?.availableFinishes[0] || 'non-foil';
  }

  register(cardId: string, handler: CardHandler) {
    this.handlers.set(cardId, handler);
    if (!this.entryStates.has(cardId)) {
      this.entryStates.set(cardId, {
        count: '',
        finish: this.getDefaultFinish(cardId),
        special: 'none',
        isNegative: false
      });
    }
  }

  unregister(cardId: string) {
    this.handlers.delete(cardId);
    this.entryStates.delete(cardId);
    this.isEntering.delete(cardId);
  }

  reset(cardId: string) {
    this.isEntering.set(cardId, false);
    const state = this.entryStates.get(cardId);
    if (state) {
      state.count = '';
      state.finish = this.getDefaultFinish(cardId);
      state.special = 'none';
      state.isNegative = false;
    }

    const handler = this.handlers.get(cardId);
    if (handler) {
      handler.onOverlayUpdate({
        count: '',
        isNegative: false,
        visible: false,
        finish: this.getDefaultFinish(cardId),
        special: 'none'
      });
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
        this.recentEnterKeydown = true;
        event.preventDefault();
        event.stopPropagation();
      }
      return;
    }

    // Valid entry keys
    const validKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
      'z', 'n', 'f', 'o', 'e', 'h', 'g', 'i', 'r', 'p', 't', 'm',
      '+', '`', '-', '~', 'x', 'backspace'];

    if (validKeys.includes(key) || (isShift && key === '~')) {
      event.preventDefault();
      event.stopPropagation();

      if (!this.isEntering.get(cardId)) {
        this.isEntering.set(cardId, true);
      }

      // Process the key
      this.processKey(cardId, key, isShift);
    }
  }

  private processKey(cardId: string, key: string, isShift: boolean) {
    const handler = this.handlers.get(cardId);
    if (!handler) return;

    const state = { ...this.entryStates.get(cardId)! };

    // Number keys
    if (key >= '0' && key <= '9') {
      state.count = state.count === '0' ? key : state.count + key;
    }
    // Backspace - delete last digit
    else if (key === 'backspace') {
      const current = state.count || '';
      if (current.length > 1) {
        state.count = current.slice(0, -1);
      } else {
        state.count = '';
      }
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

    // Update overlay via React callback
    handler.onOverlayUpdate({ ...state, visible: true });
  }

  private cancelEntry(cardId: string) {
    this.reset(cardId);
  }

  private async submitEntry(cardId: string) {
    const handler = this.handlers.get(cardId);
    const state = this.entryStates.get(cardId);
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
    perfMonitor.start('card-entry-submit');
    try {
      await handler.onSubmit({
        cardId: handler.cardId,
        count,
        finish: state.finish,
        special: state.special,
        setId: '', // Will be filled in by the onSubmit handler
        setCode: '',
        setGroupId: null
      });
    } catch (error) {
      logger.error('Failed to submit card entry:', error);
    } finally {
      perfMonitor.end('card-entry-submit');
    }

    // Reset entry state
    this.reset(cardId);
  }
}

// Export singleton instance
export const globalCardEntry = new GlobalCardEntryHandler();