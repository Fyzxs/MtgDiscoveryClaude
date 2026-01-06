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
import { SearchIcon, ArrowDropDownIcon, MenuIcon } from '../../atoms/Icons';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';
import { NavigationDrawer } from './NavigationDrawer';

export const Header: React.FC = () => {
  const [setCode, setSetCode] = useState('');
  const [searchAnchorEl, setSearchAnchorEl] = useState<null | HTMLElement>(null);
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

  // handleSearchMenuClick no longer needed - using href directly

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

            {/* Search Button */}
            <IconButton
              onClick={handleSearchMenuOpen}
              aria-label="Search options"
              sx={{ minWidth: 44, minHeight: 44 }}
            >
              <SearchIcon />
            </IconButton>

            {/* Search Menu - shared with desktop */}
            <Menu
              anchorEl={searchAnchorEl}
              open={Boolean(searchAnchorEl)}
              onClose={handleSearchMenuClose}
              role="menu"
              aria-label="Search menu"
            >
              <MenuItem
                component="a"
                href={buildUrlWithCollector('/search/cards')}
                onClick={(e: React.MouseEvent) => {
                  e.preventDefault();
                  handleSearchMenuClose();
                  navigateWithCollector('/search/cards');
                }}
                sx={{ minHeight: 48 }}
              >
                Search Cards
              </MenuItem>
              <MenuItem
                component="a"
                href={buildUrlWithCollector('/search/artists')}
                onClick={(e: React.MouseEvent) => {
                  e.preventDefault();
                  handleSearchMenuClose();
                  navigateWithCollector('/search/artists');
                }}
                sx={{ minHeight: 48 }}
              >
                Search Artists
              </MenuItem>
            </Menu>
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

          <Button
            color="primary"
            component="a"
            href={buildUrlWithCollector('/sets')}
            role="menuitem"
            aria-label="Browse all Magic sets"
            onClick={(e: React.MouseEvent) => {
              e.preventDefault();
              navigateWithCollector('/sets');
            }}
            sx={{
              textTransform: 'none',
              fontWeight: 500,
              textDecoration: 'none'
            }}
          >
            All Sets
          </Button>
          
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
            {collectorParam.hasCollector && (
              <MenuItem
                component="a"
                href={buildUrlWithCollector('/convention-signing')}
                onClick={(e: React.MouseEvent) => {
                  e.preventDefault();
                  handleSearchMenuClose();
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
        </Box>

        {/* Spacer */}
        <Box sx={{ flexGrow: 1 }} />

        {/* Authentication Button */}
        <AuthButton />
      </Toolbar>
    </AppBar>
  );
};