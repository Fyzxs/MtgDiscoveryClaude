import React from 'react';
import { Box } from '../../atoms';
import { Header } from '../organisms/Header';
import { Footer } from '../organisms/Footer';
import { SkipNavigation } from '../atoms/shared/SkipNavigation';
import { QuickEntryKeysFab } from '../molecules/Cards/QuickEntryKeysFab';
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
      <SkipNavigation />
      <Header />
      <Box 
        component="main" 
        id="main-content"
        role="main"
        tabIndex={-1}
        sx={{
          ...mainContent,
          '&:focus': {
            outline: 'none'
          }
        }}
      >
        {children}
      </Box>
      <Footer />
      <QuickEntryKeysFab />
    </Box>
  );
};