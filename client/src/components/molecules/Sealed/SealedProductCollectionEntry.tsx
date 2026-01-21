import React, { useState, useEffect } from 'react';
import { Box, Typography, IconButton, Button, Drawer, Stack, useTheme, useMediaQuery } from '@mui/material';
import { Add, Remove } from '@mui/icons-material';

interface SealedProductCollectionEntryProps {
  productName: string;
  currentQuantity: number;
  onQuantityChange: (delta: number) => void;
  open: boolean;
  onClose: () => void;
  isUpdating?: boolean;
}

export const SealedProductCollectionEntry: React.FC<SealedProductCollectionEntryProps> = ({
  productName,
  currentQuantity,
  onQuantityChange,
  open,
  onClose,
  isUpdating = false
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));
  const [localQuantity, setLocalQuantity] = useState(currentQuantity);

  useEffect(() => {
    if (open) {
      setLocalQuantity(currentQuantity);
    }
  }, [open, currentQuantity]);

  const handleIncrement = () => {
    setLocalQuantity(prev => prev + 1);
  };

  const handleDecrement = () => {
    setLocalQuantity(prev => Math.max(0, prev - 1));
  };

  const handleSave = () => {
    const delta = localQuantity - currentQuantity;
    if (delta !== 0) {
      onQuantityChange(delta);
    }
    onClose();
  };

  const handleQuickAdd = (amount: number) => {
    onQuantityChange(amount);
    onClose();
  };

  const handleRemove = () => {
    onQuantityChange(-currentQuantity);
    onClose();
  };

  const content = (
    <Box sx={{ p: { xs: 2, md: 3 } }}>
      <Typography
        variant="h6"
        sx={{
          mb: 3,
          fontWeight: 600,
          textAlign: 'center'
        }}
      >
        {productName}
      </Typography>

      <Stack
        direction="row"
        spacing={2}
        alignItems="center"
        justifyContent="center"
        sx={{ mb: 3 }}
      >
        <IconButton
          onClick={handleDecrement}
          disabled={localQuantity === 0 || isUpdating}
          size="large"
          sx={{
            bgcolor: 'action.hover',
            '&:hover': {
              bgcolor: 'action.selected',
            },
            '&.Mui-disabled': {
              bgcolor: 'action.disabledBackground',
            },
            minWidth: 44,
            minHeight: 44,
          }}
          aria-label="Decrease quantity"
        >
          <Remove />
        </IconButton>
        <Typography
          variant="h3"
          sx={{
            minWidth: 80,
            textAlign: 'center',
            fontWeight: 700,
            color: 'primary.main'
          }}
        >
          {localQuantity}
        </Typography>
        <IconButton
          onClick={handleIncrement}
          disabled={isUpdating}
          size="large"
          sx={{
            bgcolor: 'action.hover',
            '&:hover': {
              bgcolor: 'action.selected',
            },
            '&.Mui-disabled': {
              bgcolor: 'action.disabledBackground',
            },
            minWidth: 44,
            minHeight: 44,
          }}
          aria-label="Increase quantity"
        >
          <Add />
        </IconButton>
      </Stack>

      <Stack
        direction="row"
        spacing={1}
        sx={{ mb: 2 }}
      >
        <Button
          variant="outlined"
          size="medium"
          onClick={() => handleQuickAdd(1)}
          disabled={isUpdating}
          fullWidth
          sx={{ minHeight: 44 }}
        >
          +1
        </Button>
        <Button
          variant="outlined"
          size="medium"
          onClick={() => handleQuickAdd(5)}
          disabled={isUpdating}
          fullWidth
          sx={{ minHeight: 44 }}
        >
          +5
        </Button>
        <Button
          variant="outlined"
          size="medium"
          onClick={() => handleQuickAdd(10)}
          disabled={isUpdating}
          fullWidth
          sx={{ minHeight: 44 }}
        >
          +10
        </Button>
      </Stack>

      <Stack spacing={1}>
        <Button
          variant="contained"
          onClick={handleSave}
          disabled={isUpdating}
          fullWidth
          sx={{
            minHeight: 48,
            fontWeight: 600
          }}
        >
          Save
        </Button>
        {currentQuantity > 0 && (
          <Button
            variant="outlined"
            color="error"
            onClick={handleRemove}
            disabled={isUpdating}
            fullWidth
            sx={{ minHeight: 48 }}
          >
            Remove from Collection
          </Button>
        )}
      </Stack>
    </Box>
  );

  if (isMobile) {
    return (
      <Drawer
        anchor="bottom"
        open={open}
        onClose={onClose}
        sx={{
          '& .MuiDrawer-paper': {
            borderTopLeftRadius: 16,
            borderTopRightRadius: 16,
            maxWidth: '100%',
          }
        }}
      >
        {content}
      </Drawer>
    );
  }

  return (
    <Drawer
      anchor="right"
      open={open}
      onClose={onClose}
      sx={{
        '& .MuiDrawer-paper': {
          width: 320,
          borderTopLeftRadius: 8,
          borderBottomLeftRadius: 8,
          boxShadow: theme.shadows[8],
        }
      }}
    >
      {content}
    </Drawer>
  );
};
