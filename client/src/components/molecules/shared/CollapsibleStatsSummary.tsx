import React, { useState } from 'react';
import { Box, Typography, Paper, Collapse } from '../../atoms';
import { ExpandMoreIcon, ExpandLessIcon } from '../../atoms/Icons';
import { CollectionStatsSummary } from './CollectionStatsSummary';
import { touchTargetStyles } from '../../../styles/touchTargets';
import type { MtgSet } from '../../../types/set';

interface CollapsibleStatsSummaryProps {
  sets: MtgSet[];
}

/**
 * Mobile-friendly collapsible wrapper for CollectionStatsSummary
 * Shows a compact header that expands to reveal full stats on tap
 */
export const CollapsibleStatsSummary: React.FC<CollapsibleStatsSummaryProps> = ({ sets }) => {
  const [expanded, setExpanded] = useState(false);

  const handleToggle = () => {
    setExpanded(!expanded);
  };

  // Calculate quick summary stat for collapsed view
  const totalCards = sets.reduce((acc, set) => acc + (set.userCollection?.totalCards ?? 0), 0);

  return (
    <Paper
      elevation={0}
      sx={{
        mb: 2,
        mx: 'auto',
        width: 'fit-content',
        minWidth: 200,
        bgcolor: 'background.paper',
        border: '1px solid',
        borderColor: 'divider',
        borderRadius: 2,
        overflow: 'hidden',
      }}
    >
      <Box
        onClick={handleToggle}
        role="button"
        tabIndex={0}
        aria-expanded={expanded}
        aria-label={expanded ? 'Collapse collection stats' : 'Expand collection stats'}
        onKeyDown={(e: React.KeyboardEvent) => {
          if (e.key === 'Enter' || e.key === ' ') {
            e.preventDefault();
            handleToggle();
          }
        }}
        sx={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          gap: 1,
          py: 1.5,
          px: 2,
          cursor: 'pointer',
          ...touchTargetStyles.minimum,
          '&:hover': {
            backgroundColor: 'action.hover',
          },
          '&:focus': {
            outline: '2px solid',
            outlineColor: 'primary.main',
            outlineOffset: '-2px',
          },
        }}
      >
        <Typography
          variant="body2"
          sx={{ fontWeight: 500 }}
        >
          <Typography
            component="span"
            sx={{ color: 'primary.main', fontWeight: 'bold' }}
          >
            {totalCards.toLocaleString()}
          </Typography>
          {' cards in collection'}
        </Typography>
        {expanded ? (
          <ExpandLessIcon sx={{ fontSize: 20, color: 'text.secondary' }} />
        ) : (
          <ExpandMoreIcon sx={{ fontSize: 20, color: 'text.secondary' }} />
        )}
      </Box>

      <Collapse in={expanded}>
        <Box sx={{ borderTop: '1px solid', borderColor: 'divider' }}>
          <CollectionStatsSummary sets={sets} sx={{ border: 'none', mb: 0, borderRadius: 0 }} />
        </Box>
      </Collapse>
    </Paper>
  );
};
