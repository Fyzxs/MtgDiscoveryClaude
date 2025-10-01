import React from 'react';
import {
  Typography,
  Box,
  Grid,
  Card,
  CardContent,
  Button,
  Stack,
  Avatar,
  Chip,
  LinearProgress,
  List,
  ListItem,
  ListItemText,
  ListItemAvatar,
  IconButton,
  Divider
} from '@mui/material';
import {
  Add,
  Search,
  TrendingUp,
  Collections,
  Star,
  Visibility,
  MoreVert,
  Timeline,
  Assessment,
  Speed
} from '@mui/icons-material';
import { DashboardTemplate } from './DashboardTemplate';

/**
 * Example usage of DashboardTemplate for a user collection dashboard
 */
export const ExampleCollectionDashboard: React.FC = () => {
  // Mock data for demonstration
  const mockStats = [
    { title: 'Total Cards', value: '1,247', change: '+23 this week', icon: Collections, color: 'primary' },
    { title: 'Unique Cards', value: '892', change: '+15 this week', icon: Star, color: 'secondary' },
    { title: 'Deck Value', value: '$3,420', change: '+$127 this week', icon: TrendingUp, color: 'success' },
    { title: 'Completion Rate', value: '67%', change: '+2% this month', icon: Assessment, color: 'info' }
  ];

  const mockRecentActivity = [
    { action: 'Added 5 cards to collection', set: 'Dominaria United', time: '2 hours ago', avatar: 'D' },
    { action: 'Updated deck "Mono Red Aggro"', set: 'Standard', time: '1 day ago', avatar: 'S' },
    { action: 'Completed set "The Brothers\' War"', set: 'BRO', time: '3 days ago', avatar: 'B' },
    { action: 'Added new deck "Esper Control"', set: 'Standard', time: '1 week ago', avatar: 'E' }
  ];

  const mockTopSets = [
    { name: 'Dominaria United', completion: 85, total: 281, owned: 239 },
    { name: 'The Brothers\' War', completion: 72, total: 287, owned: 207 },
    { name: 'Phyrexia: All Will Be One', completion: 64, total: 271, owned: 173 },
    { name: 'March of the Machine', completion: 51, total: 281, owned: 143 }
  ];

  return (
    <DashboardTemplate
      header={
        <Box>
          <Typography variant="h3" component="h1" gutterBottom>
            Welcome back, Planeswalker!
          </Typography>
          <Typography variant="h6" color="text.secondary">
            Here's your Magic: The Gathering collection overview
          </Typography>
        </Box>
      }
      heroStats={
        <Grid container spacing={3}>
          {mockStats.map((stat, index) => (
            <Grid item xs={12} sm={6} lg={3} key={index}>
              <Card sx={{ height: '100%' }}>
                <CardContent>
                  <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                    <Box
                      sx={{
                        p: 1,
                        borderRadius: 2,
                        bgcolor: `${stat.color}.light`,
                        color: `${stat.color}.dark`,
                        mr: 2
                      }}
                    >
                      <stat.icon />
                    </Box>
                    <Box>
                      <Typography variant="h4" component="div" fontWeight="bold">
                        {stat.value}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        {stat.title}
                      </Typography>
                    </Box>
                  </Box>
                  <Typography variant="caption" color="success.main">
                    {stat.change}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      }
      mainWidgets={
        <Grid container spacing={3}>
          {/* Recent Activity Widget */}
          <Grid item xs={12} lg={8}>
            <Card sx={{ height: '100%' }}>
              <CardContent>
                <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 2 }}>
                  <Typography variant="h6" component="h2">
                    Recent Activity
                  </Typography>
                  <IconButton size="small">
                    <MoreVert />
                  </IconButton>
                </Box>
                <List>
                  {mockRecentActivity.map((activity, index) => (
                    <React.Fragment key={index}>
                      <ListItem alignItems="flex-start" sx={{ px: 0 }}>
                        <ListItemAvatar>
                          <Avatar sx={{ bgcolor: 'primary.main' }}>
                            {activity.avatar}
                          </Avatar>
                        </ListItemAvatar>
                        <ListItemText
                          primary={activity.action}
                          secondary={
                            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mt: 0.5 }}>
                              <Chip label={activity.set} size="small" variant="outlined" />
                              <Typography variant="caption" color="text.secondary">
                                {activity.time}
                              </Typography>
                            </Box>
                          }
                        />
                      </ListItem>
                      {index < mockRecentActivity.length - 1 && <Divider component="li" />}
                    </React.Fragment>
                  ))}
                </List>
              </CardContent>
            </Card>
          </Grid>

          {/* Top Sets Widget */}
          <Grid item xs={12} lg={4}>
            <Card sx={{ height: '100%' }}>
              <CardContent>
                <Typography variant="h6" component="h2" gutterBottom>
                  Set Completion Progress
                </Typography>
                <Stack spacing={2}>
                  {mockTopSets.map((set, index) => (
                    <Box key={index}>
                      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                        <Typography variant="body2" fontWeight="medium">
                          {set.name}
                        </Typography>
                        <Typography variant="caption" color="text.secondary">
                          {set.owned}/{set.total}
                        </Typography>
                      </Box>
                      <LinearProgress
                        variant="determinate"
                        value={set.completion}
                        sx={{ height: 8, borderRadius: 4 }}
                      />
                      <Typography variant="caption" color="text.secondary" sx={{ mt: 0.5, display: 'block' }}>
                        {set.completion}% complete
                      </Typography>
                    </Box>
                  ))}
                </Stack>
              </CardContent>
            </Card>
          </Grid>

          {/* Quick Stats Widget */}
          <Grid item xs={12} md={6}>
            <Card>
              <CardContent>
                <Typography variant="h6" component="h2" gutterBottom>
                  Collection Insights
                </Typography>
                <Grid container spacing={2}>
                  <Grid item xs={6}>
                    <Box sx={{ textAlign: 'center', py: 2 }}>
                      <Typography variant="h4" color="primary.main" fontWeight="bold">
                        23
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Decks Built
                      </Typography>
                    </Box>
                  </Grid>
                  <Grid item xs={6}>
                    <Box sx={{ textAlign: 'center', py: 2 }}>
                      <Typography variant="h4" color="secondary.main" fontWeight="bold">
                        156
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        Rare/Mythics
                      </Typography>
                    </Box>
                  </Grid>
                </Grid>
              </CardContent>
            </Card>
          </Grid>

          {/* Performance Widget */}
          <Grid item xs={12} md={6}>
            <Card>
              <CardContent>
                <Typography variant="h6" component="h2" gutterBottom>
                  Collection Growth
                </Typography>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, py: 2 }}>
                  <Speed sx={{ fontSize: 40, color: 'success.main' }} />
                  <Box>
                    <Typography variant="h5" fontWeight="bold" color="success.main">
                      +12%
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Growth this month
                    </Typography>
                  </Box>
                </Box>
                <Typography variant="caption" color="text.secondary">
                  Your collection is growing faster than 78% of users
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      }
      secondaryContent={
        <Box>
          <Typography variant="h6" component="h2" gutterBottom>
            Collection Notifications
          </Typography>
          <Stack spacing={1}>
            <Box sx={{ p: 2, bgcolor: 'info.light', borderRadius: 1 }}>
              <Typography variant="body2">
                <strong>New set released!</strong> March of the Machine: The Aftermath is now available
              </Typography>
            </Box>
            <Box sx={{ p: 2, bgcolor: 'warning.light', borderRadius: 1 }}>
              <Typography variant="body2">
                <strong>Price alert:</strong> Lightning Bolt has increased by 15% this week
              </Typography>
            </Box>
            <Box sx={{ p: 2, bgcolor: 'success.light', borderRadius: 1 }}>
              <Typography variant="body2">
                <strong>Milestone achieved!</strong> You've collected 1,000+ cards
              </Typography>
            </Box>
          </Stack>
        </Box>
      }
      quickActions={
        <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
          <Button
            variant="contained"
            startIcon={<Add />}
            size="large"
          >
            Add Cards
          </Button>
          <Button
            variant="outlined"
            startIcon={<Search />}
            size="large"
          >
            Search Collection
          </Button>
          <Button
            variant="outlined"
            startIcon={<Collections />}
            size="large"
          >
            Build Deck
          </Button>
          <Button
            variant="outlined"
            startIcon={<Timeline />}
            size="large"
          >
            View Analytics
          </Button>
        </Stack>
      }
      layout="single"
    />
  );
};

/**
 * Example usage with sidebar for analytics dashboard
 */
export const ExampleAnalyticsDashboard: React.FC = () => {
  return (
    <DashboardTemplate
      header={
        <Box>
          <Typography variant="h3" component="h1" gutterBottom>
            Collection Analytics
          </Typography>
          <Typography variant="subtitle1" color="text.secondary">
            Detailed insights into your Magic collection
          </Typography>
        </Box>
      }
      heroStats={
        <Grid container spacing={2}>
          <Grid item xs={6} md={3}>
            <Card sx={{ textAlign: 'center', py: 2 }}>
              <CardContent>
                <Typography variant="h4" color="primary.main">
                  $4,200
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Total Value
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={6} md={3}>
            <Card sx={{ textAlign: 'center', py: 2 }}>
              <CardContent>
                <Typography variant="h4" color="secondary.main">
                  89%
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Standard Legal
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={6} md={3}>
            <Card sx={{ textAlign: 'center', py: 2 }}>
              <CardContent>
                <Typography variant="h4" color="success.main">
                  +23%
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Monthly Growth
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={6} md={3}>
            <Card sx={{ textAlign: 'center', py: 2 }}>
              <CardContent>
                <Typography variant="h4" color="info.main">
                  142
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Unique Artists
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      }
      mainWidgets={
        <Box sx={{ height: 400, bgcolor: 'grey.100', borderRadius: 2, p: 3 }}>
          <Typography variant="h6" gutterBottom>
            Main Analytics Charts
          </Typography>
          <Typography variant="body1" color="text.secondary" sx={{ textAlign: 'center', mt: 8 }}>
            [Collection value trends, rarity distribution, format breakdown charts would go here]
          </Typography>
        </Box>
      }
      sidebar={
        <Stack spacing={3}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Filters
              </Typography>
              <Stack spacing={2}>
                <Button variant="outlined" fullWidth>
                  Date Range
                </Button>
                <Button variant="outlined" fullWidth>
                  Format Filter
                </Button>
                <Button variant="outlined" fullWidth>
                  Rarity Filter
                </Button>
              </Stack>
            </CardContent>
          </Card>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Export Data
              </Typography>
              <Stack spacing={1}>
                <Button variant="text" size="small" fullWidth>
                  Download CSV
                </Button>
                <Button variant="text" size="small" fullWidth>
                  Export PDF Report
                </Button>
              </Stack>
            </CardContent>
          </Card>
        </Stack>
      }
      quickActions={
        <Stack direction="row" spacing={2}>
          <Button variant="contained" startIcon={<Assessment />}>
            Generate Report
          </Button>
          <Button variant="outlined" startIcon={<Visibility />}>
            View Details
          </Button>
        </Stack>
      }
      layout="sidebar"
      mobileSidebar={true}
    />
  );
};

/**
 * Minimal dashboard example with just widgets
 */
export const ExampleMinimalDashboard: React.FC = () => {
  return (
    <DashboardTemplate
      header={
        <Typography variant="h4" component="h1">
          Quick Dashboard
        </Typography>
      }
      mainWidgets={
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Widget 1
                </Typography>
                <Box sx={{
                  height: 200,
                  bgcolor: 'primary.light',
                  borderRadius: 1,
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center'
                }}>
                  <Typography variant="body1" color="primary.dark">
                    Primary Dashboard Widget
                  </Typography>
                </Box>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} md={6}>
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Widget 2
                </Typography>
                <Box sx={{
                  height: 200,
                  bgcolor: 'secondary.light',
                  borderRadius: 1,
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center'
                }}>
                  <Typography variant="body1" color="secondary.dark">
                    Secondary Dashboard Widget
                  </Typography>
                </Box>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      }
      quickActions={
        <Button variant="contained" startIcon={<Add />}>
          Primary Action
        </Button>
      }
      layout="single"
    />
  );
};