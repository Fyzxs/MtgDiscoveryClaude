import React, { useState } from 'react';
import {
  Box,
  Typography,
  Popover,
  Tooltip,
  useTheme,
  useMediaQuery
} from '../../atoms';
import { useTranslation } from 'react-i18next';
import type { UserCardData } from '../../../types/card';
import type { CardFinish, CardSpecial } from '../../../types/collection';
import { useSymbols, useFinishDisplay, useSpecialDisplay } from '../../../hooks/useSymbols';

// Helper component to wrap symbols with localized tooltips
const SymbolWithTooltip: React.FC<{
  symbol: string;
  label: string;
  children: React.ReactNode
}> = ({ label, children }) => (
  <Tooltip title={label} arrow placement="top">
    <span role="img" aria-label={label} style={{ cursor: 'help' }}>
      {children}
    </span>
  </Tooltip>
);

interface CollectionSummaryProps {
  collectionData?: UserCardData | UserCardData[];
  size?: 'small' | 'medium' | 'large';
}

export const CollectionSummaryI18n: React.FC<CollectionSummaryProps> = ({
  collectionData,
  size = 'medium'
}) => {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const [isHovered, setIsHovered] = useState(false);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  // Translation hooks
  const { t: tCollection } = useTranslation('collection');
  const { getStatusSymbol } = useSymbols();
  const finishDisplay = useFinishDisplay();
  const specialDisplay = useSpecialDisplay();

  // Convert to array if single item, handle empty data
  const collection = collectionData
    ? (Array.isArray(collectionData) ? collectionData : [collectionData])
    : [];

  // Calculate totals and group data
  const totalCards = collection.reduce((sum, item) => sum + (item?.count || 0), 0);

  // Show empty state for cards with 0 collection count or no data
  if (totalCards === 0 || collection.length === 0) {
    const notCollectedSymbol = getStatusSymbol('notCollected');
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
          {notCollectedSymbol}
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

    if (finishTypes.includes('non-foil')) {
      const symbol = finishDisplay.getSymbol('non-foil');
      const label = finishDisplay.getLabel('non-foil');
      indicators.push(
        <SymbolWithTooltip key="nonfoil" symbol={symbol} label={label}>
          {symbol}
        </SymbolWithTooltip>
      );
    }

    if (finishTypes.includes('foil')) {
      const symbol = finishDisplay.getSymbol('foil');
      const label = finishDisplay.getLabel('foil');
      indicators.push(
        <SymbolWithTooltip key="foil" symbol={symbol} label={label}>
          {symbol}
        </SymbolWithTooltip>
      );
    }

    if (finishTypes.includes('etched')) {
      const symbol = finishDisplay.getSymbol('etched');
      const label = finishDisplay.getLabel('etched');
      indicators.push(
        <SymbolWithTooltip key="etched" symbol={symbol} label={label}>
          {symbol}
        </SymbolWithTooltip>
      );
    }

    return indicators.length > 0 ? <>{indicators}</> : null;
  };

  // Get special indicators (always show if any special types exist)
  const getSpecialIndicators = () => {
    if (!hasSpecials) return null;
    const indicators: React.ReactElement[] = [];

    // Order: Artist Proof → Signed → Altered
    if (specialTypes.has('artist_proof')) {
      const symbol = specialDisplay.getSymbol('artist_proof' as 'artist-proof');
      const label = specialDisplay.getLabel('artist_proof' as 'artist-proof');
      indicators.push(
        <SymbolWithTooltip key="artist_proof" symbol={symbol} label={label}>
          {symbol}
        </SymbolWithTooltip>
      );
    }

    if (specialTypes.has('signed')) {
      const symbol = specialDisplay.getSymbol('signed');
      const label = specialDisplay.getLabel('signed');
      indicators.push(
        <SymbolWithTooltip key="signed" symbol={symbol} label={label}>
          {symbol}
        </SymbolWithTooltip>
      );
    }

    if (specialTypes.has('altered')) {
      const symbol = specialDisplay.getSymbol('altered');
      const label = specialDisplay.getLabel('altered');
      indicators.push(
        <SymbolWithTooltip key="altered" symbol={symbol} label={label}>
          {symbol}
        </SymbolWithTooltip>
      );
    }

    return <>{indicators}</>;
  };

  // Get counts for hover state
  const getFinishCounts = () => {
    const counts: React.ReactElement[] = [];

    if (finishTypes.includes('non-foil')) {
      const count = finishGroups['non-foil'].reduce((sum, item) => sum + item.count, 0);
      const symbol = finishDisplay.getSymbol('non-foil');
      const label = finishDisplay.getLabel('non-foil');
      counts.push(
        <span key="nonfoil">
          <SymbolWithTooltip symbol={symbol} label={label}>
            {symbol}
          </SymbolWithTooltip>
          {count}
        </span>
      );
    }

    if (finishTypes.includes('foil')) {
      const count = finishGroups.foil.reduce((sum, item) => sum + item.count, 0);
      const symbol = finishDisplay.getSymbol('foil');
      const label = finishDisplay.getLabel('foil');
      counts.push(
        <span key="foil">
          <SymbolWithTooltip symbol={symbol} label={label}>
            {symbol}
          </SymbolWithTooltip>
          {count}
        </span>
      );
    }

    if (finishTypes.includes('etched')) {
      const count = finishGroups.etched.reduce((sum, item) => sum + item.count, 0);
      const symbol = finishDisplay.getSymbol('etched');
      const label = finishDisplay.getLabel('etched');
      counts.push(
        <span key="etched">
          <SymbolWithTooltip symbol={symbol} label={label}>
            {symbol}
          </SymbolWithTooltip>
          {count}
        </span>
      );
    }

    return <>{counts.map((item, index) => <React.Fragment key={`finish-${index}`}>{item}{index < counts.length - 1 ? ' ' : ''}</React.Fragment>)}</>;
  };

  const getSpecialCounts = () => {
    const counts: React.ReactElement[] = [];

    if (specialTypes.has('artist_proof')) {
      const count = collection.filter(item => item && item.special === 'artist_proof').reduce((sum, item) => sum + item.count, 0);
      const symbol = specialDisplay.getSymbol('artist_proof' as 'artist-proof');
      const label = specialDisplay.getLabel('artist_proof' as 'artist-proof');
      counts.push(
        <span key="artist_proof">
          <SymbolWithTooltip symbol={symbol} label={label}>
            {symbol}
          </SymbolWithTooltip>
          {count}
        </span>
      );
    }

    if (specialTypes.has('signed')) {
      const count = collection.filter(item => item.special === 'signed').reduce((sum, item) => sum + item.count, 0);
      const symbol = specialDisplay.getSymbol('signed');
      const label = specialDisplay.getLabel('signed');
      counts.push(
        <span key="signed">
          <SymbolWithTooltip symbol={symbol} label={label}>
            {symbol}
          </SymbolWithTooltip>
          {count}
        </span>
      );
    }

    if (specialTypes.has('altered')) {
      const count = collection.filter(item => item.special === 'altered').reduce((sum, item) => sum + item.count, 0);
      const symbol = specialDisplay.getSymbol('altered');
      const label = specialDisplay.getLabel('altered');
      counts.push(
        <span key="altered">
          <SymbolWithTooltip symbol={symbol} label={label}>
            {symbol}
          </SymbolWithTooltip>
          {count}
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
            [{totalCards}] {tCollection('cards')}
          </Typography>

          {/* Group by finish type */}
          {finishTypes.sort((a, b) => {
            const order: Record<string, number> = { 'non-foil': 0, foil: 1, etched: 2 };
            return (order[a] ?? 999) - (order[b] ?? 999);
          }).map((finish) => {
            const finishCards = finishGroups[finish];
            const finishTotal = finishCards.reduce((sum, item) => sum + item.count, 0);
            const finishSymbol = finishDisplay.getSymbol(finish as CardFinish);
            const finishLabel = finishDisplay.getLabel(finish as CardFinish);

            // Group by special type within finish
            const regularCount = finishCards.filter(item => item.special === 'none').reduce((sum, item) => sum + item.count, 0);
            const specialCards = finishCards.filter(item => item.special !== 'none');

            return (
              <Typography key={finish} variant="body2" sx={{ mb: 1 }}>
                - <SymbolWithTooltip symbol={finishSymbol} label={finishLabel}>
                  {finishSymbol}
                </SymbolWithTooltip> {finishLabel}: {finishTotal} (
                {regularCount > 0 && <>{regularCount}</>}
                {regularCount > 0 && specialCards.length > 0 && <>, </>}
                {specialCards.map((card, idx) => {
                  const specialSymbol = specialDisplay.getSymbol((card.special === 'artist_proof' ? 'artist-proof' : card.special) as CardSpecial);
                  const specialLabel = specialDisplay.getLabel((card.special === 'artist_proof' ? 'artist-proof' : card.special) as CardSpecial);
                  return (
                    <React.Fragment key={idx}>
                      {idx > 0 && ', '}
                      <SymbolWithTooltip symbol={specialSymbol} label={specialLabel}>
                        {specialSymbol}
                      </SymbolWithTooltip> {card.count}
                    </React.Fragment>
                  );
                })}
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
                const specialSymbol = specialDisplay.getSymbol((special === 'artist_proof' ? 'artist-proof' : special) as CardSpecial);
                const specialLabel = specialDisplay.getLabel((special === 'artist_proof' ? 'artist-proof' : special) as CardSpecial);

                return (
                  <Typography key={special} variant="body2" sx={{ mb: 1 }}>
                    - <SymbolWithTooltip symbol={specialSymbol} label={specialLabel}>
                      {specialSymbol}
                    </SymbolWithTooltip> {specialLabel}: {totalCount}
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