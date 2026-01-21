import React from 'react';
import { Button } from '../../atoms';
import { useNavigate, useLocation } from 'react-router-dom';
import { useUser } from '../../../contexts/UserContext';
import { CollectionsBookmarkIcon, VisibilityOffIcon } from '../../atoms';

export const MyCollectionButton: React.FC = () => {
  const { userProfile } = useUser();
  const navigate = useNavigate();
  const location = useLocation();

  const searchParams = new URLSearchParams(location.search);
  const hasCollectorParam = searchParams.has('ctor');

  const handleMyCollection = () => {
    if (userProfile?.id) {
      const searchParams = new URLSearchParams(location.search);
      searchParams.set('ctor', userProfile.id);
      const newUrl = `${location.pathname}?${searchParams.toString()}`;
      navigate(newUrl);
    }
  };

  const handleHideCollection = () => {
    const searchParams = new URLSearchParams(location.search);
    searchParams.delete('ctor');
    const newUrl = searchParams.toString()
      ? `${location.pathname}?${searchParams.toString()}`
      : location.pathname;
    navigate(newUrl);
  };

  if (!userProfile?.id) {
    return null;
  }

  // If ctor param exists, show "Hide Collection" button
  if (hasCollectorParam) {
    return (
      <Button
        onClick={handleHideCollection}
        variant="outlined"
        size="small"
        startIcon={<VisibilityOffIcon />}
        sx={{
          borderColor: 'primary.main',
          color: 'primary.main',
          '&:hover': {
            borderColor: 'primary.dark',
            bgcolor: 'rgba(144, 202, 249, 0.08)',
          }
        }}
      >
        Hide Collection
      </Button>
    );
  }

  // Otherwise show "My Collection" button
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