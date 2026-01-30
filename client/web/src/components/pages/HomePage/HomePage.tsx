import React from 'react';
import { Box } from '../../atoms';
import { useHomePageData } from '../../../hooks/useHomePageData';
import { HeroSection } from './sections/HeroSection';
import { FeatureHighlights } from './sections/FeatureHighlights';
import { FeaturedSetsCarousel } from './sections/FeaturedSetsCarousel';
import { CollectionPreview } from './sections/CollectionPreview';
import { ArtistSpotlight } from './sections/ArtistSpotlight';
import { BottomCTA } from './sections/BottomCTA';
import { FadeInSection } from './components/FadeInSection';

export const HomePage: React.FC = () => {
  const {
    stats,
    statsError,
    recentSets,
    wishlistCards,
    isLoading,
    isAuthenticated,
    userName,
  } = useHomePageData();

  return (
    <Box>
      <HeroSection
        isAuthenticated={isAuthenticated}
        userName={userName}
        stats={stats}
        statsError={statsError}
      />

      <FadeInSection>
        <FeatureHighlights isAuthenticated={isAuthenticated} />
      </FadeInSection>

      <FadeInSection>
        <FeaturedSetsCarousel />
      </FadeInSection>

      <FadeInSection>
        <CollectionPreview
          isAuthenticated={isAuthenticated}
          recentSets={recentSets}
          wishlistCards={wishlistCards}
          isLoading={isLoading}
        />
      </FadeInSection>

      <FadeInSection>
        <ArtistSpotlight />
      </FadeInSection>

      <FadeInSection>
        <BottomCTA isAuthenticated={isAuthenticated} />
      </FadeInSection>
    </Box>
  );
};

export default HomePage;
