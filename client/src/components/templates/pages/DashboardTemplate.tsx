import React from 'react';
import { Container, Box, type SxProps, type Theme } from '../../atoms';

interface DashboardTemplateProps {
  /** Dashboard header content (welcome message, user info, page title) */
  header?: React.ReactNode;

  /** Hero stats/metrics section (key numbers, progress indicators, summary cards) */
  heroStats?: React.ReactNode;

  /** Main widget grid for dashboard cards and primary content */
  mainWidgets: React.ReactNode;

  /** Secondary content area (recent activity, notifications, updates) */
  secondaryContent?: React.ReactNode;

  /** Quick actions panel (frequently used actions, shortcuts) */
  quickActions?: React.ReactNode;

  /** Optional sidebar content (navigation, secondary info, filters) */
  sidebar?: React.ReactNode;

  /** Container max width (defaults to false for full width dashboards) */
  maxWidth?: false | 'xs' | 'sm' | 'md' | 'lg' | 'xl';

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

  /** Widget grid spacing */
  widgetSpacing?: number;
}

/**
 * DashboardTemplate - A reusable template for dashboard/overview pages
 *
 * Provides a consistent layout structure for dashboard pages that display
 * collections of widgets, metrics, and summary information. Ideal for user
 * collections, homepage, analytics views, and overview screens.
 *
 * Key features:
 * - Flexible widget-based layout with responsive grid
 * - Hero stats section for key metrics and progress indicators
 * - Secondary content area for activity feeds and notifications
 * - Quick actions panel for frequently used functions
 * - Optional sidebar for navigation or secondary information
 * - Responsive design that adapts to screen size
 * - Support for both single-column and sidebar layouts
 *
 * Layout Structure:
 * [Dashboard Header]
 * [Hero Stats/Metrics]
 * [Main Widget Grid]          [Optional Sidebar]
 * [Secondary Content Area]
 * [Quick Actions Panel]
 *
 * @example
 * <DashboardTemplate
 *   header={
 *     <Box>
 *       <Typography variant="h3">Welcome back, John!</Typography>
 *       <Typography variant="subtitle1">Here's your collection overview</Typography>
 *     </Box>
 *   }
 *   heroStats={
 *     <Grid container spacing={3}>
 *       <Grid item xs={12} sm={6} md={3}>
 *         <StatCard title="Total Cards" value="1,247" />
 *       </Grid>
 *       <Grid item xs={12} sm={6} md={3}>
 *         <StatCard title="Unique Cards" value="892" />
 *       </Grid>
 *     </Grid>
 *   }
 *   mainWidgets={
 *     <Grid container spacing={3}>
 *       <Grid item xs={12} lg={8}>
 *         <RecentActivityWidget />
 *       </Grid>
 *       <Grid item xs={12} lg={4}>
 *         <TopSetsWidget />
 *       </Grid>
 *     </Grid>
 *   }
 *   secondaryContent={
 *     <NotificationFeed />
 *   }
 *   quickActions={
 *     <Stack direction="row" spacing={2}>
 *       <Button startIcon={<Add />}>Add Cards</Button>
 *       <Button startIcon={<Search />}>Search Collection</Button>
 *     </Stack>
 *   }
 *   layout="single"
 * />
 */
export const DashboardTemplate: React.FC<DashboardTemplateProps> = ({
  header,
  heroStats,
  mainWidgets,
  secondaryContent,
  quickActions,
  sidebar,
  maxWidth = false,
  containerPadding = { mt: 2, mb: 4, px: 3 },
  containerSx = {},
  layout = 'single',
  mobileSidebar = false,
  widgetSpacing = 3
}) => {
  const hasHeader = Boolean(header);
  const hasHeroStats = Boolean(heroStats);
  const hasSecondaryContent = Boolean(secondaryContent);
  const hasQuickActions = Boolean(quickActions);
  const hasSidebar = Boolean(sidebar) && layout === 'sidebar';

  return (
    <Container
      maxWidth={maxWidth}
      sx={{
        mt: containerPadding.mt,
        mb: containerPadding.mb,
        px: containerPadding.px,
        mx: 'auto',
        ...containerSx
      }}
    >
      {/* Dashboard Header Section */}
      {hasHeader && (
        <Box
          component="header"
          sx={{
            mb: hasHeroStats ? 4 : widgetSpacing,
            display: 'flex',
            justifyContent: 'center',
            textAlign: 'center'
          }}
        >
          {header}
        </Box>
      )}

      {/* Hero Stats/Metrics Section */}
      {hasHeroStats && (
        <Box
          component="section"
          aria-label="Key metrics and statistics"
          sx={{
            mb: widgetSpacing,
            p: 2,
            borderRadius: 2,
            bgcolor: 'background.paper'
          }}
        >
          {heroStats}
        </Box>
      )}

      {/* Main Layout Container */}
      <Box sx={{
        display: hasSidebar ? 'flex' : 'block',
        gap: hasSidebar ? widgetSpacing : 0,
        alignItems: 'flex-start',
        flexDirection: { xs: 'column', lg: hasSidebar ? 'row' : 'column' }
      }}>

        {/* Main Content Area */}
        <Box
          component="main"
          sx={{
            flex: hasSidebar ? 1 : 'none',
            width: hasSidebar ? { xs: '100%', lg: 'auto' } : '100%',
            order: { xs: 1, lg: 1 }
          }}
        >
          {/* Main Widget Grid */}
          <Box
            component="section"
            aria-label="Main dashboard widgets"
            sx={{
              mb: hasSecondaryContent || hasQuickActions ? widgetSpacing : 0
            }}
          >
            {mainWidgets}
          </Box>

          {/* Secondary Content Area */}
          {hasSecondaryContent && (
            <Box
              component="section"
              aria-label="Secondary content and activity"
              sx={{
                mb: hasQuickActions ? widgetSpacing : 0,
                p: 2,
                borderRadius: 2,
                bgcolor: 'background.paper'
              }}
            >
              {secondaryContent}
            </Box>
          )}

          {/* Quick Actions Panel */}
          {hasQuickActions && (
            <Box
              component="section"
              aria-label="Quick actions"
              sx={{
                p: 2,
                borderRadius: 2,
                bgcolor: 'background.paper',
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                flexWrap: 'wrap',
                gap: 2
              }}
            >
              {quickActions}
            </Box>
          )}
        </Box>

        {/* Sidebar Content */}
        {hasSidebar && (
          <Box
            component="aside"
            sx={{
              flexShrink: 0,
              width: { xs: '100%', lg: 320 },
              order: { xs: mobileSidebar ? 2 : 3, lg: 2 },
              mt: { xs: widgetSpacing, lg: 0 },
              p: 2,
              borderRadius: 2,
              bgcolor: 'background.paper',
              height: 'fit-content'
            }}
          >
            {sidebar}
          </Box>
        )}
      </Box>
    </Container>
  );
};