import React, { useState } from 'react';
import {
  Drawer,
  Box,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Divider,
  Typography,
  IconButton,
  TextField,
  InputAdornment
} from '../../atoms';
import { CloseIcon, SearchIcon, CollectionsBookmarkIcon, NavigateNextIcon, FavoriteBorderIcon, HotelIcon } from '../../atoms';
import { useTheme } from '../../atoms';
import { useAuth0 } from '@auth0/auth0-react';
import { useCollectorNavigation } from '../../../hooks/useCollectorNavigation';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';
import { AuthButton } from '../../auth/AuthButton';

interface NavigationDrawerProps {
  open: boolean;
  onClose: () => void;
}

interface NavItem {
  label: string;
  path: string;
  icon?: React.ReactNode;
  requiresCollector?: boolean;
  hideOnMobile?: boolean;
}

const NAV_ITEMS: NavItem[] = [
  { label: 'Home', path: '/' },
  { label: 'All Sets', path: '/sets', icon: <CollectionsBookmarkIcon /> },
  { label: 'Wishlist', path: '/wishlist', icon: <FavoriteBorderIcon />, requiresCollector: true },
  { label: 'Convention Signing', path: '/convention-signing', icon: <HotelIcon />, requiresCollector: true },
  { label: 'Search Cards', path: '/search/cards', icon: <SearchIcon /> },
  { label: 'Search Artists', path: '/search/artists', icon: <SearchIcon /> },
];

/**
 * Mobile navigation drawer that slides in from the left.
 * Contains all navigation items and auth button.
 */
export const NavigationDrawer: React.FC<NavigationDrawerProps> = ({
  open,
  onClose
}) => {
  const theme = useTheme();
  const { isAuthenticated } = useAuth0();
  const { navigateWithCollector, buildUrlWithCollector, collectorParam } = useCollectorNavigation();
  const { isMobile } = useResponsiveBreakpoints();
  const [setCode, setSetCode] = useState('');

  const handleNavigation = (path: string) => {
    onClose();
    navigateWithCollector(path);
  };

  const handleSetCodeSubmit = () => {
    if (setCode.trim()) {
      const code = setCode.trim().toLowerCase();
      onClose();
      navigateWithCollector(`/set/${code}`);
      setSetCode('');
    }
  };

  const handleSetCodeKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      e.stopPropagation();
      handleSetCodeSubmit();
    }
  };

  const filteredNavItems = NAV_ITEMS.filter(item => {
    if (item.requiresCollector && !collectorParam.hasCollector && !isAuthenticated) {
      return false;
    }
    if (item.hideOnMobile && isMobile) {
      return false;
    }
    return true;
  });

  return (
    <Drawer
      anchor="left"
      open={open}
      onClose={onClose}
      PaperProps={{
        sx: {
          width: '85vw',
          maxWidth: 320,
          bgcolor: 'background.paper',
        }
      }}
    >
      {/* Header */}
      <Box sx={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        px: 2,
        py: 1.5,
        minHeight: theme.mtg.mobile.headerHeight,
        borderBottom: 1,
        borderColor: 'divider',
      }}>
        <Typography
          variant="h6"
          sx={{
            fontWeight: 'bold',
            background: theme.mtg.gradients.header,
            backgroundClip: 'text',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
          }}
        >
          MtgDiscovery
        </Typography>
        <IconButton
          onClick={onClose}
          sx={{ minWidth: 44, minHeight: 44 }}
        >
          <CloseIcon />
        </IconButton>
      </Box>

      {/* Jump to Set */}
      <Box sx={{ px: 2, py: 2, borderBottom: 1, borderColor: 'divider' }}>
        <Typography variant="caption" color="text.secondary" sx={{ mb: 1, display: 'block' }}>
          Jump to Set
        </Typography>
        <TextField
          value={setCode}
          onChange={(e) => setSetCode(e.target.value)}
          onKeyDown={handleSetCodeKeyDown}
          placeholder="Enter set code (e.g., lea)"
          size="small"
          fullWidth
          InputProps={{
            endAdornment: (
              <InputAdornment position="end">
                <IconButton
                  onClick={handleSetCodeSubmit}
                  disabled={!setCode.trim()}
                  edge="end"
                  size="small"
                >
                  <NavigateNextIcon />
                </IconButton>
              </InputAdornment>
            ),
          }}
          sx={{
            '& .MuiOutlinedInput-root': {
              bgcolor: 'background.default',
            }
          }}
        />
      </Box>

      {/* Navigation Items */}
      <List sx={{ flex: 1, py: 1 }}>
        {filteredNavItems.map((item) => (
          <ListItem key={item.path} disablePadding>
            <ListItemButton
              component="a"
              href={buildUrlWithCollector(item.path)}
              onClick={(e: React.MouseEvent) => {
                e.preventDefault();
                handleNavigation(item.path);
              }}
              sx={{
                minHeight: 56,
                px: 2,
              }}
            >
              {item.icon && (
                <Box sx={{ mr: 2, display: 'flex', color: 'text.secondary' }}>
                  {item.icon}
                </Box>
              )}
              <ListItemText
                primary={item.label}
                primaryTypographyProps={{
                  fontWeight: 500,
                  fontSize: '1rem',
                }}
              />
            </ListItemButton>
          </ListItem>
        ))}
      </List>

      <Divider />

      {/* Auth Section */}
      <Box sx={{
        p: 2,
        pb: 'calc(16px + env(safe-area-inset-bottom, 0px))',
      }}>
        <AuthButton fullWidth />
      </Box>
    </Drawer>
  );
};
