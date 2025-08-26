import React, { Suspense, lazy } from 'react';
import { Box, CircularProgress, Typography } from '@mui/material';
import type { Card } from '../../types/card';

// Lazy load the CardDetailsModal
const CardDetailsModal = lazy(() => 
  import('./CardDetailsModal').then(module => ({ 
    default: module.CardDetailsModal 
  }))
);

interface LazyCardDetailsModalProps {
  open: boolean;
  onClose: () => void;
  card?: Card;
  onPrevious?: () => void;
  onNext?: () => void;
  hasPrevious?: boolean;
  hasNext?: boolean;
}

/**
 * Lazy-loaded wrapper for CardDetailsModal to improve initial page load performance
 * Only loads the modal code when it's actually opened
 */
export const LazyCardDetailsModal: React.FC<LazyCardDetailsModalProps> = React.memo((props) => {
  // Don't render anything if modal is not open
  if (!props.open) {
    return null;
  }

  const LoadingFallback = () => (
    <Box
      sx={{
        position: 'fixed',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        bgcolor: 'rgba(0, 0, 0, 0.5)',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        zIndex: 1300,
        flexDirection: 'column',
        gap: 2
      }}
    >
      <CircularProgress />
      <Typography color="white" variant="body2">
        Loading card details...
      </Typography>
    </Box>
  );

  return (
    <Suspense fallback={<LoadingFallback />}>
      <CardDetailsModal {...props} />
    </Suspense>
  );
});

LazyCardDetailsModal.displayName = 'LazyCardDetailsModal';