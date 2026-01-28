import React from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Tooltip,
  LockIcon,
} from '../../../atoms';
import { AppButton } from '../../../molecules/shared/AppButton';

interface FeatureCardProps {
  icon: React.ComponentType;
  title: string;
  description: string;
  ctaLabel: string;
  route: string;
  authRequired: boolean;
  isAuthenticated: boolean;
}

export const FeatureCard: React.FC<FeatureCardProps> = React.memo(({
  icon: Icon,
  title,
  description,
  ctaLabel,
  route,
  authRequired,
  isAuthenticated,
}) => {
  const navigate = useNavigate();
  const featureId = title.toLowerCase().replace(/\s+/g, '-');

  return (
    <Card
      role="article"
      aria-labelledby={`feature-${featureId}-title`}
      sx={{
        position: 'relative',
        bgcolor: 'background.paper',
        border: '1px solid',
        borderColor: 'transparent',
        transition: 'all 0.2s ease-in-out',
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: 8,
          borderColor: 'primary.main',
        },
        '&:focus-within': {
          outline: '2px solid',
          outlineColor: 'primary.main',
          outlineOffset: 2,
        },
      }}
    >
      {authRequired && !isAuthenticated && (
        <Tooltip title="Sign in required to access this feature">
          <Box
            sx={{
              position: 'absolute',
              top: 12,
              right: 12,
              color: 'text.secondary',
            }}
            aria-label="Sign in required to access this feature"
          >
            <LockIcon fontSize="small" />
          </Box>
        </Tooltip>
      )}
      <CardContent
        sx={{
          flex: 1,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          textAlign: 'center',
          p: 3,
        }}
      >
        <Box
          sx={{
            width: 56,
            height: 56,
            borderRadius: '50%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            background: (theme) => theme.mtg.gradients.header,
            mb: 2,
          }}
        >
          <Icon />
        </Box>
        <Typography
          id={`feature-${featureId}-title`}
          variant="h6"
          component="h3"
          gutterBottom
        >
          {title}
        </Typography>
        <Typography
          variant="body2"
          color="text.secondary"
          sx={{ mb: 2, flex: 1 }}
        >
          {description}
        </Typography>
        <AppButton
          variant="text"
          onClick={() => navigate(route)}
          sx={{ mt: 'auto' }}
        >
          {ctaLabel}
        </AppButton>
      </CardContent>
    </Card>
  );
});
