import React, { type ReactNode } from 'react';
import { Box, Typography, IconButton, Chip } from '../../atoms';
import { ExpandMoreIcon, ExpandLessIcon } from '../../atoms/Icons';

interface ExpandableSectionProps {
  title: string;
  count?: number;
  isLoading?: boolean;
  isError?: boolean;
  children: ReactNode;
  expanded: boolean;
  onExpandedChange: (expanded: boolean) => void;
  defaultExpanded?: boolean;
}

/**
 * Reusable expandable section component with standardized expand/collapse behavior
 */
export const ExpandableSection: React.FC<ExpandableSectionProps> = ({
  title,
  count,
  isLoading = false,
  isError = false,
  children,
  expanded,
  onExpandedChange,
}) => {

  const getBadgeContent = () => {
    if (isLoading) return 'Loading...';
    if (isError) return 'Error';
    if (count !== undefined) return count.toString();
    return null;
  };

  const handleToggleExpanded = () => {
    onExpandedChange(!expanded);
  };

  const badgeContent = getBadgeContent();

  return (
    <Box>
      <Box
        sx={{
          display: 'flex',
          alignItems: 'center',
          cursor: 'pointer',
          '&:hover': {
            '& .expand-icon': {
              color: 'primary.main'
            }
          }
        }}
        onClick={handleToggleExpanded}
      >
        <Typography variant="subtitle1" fontWeight="bold">
          {title}
        </Typography>
        {badgeContent && (
          <Chip
            label={badgeContent}
            size="small"
            sx={{
              ml: 1,
              height: 22,
              fontSize: '0.8rem',
              fontWeight: 'bold',
              bgcolor: isError ? 'error.main' : isLoading ? 'action.disabled' : 'primary.main',
              color: isError ? 'error.contrastText' : isLoading ? 'text.disabled' : 'primary.contrastText',
              '& .MuiChip-label': {
                px: 1.5
              }
            }}
          />
        )}
        <IconButton
          size="small"
          className="expand-icon"
          sx={{ ml: 'auto' }}
        >
          {expanded ? <ExpandLessIcon /> : <ExpandMoreIcon />}
        </IconButton>
      </Box>

      {expanded && (
        <Box sx={{ mt: 1 }}>
          {children}
        </Box>
      )}
    </Box>
  );
};