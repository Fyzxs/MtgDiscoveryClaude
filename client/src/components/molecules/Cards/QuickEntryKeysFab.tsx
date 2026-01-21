import React, { useState } from 'react';
import { Fab, Zoom, Paper, Box, Typography, IconButton, Collapse } from '@mui/material';
import KeyboardIcon from '@mui/icons-material/Keyboard';
import CloseIcon from '@mui/icons-material/Close';
import { useTheme } from '@mui/material/styles';
import { useLocation } from 'react-router-dom';
import { useUser } from '../../../contexts/UserContext';

export const QuickEntryKeysFab: React.FC = () => {
  const [isExpanded, setIsExpanded] = useState(false);
  const theme = useTheme();
  const location = useLocation();
  const { userProfile } = useUser();

  // Check if current page has cards that can be added to collection
  const hasCards = () => {
    const pathname = location.pathname;
    return pathname.includes('/set/') ||
           pathname.includes('/artists/') ||
           pathname.includes('/card/');
  };

  // Check if ctor parameter is in URL
  const hasCtorParam = () => {
    const urlParams = new URLSearchParams(location.search);
    return urlParams.has('ctor');
  };

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

  // Only show FAB if:
  // 1. User is authenticated
  // 2. Current page has cards that can be added to collection
  // 3. ctor parameter is present in URL
  if (!userProfile?.id || !hasCards() || !hasCtorParam()) {
    return null;
  }

  return (
    <>
      <Zoom in={!isExpanded}>
        <Fab
          color="secondary"
          size="medium"
          onClick={() => setIsExpanded(true)}
          sx={{
            position: 'fixed',
            bottom: 16,
            right: 88,
            zIndex: 1000
          }}
          aria-label="show quick entry keys"
        >
          <KeyboardIcon />
        </Fab>
      </Zoom>

      <Collapse in={isExpanded} orientation="horizontal">
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
            minWidth: 280,
            maxWidth: 320
          }}
        >
          <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 1.5 }}>
            <Typography
              variant="subtitle2"
              sx={{
                color: 'primary.main',
                fontWeight: 'bold'
              }}
            >
              Quick Entry Keys
            </Typography>
            <IconButton
              size="small"
              onClick={() => setIsExpanded(false)}
              sx={{ color: 'text.secondary' }}
              aria-label="close quick entry keys"
            >
              <CloseIcon fontSize="small" />
            </IconButton>
          </Box>

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
      </Collapse>
    </>
  );
};