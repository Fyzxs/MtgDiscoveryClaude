import React from 'react';
import {
  Drawer,
  Box,
  Typography,
  IconButton,
  Button,
  Divider,
} from '../../atoms';
import { CloseIcon } from '../../atoms';
import { touchTargetStyles } from '../../../styles/touchTargets';

interface EdgeDrawerProps {
  open: boolean;
  onOpen: () => void;
  onClose: () => void;
  children: React.ReactNode;
  /** Label shown on the edge tab (displayed sideways) */
  tabLabel?: string;
  /** Badge count shown on tab */
  activeCount?: number;
  /** Title shown in drawer header */
  title?: string;
}

/**
 * Right-edge drawer with a persistent tab handle
 * Tab is always visible on the right edge of the screen
 * Tapping the tab slides the drawer out from the right
 */
export const EdgeDrawer: React.FC<EdgeDrawerProps> = ({
  open,
  onOpen,
  onClose,
  children,
  tabLabel = 'Filter',
  activeCount = 0,
  title = 'Filters',
}) => {
  return (
    <>
      {/* Edge Tab - always visible when drawer is closed */}
      {!open && (
        <Box
          onClick={onOpen}
          role="button"
          tabIndex={0}
          aria-label={`Open ${tabLabel}`}
          onKeyDown={(e: React.KeyboardEvent) => {
            if (e.key === 'Enter' || e.key === ' ') {
              e.preventDefault();
              onOpen();
            }
          }}
          sx={{
            position: 'fixed',
            right: 0,
            top: 80,
            zIndex: 1200,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            bgcolor: 'primary.main',
            color: 'primary.contrastText',
            borderTopLeftRadius: 8,
            borderBottomLeftRadius: 8,
            cursor: 'pointer',
            boxShadow: 3,
            // Tab dimensions
            width: 28,
            minHeight: 100,
            py: 1.5,
            '&:hover': {
              bgcolor: 'primary.dark',
            },
            '&:focus': {
              outline: '2px solid',
              outlineColor: 'primary.light',
              outlineOffset: '2px',
            },
          }}
        >
          {/* Sideways text */}
          <Typography
            sx={{
              writingMode: 'vertical-rl',
              textOrientation: 'mixed',
              transform: 'rotate(180deg)',
              fontSize: '0.75rem',
              fontWeight: 600,
              letterSpacing: '0.05em',
              textTransform: 'uppercase',
              whiteSpace: 'nowrap',
            }}
          >
            {tabLabel}
            {activeCount > 0 && ` (${activeCount})`}
          </Typography>
        </Box>
      )}

      {/* Drawer */}
      <Drawer
        anchor="right"
        open={open}
        onClose={onClose}
        PaperProps={{
          sx: {
            width: '85vw',
            maxWidth: 320,
            bgcolor: 'background.paper',
          },
        }}
      >
        {/* Header */}
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
            px: 2,
            py: 1.5,
            borderBottom: '1px solid',
            borderColor: 'divider',
          }}
        >
          <Typography variant="h6" sx={{ fontWeight: 600 }}>
            {title}
            {activeCount > 0 && (
              <Typography
                component="span"
                sx={{ ml: 1, color: 'primary.main', fontSize: '0.875rem' }}
              >
                ({activeCount} active)
              </Typography>
            )}
          </Typography>
          <IconButton
            onClick={onClose}
            aria-label="Close"
            sx={{ ...touchTargetStyles.minimum }}
          >
            <CloseIcon />
          </IconButton>
        </Box>

        {/* Content */}
        <Box
          sx={{
            overflow: 'auto',
            flex: 1,
            p: 2,
          }}
        >
          {children}
        </Box>

        {/* Footer */}
        <Box
          sx={{
            p: 2,
            borderTop: '1px solid',
            borderColor: 'divider',
            pb: 'calc(16px + env(safe-area-inset-bottom, 0px))',
          }}
        >
          <Button
            fullWidth
            variant="contained"
            onClick={onClose}
            sx={{ ...touchTargetStyles.comfortable }}
          >
            Apply
          </Button>
        </Box>
      </Drawer>
    </>
  );
};
