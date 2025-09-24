import React, { useState } from 'react';
import {
  Box,
  Typography,
  Popover,
  Tooltip,
  useTheme,
  useMediaQuery
} from '@mui/material';
import type { UserCardData, Card } from '../../../types/card';

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

interface CollectionSummaryProps {
  collectionData?: UserCardData | UserCardData[];
  size?: 'small' | 'medium' | 'large';
  card?: Card; // Optional card object for debugging
}

export const CollectionSummary: React.FC<CollectionSummaryProps> = ({
  collectionData,
  size = 'medium',
  card
}) => {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const [isHovered, setIsHovered] = useState(false);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  // Convert to array if single item, handle empty data
  const collection = collectionData
    ? (Array.isArray(collectionData) ? collectionData : [collectionData])
    : [];

  // Calculate totals and group data
  const totalCards = collection.reduce((sum, item) => sum + (item?.count || 0), 0);

  // Show empty state for cards with 0 collection count or no data
  if (totalCards === 0 || collection.length === 0) {
    return (
      <Box
        sx={{
          display: 'inline-flex',
          alignItems: 'center',
          bgcolor: 'rgba(0, 0, 0, 0.8)',
          borderRadius: 1,
          px: 1,
          py: 0.5
        }}
      >
        <Typography
          variant="body2"
          sx={{
            fontSize: size === 'small' ? '0.75rem' : size === 'large' ? '1rem' : '0.875rem',
            color: 'white',
            fontWeight: 500
          }}
        >
          ⭕
        </Typography>
      </Box>
    );
  }

  // Group by finish type and check for multiple finishes
  const finishGroups = collection.reduce((acc, item) => {
    if (item && item.count > 0) {
      if (!acc[item.finish]) acc[item.finish] = [];
      acc[item.finish].push(item);
    }
    return acc;
  }, {} as Record<string, UserCardData[]>);

  const finishTypes = Object.keys(finishGroups);

  // Group by special type and check for specials
  const specialTypes = new Set(collection.filter(item => item.special !== 'none').map(item => item.special));
  const hasSpecials = specialTypes.size > 0;

  // Get finish indicators (always show finish types)
  const getFinishIndicators = () => {
    const indicators: React.ReactElement[] = [];
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
    const indicators: React.ReactElement[] = [];
    // Order: 📜 → ✍️ → 🎨
    if (specialTypes.has('artist_proof')) {
      indicators.push(<EmojiWithTooltip key="artist_proof" emoji="📜">📜</EmojiWithTooltip>);
    }
    if (specialTypes.has('signed')) {
      indicators.push(<EmojiWithTooltip key="signed" emoji="✍️">✍️</EmojiWithTooltip>);
    }
    if (specialTypes.has('altered')) {
      indicators.push(<EmojiWithTooltip key="altered" emoji="🎨">🎨</EmojiWithTooltip>);
    }
    return <>{indicators}</>;
  };

  // Get counts for hover state
  const getFinishCounts = () => {
    const counts: React.ReactElement[] = [];
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

  const getSpecialCounts = () => {
    const counts: React.ReactElement[] = [];
    if (specialTypes.has('artist_proof')) {
      const count = collection.filter(item => item && item.special === 'artist_proof').reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="artist_proof">
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

  // Format display based on state
  const getDisplayText = () => {
    if (isHovered && !isMobile) {
      // Hover state: show counts with padding to prevent size reduction
      const finishPart = getFinishCounts();
      const specialPart = hasSpecials ? getSpecialCounts() : null;
      const separator = finishPart && specialPart ? ' | ' : '';
      return (
        <>
          &nbsp;&nbsp;{finishPart}
          {separator}
          {specialPart}&nbsp;&nbsp;
        </>
      );
    } else {
      // Default state: show indicators only
      const finishPart = getFinishIndicators();
      const specialPart = getSpecialIndicators();
      const separator = finishPart && specialPart ? ' | ' : '';
      return (
        <>
          [{totalCards}]
          {(finishPart || specialPart) && ' '}
          {finishPart}
          {separator}
          {specialPart}
        </>
      );
    }
  };

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    if (isMobile && !isHovered) {
      // First click on mobile: show hover state
      setIsHovered(true);
    } else {
      // Second click on mobile or any click on desktop: show detailed popover
      setAnchorEl(event.currentTarget);
    }
  };

  const handleMouseEnter = () => {
    if (!isMobile) {
      setIsHovered(true);
    }
  };

  const handleMouseLeave = () => {
    if (!isMobile) {
      setIsHovered(false);
    }
  };

  const handlePopoverClose = () => {
    setAnchorEl(null);
    if (isMobile) {
      setIsHovered(false);
    }
  };

  const open = Boolean(anchorEl);

  return (
    <Box
      sx={{
        display: 'inline-flex',
        alignItems: 'center',
        bgcolor: 'rgba(0, 0, 0, 0.8)',
        borderRadius: 1,
        px: 1,
        py: 0.5
      }}
    >
      <Typography
        variant="body2"
        onClick={handleClick}
        onMouseEnter={handleMouseEnter}
        onMouseLeave={handleMouseLeave}
        sx={{
          fontSize: size === 'small' ? '0.75rem' : size === 'large' ? '1rem' : '0.875rem',
          fontWeight: 500,
          color: 'white',
          cursor: 'pointer',
          userSelect: 'none',
          minWidth: 'max-content',
          whiteSpace: 'nowrap',
          '&:hover': {
            color: 'primary.light'
          }
        }}
      >
        {getDisplayText()}
      </Typography>

      <Popover
        open={open}
        anchorEl={anchorEl}
        onClose={handlePopoverClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        sx={{ mt: 0.5 }}
      >
        <Box sx={{ p: 2, minWidth: 300 }}>
          <Typography variant="h6" gutterBottom sx={{ fontSize: '1rem', fontWeight: 600, mb: 2 }}>
            [{totalCards}] Cards
          </Typography>

          {/* Group by finish type */}
          {finishTypes.sort((a, b) => {
            const order = { nonfoil: 0, foil: 1, etched: 2 };
            return (order as any)[a] - (order as any)[b];
          }).map((finish) => {
            const finishCards = finishGroups[finish];
            const finishTotal = finishCards.reduce((sum, item) => sum + item.count, 0);
            const finishIcon = finish === 'nonfoil' ? '📄' : finish === 'foil' ? '✨' : '🌟';
            const finishName = finish === 'nonfoil' ? 'Nonfoil' : finish === 'foil' ? 'Foil' : 'Etched';

            // Group by special type within finish
            const regularCount = finishCards.filter(item => item.special === 'none').reduce((sum, item) => sum + item.count, 0);
            const specialCards = finishCards.filter(item => item.special !== 'none');

            return (
              <Typography key={finish} variant="body2" sx={{ mb: 1 }}>
                - <EmojiWithTooltip emoji={finishIcon as keyof typeof EMOJI_TOOLTIPS}>
                  {finishIcon}
                </EmojiWithTooltip> {finishName}: {finishTotal} (
                {regularCount > 0 && <>{regularCount}</>}
                {regularCount > 0 && specialCards.length > 0 && <>, </>}
                {specialCards.map((card, idx) => (
                  <React.Fragment key={idx}>
                    {idx > 0 && ', '}
                    <EmojiWithTooltip emoji={card.special === 'artist_proof' ? '📜' : card.special === 'signed' ? '✍️' : '🎨'}>
                      {card.special === 'artist_proof' ? '📜' : card.special === 'signed' ? '✍️' : '🎨'}
                    </EmojiWithTooltip> {card.count}
                  </React.Fragment>
                ))}
                {specialCards.length === 0 && regularCount === 0 && <>0</>}
                )
              </Typography>
            );
          })}

          {/* Special types aggregate */}
          {hasSpecials && (
            <>
              <Box sx={{ borderBottom: 1, borderColor: 'divider', my: 2 }} />
              {['artist_proof', 'signed', 'altered'].filter(special =>
                collection.some(item => item.special === special)
              ).map((special) => {
                const totalCount = collection
                  .filter(item => item.special === special)
                  .reduce((sum, item) => sum + item.count, 0);
                const specialIcon = special === 'artist_proof' ? '📜' : special === 'signed' ? '✍️' : '🎨';
                const specialName = special === 'artist_proof' ? 'Artist Proof' : special === 'signed' ? 'Signed' : 'Altered';

                return (
                  <Typography key={special} variant="body2" sx={{ mb: 1 }}>
                    - <EmojiWithTooltip emoji={specialIcon}>
                      {specialIcon}
                    </EmojiWithTooltip> {specialName}: {totalCount}
                  </Typography>
                );
              })}
            </>
          )}
        </Box>
      </Popover>
    </Box>
  );
};