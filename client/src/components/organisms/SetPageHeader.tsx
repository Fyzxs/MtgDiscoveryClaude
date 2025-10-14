import React, { useState } from 'react';
import { Box, Typography } from '../atoms';
import { MtgSetCard } from '../molecules/Sets/MtgSetCard';
import { SetCollectionPanel } from './SetCollectionPanel';
import { SectionErrorBoundary } from '../ErrorBoundaries';
import type { MtgSet } from '../../types/set';
import { useCollectorParam } from '../../hooks/useCollectorParam';

interface SetPageHeaderProps {
  setInfo?: MtgSet;
  setName: string;
  setCode: string;
  availableGroupIds?: string[];
}

export const SetPageHeader: React.FC<SetPageHeaderProps> = ({
  setInfo,
  setName,
  setCode,
  availableGroupIds
}) => {
  const [isPanelExpanded, setIsPanelExpanded] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);
  const { hasCollector } = useCollectorParam();

  const handleGroupToggled = () => {
    // Trigger refresh in MtgSetCard by changing key
    setRefreshTrigger(prev => prev + 1);
  };

  return (
    <>
      {/* Set Information Card */}
      {setInfo && (
        <SectionErrorBoundary name="SetInfoCard">
          <Box sx={{ mb: 4, display: 'flex', justifyContent: 'center' }}>
            <Box sx={{ position: 'relative' }}>
              <MtgSetCard key={refreshTrigger} set={setInfo} />
              {/* Only render panel if collector parameter exists */}
              {hasCollector && (
                <Box sx={{ position: 'absolute', left: '100%', top: 0 }}>
                  <SetCollectionPanel
                    set={setInfo}
                    isExpanded={isPanelExpanded}
                    onToggle={() => setIsPanelExpanded(!isPanelExpanded)}
                    availableGroupIds={availableGroupIds}
                    onGroupToggled={handleGroupToggled}
                  />
                </Box>
              )}
            </Box>
          </Box>
        </SectionErrorBoundary>
      )}

      {/* Fallback title if no set info */}
      {!setInfo && (
        <Typography variant="h3" component="h1" gutterBottom sx={{ mb: 4, textAlign: 'center' }}>
          {setName} ({setCode.toUpperCase()})
        </Typography>
      )}
    </>
  );
};