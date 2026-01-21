import { useState, useCallback, useImperativeHandle, forwardRef } from 'react';
import { Box, Button, Collapse } from '../../atoms';
import { CollectionToast } from '../atoms/Cards/CollectionToast';
import type { ToastMessage } from '../atoms/Cards/CollectionToast';
import type { Theme } from '@mui/material';

export interface NotificationToastStackRef {
  addToast: (toast: Omit<ToastMessage, 'id'>) => void;
}

export const NotificationToastStack = forwardRef<NotificationToastStackRef>((_, ref) => {
  const [toasts, setToasts] = useState<ToastMessage[]>([]);

  const addToast = useCallback((toast: Omit<ToastMessage, 'id'>) => {
    const id = `toast-${Date.now()}-${Math.random()}`;
    const newToast: ToastMessage = {
      ...toast,
      id,
      sticky: toast.type === 'error'
    };

    setToasts(prev => [...prev, newToast]);
  }, []);

  const removeToast = useCallback((id: string) => {
    setToasts(prev => prev.filter(toast => toast.id !== id));
  }, []);

  const clearAllErrors = useCallback(() => {
    setToasts(prev => prev.filter(toast => toast.type !== 'error'));
  }, []);

  useImperativeHandle(ref, () => ({
    addToast
  }), [addToast]);

  const errorCount = toasts.filter(t => t.type === 'error').length;

  return (
    <Box
      sx={{
        position: 'fixed',
        top: (theme: Theme) => theme.spacing(2),
        right: (theme: Theme) => theme.spacing(2),
        zIndex: 1400,
        maxWidth: 400,
        display: 'flex',
        flexDirection: 'column',
        gap: 1
      }}
    >
      {errorCount > 3 && (
        <Button
          variant="contained"
          color="error"
          size="small"
          onClick={clearAllErrors}
          sx={{ mb: 1, alignSelf: 'flex-end' }}
        >
          Clear All Errors ({errorCount})
        </Button>
      )}

      {toasts.map(toast => (
        <Collapse key={toast.id} in={true}>
          <Box sx={{ mb: 1 }}>
            <CollectionToast
              message={toast}
              onClose={removeToast}
            />
          </Box>
        </Collapse>
      ))}
    </Box>
  );
});

NotificationToastStack.displayName = 'NotificationToastStack';