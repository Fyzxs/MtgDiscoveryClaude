import React from 'react';
import { PageContainer } from '../molecules/layouts';
import { ResultsSummary } from '../molecules/shared/ResultsSummary';
import { BackToTopFab } from '../molecules/shared/BackToTopFab';
import { QueryStateContainer } from '../molecules/shared/QueryStateContainer';

interface SetPageTemplateProps {
  // Query state
  isLoading: boolean;
  error?: Error | null;

  // Content sections
  header: React.ReactNode;
  filters?: React.ReactNode;
  cardDisplay: React.ReactNode;

  // Results summary
  currentCount: number;
  totalCount: number;

  children?: never; // Prevent accidental children
}

export const SetPageTemplate: React.FC<SetPageTemplateProps> = ({
  isLoading,
  error,
  header,
  filters,
  cardDisplay,
  currentCount,
  totalCount
}) => {
  return (
    <QueryStateContainer
      loading={isLoading}
      error={error}
      containerProps={{ maxWidth: false }}
    >
      <PageContainer maxWidth={false} sx={{ mt: 2, mb: 4, px: 3 }}>
        {/* Header Section */}
        {header}

        {/* Filters Section */}
        {filters}

        {/* Results Summary */}
        <ResultsSummary
          current={currentCount}
          total={totalCount}
          label="cards"
          textAlign="center"
        />

        {/* Card Display Section */}
        {cardDisplay}

        {/* Back to Top Button */}
        <BackToTopFab />
      </PageContainer>
    </QueryStateContainer>
  );
};