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
  private overlayElements = new Map<string, HTMLElement>();
  private recentEnterKeydown = false;
  // Version counter per card to prevent deferred resets from stomping on new entries
  private stateVersion = new Map<string, number>();
  // Timing reference for relative logging
  private _t0 = 0;
  private _elapsed() { return `+${(performance.now() - this._t0).toFixed(1)}ms`; }

  constructor() {
    // Install ONE global handler that never changes
    document.addEventListener('keydown', this.handleKeyDown.bind(this), true);
    document.addEventListener('keyup', this.handleKeyUp.bind(this), true);

    // Inject global CSS for overlay visibility — plain CSS with !important
    // beats any Emotion/MUI-generated class, and data attributes are never
    // touched by React reconciliation.
    const style = document.createElement('style');
    style.textContent = `[data-overlay-visible="true"] {
      display: flex !important;
      opacity: 1 !important;
      visibility: visible !important;
      pointer-events: auto !important;
    }`;
    document.head.appendChild(style);
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
    this.hideOverlay(cardId);
    this.handlers.delete(cardId);
    this.entryStates.delete(cardId);
    this.isEntering.delete(cardId);
    // overlayElements NOT deleted here — managed by CollectionEntryOverlay's
    // useLayoutEffect lifecycle (registerOverlay(cardId, null) on unmount)
    this.stateVersion.delete(cardId);
  }

  registerOverlay(cardId: string, element: HTMLElement | null) {
    if (element) {
      this.overlayElements.set(cardId, element);
    } else {
      this.overlayElements.delete(cardId);
    }
  }

  private getOverlayElement(cardId: string): HTMLElement | null {
    // Fast path: use pre-registered element if still in the DOM
    const registered = this.overlayElements.get(cardId);
    if (registered?.isConnected) return registered;

    // Slow path: element was detached (component remounted after re-render).
    // Query DOM for the current overlay inside this card.
    const card = document.querySelector(`[data-card-id="${cardId}"]`);
    const overlay = card?.querySelector('[data-collection-overlay="true"]') as HTMLElement | null;
    if (overlay) {
      this.overlayElements.set(cardId, overlay);
      console.log(`[OVERLAY ${this._elapsed()}] RE-ACQUIRED element for ${cardId.slice(0, 8)}`);
    }
    return overlay;
  }

  private hideOverlay(cardId: string) {
    const overlay = this.getOverlayElement(cardId);
    if (overlay) {
      overlay.removeAttribute('data-overlay-visible');
      const cs = getComputedStyle(overlay);
      console.log(`[OVERLAY ${this._elapsed()}] HIDE DOM done — computed: display=${cs.display} opacity=${cs.opacity} connected=${overlay.isConnected}`);
    } else {
      console.warn(`[OVERLAY ${this._elapsed()}] HIDE — NO ELEMENT for ${cardId.slice(0, 8)}`);
    }
  }

  private showOverlay(cardId: string) {
    const overlay = this.getOverlayElement(cardId);
    if (overlay) {
      overlay.setAttribute('data-overlay-visible', 'true');
      const cs = getComputedStyle(overlay);
      console.log(`[OVERLAY ${this._elapsed()}] SHOW DOM done — computed: display=${cs.display} opacity=${cs.opacity} vis=${cs.visibility} connected=${overlay.isConnected}`);
    } else {
      console.warn(`[OVERLAY ${this._elapsed()}] SHOW — NO ELEMENT for ${cardId.slice(0, 8)}`);
    }
  }

  reset(cardId: string) {
    console.log(`[OVERLAY ${this._elapsed()}] RESET start`);

    this.isEntering.set(cardId, false);
    const state = this.entryStates.get(cardId);
    if (state) {
      state.count = '';
      state.finish = this.getDefaultFinish(cardId);
      state.special = 'none';
      state.isNegative = false;
    }

    // Defer BOTH the DOM hide and React state sync to the next paint frame.
    // When the user types a digit and presses Enter rapidly, the browser
    // processes both keydown events in a single JS task — showOverlay()
    // and hideOverlay() would both run before the browser ever paints,
    // so the overlay is never visible. By deferring the hide past one
    // paint frame, the SHOW state is guaranteed to render first.
    // Version check prevents stale resets from stomping on new entries.
    const version = (this.stateVersion.get(cardId) || 0) + 1;
    this.stateVersion.set(cardId, version);

    const handler = this.handlers.get(cardId);
    const t0 = this._t0;
    console.log(`[OVERLAY ${this._elapsed()}] RESET deferred hide+sync scheduled (v${version})`);
    requestAnimationFrame(() => {
      const e = `+${(performance.now() - t0).toFixed(1)}ms`;
      console.log(`[OVERLAY ${e}] RESET rAF (v${version})`);
      setTimeout(() => {
        const e2 = `+${(performance.now() - t0).toFixed(1)}ms`;
        if (this.stateVersion.get(cardId) === version) {
          console.log(`[OVERLAY ${e2}] RESET deferred hide+sync (v${version})`);
          this.hideOverlay(cardId);
          if (handler) {
            handler.onOverlayUpdate({
              count: '',
              isNegative: false,
              visible: false,
              finish: this.getDefaultFinish(cardId),
              special: 'none'
            });
          }
        } else {
          console.log(`[OVERLAY ${e2}] RESET deferred SKIPPED v${version}→v${this.stateVersion.get(cardId)}`);
        }
      }, 0);
    });

    console.log(`[OVERLAY ${this._elapsed()}] RESET end`);
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
        this.reset(cardId);
        event.preventDefault();
        event.stopPropagation();
      }
      return;
    }

    // Handle enter
    if (key === 'enter') {
      if (this.isEntering.get(cardId)) {
        this._t0 = performance.now();
        console.log(`[OVERLAY +0.0ms] ── ENTER ──`);
        this.submitEntry(cardId);
        console.log(`[OVERLAY ${this._elapsed()}] ENTER handler done (sync)`);
        // Paint markers
        const t0 = this._t0;
        requestAnimationFrame(() => {
          console.log(`[OVERLAY +${(performance.now() - t0).toFixed(1)}ms] ENTER rAF (pre-paint)`);
          setTimeout(() => {
            console.log(`[OVERLAY +${(performance.now() - t0).toFixed(1)}ms] ENTER post-paint`);
          }, 0);
        });
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

      const wasEntering = this.isEntering.get(cardId);
      if (!wasEntering) {
        this.isEntering.set(cardId, true);
      }

      this._t0 = performance.now();
      console.log(`[OVERLAY +0.0ms] ── KEY '${key}' ── wasEntering=${wasEntering}`);
      this.processKey(cardId, key, isShift);
      console.log(`[OVERLAY ${this._elapsed()}] KEY handler done (sync)`);
    }
  }

  private processKey(cardId: string, key: string, isShift: boolean) {
    const handler = this.handlers.get(cardId);
    if (!handler) return;

    // Invalidate any pending deferred reset so it won't stomp on this new entry
    const version = (this.stateVersion.get(cardId) || 0) + 1;
    this.stateVersion.set(cardId, version);

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
        'r': 'proof',
        'p': 'proof',
        't': 'altered',
        'm': 'altered'
      };
      const newSpecial = specialMap[key];
      state.special = state.special === newSpecial ? 'none' : newSpecial;
    }

    // Update state
    this.entryStates.set(cardId, state);

    // Update count text directly in the DOM for instant visual feedback,
    // then show overlay via DOM attribute. This avoids waiting for React's
    // synchronous batch processing which blocks the browser paint.
    const overlay = this.getOverlayElement(cardId);
    if (overlay) {
      const countEl = overlay.querySelector('[data-overlay-count]');
      if (countEl) {
        countEl.textContent = state.isNegative ? `-${state.count}` : (state.count || '');
      }
    }
    this.showOverlay(cardId);

    // Defer React state sync PAST the browser paint so the DOM-driven
    // overlay visibility and content aren't blocked by React rendering.
    // Version check prevents stale callbacks from firing after a reset.
    const stateSnapshot = { ...state, visible: true };
    const capturedVersion = version;
    console.log(`[OVERLAY ${this._elapsed()}] processKey deferring onOverlayUpdate (count=${state.count})`);
    requestAnimationFrame(() => {
      setTimeout(() => {
        if (this.stateVersion.get(cardId) !== capturedVersion) return;
        console.log(`[OVERLAY ${this._elapsed()}] processKey deferred onOverlayUpdate (count=${stateSnapshot.count})`);
        handler.onOverlayUpdate(stateSnapshot);
      }, 0);
    });
  }

  private submitEntry(cardId: string) {
    console.log(`[OVERLAY ${this._elapsed()}] submitEntry start`);

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

    // Capture values BEFORE resetting
    const finish = state.finish;
    const special = state.special;

    // Hide overlay synchronously BEFORE reset() defers its own hide.
    // This prevents a flash when processKey's deferred showOverlay fires
    // in the same frame as submitEntry (the sync hide undoes it immediately).
    this.hideOverlay(cardId);
    this.reset(cardId);

    // Defer mutation PAST the browser paint so hideOverlay()'s DOM change
    // is painted before any synchronous work in onSubmit (Apollo cache reads,
    // optimistic update computation, etc.) blocks the thread.
    const t0 = this._t0;
    requestAnimationFrame(() => {
      console.log(`[OVERLAY +${(performance.now() - t0).toFixed(1)}ms] submitEntry rAF (pre-paint)`);
      setTimeout(async () => {
        console.log(`[OVERLAY +${(performance.now() - t0).toFixed(1)}ms] submitEntry post-paint → onSubmit`);
        perfMonitor.start('card-entry-submit');
        try {
          await handler.onSubmit({
            cardId: handler.cardId,
            count,
            finish,
            special,
            setId: '',
            setCode: '',
            setGroupId: null
          });
          console.log(`[OVERLAY +${(performance.now() - t0).toFixed(1)}ms] submitEntry onSubmit resolved`);
        } catch (error) {
          logger.error('Failed to submit card entry:', error);
        } finally {
          perfMonitor.end('card-entry-submit');
        }
      }, 0);
    });

    console.log(`[OVERLAY ${this._elapsed()}] submitEntry sync done → browser free to paint`);
  }
}

// Export singleton instance
export const globalCardEntry = new GlobalCardEntryHandler();
