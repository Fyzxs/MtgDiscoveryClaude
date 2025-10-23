import React from 'react';
import { type SxProps, type Theme } from '../../atoms';
import { PageContainer, Section } from '../../molecules/layouts';

interface DetailTemplateProps {
  /** Breadcrumb navigation for context and back navigation */
  breadcrumb?: React.ReactNode;

  /** Main header with title, subtitle, and primary actions */
  header: React.ReactNode;

  /** Hero section for main image, key stats, and primary information */
  heroSection?: React.ReactNode;

  /** Main content area with tabs, sections, or detailed information */
  mainContent: React.ReactNode;

  /** Sidebar content for related items, quick actions, or metadata */
  sidebar?: React.ReactNode;

  /** Related content section for similar items or recommendations */
  relatedContent?: React.ReactNode;

  /** Actions area for save, share, collection actions, etc. */
  actions?: React.ReactNode;

  /** Container max width (defaults to false for full width) */
  maxWidth?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | false;

  /** Container padding overrides */
  containerPadding?: {
    mt?: number;
    mb?: number;
    px?: number;
  };

  /** Additional container styles */
  containerSx?: SxProps<Theme>;

  /** Layout mode - single column or with sidebar */
  layout?: 'single' | 'sidebar';

  /** Whether to show sidebar on mobile (moves to bottom if false) */
  mobileSidebar?: boolean;
}

/**
 * DetailTemplate - A reusable template for detail view pages
 *
 * Provides a consistent layout structure for individual item detail pages
 * like card details, set details, artist details, etc.
 *
 * Key features:
 * - Flexible layout with optional sidebar
 * - Responsive design (sidebar moves to bottom on mobile)
 * - Hero section for primary visual/stats
 * - Related content recommendations
 * - Action areas for user interactions
 * - Breadcrumb navigation for context
 *
 * Layout Structure:
 * [Breadcrumb Navigation]
 * [Main Header with Actions]
 * [Hero Section - Image/Key Info]
 * [Main Content]               [Sidebar Content]
 * [Related Content Section]
 * [Actions Area]
 *
 * @example
 * <DetailTemplate
 *   breadcrumb={
 *     <Breadcrumbs>
 *       <Link to="/sets">Sets</Link>
 *       <Typography color="text.primary">Dominaria United</Typography>
 *     </Breadcrumbs>
 *   }
 *   header={
 *     <Box>
 *       <Typography variant="h2">Dominaria United</Typography>
 *       <Typography variant="subtitle1">Standard Legal Set</Typography>
 *     </Box>
 *   }
 *   heroSection={
 *     <Box sx={{ display: 'flex', gap: 3 }}>
 *       <SetIcon code="DMU" size="large" />
 *       <Box>
 *         <Typography>Release Date: September 9, 2022</Typography>
 *         <Typography>281 Cards</Typography>
 *       </Box>
 *     </Box>
 *   }
 *   mainContent={
 *     <Tabs value={tab} onChange={setTab}>
 *       <Tab label="Cards" />
 *       <Tab label="Statistics" />
 *     </Tabs>
 *   }
 *   sidebar={
 *     <Box>
 *       <Typography variant="h6">Quick Stats</Typography>
 *       <List>...</List>
 *     </Box>
 *   }
 *   relatedContent={
 *     <Box>
 *       <Typography variant="h6">Related Sets</Typography>
 *       <Grid>...</Grid>
 *     </Box>
 *   }
 *   layout="sidebar"
 * />
 */
export const DetailTemplate: React.FC<DetailTemplateProps> = ({
  breadcrumb,
  header,
  heroSection,
  mainContent,
  sidebar,
  relatedContent,
  actions,
  maxWidth = false,
  containerPadding = { mt: 2, mb: 4, px: 3 },
  containerSx = {},
  layout = 'single',
  mobileSidebar = false
}) => {
  const hasBreadcrumb = Boolean(breadcrumb);
  const hasHeroSection = Boolean(heroSection);
  const hasSidebar = Boolean(sidebar) && layout === 'sidebar';
  const hasRelatedContent = Boolean(relatedContent);
  const hasActions = Boolean(actions);

  return (
    <PageContainer
      maxWidth={maxWidth}
      sx={{
        mt: containerPadding.mt,
        mb: containerPadding.mb,
        px: containerPadding.px,
        mx: 'auto',
        ...containerSx
      }}
    >
      {/* Breadcrumb Navigation */}
      {hasBreadcrumb && (
        <Section
          component="nav"
          label="Breadcrumb navigation"
          asSection={false}
          sx={{
            mb: 2,
            display: 'flex',
            justifyContent: 'flex-start'
          }}
        >
          {breadcrumb}
        </Section>
      )}

      {/* Main Header Section */}
      <Section
        component="header"
        asSection={false}
        sx={{
          mb: hasHeroSection ? 3 : 4,
          display: 'flex',
          justifyContent: 'center',
          textAlign: 'center'
        }}
      >
        {header}
      </Section>

      {/* Hero Section */}
      {hasHeroSection && (
        <Section
          label="Hero information"
          sx={{
            mb: 4,
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            flexDirection: { xs: 'column', sm: 'row' },
            gap: { xs: 2, sm: 4 },
            py: 2
          }}
        >
          {heroSection}
        </Section>
      )}

      {/* Main Layout Container */}
      <Section asSection={false} sx={{
        display: hasSidebar ? 'flex' : 'block',
        gap: hasSidebar ? 4 : 0,
        alignItems: 'flex-start',
        flexDirection: { xs: 'column', md: hasSidebar ? 'row' : 'column' }
      }}>

        {/* Main Content Area */}
        <Section
          component="main"
          asSection={false}
          sx={{
            flex: hasSidebar ? 1 : 'none',
            width: hasSidebar ? { xs: '100%', md: 'auto' } : '100%',
            order: { xs: 1, md: 1 }
          }}
        >
          {mainContent}
        </Section>

        {/* Sidebar Content */}
        {hasSidebar && (
          <Section
            component="aside"
            asSection={false}
            sx={{
              flexShrink: 0,
              width: { xs: '100%', md: 320 },
              order: { xs: mobileSidebar ? 2 : 3, md: 2 },
              mt: { xs: 4, md: 0 }
            }}
          >
            {sidebar}
          </Section>
        )}
      </Section>

      {/* Related Content Section */}
      {hasRelatedContent && (
        <Section
          label="Related content"
          sx={{
            mt: 6,
            mb: hasActions ? 4 : 0,
            order: { xs: mobileSidebar && hasSidebar ? 2 : 4, md: 3 }
          }}
        >
          {relatedContent}
        </Section>
      )}

      {/* Actions Area */}
      {hasActions && (
        <Section
          label="Page actions"
          sx={{
            mt: 4,
            display: 'flex',
            justifyContent: 'center',
            gap: 2,
            flexWrap: 'wrap',
            order: { xs: 4, md: 4 }
          }}
        >
          {actions}
        </Section>
      )}
    </PageContainer>
  );
};