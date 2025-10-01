import React from 'react';
import { Button } from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';
import { useUser } from '../../../contexts/UserContext';
import CollectionsBookmarkIcon from '@mui/icons-material/CollectionsBookmark';

export const MyCollectionButton: React.FC = () => {
  const { userProfile } = useUser();
  const navigate = useNavigate();
  const location = useLocation();

  const handleMyCollection = () => {
    if (userProfile?.id) {
      const searchParams = new URLSearchParams(location.search);
      searchParams.set('ctor', userProfile.id);
      const newUrl = `${location.pathname}?${searchParams.toString()}`;
      navigate(newUrl);
    }
  };

  if (!userProfile?.id) {
    return null;
  }

  return (
    <Button
      onClick={handleMyCollection}
      variant="contained"
      size="small"
      startIcon={<CollectionsBookmarkIcon />}
      sx={{
        bgcolor: 'primary.main',
        '&:hover': {
          bgcolor: 'primary.dark',
        }
      }}
    >
      My Collection
    </Button>
  );
};