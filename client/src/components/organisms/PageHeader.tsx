import React from 'react';
import { 
  Box, 
  Typography, 
  Breadcrumbs, 
  Link,
  Chip,
  Avatar,
  Stack,
  SxProps,
  Theme,
  Divider
} from '@mui/material';
import { BadgeGroup } from '../molecules/shared/BadgeGroup';

export interface BreadcrumbItem {
  label: string;
  href?: string;
  onClick?: () => void;
  disabled?: boolean;
}

export interface PageAction {
  label: string;
  onClick: () => void;
  variant?: 'text' | 'outlined' | 'contained';
  color?: 'primary' | 'secondary' | 'success' | 'error' | 'warning' | 'info';
  disabled?: boolean;
  icon?: React.ReactNode;
}

export interface PageStat {
  label: string;
  value: string | number;
  color?: 'default' | 'primary' | 'secondary' | 'success' | 'error' | 'warning' | 'info';
}

interface PageHeaderProps {
  title: string;
  subtitle?: string;
  description?: string;
  breadcrumbs?: BreadcrumbItem[];
  avatar?: {
    src?: string;
    alt?: string;
    fallback?: string;
  };
  badges?: React.ReactNode[];
  stats?: PageStat[];
  actions?: PageAction[];
  showDivider?: boolean;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Consistent page header component for all main pages
 * Provides title, breadcrumbs, stats, actions, and other common header elements
 */
export const PageHeader: React.FC<PageHeaderProps> = React.memo(({
  title,
  subtitle,
  description,
  breadcrumbs = [],
  avatar,
  badges = [],
  stats = [],
  actions = [],
  showDivider = true,
  sx = {},
  className = ''
}) => {
  const handleBreadcrumbClick = (item: BreadcrumbItem) => {
    if (item.onClick && !item.disabled) {
      item.onClick();
    }
  };

  return (
    <Box 
      className={className}
      sx={{ 
        mb: 3,
        ...sx 
      }}
    >
      {/* Breadcrumbs */}
      {breadcrumbs.length > 0 && (
        <Breadcrumbs sx={{ mb: 2 }}>
          {breadcrumbs.map((item, index) => (
            <Link
              key={index}
              href={item.href}
              onClick={item.onClick ? () => handleBreadcrumbClick(item) : undefined}
              color={item.disabled ? 'text.disabled' : 'inherit'}
              underline={item.disabled ? 'none' : 'hover'}
              sx={{ 
                cursor: item.onClick && !item.disabled ? 'pointer' : 'default',
                pointerEvents: item.disabled ? 'none' : 'auto'
              }}
            >
              {item.label}
            </Link>
          ))}
        </Breadcrumbs>
      )}

      {/* Main Header Content */}
      <Box sx={{ 
        display: 'flex', 
        alignItems: 'flex-start',
        justifyContent: 'space-between',
        flexWrap: 'wrap',
        gap: 2,
        mb: 2
      }}>
        {/* Left Side - Title, Avatar, Info */}
        <Box sx={{ display: 'flex', alignItems: 'flex-start', gap: 2, flex: 1, minWidth: 0 }}>
          {/* Avatar */}
          {avatar && (
            <Avatar
              src={avatar.src}
              alt={avatar.alt}
              sx={{ width: 64, height: 64, fontSize: '1.5rem' }}
            >
              {avatar.fallback}
            </Avatar>
          )}

          {/* Title and Info */}
          <Box sx={{ flex: 1, minWidth: 0 }}>
            <Typography 
              variant="h4" 
              component="h1" 
              gutterBottom
              sx={{ 
                fontWeight: 'bold',
                wordBreak: 'break-word'
              }}
            >
              {title}
            </Typography>

            {subtitle && (
              <Typography 
                variant="h6" 
                color="text.secondary" 
                gutterBottom
                sx={{ 
                  fontWeight: 'normal',
                  wordBreak: 'break-word'
                }}
              >
                {subtitle}
              </Typography>
            )}

            {description && (
              <Typography 
                variant="body1" 
                color="text.secondary" 
                sx={{ 
                  mb: 1,
                  wordBreak: 'break-word'
                }}
              >
                {description}
              </Typography>
            )}

            {/* Badges */}
            {badges.length > 0 && (
              <BadgeGroup spacing={1} sx={{ mb: 1 }}>
                {badges}
              </BadgeGroup>
            )}

            {/* Stats */}
            {stats.length > 0 && (
              <Stack direction="row" spacing={2} flexWrap="wrap" sx={{ gap: 1 }}>
                {stats.map((stat, index) => (
                  <Chip
                    key={index}
                    label={`${stat.label}: ${stat.value}`}
                    variant="outlined"
                    size="small"
                    color={stat.color}
                  />
                ))}
              </Stack>
            )}
          </Box>
        </Box>

        {/* Right Side - Actions */}
        {actions.length > 0 && (
          <Stack direction="row" spacing={1} flexWrap="wrap" sx={{ gap: 1 }}>
            {actions.map((action, index) => (
              <Chip
                key={index}
                label={action.label}
                variant={action.variant || 'outlined'}
                color={action.color || 'default'}
                disabled={action.disabled}
                onClick={action.onClick}
                clickable
                icon={action.icon}
              />
            ))}
          </Stack>
        )}
      </Box>

      {showDivider && <Divider />}
    </Box>
  );
});

interface SimplePageHeaderProps {
  title: string;
  subtitle?: string;
  backLabel?: string;
  onBack?: () => void;
  children?: React.ReactNode;
  sx?: SxProps<Theme>;
  className?: string;
}

/**
 * Simplified page header for basic pages
 */
export const SimplePageHeader: React.FC<SimplePageHeaderProps> = React.memo(({
  title,
  subtitle,
  backLabel = 'Back',
  onBack,
  children,
  sx = {},
  className = ''
}) => {
  const breadcrumbs = onBack ? [{ label: backLabel, onClick: onBack }] : [];

  return (
    <PageHeader
      title={title}
      subtitle={subtitle}
      breadcrumbs={breadcrumbs}
      sx={sx}
      className={className}
    >
      {children}
    </PageHeader>
  );
});

PageHeader.displayName = 'PageHeader';
SimplePageHeader.displayName = 'SimplePageHeader';