import React from 'react';
import { Tooltip } from '@mui/material';
import type { CollectionItem } from '../types/card';

// Emoji definitions with tooltips for accessibility
const EMOJI_TOOLTIPS = {
  '📄': 'Nonfoil',
  '✨': 'Foil',
  '🌟': 'Etched',
  '📜': 'Artist Proof',
  '✍️': 'Signed',
  '🎨': 'Altered'
} as const;

// Helper component to wrap emoji with tooltip
const EmojiWithTooltip: React.FC<{ emoji: keyof typeof EMOJI_TOOLTIPS; children: React.ReactNode }> = ({ emoji, children }) => (
  <Tooltip title={EMOJI_TOOLTIPS[emoji]} arrow placement="top">
    <span role="img" aria-label={EMOJI_TOOLTIPS[emoji]} style={{ cursor: 'help' }}>
      {children}
    </span>
  </Tooltip>
);

export function formatCollectionInline(collection: CollectionItem[]): React.ReactNode {
  const totalCards = collection.reduce((sum, item) => sum + item.count, 0);

  if (totalCards === 0) {
    return '[0]';
  }

  // Group by finish type and check for multiple finishes
  const finishGroups = collection.reduce((acc, item) => {
    if (!acc[item.finish]) acc[item.finish] = [];
    acc[item.finish].push(item);
    return acc;
  }, {} as Record<string, CollectionItem[]>);

  const finishTypes = Object.keys(finishGroups);
  const hasMultipleFinishes = finishTypes.length > 1;

  // Group by special type and check for specials
  const specialTypes = new Set(collection.filter(item => item.special !== 'none').map(item => item.special));
  const hasSpecials = specialTypes.size > 0;

  // Get finish indicators (always show finish types)
  const getFinishIndicators = () => {
    const indicators: JSX.Element[] = [];
    if (finishTypes.includes('nonfoil')) {
      indicators.push(<EmojiWithTooltip key="nonfoil" emoji="📄">📄</EmojiWithTooltip>);
    }
    if (finishTypes.includes('foil')) {
      indicators.push(<EmojiWithTooltip key="foil" emoji="✨">✨</EmojiWithTooltip>);
    }
    if (finishTypes.includes('etched')) {
      indicators.push(<EmojiWithTooltip key="etched" emoji="🌟">🌟</EmojiWithTooltip>);
    }
    return indicators.length > 0 ? <>{indicators}</> : null;
  };

  // Get special indicators (always show if any special types exist)
  const getSpecialIndicators = () => {
    if (!hasSpecials) return null;
    const indicators: JSX.Element[] = [];
    // Order: 📜 → ✍️ → 🎨
    if (specialTypes.has('proof')) {
      indicators.push(<EmojiWithTooltip key="proof" emoji="📜">📜</EmojiWithTooltip>);
    }
    if (specialTypes.has('signed')) {
      indicators.push(<EmojiWithTooltip key="signed" emoji="✍️">✍️</EmojiWithTooltip>);
    }
    if (specialTypes.has('altered')) {
      indicators.push(<EmojiWithTooltip key="altered" emoji="🎨">🎨</EmojiWithTooltip>);
    }
    return <>{indicators}</>;
  };

  const finishPart = getFinishIndicators();
  const specialPart = getSpecialIndicators();
  const separator = finishPart && specialPart ? ' | ' : '';

  return (
    <>
      [{totalCards}]
      {(finishPart || specialPart) && ' → '}
      {finishPart}
      {separator}
      {specialPart}
    </>
  );
}