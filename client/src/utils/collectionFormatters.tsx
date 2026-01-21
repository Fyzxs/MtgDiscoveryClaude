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

  // Get finish counts with emojis
  const getFinishCounts = () => {
    const counts: JSX.Element[] = [];
    if (finishTypes.includes('nonfoil')) {
      const count = finishGroups.nonfoil.reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="nonfoil">
          <EmojiWithTooltip emoji="📄">📄</EmojiWithTooltip>{count}
        </span>
      );
    }
    if (finishTypes.includes('foil')) {
      const count = finishGroups.foil.reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="foil">
          <EmojiWithTooltip emoji="✨">✨</EmojiWithTooltip>{count}
        </span>
      );
    }
    if (finishTypes.includes('etched')) {
      const count = finishGroups.etched.reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="etched">
          <EmojiWithTooltip emoji="🌟">🌟</EmojiWithTooltip>{count}
        </span>
      );
    }
    return <>{counts.map((item, index) => <React.Fragment key={`finish-${index}`}>{item}{index < counts.length - 1 ? ' ' : ''}</React.Fragment>)}</>;
  };

  // Get special counts with emojis
  const getSpecialCounts = () => {
    if (!hasSpecials) return null;
    const counts: JSX.Element[] = [];
    // Order: 📜 → ✍️ → 🎨
    if (specialTypes.has('artist_proof')) {
      const count = collection.filter(item => item.special === 'artist_proof').reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="proof">
          <EmojiWithTooltip emoji="📜">📜</EmojiWithTooltip>{count}
        </span>
      );
    }
    if (specialTypes.has('signed')) {
      const count = collection.filter(item => item.special === 'signed').reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="signed">
          <EmojiWithTooltip emoji="✍️">✍️</EmojiWithTooltip>{count}
        </span>
      );
    }
    if (specialTypes.has('altered')) {
      const count = collection.filter(item => item.special === 'altered').reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="altered">
          <EmojiWithTooltip emoji="🎨">🎨</EmojiWithTooltip>{count}
        </span>
      );
    }
    return <>{counts.map((item, index) => <React.Fragment key={`special-${index}`}>{item}{index < counts.length - 1 ? ' ' : ''}</React.Fragment>)}</>;
  };

  const finishPart = getFinishCounts();
  const specialPart = hasSpecials ? getSpecialCounts() : null;
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