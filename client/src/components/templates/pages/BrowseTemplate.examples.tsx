import React from 'react';
import { Typography, Box, Pagination, Card, CardContent, Stack } from '@mui/material';
import { BrowseTemplate } from './BrowseTemplate';
import { FilterPanel } from '../../organisms/filters/FilterPanel';
import { ResultsSummary } from '../../molecules/shared/ResultsSummary';
import { ResponsiveGridAutoFit } from '../../atoms/layouts/ResponsiveGrid';

/**
 * Example usage of BrowseTemplate for a sets page
 */
export const ExampleSetsPage: React.FC = () => {
  // Mock data for demonstration
  const mockFilterConfig = {
    search: {
      value: '',
      onChange: (value: string) => console.log('Search:', value),
      placeholder: 'Search sets...',
      debounceMs: 300
    },
    multiSelects: [
      {
        key: 'setTypes',
        value: [],
        onChange: (value: string[]) => console.log('Set types:', value),
        options: ['Core', 'Expansion', 'Masters'],
        label: 'Set Types',
        placeholder: 'All Types'
      }
    ],
    sort: {
      value: 'release-desc',
      onChange: (value: string) => console.log('Sort:', value),
      options: [
        { value: 'release-desc', label: 'Release Date (Newest)' },
        { value: 'release-asc', label: 'Release Date (Oldest)' },
        { value: 'name-asc', label: 'Name (A-Z)' }
      ]
    }
  };

  const mockSets = Array.from({ length: 12 }, (_, i) => ({
    id: i + 1,
    name: `Set ${i + 1}`,
    code: `SET${i + 1}`,
    type: 'Expansion'
  }));

  return (
    <BrowseTemplate
      header={
        <Typography variant="h3" component="h1" gutterBottom sx={{ textAlign: 'center' }}>
          All Magic: The Gathering Sets
        </Typography>
      }
      filters={
        <FilterPanel
          config={mockFilterConfig}
          layout="horizontal"
        />
      }
      summary={
        <ResultsSummary
          current={12}
          total={617}
          label="sets"
          textAlign="center"
        />
      }
      content={
        <ResponsiveGridAutoFit minItemWidth={240} spacing={1.5}>
          {mockSets.map(set => (
            <Card key={set.id} sx={{ cursor: 'pointer' }}>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  {set.name}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  {set.code} • {set.type}
                </Typography>
              </CardContent>
            </Card>
          ))}
        </ResponsiveGridAutoFit>
      }
      pagination={
        <Pagination count={25} color="primary" />
      }
    />
  );
};

/**
 * Example usage with sidebar
 */
export const ExampleBrowseWithSidebar: React.FC = () => {
  return (
    <BrowseTemplate
      header={
        <Typography variant="h3" component="h1" gutterBottom>
          Advanced Browse Example
        </Typography>
      }
      sidebar={
        <Stack spacing={2}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Quick Filters
              </Typography>
              <Typography variant="body2">
                Sidebar content for additional filters or information
              </Typography>
            </CardContent>
          </Card>
        </Stack>
      }
      summary={
        <ResultsSummary
          current={25}
          total={100}
          label="items"
          textAlign="left"
        />
      }
      content={
        <Box sx={{ p: 3, border: '1px dashed', borderColor: 'divider' }}>
          <Typography variant="body1" color="text.secondary" sx={{ textAlign: 'center' }}>
            Main content area with sidebar layout
          </Typography>
        </Box>
      }
    />
  );
};

/**
 * Minimal example - just header and content
 */
export const ExampleMinimalBrowse: React.FC = () => {
  return (
    <BrowseTemplate
      header={
        <Typography variant="h4" component="h1">
          Simple Browse Page
        </Typography>
      }
      content={
        <Box sx={{
          p: 6,
          textAlign: 'center',
          border: '2px dashed',
          borderColor: 'primary.main',
          borderRadius: 2
        }}>
          <Typography variant="h6" color="primary">
            Main Content Area
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 2 }}>
            This demonstrates the minimal usage of BrowseTemplate with just header and content
          </Typography>
        </Box>
      }
    />
  );
};