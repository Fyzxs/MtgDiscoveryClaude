import React, { useMemo, useRef, useState, useCallback, type KeyboardEvent } from 'react';
import { useQuery } from '@apollo/client/react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, IconButton, Button, Skeleton } from '../../../atoms';
import { NavigateBeforeIcon, NavigateNextIcon } from '../../../atoms';
import { SetPreviewCard, SET_PREVIEW_CARD_WIDTH } from '../components/SetPreviewCard';
import { GET_ALL_SETS } from '../../../../graphql/queries/sets';

/** Maximum number of featured sets to display */
const MAX_FEATURED_SETS = 12;

/** Gap between cards in pixels (matches theme spacing 2 = 16px) */
const CARD_GAP = 16;

/**
 * Featured sets carousel section.
 * Displays the latest expansion and core sets with horizontal scrolling.
 * Supports keyboard navigation (arrow keys, Home, End).
 */
export const FeaturedSetsCarousel: React.FC = () => {
  const navigate = useNavigate();
  const scrollContainerRef = useRef<HTMLDivElement>(null);
  const [isHovering, setIsHovering] = useState(false);

  const { data, loading, error } = useQuery(GET_ALL_SETS, {
    variables: { args: null },
  });

  const featuredSets = useMemo(() => {
    if (data?.allSets?.__typename !== 'SetsSuccessResponse') {
      return [];
    }

    const sets = data.allSets.data || [];

    return sets
      .filter(
        (set: { setType: string }) =>
          set.setType === 'expansion' || set.setType === 'core'
      )
      .sort((a: { releasedAt: string }, b: { releasedAt: string }) => {
        return new Date(b.releasedAt).getTime() - new Date(a.releasedAt).getTime();
      })
      .slice(0, MAX_FEATURED_SETS);
  }, [data]);

  const scrollAmount = SET_PREVIEW_CARD_WIDTH + CARD_GAP;

  const handleScroll = useCallback((direction: 'left' | 'right') => {
    if (scrollContainerRef.current === null) {
      return;
    }
    const amount = direction === 'left' ? -scrollAmount : scrollAmount;
    scrollContainerRef.current.scrollBy({ left: amount, behavior: 'smooth' });
  }, [scrollAmount]);

  const handleKeyDown = useCallback((event: KeyboardEvent<HTMLDivElement>) => {
    if (scrollContainerRef.current === null) {
      return;
    }

    if (event.key === 'ArrowLeft') {
      event.preventDefault();
      handleScroll('left');
    }

    if (event.key === 'ArrowRight') {
      event.preventDefault();
      handleScroll('right');
    }

    if (event.key === 'Home') {
      event.preventDefault();
      scrollContainerRef.current.scrollTo({ left: 0, behavior: 'smooth' });
    }

    if (event.key === 'End') {
      event.preventDefault();
      scrollContainerRef.current.scrollTo({
        left: scrollContainerRef.current.scrollWidth,
        behavior: 'smooth',
      });
    }
  }, [handleScroll]);

  const handleSetClick = useCallback((code: string) => {
    navigate(`/set/${code}`);
  }, [navigate]);

  const handleViewAll = useCallback(() => {
    navigate('/sets');
  }, [navigate]);

  if (loading) {
    return (
      <Box
        component="section"
        aria-labelledby="sets-heading"
        sx={{
          maxWidth: 1200,
          mx: 'auto',
          px: { xs: 2, md: 4 },
          py: { xs: 2, md: 4 },
        }}
      >
        <Box
          sx={{
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
            mb: 3,
          }}
        >
          <Typography id="sets-heading" variant="h5" sx={{ fontWeight: 600 }}>
            Latest Releases
          </Typography>
        </Box>

        <Box sx={{ display: 'flex', gap: 2, overflowX: 'auto' }}>
          {[...Array(4)].map((_, index) => (
            <Skeleton
              key={index}
              variant="rectangular"
              width={SET_PREVIEW_CARD_WIDTH}
              height={SET_PREVIEW_CARD_WIDTH}
              sx={{ borderRadius: 2, flexShrink: 0 }}
            />
          ))}
        </Box>
      </Box>
    );
  }

  if (error !== undefined || featuredSets.length === 0) {
    return null;
  }

  return (
    <Box
      component="section"
      aria-labelledby="sets-heading"
      sx={{
        maxWidth: 1200,
        mx: 'auto',
        px: { xs: 2, md: 4 },
        py: { xs: 2, md: 4 },
      }}
    >
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          mb: 3,
        }}
      >
        <Typography id="sets-heading" variant="h5" sx={{ fontWeight: 600 }}>
          Latest Releases
        </Typography>

        <Button
          variant="text"
          onClick={handleViewAll}
          sx={{
            color: 'primary.main',
            textDecoration: 'none',
            fontWeight: 500,
            fontSize: '0.875rem',
            '&:hover': {
              textDecoration: 'underline',
              bgcolor: 'transparent',
            },
          }}
        >
          See All
        </Button>
      </Box>

      <Box
        sx={{ position: 'relative' }}
        onMouseEnter={() => setIsHovering(true)}
        onMouseLeave={() => setIsHovering(false)}
      >
        <Box
          ref={scrollContainerRef}
          role="region"
          aria-label="Featured sets carousel"
          aria-roledescription="carousel"
          tabIndex={0}
          onKeyDown={handleKeyDown}
          sx={{
            display: 'flex',
            overflowX: 'auto',
            scrollSnapType: 'x mandatory',
            gap: 2,
            scrollbarWidth: 'none',
            '&::-webkit-scrollbar': {
              display: 'none',
            },
          }}
        >
          {featuredSets.map((set: {
            code: string;
            name: string;
            cardCount: number;
            releasedAt: string;
            iconSvgUri: string;
          }) => (
            <Box key={set.code} sx={{ scrollSnapAlign: 'start' }}>
              <SetPreviewCard
                code={set.code}
                name={set.name}
                cardCount={set.cardCount}
                releasedAt={set.releasedAt}
                iconSvgUri={set.iconSvgUri}
                onClick={handleSetClick}
              />
            </Box>
          ))}
        </Box>

        <IconButton
          aria-label="Previous sets"
          onClick={() => handleScroll('left')}
          sx={{
            position: 'absolute',
            left: -20,
            top: '50%',
            transform: 'translateY(-50%)',
            bgcolor: 'background.paper',
            boxShadow: 2,
            opacity: isHovering ? 1 : 0,
            transition: 'opacity 0.3s',
            display: { xs: 'none', md: 'flex' },
            '&:hover': {
              bgcolor: 'background.paper',
              boxShadow: 4,
            },
          }}
        >
          <NavigateBeforeIcon />
        </IconButton>

        <IconButton
          aria-label="Next sets"
          onClick={() => handleScroll('right')}
          sx={{
            position: 'absolute',
            right: -20,
            top: '50%',
            transform: 'translateY(-50%)',
            bgcolor: 'background.paper',
            boxShadow: 2,
            opacity: isHovering ? 1 : 0,
            transition: 'opacity 0.3s',
            display: { xs: 'none', md: 'flex' },
            '&:hover': {
              bgcolor: 'background.paper',
              boxShadow: 4,
            },
          }}
        >
          <NavigateNextIcon />
        </IconButton>
      </Box>
    </Box>
  );
};
