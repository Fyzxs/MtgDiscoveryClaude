import React, { useEffect } from 'react';
import { Alert, IconButton, Box, CloseIcon } from '../../atoms';
import {
  FINISH_DISPLAY_NAMES,
  SPECIAL_DISPLAY_NAMES
} from '../../../types/collection';
import type {
  CardFinish,
  CardSpecial
} from '../../../types/collection';

export interface ToastMessage {
  id: string;
  type: 'success' | 'error' | 'wishlist' | 'wishlist-remove';
  count: number;
  finish: CardFinish;
  special: CardSpecial;
  cardName?: string;
  errorMessage?: string;
  sticky?: boolean;
}

interface CollectionToastProps {
  message: ToastMessage;
  onClose: (id: string) => void;
  autoHideDuration?: number;
}

export const CollectionToast: React.FC<CollectionToastProps> = ({
  message,
  onClose,
  autoHideDuration = 10000
}) => {
  useEffect(() => {
    if (message.type !== 'error' && !message.sticky) {
      const timer = setTimeout(() => {
        onClose(message.id);
      }, autoHideDuration);

      return () => clearTimeout(timer);
    }
  }, [message, onClose, autoHideDuration]);

  const finishText = FINISH_DISPLAY_NAMES[message.finish];
  const specialText = SPECIAL_DISPLAY_NAMES[message.special];

  const getDisplayText = () => {
    switch (message.type) {
      case 'success':
        return `${message.count > 0 ? '+' : ''}${message.count} ${finishText}${specialText ? ` ${specialText}` : ''} Added to Collection`;
      case 'wishlist':
        return `♡ ${message.count > 0 ? '+' : ''}${message.count} ${finishText}${specialText ? ` ${specialText}` : ''} Added to Wishlist`;
      case 'wishlist-remove':
        return `♡ -${Math.abs(message.count)} ${finishText}${specialText ? ` ${specialText}` : ''} Removed from Wishlist`;
      case 'error':
        return message.errorMessage || `${Math.abs(message.count)} ${finishText}${specialText ? ` ${specialText}` : ''} Failed`;
      default:
        return '';
    }
  };

  const displayText = getDisplayText();

  // Map wishlist types to MUI severity (info for wishlist actions)
  const getSeverity = (): 'success' | 'error' | 'info' => {
    if (message.type === 'error') return 'error';
    if (message.type === 'wishlist' || message.type === 'wishlist-remove') return 'info';
    return 'success';
  };

  const severity = getSeverity();

  return (
    <Alert
      severity={severity}
      sx={{
        minWidth: 300,
        boxShadow: 3,
        alignItems: 'center',
        '& .MuiAlert-message': {
          flex: 1
        }
      }}
      action={
        <IconButton
          size="small"
          onClick={() => onClose(message.id)}
          sx={{
            color: severity === 'error' ? 'error.contrastText' : severity === 'info' ? 'info.contrastText' : 'success.contrastText'
          }}
        >
          <CloseIcon fontSize="small" />
        </IconButton>
      }
    >
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
        {displayText}
        {message.cardName && (
          <Box component="span" sx={{ fontStyle: 'italic', opacity: 0.9 }}>
            - {message.cardName}
          </Box>
        )}
      </Box>
    </Alert>
  );
};