import React from 'react';
import {
  Box,
  Typography,
  Grid
} from '../../atoms';
import { MultiSelectDropdown } from './MultiSelectDropdown';
import type { CollectorFiltersConfig } from '../../../types/filters';
import type { StyledComponentProps } from '../../../types/components';

interface CollectorFiltersSectionProps extends StyledComponentProps {
  config: CollectorFiltersConfig;
  title?: string;
  spacing?: number;
}

const CollectorFiltersSectionComponent: React.FC<CollectorFiltersSectionProps> = ({
  config,
  title,
  spacing = 2,
  sx = {}
}) => {
  const {
    collectionCounts,
    signedCards,
    finishes,
    collectionStatus
  } = config;

  // If no collector filters are configured, don't render anything
  if (!collectionCounts && !signedCards && !finishes && !collectionStatus) {
    return null;
  }

  return (
    <Box
      component={title ? 'fieldset' : 'div'}
      sx={{
        border: '1px solid',
        borderColor: 'divider',
        borderRadius: 2,
        p: 2,
        m: 0,
        bgcolor: theme => theme.palette.mode === 'dark'
          ? theme.palette.grey[900]
          : theme.palette.grey[100],
        ...sx
      }}
    >
      {title && (
        <Typography
          component="legend"
          sx={{
            px: 1,
            fontSize: '0.875rem',
            color: 'text.secondary',
            fontWeight: 500
          }}
        >
          {title}
        </Typography>
      )}

      <Grid
        container
        spacing={spacing}
        sx={{ alignItems: 'center' }}
        role="group"
        aria-label="Collector filter options"
      >
        {/* Collection Count Filter */}
        {collectionCounts && (
          <Grid size={{ xs: 12, sm: 'auto' }}>
            <MultiSelectDropdown
              value={collectionCounts.value}
              onChange={collectionCounts.onChange}
              options={collectionCounts.options}
              label={collectionCounts.label}
              placeholder={collectionCounts.placeholder}
              minWidth={collectionCounts.minWidth || 200}
              fullWidth={collectionCounts.fullWidth !== false}
              loading={collectionCounts.loading}
              disabled={collectionCounts.disabled}
            />
          </Grid>
        )}

        {/* Signed Cards Filter */}
        {signedCards && (
          <Grid size={{ xs: 12, sm: 'auto' }}>
            <MultiSelectDropdown
              value={signedCards.value}
              onChange={signedCards.onChange}
              options={signedCards.options}
              label={signedCards.label}
              placeholder={signedCards.placeholder}
              minWidth={signedCards.minWidth || 150}
              fullWidth={signedCards.fullWidth !== false}
              loading={signedCards.loading}
              disabled={signedCards.disabled}
            />
          </Grid>
        )}

        {/* Finishes Filter */}
        {finishes && (
          <Grid size={{ xs: 12, sm: 'auto' }}>
            <MultiSelectDropdown
              value={finishes.value}
              onChange={finishes.onChange}
              options={finishes.options}
              label={finishes.label}
              placeholder={finishes.placeholder}
              minWidth={finishes.minWidth || 150}
              fullWidth={finishes.fullWidth !== false}
              loading={finishes.loading}
              disabled={finishes.disabled}
            />
          </Grid>
        )}

        {/* Collection Status Filter */}
        {collectionStatus && (
          <Grid size={{ xs: 12, sm: 'auto' }}>
            <MultiSelectDropdown
              value={collectionStatus.value}
              onChange={collectionStatus.onChange}
              options={collectionStatus.options}
              label={collectionStatus.label}
              placeholder={collectionStatus.placeholder}
              minWidth={collectionStatus.minWidth || 180}
              fullWidth={collectionStatus.fullWidth !== false}
              loading={collectionStatus.loading}
              disabled={collectionStatus.disabled}
            />
          </Grid>
        )}
      </Grid>
    </Box>
  );
};

/**
 * Memoized CollectorFiltersSection component
 * Prevents unnecessary re-renders when collector filter configuration hasn't changed
 */
export const CollectorFiltersSection = React.memo(CollectorFiltersSectionComponent);