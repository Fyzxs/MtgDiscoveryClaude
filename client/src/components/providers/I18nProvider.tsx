import React, { Suspense } from 'react';
import { I18nextProvider } from 'react-i18next';
import { CircularProgress, Box } from '../../atoms';
import i18n from '../../i18n/config';

interface I18nProviderProps {
  children: React.ReactNode;
}

const LoadingFallback: React.FC = () => (
  <Box
    sx={{
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      height: '100vh',
      flexDirection: 'column',
      gap: 2
    }}
  >
    <CircularProgress />
    <span>Loading translations...</span>
  </Box>
);

export const I18nProvider: React.FC<I18nProviderProps> = ({ children }) => {
  return (
    <I18nextProvider i18n={i18n}>
      <Suspense fallback={<LoadingFallback />}>
        {children}
      </Suspense>
    </I18nextProvider>
  );
};