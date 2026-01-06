import React, { useState } from 'react';
import { Box, Typography, IconButton, Tooltip } from '../../atoms';
import { ContentCopyIcon } from '../../atoms/Icons';
import { MtgSetCard } from '../../molecules/Sets/MtgSetCard';
import { SetCollectionPanel } from './SetCollectionPanel';
import { SectionErrorBoundary } from '../../utils/ErrorBoundaries';
import type { MtgSet } from '../../../types/set';
import { useCollectorParam } from '../../../hooks/useCollectorParam';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';

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
  const { isMobile, isTablet } = useResponsiveBreakpoints();

  // Use desktop layout for collection panel (positioned to side)
  const showCollectionPanel = hasCollector && (isMobile === false && isTablet === false);

  const handleGroupToggled = () => {
    // Trigger refresh in MtgSetCard by changing key
    setRefreshTrigger(prev => prev + 1);
  };

  return (
    <>
      {/* Set Information Card */}
      {setInfo && (
        <SectionErrorBoundary name="SetInfoCard">
          <Box sx={{ mb: 4, display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 2 }}>
            {/* Set ID - only visible on wide screens (lg+) */}
            <Box sx={{
              display: { xs: 'none', lg: 'flex' },
              alignItems: 'center',
              gap: 1,
              bgcolor: 'background.paper',
              borderRadius: 1,
              px: 2,
              py: 1
            }}>
              <Typography variant="caption" sx={{ fontFamily: 'monospace', color: 'text.secondary' }}>
                {setInfo.id}
              </Typography>
              <Tooltip title="Copy ID">
                <IconButton
                  size="small"
                  onClick={() => {
                    navigator.clipboard.writeText(setInfo.id);
                  }}
                  sx={{ p: 0.5 }}
                >
                  <ContentCopyIcon fontSize="small" />
                </IconButton>
              </Tooltip>
            </Box>

            <Box sx={{ position: 'relative' }}>
              <MtgSetCard key={refreshTrigger} set={setInfo} />
              {/* Only render panel on desktop (positioned to side of card) */}
              {showCollectionPanel && (
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