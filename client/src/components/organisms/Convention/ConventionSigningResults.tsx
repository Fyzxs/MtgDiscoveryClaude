import React, { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { useLazyQuery } from '@apollo/client/react';
import { COLLECTION_PARAM_NAME } from '../../../hooks/useCollectionParam';
import {
  Box,
  Typography,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Chip,
  Grid,
  Link,
  CircularProgress
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import PersonIcon from '@mui/icons-material/Person';
import StyleIcon from '@mui/icons-material/Style';
import OpenInNewIcon from '@mui/icons-material/OpenInNew';
import { GET_CARDS_BY_IDS } from '../../../graphql/queries/cards';
import { MtgCard } from '../Cards/MtgCard';
import type { Card, CardContext } from '../../../types/card';

interface CollectedDetail {
  finish: string;
  special: string;
  count: number;
}

interface SigningCard {
  cardId: string;
  cardName: string;
  imageUri: string;
  unsignedCopies: number;
  isPartiallySigned: boolean;
  isFullySigned: boolean;
  collectedDetails: CollectedDetail[];
}

interface SigningArtistGroup {
  artistId: string;
  artistName: string;
  unsignedCount: number;
  partiallySignedCount: number;
  cards: SigningCard[];
}

interface SigningSetGroup {
  setId: string;
  setCode: string;
  setName: string;
  releasedAt: string;
  artistCount: number;
  unsignedCardCount: number;
  artists: SigningArtistGroup[];
}

interface SigningResult {
  sets: SigningSetGroup[];
}

interface ConventionSigningResultsProps {
  data: SigningResult;
  collectorId: string | null;
}

interface CardsByIdsResponse {
  cardsById: {
    __typename: string;
    data?: Card[];
    status?: {
      message: string;
    };
  };
}

interface LoadedCardData {
  cards: Card[];
  loading: boolean;
  error: string | null;
}

/**
 * Build a URL to the set page with artist filtering pre-applied and collector preserved
 * Artist param format: artists=Amy+Weber%2CChristopher+Rush (comma-separated, URL encoded)
 */
const buildSetUrl = (setGroup: SigningSetGroup, collectorId: string | null): string => {
  const baseUrl = `/set/${setGroup.setCode.toLowerCase()}`;
  const artistNames = setGroup.artists
    .map(a => a.artistName)
    .filter(name => name && name.length > 0);

  const params = new URLSearchParams();

  // Preserve collection parameter
  if (collectorId) {
    params.set(COLLECTION_PARAM_NAME, collectorId);
  }

  // Add artist filters as comma-separated value
  if (artistNames.length > 0) {
    params.set('artists', artistNames.join(','));
  }

  // Filter to only show cards in collection (counts 1, 2, 3, 4+)
  params.set('counts', '1,2,3,4plus');

  // Filter to paper format only
  params.set('formats', 'paper');

  const queryString = params.toString();
  return queryString ? `${baseUrl}?${queryString}` : baseUrl;
};

// Track pending fetch requests
interface PendingFetch {
  artistKey: string;
  cardIds: string[];
}

/**
 * ConventionSigningResults - Display signing results grouped by Set -> Artist -> Cards
 * Cards are lazy-loaded when an artist accordion is expanded
 */
export const ConventionSigningResults: React.FC<ConventionSigningResultsProps> = ({ data, collectorId }) => {
  const [expandedSets, setExpandedSets] = useState<Set<string>>(new Set());
  const [expandedArtists, setExpandedArtists] = useState<Set<string>>(new Set());
  const [loadedCards, setLoadedCards] = useState<Map<string, LoadedCardData>>(new Map());
  const [pendingFetch, setPendingFetch] = useState<PendingFetch | null>(null);
  const fetchingRef = useRef<Set<string>>(new Set());
  const navigate = useNavigate();

  // Lazy query for fetching card details
  const [fetchCards] = useLazyQuery<CardsByIdsResponse>(GET_CARDS_BY_IDS, {
    fetchPolicy: 'cache-first'
  });

  // Effect to handle pending fetches outside of render
  useEffect(() => {
    if (pendingFetch === null) {
      return;
    }

    const { artistKey, cardIds } = pendingFetch;

    // Prevent duplicate fetches
    if (fetchingRef.current.has(artistKey)) {
      setPendingFetch(null);
      return;
    }

    fetchingRef.current.add(artistKey);
    setPendingFetch(null);

    // Mark as loading
    setLoadedCards(current => {
      const updated = new Map(current);
      updated.set(artistKey, { cards: [], loading: true, error: null });
      return updated;
    });

    fetchCards({
      variables: {
        ids: {
          cardIds,
          ...(collectorId ? { userId: collectorId } : {})
        }
      }
    }).then(result => {
      if (result.data?.cardsById.__typename === 'CardsSuccessResponse') {
        setLoadedCards(current => {
          const updated = new Map(current);
          updated.set(artistKey, {
            cards: result.data?.cardsById.data || [],
            loading: false,
            error: null
          });
          return updated;
        });
      } else {
        setLoadedCards(current => {
          const updated = new Map(current);
          updated.set(artistKey, {
            cards: [],
            loading: false,
            error: result.data?.cardsById.status?.message || 'Failed to load cards'
          });
          return updated;
        });
      }
    }).catch(err => {
      setLoadedCards(current => {
        const updated = new Map(current);
        updated.set(artistKey, {
          cards: [],
          loading: false,
          error: err.message || 'Failed to load cards'
        });
        return updated;
      });
    });
  }, [pendingFetch, fetchCards, collectorId]);

  const handleSetToggle = (setId: string) => {
    setExpandedSets(prev => {
      const next = new Set(prev);
      if (next.has(setId)) {
        next.delete(setId);
      } else {
        next.add(setId);
      }
      return next;
    });
  };

  const handleArtistToggle = (artistKey: string, cardIds: string[]) => {
    setExpandedArtists(prev => {
      const next = new Set(prev);
      if (next.has(artistKey)) {
        next.delete(artistKey);
      } else {
        next.add(artistKey);
      }
      return next;
    });

    // If expanding and haven't loaded yet, queue a fetch
    if (!expandedArtists.has(artistKey) && !loadedCards.has(artistKey) && !fetchingRef.current.has(artistKey)) {
      setPendingFetch({ artistKey, cardIds });
    }
  };

  if (data.sets.length === 0) {
    return (
      <Box sx={{ textAlign: 'center', py: 8 }}>
        <Typography variant="h6" color="text.secondary">
          No cards to sign
        </Typography>
        <Typography variant="body2" color="text.secondary">
          You don't have any unsigned cards from the selected artists.
        </Typography>
      </Box>
    );
  }

  // Context for card display - we're on a signing page, not a set/artist page
  const cardContext: CardContext = {
    isOnSetPage: false,
    isOnArtistPage: false,
    showCollectorInfo: true,
    hasCollector: Boolean(collectorId)
  };

  return (
    <Box sx={{ width: '100%' }}>
      {data.sets.map((setGroup) => (
        <Accordion
          key={setGroup.setId}
          expanded={expandedSets.has(setGroup.setId)}
          onChange={() => handleSetToggle(setGroup.setId)}
          sx={{
            bgcolor: 'grey.900',
            '&:before': { display: 'none' },
            mb: 1,
            borderRadius: 1,
            '&.Mui-expanded': {
              mb: 1,
              borderRadius: 1
            }
          }}
        >
          <AccordionSummary
            expandIcon={<ExpandMoreIcon />}
            sx={{
              '& .MuiAccordionSummary-content': {
                display: 'flex',
                alignItems: 'center',
                gap: 2,
                flexWrap: 'wrap'
              }
            }}
          >
            <Box sx={{ flexGrow: 1, display: 'flex', alignItems: 'center', gap: 1 }}>
              <Link
                component="a"
                href={buildSetUrl(setGroup, collectorId)}
                onClick={(e: React.MouseEvent) => {
                  e.stopPropagation();
                  e.preventDefault();
                  navigate(buildSetUrl(setGroup, collectorId));
                }}
                sx={{
                  color: 'primary.main',
                  textDecoration: 'none',
                  cursor: 'pointer',
                  '&:hover': {
                    textDecoration: 'underline'
                  }
                }}
              >
                <Typography variant="h6" component="span">
                  {setGroup.setName || setGroup.setCode.toUpperCase()}
                </Typography>
              </Link>
              <OpenInNewIcon sx={{ fontSize: 16, color: 'text.secondary' }} />
              {setGroup.releasedAt && (
                <Typography variant="caption" color="text.secondary" sx={{ ml: 1 }}>
                  ({setGroup.releasedAt})
                </Typography>
              )}
            </Box>
            <Chip
              icon={<PersonIcon />}
              label={`${setGroup.artistCount} artist${setGroup.artistCount !== 1 ? 's' : ''}`}
              size="small"
              variant="outlined"
            />
            <Chip
              icon={<StyleIcon />}
              label={`${setGroup.unsignedCardCount} unsigned`}
              size="small"
              color="primary"
            />
          </AccordionSummary>

          <AccordionDetails sx={{ pt: 0 }}>
            {setGroup.artists.map((artistGroup) => {
              const artistKey = `${setGroup.setId}-${artistGroup.artistId}`;
              const cardIds = artistGroup.cards.map(c => c.cardId);
              const isExpanded = expandedArtists.has(artistKey);
              const cardData = loadedCards.get(artistKey);

              return (
                <Accordion
                  key={artistKey}
                  expanded={isExpanded}
                  onChange={() => handleArtistToggle(artistKey, cardIds)}
                  sx={{
                    bgcolor: 'grey.800',
                    '&:before': { display: 'none' },
                    mb: 1,
                    borderRadius: 1
                  }}
                >
                  <AccordionSummary
                    expandIcon={<ExpandMoreIcon />}
                    sx={{
                      '& .MuiAccordionSummary-content': {
                        display: 'flex',
                        alignItems: 'center',
                        gap: 2,
                        flexWrap: 'wrap'
                      }
                    }}
                  >
                    <Typography sx={{ flexGrow: 1 }}>
                      {artistGroup.artistName || 'Unknown Artist'}
                    </Typography>
                    <Chip
                      label={`${artistGroup.unsignedCount} cards`}
                      size="small"
                      variant="outlined"
                    />
                    {artistGroup.partiallySignedCount > 0 && (
                      <Chip
                        label={`${artistGroup.partiallySignedCount} partially signed`}
                        size="small"
                        color="warning"
                        variant="outlined"
                      />
                    )}
                  </AccordionSummary>

                  <AccordionDetails>
                    {cardData?.loading ? (
                      <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
                        <CircularProgress size={32} />
                      </Box>
                    ) : cardData?.error ? (
                      <Box sx={{ textAlign: 'center', py: 4 }}>
                        <Typography color="error">{cardData.error}</Typography>
                      </Box>
                    ) : cardData?.cards && cardData.cards.length > 0 ? (
                      <Grid container spacing={2}>
                        {cardData.cards.map((card, index) => {
                          // Find the signing card data to show status
                          const signingCard = artistGroup.cards.find(c => c.cardId === card.id);
                          const isFullySigned = signingCard?.isFullySigned ?? false;

                          return (
                            <Grid key={card.id} size={{ xs: 6, sm: 4, md: 3, lg: 2 }}>
                              <Box sx={{ position: 'relative' }}>
                                <MtgCard
                                  card={card}
                                  context={cardContext}
                                  index={index}
                                  groupId={artistKey}
                                />
                                {/* Green overlay for fully signed cards */}
                                {isFullySigned && (
                                  <Box
                                    sx={{
                                      position: 'absolute',
                                      top: 0,
                                      left: 0,
                                      right: 0,
                                      bottom: 0,
                                      bgcolor: 'success.main',
                                      opacity: 0.4,
                                      borderRadius: 2,
                                      pointerEvents: 'none',
                                      display: 'flex',
                                      alignItems: 'center',
                                      justifyContent: 'center'
                                    }}
                                  >
                                    <Typography
                                      variant="h6"
                                      sx={{
                                        color: 'white',
                                        fontWeight: 'bold',
                                        textShadow: '0 2px 4px rgba(0,0,0,0.5)',
                                        opacity: 1
                                      }}
                                    >
                                      SIGNED
                                    </Typography>
                                  </Box>
                                )}
                              </Box>
                            </Grid>
                          );
                        })}
                      </Grid>
                    ) : (
                      <Box sx={{ textAlign: 'center', py: 4 }}>
                        <Typography color="text.secondary">
                          Click to load card details...
                        </Typography>
                      </Box>
                    )}
                  </AccordionDetails>
                </Accordion>
              );
            })}
          </AccordionDetails>
        </Accordion>
      ))}
    </Box>
  );
};
