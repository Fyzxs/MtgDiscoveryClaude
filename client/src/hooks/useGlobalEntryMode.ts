import { useState, useEffect } from 'react';

// Simple global state for tracking if any card is entering
let globalEntryMode = false;
const listeners = new Set<(isEntering: boolean) => void>();

function setGlobalEntryMode(isEntering: boolean) {
  if (globalEntryMode !== isEntering) {
    globalEntryMode = isEntering;
    listeners.forEach(listener => listener(isEntering));
  }
}

function subscribeToGlobalEntryMode(listener: (isEntering: boolean) => void) {
  listeners.add(listener);
  // Immediately call with current state
  listener(globalEntryMode);

  return () => {
    listeners.delete(listener);
  };
}

// Hook for components that need to know about global entry mode
export function useGlobalEntryMode() {
  const [isEntering, setIsEntering] = useState(globalEntryMode);

  useEffect(() => {
    return subscribeToGlobalEntryMode(setIsEntering);
  }, []);

  return isEntering;
}

// Hook for components that need to set global entry mode
export function useGlobalEntryModeController() {
  return {
    setGlobalEntryMode,
    isEntering: globalEntryMode
  };
}