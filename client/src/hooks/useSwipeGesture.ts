import { useCallback, useRef } from 'react';

export type SwipeDirection = 'left' | 'right' | 'up' | 'down';

interface UseSwipeGestureOptions {
  onSwipe?: (direction: SwipeDirection, distance: number, velocity: number) => void;
  onSwipeLeft?: (distance: number, velocity: number) => void;
  onSwipeRight?: (distance: number, velocity: number) => void;
  onSwipeUp?: (distance: number, velocity: number) => void;
  onSwipeDown?: (distance: number, velocity: number) => void;
  onSwipeStart?: (startX: number, startY: number) => void;
  onSwipeMove?: (currentX: number, currentY: number, deltaX: number, deltaY: number) => void;
  onSwipeEnd?: () => void;
  threshold?: number; // Minimum distance in pixels to register as swipe
  velocityThreshold?: number; // Minimum velocity to register as swipe
  preventDefaultTouchmove?: boolean;
  trackMouse?: boolean; // Whether to track mouse events as well
}

interface SwipeState {
  startX: number;
  startY: number;
  startTime: number;
  isSwiping: boolean;
}

interface UseSwipeGestureReturn {
  onTouchStart: (event: TouchEvent) => void;
  onTouchMove: (event: TouchEvent) => void;
  onTouchEnd: (event: TouchEvent) => void;
  onTouchCancel: (event: TouchEvent) => void;
  onMouseDown?: (event: MouseEvent) => void;
  onMouseMove?: (event: MouseEvent) => void;
  onMouseUp?: (event: MouseEvent) => void;
  onMouseLeave?: (event: MouseEvent) => void;
}

/**
 * Hook for handling swipe gestures with support for velocity and direction detection
 * Supports both touch and mouse events for broader device compatibility
 */
export const useSwipeGesture = ({
  onSwipe,
  onSwipeLeft,
  onSwipeRight,
  onSwipeUp,
  onSwipeDown,
  onSwipeStart,
  onSwipeMove,
  onSwipeEnd,
  threshold = 50,
  velocityThreshold = 0.3,
  preventDefaultTouchmove = false,
  trackMouse = false,
}: UseSwipeGestureOptions): UseSwipeGestureReturn => {
  const swipeState = useRef<SwipeState>({
    startX: 0,
    startY: 0,
    startTime: 0,
    isSwiping: false,
  });

  const getSwipeDirection = useCallback((deltaX: number, deltaY: number): SwipeDirection => {
    const absDeltaX = Math.abs(deltaX);
    const absDeltaY = Math.abs(deltaY);

    if (absDeltaX > absDeltaY) {
      return deltaX > 0 ? 'right' : 'left';
    } else {
      return deltaY > 0 ? 'down' : 'up';
    }
  }, []);

  const handleStart = useCallback(
    (clientX: number, clientY: number) => {
      swipeState.current = {
        startX: clientX,
        startY: clientY,
        startTime: Date.now(),
        isSwiping: true,
      };

      onSwipeStart?.(clientX, clientY);
    },
    [onSwipeStart]
  );

  const handleMove = useCallback(
    (clientX: number, clientY: number) => {
      if (!swipeState.current.isSwiping) return;

      const deltaX = clientX - swipeState.current.startX;
      const deltaY = clientY - swipeState.current.startY;

      onSwipeMove?.(clientX, clientY, deltaX, deltaY);
    },
    [onSwipeMove]
  );

  const handleEnd = useCallback(
    (clientX: number, clientY: number) => {
      if (!swipeState.current.isSwiping) return;

      const deltaX = clientX - swipeState.current.startX;
      const deltaY = clientY - swipeState.current.startY;
      const deltaTime = Date.now() - swipeState.current.startTime;

      const distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
      const velocity = distance / deltaTime; // pixels per ms

      swipeState.current.isSwiping = false;
      onSwipeEnd?.();

      // Check if swipe meets threshold requirements
      if (distance >= threshold && velocity >= velocityThreshold) {
        const direction = getSwipeDirection(deltaX, deltaY);

        onSwipe?.(direction, distance, velocity * 1000); // Convert to pixels per second

        // Call direction-specific handlers
        switch (direction) {
          case 'left':
            onSwipeLeft?.(distance, velocity * 1000);
            break;
          case 'right':
            onSwipeRight?.(distance, velocity * 1000);
            break;
          case 'up':
            onSwipeUp?.(distance, velocity * 1000);
            break;
          case 'down':
            onSwipeDown?.(distance, velocity * 1000);
            break;
        }
      }
    },
    [threshold, velocityThreshold, getSwipeDirection, onSwipe, onSwipeLeft, onSwipeRight, onSwipeUp, onSwipeDown, onSwipeEnd]
  );

  const handleCancel = useCallback(() => {
    swipeState.current.isSwiping = false;
    onSwipeEnd?.();
  }, [onSwipeEnd]);

  // Touch event handlers
  const touchHandlers = {
    onTouchStart: (event: TouchEvent) => {
      if (event.touches.length === 1) {
        const touch = event.touches[0];
        handleStart(touch.clientX, touch.clientY);
      }
    },
    onTouchMove: (event: TouchEvent) => {
      if (preventDefaultTouchmove) {
        event.preventDefault();
      }

      if (event.touches.length === 1 && swipeState.current.isSwiping) {
        const touch = event.touches[0];
        handleMove(touch.clientX, touch.clientY);
      }
    },
    onTouchEnd: (event: TouchEvent) => {
      if (event.changedTouches.length === 1) {
        const touch = event.changedTouches[0];
        handleEnd(touch.clientX, touch.clientY);
      }
    },
    onTouchCancel: () => {
      handleCancel();
    },
  };

  // Mouse event handlers (optional)
  const mouseHandlers = trackMouse ? {
    onMouseDown: (event: MouseEvent) => {
      handleStart(event.clientX, event.clientY);
    },
    onMouseMove: (event: MouseEvent) => {
      if (swipeState.current.isSwiping) {
        handleMove(event.clientX, event.clientY);
      }
    },
    onMouseUp: (event: MouseEvent) => {
      handleEnd(event.clientX, event.clientY);
    },
    onMouseLeave: () => {
      handleCancel();
    },
  } : {};

  return {
    ...touchHandlers,
    ...mouseHandlers,
  };
};