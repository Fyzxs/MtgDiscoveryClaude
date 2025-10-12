class DirectDomHelpPanel {
  private panel: HTMLElement | null = null;
  private styleElement: HTMLStyleElement | null = null;

  constructor() {
    this.injectStyles();
    this.createPanel();
  }

  private injectStyles() {
    if (this.styleElement) return;

    this.styleElement = document.createElement('style');
    this.styleElement.textContent = `
      .direct-help-panel {
        position: fixed !important;
        bottom: 16px !important;
        right: 16px !important;
        padding: 16px !important;
        background-color: rgba(33, 33, 33, 0.98) !important;
        border: 1px solid #1976d2 !important;
        border-radius: 4px !important;
        box-shadow: 0 8px 16px rgba(0,0,0,0.4) !important;
        z-index: 1300 !important;
        min-width: 240px !important;
        font-family: "Roboto","Helvetica","Arial",sans-serif !important;
        display: none !important;
      }

      .direct-help-panel.visible {
        display: block !important;
      }

      .help-panel-title {
        color: #1976d2 !important;
        font-weight: bold !important;
        margin-bottom: 12px !important;
        text-align: center !important;
        border-bottom: 1px solid rgba(255,255,255,0.12) !important;
        padding-bottom: 8px !important;
        font-size: 0.875rem !important;
      }

      .help-panel-items {
        display: flex !important;
        flex-direction: column !important;
        gap: 4px !important;
      }

      .help-panel-item {
        display: flex !important;
        justify-content: space-between !important;
        align-items: center !important;
        padding: 2px 0 !important;
      }

      .help-panel-key {
        font-family: monospace !important;
        color: #29b6f6 !important;
        font-weight: 500 !important;
        min-width: 60px !important;
        font-size: 0.875rem !important;
      }

      .help-panel-desc {
        color: rgba(255,255,255,0.7) !important;
        margin-left: 16px !important;
        font-size: 0.875rem !important;
      }
    `;
    document.head.appendChild(this.styleElement);
  }

  private createPanel() {
    if (this.panel) return;

    this.panel = document.createElement('div');
    this.panel.className = 'direct-help-panel';
    this.panel.innerHTML = `
      <div class="help-panel-title">Quick Entry Keys</div>
      <div class="help-panel-items">
        <div class="help-panel-item">
          <span class="help-panel-key">0-9</span>
          <span class="help-panel-desc">Quantity</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">+/\`</span>
          <span class="help-panel-desc">Increment</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">-/~</span>
          <span class="help-panel-desc">Decrement</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">Z/N</span>
          <span class="help-panel-desc">Non-foil</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">F/O</span>
          <span class="help-panel-desc">Foil</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">E/H</span>
          <span class="help-panel-desc">Etched</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">G/I</span>
          <span class="help-panel-desc">Signed</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">R/P</span>
          <span class="help-panel-desc">Artist Proof</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">T/M</span>
          <span class="help-panel-desc">Modified/Altered</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">X</span>
          <span class="help-panel-desc">Negate Quantity</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">Enter</span>
          <span class="help-panel-desc">Confirm</span>
        </div>
        <div class="help-panel-item">
          <span class="help-panel-key">Esc</span>
          <span class="help-panel-desc">Cancel</span>
        </div>
      </div>
    `;

    document.body.appendChild(this.panel);
  }

  show() {
    if (this.panel) {
      this.panel.classList.add('visible');
    }
  }

  hide() {
    if (this.panel) {
      this.panel.classList.remove('visible');
    }
  }
}

// Singleton instance
export const domHelpPanel = new DirectDomHelpPanel();