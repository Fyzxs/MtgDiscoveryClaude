import React, { useState } from 'react';
import { 
  AppBar, 
  Toolbar, 
  Typography, 
  Box, 
  Button
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { SearchInput } from '../atoms/shared/SearchInput';

export const Header: React.FC = () => {
  const [setCode, setSetCode] = useState('');
  const theme = useTheme();

  const handleSetCodeSubmit = () => {
    if (setCode.trim()) {
      // Navigate to set page
      window.location.href = `?page=set&set=${setCode.trim().toLowerCase()}`;
      setSetCode('');
    }
  };

  return (
    <AppBar 
      position="sticky" 
      sx={{ 
        backgroundColor: 'background.paper',
        backgroundImage: 'none',
        borderBottom: '1px solid',
        borderColor: 'divider'
      }}
    >
      <Toolbar sx={{ gap: 3 }}>
        {/* Site Logo/Name */}
        <Typography 
          variant="h5" 
          component="div"
          sx={{ 
            fontWeight: 'bold',
            background: theme.mtg.gradients.header,
            backgroundClip: 'text',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
            cursor: 'pointer',
            '&:hover': {
              background: theme.mtg.gradients.hover,
              backgroundClip: 'text',
              WebkitBackgroundClip: 'text',
              WebkitTextFillColor: 'transparent',
            }
          }}
          onClick={() => window.location.href = '/'}
        >
          MtgDiscovery
        </Typography>

        {/* Navigation Links */}
        <Box sx={{ display: 'flex', gap: 2, alignItems: 'center' }}>
          <Button 
            color="primary" 
            onClick={() => window.location.href = '?page=all-sets'}
            sx={{ 
              textTransform: 'none',
              fontWeight: 500
            }}
          >
            All Sets
          </Button>
        </Box>

        {/* Spacer */}
        <Box sx={{ flexGrow: 1 }} />

        {/* Set Code Search */}
        <SearchInput
          value={setCode}
          onChange={setSetCode}
          onSubmit={handleSetCodeSubmit}
          placeholder="Jump to Set"
          label="Set Code"
          expandable={true}
          expandedWidth={200}
          collapsedWidth={150}
          size="small"
        />
      </Toolbar>
    </AppBar>
  );
};