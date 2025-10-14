import React, { useState } from 'react';
import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  Button,
  Menu,
  MenuItem
} from '../atoms';
import { useTheme } from '../atoms';
import { SearchInput } from '../molecules/shared/SearchInput';
import { AuthButton } from '../auth/AuthButton';
import { useCollectorNavigation } from '../../hooks/useCollectorNavigation';
import { SearchIcon, ArrowDropDownIcon } from '../atoms/Icons';
// import { LanguageSwitcher } from '../molecules/shared/LanguageSwitcher'; // Disabled until translations are available

export const Header: React.FC = () => {
  const [setCode, setSetCode] = useState('');
  const [searchAnchorEl, setSearchAnchorEl] = useState<null | HTMLElement>(null);
  const theme = useTheme();
  const { buildUrlWithCollector, navigateWithCollector } = useCollectorNavigation();

  const handleSetCodeSubmit = () => {
    if (setCode.trim()) {
      // Navigate to set page while preserving collector parameter
      navigateWithCollector(`/set/${setCode.trim().toLowerCase()}`);
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
          </Menu>
        </Box>

        {/* Spacer */}
        <Box sx={{ flexGrow: 1 }} />

        {/* Authentication Button */}
        <AuthButton />

        {/* Language Switcher - Far Right (Disabled until translations are available) */}
        {/* <Box sx={{ ml: 2 }}>
          <LanguageSwitcher compact={true} />
        </Box> */}
      </Toolbar>
    </AppBar>
  );
};