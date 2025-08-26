import React from 'react';
import { Container, Box, SxProps, Theme } from '@mui/material';
import { PageHeader, PageHeaderProps } from '../organisms/PageHeader';
import { FilteredCardView, FilteredItem, FilterConfig } from '../organisms/FilteredCardView';
import { SearchFilter } from '../molecules/shared/SearchBar';

interface GridViewProps<T extends FilteredItem> extends Omit<PageHeaderProps, 'children'> {
  items: T[];
  renderItem: (item: T, index: number) => React.ReactNode;
  searchValue: string;
  onSearchChange: (value: string) => void;
  onSearch: (query: string) => void;
  searchPlaceholder?: string;
  filters?: FilterConfig[];
  activeSearchFilters?: SearchFilter[];
  onSearchFilterRemove?: (filterKey: string) => void;
  isLoading?: boolean;
  emptyState?: React.ReactNode;
  maxWidth?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | false;
  containerSx?: SxProps<Theme>;
  contentSx?: SxProps<Theme>;
  className?: string;
}

/**
 * Complete page template for grid-based layouts
 * Combines PageHeader with FilteredCardView for consistent page structure
 */
export const GridView = <T extends FilteredItem>({
  // Page Header Props
  title,
  subtitle,
  description,
  breadcrumbs,
  avatar,
  badges,
  stats,
  actions,
  showDivider = true,
  
  // Content Props
  items,
  renderItem,
  searchValue,
  onSearchChange,
  onSearch,
  searchPlaceholder,
  filters,
  activeSearchFilters,
  onSearchFilterRemove,
  isLoading,
  emptyState,
  
  // Layout Props
  maxWidth = 'xl',
  containerSx = {},
  contentSx = {},
  sx = {},
  className = ''
}: GridViewProps<T>) => {
  return (
    <Container 
      maxWidth={maxWidth}
      className={className}
      sx={{
        py: 3,
        ...containerSx
      }}
    >
      <PageHeader
        title={title}
        subtitle={subtitle}
        description={description}
        breadcrumbs={breadcrumbs}
        avatar={avatar}
        badges={badges}
        stats={stats}
        actions={actions}
        showDivider={showDivider}
        sx={sx}
      />
      
      <Box sx={contentSx}>
        <FilteredCardView
          items={items}
          renderItem={renderItem}
          searchValue={searchValue}
          onSearchChange={onSearchChange}
          onSearch={onSearch}
          searchPlaceholder={searchPlaceholder}
          filters={filters}
          activeSearchFilters={activeSearchFilters}
          onSearchFilterRemove={onSearchFilterRemove}
          isLoading={isLoading}
          emptyState={emptyState}
          showHeader={false} // Header is handled by PageHeader
          enableScrollToTop={true}
        />
      </Box>
    </Container>
  );
};

interface SimpleGridViewProps<T extends FilteredItem> {
  title: string;
  subtitle?: string;
  items: T[];
  renderItem: (item: T, index: number) => React.ReactNode;
  searchValue: string;
  onSearchChange: (value: string) => void;
  isLoading?: boolean;
  maxWidth?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | false;
  className?: string;
}

/**
 * Simplified grid view template for basic use cases
 */
export const SimpleGridView = <T extends FilteredItem>({
  title,
  subtitle,
  items,
  renderItem,
  searchValue,
  onSearchChange,
  isLoading = false,
  maxWidth = 'xl',
  className = ''
}: SimpleGridViewProps<T>) => {
  const handleSearch = (query: string) => {
    onSearchChange(query);
  };

  return (
    <GridView
      title={title}
      subtitle={subtitle}
      items={items}
      renderItem={renderItem}
      searchValue={searchValue}
      onSearchChange={onSearchChange}
      onSearch={handleSearch}
      isLoading={isLoading}
      maxWidth={maxWidth}
      className={className}
    />
  );
};

interface CollectionGridViewProps<T extends FilteredItem> {
  title: string;
  items: T[];
  renderItem: (item: T, index: number) => React.ReactNode;
  totalCount?: number;
  selectedCount?: number;
  searchValue: string;
  onSearchChange: (value: string) => void;
  onSearch: (query: string) => void;
  filters?: FilterConfig[];
  isLoading?: boolean;
  onClearAll?: () => void;
  onSelectAll?: () => void;
  className?: string;
}

/**
 * Specialized grid view for collections with selection capabilities
 */
export const CollectionGridView = <T extends FilteredItem>({
  title,
  items,
  renderItem,
  totalCount,
  selectedCount = 0,
  searchValue,
  onSearchChange,
  onSearch,
  filters,
  isLoading,
  onClearAll,
  onSelectAll,
  className = ''
}: CollectionGridViewProps<T>) => {
  const stats = [];
  if (totalCount !== undefined) {
    stats.push({ label: 'Total', value: totalCount });
  }
  if (selectedCount > 0) {
    stats.push({ label: 'Selected', value: selectedCount, color: 'primary' as const });
  }

  const actions = [];
  if (onSelectAll && selectedCount < items.length) {
    actions.push({
      label: 'Select All',
      onClick: onSelectAll,
      color: 'primary' as const,
      variant: 'contained' as const
    });
  }
  if (onClearAll && selectedCount > 0) {
    actions.push({
      label: 'Clear Selection',
      onClick: onClearAll,
      color: 'secondary' as const,
      variant: 'outlined' as const
    });
  }

  return (
    <GridView
      title={title}
      subtitle={`Manage your ${title.toLowerCase()}`}
      stats={stats}
      actions={actions}
      items={items}
      renderItem={renderItem}
      searchValue={searchValue}
      onSearchChange={onSearchChange}
      onSearch={onSearch}
      filters={filters}
      isLoading={isLoading}
      className={className}
    />
  );
};