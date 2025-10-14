import React, { useState, useEffect } from 'react';
import { logger } from '../../../utils/logger';
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
import { useSetCollectionProgress, type SetCollectionProgress } from '../../../hooks/useSetCollectionProgress';

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
  const [collectionProgress, setCollectionProgress] = useState<SetCollectionProgress | undefined>(undefined);
  const setTypeColor = getSetTypeColor(set.setType);
  const theme = useTheme();
  const { buildUrlWithCollector, createCollectorClickHandler} = useCollectorNavigation();
  const { hasCollector } = useCollectorParam();
  const { getCollectionProgress } = useSetCollectionProgress();

  const setPath = `/set/${set.code}`;
  const setUrl = buildUrlWithCollector(setPath);

  useEffect(() => {
    if (!hasCollector) {
      return;
    }

    let cancelled = false;

    getCollectionProgress(set)
      .then(progress => {
        if (!cancelled) {
          logger.debug('Collection progress received for', set.code, ':', progress);
          setCollectionProgress(progress);
        }
      })
      .catch(error => {
        if (!cancelled) {
          logger.error('Error fetching collection progress:', error);
        }
      });

    return () => {
      cancelled = true;
    };
  }, [hasCollector, set, getCollectionProgress]);

  const handleCardClick = (e: React.MouseEvent) => {
    // Only handle click if there's a custom onSetClick handler
    // Otherwise let the anchor tag handle navigation naturally
    if (onSetClick && e.button === 0) {
      e.preventDefault();
      onSetClick(set.code);
    } else if (e.button === 0) {
      // Use collector navigation for regular clicks
      createCollectorClickHandler(setPath)(e);
    }
    // Allow right-click, middle-click, and normal navigation to work
  };


  return (
    <Card
      data-mtg-set="true"
      className={className}
      sx={{
        cursor: 'pointer',
        transition: 'all 0.1s ease',
        backgroundColor: 'background.paper',
        border: `1px solid ${theme.palette.mtg.cardBorder}`,
        '&:hover': {
          transform: 'translateY(-6px)',
          boxShadow: theme.mtg.shadows.card.hover,
          backgroundColor: alpha(theme.palette.primary.main, 0.05),
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
          borderColor={isHovered ? '#1976d2' : setTypeColor}
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
                  collected={collectionProgress.uniqueCards}
                  total={collectionProgress.setTotalCards}
                  percentage={collectionProgress.percentage}
                />
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ fontSize: '0.75rem', mt: 0.5 }}
                >
                  {collectionProgress.totalCards} cards collected
                </Typography>
              </>
            ) : (
              <>
                <CollectionProgressBar
                  collected={0}
                  total={set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount}
                  percentage={0}
                />
                <Typography
                  variant="body2"
                  color="text.secondary"
                  sx={{ fontSize: '0.75rem', mt: 0.5 }}
                >
                  0 cards collected
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