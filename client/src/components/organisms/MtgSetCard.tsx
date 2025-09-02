import React from 'react';
import { Card, CardContent, Box, CardActionArea } from '@mui/material';
import { useTheme, alpha } from '@mui/material/styles';
import type { MtgSet, SetContext } from '../../types/set';
import { getSetTypeColor } from '../../constants/setTypeColors';
import { SetTitle } from '../atoms/Sets/SetTitle';
import { CardCountDisplay } from '../atoms/shared/CardCountDisplay';
import { TopBadges } from '../molecules/Sets/TopBadges';
import { SetIconDisplay } from '../molecules/Sets/SetIconDisplay';
import { BottomBadges } from '../molecules/Sets/BottomBadges';

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
  const [isHovered, setIsHovered] = React.useState(false);
  const setTypeColor = getSetTypeColor(set.setType);
  const theme = useTheme();

  const handleCardClick = (e: React.MouseEvent) => {
    // Prevent default navigation if left-clicking
    if (e.button === 0) {
      e.preventDefault();
      if (onSetClick) {
        onSetClick(set.code);
      }
    }
    // Allow right-click and middle-click to work normally
  };

  const setUrl = `/set/${set.code}`;

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

          <CardCountDisplay count={set.printedSize && set.printedSize > 0 ? set.printedSize : set.cardCount} />
        </Box>
      </CardContent>
      </CardActionArea>
    </Card>
  );
};