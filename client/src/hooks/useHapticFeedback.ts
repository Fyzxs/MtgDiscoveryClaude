import { useCallback, useRef } from 'react';

export type HapticFeedbackType =
  | 'light'
  | 'medium'
  | 'heavy'
  | 'selection'
  | 'impact'
  | 'notification'
  | 'success'
  | 'warning'
  | 'error';

interface HapticEngine {
  impactOccurred?: (intensity: number) => void;
  selectionChanged?: () => void;
  notificationOccurred?: (type: number) => void;
}

interface WindowWithHaptics extends Window {
  hapticFeedback?: HapticEngine;
}

interface UseHapticFeedbackOptions {
  enabled?: boolean;
  // Fallback vibration patterns for devices that don't support advanced haptics
  vibrationPatterns?: {
    [key in HapticFeedbackType]?: number | number[];
  };
}

interface UseHapticFeedbackReturn {
  triggerHaptic: (type: HapticFeedbackType) => void;
  isSupported: boolean;
  isEnabled: boolean;
  setEnabled: (enabled: boolean) => void;
}

/**
 * Hook for handling haptic feedback across different devices and platforms
 * Supports both modern Haptic API and fallback vibration patterns
 */
export const useHapticFeedback = ({
  enabled = true,
  vibrationPatterns = {
    light: 10,
    medium: 20,
    heavy: 30,
    selection: 10,
    impact: 25,
    notification: [50, 50, 50],
    success: [100, 50, 100],
    warning: [50, 50, 50, 50, 50],
    error: [100, 100, 100],
  },
}: UseHapticFeedbackOptions = {}): UseHapticFeedbackReturn => {
  const isEnabledRef = useRef(enabled);

  // Check for haptic feedback support
  const isSupported = useCallback(() => {
    // Check for modern Haptic API (iOS Safari)
    if ('hapticFeedback' in window) {
      return true;
    }

    // Check for vibration API (Android and some desktop browsers)
    if ('vibrate' in navigator) {
      return true;
    }

    return false;
  }, []);

  const triggerModernHaptic = useCallback((type: HapticFeedbackType) => {
    // Modern Haptic API (primarily iOS Safari)
    const hapticEngine = (window as WindowWithHaptics).hapticFeedback;

    if (!hapticEngine) return false;

    try {
      switch (type) {
        case 'light':
          hapticEngine.impactOccurred?.(0); // Light impact
          break;
        case 'medium':
          hapticEngine.impactOccurred?.(1); // Medium impact
          break;
        case 'heavy':
          hapticEngine.impactOccurred?.(2); // Heavy impact
          break;
        case 'selection':
          hapticEngine.selectionChanged?.();
          break;
        case 'success':
          hapticEngine.notificationOccurred?.(0); // Success notification
          break;
        case 'warning':
          hapticEngine.notificationOccurred?.(1); // Warning notification
          break;
        case 'error':
          hapticEngine.notificationOccurred?.(2); // Error notification
          break;
        case 'impact':
        case 'notification':
        default:
          hapticEngine.impactOccurred?.(1); // Default to medium impact
          break;
      }
      return true;
    } catch (error) {
      console.warn('Haptic feedback failed:', error);
      return false;
    }
  }, []);

  const triggerVibration = useCallback((type: HapticFeedbackType) => {
    if (!('vibrate' in navigator)) return false;

    try {
      const pattern = vibrationPatterns[type];
      if (pattern !== undefined) {
        navigator.vibrate(pattern);
        return true;
      }
    } catch (error) {
      console.warn('Vibration failed:', error);
    }

    return false;
  }, [vibrationPatterns]);

  const triggerHaptic = useCallback((type: HapticFeedbackType) => {
    if (!isEnabledRef.current) return;

    // Try modern haptic API first
    if (triggerModernHaptic(type)) {
      return;
    }

    // Fallback to vibration API
    triggerVibration(type);
  }, [triggerModernHaptic, triggerVibration]);

  const setEnabled = useCallback((enabled: boolean) => {
    isEnabledRef.current = enabled;
  }, []);

  return {
    triggerHaptic,
    isSupported: isSupported(),
    isEnabled: isEnabledRef.current,
    setEnabled,
  };
};