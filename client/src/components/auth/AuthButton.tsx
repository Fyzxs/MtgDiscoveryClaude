import React from 'react';
import { Button, Box, Typography, CircularProgress } from '@mui/material';
import { useAuth0 } from '@auth0/auth0-react';
import { MyCollectionButton } from '../molecules/ui/MyCollectionButton';

export const AuthButton: React.FC = () => {
  const { isAuthenticated, user, loginWithRedirect, logout, isLoading, getAccessTokenSilently, getIdTokenClaims } = useAuth0();

  // Debug function to log JWT tokens
  const logTokenInfo = async () => {
    if (isAuthenticated) {
      try {
        console.log('=== JWT TOKEN INFO ===');
        console.log('User object:', user);
        
        // Get ID token claims
        const idTokenClaims = await getIdTokenClaims();
        console.log('ID Token Claims:', idTokenClaims);
        console.log('Raw ID Token:', idTokenClaims?.__raw);
        
        // Get access token
        const accessToken = await getAccessTokenSilently();
        console.log('Access Token:', accessToken);
        
        // Decode JWT manually (base64)
        if (idTokenClaims?.__raw) {
          const parts = idTokenClaims.__raw.split('.');
          const header = JSON.parse(atob(parts[0]));
          const payload = JSON.parse(atob(parts[1]));
          console.log('JWT Header:', header);
          console.log('JWT Payload:', payload);
        }
      } catch (error) {
        console.error('Error getting tokens:', error);
      }
    }
  };

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
        <MyCollectionButton />
        <Typography
          variant="body2"
          sx={{
            color: 'text.primary',
            cursor: 'pointer',
            '&:hover': { textDecoration: 'underline' }
          }}
          onClick={logTokenInfo}
          title="Click to log JWT info to console"
        >
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