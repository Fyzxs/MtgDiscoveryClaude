import { useState, useEffect, useRef, useCallback } from 'react';

interface ContainerDimensions {
  width: number;
  height: number;
  measured: boolean; // Track whether these are measured or estimated dimensions
}

export function useContainerDimensions(defaultHeight: number = 800) {
  // Use a more intelligent initial width based on viewport
  const getInitialWidth = () => {
    if (typeof window !== 'undefined') {
      const viewportWidth = window.innerWidth;
      
      // Be more aggressive with width estimation to avoid the 4-card problem
      // Account for various container constraints but err on the side of being too wide
      let estimatedWidth;
      
      if (viewportWidth >= 1400) {
        // Large screens: be generous, containers are usually wide
        estimatedWidth = viewportWidth * 0.95;
      } else if (viewportWidth >= 1000) {
        // Medium screens: account for some padding but stay wide
        estimatedWidth = viewportWidth * 0.92;
      } else {
        // Smaller screens: use most available space
        estimatedWidth = viewportWidth * 0.88;
      }
      
      // For card grids, it's better to estimate too wide than too narrow
      // This prevents the jarring 4-card → full-width transition
      return Math.round(Math.max(estimatedWidth, 1000));
    }
    return 1400; // Higher fallback for SSR to avoid 4-card scenario
  };

  const [dimensions, setDimensions] = useState<ContainerDimensions>({ 
    width: getInitialWidth(), 
    height: defaultHeight,
    measured: false
  });
  const containerRef = useRef<HTMLDivElement>(null);

  const updateDimensions = useCallback(() => {
    if (containerRef.current) {
      const rect = containerRef.current.getBoundingClientRect();
      setDimensions({
        width: rect.width,
        height: Math.max(defaultHeight, window.innerHeight - rect.top - 100), // Leave some bottom padding
        measured: true
      });
    }
  }, [defaultHeight]);

  useEffect(() => {
    // Delay initial measurement slightly to ensure DOM is fully laid out
    const measurementTimer = setTimeout(() => {
      updateDimensions();
    }, 10);

    // Set up resize observer for more accurate container size tracking
    const resizeObserver = new ResizeObserver(updateDimensions);
    
    if (containerRef.current) {
      resizeObserver.observe(containerRef.current);
    }

    // Also listen to window resize as fallback
    window.addEventListener('resize', updateDimensions);

    return () => {
      clearTimeout(measurementTimer);
      resizeObserver.disconnect();
      window.removeEventListener('resize', updateDimensions);
    };
  }, [updateDimensions]);

  return {
    containerRef,
    dimensions
  };
}