import React from 'react';
import { Box, Typography, Button } from '@mui/material';
import SearchOffIcon from '@mui/icons-material/SearchOff';

interface EmptyStateProps {
  message?: string;
  description?: string;
  icon?: React.ReactNode;
  action?: {
    label: string;
    onClick: () => void;
  };
  sx?: any;
}

export const EmptyState: React.FC<EmptyStateProps> = ({
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

// Pre-configured empty state for search results
export const SearchEmptyState: React.FC<{ 
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