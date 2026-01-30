import { useState, useEffect } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Box,
  CircularProgress,
  Alert,
  Typography,
} from '@mui/material';
import { useMutation } from '@apollo/client/react';
import { RENAME_COLLECTION } from '../../graphql/mutations/renameCollection';
import type { RenameCollectionMutation } from '../../generated/graphql';

const ALLOWED_NAME_PATTERN = /^[a-zA-Z0-9 -]+$/;

interface RenameCollectionDialogProps {
  open: boolean;
  collectionId: string;
  currentName: string;
  onClose: () => void;
  onSuccess?: () => void;
}

export function RenameCollectionDialog({
  open,
  collectionId,
  currentName,
  onClose,
  onSuccess,
}: RenameCollectionDialogProps) {
  const [name, setName] = useState(currentName);
  const [error, setError] = useState<string | null>(null);

  const [renameCollection, { loading: isSubmitting }] = useMutation<RenameCollectionMutation>(RENAME_COLLECTION);

  // Reset name when dialog opens with different collection
  useEffect(() => {
    if (open) {
      setName(currentName);
      setError(null);
    }
  }, [open, currentName]);

  const handleSubmit = async () => {
    if (!name.trim()) {
      setError('Name is required');
      return;
    }
    if (name.length > 100) {
      setError('Name must be 100 characters or fewer');
      return;
    }
    if (name.toLowerCase() === 'default') {
      setError('The name "default" is reserved');
      return;
    }
    if (ALLOWED_NAME_PATTERN.test(name) === false) {
      setError('Name can only contain letters, numbers, spaces, and dashes');
      return;
    }
    if (name.trim() === currentName) {
      handleClose();
      return;
    }

    setError(null);

    try {
      const result = await renameCollection({
        variables: { collectionId, name: name.trim() },
      });

      const response = result.data?.renameCollection;
      if (response?.__typename === 'FailureResponse') {
        setError(response.status?.message ?? 'Failed to rename collection');
        return;
      }

      onSuccess?.();
      handleClose();
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Failed to rename collection');
    }
  };

  const handleClose = () => {
    setError(null);
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Rename Collection</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
          {error && <Alert severity="error">{error}</Alert>}

          <TextField
            label="Collection Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            fullWidth
            required
            autoFocus
            inputProps={{ maxLength: 100 }}
            onKeyDown={(e) => {
              if (e.key === 'Enter') {
                e.preventDefault();
                handleSubmit();
              }
            }}
          />

          <Typography variant="caption" color="text.secondary" sx={{ display: 'block' }}>
            ⚠️ MtgDiscovery is a family-friendly website. We will not tolerate the use of foul, abusive, discriminatory, offensive, sexual, or hateful language.
          </Typography>
        </Box>
      </DialogContent>
      <DialogActions sx={{ px: 3, pb: 2 }}>
        <Button onClick={handleClose} disabled={isSubmitting}>
          Cancel
        </Button>
        <Button
          onClick={handleSubmit}
          variant="contained"
          disabled={isSubmitting || !name.trim()}
          startIcon={isSubmitting ? <CircularProgress size={16} /> : null}
        >
          Rename
        </Button>
      </DialogActions>
    </Dialog>
  );
}
