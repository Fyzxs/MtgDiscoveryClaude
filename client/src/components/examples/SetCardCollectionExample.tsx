import React from 'react';
import { Box, Typography } from '@mui/material';
import { MtgSetCardWithCollection } from '../organisms/MtgSetCardWithCollection';
import { useSetCollectionProgress } from '../../hooks/useSetCollectionProgress';
import type { MtgSet } from '../../types/set';

/**
 * Example component showing how to use the enhanced set card with collection tracking
 * This demonstrates the integration between:
 * - MtgSetCardWithCollection (main set card component)
 * - SetCollectionTracker (collection progress display)
 * - useSetCollectionProgress (collection data management)
 */

interface SetCardCollectionExampleProps {
  sets: MtgSet[];
  onSetClick?: (setCode: string) => void;
}

export const SetCardCollectionExample: React.FC<SetCardCollectionExampleProps> = ({
  sets,
  onSetClick
}) => {
  const { getCollectionProgress, updateGroupSelection } = useSetCollectionProgress();

  const handleGroupToggle = (setId: string, groupId: string, isSelected: boolean) => {
    updateGroupSelection(setId, groupId, isSelected);
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" gutterBottom>
        MTG Set Cards with Collection Tracking
      </Typography>

      <Typography variant="body1" sx={{ mb: 3, color: 'text.secondary' }}>
        When a collector parameter is present in the URL, set cards can be expanded
        to show detailed collection progress by group and finish type.
      </Typography>

      <Box sx={{
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fill, minmax(240px, 1fr))',
        gap: 3,
        '& [data-mtg-set]': {
          transition: 'width 0.2s ease',
          '&:hover': {
            zIndex: 1
          }
        }
      }}>
        {sets.map((set) => {
          const collectionProgress = getCollectionProgress(set);

          return (
            <MtgSetCardWithCollection
              key={set.id}
              set={set}
              collectionProgress={collectionProgress}
              onSetClick={onSetClick}
              onGroupToggle={handleGroupToggle}
            />
          );
        })}
      </Box>

      <Box sx={{ mt: 4, p: 2, bgcolor: 'grey.900', borderRadius: 2 }}>
        <Typography variant="h6" gutterBottom>
          Usage Notes:
        </Typography>
        <Typography variant="body2" component="ul" sx={{ pl: 2 }}>
          <li>Double-click any set card to expand collection tracking (when collector mode is active)</li>
          <li>Check/uncheck groups to include them in overall completion percentage</li>
          <li>Hover over finish type indicators for detailed progress tooltips</li>
          <li>Progress bars use different colors for different finish types (foil, non-foil, etched)</li>
          <li>Cards automatically resize to accommodate expanded content</li>
        </Typography>
      </Box>
    </Box>
  );
};