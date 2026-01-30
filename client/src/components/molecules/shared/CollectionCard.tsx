import { Card, CardContent, CardActionArea, Typography, Box, IconButton, Menu, MenuItem, Divider } from '@mui/material';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { useState } from 'react';
import type { MouseEvent } from 'react';
import { COLLECTION_PARAM_NAME } from '../../../hooks/useCollectionParam';

interface CollectionCardProps {
  name: string;
  isDefault: boolean;
  collectionId: string;
  isActive?: boolean;
  isOwner?: boolean;
  onClick?: () => void;
  onRename?: () => void;
  onDelete?: () => void;
  onShareCopied?: () => void;
}

export function CollectionCard({
  name,
  isDefault,
  collectionId,
  isActive = false,
  isOwner = true,
  onClick,
  onRename,
  onDelete,
  onShareCopied,
}: CollectionCardProps) {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const menuOpen = Boolean(anchorEl);

  const handleMenuClick = (event: MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleRename = () => {
    handleMenuClose();
    onRename?.();
  };

  const handleDelete = () => {
    handleMenuClose();
    onDelete?.();
  };

  const handleShare = async () => {
    handleMenuClose();
    const shareUrl = `${window.location.origin}/sets?${COLLECTION_PARAM_NAME}=${collectionId}`;
    try {
      await navigator.clipboard.writeText(shareUrl);
      onShareCopied?.();
    } catch (err) {
      // Fallback for older browsers
      const textArea = document.createElement('textarea');
      textArea.value = shareUrl;
      document.body.appendChild(textArea);
      textArea.select();
      document.execCommand('copy');
      document.body.removeChild(textArea);
      onShareCopied?.();
    }
  };

  return (
    <Card
      sx={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        border: isActive ? 2 : 1,
        borderColor: isActive ? 'primary.main' : 'divider',
        transition: 'border-color 0.2s',
        '&:hover': {
          borderColor: isActive ? 'primary.main' : 'primary.light',
        },
      }}
    >
      <CardActionArea onClick={onClick} sx={{ flexGrow: 1 }}>
        <CardContent>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Typography variant="h6" component="div" noWrap sx={{ minWidth: 0 }}>
              {name}
            </Typography>
            <IconButton
              size="small"
              onClick={handleMenuClick}
              sx={{ flexShrink: 0 }}
            >
              <MoreVertIcon fontSize="small" />
            </IconButton>
          </Box>
        </CardContent>
      </CardActionArea>

      <Menu
        anchorEl={anchorEl}
        open={menuOpen}
        onClose={handleMenuClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      >
        <MenuItem onClick={handleRename}>Rename</MenuItem>
        <MenuItem onClick={handleShare}>Copy Share Link</MenuItem>
        {isOwner && !isDefault && (
          <>
            <Divider />
            <MenuItem onClick={handleDelete} sx={{ color: 'error.main' }}>Delete</MenuItem>
          </>
        )}
      </Menu>
    </Card>
  );
}
