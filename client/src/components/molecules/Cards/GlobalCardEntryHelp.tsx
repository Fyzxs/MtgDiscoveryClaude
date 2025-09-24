import React from 'react';
import { CardEntryHelpPanel } from './CardEntryHelpPanel';
import { useGlobalEntryMode } from '../../../hooks/useGlobalEntryMode';

export const GlobalCardEntryHelp: React.FC = () => {
  const isVisible = useGlobalEntryMode();

  return <CardEntryHelpPanel isVisible={isVisible} />;
};