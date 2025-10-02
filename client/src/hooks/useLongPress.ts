import { useCallback, useRef } from 'react';

interface UseLongPressOptions {
  onLongPress: (event: TouchEvent | MouseEvent) => void;
  onClick?: (event: TouchEvent | MouseEvent) => void;
  threshold?: number; // Duration in ms for long press detection
  onStart?: (event: TouchEvent | MouseEvent) => void;
  onFinish?: (event: TouchEvent | MouseEvent) => void;
  onCancel?: (event: TouchEvent | MouseEvent) => void;
}

interface UseLongPressReturn {
  onMouseDown: (event: MouseEvent) => void;
  onMouseUp: (event: MouseEvent) => void;
  onMouseLeave: (event: MouseEvent) => void;
  onTouchStart: (event: TouchEvent) => void;
  onTouchEnd: (event: TouchEvent) => void;
  onTouchCancel: (event: TouchEvent) => void;
}

/**
 * Hook for handling long press gestures on both touch and mouse devices
 * Supports haptic feedback and provides proper touch/mouse event handling
 */
export const useLongPress = ({
  onLongPress,
  onClick,
  threshold = 500,
  onStart,
  onFinish,
  onCancel,
}: UseLongPressOptions): UseLongPressReturn => {
  const isLongPressActive = useRef<boolean>(false);
  const isPressed = useRef<boolean>(false);
  const timerId = useRef<NodeJS.Timeout | null>(null);

  const start = useCallback(
    (event: TouchEvent | MouseEvent) => {
      if (isPressed.current) return;

      isPressed.current = true;
      isLongPressActive.current = false;

      onStart?.(event);

      timerId.current = setTimeout(() => {
        if (isPressed.current) {
          isLongPressActive.current = true;
          onLongPress(event);

          // Trigger haptic feedback on supported devices
          if ('vibrate' in navigator) {
            navigator.vibrate(50); // Short vibration for feedback
          }
        }
      }, threshold);
    },
    [onLongPress, onStart, threshold]
  );

  const clear = useCallback(
    (event: TouchEvent | MouseEvent, shouldTriggerClick = true) => {
      if (timerId.current) {
        clearTimeout(timerId.current);
        timerId.current = null;
      }

      if (isPressed.current) {
        isPressed.current = false;

        if (shouldTriggerClick && !isLongPressActive.current && onClick) {
          onClick(event);
        }

        if (isLongPressActive.current) {
          onFinish?.(event);
        }

        isLongPressActive.current = false;
      }
    },
    [onClick, onFinish]
  );

  const cancel = useCallback(
    (event: TouchEvent | MouseEvent) => {
      if (timerId.current) {
        clearTimeout(timerId.current);
        timerId.current = null;
      }

      if (isPressed.current) {
        isPressed.current = false;
        onCancel?.(event);
        isLongPressActive.current = false;
      }
    },
    [onCancel]
  );

  return {
    onMouseDown: (event: MouseEvent) => start(event),
    onMouseUp: (event: MouseEvent) => clear(event),
    onMouseLeave: (event: MouseEvent) => cancel(event),
    onTouchStart: (event: TouchEvent) => {
      // Prevent mouse events from also firing on touch devices
      event.preventDefault();
      start(event);
    },
    onTouchEnd: (event: TouchEvent) => clear(event),
    onTouchCancel: (event: TouchEvent) => cancel(event),
  };
};