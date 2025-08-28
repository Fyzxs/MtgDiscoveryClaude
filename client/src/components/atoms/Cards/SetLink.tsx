import React from 'react';
import { Link, Typography, Box } from '@mui/material';
import { SetIcon } from '../Sets/SetIcon';

interface SetLinkProps {
  setCode?: string;
  setName?: string;
  rarity?: string;
  onSetClick?: (setCode?: string) => void;
  className?: string;
}

export const SetLink: React.FC<SetLinkProps> = ({ 
  setCode,
  setName,
  rarity,
  onSetClick,
  className 
}) => {
  if (!setName) return null;

  return (
    <Box className={className}>
      <Link
        href={`/sets/${setCode?.toLowerCase()}`}
        tabIndex={-1}
        onClick={(e) => {
          e.stopPropagation();
          if (onSetClick) {
            e.preventDefault();
            onSetClick(setCode);
          }
        }}
        sx={{
          display: 'inline-flex',
          alignItems: 'center',
          gap: 0.5,
          color: 'white',
          textDecoration: 'none',
          fontSize: '0.75rem',
          px: 0.5,
          py: 0.25,
          borderRadius: 1,
          '&:hover': {
            bgcolor: 'rgba(0, 0, 0, 1)',
            color: 'primary.main'
          },
          transition: 'all 0.2s ease'
        }}
        aria-label={`View all cards from ${setName}`}
      >
        {setCode && (
          <SetIcon 
            setCode={setCode} 
            rarity={rarity}
            size="small"
            className="group-hover/set:opacity-100"
          />
        )}
        <Typography 
          variant="caption" 
          sx={{ 
            fontSize: 'inherit',
            overflow: 'hidden',
            textOverflow: 'ellipsis',
            whiteSpace: 'nowrap'
          }}
        >
          {setName}{setCode && ` (${setCode})`}
        </Typography>
      </Link>
    </Box>
  );
};