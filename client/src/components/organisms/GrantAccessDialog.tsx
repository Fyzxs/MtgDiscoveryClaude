import { useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
  Alert,
  Box,
} from '@mui/material';
import type { SelectChangeEvent } from '@mui/material';

interface GrantAccessDialogProps {
  open: boolean;
  collectionName: string;
  onClose: () => void;
  onGrant: (targetUserId: string, role: string) => Promise<void>;
}

export function GrantAccessDialog({
  open,
  collectionName,
  onClose,
  onGrant,
}: GrantAccessDialogProps) {
  const [targetUserId, setTargetUserId] = useState('');
  const [role, setRole] = useState('viewer');
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
      await onGrant(targetUserId.trim(), role);
      handleClose();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to grant access');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleClose = () => {
    setTargetUserId('');
    setRole('viewer');
    setError(null);
    onClose();
  };

  const handleRoleChange = (event: SelectChangeEvent) => {
    setRole(event.target.value);
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Grant Access to "{collectionName}"</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
          {error && <Alert severity="error">{error}</Alert>}

          <TextField
            autoFocus
            label="User ID"
            fullWidth
            value={targetUserId}
            onChange={(e) => setTargetUserId(e.target.value)}
            placeholder="Enter the user's ID"
            disabled={isSubmitting}
          />

          <FormControl fullWidth>
            <InputLabel id="role-select-label">Role</InputLabel>
            <Select
              labelId="role-select-label"
              value={role}
              label="Role"
              onChange={handleRoleChange}
              disabled={isSubmitting}
            >
              <MenuItem value="viewer">Viewer - Can view cards in this collection</MenuItem>
              <MenuItem value="editor">Editor - Can add and remove cards</MenuItem>
            </Select>
          </FormControl>
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={isSubmitting}>
          Cancel
        </Button>
        <Button onClick={handleSubmit} variant="contained" disabled={isSubmitting}>
          {isSubmitting ? 'Granting...' : 'Grant Access'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
