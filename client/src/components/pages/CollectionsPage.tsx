import { useState } from 'react';
import {
  Container,
  Typography,
  Box,
  Button,
  Grid,
  CircularProgress,
  Alert,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { useMutation } from '@apollo/client/react';
import { CollectionCard } from '../molecules/shared/CollectionCard';
import { CreateCollectionDialog } from '../organisms/CreateCollectionDialog';
import { RenameCollectionDialog } from '../organisms/RenameCollectionDialog';
import { DeleteCollectionDialog } from '../organisms/DeleteCollectionDialog';
import { useCollectionManagement } from '../../contexts/CollectionManagementContext';
import { useToast } from '../../contexts/ToastContext';
import { DELETE_COLLECTION } from '../../graphql/mutations/deleteCollection';
import type { DeleteCollectionMutation } from '../../generated/graphql';

interface CollectionToEdit {
  collectionId: string;
  name: string;
}

export default function CollectionsPage() {
  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [renameTarget, setRenameTarget] = useState<CollectionToEdit | null>(null);
  const [deleteTarget, setDeleteTarget] = useState<CollectionToEdit | null>(null);
  const { collections, activeCollectionId, isLoading, error, setActiveCollection, refetchCollections } = useCollectionManagement();
  const { showSuccess } = useToast();

  const [deleteCollection] = useMutation<DeleteCollectionMutation>(DELETE_COLLECTION);

  const handleCollectionClick = (collectionId: string) => {
    setActiveCollection(collectionId);
  };

  const handleCreateSuccess = () => {
    refetchCollections();
  };

  const handleRenameSuccess = () => {
    refetchCollections();
  };

  const handleDeleteConfirm = async () => {
    if (deleteTarget === null) return;

    const result = await deleteCollection({
      variables: { collectionId: deleteTarget.collectionId },
    });

    const response = result.data?.deleteCollection;
    if (response?.__typename === 'FailureResponse') {
      throw new Error(response.status?.message ?? 'Failed to delete collection');
    }

    refetchCollections();
  };

  if (isLoading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4, display: 'flex', justifyContent: 'center' }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error">Failed to load collections: {error.message}</Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          My Collections
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => setCreateDialogOpen(true)}
        >
          New Collection
        </Button>
      </Box>

      {collections.length === 0 ? (
        <Box sx={{ textAlign: 'center', py: 8 }}>
          <Typography variant="h6" color="text.secondary" gutterBottom>
            No collections yet
          </Typography>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Create your first collection to start organizing your cards
          </Typography>
          <Button
            variant="outlined"
            startIcon={<AddIcon />}
            onClick={() => setCreateDialogOpen(true)}
          >
            Create Collection
          </Button>
        </Box>
      ) : (
        <Grid container spacing={3}>
          {collections.map((collection) => (
            <Grid item xs={12} sm={6} md={4} key={collection.collectionId}>
              <CollectionCard
                name={collection.name}
                isDefault={collection.isDefault}
                collectionId={collection.collectionId}
                isActive={collection.collectionId === activeCollectionId}
                onClick={() => handleCollectionClick(collection.collectionId)}
                onRename={() => setRenameTarget({
                  collectionId: collection.collectionId,
                  name: collection.name,
                })}
                onShareCopied={() => showSuccess('Link copied to clipboard')}
                onDelete={collection.isDefault ? undefined : () => setDeleteTarget({
                  collectionId: collection.collectionId,
                  name: collection.name,
                })}
              />
            </Grid>
          ))}
        </Grid>
      )}

      {/* Create Collection Dialog */}
      <CreateCollectionDialog
        open={createDialogOpen}
        onClose={() => setCreateDialogOpen(false)}
        onSuccess={handleCreateSuccess}
      />

      {/* Rename Collection Dialog */}
      <RenameCollectionDialog
        open={renameTarget !== null}
        collectionId={renameTarget?.collectionId ?? ''}
        currentName={renameTarget?.name ?? ''}
        onClose={() => setRenameTarget(null)}
        onSuccess={handleRenameSuccess}
      />

      {/* Delete Collection Dialog */}
      <DeleteCollectionDialog
        open={deleteTarget !== null}
        collectionName={deleteTarget?.name ?? ''}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleDeleteConfirm}
      />
    </Container>
  );
}
