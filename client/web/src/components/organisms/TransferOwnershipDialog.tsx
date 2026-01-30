import { useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Alert,
  Box,
  Typography,
} from '@mui/material';

interface TransferOwnershipDialogProps {
  open: boolean;
  collectionName: string;
  onClose: () => void;
  onTransfer: (targetUserId: string) => Promise<void>;
}

export function TransferOwnershipDialog({
  open,
  collectionName,
  onClose,
  onTransfer,
}: TransferOwnershipDialogProps) {
  const [targetUserId, setTargetUserId] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async () => {
    if (targetUserId.trim() === '') {
      setError('User ID is required');
      return;
    }

    setError(null);
    setIsSubmitting(true);

    try {
      await onTransfer(targetUserId.trim());
      handleClose();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to transfer ownership');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleClose = () => {
    setTargetUserId('');
    setError(null);
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Transfer Ownership</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
          {error && <Alert severity="error">{error}</Alert>}

          <Alert severity="warning">
            You are about to transfer ownership of "{collectionName}". You will become an admin
            of this collection but will no longer be able to delete it or transfer ownership again.
          </Alert>

          <Typography variant="body2" color="text.secondary">
            Enter the User ID of the person you want to transfer ownership to.
            They must already have access to this collection.
          </Typography>

          <TextField
            autoFocus
            label="New Owner's User ID"
            fullWidth
            value={targetUserId}
            onChange={(e) => setTargetUserId(e.target.value)}
            placeholder="Enter user ID"
            disabled={isSubmitting}
          />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isSubmitting}>
          Cancel
        </Button>
        <Button
          onClick={handleSubmit}
          variant="contained"
          color="warning"
          disabled={isSubmitting}
        >
          {isSubmitting ? 'Transferring...' : 'Transfer Ownership'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
