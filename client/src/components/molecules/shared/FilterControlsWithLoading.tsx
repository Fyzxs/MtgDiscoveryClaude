import React from 'react';
import { LoadingModal } from '../feedback/LoadingModal';

interface FilterControlsWithLoadingProps {
  children: React.ReactNode;
  isLoading: boolean;
  loadingMessage?: string;
}

/**
 * FilterControlsWithLoading - Wraps filter controls and shows a loading modal during filter/sort operations
 *
 * Features:
 * - Shows loading modal while filters are being applied
 * - Prevents UI blocking during expensive filter/sort operations
 *
 * Usage:
 * <FilterControlsWithLoading isLoading={isPending}>
 *   <ArtistPageFilters filters={...} />
 * </FilterControlsWithLoading>
 */
export const FilterControlsWithLoading: React.FC<FilterControlsWithLoadingProps> = React.memo(({
  children,
  isLoading,
  loadingMessage = 'Applying filters and sorting...'
}) => {
  return (
    <>
      {children}
      <LoadingModal open={isLoading} message={loadingMessage} />
    </>
  );
});

FilterControlsWithLoading.displayName = 'FilterControlsWithLoading';
