import { useState } from 'react';
import {
  Box,
  Container,
  Typography,
  Paper,
  TextField,
  IconButton,
  Tooltip,
  Snackbar,
  Alert,
  CircularProgress,
} from '@mui/material';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import CheckIcon from '@mui/icons-material/Check';
import { useUser } from '../../contexts/UserContext';

export default function UserProfilePage() {
  const { userProfile, isLoading } = useUser();
  const [copied, setCopied] = useState(false);

  const userId = userProfile?.collectorProfile?.id ?? userProfile?.id ?? '';
  const userNickname = userProfile?.nickname ?? userProfile?.name;

  const handleCopyUserId = async () => {
    try {
      await navigator.clipboard.writeText(userId);
      setCopied(true);
    } catch (err) {
      console.error('Failed to copy user ID:', err);
    }
  };

  const handleSnackbarClose = () => {
    setCopied(false);
  };

  if (isLoading) {
    return (
      <Container maxWidth="sm">
        <Box sx={{ py: 4, display: 'flex', justifyContent: 'center' }}>
          <CircularProgress />
        </Box>
      </Container>
    );
  }

  if (!userProfile) {
    return (
      <Container maxWidth="sm">
        <Box sx={{ py: 4 }}>
          <Typography variant="h5" color="text.secondary">
            Please log in to view your profile.
          </Typography>
        </Box>
      </Container>
    );
  }

  return (
    <Container maxWidth="sm">
      <Box sx={{ py: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          My Profile
        </Typography>

        {userNickname && (
          <Typography variant="h6" color="text.secondary" sx={{ mb: 3 }}>
            {userNickname}
          </Typography>
        )}

        <Paper sx={{ p: 3 }}>
          <Typography variant="subtitle2" color="text.secondary" gutterBottom>
            Your User ID
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Share this ID with others so they can add you to their collections.
          </Typography>

          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <TextField
              fullWidth
              value={userId}
              InputProps={{
                readOnly: true,
                sx: { fontFamily: 'monospace' },
              }}
              size="small"
            />
            <Tooltip title={copied ? 'Copied!' : 'Copy to clipboard'}>
              <IconButton onClick={handleCopyUserId} color={copied ? 'success' : 'default'}>
                {copied ? <CheckIcon /> : <ContentCopyIcon />}
              </IconButton>
            </Tooltip>
          </Box>
        </Paper>

        <Paper sx={{ p: 3, mt: 3 }}>
          <Typography variant="subtitle2" color="text.secondary" gutterBottom>
            Sharing Collections
          </Typography>
          <Typography variant="body2" color="text.secondary">
            To share a collection with someone, go to your Collections page,
            click the menu on a collection, and select &quot;Share Collection&quot;.
            You&apos;ll need their User ID to grant them access.
          </Typography>
        </Paper>
      </Box>

      <Snackbar
        open={copied}
        autoHideDuration={2000}
        onClose={handleSnackbarClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      >
        <Alert severity="success" onClose={handleSnackbarClose}>
          User ID copied to clipboard
        </Alert>
      </Snackbar>
    </Container>
  );
}
