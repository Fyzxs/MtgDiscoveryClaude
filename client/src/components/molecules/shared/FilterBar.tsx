import React from 'react';
import { Box, Paper, Typography, Divider, SxProps, Theme } from '@mui/material';
import { FilterToggle, ToggleButtonGroup } from '../../atoms/shared/ToggleButton';

export interface FilterOption {
  key: string;
  label: string;
  count?: number;
  isActive: boolean;
  disabled?: boolean;
}

interface FilterSection {
  title: string;
  options: FilterOption[];
  onToggle: (key: string, active: boolean) => void;
  showCounts?: boolean;
  multiSelect?: boolean;
}

interface FilterBarProps {
  sections: FilterSection[];
  title?: string;
  elevation?: number;
  orientation?: 'horizontal' | 'vertical';
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Reusable filter bar component for creating consistent filtering interfaces
 * Supports multiple filter sections with toggle buttons
 */
export const FilterBar: React.FC<FilterBarProps> = React.memo(({
  sections,
  title,
  elevation = 1,
  orientation = 'horizontal',
  sx = {},
  className = ''
}) => {
  if (sections.length === 0) return null;

  return (
    <Paper
      elevation={elevation}
      className={className}
      sx={{
        p: 2,
        mb: 2,
        ...sx
      }}
    >
      {title && (
        <>
          <Typography variant="h6" gutterBottom>
            {title}
          </Typography>
          <Divider sx={{ mb: 2 }} />
        </>
      )}
      
      <Box
        sx={{
          display: 'flex',
          flexDirection: orientation === 'horizontal' ? 'row' : 'column',
          gap: 3,
          flexWrap: 'wrap'
        }}
      >
        {sections.map((section, sectionIndex) => (
          <Box key={section.title} sx={{ flex: 1, minWidth: 200 }}>
            <Typography variant="subtitle2" fontWeight="bold" gutterBottom>
              {section.title}
            </Typography>
            <ToggleButtonGroup 
              orientation={orientation === 'horizontal' ? 'horizontal' : 'vertical'}
              spacing={1}
            >
              {section.options.map((option) => (
                <FilterToggle
                  key={option.key}
                  label={option.label}
                  count={option.count}
                  isActive={option.isActive}
                  onToggle={(active) => section.onToggle(option.key, active)}
                  showCount={section.showCounts}
                  disabled={option.disabled}
                />
              ))}
            </ToggleButtonGroup>
          </Box>
        ))}
      </Box>
    </Paper>
  );
});

interface SimpleFilterBarProps {
  options: FilterOption[];
  onToggle: (key: string, active: boolean) => void;
  showCounts?: boolean;
  title?: string;
  orientation?: 'horizontal' | 'vertical';
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Simplified filter bar for single-section filtering
 */
export const SimpleFilterBar: React.FC<SimpleFilterBarProps> = React.memo(({
  options,
  onToggle,
  showCounts = false,
  title,
  orientation = 'horizontal',
  sx = {},
  className = ''
}) => {
  const section: FilterSection = {
    title: title || 'Filters',
    options,
    onToggle,
    showCounts
  };

  return (
    <FilterBar
      sections={[section]}
      title={title}
      orientation={orientation}
      sx={sx}
      className={className}
    />
  );
});

interface QuickFilterBarProps {
  options: FilterOption[];
  onToggle: (key: string, active: boolean) => void;
  showCounts?: boolean;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Minimal filter bar without paper wrapper for inline use
 */
export const QuickFilterBar: React.FC<QuickFilterBarProps> = React.memo(({
  options,
  onToggle,
  showCounts = false,
  sx = {},
  className = ''
}) => {
  return (
    <Box
      className={className}
      sx={{
        display: 'flex',
        gap: 1,
        flexWrap: 'wrap',
        alignItems: 'center',
        ...sx
      }}
    >
      {options.map((option) => (
        <FilterToggle
          key={option.key}
          label={option.label}
          count={option.count}
          isActive={option.isActive}
          onToggle={(active) => onToggle(option.key, active)}
          showCount={showCounts}
          disabled={option.disabled}
        />
      ))}
    </Box>
  );
});

FilterBar.displayName = 'FilterBar';
SimpleFilterBar.displayName = 'SimpleFilterBar';
QuickFilterBar.displayName = 'QuickFilterBar';