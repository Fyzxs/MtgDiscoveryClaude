import React, { useState, useCallback } from 'react';
import {
  Box,
  Menu,
  MenuItem,
  IconButton,
  Avatar,
  Typography,
  Divider,
  ListItemIcon,
  ListItemText,
  Button,
  CircularProgress,
} from '@mui/material';
import { alpha, useTheme } from '@mui/material/styles';
import PersonIcon from '@mui/icons-material/Person';
import FavoriteIcon from '@mui/icons-material/Favorite';
import LogoutIcon from '@mui/icons-material/Logout';
import LoginIcon from '@mui/icons-material/Login';
import { useAuth0 } from '@auth0/auth0-react';
import { useApolloClient } from '@apollo/client/react';
import { useNavigate } from 'react-router-dom';
import { useAuthState } from '../../../contexts/AuthStateContext';
import { useUser } from '../../../contexts/UserContext';
import { logger } from '../../../utils/logger';

interface AccountMenuProps {
  variant?: 'desktop' | 'mobile';
}

const getUserInitials = (name?: string, email?: string): string => {
  const displayName = name || email;
  if (!displayName) return '?';

  const parts = displayName.split(' ');
  if (parts.length >= 2) {
    return `${parts[0][0]}${parts[1][0]}`.toUpperCase();
  }
  return displayName.slice(0, 2).toUpperCase();
};

export const AccountMenu: React.FC<AccountMenuProps> = ({ variant = 'desktop' }) => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const menuOpen = Boolean(anchorEl);

  const theme = useTheme();
  const navigate = useNavigate();
  const apolloClient = useApolloClient();

  const { isAuthenticated, user, isLoading } = useAuth0();
  const { login, logout } = useAuthState();
  const { collectorProfile } = useUser();

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = useCallback(async () => {
    try {
      await apolloClient.clearStore();
      logger.debug('AccountMenu - Apollo cache cleared');
    } catch (error) {
      logger.error('AccountMenu - Error clearing Apollo cache:', error);
    }
    handleMenuClose();
    logout();
  }, [apolloClient, logout]);

  const handleNavigate = (route: string) => {
    handleMenuClose();
    navigate(route);
  };

  // Loading state
  if (isLoading) {
    return (
      <IconButton
        disabled
        sx={{
          p: 0.5,
          minWidth: 44,
          minHeight: 44,
        }}
      >
        <CircularProgress size={24} />
      </IconButton>
    );
  }

  // Unauthenticated state
  if (!isAuthenticated || !user) {
    return (
      <Button
        onClick={login}
        variant="contained"
        size="small"
        startIcon={<LoginIcon />}
        sx={{
          minHeight: 44,
          textTransform: 'none',
          fontWeight: 500,
        }}
      >
        Login
      </Button>
    );
  }

  // Authenticated state
  const userInitials = getUserInitials(user.name, user.email);
  const displayName = user.name || user.nickname || user.email || 'User';
  const displayEmail = user.email || '';

  return (
    <>
      <IconButton
        onClick={handleMenuOpen}
        id="account-button"
        aria-label="Account menu"
        aria-controls={menuOpen ? 'account-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={menuOpen}
        sx={{
          p: 0.5,
          '&:hover': {
            bgcolor: alpha(theme.palette.primary.main, 0.1),
          },
          '&:focus': {
            outline: '2px solid',
            outlineColor: 'primary.main',
            outlineOffset: '2px',
          },
        }}
      >
        <Avatar
          src={user.picture}
          alt={displayName}
          sx={{
            width: variant === 'desktop' ? 40 : 36,
            height: variant === 'desktop' ? 40 : 36,
            bgcolor: 'primary.main',
            fontSize: '1rem',
            fontWeight: 600,
          }}
        >
          {userInitials}
        </Avatar>
      </IconButton>

      <Menu
        id="account-menu"
        anchorEl={anchorEl}
        open={menuOpen}
        onClose={handleMenuClose}
        role="menu"
        aria-labelledby="account-button"
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'right',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'right',
        }}
        slotProps={{
          paper: {
            elevation: 8,
            sx: {
              minWidth: 240,
              mt: 1.5,
              borderRadius: 2,
              border: '1px solid',
              borderColor: 'divider',
              overflow: 'visible',
              '&::before': {
                content: '""',
                display: 'block',
                position: 'absolute',
                top: 0,
                right: 14,
                width: 10,
                height: 10,
                bgcolor: 'background.paper',
                transform: 'translateY(-50%) rotate(45deg)',
                zIndex: 0,
                borderLeft: '1px solid',
                borderTop: '1px solid',
                borderColor: 'divider',
              },
            },
          },
        }}
      >
        {/* User Info Header */}
        <Box
          sx={{
            px: 2,
            py: 1.5,
            display: 'flex',
            alignItems: 'center',
            gap: 1.5,
            bgcolor: alpha(theme.palette.primary.main, 0.05),
            borderBottom: '1px solid',
            borderColor: 'divider',
          }}
        >
          <Avatar
            src={user.picture}
            alt={displayName}
            sx={{
              width: 48,
              height: 48,
              bgcolor: 'primary.main',
            }}
          >
            {userInitials}
          </Avatar>
          <Box sx={{ overflow: 'hidden' }}>
            <Typography
              variant="subtitle2"
              sx={{
                fontWeight: 600,
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
              }}
            >
              {displayName}
            </Typography>
            <Typography
              variant="caption"
              color="text.secondary"
              sx={{
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
                display: 'block',
              }}
            >
              {displayEmail}
            </Typography>
          </Box>
        </Box>

        {/* Navigation Items */}
        <MenuItem
          onClick={() => handleNavigate('/account/profile')}
          sx={{
            py: 1.5,
            px: 2,
            minHeight: 48,
            mt: 1,
            mx: 1,
            borderRadius: 1,
            '&:hover': {
              bgcolor: alpha(theme.palette.primary.main, 0.08),
            },
          }}
        >
          <ListItemIcon sx={{ minWidth: 40 }}>
            <PersonIcon />
          </ListItemIcon>
          <ListItemText primary="My Profile" />
        </MenuItem>

        {collectorProfile && (
          <MenuItem
            onClick={() => handleNavigate('/wishlist')}
            sx={{
              py: 1.5,
              px: 2,
              minHeight: 48,
              mx: 1,
              borderRadius: 1,
              '&:hover': {
                bgcolor: alpha(theme.palette.primary.main, 0.08),
              },
            }}
          >
            <ListItemIcon sx={{ minWidth: 40 }}>
              <FavoriteIcon />
            </ListItemIcon>
            <ListItemText primary="Wishlist" />
          </MenuItem>
        )}

        <Divider sx={{ my: 1 }} />

        {/* Sign Out */}
        <MenuItem
          onClick={handleLogout}
          sx={{
            py: 1.5,
            px: 2,
            minHeight: 48,
            mx: 1,
            mb: 1,
            borderRadius: 1,
            color: 'error.main',
            '&:hover': {
              bgcolor: alpha(theme.palette.error.main, 0.08),
            },
          }}
        >
          <ListItemIcon sx={{ minWidth: 40, color: 'inherit' }}>
            <LogoutIcon />
          </ListItemIcon>
          <ListItemText primary="Sign Out" />
        </MenuItem>
      </Menu>
    </>
  );
};
