import React, { useState } from 'react';
import { Box, Container, Typography, Collapse } from '../../atoms';
import { useTheme } from '../../atoms';
import { ExpandMoreIcon, ExpandLessIcon } from '../../atoms/Icons';
import { touchTargetStyles } from '../../../styles/touchTargets';

export const Footer: React.FC = () => {
  const [expanded, setExpanded] = useState(false);
  const theme = useTheme();

  const handleToggle = () => {
    setExpanded(!expanded);
  };

  return (
    <Box
      component="footer"
      sx={{
        mt: 'auto',
        backgroundColor: 'background.paper',
        borderTop: '1px solid',
        borderColor: 'divider',
      }}
    >
      <Container maxWidth="lg">
        <Box
          onClick={handleToggle}
          role="button"
          tabIndex={0}
          aria-expanded={expanded}
          aria-label={expanded ? 'Collapse footer' : 'Expand footer for legal information'}
          onKeyDown={(e: React.KeyboardEvent) => {
            if (e.key === 'Enter' || e.key === ' ') {
              e.preventDefault();
              handleToggle();
            }
          }}
          sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            gap: 1,
            py: 1.5,
            cursor: 'pointer',
            ...touchTargetStyles.minimum,
            '&:hover': {
              backgroundColor: 'action.hover',
            },
            '&:focus': {
              outline: '2px solid',
              outlineColor: 'primary.main',
              outlineOffset: '-2px',
            },
          }}
        >
          <Typography
            variant="caption"
            color="text.secondary"
            sx={{ fontWeight: 500 }}
          >
            Fan Content
          </Typography>
          {expanded ? (
            <ExpandLessIcon sx={{ fontSize: 18, color: 'text.secondary' }} />
          ) : (
            <ExpandMoreIcon sx={{ fontSize: 18, color: 'text.secondary' }} />
          )}
        </Box>

        <Collapse in={expanded}>
          <Box sx={{ textAlign: 'center', pb: 2, px: 2 }}>
            <Typography
              variant="caption"
              color="text.secondary"
              sx={{
                display: 'block',
                maxWidth: '600px',
                mx: 'auto',
                lineHeight: 1.6,
              }}
            >
              MtgDiscovery is unofficial Fan Content permitted under the Fan Content Policy.
              Not approved/endorsed by Wizards.
              Portions of the materials used are property of Wizards of the Coast. ©Wizards of the Coast LLC.
            </Typography>
          </Box>
        </Collapse>
      </Container>
    </Box>
  );
};