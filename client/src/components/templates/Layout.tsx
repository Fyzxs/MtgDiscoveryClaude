import React from 'react';
import { Section } from '../molecules/layouts';
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
    <Section
      asSection={false}
      sx={{
        ...pageContainer,
        backgroundColor: 'background.default'
      }}
    >
      <SkipNavigation />
      <Header />
      <Section
        component="main"
        asSection={false}
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
      </Section>
      <Footer />
      <QuickEntryKeysFab />
    </Section>
  );
};