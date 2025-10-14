import React from 'react';
import { Box, Typography, Paper } from '../../atoms';
import { useTheme } from '../../atoms';

interface CardEntryHelpPanelProps {
  isVisible: boolean;
}

export const CardEntryHelpPanel: React.FC<CardEntryHelpPanelProps> = ({
  isVisible
}) => {
  const theme = useTheme();

  const helpItems = [
    { keys: '0-9', description: 'Quantity' },
    { keys: '+/`', description: 'Increment' },
    { keys: '-/~', description: 'Decrement' },
    { keys: 'Z/N', description: 'Non-foil' },
    { keys: 'F/O', description: 'Foil' },
    { keys: 'E/H', description: 'Etched' },
    { keys: 'G/I', description: 'Signed' },
    { keys: 'R/P', description: 'Artist Proof' },
    { keys: 'T/M', description: 'Modified/Altered' },
    { keys: 'X', description: 'Negate Quantity' },
    { keys: 'Enter', description: 'Confirm' },
    { keys: 'Esc', description: 'Cancel' }
  ];

  if (!isVisible) return null;

  return (
    <Paper
      elevation={8}
      sx={{
        position: 'fixed',
        bottom: theme.spacing(2),
        right: theme.spacing(2),
        p: 2,
        bgcolor: 'grey.900',
        border: '1px solid',
        borderColor: 'primary.dark',
        zIndex: 1300,
        minWidth: 240
      }}
    >
      <Typography
        variant="subtitle2"
        sx={{
          color: 'primary.main',
          fontWeight: 'bold',
          mb: 1.5,
          textAlign: 'center',
          borderBottom: '1px solid',
          borderColor: 'divider',
          pb: 1
        }}
      >
        Quick Entry Keys
      </Typography>

      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
        {helpItems.map((item, index) => (
          <Box
            key={index}
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center',
              py: 0.25
            }}
          >
            <Typography
              variant="body2"
              sx={{
                fontFamily: 'monospace',
                color: 'info.main',
                fontWeight: 500,
                minWidth: 60
              }}
            >
              {item.keys}
            </Typography>
            <Typography
              variant="body2"
              sx={{
                color: 'text.secondary',
                ml: 2
              }}
            >
              {item.description}
            </Typography>
          </Box>
        ))}
      </Box>
    </Paper>
  );
};