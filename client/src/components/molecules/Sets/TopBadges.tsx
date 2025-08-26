import React from 'react';
import { SetCodeBadge } from '../../atoms/Sets/SetCodeBadge';
import { ReleaseDateBadge } from '../../atoms/shared/ReleaseDateBadge';
import { TopBadgeGroup } from '../shared/BadgeGroup';

interface TopBadgesProps {
  setCode: string;
  releaseDate: string;
}

export const TopBadges: React.FC<TopBadgesProps> = React.memo(({ setCode, releaseDate }) => {
  return (
    <TopBadgeGroup>
      <SetCodeBadge code={setCode} />
      <ReleaseDateBadge date={releaseDate} />
    </TopBadgeGroup>
  );
});

TopBadges.displayName = 'TopBadges';