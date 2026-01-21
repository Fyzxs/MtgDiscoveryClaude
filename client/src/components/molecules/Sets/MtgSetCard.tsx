import React, { useState, useMemo } from 'react';
import { Card, CardContent, Box, CardActionArea, Typography } from '../../atoms';
import { useTheme, alpha } from '../../atoms';
import type { MtgSet, SetContext } from '../../../types/set';
import { getSetTypeColor } from '../../../constants/setTypeColors';
import { SetTitle } from '../../atoms/Sets/SetTitle';
import { CardCountDisplay } from '../../atoms/shared/CardCountDisplay';
import { CollectionProgressBar } from '../../atoms/shared/CollectionProgressBar';
import { TopBadges } from './TopBadges';
import { SetIconDisplay } from './SetIconDisplay';
import { BottomBadges } from './BottomBadges';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
import { useCollectorParam } from '../../../hooks/useCollectorParam';

interface MtgSetCardProps {
  set: MtgSet;
  context?: SetContext;
  onSetClick?: (setCode?: string) => void;
  className?: string;
}

export const MtgSetCard: React.FC<MtgSetCardProps> = ({
  set,
  onSetClick,
  className = ''
}) => {
  const [isHovered, setIsHovered] = useState(false);
  const setTypeColor = getSetTypeColor(set.setType);
  const theme = useTheme();
  const { buildUrlWithCollector, createCollectorClickHandler } = useCollectorNavigation();
  const { hasCollector } = useCollectorParam();

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
    // Use counts.total which already accounts for all finish types
    const totalAvailableInTrackingGroups = collectingGroups.reduce((sum, g) => {
      const collectingFinishes = g.collectingFinishes || [];
      let total = 0;

      // Sum only the finish types the user is collecting
      if (collectingFinishes.includes('nonFoil')) {
        total += g.counts.nonFoil;
      }
      if (collectingFinishes.includes('foil')) {
        total += g.counts.foil;
      }
      if (collectingFinishes.includes('etched')) {
        total += g.counts.etched;
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
  }, [hasCollector, set.userCollection]);

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
          transform: 'translateY(-6px)',
          boxShadow: theme.mtg.shadows.card.hover,
          backgroundColor: getHoverBackgroundColor(),
          borderColor: alpha(theme.palette.primary.main, 0.3),
        },
        height: '360px',
        width: '240px',
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
          p: 2,
          height: '100%',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          textAlign: 'center',
          justifyContent: 'center',
          gap: 0.25
        }}>
          <Box sx={{ width: '100%' }}>
            <SetTitle name={set.name} />

            <TopBadges
              setCode={set.code}
              releaseDate={set.releasedAt}
            />
          </Box>

          <SetIconDisplay
            iconSvgUri={set.iconSvgUri}
            setName={set.name}
            borderColor={isHovered ? theme.palette.primary.main : setTypeColor}
          />

          <Box sx={{ width: '100%' }}>
            <BottomBadges
              setType={set.setType}
              digital={set.digital}
              foilOnly={set.foilOnly}
            />

            {hasCollector ? (
              collectionProgress ? (
                <>
                  <CollectionProgressBar
                    collected={collectionProgress.setTotalCards > 0
                      ? collectionProgress.uniqueCards
                      : set.userCollection?.uniqueCards || 0}
                    total={collectionProgress.setTotalCards}
                    percentage={collectionProgress.percentage}
                  />
                  <Typography
                    variant="body2"
                    color="text.secondary"
                    sx={{ fontSize: '0.75rem', mt: 0.5 }}
                  >
                    {collectionProgress.setTotalCards > 0
                      ? `${collectionProgress.totalCards} collected`
                      : `${set.userCollection?.totalCards || 0} entered`
                    }
                    {` (${set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount} in set)`}
                  </Typography>
                </>
              ) : (
                <>
                  <CollectionProgressBar
                    collected={set.userCollection?.uniqueCards || 0}
                    total={0}
                    percentage={0}
                  />
                  <Typography
                    variant="body2"
                    color="text.secondary"
                    sx={{ fontSize: '0.75rem', mt: 0.5 }}
                  >
                    {`${set.userCollection?.totalCards || 0} entered (${set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount} in set)`}
                  </Typography>
                </>
              )
            ) : (
              <CardCountDisplay count={set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount} />
            )}
          </Box>

        </CardContent>
      </CardActionArea>
    </Card>
  );
};