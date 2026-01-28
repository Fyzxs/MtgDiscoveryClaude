import { useEffect, useRef, useState } from 'react';

interface UseInViewResult {
  ref: React.RefObject<HTMLElement | null>;
  isInView: boolean;
}

/**
 * Hook that detects when an element enters the viewport using IntersectionObserver.
 * Once the element is in view, it stays marked as visible (one-shot trigger).
 *
 * @param threshold - Percentage of element visibility required to trigger (0-1, default: 0.1)
 * @returns Object with ref to attach to element and isInView boolean state
 *
 * @example
 * const { ref, isInView } = useInView(0.2);
 * return (
 *   <Box ref={ref} sx={{ opacity: isInView ? 1 : 0 }}>
 *     Content that fades in
 *   </Box>
 * );
 */
export function useInView(threshold = 0.1): UseInViewResult {
  const ref = useRef<HTMLElement | null>(null);
  const [isInView, setIsInView] = useState(false);

  useEffect(() => {
    const element = ref.current;
    if (element === null) {
      return;
    }

    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setIsInView(true);
          observer.unobserve(element);
        }
      },
      { threshold }
    );

    observer.observe(element);

    return () => {
      observer.disconnect();
    };
  }, [threshold]);

  return { ref, isInView };
}
