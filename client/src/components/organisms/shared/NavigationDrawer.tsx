import React from 'react';
import {
  Drawer,
  Box,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Divider,
  Typography,
  IconButton
} from '../../atoms';
import { CloseIcon, SearchIcon, CollectionsBookmarkIcon } from '../../atoms/Icons';
import { useTheme } from '../../atoms';
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
  { label: 'Search Cards', path: '/search/cards', icon: <SearchIcon /> },
  { label: 'Search Artists', path: '/search/artists', icon: <SearchIcon /> },
  { label: 'Convention Signing', path: '/convention-signing', requiresCollector: true, hideOnMobile: true },
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
  const { navigateWithCollector, buildUrlWithCollector, collectorParam } = useCollectorNavigation();
  const { isMobile } = useResponsiveBreakpoints();

  const handleNavigation = (path: string) => {
    navigateWithCollector(path);
    onClose();
  };

  const filteredNavItems = NAV_ITEMS.filter(item => {
    if (item.requiresCollector && !collectorParam.hasCollector) {
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
