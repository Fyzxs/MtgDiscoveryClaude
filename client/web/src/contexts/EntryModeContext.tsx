import React, { createContext, useContext, useState, useCallback, useEffect } from 'react';
import type { EntryMode } from '../types/collection';

interface EntryModeContextValue {
  mode: EntryMode;
  toggleMode: () => void;
  setMode: (mode: EntryMode) => void;
  isWishlistMode: boolean;
  isCollectionMode: boolean;
}

const EntryModeContext = createContext<EntryModeContextValue | undefined>(undefined);

// eslint-disable-next-line react-refresh/only-export-components -- Standard pattern: export hook with Provider
export const useEntryMode = () => {
  const context = useContext(EntryModeContext);
  if (!context) {
    throw new Error('useEntryMode must be used within EntryModeProvider');
  }
  return context;
};

interface EntryModeProviderProps {
  children: React.ReactNode;
}

export const EntryModeProvider: React.FC<EntryModeProviderProps> = ({ children }) => {
  const [mode, setModeState] = useState<EntryMode>('collection');

  const toggleMode = useCallback(() => {
    setModeState(current => current === 'collection' ? 'wishlist' : 'collection');
  }, []);

  const setMode = useCallback((newMode: EntryMode) => {
    setModeState(newMode);
  }, []);

  // Sync mode to DOM for CSS-based styling (overlay uses this)
  useEffect(() => {
    document.body.setAttribute('data-entry-mode', mode);
  }, [mode]);

  // Global keyboard listener for Q and Y keys to toggle mode
  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      // Don't toggle if user is typing in an input, textarea, or contenteditable
      const target = event.target as HTMLElement;
      if (
        target.tagName === 'INPUT' ||
        target.tagName === 'TEXTAREA' ||
        target.isContentEditable
      ) {
        return;
      }

      // Q or Y keys toggle between modes
      const key = event.key.toLowerCase();
      if (key === 'q' || key === 'y') {
        event.preventDefault();
        toggleMode();
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [toggleMode]);

  const value: EntryModeContextValue = {
    mode,
    toggleMode,
    setMode,
    isWishlistMode: mode === 'wishlist',
    isCollectionMode: mode === 'collection'
  };

  return (
    <EntryModeContext.Provider value={value}>
      {children}
    </EntryModeContext.Provider>
  );
};
