import React from 'react';
import { Card, CardContent } from '@mui/material';
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

  const handleCardClick = () => {
    if (onSetClick) {
      onSetClick(set.code);
    }
  };

  return (
    <Card
      data-mtg-set="true"
      onClick={handleCardClick}
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
      className={className}
      sx={{
        cursor: 'pointer',
        transition: 'all 0.3s ease',
        backgroundColor: 'background.paper',
        border: '1px solid rgba(255, 255, 255, 0.12)',
        '&:hover': {
          transform: 'translateY(-6px)',
          boxShadow: '0 12px 40px rgba(25, 118, 210, 0.4), 0 0 20px rgba(25, 118, 210, 0.2)',
          backgroundColor: 'rgba(25, 118, 210, 0.05)',
          borderColor: 'rgba(25, 118, 210, 0.3)',
        },
        minHeight: '280px',
        width: '280px',
        position: 'relative',
        overflow: 'visible',
      }}
    >
      <CardContent sx={{ p: 3, display: 'flex', flexDirection: 'column', alignItems: 'center', textAlign: 'center' }}>
        <SetTitle name={set.name} />
        
        <TopBadges 
          setCode={set.code} 
          releaseDate={set.releasedAt} 
        />

        <SetIconDisplay
          iconSvgUri={set.iconSvgUri}
          setName={set.name}
          borderColor={isHovered ? '#1976d2' : setTypeColor}
        />

        <BottomBadges
          setType={set.setType}
          digital={set.digital}
          foilOnly={set.foilOnly}
        />

        <CardCountDisplay count={set.cardCount} />
      </CardContent>
    </Card>
  );
};