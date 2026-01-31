/**
 * Direct DOM Overlay for Sealed Products
 * Matches directDomOverlay pattern exactly but adapted for sealed products
 * Key difference: Uses data-product-uuid instead of data-card-id
 * Simplified: No finish/special display (sealed products don't have variants)
 */

interface SealedProductEntryState {
  count: string;
  isNegative: boolean;
}

class DirectSealedProductOverlay {
  private overlays = new Map<string, HTMLElement>();
  private styleElement: HTMLStyleElement | null = null;

  constructor() {
    this.injectStyles();
  }

  private injectStyles() {
    if (this.styleElement) return;

    this.styleElement = document.createElement('style');
    this.styleElement.textContent = `
      .direct-dom-overlay-sealed {
        position: absolute !important;
        bottom: 0 !important;
        left: 0 !important;
        right: 0 !important;
        width: 100% !important;
        height: 33% !important;
        background: rgba(0, 0, 0, 0.95) !important;
        backdrop-filter: blur(4px) !important;
        border-top: 2px solid #1976d2 !important;
        z-index: 1000 !important;
        display: flex !important;
        flex-direction: column !important;
        align-items: center !important;
        justify-content: center !important;
        font-family: "Roboto","Helvetica","Arial",sans-serif !important;
        pointer-events: none !important;
        box-sizing: border-box !important;
        opacity: 0 !important;
        visibility: hidden !important;
        will-change: opacity !important;
      }

      .direct-dom-overlay-sealed.visible {
        opacity: 1 !important;
        visibility: visible !important;
        pointer-events: auto !important;
      }

      .direct-dom-overlay-sealed.flash {
        background: rgba(211, 47, 47, 0.95) !important;
        border-top-color: #f44336 !important;
      }

      /* Mode label at top of overlay */
      .dom-overlay-sealed-mode {
        font-size: 0.75rem !important;
        font-weight: 500 !important;
        text-transform: uppercase !important;
        letter-spacing: 0.1em !important;
        color: rgba(255, 255, 255, 0.6) !important;
        margin-bottom: 4px !important;
      }

      .dom-overlay-sealed-mode::after {
        content: 'collection update' !important;
      }

      .dom-overlay-sealed-count {
        font-size: 3rem !important;
        font-weight: bold !important;
        color: #1976d2 !important;
        line-height: 1 !important;
        margin-bottom: 8px !important;
        will-change: contents !important;
      }

      .dom-overlay-sealed-count.negative {
        color: #f44336 !important;
      }

      /* Responsive adjustments for smaller screens */
      @media (max-width: 600px) {
        .dom-overlay-sealed-count {
          font-size: 1.75rem !important;
          margin-bottom: 4px !important;
        }

        .dom-overlay-sealed-mode {
          font-size: 0.65rem !important;
        }
      }

      /* Tablet adjustments */
      @media (min-width: 601px) and (max-width: 900px) {
        .dom-overlay-sealed-count {
          font-size: 2.25rem !important;
          margin-bottom: 6px !important;
        }

        .dom-overlay-sealed-mode {
          font-size: 0.7rem !important;
        }
      }
    `;
    document.head.appendChild(this.styleElement);
  }

  // Pre-create overlay for instant show
  ensureOverlay(productUuid: string) {
    if (!this.overlays.has(productUuid)) {
      try {
        const overlay = this.createOverlay(productUuid);
        this.overlays.set(productUuid, overlay);
      } catch {
        // Product might not be ready yet
      }
    }
  }

  show(productUuid: string, entryState: SealedProductEntryState, invalidFlash = false) {
    // Get overlay (should already exist!)
    let overlay = this.overlays.get(productUuid);
    if (!overlay) {
      // Fallback: create if somehow missing
      overlay = this.createOverlay(productUuid);
      this.overlays.set(productUuid, overlay);
    }

    // Update content
    this.updateOverlay(overlay, entryState, invalidFlash);

    // Show immediately
    overlay.classList.add('visible');
  }

  hide(productUuid: string) {
    const overlay = this.overlays.get(productUuid);
    if (overlay) {
      overlay.classList.remove('visible');
    }
  }

  flash(productUuid: string) {
    const overlay = this.overlays.get(productUuid);
    if (overlay) {
      overlay.classList.add('flash');
      setTimeout(() => overlay.classList.remove('flash'), 150);
    }
  }

  private createOverlay(productUuid: string): HTMLElement {
    const productElement = document.querySelector(`[data-product-uuid="${productUuid}"]`);
    if (!productElement) {
      throw new Error(`Product element not found: ${productUuid}`);
    }

    const overlay = document.createElement('div');
    overlay.className = 'direct-dom-overlay-sealed';
    overlay.dataset.overlayFor = productUuid;
    // CRITICAL: Set position absolute inline to prevent flexbox layout shift
    overlay.style.position = 'absolute';

    // Create mode label at top
    const modeLabel = document.createElement('div');
    modeLabel.className = 'dom-overlay-sealed-mode';
    overlay.appendChild(modeLabel);

    // Create count display
    const count = document.createElement('div');
    count.className = 'dom-overlay-sealed-count';
    overlay.appendChild(count);

    // Append to product
    productElement.appendChild(overlay);

    return overlay;
  }

  private updateOverlay(overlay: HTMLElement, entryState: SealedProductEntryState, invalidFlash: boolean) {
    const count = overlay.querySelector('.dom-overlay-sealed-count') as HTMLElement;

    if (count) {
      const displayCount = entryState.isNegative
        ? `-${entryState.count || '0'}`
        : entryState.count || '0';
      count.textContent = displayCount;
      count.classList.toggle('negative', entryState.isNegative);
    }

    if (invalidFlash) {
      this.flash(overlay.dataset.overlayFor!);
    }
  }

  update(productUuid: string, state: SealedProductEntryState): void {
    const overlay = this.overlays.get(productUuid);
    if (!overlay) return;

    this.updateOverlay(overlay, state, false);
  }

  cleanup(productUuid: string) {
    const overlay = this.overlays.get(productUuid);
    if (overlay) {
      overlay.remove();
      this.overlays.delete(productUuid);
    }
  }

  cleanupAll() {
    this.overlays.forEach(overlay => overlay.remove());
    this.overlays.clear();
  }
}

// Singleton instance
export const sealedProductOverlay = new DirectSealedProductOverlay();
