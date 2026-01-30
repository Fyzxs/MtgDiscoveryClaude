import { Card, CardContent, CardActionArea, Typography, Box, IconButton, Chip } from '@mui/material';
import LockIcon from '@mui/icons-material/Lock';
import PublicIcon from '@mui/icons-material/Public';
import ExitToAppIcon from '@mui/icons-material/ExitToApp';
import { CollectionBadge } from '../../atoms/shared/CollectionBadge';

interface SharedCollectionCardProps {
  collectionId: string;
  name: string;
  type: string;
  visibility: string;
  ownerId: string;
  userRole: string;
  isActive?: boolean;
  onClick?: () => void;
  onLeave?: () => void;
}

export function SharedCollectionCard({
  collectionId,
  name,
  type,
  visibility,
  ownerId,
  userRole,
  isActive = false,
  onClick,
  onLeave,
}: SharedCollectionCardProps) {
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

  const handleLeave = (event: React.MouseEvent) => {
    event.stopPropagation();
    onLeave?.();
  };

  return (
    <Card
      sx={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        border: isActive ? 2 : 1,
        borderColor: isActive ? 'primary.main' : 'divider',
        transition: 'border-color 0.2s',
        bgcolor: 'action.hover',
        '&:hover': {
          borderColor: isActive ? 'primary.main' : 'primary.light',
        },
      }}
    >
      <CardActionArea onClick={onClick} sx={{ flexGrow: 1 }}>
        <CardContent>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 1 }}>
            <Typography variant="h6" component="div" noWrap sx={{ flexGrow: 1, mr: 1 }}>
              {name}
            </Typography>
            <IconButton
              size="small"
              onClick={handleLeave}
              title="Leave collection"
              sx={{ ml: 'auto' }}
            >
              <ExitToAppIcon fontSize="small" />
            </IconButton>
          </Box>

          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
            <CollectionBadge type={type} isDefault={false} />
            <Chip
              label={userRole}
              size="small"
              color={getRoleColor(userRole)}
            />
            {visibility === 'private' ? (
              <LockIcon fontSize="small" sx={{ color: 'text.secondary' }} />
            ) : (
              <PublicIcon fontSize="small" sx={{ color: 'text.secondary' }} />
            )}
          </Box>

          <Typography variant="body2" color="text.secondary">
            Owned by: {ownerId}
          </Typography>
        </CardContent>
      </CardActionArea>
    </Card>
  );
}
