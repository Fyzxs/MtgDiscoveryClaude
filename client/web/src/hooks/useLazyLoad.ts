import { useEffect, useRef, useState } from 'react';

interface UseLazyLoadOptions {
  rootMargin?: string;
  threshold?: number | number[];
  enabled?: boolean;
}

export function useLazyLoad(options: UseLazyLoadOptions = {}) {
  const {
    rootMargin = '50px', // Start loading 50px before the element enters viewport
    threshold = 0.01,
    enabled = true
  } = options;

  const [isInView, setIsInView] = useState(false);
  const [hasBeenInView, setHasBeenInView] = useState(false);
  const elementRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!enabled) {
      setIsInView(true);
      setHasBeenInView(true);
      return;
    }

    const element = elementRef.current;
    if (!element) return;

    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            setIsInView(true);
            if (!hasBeenInView) {
              setHasBeenInView(true);
              // Once loaded, we can disconnect the observer
              observer.disconnect();
            }
          } else {
            setIsInView(false);
          }
        });
      },
      {
        rootMargin,
        threshold
      }
    );

    observer.observe(element);

    return () => {
      observer.disconnect();
    };
  }, [enabled, rootMargin, threshold, hasBeenInView]);

  return {
    ref: elementRef,
    isInView,
    hasBeenInView
  };
}