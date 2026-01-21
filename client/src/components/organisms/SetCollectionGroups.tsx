import React from 'react';
import { Box, Checkbox, FormControlLabel, Typography, LinearProgress } from '@mui/material';
import { useTheme } from '@mui/material/styles';

interface GroupFinishProgress {
  finishType: 'nonFoil' | 'foil' | 'etched';
  collected: number;
  total: number;
  percentage: number;
  emoji: string;
}

interface CollectionGroup {
  setGroupId: string;
  displayName: string;
  isCollecting: boolean;
  finishes: GroupFinishProgress[];
}

interface SetCollectionGroupsProps {
  groups: CollectionGroup[];
  onGroupToggle: (groupId: string, isCollecting: boolean) => void;
}

const FINISH_EMOJIS: Record<string, string> = {
  nonFoil: '🔹',
  foil: '✨',
  etched: '⚡'
};

export const SetCollectionGroups: React.FC<SetCollectionGroupsProps> = ({
  groups,
  onGroupToggle
}) => {
  const theme = useTheme();

  if (groups.length === 0) {
    return null;
  }

  return (
    <Box sx={{ width: '100%', mt: 2, mb: 3 }}>
      <Typography variant="h6" sx={{ mb: 2 }}>
        Collection Tracking
      </Typography>

      {groups.map((group) => (
        <Box
          key={group.setGroupId}
          sx={{
            mb: 2,
            p: 2,
            border: `1px solid ${theme.palette.divider}`,
            borderRadius: 1,
            bgcolor: theme.palette.background.paper
          }}
        >
          <FormControlLabel
            control={
              <Checkbox
                checked={group.isCollecting}
                onChange={(e) => onGroupToggle(group.setGroupId, e.target.checked)}
                sx={{ mr: 1 }}
              />
            }
            label={
              <Typography variant="subtitle1" sx={{ fontWeight: 600 }}>
                {group.displayName}
              </Typography>
            }
          />

          <Box sx={{ mt: 1, ml: 4 }}>
            {group.finishes.map((finish) => (
              <Box
                key={finish.finishType}
                sx={{
                  display: 'flex',
                  alignItems: 'center',
                  mb: 1,
                  gap: 1
                }}
              >
                <Typography
                  variant="body2"
                  sx={{ minWidth: '120px', fontSize: '0.875rem' }}
                >
                  {finish.emoji} {finish.finishType === 'nonFoil' ? 'Non-foil' : finish.finishType === 'foil' ? 'Foil' : 'Etched'}
                </Typography>

                <Box sx={{ flex: 1, position: 'relative' }}>
                  <LinearProgress
                    variant="determinate"
                    value={finish.percentage}
                    sx={{
                      height: 20,
                      borderRadius: 1,
                      bgcolor: theme.palette.grey[800],
                      '& .MuiLinearProgress-bar': {
                        borderRadius: 1,
                        background: `linear-gradient(90deg, ${theme.palette.primary.main} 0%, ${theme.palette.primary.light} 100%)`,
                      }
                    }}
                  />
                  <Typography
                    variant="caption"
                    sx={{
                      position: 'absolute',
                      top: '50%',
                      left: '50%',
                      transform: 'translate(-50%, -50%)',
                      fontSize: '0.7rem',
                      fontWeight: 600,
                      color: 'text.primary',
                      textShadow: '0 1px 2px rgba(0,0,0,0.8)'
                    }}
                  >
                    {finish.collected} / {finish.total} ({Math.round(finish.percentage)}%)
                  </Typography>
                </Box>
              </Box>
            ))}
          </Box>
        </Box>
      ))}
    </Box>
  );
};
