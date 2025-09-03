import React from 'react';
import { Button, Box, Typography, CircularProgress } from '@mui/material';
import { useAuth0 } from '@auth0/auth0-react';

export const AuthButton: React.FC = () => {
  const { isAuthenticated, user, loginWithRedirect, logout, isLoading } = useAuth0();

  if (isLoading) {
    return (
      <Button disabled startIcon={<CircularProgress size={20} />}>
        Loading...
      </Button>
    );
  }

  if (isAuthenticated && user) {
    return (
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
        <Typography variant="body2" sx={{ color: 'text.primary' }}>
          Welcome, {user.name || user.email}
        </Typography>
        <Button 
          onClick={() => logout({ logoutParams: { returnTo: window.location.origin } })} 
          variant="outlined" 
          size="small"
        >
          Logout
        </Button>
      </Box>
    );
  }

  return (
    <Button onClick={() => loginWithRedirect()} variant="contained" size="small">
      Login
    </Button>
  );
};