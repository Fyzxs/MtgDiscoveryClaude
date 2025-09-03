import { useState, useEffect, useRef, useCallback } from 'react';

interface ContainerDimensions {
  width: number;
  height: number;
}

export function useContainerDimensions(defaultHeight: number = 800) {
  const [dimensions, setDimensions] = useState<ContainerDimensions>({ 
    width: 1200, 
    height: defaultHeight 
  });
  const containerRef = useRef<HTMLDivElement>(null);

  const updateDimensions = useCallback(() => {
    if (containerRef.current) {
      const rect = containerRef.current.getBoundingClientRect();
      setDimensions({
        width: rect.width,
        height: Math.max(defaultHeight, window.innerHeight - rect.top - 100) // Leave some bottom padding
      });
    }
  }, [defaultHeight]);

  useEffect(() => {
    // Initial measurement
    updateDimensions();

    // Set up resize observer for more accurate container size tracking
    const resizeObserver = new ResizeObserver(updateDimensions);
    
    if (containerRef.current) {
      resizeObserver.observe(containerRef.current);
    }

    // Also listen to window resize as fallback
    window.addEventListener('resize', updateDimensions);

    return () => {
      resizeObserver.disconnect();
      window.removeEventListener('resize', updateDimensions);
    };
  }, [updateDimensions]);

  return {
    containerRef,
    dimensions
  };
}