import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useQuery } from '@apollo/client/react';
import {
  Container,
  Typography,
  Box,
  Button,
  CircularProgress
} from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { ResultsSummary } from '../components/molecules/shared/ResultsSummary';
import { GET_CARDS_BY_NAME } from '../graphql/queries/cards';
import { MtgCard } from '../components/organisms/MtgCard';
import { ResponsiveGridAutoFit } from '../components/atoms/layouts/ResponsiveGrid';
import { useCardFiltering } from '../hooks/useCardFiltering';
import { CardFilterPanel } from '../components/molecules/shared/CardFilterPanel';
import { CARD_DETAIL_SORT_OPTIONS } from '../config/cardSortOptions';


interface CardData {
  id: string;
  name: string;
  setCode: string;
  setName: string;
  releasedAt: string;
  collectorNumber: string;
  rarity: string;
  artist: string;
  digital?: boolean;
  prices: {
    usd: string | null;
    usdFoil: string | null;
  };
  imageUris?: {
    normal: string;
    large: string;
    small: string;
  };
  cardFaces?: Array<{
    imageUris?: {
      normal: string;
      large: string;
      small: string;
    };
  }>;
}

interface CardsResponse {
  cardsByName: {
    __typename: string;
    data?: CardData[];
    status?: {
      message: string;
    };
  };
}

export const CardDetailPage: React.FC = () => {
  const { cardName } = useParams<{ cardName: string }>();
  const navigate = useNavigate();
  const decodedCardName = decodeURIComponent(cardName || '');

  const { loading, error, data } = useQuery<CardsResponse>(GET_CARDS_BY_NAME, {
    variables: {
      cardName: {
        cardName: decodedCardName
      }
    },
    skip: !cardName
  });

  const cards = data?.cardsByName?.data || [];
  const hasError = error || data?.cardsByName?.__typename === 'FailureResponse';
  const errorMessage = error?.message || data?.cardsByName?.status?.message;

  const handleBackClick = () => {
    navigate('/search/cards');
  };

  // Use the shared card filtering hook (no search or sets filter for card detail page)
  const {
    filteredCards,
    totalCards,
    sortBy,
    filters,
    setSortBy,
    updateFilter,
    uniqueArtists,
    uniqueRarities,
    hasMultipleArtists,
    hasMultipleRarities
  } = useCardFiltering(cards, {
    defaultSort: 'release-desc',
    includeSets: false
  });

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
          <CircularProgress />
        </Box>
      </Container>
    );
  }

  if (hasError) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Button
          startIcon={<ArrowBackIcon />}
          onClick={handleBackClick}
          sx={{ mb: 2 }}
        >
          Back to Search
        </Button>
        <Typography color="error">
          {errorMessage || 'Failed to load card details'}
        </Typography>
      </Container>
    );
  }

  if (cards.length === 0) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Button
          startIcon={<ArrowBackIcon />}
          onClick={handleBackClick}
          sx={{ mb: 2 }}
        >
          Back to Search
        </Button>
        <Typography>No cards found with name "{decodedCardName}"</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Button
        startIcon={<ArrowBackIcon />}
        onClick={handleBackClick}
        sx={{ mb: 3 }}
      >
        Back to Search
      </Button>

      {/* Centered card name */}
      <Box sx={{ textAlign: 'center', mb: 4 }}>
        <Typography variant="h2" fontWeight="bold">
          {decodedCardName}
        </Typography>
      </Box>

      {/* Card Filter Panel - Centered controls */}
      <CardFilterPanel
        sortBy={sortBy}
        filters={filters}
        onSortChange={setSortBy}
        onFilterChange={updateFilter}
        uniqueArtists={uniqueArtists}
        uniqueRarities={uniqueRarities}
        hasMultipleArtists={hasMultipleArtists}
        hasMultipleRarities={hasMultipleRarities}
        filteredCount={filteredCards.length}
        totalCount={totalCards}
        showSearch={false}
        sortOptions={CARD_DETAIL_SORT_OPTIONS}
        centerControls={true}
      />

      {/* Results Summary */}
      <ResultsSummary 
        current={filteredCards.length}
        total={totalCards}
        label="printings"
        textAlign="center"
      />

      {/* Cards Grid */}
      <ResponsiveGridAutoFit 
        minWidth={250}
        maxColumns={6}
        gap={2}
      >
        {filteredCards.map((card) => (
          <MtgCard 
            key={card.id}
            card={card}
            context={{ isOnCardPage: true }}
            onSelection={() => {}}
            isSelected={false}
          />
        ))}
      </ResponsiveGridAutoFit>
    </Container>
  );
};