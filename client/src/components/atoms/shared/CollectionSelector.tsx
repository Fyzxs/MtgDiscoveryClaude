import { useState } from 'react';
import {
  FormControl,
  Select,
  MenuItem,
  ListItemIcon,
  ListItemText,
  Divider,
  Box,
} from '@mui/material';
import type { SelectChangeEvent } from '@mui/material';
import SettingsIcon from '@mui/icons-material/Settings';
import LockIcon from '@mui/icons-material/Lock';
import PublicIcon from '@mui/icons-material/Public';
import { CollectionBadge } from './CollectionBadge';

interface Collection {
  collectionId: string;
  name: string;
  type: string;
  visibility: string;
  isDefault: boolean;
}

interface CollectionSelectorProps {
  collections: Collection[];
  activeCollectionId: string | null;
  onSelectCollection: (collectionId: string) => void;
  onCreateNew: () => void;
}

export function CollectionSelector({
  collections,
  activeCollectionId,
  onSelectCollection,
  onCreateNew,
}: CollectionSelectorProps) {
  const [open, setOpen] = useState(false);

  const handleChange = (event: SelectChangeEvent) => {
    const value = event.target.value;
    if (value === '__create_new__') {
      onCreateNew();
    } else {
      onSelectCollection(value);
    }
  };

  const activeCollection = collections.find(c => c.collectionId === activeCollectionId);

  return (
    <FormControl size="small" sx={{ minWidth: 200 }}>
      <Select
        value={activeCollectionId ?? ''}
        onChange={handleChange}
        open={open}
        onOpen={() => setOpen(true)}
        onClose={() => setOpen(false)}
        displayEmpty
        renderValue={() => (
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            {activeCollection ? (
              <>
                <CollectionBadge type={activeCollection.type} isDefault={activeCollection.isDefault} />
                <span>{activeCollection.name}</span>
              </>
            ) : (
              <span>Select Collection</span>
            )}
          </Box>
        )}
        sx={{
          bgcolor: 'background.paper',
          '& .MuiSelect-select': {
            display: 'flex',
            alignItems: 'center',
          },
        }}
      >
        {collections.map((collection) => (
          <MenuItem
            key={collection.collectionId}
            value={collection.collectionId}
            selected={collection.collectionId === activeCollectionId}
          >
            <ListItemIcon sx={{ minWidth: 'auto', mr: 1 }}>
              {collection.visibility === 'private' ? (
                <LockIcon fontSize="small" />
              ) : (
                <PublicIcon fontSize="small" />
              )}
            </ListItemIcon>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flexGrow: 1 }}>
              <CollectionBadge type={collection.type} isDefault={collection.isDefault} />
              <ListItemText primary={collection.name} />
            </Box>
          </MenuItem>
        ))}
        <Divider />
        <MenuItem value="__create_new__">
          <ListItemIcon sx={{ minWidth: 'auto', mr: 1 }}>
            <SettingsIcon fontSize="small" />
          </ListItemIcon>
          <ListItemText primary="My Collections" />
        </MenuItem>
      </Select>
    </FormControl>
  );
}
