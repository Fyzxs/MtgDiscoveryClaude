import React, { useEffect } from 'react';
import { Box, CircularProgress, Typography } from '@mui/material';

/**
 * Logout redirect page - Auth0 redirects here after logout.
 * This page should not normally be seen since we now redirect directly to origin.
 * It serves as a fallback landing page if the Auth0 logout config uses this route.
 */
export const LogoutRedirectPage: React.FC = () => {
  useEffect(() => {
    // Additional cleanup in case we land here
    localStorage.removeItem('mtg-user-data');
    localStorage.removeItem('mtg-discovery-active-collection');

    // Redirect to home after brief delay
    const timer = setTimeout(() => {
      window.location.href = '/';
    }, 100);

    return () => clearTimeout(timer);
  }, []);

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        minHeight: '60vh',
        gap: 2,
      }}
    >
      <CircularProgress />
      <Typography variant="body2" color="text.secondary">
        Signing out...
      </Typography>
    </Box>
  );
};

export default LogoutRedirectPage;
