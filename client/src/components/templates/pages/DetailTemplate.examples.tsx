import React, { useState } from 'react';
import {
  Typography,
  Box,
  Breadcrumbs,
  Link,
  Tabs,
  Tab,
  Button,
  Card,
  CardContent,
  List,
  ListItem,
  ListItemText,
  Avatar,
  Chip,
  Stack,
  Paper,
  Divider
} from '@mui/material';
import ShareIcon from '@mui/icons-material/Share';
import FavoriteIcon from '@mui/icons-material/Favorite';
import BookmarkIcon from '@mui/icons-material/Bookmark';
import { DetailTemplate } from './DetailTemplate';
import { ResponsiveGridAutoFit } from '../../atoms/layouts/ResponsiveGrid';

/**
 * Example usage of DetailTemplate for a card detail page
 */
export const ExampleCardDetailPage: React.FC = () => {
  const [tabValue, setTabValue] = useState(0);

  return (
    <DetailTemplate
      breadcrumb={
        <Breadcrumbs aria-label="breadcrumb">
          <Link underline="hover" color="inherit" href="/cards">
            Cards
          </Link>
          <Link underline="hover" color="inherit" href="/cards?search=lightning">
            Search Results
          </Link>
          <Typography color="text.primary">Lightning Bolt</Typography>
        </Breadcrumbs>
      }
      header={
        <Box>
          <Typography variant="h2" fontWeight="bold" gutterBottom>
            Lightning Bolt
          </Typography>
          <Typography variant="h6" color="text.secondary">
            Instant • Mana Cost: R
          </Typography>
        </Box>
      }
      heroSection={
        <Box sx={{ display: 'flex', gap: 4, alignItems: 'center' }}>
          {/* Mock card image */}
          <Paper
            sx={{
              width: 200,
              height: 279,
              bgcolor: 'grey.200',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              borderRadius: 2
            }}
          >
            <Typography variant="body2" color="text.secondary">
              Card Image
            </Typography>
          </Paper>
          <Box>
            <Typography variant="h6" gutterBottom>Quick Stats</Typography>
            <Stack spacing={1}>
              <Chip label="Rare" color="warning" size="small" />
              <Typography variant="body2">
                <strong>Artist:</strong> Christopher Rush
              </Typography>
              <Typography variant="body2">
                <strong>Set:</strong> Alpha (LEA)
              </Typography>
              <Typography variant="body2">
                <strong>Collector #:</strong> 161
              </Typography>
            </Stack>
          </Box>
        </Box>
      }
      mainContent={
        <Box>
          <Tabs value={tabValue} onChange={(_, newValue) => setTabValue(newValue)} sx={{ mb: 3 }}>
            <Tab label="All Printings" />
            <Tab label="Rules Text" />
            <Tab label="Price History" />
          </Tabs>

          <Box role="tabpanel" hidden={tabValue !== 0}>
            {tabValue === 0 && (
              <ResponsiveGridAutoFit minItemWidth={220} spacing={2}>
                {Array.from({ length: 8 }, (_, i) => (
                  <Card key={i}>
                    <CardContent>
                      <Typography variant="h6">Set {i + 1}</Typography>
                      <Typography variant="body2" color="text.secondary">
                        Release Date: 2023-{String(i + 1).padStart(2, '0')}-01
                      </Typography>
                    </CardContent>
                  </Card>
                ))}
              </ResponsiveGridAutoFit>
            )}
          </Box>

          <Box role="tabpanel" hidden={tabValue !== 1}>
            {tabValue === 1 && (
              <Card>
                <CardContent>
                  <Typography variant="h6" gutterBottom>Rules Text</Typography>
                  <Typography variant="body1">
                    Lightning Bolt deals 3 damage to any target.
                  </Typography>
                  <Divider sx={{ my: 2 }} />
                  <Typography variant="body2" color="text.secondary" fontStyle="italic">
                    "The sparkmage shrieked, calling on the rage of the storms of his youth.
                    To his surprise, the sky responded with a fierce energy he'd never experienced."
                  </Typography>
                </CardContent>
              </Card>
            )}
          </Box>

          <Box role="tabpanel" hidden={tabValue !== 2}>
            {tabValue === 2 && (
              <Card>
                <CardContent>
                  <Typography variant="h6" gutterBottom>Price Trends</Typography>
                  <Typography variant="body2" color="text.secondary">
                    Price history chart would go here
                  </Typography>
                </CardContent>
              </Card>
            )}
          </Box>
        </Box>
      }
      sidebar={
        <Box>
          <Card sx={{ mb: 3 }}>
            <CardContent>
              <Typography variant="h6" gutterBottom>Legality</Typography>
              <Stack spacing={1}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                  <Typography variant="body2">Standard</Typography>
                  <Chip label="Legal" color="success" size="small" />
                </Box>
                <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                  <Typography variant="body2">Modern</Typography>
                  <Chip label="Legal" color="success" size="small" />
                </Box>
                <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                  <Typography variant="body2">Legacy</Typography>
                  <Chip label="Legal" color="success" size="small" />
                </Box>
              </Stack>
            </CardContent>
          </Card>

          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>Collection</Typography>
              <Typography variant="body2" color="text.secondary" gutterBottom>
                You own 3 copies
              </Typography>
              <Button variant="outlined" size="small" fullWidth>
                Add to Collection
              </Button>
            </CardContent>
          </Card>
        </Box>
      }
      relatedContent={
        <Box>
          <Typography variant="h5" gutterBottom sx={{ textAlign: 'center' }}>
            Similar Cards
          </Typography>
          <ResponsiveGridAutoFit minItemWidth={200} spacing={2}>
            {Array.from({ length: 4 }, (_, i) => (
              <Card key={i}>
                <CardContent>
                  <Typography variant="h6">Lightning Strike</Typography>
                  <Typography variant="body2" color="text.secondary">
                    Similar instant spell
                  </Typography>
                </CardContent>
              </Card>
            ))}
          </ResponsiveGridAutoFit>
        </Box>
      }
      actions={
        <>
          <Button variant="contained" startIcon={<FavoriteIcon />}>
            Add to Favorites
          </Button>
          <Button variant="outlined" startIcon={<BookmarkIcon />}>
            Add to Deck
          </Button>
          <Button variant="outlined" startIcon={<ShareIcon />}>
            Share
          </Button>
        </>
      }
      layout="sidebar"
      mobileSidebar={true}
    />
  );
};

/**
 * Example usage of DetailTemplate for a set detail page
 */
export const ExampleSetDetailPage: React.FC = () => {
  const [tabValue, setTabValue] = useState(0);

  return (
    <DetailTemplate
      breadcrumb={
        <Breadcrumbs aria-label="breadcrumb">
          <Link underline="hover" color="inherit" href="/sets">
            Sets
          </Link>
          <Typography color="text.primary">Dominaria United</Typography>
        </Breadcrumbs>
      }
      header={
        <Box>
          <Typography variant="h2" fontWeight="bold" gutterBottom>
            Dominaria United
          </Typography>
          <Typography variant="h6" color="text.secondary">
            Standard Legal Set • 281 Cards
          </Typography>
        </Box>
      }
      heroSection={
        <Box sx={{ display: 'flex', gap: 4, alignItems: 'center' }}>
          <Avatar
            sx={{
              width: 80,
              height: 80,
              bgcolor: 'primary.main',
              fontSize: '2rem',
              fontWeight: 'bold'
            }}
          >
            DMU
          </Avatar>
          <Box>
            <Typography variant="h6" gutterBottom>Set Information</Typography>
            <Stack spacing={1}>
              <Typography variant="body2">
                <strong>Release Date:</strong> September 9, 2022
              </Typography>
              <Typography variant="body2">
                <strong>Set Type:</strong> Expansion
              </Typography>
              <Typography variant="body2">
                <strong>Block:</strong> Dominaria United
              </Typography>
            </Stack>
          </Box>
        </Box>
      }
      mainContent={
        <Box>
          <Tabs value={tabValue} onChange={(_, newValue) => setTabValue(newValue)} sx={{ mb: 3 }}>
            <Tab label="All Cards" />
            <Tab label="By Rarity" />
            <Tab label="Statistics" />
          </Tabs>

          <Box role="tabpanel" hidden={tabValue !== 0}>
            {tabValue === 0 && (
              <ResponsiveGridAutoFit minItemWidth={200} spacing={2}>
                {Array.from({ length: 12 }, (_, i) => (
                  <Card key={i}>
                    <CardContent>
                      <Typography variant="h6">Card {i + 1}</Typography>
                      <Typography variant="body2" color="text.secondary">
                        Creature • 2/2
                      </Typography>
                    </CardContent>
                  </Card>
                ))}
              </ResponsiveGridAutoFit>
            )}
          </Box>

          <Box role="tabpanel" hidden={tabValue !== 1}>
            {tabValue === 1 && (
              <Box>
                <Typography variant="h6" gutterBottom>Cards by Rarity</Typography>
                <Typography variant="body2" color="text.secondary">
                  Rarity breakdown would go here
                </Typography>
              </Box>
            )}
          </Box>

          <Box role="tabpanel" hidden={tabValue !== 2}>
            {tabValue === 2 && (
              <Box>
                <Typography variant="h6" gutterBottom>Set Statistics</Typography>
                <Typography variant="body2" color="text.secondary">
                  Statistical analysis would go here
                </Typography>
              </Box>
            )}
          </Box>
        </Box>
      }
      relatedContent={
        <Box>
          <Typography variant="h5" gutterBottom sx={{ textAlign: 'center' }}>
            Related Sets
          </Typography>
          <ResponsiveGridAutoFit minItemWidth={250} spacing={2}>
            {Array.from({ length: 3 }, (_, i) => (
              <Card key={i}>
                <CardContent>
                  <Typography variant="h6">Related Set {i + 1}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    Part of the same block or cycle
                  </Typography>
                </CardContent>
              </Card>
            ))}
          </ResponsiveGridAutoFit>
        </Box>
      }
      actions={
        <>
          <Button variant="contained">
            View All Cards
          </Button>
          <Button variant="outlined" startIcon={<BookmarkIcon />}>
            Track Set
          </Button>
          <Button variant="outlined" startIcon={<ShareIcon />}>
            Share Set
          </Button>
        </>
      }
      layout="single"
    />
  );
};

/**
 * Example usage of DetailTemplate for an artist detail page
 */
export const ExampleArtistDetailPage: React.FC = () => {
  return (
    <DetailTemplate
      breadcrumb={
        <Breadcrumbs aria-label="breadcrumb">
          <Link underline="hover" color="inherit" href="/artists">
            Artists
          </Link>
          <Typography color="text.primary">Rebecca Guay</Typography>
        </Breadcrumbs>
      }
      header={
        <Box>
          <Typography variant="h2" fontWeight="bold" gutterBottom>
            Rebecca Guay
          </Typography>
          <Typography variant="h6" color="text.secondary">
            Magic: The Gathering Artist
          </Typography>
        </Box>
      }
      heroSection={
        <Box sx={{ display: 'flex', gap: 4, alignItems: 'center' }}>
          <Avatar
            sx={{
              width: 120,
              height: 120,
              bgcolor: 'secondary.main'
            }}
          >
            RG
          </Avatar>
          <Box>
            <Typography variant="h6" gutterBottom>Artist Stats</Typography>
            <Stack spacing={1}>
              <Typography variant="body2">
                <strong>Cards Illustrated:</strong> 127
              </Typography>
              <Typography variant="body2">
                <strong>First Card:</strong> Angelic Page (1996)
              </Typography>
              <Typography variant="body2">
                <strong>Most Recent:</strong> Rhystic Study (2020)
              </Typography>
            </Stack>
          </Box>
        </Box>
      }
      mainContent={
        <Box>
          <Typography variant="h5" gutterBottom>
            Artwork Gallery
          </Typography>
          <ResponsiveGridAutoFit minItemWidth={200} spacing={2}>
            {Array.from({ length: 15 }, (_, i) => (
              <Card key={i}>
                <CardContent>
                  <Typography variant="h6">Card {i + 1}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    By Rebecca Guay
                  </Typography>
                </CardContent>
              </Card>
            ))}
          </ResponsiveGridAutoFit>
        </Box>
      }
      sidebar={
        <Box>
          <Card sx={{ mb: 3 }}>
            <CardContent>
              <Typography variant="h6" gutterBottom>Popular Cards</Typography>
              <List dense>
                <ListItem>
                  <ListItemText primary="Rhystic Study" secondary="Most played card" />
                </ListItem>
                <ListItem>
                  <ListItemText primary="Angelic Page" secondary="First illustration" />
                </ListItem>
                <ListItem>
                  <ListItemText primary="Enchantress" secondary="Fan favorite" />
                </ListItem>
              </List>
            </CardContent>
          </Card>

          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>Art Style</Typography>
              <Stack spacing={1}>
                <Chip label="Traditional" size="small" />
                <Chip label="Fantasy" size="small" />
                <Chip label="Detailed" size="small" />
              </Stack>
            </CardContent>
          </Card>
        </Box>
      }
      relatedContent={
        <Box>
          <Typography variant="h5" gutterBottom sx={{ textAlign: 'center' }}>
            Similar Artists
          </Typography>
          <ResponsiveGridAutoFit minItemWidth={200} spacing={2}>
            {Array.from({ length: 4 }, (_, i) => (
              <Card key={i}>
                <CardContent>
                  <Typography variant="h6">Artist {i + 1}</Typography>
                  <Typography variant="body2" color="text.secondary">
                    Similar style and themes
                  </Typography>
                </CardContent>
              </Card>
            ))}
          </ResponsiveGridAutoFit>
        </Box>
      }
      actions={
        <>
          <Button variant="contained">
            View All Artwork
          </Button>
          <Button variant="outlined" startIcon={<FavoriteIcon />}>
            Follow Artist
          </Button>
          <Button variant="outlined" startIcon={<ShareIcon />}>
            Share
          </Button>
        </>
      }
      layout="sidebar"
      mobileSidebar={false}
    />
  );
};