/**
 * Global image cache to track loaded images across component instances
 * This prevents images from disappearing when components are unmounted/remounted
 * during filtering operations
 */
class ImageCache {
  private loadedImages: Set<string> = new Set();
  private loadingImages: Map<string, Promise<void>> = new Map();

  /**
   * Check if an image URL has been loaded
   */
  isLoaded(url: string): boolean {
    return this.loadedImages.has(url);
  }

  /**
   * Mark an image as loaded
   */
  markLoaded(url: string): void {
    this.loadedImages.add(url);
    this.loadingImages.delete(url);
  }

  /**
   * Check if an image is currently loading
   */
  isLoading(url: string): boolean {
    return this.loadingImages.has(url);
  }

  /**
   * Preload an image and track its loading state
   */
  async preload(url: string): Promise<boolean> {
    if (this.isLoaded(url)) {
      return true;
    }

    // If already loading, wait for the existing promise
    if (this.loadingImages.has(url)) {
      try {
        await this.loadingImages.get(url);
        return true;
      } catch {
        return false;
      }
    }

    // Start loading the image
    const loadPromise = new Promise<void>((resolve, reject) => {
      const img = new Image();
      
      img.onload = () => {
        this.markLoaded(url);
        resolve();
      };
      
      img.onerror = () => {
        this.loadingImages.delete(url);
        reject();
      };
      
      img.src = url;

      // Check if already in browser cache
      if (img.complete && img.naturalWidth > 0) {
        this.markLoaded(url);
        resolve();
      }
    });

    this.loadingImages.set(url, loadPromise);

    try {
      await loadPromise;
      return true;
    } catch {
      return false;
    }
  }

  /**
   * Clear the cache (useful for memory management if needed)
   */
  clear(): void {
    this.loadedImages.clear();
    this.loadingImages.clear();
  }
}

// Export a singleton instance
export const imageCache = new ImageCache();