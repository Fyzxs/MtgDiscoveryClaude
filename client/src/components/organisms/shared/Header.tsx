import React, { useState } from 'react';
import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  Button,
  Menu,
  MenuItem,
  IconButton
} from '../../atoms';
import { useTheme } from '../../atoms';
import { SearchInput } from '../../molecules/shared/SearchInput';
import { AuthButton } from '../../auth/AuthButton';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
import { SearchIcon, ArrowDropDownIcon, MenuIcon, CollectionsBookmarkIcon } from '../../atoms';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';
import { NavigationDrawer } from './NavigationDrawer';

export const Header: React.FC = () => {
  const [setCode, setSetCode] = useState('');
  const [searchAnchorEl, setSearchAnchorEl] = useState<null | HTMLElement>(null);
  const [browseAnchorEl, setBrowseAnchorEl] = useState<null | HTMLElement>(null);
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const theme = useTheme();
  const { buildUrlWithCollector, navigateWithCollector, collectorParam } = useCollectorNavigation();
  const { isMobile, isTablet, isDesktop } = useResponsiveBreakpoints();

  // Show compact header on mobile, tablet, and narrow desktop (up to 1199px)
  const showMobileHeader = isMobile || isTablet || isDesktop;

  const handleSetCodeSubmit = () => {
    if (setCode.trim()) {
      // Navigate to set page while preserving only collector parameter
      // Use replace to avoid polluting browser history with intermediate states
      navigateWithCollector(`/set/${setCode.trim().toLowerCase()}`, undefined, { replace: true });
      setSetCode('');
    }
  };

  const handleSearchMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setSearchAnchorEl(event.currentTarget);
  };

  const handleSearchMenuClose = () => {
    setSearchAnchorEl(null);
  };

  const handleBrowseMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setBrowseAnchorEl(event.currentTarget);
  };

  const handleBrowseMenuClose = () => {
    setBrowseAnchorEl(null);
  };

  // Mobile Header
  if (showMobileHeader) {
    return (
      <>
        <AppBar
          component="header"
          position="sticky"
          role="banner"
          sx={{
            backgroundColor: 'background.paper',
            backgroundImage: 'none',
            borderBottom: '1px solid',
            borderColor: 'divider'
          }}
        >
          <Toolbar
            component="nav"
            role="navigation"
            aria-label="Main navigation"
            sx={{
              gap: 1,
              minHeight: theme.mtg.mobile.headerHeight,
              px: 1,
            }}
          >
            {/* Menu Button */}
            <IconButton
              type="button"
              onClick={() => setMobileMenuOpen(true)}
              aria-label="Open navigation menu"
              sx={{ minWidth: 44, minHeight: 44 }}
            >
              <MenuIcon />
            </IconButton>

            {/* Site Logo/Name - Centered */}
            <Typography
              variant="h6"
              component="button"
              role="button"
              tabIndex={0}
              aria-label="Go to homepage"
              onClick={() => navigateWithCollector('/')}
              sx={{
                flex: 1,
                textAlign: 'center',
                fontWeight: 'bold',
                background: theme.mtg.gradients.header,
                backgroundClip: 'text',
                WebkitBackgroundClip: 'text',
                WebkitTextFillColor: 'transparent',
                cursor: 'pointer',
                border: 'none',
                backgroundColor: 'transparent',
                padding: 0,
              }}
            >
              MtgDiscovery
            </Typography>

            {/* Spacer to balance layout */}
            <Box sx={{ minWidth: 44, minHeight: 44 }} />
          </Toolbar>
        </AppBar>

        {/* Navigation Drawer */}
        <NavigationDrawer
          open={mobileMenuOpen}
          onClose={() => setMobileMenuOpen(false)}
        />
      </>
    );
  }

  // Desktop Header
  return (
    <AppBar
      component="header"
      position="sticky"
      role="banner"
      sx={{
        backgroundColor: 'background.paper',
        backgroundImage: 'none',
        borderBottom: '1px solid',
        borderColor: 'divider'
      }}
    >
      <Toolbar component="nav" role="navigation" aria-label="Main navigation" sx={{ gap: 3 }}>
        {/* Site Logo/Name */}
        <Typography
          variant="h5"
          component="button"
          role="button"
          tabIndex={0}
          aria-label="Go to homepage"
          onKeyDown={(e: React.KeyboardEvent) => {
            if (e.key === 'Enter' || e.key === ' ') {
              e.preventDefault();
              navigateWithCollector('/');
            }
          }}
          sx={{
            fontWeight: 'bold',
            background: theme.mtg.gradients.header,
            backgroundClip: 'text',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
            cursor: 'pointer',
            border: 'none',
            backgroundColor: 'transparent',
            padding: 0,
            '&:hover': {
              background: theme.mtg.gradients.hover,
              backgroundClip: 'text',
              WebkitBackgroundClip: 'text',
              WebkitTextFillColor: 'transparent',
            },
            '&:focus': {
              outline: '2px solid',
              outlineColor: 'primary.main',
              outlineOffset: '2px',
              borderRadius: 1
            }
          }}
          onClick={() => navigateWithCollector('/')}
        >
          MtgDiscovery
        </Typography>

        {/* Navigation Links */}
        <Box sx={{ display: 'flex', gap: 2, alignItems: 'center' }} role="menubar" aria-label="Primary navigation">
          {/* Set Code Search */}
          <Box role="search" aria-label="Quick set search">
            <SearchInput
              value={setCode}
              onChange={setSetCode}
              onSubmit={handleSetCodeSubmit}
              placeholder="Jump to Set"
              label="Set Code"
              expandable={true}
              expandedWidth={200}
              collapsedWidth={150}
              size="small"
            />
          </Box>

          {/* Browse Dropdown */}
          <Button
            color="primary"
            onClick={handleBrowseMenuOpen}
            startIcon={<CollectionsBookmarkIcon />}
            endIcon={<ArrowDropDownIcon />}
            role="menuitem"
            aria-label="Browse options"
            aria-haspopup="true"
            aria-expanded={Boolean(browseAnchorEl)}
            sx={{
              textTransform: 'none',
              fontWeight: 500
            }}
          >
            Browse
          </Button>
          <Menu
            anchorEl={browseAnchorEl}
            open={Boolean(browseAnchorEl)}
            onClose={handleBrowseMenuClose}
            role="menu"
            aria-label="Browse menu"
            anchorOrigin={{
              vertical: 'bottom',
              horizontal: 'left',
            }}
            transformOrigin={{
              vertical: 'top',
              horizontal: 'left',
            }}
          >
            <MenuItem
              component="a"
              href={buildUrlWithCollector('/sets')}
              onClick={(e: React.MouseEvent) => {
                e.preventDefault();
                handleBrowseMenuClose();
                navigateWithCollector('/sets');
              }}
              role="menuitem"
              aria-label="Browse all Magic sets"
              sx={{ textDecoration: 'none', color: 'inherit' }}
            >
              All Sets
            </MenuItem>
            {collectorParam.hasCollector && (
              <MenuItem
                component="a"
                href={buildUrlWithCollector('/wishlist')}
                onClick={(e: React.MouseEvent) => {
                  e.preventDefault();
                  handleBrowseMenuClose();
                  navigateWithCollector('/wishlist');
                }}
                role="menuitem"
                aria-label="View your wishlist"
                sx={{ textDecoration: 'none', color: 'inherit' }}
              >
                Wishlist
              </MenuItem>
            )}
            {collectorParam.hasCollector && (
              <MenuItem
                component="a"
                href={buildUrlWithCollector('/convention-signing')}
                onClick={(e: React.MouseEvent) => {
                  e.preventDefault();
                  handleBrowseMenuClose();
                  navigateWithCollector('/convention-signing');
                }}
                role="menuitem"
                aria-label="Plan cards to get signed at conventions"
                sx={{ textDecoration: 'none', color: 'inherit' }}
              >
                Convention Signing
              </MenuItem>
            )}
          </Menu>

          {/* Search Dropdown */}
          <Button
            color="primary"
            onClick={handleSearchMenuOpen}
            startIcon={<SearchIcon />}
            endIcon={<ArrowDropDownIcon />}
            role="menuitem"
            aria-label="Search options"
            aria-haspopup="true"
            aria-expanded={Boolean(searchAnchorEl)}
            sx={{ 
              textTransform: 'none',
              fontWeight: 500
            }}
          >
            Search
          </Button>
          <Menu
            anchorEl={searchAnchorEl}
            open={Boolean(searchAnchorEl)}
            onClose={handleSearchMenuClose}
            role="menu"
            aria-label="Search menu"
            anchorOrigin={{
              vertical: 'bottom',
              horizontal: 'left',
            }}
            transformOrigin={{
              vertical: 'top',
              horizontal: 'left',
            }}
          >
            <MenuItem
              component="a"
              href={buildUrlWithCollector('/search/cards')}
              onClick={(e: React.MouseEvent) => {
                e.preventDefault();
                handleSearchMenuClose();
                navigateWithCollector('/search/cards');
              }}
              role="menuitem"
              aria-label="Search for Magic cards"
              sx={{ textDecoration: 'none', color: 'inherit' }}
            >
              Cards
            </MenuItem>
            <MenuItem
              component="a"
              href={buildUrlWithCollector('/search/artists')}
              onClick={(e: React.MouseEvent) => {
                e.preventDefault();
                handleSearchMenuClose();
                navigateWithCollector('/search/artists');
              }}
              role="menuitem"
              aria-label="Search for Magic artists"
              sx={{ textDecoration: 'none', color: 'inherit' }}
            >
              Artists
            </MenuItem>
          </Menu>
        </Box>

        {/* Spacer */}
        <Box sx={{ flexGrow: 1 }} />

        {/* Authentication Button */}
        <AuthButton />
      </Toolbar>
    </AppBar>
  );
};