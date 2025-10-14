import { Box, Typography } from '..';
import type { StyledComponentProps } from '../../../types/components';

interface CollectorNumberProps extends StyledComponentProps {
  number?: string;
  setCode?: string;
}

export const CollectorNumber = ({ 
  number, 
  setCode,
  className,
  sx
}: CollectorNumberProps) => {
  if (!number) return null;

  return (
    <Box 
      className={className}
      sx={{ 
        display: 'flex', 
        alignItems: 'center', 
        gap: 0.5, 
        color: 'grey.400',
        fontSize: '0.875rem',
        ...sx
      }}
    >
      <Typography 
        variant="caption" 
        sx={{ 
          fontFamily: 'monospace',
          fontSize: 'inherit'
        }}
      >
        #{number}
      </Typography>
      {setCode && (
        <Typography 
          variant="caption" 
          sx={{ 
            fontSize: '0.75rem', 
            textTransform: 'uppercase', 
            color: 'grey.500'
          }}
        >
          • {setCode}
        </Typography>
      )}
    </Box>
  );
};