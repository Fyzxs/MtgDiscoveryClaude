import React from 'react';
import { Box } from '@mui/material';
import { Header } from '../organisms/Header';
import { Footer } from '../organisms/Footer';
import { pageContainer, mainContent } from '../../styles/layoutStyles';

interface LayoutProps {
  children: React.ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <Box 
      sx={{ 
        ...pageContainer,
        backgroundColor: 'background.default'
      }}
    >
      <Header />
      <Box 
        component="main" 
        sx={mainContent}
      >
        {children}
      </Box>
      <Footer />
    </Box>
  );
};