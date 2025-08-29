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
import { useNavigate } from 'react-router-dom';
import SearchIcon from '@mui/icons-material/Search';
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import { SearchInput } from '../atoms/shared/SearchInput';

export const Header: React.FC = () => {
  const [setCode, setSetCode] = useState('');
  const [searchAnchorEl, setSearchAnchorEl] = useState<null | HTMLElement>(null);
  const theme = useTheme();
  const navigate = useNavigate();

  const handleSetCodeSubmit = () => {
    if (setCode.trim()) {
      // Navigate to set page
      navigate(`/set/${setCode.trim().toLowerCase()}`);
      setSetCode('');
    }
  };

  const handleSearchMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setSearchAnchorEl(event.currentTarget);
  };

  const handleSearchMenuClose = () => {
    setSearchAnchorEl(null);
  };

  const handleSearchMenuClick = (path: string) => {
    navigate(path);
    handleSearchMenuClose();
  };

  return (
    <AppBar 
      position="sticky" 
      sx={{ 
        backgroundColor: 'background.paper',
        backgroundImage: 'none',
        borderBottom: '1px solid',
        borderColor: 'divider'
      }}
    >
      <Toolbar sx={{ gap: 3 }}>
        {/* Site Logo/Name */}
        <Typography 
          variant="h5" 
          component="div"
          sx={{ 
            fontWeight: 'bold',
            background: theme.mtg.gradients.header,
            backgroundClip: 'text',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
            cursor: 'pointer',
            '&:hover': {
              background: theme.mtg.gradients.hover,
              backgroundClip: 'text',
              WebkitBackgroundClip: 'text',
              WebkitTextFillColor: 'transparent',
            }
          }}
          onClick={() => window.location.href = '/'}
        >
          MtgDiscovery
        </Typography>

        {/* Navigation Links */}
        <Box sx={{ display: 'flex', gap: 2, alignItems: 'center' }}>
          <Button 
            color="primary" 
            onClick={() => navigate('/sets')}
            sx={{ 
              textTransform: 'none',
              fontWeight: 500
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
            anchorOrigin={{
              vertical: 'bottom',
              horizontal: 'left',
            }}
            transformOrigin={{
              vertical: 'top',
              horizontal: 'left',
            }}
          >
            <MenuItem onClick={() => handleSearchMenuClick('/search/cards')}>
              Cards
            </MenuItem>
            <MenuItem disabled sx={{ color: 'text.disabled' }}>
              Artists (Coming Soon)
            </MenuItem>
          </Menu>
        </Box>

        {/* Spacer */}
        <Box sx={{ flexGrow: 1 }} />

        {/* Set Code Search */}
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
      </Toolbar>
    </AppBar>
  );
};