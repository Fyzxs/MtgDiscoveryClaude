import type { CollectionEntryState } from '../types/collection';
import { logger } from './logger';
import { FINISH_DISPLAY_NAMES, SPECIAL_DISPLAY_NAMES } from '../types/collection';

class DirectDomOverlay {
  private overlays = new Map<string, HTMLElement>();
  private styleElement: HTMLStyleElement | null = null;

  constructor() {
    this.injectStyles();
  }

  private injectStyles() {
    if (this.styleElement) return;

    this.styleElement = document.createElement('style');
    this.styleElement.textContent = `
      .direct-dom-overlay {
        position: absolute !important;
        bottom: 0 !important;
        left: 0 !important;
        right: 0 !important;
        width: 100% !important;
        height: 33% !important;
        padding: 4px !important;
        background: rgba(0, 0, 0, 0.95) !important;
        backdrop-filter: blur(4px) !important;
        border-top: 1px solid #1976d2 !important;
        z-index: 1000 !important;
        display: flex !important;
        flex-direction: column !important;
        align-items: center !important;
        justify-content: space-between !important;
        font-family: "Roboto","Helvetica","Arial",sans-serif !important;
        pointer-events: none !important;
        box-sizing: border-box !important;
        opacity: 0 !important;
        visibility: hidden !important;
        will-change: opacity !important;
      }

      .direct-dom-overlay.visible {
        opacity: 1 !important;
        visibility: visible !important;
        pointer-events: auto !important;
      }

      .direct-dom-overlay.flash {
        background: rgba(211, 47, 47, 0.95) !important;
        border-top-color: #f44336 !important;
      }

      /* Mode label at top of overlay */
      .dom-overlay-mode {
        font-size: 0.5rem !important;
        font-weight: 500 !important;
        text-transform: uppercase !important;
        letter-spacing: 0.05em !important;
        color: rgba(255, 255, 255, 0.6) !important;
        text-align: center !important;
        width: 100% !important;
        flex-shrink: 0 !important;
        line-height: 1 !important;
      }

      .dom-overlay-mode::after {
        content: 'collection update' !important;
      }

      body[data-entry-mode="wishlist"] .dom-overlay-mode::after {
        content: '♡ Wishlist ♡' !important;
      }

      body[data-entry-mode="wishlist"] .dom-overlay-mode {
        color: #f48fb1 !important;
      }

      /* Wishlist mode styling */
      body[data-entry-mode="wishlist"] .direct-dom-overlay {
        background: rgba(45, 20, 45, 0.95) !important;
        border-top: 2px solid #e91e63 !important;
      }

      body[data-entry-mode="wishlist"] .dom-overlay-count {
        color: #e91e63 !important;
      }

      body[data-entry-mode="wishlist"] .dom-overlay-count.negative {
        color: #f44336 !important;
      }

      /* Main content container - side by side layout */
      .dom-overlay-main {
        display: flex !important;
        flex-direction: row !important;
        align-items: stretch !important;
        justify-content: space-between !important;
        width: 100% !important;
        flex: 1 1 auto !important;
        gap: 4px !important;
      }

      /* Count on the right - takes full height */
      .dom-overlay-count {
        font-size: 2rem !important;
        font-weight: bold !important;
        color: #1976d2 !important;
        line-height: 1 !important;
        display: flex !important;
        align-items: center !important;
        justify-content: center !important;
        flex-shrink: 1 !important;
        min-width: 0 !important;
        will-change: contents !important;
      }

      .dom-overlay-count.negative {
        color: #f44336 !important;
      }

      /* Details on the left */
      .dom-overlay-details {
        display: flex !important;
        flex-direction: column !important;
        align-items: flex-start !important;
        justify-content: center !important;
        gap: 2px !important;
        flex: 0 0 auto !important;
      }

      .dom-overlay-finish {
        font-size: 0.7rem !important;
        font-weight: 500 !important;
        color: rgba(255, 255, 255, 0.87) !important;
        line-height: 1.2 !important;
      }

      .dom-overlay-divider {
        display: none !important;
      }

      .dom-overlay-special {
        font-size: 0.7rem !important;
        font-weight: 500 !important;
        color: #ff9800 !important;
        line-height: 1.2 !important;
      }

      /* Larger cards (regular card view) */
      @media (min-width: 600px) {
        .dom-overlay-mode {
          font-size: 0.65rem !important;
        }

        .dom-overlay-count {
          font-size: 3rem !important;
        }

        .dom-overlay-finish {
          font-size: 0.9rem !important;
        }

        .dom-overlay-special {
          font-size: 0.9rem !important;
        }
      }

      /* Desktop large cards */
      @media (min-width: 1200px) {
        .dom-overlay-mode {
          font-size: 0.75rem !important;
        }

        .dom-overlay-count {
          font-size: 4rem !important;
        }

        .dom-overlay-finish {
          font-size: 1.1rem !important;
        }

        .dom-overlay-special {
          font-size: 1.1rem !important;
        }
      }
    `;
    document.head.appendChild(this.styleElement);
  }

  // Pre-create overlay for instant show
  ensureOverlay(cardId: string) {
    if (!this.overlays.has(cardId)) {
      try {
        const overlay = this.createOverlay(cardId);
        this.overlays.set(cardId, overlay);
      } catch {
        // Card might not be ready yet
      }
    }
  }

  show(cardId: string, entryState: CollectionEntryState, invalidFlash = false) {
    // Get overlay (should already exist!)
    let overlay = this.overlays.get(cardId);
    if (!overlay) {
      // Fallback: create if somehow missing
      overlay = this.createOverlay(cardId);
      this.overlays.set(cardId, overlay);
    }

    // Update content
    this.updateOverlay(overlay, entryState, invalidFlash);

    // Show immediately
    overlay.classList.add('visible');
  }

  hide(cardId: string) {
    const overlay = this.overlays.get(cardId);
    if (overlay) {
      const start = performance.now();
      // PERFORMANCE: Opacity/visibility change - GPU-accelerated, no layout shift
      overlay.classList.remove('visible');
      const duration = performance.now() - start;
      if (duration > 1) {
        logger.warn(`[DirectDomOverlay] Hide took ${duration.toFixed(2)}ms for card: ${cardId}`);
      }
    }
  }

  flash(cardId: string) {
    const overlay = this.overlays.get(cardId);
    if (overlay) {
      overlay.classList.add('flash');
      setTimeout(() => overlay.classList.remove('flash'), 150);
    }
  }

  private createOverlay(cardId: string): HTMLElement {
    const cardElement = document.querySelector(`[data-card-id="${cardId}"]`);
    if (!cardElement) {
      throw new Error(`Card element not found: ${cardId}`);
    }

    const overlay = document.createElement('div');
    overlay.className = 'direct-dom-overlay';
    overlay.dataset.overlayFor = cardId;

    // Create mode label at top
    const modeLabel = document.createElement('div');
    modeLabel.className = 'dom-overlay-mode';
    overlay.appendChild(modeLabel);

    // Create main content container (side by side)
    const mainContent = document.createElement('div');
    mainContent.className = 'dom-overlay-main';

    // Left side: details container
    const details = document.createElement('div');
    details.className = 'dom-overlay-details';

    const finish = document.createElement('span');
    finish.className = 'dom-overlay-finish';
    details.appendChild(finish);

    const divider = document.createElement('div');
    divider.className = 'dom-overlay-divider';
    divider.style.display = 'none';
    details.appendChild(divider);

    const special = document.createElement('span');
    special.className = 'dom-overlay-special';
    special.style.visibility = 'hidden';
    details.appendChild(special);

    mainContent.appendChild(details);

    // Right side: count display
    const count = document.createElement('div');
    count.className = 'dom-overlay-count';
    mainContent.appendChild(count);

    overlay.appendChild(mainContent);

    // Append to card
    cardElement.appendChild(overlay);

    return overlay;
  }

  private updateOverlay(overlay: HTMLElement, entryState: CollectionEntryState, invalidFlash: boolean) {
    const count = overlay.querySelector('.dom-overlay-count') as HTMLElement;
    const finish = overlay.querySelector('.dom-overlay-finish') as HTMLElement;
    const special = overlay.querySelector('.dom-overlay-special') as HTMLElement;

    if (count) {
      const displayCount = entryState.isNegative
        ? `-${entryState.count || '0'}`
        : entryState.count || '0';
      count.textContent = displayCount;
      count.classList.toggle('negative', entryState.isNegative);
    }

    if (finish) {
      finish.textContent = FINISH_DISPLAY_NAMES[entryState.finish];
    }

    if (special) {
      const specialText = SPECIAL_DISPLAY_NAMES[entryState.special];
      if (specialText) {
        special.textContent = specialText;
        special.style.visibility = 'visible';
      } else {
        special.textContent = '';
        special.style.visibility = 'hidden';
      }
    }

    if (invalidFlash) {
      this.flash(overlay.dataset.overlayFor!);
    }
  }

  cleanup(cardId: string) {
    const overlay = this.overlays.get(cardId);
    if (overlay) {
      overlay.remove();
      this.overlays.delete(cardId);
    }
  }

  cleanupAll() {
    this.overlays.forEach(overlay => overlay.remove());
    this.overlays.clear();
  }
}

// Singleton instance
export const domOverlay = new DirectDomOverlay();