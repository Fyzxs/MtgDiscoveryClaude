import React from 'react';
import { Box, Typography, Button } from '@mui/material';
import SearchOffIcon from '@mui/icons-material/SearchOff';
import type { EmptyStateProps as StandardEmptyStateProps } from '../../../types/components';

type EmptyStateProps = StandardEmptyStateProps;

const EmptyStateComponent: React.FC<EmptyStateProps> = ({
  message = 'No results found',
  description,
  icon,
  action,
  sx = {}
}) => {
  return (
    <Box 
      sx={{ 
        mt: 4, 
        mb: 4,
        textAlign: 'center',
        py: 6,
        ...sx 
      }}
    >
      {icon && (
        <Box sx={{ mb: 2, color: 'text.secondary' }}>
          {icon}
        </Box>
      )}
      
      <Typography variant="h6" color="text.secondary" gutterBottom>
        {message}
      </Typography>
      
      {description && (
        <Typography variant="body2" color="text.secondary" sx={{ mt: 1, mb: 3 }}>
          {description}
        </Typography>
      )}
      
      {action && (
        <Button 
          variant="outlined" 
          onClick={action.onClick}
          sx={{ mt: 2 }}
        >
          {action.label}
        </Button>
      )}
    </Box>
  );
};

/**
 * Memoized EmptyState component
 * Static component that rarely changes
 */
export const EmptyState = React.memo(EmptyStateComponent);

// Pre-configured empty state for search results
const SearchEmptyStateComponent: React.FC<{ 
  itemType?: string;
  onClear?: () => void;
}> = ({ itemType = 'results', onClear }) => {
  return (
    <EmptyState
      icon={<SearchOffIcon sx={{ fontSize: 48 }} />}
      message={`No ${itemType} found matching your criteria`}
      description="Try adjusting your filters or search terms"
      action={onClear ? {
        label: 'Clear filters',
        onClick: onClear
      } : undefined}
    />
  );
};

/**
 * Memoized SearchEmptyState component
 */
export const SearchEmptyState = React.memo(SearchEmptyStateComponent);