import React, { useState } from 'react';
import { 
  AppBar, 
  Toolbar, 
  Typography, 
  Box, 
  Button,
  Menu,
  MenuItem
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import SearchIcon from '@mui/icons-material/Search';
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import { SearchInput } from '../atoms/shared/SearchInput';
import { AuthButton } from '../auth/AuthButton';

export const Header: React.FC = () => {
  const [setCode, setSetCode] = useState('');
  const [searchAnchorEl, setSearchAnchorEl] = useState<null | HTMLElement>(null);
  const theme = useTheme();

  const handleSetCodeSubmit = () => {
    if (setCode.trim()) {
      // Force complete URL change to clear all parameters
      window.location.href = `/set/${setCode.trim().toLowerCase()}`;
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
          onKeyDown={(e) => {
            if (e.key === 'Enter' || e.key === ' ') {
              e.preventDefault();
              window.location.href = '/';
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
          onClick={() => window.location.href = '/'}
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
            href="/sets"
            role="menuitem"
            aria-label="Browse all Magic sets"
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
              href="/search/cards"
              onClick={handleSearchMenuClose}
              role="menuitem"
              aria-label="Search for Magic cards"
              sx={{ textDecoration: 'none', color: 'inherit' }}
            >
              Cards
            </MenuItem>
            <MenuItem 
              component="a"
              href="/search/artists"
              onClick={handleSearchMenuClose}
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