import React, { useState } from 'react';
import { 
  Box, 
  Typography, 
  IconButton,
  Popover,
  Table,
  TableBody,
  TableCell,
  TableRow,
  Chip
} from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';

interface SlabbedCard {
  grade: string;
  count: number;
}

interface StandardCard {
  finish: 'nonfoil' | 'foil' | 'etched';
  special: 'none' | 'signed' | 'proof' | 'altered';
  count: number;
}

interface CollectionData {
  slabbed?: SlabbedCard[];
  standard?: StandardCard[];
}

interface CollectionSummaryProps {
  collection: CollectionData;
  size?: 'small' | 'medium' | 'large';
}

export const CollectionSummary: React.FC<CollectionSummaryProps> = ({ 
  collection, 
  size = 'medium' 
}) => {
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);

  // Calculate totals
  const slabbedTotal = collection.slabbed?.reduce((sum, item) => sum + item.count, 0) || 0;
  const standardTotal = collection.standard?.reduce((sum, item) => sum + item.count, 0) || 0;
  const totalCards = slabbedTotal + standardTotal;

  if (totalCards === 0) return null;

  // Determine which finish types are present
  const finishTypes = new Set(collection.standard?.map(item => item.finish) || []);
  const hasSlabbed = slabbedTotal > 0;

  // Get finish indicators
  const getFinishIndicators = () => {
    const indicators: string[] = [];
    if (hasSlabbed) indicators.push('🏆');
    if (finishTypes.has('nonfoil')) indicators.push('📄');
    if (finishTypes.has('foil')) indicators.push('✨');
    if (finishTypes.has('etched')) indicators.push('🌟');
    return indicators.join('');
  };

  const handlePopoverOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handlePopoverClose = () => {
    setAnchorEl(null);
  };

  const open = Boolean(anchorEl);

  // Group standard cards by finish for display
  const groupedByFinish = collection.standard?.reduce((acc, item) => {
    if (!acc[item.finish]) acc[item.finish] = [];
    acc[item.finish].push(item);
    return acc;
  }, {} as Record<string, StandardCard[]>) || {};

  const getFinishDisplayName = (finish: string) => {
    switch (finish) {
      case 'nonfoil': return 'Regular';
      case 'foil': return 'Foil';
      case 'etched': return 'Etched';
      default: return finish;
    }
  };

  const getSpecialDisplayName = (special: string) => {
    switch (special) {
      case 'none': return '';
      case 'signed': return 'Signed';
      case 'proof': return 'Artist Proof';
      case 'altered': return 'Altered';
      default: return special;
    }
  };

  const fontSize = {
    small: '0.875rem',
    medium: '1rem',
    large: '1.125rem'
  }[size];

  return (
    <Box sx={{ display: 'inline-flex', alignItems: 'center', gap: 0.5 }}>
      <Typography 
        variant="body2" 
        sx={{ 
          fontSize,
          fontWeight: 500,
          color: 'text.primary'
        }}
      >
        {totalCards} owned
      </Typography>
      
      {getFinishIndicators() && (
        <Typography 
          variant="body2" 
          sx={{ 
            fontSize: '0.9em',
            lineHeight: 1
          }}
        >
          {getFinishIndicators()}
        </Typography>
      )}

      <IconButton
        size="small"
        onClick={handlePopoverOpen}
        sx={{ 
          ml: 0.5,
          p: 0.25,
          '& .MuiSvgIcon-root': {
            fontSize: '1rem'
          }
        }}
      >
        <InfoIcon fontSize="inherit" />
      </IconButton>

      <Popover
        open={open}
        anchorEl={anchorEl}
        onClose={handlePopoverClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        sx={{
          mt: 0.5
        }}
      >
        <Box sx={{ p: 2, minWidth: 280 }}>
          <Typography variant="h6" gutterBottom sx={{ fontSize: '1rem', fontWeight: 600 }}>
            Collection Breakdown
          </Typography>

          {hasSlabbed && (
            <Box sx={{ mb: 2 }}>
              <Typography variant="subtitle2" sx={{ mb: 1, display: 'flex', alignItems: 'center', gap: 0.5 }}>
                🏆 Slabbed ({slabbedTotal})
              </Typography>
              <Table size="small">
                <TableBody>
                  {collection.slabbed?.map((slab, index) => (
                    <TableRow key={index}>
                      <TableCell sx={{ py: 0.5, px: 1, border: 'none' }}>
                        Grade {slab.grade}
                      </TableCell>
                      <TableCell sx={{ py: 0.5, px: 1, border: 'none', textAlign: 'right' }}>
                        {slab.count}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </Box>
          )}

          {Object.entries(groupedByFinish).map(([finish, cards]) => {
            const finishTotal = cards.reduce((sum, card) => sum + card.count, 0);
            const finishIcon = finish === 'nonfoil' ? '📄' : finish === 'foil' ? '✨' : '🌟';
            
            return (
              <Box key={finish} sx={{ mb: 2 }}>
                <Typography variant="subtitle2" sx={{ mb: 1, display: 'flex', alignItems: 'center', gap: 0.5 }}>
                  {finishIcon} {getFinishDisplayName(finish)} ({finishTotal})
                </Typography>
                <Table size="small">
                  <TableBody>
                    {cards.map((card, index) => (
                      <TableRow key={index}>
                        <TableCell sx={{ py: 0.5, px: 1, border: 'none' }}>
                          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                            {getSpecialDisplayName(card.special) || 'Regular'}
                            {card.special !== 'none' && (
                              <Chip 
                                label={getSpecialDisplayName(card.special)} 
                                size="small" 
                                variant="outlined"
                                sx={{ height: '20px', fontSize: '0.7rem' }}
                              />
                            )}
                          </Box>
                        </TableCell>
                        <TableCell sx={{ py: 0.5, px: 1, border: 'none', textAlign: 'right' }}>
                          {card.count}
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </Box>
            );
          })}
        </Box>
      </Popover>
    </Box>
  );
};