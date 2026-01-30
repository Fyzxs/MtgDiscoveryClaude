import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Typography,
  Chip,
  Box,
  CircularProgress,
  Alert,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import PersonAddIcon from '@mui/icons-material/PersonAdd';

interface AuthorizedUser {
  userId: string;
  role: string;
  grantedAt: string;
  grantedBy: string;
}

interface AccessListDialogProps {
  open: boolean;
  collectionName: string;
  authorizedUsers: AuthorizedUser[];
  isLoading: boolean;
  error: string | null;
  currentUserId: string;
  isOwner: boolean;
  onClose: () => void;
  onRevoke: (targetUserId: string) => Promise<void>;
  onGrantClick: () => void;
}

export function AccessListDialog({
  open,
  collectionName,
  authorizedUsers,
  isLoading,
  error,
  currentUserId,
  isOwner,
  onClose,
  onRevoke,
  onGrantClick,
}: AccessListDialogProps) {
  const formatDate = (dateString: string) => {
    try {
      return new Date(dateString).toLocaleDateString();
    } catch {
      return dateString;
    }
  };

  const getRoleColor = (role: string): 'primary' | 'secondary' | 'default' => {
    switch (role) {
      case 'admin':
        return 'primary';
      case 'editor':
        return 'secondary';
      default:
        return 'default';
    }
  };

  const canRevoke = (user: AuthorizedUser): boolean => {
    if (isOwner) {
      return true;
    }
    return user.userId === currentUserId;
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Typography variant="h6">Access List - "{collectionName}"</Typography>
          {isOwner && (
            <Button
              startIcon={<PersonAddIcon />}
              onClick={onGrantClick}
              size="small"
              variant="outlined"
            >
              Grant Access
            </Button>
          )}
        </Box>
      </DialogTitle>
      <DialogContent>
        {isLoading && (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
            <CircularProgress />
          </Box>
        )}

        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

        {authorizedUsers.length === 0 && isLoading === false && (
          <Typography color="text.secondary" sx={{ py: 2, textAlign: 'center' }}>
            No users have been granted access to this collection.
          </Typography>
        )}

        {authorizedUsers.length > 0 && (
          <List>
            {authorizedUsers.map((user) => (
              <ListItem key={user.userId} divider>
                <ListItemText
                  primary={
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                      <Typography variant="body1">{user.userId}</Typography>
                      <Chip
                        label={user.role}
                        size="small"
                        color={getRoleColor(user.role)}
                      />
                    </Box>
                  }
                  secondary={`Granted on ${formatDate(user.grantedAt)} by ${user.grantedBy}`}
                />
                {canRevoke(user) && (
                  <ListItemSecondaryAction>
                    <IconButton
                      edge="end"
                      aria-label="revoke access"
                      onClick={() => onRevoke(user.userId)}
                      color="error"
                    >
                      <DeleteIcon />
                    </IconButton>
                  </ListItemSecondaryAction>
                )}
              </ListItem>
            ))}
          </List>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Close</Button>
      </DialogActions>
    </Dialog>
  );
}
