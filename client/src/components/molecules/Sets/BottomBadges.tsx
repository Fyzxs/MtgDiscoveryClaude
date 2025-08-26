import React from 'react';
import { SetTypeBadge } from '../../atoms/Sets/SetTypeBadge';
import { DigitalBadge } from '../../atoms/Sets/DigitalBadge';
import { FoilOnlyBadge } from '../../atoms/Sets/FoilOnlyBadge';
import { BottomBadgeGroup } from '../shared/BadgeGroup';

interface BottomBadgesProps {
  setType: string;
  digital: boolean;
  foilOnly: boolean;
}

export const BottomBadges: React.FC<BottomBadgesProps> = React.memo(({ 
  setType, 
  digital, 
  foilOnly 
}) => {
  return (
    <BottomBadgeGroup>
      <SetTypeBadge setType={setType} />
      <DigitalBadge show={digital} />
      <FoilOnlyBadge show={foilOnly} />
    </BottomBadgeGroup>
  );
});

BottomBadges.displayName = 'BottomBadges';