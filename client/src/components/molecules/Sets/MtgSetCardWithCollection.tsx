import React, { useState } from 'react';
import { Card, CardContent, Box, CardActionArea, Collapse } from '@mui/material';
import { useTheme, alpha } from '@mui/material/styles';
import type { MtgSet, SetContext } from '../../../types/set';
import { getSetTypeColor } from '../../../constants/setTypeColors';
import { SetTitle } from '../../atoms/Sets/SetTitle';
import { CardCountDisplay } from '../../atoms/shared/CardCountDisplay';
import { TopBadges } from './TopBadges';
import { SetIconDisplay } from './SetIconDisplay';
import { BottomBadges } from './BottomBadges';
import { SetCollectionTracker } from './SetCollectionTracker';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
import { useCollectorParam } from '../../../hooks/useCollectorParam';

interface FinishProgress {
  finishType: 'non-foil' | 'foil' | 'etched';
  collectedCards: number;
  totalCards: number;
  percentage: number;
  emoji: string;
}

interface GroupProgress {
  groupId: string;
  groupName: string;
  displayName: string;
  isSelected: boolean;
  finishes: FinishProgress[];
}

interface SetCollectionProgress {
  setId: string;
  setName: string;
  setTotalCards: number;
  overallUniqueCards: number;
  overallPercentage: number;
  overallTotalCards: number;
  groups: GroupProgress[];
}

interface MtgSetCardWithCollectionProps {
  set: MtgSet;
  context?: SetContext;
  collectionProgress?: SetCollectionProgress;
  onSetClick?: (setCode?: string) => void;
  onGroupToggle?: (setId: string, groupId: string, isSelected: boolean) => void;
  className?: string;
}

export const MtgSetCardWithCollection: React.FC<MtgSetCardWithCollectionProps> = ({
  set,
  collectionProgress,
  onSetClick,
  onGroupToggle,
  className = ''
}) => {
  const [isHovered, setIsHovered] = useState(false);
  const [isExpanded, setIsExpanded] = useState(false);
  const setTypeColor = getSetTypeColor(set.setType);
  const theme = useTheme();
  const { buildUrlWithCollector, createCollectorClickHandler } = useCollectorNavigation();
  const { hasCollector } = useCollectorParam();

  const setPath = `/set/${set.code}`;
  const setUrl = buildUrlWithCollector(setPath);

  const handleCardClick = (e: React.MouseEvent) => {
    // Toggle expansion if collector is present and collection data is available
    if (hasCollector && collectionProgress && e.detail === 2) { // Double click
      setIsExpanded(!isExpanded);
      return;
    }

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

  const handleGroupToggle = (groupId: string, isSelected: boolean) => {
    if (onGroupToggle) {
      onGroupToggle(set.id, groupId, isSelected);
    }
  };

  // Calculate dynamic height based on expansion state
  const baseHeight = 360;
  const expandedHeight = collectionProgress ? baseHeight + (collectionProgress.groups.length * 80) + 100 : baseHeight;
  const cardHeight = isExpanded ? expandedHeight : baseHeight;

  return (
    <Card
      data-mtg-set="true"
      className={className}
      sx={{
        cursor: 'pointer',
        transition: 'all 0.2s ease',
        backgroundColor: 'background.paper',
        border: `1px solid ${theme.palette.mtg.cardBorder}`,
        '&:hover': {
          transform: 'translateY(-6px)',
          boxShadow: theme.mtg.shadows.card.hover,
          backgroundColor: alpha(theme.palette.primary.main, 0.05),
          borderColor: alpha(theme.palette.primary.main, 0.3),
        },
        height: cardHeight,
        width: isExpanded ? '320px' : '240px', // Wider when expanded
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
          justifyContent: isExpanded ? 'flex-start' : 'center',
          gap: 0.25
        }}>
          <Box sx={{ width: '100%' }}>
            <SetTitle name={set.name} />

            <TopBadges
              setCode={set.code}
              releaseDate={set.releasedAt}
            />
          </Box>

          {!isExpanded && (
            <SetIconDisplay
              iconSvgUri={set.iconSvgUri}
              setName={set.name}
              borderColor={isHovered ? '#1976d2' : setTypeColor}
            />
          )}

          <Box sx={{ width: '100%' }}>
            <BottomBadges
              setType={set.setType}
              digital={set.digital}
              foilOnly={set.foilOnly}
            />

            {!isExpanded && (
              <CardCountDisplay count={set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount} />
            )}
          </Box>

          {/* Collection Tracker - shown when collector is present and expanded */}
          {hasCollector && collectionProgress && (
            <Collapse in={isExpanded} timeout={200}>
              <SetCollectionTracker
                progress={collectionProgress}
                onGroupToggle={handleGroupToggle}
              />
            </Collapse>
          )}

          {/* Expansion hint for collector mode */}
          {hasCollector && collectionProgress && !isExpanded && (
            <Box sx={{ mt: 1, opacity: 0.7 }}>
              <Box
                component="span"
                sx={{
                  fontSize: '0.75rem',
                  color: 'text.secondary',
                  fontStyle: 'italic'
                }}
              >
                Double-click to track collection
              </Box>
            </Box>
          )}
        </CardContent>
      </CardActionArea>
    </Card>
  );
};