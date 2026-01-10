import React, { useState, useMemo } from 'react';
import { Card, CardContent, Box, CardActionArea, Typography } from '../../atoms';
import { useTheme, alpha } from '../../atoms';
import type { MtgSet, SetContext } from '../../../types/set';
import { getSetTypeColor } from '../../../constants/setTypeColors';
import { SetTitle } from '../../atoms/Sets/SetTitle';
import { SetCodeBadge } from '../../atoms/Sets/SetCodeBadge';
import { SetDateBadge } from '../../atoms/shared/SetDateBadge';
import { CardCountDisplay } from '../../atoms/shared/CardCountDisplay';
import { CollectionProgressBar } from '../../atoms/shared/CollectionProgressBar';
import { TopBadges } from './TopBadges';
import { SetIconDisplay } from './SetIconDisplay';
import { SetNameWrapDisplay } from './SetNameWrapDisplay';
import { BottomBadges } from './BottomBadges';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
import { useCollectorParam } from '../../../hooks/useCollectorParam';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';

export type SetCardSize = 'sm' | 'md' | 'lg';

// Size configurations for responsive set cards
const SIZE_CONFIG: Record<SetCardSize, {
  width: number;
  height: number;
  iconSize: number;
  padding: number;
  titleSize: string;
  badgeFontSize: string;
  showIcon: boolean;
  showTopBadges: boolean;
  showBottomBadges: boolean;
  singleLineTitle: boolean;
  showProgressBar: boolean;
  useWrapDisplay: boolean;
  wrapFontSize: number;
  showCodeDateAboveIcon: boolean;  // For compact sizes: show set code + date between title and icon
}> = {
  sm: {
    width: 140,
    height: 115,
    iconSize: 44,
    padding: 0.5,
    titleSize: '0.65rem',
    badgeFontSize: '0.5rem',
    showIcon: true,
    showTopBadges: false,
    showBottomBadges: false,
    singleLineTitle: false,
    showProgressBar: false,
    useWrapDisplay: false,
    wrapFontSize: 9,
    showCodeDateAboveIcon: true,
  },
  md: {
    width: 160,
    height: 130,
    iconSize: 48,
    padding: 0.75,
    titleSize: '0.7rem',
    badgeFontSize: '0.5625rem',
    showIcon: true,
    showTopBadges: false,
    showBottomBadges: false,
    singleLineTitle: false,
    showProgressBar: false,
    useWrapDisplay: false,
    wrapFontSize: 10,
    showCodeDateAboveIcon: true,
  },
  lg: {
    width: 200,
    height: 260,
    iconSize: 60,
    padding: 1.5,
    titleSize: '0.9375rem',
    badgeFontSize: '0.6875rem',
    showIcon: true,
    showTopBadges: true,
    showBottomBadges: true,
    singleLineTitle: false,
    showProgressBar: true,
    useWrapDisplay: false,
    wrapFontSize: 14,
    showCodeDateAboveIcon: false,
  },
};

interface MtgSetCardProps {
  set: MtgSet;
  context?: SetContext;
  onSetClick?: (setCode?: string) => void;
  className?: string;
  /** Explicit size override - if not provided, determined by breakpoint */
  size?: SetCardSize;
}

export const MtgSetCard: React.FC<MtgSetCardProps> = ({
  set,
  onSetClick,
  className = '',
  size: explicitSize
}) => {
  const [isHovered, setIsHovered] = useState(false);
  const setTypeColor = getSetTypeColor(set.setType);
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

    // Calculate actual cards collected in tracking groups (only those with collecting: true)
    // Only count finishes that the user is collecting
    const collectedInTrackingGroups = collectingGroups.reduce((sum, collectingGroup) => {
      const groupData = set.userCollection?.groups.find(g => g.setGroupId === collectingGroup.setGroupId);
      if (!groupData) {
        return sum;
      }

      const collectingFinishes = collectingGroup.collectingFinishes || [];
      let groupCollected = 0;

      if (collectingFinishes.includes('nonFoil')) {
        groupCollected += groupData.group.nonFoil.cards.length;
      }
      if (collectingFinishes.includes('foil')) {
        groupCollected += groupData.group.foil.cards.length;
      }
      if (collectingFinishes.includes('etched')) {
        groupCollected += groupData.group.etched.cards.length;
      }

      return sum + groupCollected;
    }, 0);

    // Total available cards in tracking groups (only those with collecting: true)
    // Use set.groupings[].cardCounts as source of truth for available cards per grouping
    const totalAvailableInTrackingGroups = collectingGroups.reduce((sum, g) => {
      // Look up the grouping metadata from the set to get accurate card counts
      const grouping = set.groupings?.find(gr => gr.id === g.setGroupId);
      const finishCounts = grouping?.cardCounts || g.counts || { total: 0, nonFoil: 0, foil: 0, etched: 0 };

      const collectingFinishes = g.collectingFinishes || [];
      let total = 0;

      // Sum only the finish types the user is collecting
      if (collectingFinishes.includes('nonFoil')) {
        total += finishCounts.nonFoil;
      }
      if (collectingFinishes.includes('foil')) {
        total += finishCounts.foil;
      }
      if (collectingFinishes.includes('etched')) {
        total += finishCounts.etched;
      }

      return sum + total;
    }, 0);

    const percentage = totalAvailableInTrackingGroups > 0
      ? (collectedInTrackingGroups / totalAvailableInTrackingGroups) * 100
      : 0;

    return {
      setTotalCards: totalAvailableInTrackingGroups,
      uniqueCards: collectedInTrackingGroups,
      totalCards: set.userCollection.totalCards,
      percentage
    };
  }, [hasCollector, set.userCollection, set.groupings]);

  const handleCardClick = (e: React.MouseEvent) => {
    // Allow browser's default behavior for modifier keys (CTRL/CMD+click = new tab, etc.)
    if (e.ctrlKey || e.metaKey || e.shiftKey || e.button !== 0) {
      return;
    }

    // Only handle click if there's a custom onSetClick handler
    // Otherwise let the anchor tag handle navigation naturally
    if (onSetClick) {
      e.preventDefault();
      onSetClick(set.code);
    } else {
      // Use collector navigation for regular clicks
      createCollectorClickHandler(setPath)(e);
    }
  };

  const getBackgroundColor = () => {
    if (!hasCollector || !collectionProgress) {
      return theme.palette.background.paper;
    }

    const isNotCollecting = collectionProgress.setTotalCards === 0 && collectionProgress.percentage === 0;
    if (isNotCollecting) {
      return theme.palette.background.paper;
    }

    const percentage = collectionProgress.percentage;

    if (percentage >= 100) {
      return alpha(theme.palette.success.dark, 0.25);
    }
    if (percentage > 75) {
      return alpha(theme.palette.secondary.dark, 0.25);
    }
    return alpha(theme.palette.primary.dark, 0.25);
  };

  const getHoverBackgroundColor = () => {
    if (!hasCollector || !collectionProgress) {
      return alpha(theme.palette.primary.main, 0.05);
    }

    const isNotCollecting = collectionProgress.setTotalCards === 0 && collectionProgress.percentage === 0;
    if (isNotCollecting) {
      return alpha(theme.palette.primary.main, 0.05);
    }

    const percentage = collectionProgress.percentage;

    if (percentage >= 100) {
      return alpha(theme.palette.success.main, 0.35);
    }
    if (percentage > 75) {
      return alpha(theme.palette.secondary.main, 0.35);
    }
    return alpha(theme.palette.primary.main, 0.35);
  };

  return (
    <Card
      data-mtg-set="true"
      className={className}
      sx={{
        cursor: 'pointer',
        transition: 'all 0.1s ease',
        backgroundColor: getBackgroundColor(),
        border: `1px solid ${theme.palette.mtg.cardBorder}`,
        '&:hover': {
          transform: isMobile ? 'none' : 'translateY(-6px)',
          boxShadow: isMobile ? 'none' : theme.mtg.shadows.card.hover,
          backgroundColor: getHoverBackgroundColor(),
          borderColor: alpha(theme.palette.primary.main, 0.3),
        },
        // Use responsive sizing
        height: sizeConfig.height,
        width: sizeConfig.width,
        // Ensure minimum touch target
        minHeight: 44,
        minWidth: 44,
        position: 'relative',
        overflow: 'hidden',
        display: 'flex',
        flexDirection: 'column',
      }}
    >
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
        <CardContent sx={{
          p: sizeConfig.padding,
          height: '100%',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          textAlign: 'center',
          justifyContent: sizeConfig.singleLineTitle ? 'center' : 'space-between',
          gap: sizeConfig.singleLineTitle ? 0 : 0.25
        }}>
          {/* Wrap Display - text flows around icon (for sm size) */}
          {sizeConfig.useWrapDisplay ? (
            <SetNameWrapDisplay
              setName={set.name}
              centerWidth={sizeConfig.iconSize}
              centerHeight={sizeConfig.iconSize}
              containerWidth={sizeConfig.width - (sizeConfig.padding * 8 * 2)}
              fontSize={sizeConfig.wrapFontSize}
              fontWeight={500}
              centerElement={
                <SetIconDisplay
                  iconSvgUri={set.iconSvgUri}
                  setName={set.name}
                  borderColor={isHovered ? theme.palette.primary.main : setTypeColor}
                  size={sizeConfig.iconSize}
                />
              }
            />
          ) : (
            <>
              {/* Title - single line with ellipsis for compact */}
              <Box sx={{
                width: '100%',
                display: 'flex',
                justifyContent: 'center',
                overflow: 'hidden',
              }}>
                <SetTitle
                  name={set.name}
                  sx={{
                    fontSize: sizeConfig.titleSize,
                    lineHeight: 1.1,
                    minHeight: sizeConfig.singleLineTitle ? 'auto' : '48px',
                    mb: 0,
                    ...(sizeConfig.singleLineTitle && {
                      display: 'block',
                      WebkitLineClamp: 'unset',
                      WebkitBoxOrient: 'unset',
                      whiteSpace: 'nowrap',
                      overflow: 'hidden',
                      textOverflow: 'ellipsis',
                      maxWidth: '100%',
                    }),
                  }}
                />
              </Box>

              {sizeConfig.showTopBadges && (
                <TopBadges
                  setCode={set.code}
                  releaseDate={set.releasedAt}
                  compact={false}
                />
              )}

              {/* Compact sizes: show set code + release date above icon */}
              {sizeConfig.showCodeDateAboveIcon && (
                <Box sx={{
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  gap: 0.5,
                  my: 0.25
                }}>
                  <SetCodeBadge code={set.code} compact />
                  <SetDateBadge date={set.releasedAt} compact />
                </Box>
              )}

              {sizeConfig.showIcon && (
                <SetIconDisplay
                  iconSvgUri={set.iconSvgUri}
                  setName={set.name}
                  borderColor={isHovered ? theme.palette.primary.main : setTypeColor}
                  size={sizeConfig.iconSize}
                />
              )}
            </>
          )}

          <Box sx={{ width: '100%' }}>
            {sizeConfig.showBottomBadges && (
              <BottomBadges
                setType={set.setType}
                digital={set.digital}
                foilOnly={set.foilOnly}
              />
            )}

            {hasCollector ? (
              collectionProgress ? (
                sizeConfig.showProgressBar ? (
                  // Large size: show progress bar with detailed text
                  <>
                    <CollectionProgressBar
                      collected={collectionProgress.setTotalCards > 0
                        ? collectionProgress.uniqueCards
                        : set.userCollection?.uniqueCards || 0}
                      total={collectionProgress.setTotalCards}
                      percentage={collectionProgress.percentage}
                      compact={false}
                    />
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      sx={{ fontSize: sizeConfig.badgeFontSize, mt: 0.5 }}
                    >
                      {collectionProgress.setTotalCards > 0
                        ? `${collectionProgress.totalCards} collected`
                        : `${set.userCollection?.totalCards || 0} entered`
                      } ({set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount} in set)
                    </Typography>
                  </>
                ) : (
                  // Compact size: [X/Y] - [Z%] format, no bar (SetCodeBadge shown above icon)
                  <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 0.5, flexWrap: 'wrap' }}>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      sx={{ fontSize: sizeConfig.badgeFontSize, fontWeight: 500 }}
                    >
                      {collectionProgress.setTotalCards > 0
                        ? `${collectionProgress.uniqueCards}/${collectionProgress.setTotalCards} - ${Math.round(collectionProgress.percentage)}%`
                        : `${set.userCollection?.totalCards || 0}/${set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount}`
                      }
                    </Typography>
                  </Box>
                )
              ) : (
                sizeConfig.showProgressBar ? (
                  <>
                    <CollectionProgressBar
                      collected={set.userCollection?.uniqueCards || 0}
                      total={0}
                      percentage={0}
                      compact={false}
                    />
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      sx={{ fontSize: sizeConfig.badgeFontSize, mt: 0.5 }}
                    >
                      {set.userCollection?.totalCards || 0} entered ({set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount} in set)
                    </Typography>
                  </>
                ) : (
                  <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 0.5, flexWrap: 'wrap' }}>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      sx={{ fontSize: sizeConfig.badgeFontSize, fontWeight: 500 }}
                    >
                      {set.userCollection?.totalCards || 0}/{set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount}
                    </Typography>
                  </Box>
                )
              )
            ) : (
              sizeConfig.showTopBadges ? (
                <CardCountDisplay
                  count={set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount}
                  compact={false}
                />
              ) : (
                // Compact: show card count only (SetCodeBadge shown above icon)
                <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 0.5 }}>
                  <Typography
                    variant="body2"
                    color="text.secondary"
                    sx={{ fontSize: sizeConfig.badgeFontSize, fontWeight: 500 }}
                  >
                    {set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount} cards
                  </Typography>
                </Box>
              )
            )}
          </Box>

        </CardContent>
      </CardActionArea>
    </Card>
  );
};