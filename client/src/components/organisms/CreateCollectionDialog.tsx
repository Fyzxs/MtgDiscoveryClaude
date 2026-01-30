import { useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  FormHelperText,
  Box,
  CircularProgress,
  Alert,
  Typography,
} from '@mui/material';
import { useMutation } from '@apollo/client/react';
import { CREATE_COLLECTION } from '../../graphql/mutations/createCollection';
import type { CreateCollectionMutation } from '../../generated/graphql';

const ALLOWED_NAME_PATTERN = /^[a-zA-Z0-9 -]+$/;

interface CreateCollectionDialogProps {
  open: boolean;
  onClose: () => void;
  onSuccess?: () => void;
}

export function CreateCollectionDialog({ open, onClose, onSuccess }: CreateCollectionDialogProps) {
  const [name, setName] = useState('');
  const [visibility, setVisibility] = useState('private');
  const [error, setError] = useState<string | null>(null);

  const [createCollection, { loading: isSubmitting }] = useMutation<CreateCollectionMutation>(CREATE_COLLECTION);

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

    setError(null);

    try {
      const result = await createCollection({
        variables: { name: name.trim(), type: 'custom', visibility },
      });

      const response = result.data?.createCollection;
      if (response?.__typename === 'FailureResponse') {
        setError(response.status?.message ?? 'Failed to create collection');
        return;
      }

      onSuccess?.();
      handleClose();
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Failed to create collection');
    }
  };

  const handleClose = () => {
    setName('');
    setVisibility('private');
    setError(null);
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Create New Collection</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
          {error && <Alert severity="error">{error}</Alert>}

          <TextField
            label="Collection Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            fullWidth
            required
            inputProps={{ maxLength: 100 }}
          />

          <Typography variant="caption" color="text.secondary" sx={{ display: 'block' }}>
            ⚠️ MtgDiscovery is a family-friendly website. We will not tolerate the use of foul, abusive, discriminatory, offensive, sexual, or hateful language.
          </Typography>

          <FormControl fullWidth>
            <InputLabel>Visibility</InputLabel>
            <Select
              value={visibility}
              label="Visibility"
              onChange={(e) => setVisibility(e.target.value)}
            >
              <MenuItem value="private">Private</MenuItem>
              <MenuItem value="public">Public</MenuItem>
            </Select>
            <FormHelperText>Private collections are only visible to you and authorized users</FormHelperText>
          </FormControl>
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
          Create
        </Button>
      </DialogActions>
    </Dialog>
  );
}
