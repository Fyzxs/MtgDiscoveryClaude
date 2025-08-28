import React, { type ReactNode } from 'react';
import { Box, Typography, IconButton } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';

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
  
  const getBadgeText = () => {
    if (isLoading) return '[Loading...]';
    if (isError) return '[Error]';
    if (count !== undefined) return `[${count}]`;
    return '';
  };

  const handleToggleExpanded = () => {
    onExpandedChange(!expanded);
  };

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
        {getBadgeText() && (
          <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
            {getBadgeText()}
          </Typography>
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