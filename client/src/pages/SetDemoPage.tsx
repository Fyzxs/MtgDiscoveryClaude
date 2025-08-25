import React, { useState, useEffect } from 'react';
import { useLazyQuery } from '@apollo/client/react';
import { GET_SETS_BY_CODE } from '../graphql/queries/sets';
import type { SetContext } from '../types/set';
import { MtgSetCard } from '../components/organisms/MtgSetCard';
import { MuiCard } from '../components/atoms/shared/MuiCard';
import { MuiButton } from '../components/atoms/shared/MuiButton';
import { MuiInput } from '../components/atoms/shared/MuiInput';
import { Box, Typography, FormControlLabel, Checkbox } from '@mui/material';

export const SetDemoPage: React.FC = () => {
  const [inputValue, setInputValue] = useState('');
  const [context, setContext] = useState<SetContext>({
    isOnSetListPage: false,
    isOnBlockPage: false,
  });

  const [getSets, { loading, error, data }] = useLazyQuery(GET_SETS_BY_CODE);

  const loadedSet = (data as any)?.setsByCode?.data?.[0];
  
  useEffect(() => {
    if (loadedSet) {
      console.log('Loaded set data:', loadedSet);
      console.log('Set name:', loadedSet.name);
      console.log('Set code:', loadedSet.code);
      console.log('Set type:', loadedSet.setType);
      console.log('Set block:', loadedSet.block);
      console.log('Card count:', loadedSet.cardCount);
      console.log('Release date:', loadedSet.releasedAt);
    }
  }, [loadedSet]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    getSets({ variables: { codes: { setCodes: [inputValue] } } });
  };

  const handleSetClick = (setCode?: string) => {
    console.log('Navigate to set:', setCode);
  };

  return (
    <div className="min-h-screen bg-gray-950">
      <div className="container mx-auto px-4 py-8">
        <div className="mb-8">
          <MuiButton 
            onClick={() => window.location.reload()}
            variant="outlined" 
            size="small"
            sx={{ mb: 2 }}
          >
            ← Back to Home
          </MuiButton>
          <h1 className="text-3xl font-bold text-white mb-2">Set Component Demo</h1>
          <p className="text-gray-400">
            Demonstration of MTG set display components
          </p>
        </div>

        <MuiCard className="mb-8">
          <Box component="form" onSubmit={handleSubmit} sx={{ display: 'flex', gap: 2 }}>
            <MuiInput
              label="Set Code"
              value={inputValue}
              onChange={(e) => setInputValue(e.target.value)}
              placeholder="Enter set code (e.g., lea, mh3, dsk)"
              variant="outlined"
              sx={{ flexGrow: 1 }}
            />
            <MuiButton
              type="submit"
              variant="contained"
              color="primary"
              isLoading={loading}
              sx={{ minWidth: 120 }}
            >
              Load Set
            </MuiButton>
          </Box>
          {error && (
            <div className="mt-2 text-red-500 text-sm">
              <p>Error loading set: {error.message}</p>
              {error.message.includes('fetch') && (
                <p className="mt-1 text-yellow-500">
                  Make sure the GraphQL backend is running: 
                  <code className="ml-2 px-2 py-1 bg-gray-200 dark:bg-gray-800 text-gray-900 dark:text-gray-100 rounded">
                    dotnet run --project ../src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj
                  </code>
                </p>
              )}
            </div>
          )}
        </MuiCard>

        <MuiCard className="mb-8">
          <Typography variant="h6" gutterBottom>Context Settings</Typography>
          <Typography variant="body2" color="textSecondary" paragraph>
            Control how the set components render in different page contexts
          </Typography>
          <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
            <FormControlLabel
              control={
                <Checkbox
                  checked={context.isOnSetListPage}
                  onChange={(e) => setContext({
                    ...context, 
                    isOnSetListPage: e.target.checked,
                  })}
                />
              }
              label="On Set List Page"
            />
            <FormControlLabel
              control={
                <Checkbox
                  checked={context.isOnBlockPage}
                  onChange={(e) => setContext({
                    ...context, 
                    isOnBlockPage: e.target.checked,
                    currentBlock: e.target.checked && loadedSet?.block ? loadedSet.block : undefined
                  })}
                />
              }
              label="On Block Page (hides block info)"
            />
          </Box>
        </MuiCard>

        {loadedSet ? (
          <>
            <MuiCard elevation={3} className="mb-8">
              <Typography variant="h5" gutterBottom>Single Set Display</Typography>
              <Box sx={{ display: 'flex', justifyContent: 'center', p: 2 }}>
                <Box sx={{ width: '280px' }}>
                  <MtgSetCard
                    set={loadedSet}
                    context={context}
                    onSetClick={handleSetClick}
                  />
                </Box>
              </Box>
            </MuiCard>

            <MuiCard elevation={3} className="mb-8">
              <Typography variant="h5" gutterBottom>3×3 Grid Display</Typography>
              <Typography variant="body2" color="textSecondary" paragraph>
                Nine instances of the same set to demonstrate multi-set layouts
              </Typography>
              <Box sx={{ 
                display: 'grid', 
                gridTemplateColumns: 'repeat(3, 1fr)', 
                gap: 3, 
                justifyItems: 'center',
                maxWidth: '1200px',
                mx: 'auto',
                p: 2
              }}>
                {Array.from({ length: 9 }, (_, index) => {
                  const uniqueSetId = `${loadedSet.id}-grid-${index}`;
                  return (
                    <MtgSetCard
                      key={index}
                      set={{...loadedSet, id: uniqueSetId}}
                      context={context}
                      onSetClick={handleSetClick}
                    />
                  );
                })}
              </Box>
            </MuiCard>
          </>
        ) : (
          <MuiCard elevation={2} className="mb-8">
            <Box sx={{ textAlign: 'center', py: 8 }}>
              <Box sx={{ fontSize: 64, opacity: 0.3, mb: 2 }}>📦</Box>
              <Typography variant="h6" gutterBottom>No Set Loaded</Typography>
              <Typography variant="body2" color="textSecondary">
                Enter a set code above and click "Load Set" to see the MTG set display components.
              </Typography>
            </Box>
          </MuiCard>
        )}
      </div>
    </div>
  );
};