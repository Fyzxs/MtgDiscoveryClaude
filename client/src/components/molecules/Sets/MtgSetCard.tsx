import React, { useState, useMemo } from 'react';
import { Card, CardContent, Box, CardActionArea, Typography } from '../../atoms';
import { useTheme, alpha } from '../../atoms';
import type { MtgSet, SetContext } from '../../../types/set';
import { getSetTypeColor } from '../../../constants/setTypeColors';
import { CollectionProgressBar } from '../../atoms/shared/CollectionProgressBar';
import { SetIconDisplay } from './SetIconDisplay';
import { SetTypeAccent } from '../../atoms/Sets/SetTypeAccent';
import { AppBadge, DateBadge, StatusBadge } from '../shared/badges';
import { ProgressRing } from '../../atoms/shared/ProgressRing';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
import { useCollectorParam } from '../../../hooks/useCollectorParam';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';

export type SetCardSize = 'sm' | 'md' | 'lg';

// Size configurations for responsive set cards
interface SizeConfig {
  width: number;
  height: number;
  layout: 'horizontal' | 'vertical';
  iconSize: number;
  progressRingSize: number;
  nameLines: number;
  nameFontSize: string;
  badgeFontSize: string;
  showProgressRing: boolean;
  showHoverExpand: boolean;
  accentHeight: number;
}

const SIZE_CONFIG: Record<SetCardSize, SizeConfig> = {
  sm: {
    width: 140,
    height: 115,
    layout: 'horizontal',
    iconSize: 44,
    progressRingSize: 0,
    nameLines: 2,
    nameFontSize: '0.6875rem',
    badgeFontSize: '0.5625rem',
    showProgressRing: false,
    showHoverExpand: false,
    accentHeight: 3,
  },
  md: {
    width: 160,
    height: 130,
    layout: 'horizontal',
    iconSize: 32,
    progressRingSize: 54,
    nameLines: 2,
    nameFontSize: '0.75rem',
    badgeFontSize: '0.5625rem',
    showProgressRing: true,
    showHoverExpand: false,
    accentHeight: 3,
  },
  lg: {
    width: 220,
    height: 280,
    layout: 'vertical',
    iconSize: 98,
    progressRingSize: 150,
    nameLines: 3,
    nameFontSize: '0.875rem',
    badgeFontSize: '0.6875rem',
    showProgressRing: true,
    showHoverExpand: true,
    accentHeight: 4,
  },
};

interface MtgSetCardProps {
  set: MtgSet;
  context?: SetContext;
  onSetClick?: (setCode?: string) => void;
  className?: string;
  /** Explicit size override - if not provided, determined by breakpoint */
  size?: SetCardSize;
  /** Show expanded/detailed view by default (normally shown on hover) */
  expanded?: boolean;
}

export const MtgSetCard: React.FC<MtgSetCardProps> = ({
  set,
  onSetClick,
  className = '',
  size: explicitSize,
  expanded = false,
}) => {
  const [isHovered, setIsHovered] = useState(false);
  const showExpanded = expanded || isHovered;
  const theme = useTheme();
  const { buildUrlWithCollector, createCollectorClickHandler } = useCollectorNavigation();
  const { hasCollector } = useCollectorParam();
  const { isMobile, isTablet } = useResponsiveBreakpoints();

  // Determine size based on breakpoint or explicit override
  const cardSize: SetCardSize = useMemo(() => {
    if (explicitSize) {
      return explicitSize;
    }
    if (isMobile) {
      return 'sm';
    }
    if (isTablet) {
      return 'md';
    }
    return 'lg';
  }, [explicitSize, isMobile, isTablet]);

  const sizeConfig = SIZE_CONFIG[cardSize];
  const setPath = `/set/${set.code}`;
  const setUrl = buildUrlWithCollector(setPath);

  // Calculate collection progress from embedded userCollection data
  const collectionProgress = useMemo(() => {
    if (!hasCollector || !set.userCollection) {
      return undefined;
    }

    const collectingGroups = set.userCollection.collecting.filter(g => g.collecting === true);
    const isTracking = collectingGroups.length > 0;

    if (!isTracking) {
      return {
        setTotalCards: 0,
        uniqueCards: set.userCollection.uniqueCards,
        totalCards: set.userCollection.totalCards,
        percentage: 0
      };
    }

    let collected = 0;
    let total = 0;

    for (const cg of collectingGroups) {
      const finishes = cg.collectingFinishes || [];
      const grouping = set.groupings?.find(g => g.id === cg.setGroupId);

      if (cg.counts) {
        if (finishes.includes('nonFoil')) { collected += cg.counts.nonFoil || 0; }
        if (finishes.includes('foil')) { collected += cg.counts.foil || 0; }
        if (finishes.includes('etched')) { collected += cg.counts.etched || 0; }
      }

      if (grouping?.cardCounts) {
        if (finishes.includes('nonFoil')) { total += grouping.cardCounts.nonFoil || 0; }
        if (finishes.includes('foil')) { total += grouping.cardCounts.foil || 0; }
        if (finishes.includes('etched')) { total += grouping.cardCounts.etched || 0; }
      }
    }

    const percentage = total > 0 ? (collected / total) * 100 : 0;

    return {
      setTotalCards: total,
      uniqueCards: collected,
      totalCards: set.userCollection.totalCards,
      percentage
    };
  }, [hasCollector, set.userCollection, set.groupings]);

  const handleCardClick = (e: React.MouseEvent) => {
    if (e.ctrlKey || e.metaKey || e.shiftKey || e.button !== 0) {
      return;
    }

    if (onSetClick) {
      e.preventDefault();
      onSetClick(set.code);
    } else {
      createCollectorClickHandler(setPath)(e);
    }
  };

  const percentage = collectionProgress?.percentage || 0;
  const isTracking = hasCollector && collectionProgress && collectionProgress.setTotalCards > 0;
  const totalInSet = set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount;

  // Get background based on progress
  const getBackground = () => {
    if (!isTracking) {
      return theme.palette.background.paper;
    }

    if (percentage >= 100) {
      return `linear-gradient(135deg, ${alpha(theme.palette.success.main, 0.25)} 0%, ${theme.palette.background.paper} 100%)`;
    }
    if (percentage > 75) {
      return `linear-gradient(135deg, ${alpha(theme.palette.secondary.main, 0.25)} 0%, ${theme.palette.background.paper} 100%)`;
    }
    return `linear-gradient(135deg, ${alpha(theme.palette.primary.main, 0.15)} 0%, ${theme.palette.background.paper} 100%)`;
  };

  const getHoverBackground = () => {
    if (!isTracking) {
      return alpha(theme.palette.primary.main, 0.08);
    }

    if (percentage >= 100) {
      return `linear-gradient(135deg, ${alpha(theme.palette.success.main, 0.35)} 0%, ${alpha(theme.palette.background.paper, 0.9)} 100%)`;
    }
    if (percentage > 75) {
      return `linear-gradient(135deg, ${alpha(theme.palette.secondary.main, 0.35)} 0%, ${alpha(theme.palette.background.paper, 0.9)} 100%)`;
    }
    return `linear-gradient(135deg, ${alpha(theme.palette.primary.main, 0.25)} 0%, ${alpha(theme.palette.background.paper, 0.9)} 100%)`;
  };

  // Border color for complete sets (except top accent)
  const getBorderStyle = () => {
    const baseBorder = `1px solid ${theme.palette.grey[700]}`;
    if (percentage >= 100) {
      return {
        borderLeft: `1px solid ${alpha(theme.palette.success.main, 0.5)}`,
        borderRight: `1px solid ${alpha(theme.palette.success.main, 0.5)}`,
        borderBottom: `1px solid ${alpha(theme.palette.success.main, 0.5)}`,
        borderTop: baseBorder,
      };
    }
    return { border: baseBorder };
  };

  // Render horizontal layout (sm/md)
  const renderHorizontalLayout = () => (
    <Box sx={{ display: 'flex', flexDirection: 'column', height: '100%', p: 1, pt: 1.5 }}>
      {/* Name at top, centered */}
      <Typography
        sx={{
          fontSize: sizeConfig.nameFontSize,
          fontWeight: 600,
          color: 'text.primary',
          lineHeight: 1.2,
          textAlign: 'center',
          display: '-webkit-box',
          WebkitLineClamp: sizeConfig.nameLines,
          WebkitBoxOrient: 'vertical',
          overflow: 'hidden',
          mb: 0.75,
        }}
      >
        {set.name}
      </Typography>

      {/* Body: icon left, meta right */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flex: 1 }}>
        {/* Icon area */}
        {sizeConfig.showProgressRing ? (
          <ProgressRing
            percentage={percentage}
            size={sizeConfig.progressRingSize}
            strokeWidth={4}
            showDashes={percentage < 100}
          >
            <SetIconDisplay
              iconSvgUri={set.iconSvgUri}
              setName={set.name}
              borderColor="transparent"
              size={sizeConfig.iconSize}
            />
          </ProgressRing>
        ) : (
          <Box sx={{ width: sizeConfig.iconSize, height: sizeConfig.iconSize, flexShrink: 0 }}>
            <SetIconDisplay
              iconSvgUri={set.iconSvgUri}
              setName={set.name}
              borderColor="transparent"
              size={sizeConfig.iconSize}
            />
          </Box>
        )}

        {/* Meta info: code and progress on separate lines */}
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
          <AppBadge variant="code" label={set.code} compact />
          {hasCollector && collectionProgress ? (
            <Typography
              sx={{
                fontSize: sizeConfig.badgeFontSize,
                fontWeight: 500,
                color: percentage >= 100
                  ? 'success.main'
                  : percentage > 75
                    ? 'secondary.main'
                    : 'primary.main',
              }}
            >
              {collectionProgress.setTotalCards > 0
                ? `${collectionProgress.uniqueCards}/${collectionProgress.setTotalCards} • ${Math.round(percentage)}%`
                : `${set.userCollection?.totalCards || 0}/${totalInSet}`
              }
            </Typography>
          ) : (
            <Typography
              sx={{
                fontSize: sizeConfig.badgeFontSize,
                color: 'text.secondary',
              }}
            >
              {totalInSet} cards
            </Typography>
          )}
        </Box>
      </Box>
    </Box>
  );

  // Render vertical layout (lg) - default content
  const renderVerticalDefaultContent = () => (
    <Box
      sx={{
        position: 'absolute',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        p: 1.5,
        pt: 2,
        opacity: showExpanded ? 0 : 1,
        transition: 'opacity 0.25s ease',
        pointerEvents: showExpanded ? 'none' : 'auto',
      }}
    >
      {/* Progress ring with icon */}
      <Box sx={{ mb: 1 }}>
        <ProgressRing
          percentage={percentage}
          size={sizeConfig.progressRingSize}
          strokeWidth={8}
          showDashes={percentage < 100}
        >
          <SetIconDisplay
            iconSvgUri={set.iconSvgUri}
            setName={set.name}
            borderColor="transparent"
            size={sizeConfig.iconSize}
          />
        </ProgressRing>
      </Box>

      {/* Name */}
      <Typography
        sx={{
          fontSize: sizeConfig.nameFontSize,
          fontWeight: 600,
          color: 'text.primary',
          lineHeight: 1.2,
          textAlign: 'center',
          display: '-webkit-box',
          WebkitLineClamp: sizeConfig.nameLines,
          WebkitBoxOrient: 'vertical',
          overflow: 'hidden',
          mb: 0.5,
        }}
      >
        {set.name}
      </Typography>

      {/* Unique cards count for collectors */}
      {hasCollector && collectionProgress && (
        <Typography
          sx={{
            fontSize: '0.6875rem',
            color: 'text.secondary',
          }}
        >
          {collectionProgress.uniqueCards} of {collectionProgress.setTotalCards || totalInSet} unique
        </Typography>
      )}
    </Box>
  );

  // Render vertical layout (lg) - hover content
  const renderVerticalHoverContent = () => {
    const setTypeColor = getSetTypeColor(set.setType);
    const formatSetType = (type: string) =>
      type.replace(/_/g, ' ').replace(/\b\w/g, (l) => l.toUpperCase());

    return (
      <Box
        sx={{
          position: 'absolute',
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          p: 1.5,
          opacity: showExpanded ? 1 : 0,
          transform: showExpanded ? 'translateY(0)' : 'translateY(10px)',
          transition: 'all 0.25s ease',
          pointerEvents: showExpanded ? 'auto' : 'none',
        }}
      >
        {/* Watermark icon behind content */}
        <Box
          sx={{
            position: 'absolute',
            top: '50%',
            left: 0,
            right: 0,
            transform: 'translateY(-50%)',
            opacity: 0.12,
            zIndex: 0,
            pointerEvents: 'none',
            display: 'flex',
            justifyContent: 'center',
            px: 1.5,
            '& > div': {
              width: '90% !important',
              height: 'auto !important',
              border: 'none !important',
            },
            '& img': {
              maxWidth: '100% !important',
              maxHeight: 'none !important',
              width: '100%',
              height: 'auto',
            },
          }}
        >
          <SetIconDisplay
            iconSvgUri={set.iconSvgUri}
            setName={set.name}
            borderColor="transparent"
            size={270}
          />
        </Box>

        {/* Type tag */}
        <Box
          sx={{
            bgcolor: alpha(setTypeColor, 0.8),
            color: 'white',
            fontSize: '0.5625rem',
            fontWeight: 600,
            textTransform: 'uppercase',
            letterSpacing: 0.5,
            px: 1.25,
            py: 0.5,
            borderRadius: '0 0 6px 6px',
            mt: -0.5,
            mb: 1,
            zIndex: 1,
          }}
        >
          {formatSetType(set.setType)}
        </Box>

        {/* Meta badges */}
        <Box sx={{ display: 'flex', gap: 0.75, mb: 1, zIndex: 1 }}>
          <AppBadge variant="code" label={set.code} />
          <DateBadge variant="chip" date={set.releasedAt} />
        </Box>

        {/* Full name */}
        <Typography
          sx={{
            fontSize: '0.8125rem',
            fontWeight: 600,
            color: 'text.primary',
            lineHeight: 1.3,
            textAlign: 'center',
            display: '-webkit-box',
            WebkitLineClamp: 3,
            WebkitBoxOrient: 'vertical',
            overflow: 'hidden',
            mb: 1,
            zIndex: 1,
          }}
        >
          {set.name}
        </Typography>

        {/* Collector block */}
        <Box
          sx={{
            width: '100%',
            mt: 'auto',
            zIndex: 1,
          }}
        >
          {isTracking ? (
            <>
              <CollectionProgressBar
                collected={collectionProgress!.uniqueCards}
                total={collectionProgress!.setTotalCards}
                percentage={percentage}
                compact
                showTicks
              />
              <Box sx={{ display: 'flex', justifyContent: 'space-around', mt: 0.5 }}>
                <Box sx={{ textAlign: 'center' }}>
                  <Typography sx={{ fontSize: '0.5rem', color: 'text.secondary', textTransform: 'uppercase', letterSpacing: 0.5 }}>
                    unique
                  </Typography>
                  <Typography sx={{ fontSize: '0.625rem', color: 'text.primary', fontWeight: 500 }}>
                    {set.userCollection?.uniqueCards || 0} / {totalInSet}
                  </Typography>
                </Box>
                <Typography sx={{ color: 'grey.600', alignSelf: 'center' }}>•</Typography>
                <Box sx={{ textAlign: 'center' }}>
                  <Typography sx={{ fontSize: '0.5rem', color: 'text.secondary', textTransform: 'uppercase', letterSpacing: 0.5 }}>
                    tracked
                  </Typography>
                  <Typography sx={{ fontSize: '0.625rem', color: 'text.primary', fontWeight: 500 }}>
                    {collectionProgress!.uniqueCards} / {collectionProgress!.setTotalCards}
                  </Typography>
                </Box>
                <Typography sx={{ color: 'grey.600', alignSelf: 'center' }}>•</Typography>
                <Box sx={{ textAlign: 'center' }}>
                  <Typography sx={{ fontSize: '0.5rem', color: 'text.secondary', textTransform: 'uppercase', letterSpacing: 0.5 }}>
                    total
                  </Typography>
                  <Typography sx={{ fontSize: '0.625rem', color: 'text.primary', fontWeight: 500 }}>
                    {collectionProgress!.totalCards}
                  </Typography>
                </Box>
              </Box>
            </>
          ) : (
            <Box sx={{ textAlign: 'center' }}>
              <Typography sx={{ fontSize: '0.75rem', color: 'text.secondary' }}>
                {set.userCollection?.uniqueCards || 0} of {totalInSet} unique cards
              </Typography>
              {(set.digital || set.foilOnly) && (
                <Box sx={{ display: 'flex', justifyContent: 'center', gap: 0.5, mt: 0.5 }}>
                  {set.digital && <StatusBadge variant="digital" show />}
                  {set.foilOnly && <StatusBadge variant="foil" show />}
                </Box>
              )}
            </Box>
          )}
        </Box>
      </Box>
    );
  };

  return (
    <Card
      data-mtg-set="true"
      className={className}
      sx={{
        cursor: 'pointer',
        transition: 'all 0.2s ease',
        background: getBackground(),
        ...getBorderStyle(),
        borderRadius: cardSize === 'sm' ? 1 : cardSize === 'md' ? 1.25 : 1.5,
        '&:hover': {
          transform: isMobile ? 'none' : cardSize === 'lg' ? 'translateY(-6px)' : 'translateY(-3px)',
          boxShadow: isMobile ? 'none' : theme.mtg.shadows.card.hover,
          background: getHoverBackground(),
          borderColor: alpha(theme.palette.primary.main, 0.3),
        },
        height: sizeConfig.height,
        width: sizeConfig.width,
        minHeight: 44,
        minWidth: 44,
        position: 'relative',
        overflow: 'hidden',
      }}
    >
      {/* Type accent bar at top */}
      <SetTypeAccent setType={set.setType} height={sizeConfig.accentHeight} />

      <CardActionArea
        component="a"
        href={setUrl}
        onClick={handleCardClick}
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
        sx={{
          height: '100%',
          textDecoration: 'none',
          color: 'inherit',
          '&:hover': {
            textDecoration: 'none'
          }
        }}
      >
        {sizeConfig.layout === 'horizontal' ? (
          renderHorizontalLayout()
        ) : (
          <CardContent sx={{ p: 0, height: '100%', position: 'relative' }}>
            {renderVerticalDefaultContent()}
            {sizeConfig.showHoverExpand && renderVerticalHoverContent()}
          </CardContent>
        )}
      </CardActionArea>
    </Card>
  );
};
