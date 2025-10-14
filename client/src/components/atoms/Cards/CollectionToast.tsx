import React, { useEffect } from 'react';
import Alert from '../Alert';
import IconButton from '../IconButton';
import Box from '../Box';
import {
  FINISH_DISPLAY_NAMES,
  SPECIAL_DISPLAY_NAMES
} from '../../../types/collection';
import type {
  CardFinish,
  CardSpecial
} from '../../../types/collection';
import { CloseIcon } from '../Icons';

export interface ToastMessage {
  id: string;
  type: 'success' | 'error';
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
    if (message.type === 'success' && !message.sticky) {
      const timer = setTimeout(() => {
        onClose(message.id);
      }, autoHideDuration);

      return () => clearTimeout(timer);
    }
  }, [message, onClose, autoHideDuration]);

  const finishText = FINISH_DISPLAY_NAMES[message.finish];
  const specialText = SPECIAL_DISPLAY_NAMES[message.special];

  const displayText = message.type === 'success'
    ? `${message.count > 0 ? '+' : ''}${message.count} ${finishText}${specialText ? ` ${specialText}` : ''} Success`
    : message.errorMessage || `${Math.abs(message.count)} ${finishText}${specialText ? ` ${specialText}` : ''} Failure`;

  return (
    <Alert
      severity={message.type}
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
            color: message.type === 'error' ? 'error.contrastText' : 'success.contrastText'
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