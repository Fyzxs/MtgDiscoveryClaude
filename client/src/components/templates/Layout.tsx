import React from 'react';
import { Box } from '@mui/material';
import { Header } from '../organisms/Header';
import { Footer } from '../organisms/Footer';

interface LayoutProps {
  children: React.ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <Box 
      sx={{ 
        display: 'flex', 
        flexDirection: 'column',
        minHeight: '100vh',
        backgroundColor: 'background.default'
      }}
    >
      <Header />
      <Box 
        component="main" 
        sx={{ 
          flex: 1,
          display: 'flex',
          flexDirection: 'column'
        }}
      >
        {children}
      </Box>
      <Footer />
    </Box>
  );
};