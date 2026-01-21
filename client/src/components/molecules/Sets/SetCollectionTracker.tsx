import React from 'react';
import {
  Box,
  Typography,
  Checkbox,
  FormControlLabel,
  LinearProgress,
  Tooltip,
  Chip
} from '../../atoms';

interface FinishProgress {
  finishType: 'non-foil' | 'foil' | 'etched';
  collectedCards: number;
  totalCards: number;
  percentage: number;
  emoji: string;
}

interface GroupProgress {
  groupId: string;
  groupName: string;
  displayName: string;
  isSelected: boolean;
  finishes: FinishProgress[];
}

interface SetCollectionProgress {
  setId: string;
  setName: string;
  setTotalCards: number;
  overallUniqueCards: number;
  overallPercentage: number;
  overallTotalCards: number;
  groups: GroupProgress[];
}

interface SetCollectionTrackerProps {
  progress: SetCollectionProgress;
  onGroupToggle: (groupId: string, isSelected: boolean) => void;
}

export const SetCollectionTracker: React.FC<SetCollectionTrackerProps> = ({
  progress,
  onGroupToggle
}) => {
  const handleGroupChange = (groupId: string) => (event: React.ChangeEvent<HTMLInputElement>) => {
    onGroupToggle(groupId, event.target.checked);
  };

  return (
    <Box sx={{ width: '100%', mt: 2 }}>
      {/* Overall Progress */}
      <Box sx={{ mb: 2 }}>
        <Typography variant="body2" sx={{ mb: 1, textAlign: 'center' }}>
          {progress.overallUniqueCards} of {progress.setTotalCards} cards
        </Typography>
        <LinearProgress
          variant="determinate"
          value={progress.overallPercentage}
          sx={{
            height: 6,
            borderRadius: 3,
            bgcolor: 'grey.700',
            '& .MuiLinearProgress-bar': {
              borderRadius: 3,
              bgcolor: 'primary.main'
            }
          }}
        />
        <Typography variant="caption" sx={{ display: 'block', textAlign: 'center', mt: 0.5 }}>
          {progress.overallTotalCards} cards collected
        </Typography>
      </Box>

      {/* Group Breakdown */}
      {progress.groups.map((group) => (
        <Box key={group.groupId} sx={{ mb: 1.5 }}>
          <FormControlLabel
            control={
              <Checkbox
                size="small"
                checked={group.isSelected}
                onChange={handleGroupChange(group.groupId)}
                sx={{ py: 0.5 }}
              />
            }
            label={
              <Typography variant="body2" sx={{ fontSize: '0.875rem', fontWeight: 500 }}>
                {group.displayName}
              </Typography>
            }
            sx={{ mb: 0.5 }}
          />

          {/* Finish Progress Lines */}
          <Box sx={{ ml: 4 }}>
            {group.finishes.map((finish) => (
              <Tooltip
                key={finish.finishType}
                title={
                  <Box>
                    <Typography variant="caption">
                      {finish.collectedCards} of {finish.totalCards} {finish.finishType} cards
                    </Typography>
                  </Box>
                }
                placement="right"
              >
                <Box
                  sx={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: 1,
                    py: 0.25,
                    cursor: 'help',
                    '&:hover': {
                      bgcolor: 'action.hover',
                      borderRadius: 1
                    }
                  }}
                >
                  <Chip
                    label={finish.emoji}
                    size="small"
                    sx={{
                      minWidth: 32,
                      height: 20,
                      fontSize: '0.75rem',
                      bgcolor: 'grey.800',
                      color: 'white'
                    }}
                  />
                  <Typography variant="caption" sx={{ minWidth: 60 }}>
                    {finish.collectedCards} : {Math.round(finish.percentage)}%
                  </Typography>
                  <LinearProgress
                    variant="determinate"
                    value={finish.percentage}
                    sx={{
                      flex: 1,
                      height: 3,
                      borderRadius: 2,
                      bgcolor: 'grey.700',
                      '& .MuiLinearProgress-bar': {
                        borderRadius: 2,
                        bgcolor: finish.finishType === 'foil'
                          ? 'warning.main'
                          : finish.finishType === 'etched'
                            ? 'secondary.main'
                            : 'info.main'
                      }
                    }}
                  />
                </Box>
              </Tooltip>
            ))}
          </Box>
        </Box>
      ))}
    </Box>
  );
};