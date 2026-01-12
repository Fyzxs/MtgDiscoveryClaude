import React, { useState } from 'react';
import {
  Box,
  Typography,
  Popover,
  Tooltip
} from '../../atoms';
import type { UserCardData } from '../../../types/card';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';

// Emoji definitions with tooltips for accessibility
const EMOJI_TOOLTIPS = {
  '🔹': 'Nonfoil',
  '✨': 'Foil',
  '🌟': 'Etched',
  '📜': 'Artist Proof',
  '✍️': 'Signed',
  '🎨': 'Altered'
} as const;

// Helper component to wrap emoji with tooltip (conditionally)
const EmojiWithTooltip: React.FC<{ emoji: keyof typeof EMOJI_TOOLTIPS; children: React.ReactNode; disableTooltip?: boolean }> = ({ emoji, children, disableTooltip }) => {
  if (disableTooltip) {
    return (
      <span role="img" aria-label={EMOJI_TOOLTIPS[emoji]}>
        {children}
      </span>
    );
  }
  return (
    <Tooltip title={EMOJI_TOOLTIPS[emoji]} arrow placement="top">
      <span role="img" aria-label={EMOJI_TOOLTIPS[emoji]} style={{ cursor: 'help' }} tabIndex={-1}>
        {children}
      </span>
    </Tooltip>
  );
};

interface WishlistSummaryProps {
  wishlistData?: UserCardData | UserCardData[];
  size?: 'small' | 'medium' | 'large';
  /** Force interactive mode even on mobile (for use in detail sheets) */
  forceInteractive?: boolean;
}

export const WishlistSummary: React.FC<WishlistSummaryProps> = ({
  wishlistData,
  size = 'medium',
  forceInteractive = false
}) => {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const [isHovered, setIsHovered] = useState(false);
  const { isMobile, isTablet } = useResponsiveBreakpoints();

  // On mobile/tablet: disable tooltips and clicks (unless forceInteractive)
  const isTouchDevice = isMobile || isTablet;
  const disableTooltips = isTouchDevice;
  const disableClicks = isTouchDevice && !forceInteractive;

  // Convert to array if single item, handle empty data
  const wishlist = wishlistData
    ? (Array.isArray(wishlistData) ? wishlistData : [wishlistData])
    : [];

  // Calculate totals
  const totalCards = wishlist.reduce((sum, item) => sum + (item?.count || 0), 0);

  // Don't render anything if no wishlist data
  if (totalCards === 0 || wishlist.length === 0) {
    return null;
  }

  // Group by finish type
  const finishGroups = wishlist.reduce((acc, item) => {
    if (item && item.count > 0) {
      if (!acc[item.finish]) acc[item.finish] = [];
      acc[item.finish].push(item);
    }
    return acc;
  }, {} as Record<string, UserCardData[]>);

  const finishTypes = Object.keys(finishGroups);

  // Group by special type
  const specialTypes = new Set(wishlist.filter(item => item.special !== 'none').map(item => item.special));
  const hasSpecials = specialTypes.size > 0;

  // Get finish indicators
  const getFinishIndicators = () => {
    const indicators: React.ReactElement[] = [];
    if (finishTypes.includes('nonfoil')) {
      indicators.push(<EmojiWithTooltip key="nonfoil" emoji="🔹" disableTooltip={disableTooltips}>🔹</EmojiWithTooltip>);
    }
    if (finishTypes.includes('foil')) {
      indicators.push(<EmojiWithTooltip key="foil" emoji="✨" disableTooltip={disableTooltips}>✨</EmojiWithTooltip>);
    }
    if (finishTypes.includes('etched')) {
      indicators.push(<EmojiWithTooltip key="etched" emoji="🌟" disableTooltip={disableTooltips}>🌟</EmojiWithTooltip>);
    }
    return indicators.length > 0 ? <>{indicators}</> : null;
  };

  // Get special indicators
  const getSpecialIndicators = () => {
    if (!hasSpecials) return null;
    const indicators: React.ReactElement[] = [];
    if (specialTypes.has('proof')) {
      indicators.push(<EmojiWithTooltip key="proof" emoji="📜" disableTooltip={disableTooltips}>📜</EmojiWithTooltip>);
    }
    if (specialTypes.has('signed')) {
      indicators.push(<EmojiWithTooltip key="signed" emoji="✍️" disableTooltip={disableTooltips}>✍️</EmojiWithTooltip>);
    }
    if (specialTypes.has('altered')) {
      indicators.push(<EmojiWithTooltip key="altered" emoji="🎨" disableTooltip={disableTooltips}>🎨</EmojiWithTooltip>);
    }
    return <>{indicators}</>;
  };

  // Get counts for expanded state
  const getFinishCounts = () => {
    const counts: React.ReactElement[] = [];
    if (finishTypes.includes('nonfoil')) {
      const count = finishGroups.nonfoil.reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="nonfoil">
          <EmojiWithTooltip emoji="🔹" disableTooltip={disableTooltips}>🔹</EmojiWithTooltip>{count}
        </span>
      );
    }
    if (finishTypes.includes('foil')) {
      const count = finishGroups.foil.reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="foil">
          <EmojiWithTooltip emoji="✨" disableTooltip={disableTooltips}>✨</EmojiWithTooltip>{count}
        </span>
      );
    }
    if (finishTypes.includes('etched')) {
      const count = finishGroups.etched.reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="etched">
          <EmojiWithTooltip emoji="🌟" disableTooltip={disableTooltips}>🌟</EmojiWithTooltip>{count}
        </span>
      );
    }
    return <>{counts.map((item, index) => <React.Fragment key={`finish-${index}`}>{item}{index < counts.length - 1 ? ' ' : ''}</React.Fragment>)}</>;
  };

  const getSpecialCounts = () => {
    const counts: React.ReactElement[] = [];
    if (specialTypes.has('proof')) {
      const count = wishlist.filter(item => item && item.special === 'proof').reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="proof">
          <EmojiWithTooltip emoji="📜" disableTooltip={disableTooltips}>📜</EmojiWithTooltip>{count}
        </span>
      );
    }
    if (specialTypes.has('signed')) {
      const count = wishlist.filter(item => item.special === 'signed').reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="signed">
          <EmojiWithTooltip emoji="✍️" disableTooltip={disableTooltips}>✍️</EmojiWithTooltip>{count}
        </span>
      );
    }
    if (specialTypes.has('altered')) {
      const count = wishlist.filter(item => item.special === 'altered').reduce((sum, item) => sum + item.count, 0);
      counts.push(
        <span key="altered">
          <EmojiWithTooltip emoji="🎨" disableTooltip={disableTooltips}>🎨</EmojiWithTooltip>{count}
        </span>
      );
    }
    return <>{counts.map((item, index) => <React.Fragment key={`special-${index}`}>{item}{index < counts.length - 1 ? ' ' : ''}</React.Fragment>)}</>;
  };

  // Format display based on state
  const getDisplayText = () => {
    if (isHovered) {
      // Expanded state: show total + counts breakdown
      const finishPart = getFinishCounts();
      const specialPart = hasSpecials ? getSpecialCounts() : null;
      const separator = finishPart && specialPart ? ' | ' : '';
      return (
        <>
          ♡{totalCards} {finishPart}
          {separator}
          {specialPart}
        </>
      );
    } else {
      // Default state: show indicators only
      const finishPart = getFinishIndicators();
      const specialPart = getSpecialIndicators();
      const separator = finishPart && specialPart ? ' | ' : '';
      return (
        <>
          ♡{totalCards}
          {(finishPart || specialPart) && ' '}
          {finishPart}
          {separator}
          {specialPart}
        </>
      );
    }
  };

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    if (disableClicks) return;
    event.stopPropagation();
    event.preventDefault();
    setAnchorEl(event.currentTarget);
  };

  const handleMouseEnter = () => {
    if (disableClicks) return;
    setIsHovered(true);
  };

  const handleMouseLeave = () => {
    if (disableClicks) return;
    setIsHovered(false);
  };

  const handlePopoverClose = () => {
    setAnchorEl(null);
    setIsHovered(false);
  };

  const open = Boolean(anchorEl);

  return (
    <Box
      sx={{
        display: 'inline-flex',
        alignItems: 'center',
        bgcolor: 'rgba(60, 20, 60, 0.85)', // Dark pink/purple tint for wishlist
        borderRadius: 1,
        px: 1,
        py: 0.5
      }}
    >
      <Typography
        variant="body2"
        onClick={disableClicks ? undefined : handleClick}
        onMouseEnter={disableClicks ? undefined : handleMouseEnter}
        onMouseLeave={disableClicks ? undefined : handleMouseLeave}
        sx={{
          fontSize: size === 'small' ? '0.75rem' : size === 'large' ? '1rem' : '0.875rem',
          fontWeight: 500,
          color: 'white',
          cursor: disableClicks ? 'default' : 'pointer',
          userSelect: 'none',
          minWidth: 'max-content',
          whiteSpace: 'nowrap',
          ...(!disableClicks && {
            '&:hover': {
              color: '#ffb6c1' // Light pink on hover
            }
          })
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
            ♡ {totalCards} Wishlisted
          </Typography>

          {/* Group by finish type */}
          {finishTypes.sort((a, b) => {
            const order: Record<string, number> = { nonfoil: 0, foil: 1, etched: 2 };
            return (order[a] || 999) - (order[b] || 999);
          }).map((finish) => {
            const finishCards = finishGroups[finish];
            const finishTotal = finishCards.reduce((sum, item) => sum + item.count, 0);
            const finishIcon = finish === 'nonfoil' ? '🔹' : finish === 'foil' ? '✨' : '🌟';
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
                    <EmojiWithTooltip emoji={card.special === 'proof' ? '📜' : card.special === 'signed' ? '✍️' : '🎨'}>
                      {card.special === 'proof' ? '📜' : card.special === 'signed' ? '✍️' : '🎨'}
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
              {['proof', 'signed', 'altered'].filter(special =>
                wishlist.some(item => item.special === special)
              ).map((special) => {
                const totalCount = wishlist
                  .filter(item => item.special === special)
                  .reduce((sum, item) => sum + item.count, 0);
                const specialIcon = special === 'proof' ? '📜' : special === 'signed' ? '✍️' : '🎨';
                const specialName = special === 'proof' ? 'Artist Proof' : special === 'signed' ? 'Signed' : 'Altered';

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
