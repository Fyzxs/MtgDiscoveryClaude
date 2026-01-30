import React, { useState } from 'react';
import {
  Button,
  Menu,
  MenuItem,
  ListItemIcon,
  ListItemText,
  Divider,
  CircularProgress,
} from '@mui/material';
import { alpha, useTheme } from '@mui/material/styles';
import { useNavigate, useLocation } from 'react-router-dom';
import { useUser } from '../../../contexts/UserContext';
import { useCollectionManagement } from '../../../contexts/CollectionManagementContext';
import { useCollectionParam } from '../../../hooks/useCollectionParam';
import { CollectionsBookmarkIcon, VisibilityOffIcon } from '../../atoms';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import FolderIcon from '@mui/icons-material/Folder';
import CheckIcon from '@mui/icons-material/Check';

export const MyCollectionButton: React.FC = () => {
  const { userProfile } = useUser();
  const { collections, activeCollection, setActiveCollection, isLoading } = useCollectionManagement();
  const navigate = useNavigate();
  const location = useLocation();
  const theme = useTheme();
  const { hasCollection, setCollectionId, navigateWithCollection, clearCollection } = useCollectionParam();

  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const menuOpen = Boolean(anchorEl);

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleJustBrowse = () => {
    clearCollection();
    handleMenuClose();
  };

  const handleSelectCollection = (collectionId: string) => {
    setActiveCollection(collectionId);

    // If on homepage, redirect to /sets with collection param
    if (location.pathname === '/') {
      navigateWithCollection('/sets', collectionId);
    } else {
      setCollectionId(collectionId);
    }
    handleMenuClose();
  };

  const handleMyCollections = () => {
    navigate('/account/collections');
    handleMenuClose();
  };

  if (!userProfile?.id) {
    return null;
  }

  // Determine button text based on current state
  const getButtonText = () => {
    if (hasCollection && activeCollection) {
      return activeCollection.name;
    }
    return 'Collections';
  };

  return (
    <>
      <Button
        onClick={handleMenuOpen}
        variant={hasCollection ? 'contained' : 'outlined'}
        size="small"
        startIcon={isLoading ? <CircularProgress size={16} /> : <CollectionsBookmarkIcon />}
        endIcon={<KeyboardArrowDownIcon />}
        aria-controls={menuOpen ? 'collections-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={menuOpen}
        disabled={isLoading}
        sx={{
          minWidth: 140,
          ...(hasCollection
            ? {
                bgcolor: 'primary.main',
                '&:hover': {
                  bgcolor: 'primary.dark',
                },
              }
            : {
                borderColor: 'primary.main',
                color: 'primary.main',
                '&:hover': {
                  borderColor: 'primary.dark',
                  bgcolor: 'rgba(144, 202, 249, 0.08)',
                },
              }),
        }}
      >
        {getButtonText()}
      </Button>

      <Menu
        id="collections-menu"
        anchorEl={anchorEl}
        open={menuOpen}
        onClose={handleMenuClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        slotProps={{
          paper: {
            elevation: 8,
            sx: {
              minWidth: 220,
              mt: 1,
              borderRadius: 2,
              border: '1px solid',
              borderColor: 'divider',
            },
          },
        }}
      >
        {/* Just Browse option */}
        <MenuItem
          onClick={handleJustBrowse}
          sx={{
            py: 1.5,
            px: 2,
            minHeight: 48,
            '&:hover': {
              bgcolor: alpha(theme.palette.primary.main, 0.08),
            },
          }}
        >
          <ListItemIcon sx={{ minWidth: 40 }}>
            <VisibilityOffIcon />
          </ListItemIcon>
          <ListItemText primary="Just Browse" />
          {hasCollection === false && (
            <CheckIcon sx={{ ml: 1, color: 'primary.main' }} />
          )}
        </MenuItem>

        <Divider sx={{ my: 1 }} />

        {/* My Collections header - navigates to /collections */}
        <MenuItem
          onClick={handleMyCollections}
          sx={{
            py: 1.5,
            px: 2,
            minHeight: 48,
            '&:hover': {
              bgcolor: alpha(theme.palette.primary.main, 0.08),
            },
          }}
        >
          <ListItemIcon sx={{ minWidth: 40 }}>
            <CollectionsBookmarkIcon />
          </ListItemIcon>
          <ListItemText primary="My Collections" />
        </MenuItem>

        {/* User's collections - indented sub-items */}
        {collections.map((collection) => (
          <MenuItem
            key={collection.collectionId}
            onClick={() => handleSelectCollection(collection.collectionId)}
            sx={{
              py: 1,
              pl: 6,
              pr: 2,
              minHeight: 40,
              '&:hover': {
                bgcolor: alpha(theme.palette.primary.main, 0.08),
              },
            }}
          >
            <ListItemIcon sx={{ minWidth: 32 }}>
              <FolderIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText
              primary={collection.name}
              primaryTypographyProps={{ variant: 'body2' }}
              secondary={collection.isDefault ? 'Default' : undefined}
              secondaryTypographyProps={{ variant: 'caption' }}
            />
            {hasCollection && activeCollection?.collectionId === collection.collectionId && (
              <CheckIcon sx={{ ml: 1, color: 'primary.main', fontSize: '1rem' }} />
            )}
          </MenuItem>
        ))}
      </Menu>
    </>
  );
};
