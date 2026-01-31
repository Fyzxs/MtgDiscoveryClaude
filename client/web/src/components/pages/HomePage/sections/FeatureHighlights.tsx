import React from 'react';
import { Box, Container, Grid, Typography } from '../../../atoms';
import { FeatureCard } from '../components/FeatureCard';
import { featureCards } from '../data/featureCards';

interface FeatureHighlightsProps {
  isAuthenticated: boolean;
}

export const FeatureHighlights: React.FC<FeatureHighlightsProps> = ({
  isAuthenticated,
}) => {
  return (
    <Box
      component="section"
      aria-labelledby="features-heading"
      sx={{
        py: { xs: 4, md: 8 },
      }}
    >
      <Container maxWidth="lg">
        <Typography
          id="features-heading"
          variant="h2"
          component="h2"
          align="center"
          gutterBottom
          sx={{
            mb: 6,
            fontWeight: 700,
          }}
        >
          Everything You Need to Manage Your Collection
        </Typography>

        <Grid container spacing={3}>
          {featureCards.map((feature) => (
            <Grid key={feature.id} size={{ xs: 12, sm: 6, md: 3 }}>
              <FeatureCard
                icon={feature.icon}
                title={feature.title}
                description={feature.description}
                ctaLabel={feature.ctaLabel}
                route={feature.route}
                authRequired={feature.authRequired}
                isAuthenticated={isAuthenticated}
              />
            </Grid>
          ))}
        </Grid>
      </Container>
    </Box>
  );
};
