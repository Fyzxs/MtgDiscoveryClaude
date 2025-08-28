import React from 'react';
import {
  Box,
  Typography,
  IconButton,
  Divider,
  Link,
  Grid,
  Stack,
  Button,
  Tooltip
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { getLegalityColor } from '../../theme';
import { flexBetween, flexCenter, flexCol, gap2, gap3 } from '../../styles/layoutStyles';
import { ModalErrorBoundary } from '../ErrorBoundaries';
import NavigateBeforeIcon from '@mui/icons-material/NavigateBefore';
import NavigateNextIcon from '@mui/icons-material/NavigateNext';
import OpenInNewIcon from '@mui/icons-material/OpenInNew';
import CircleIcon from '@mui/icons-material/Circle';
import CircleOutlinedIcon from '@mui/icons-material/CircleOutlined';
import RemoveCircleIcon from '@mui/icons-material/RemoveCircle';
import WarningIcon from '@mui/icons-material/Warning';
import HelpOutlineIcon from '@mui/icons-material/HelpOutline';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import CloseIcon from '@mui/icons-material/Close';
import type { Card } from '../../types/card';
import { ModalContainer } from '../molecules/shared/ModalContainer';
import { ManaCost } from '../molecules/Cards/ManaCost';
import { RarityBadge } from '../atoms/Cards/RarityBadge';
import { PriceDisplay } from '../atoms/shared/PriceDisplay';
import { RelatedCardsDisplay } from '../molecules/Cards/RelatedCardsDisplay';
import { AllPrintingsDisplay } from '../molecules/Cards/AllPrintingsDisplay';
import { RulingsDisplay } from '../molecules/Cards/RulingsDisplay';
import { CardImageDisplay } from '../molecules/Cards/CardImageDisplay';
import { CardBadges } from '../atoms/Cards/CardBadges';

interface CardDetailsModalProps {
  open: boolean;
  onClose: () => void;
  card?: Card;
  onPrevious?: () => void;
  onNext?: () => void;
  hasPrevious?: boolean;
  hasNext?: boolean;
}


const LEGALITY_ICONS: Record<string, React.ReactNode> = {
  legal: <CircleIcon sx={{ fontSize: 16 }} />,
  not_legal: <CircleOutlinedIcon sx={{ fontSize: 16 }} />,
  restricted: <WarningIcon sx={{ fontSize: 16 }} />,
  banned: <RemoveCircleIcon sx={{ fontSize: 16 }} />
};

const LEGALITY_DESCRIPTIONS: Record<string, string> = {
  legal: 'Legal',
  not_legal: 'Not legal',
  restricted: 'Restricted (only allowed one copy)',
  banned: 'Banned'
};

// Function to generate Card Kingdom search URL
const generateCardKingdomSearchUrl = (cardName: string): string => {
  const params = new URLSearchParams({
    'search': 'mtg_advanced',
    'filter[search]': 'mtg_advanced',
    'filter[tab]': 'mtg_card',
    'filter[name]': cardName
  });
  
  return `https://www.cardkingdom.com/catalog/search?${params.toString()}`;
};

const FORMAT_DISPLAY_NAMES: Record<string, string> = {
  standard: 'Standard',
  future: 'Future',
  historic: 'Historic',
  timeless: 'Timeless',
  gladiator: 'Gladiator',
  pioneer: 'Pioneer',
  explorer: 'Explorer',
  modern: 'Modern',
  legacy: 'Legacy',
  pauper: 'Pauper',
  vintage: 'Vintage',
  penny: 'Penny',
  commander: 'Commander',
  oathbreaker: 'Oathbreaker',
  standardBrawl: 'Standard Brawl',
  brawl: 'Brawl',
  alchemy: 'Alchemy',
  pauperCommander: 'Pauper EDH',
  duel: 'Duel',
  oldschool: 'Oldschool',
  premodern: 'Premodern',
  predh: 'PreDH'
};

export const CardDetailsModal: React.FC<CardDetailsModalProps> = ({
  open,
  onClose,
  card,
  onPrevious,
  onNext,
  hasPrevious,
  hasNext
}) => {
  if (!card) return null;

  const formatOracleText = (text?: string) => {
    if (!text) return null;

    return text.split('\n').map((line, index) => (
      <Typography key={index} variant="body1" paragraph component="div">
        {line.split(/(\{[^}]+\})/).map((part, i) => {
          if (part.match(/^\{.*\}$/)) {
            return (
              <Box key={i} component="span" sx={{ display: 'inline-flex', verticalAlign: 'middle' }}>
                <ManaCost manaCost={part} />
              </Box>
            );
          }
          return <span key={i}>{part}</span>;
        })}
      </Typography>
    ));
  };

  return (
    <ModalContainer
      open={open}
      onClose={onClose}
      width="90vw"
      maxWidth={1400}
      height="90vh"
      showCloseButton={false}
    >
      <ModalErrorBoundary name="CardDetails" onClose={onClose}>
        {/* Header */}
        <Box sx={{
          p: 2,
          borderBottom: 1,
          borderColor: 'divider',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between'
        }}>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            {(onPrevious || onNext) && (
              <Box sx={{ display: 'flex', gap: 1 }}>
                <IconButton
                  onClick={onPrevious}
                  disabled={!hasPrevious}
                  size="small"
                >
                  <NavigateBeforeIcon />
                </IconButton>
                <IconButton
                  onClick={onNext}
                  disabled={!hasNext}
                  size="small"
                >
                  <NavigateNextIcon />
                </IconButton>
              </Box>
            )}
            <Typography variant="h5" component="h2">
              {card.name}
            </Typography>
            {card.manaCost && <ManaCost manaCost={card.manaCost} />}
          </Box>
          <IconButton onClick={onClose}>
            <CloseIcon />
          </IconButton>
        </Box>

        {/* Content */}
        <Box sx={{
          display: 'flex',
          flex: 1,
          overflow: 'hidden'
        }}>
          {/* Left side - Card Image */}
          <Box sx={{
            width: '45%',
            p: 3,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            bgcolor: 'grey.900',
            borderRadius: '0 0 0 8px'
          }}>
            <Box sx={{ 
              position: 'relative',
              width: '100%',
              maxWidth: '600px',
              aspectRatio: '745 / 1040'
            }}>
              <CardImageDisplay
                card={card}
                size="large"
                borderRadius="6.75%"
                sx={{
                  boxShadow: 3,
                  width: '100%',
                  height: '100%'
                }}
              />
              <CardBadges 
                finishes={card.finishes}
                promoTypes={card.promoTypes}
                frameEffects={card.frameEffects}
                isPromo={card.promo}
              />
            </Box>
            {/* Card ID */}
            <Box sx={{ 
              mt: 2, 
              display: 'flex', 
              alignItems: 'center', 
              gap: 1,
              bgcolor: 'background.paper',
              borderRadius: 1,
              px: 2,
              py: 1
            }}>
              <Typography variant="caption" sx={{ fontFamily: 'monospace', color: 'text.secondary' }}>
                {card.id}
              </Typography>
              <Tooltip title="Copy ID">
                <IconButton 
                  size="small"
                  onClick={() => {
                    navigator.clipboard.writeText(card.id);
                  }}
                  sx={{ p: 0.5 }}
                >
                  <ContentCopyIcon fontSize="small" />
                </IconButton>
              </Tooltip>
            </Box>
          </Box>

          {/* Right side - Card Details */}
          <Box sx={{
            flex: 1,
            overflow: 'auto',
            p: 3
          }}>
            <Stack spacing={3}>
              {/* Basic Info */}
              <Box>
                <Typography variant="h6" gutterBottom>
                  {card.typeLine}
                </Typography>
                <Box sx={{ display: 'flex', gap: 2, alignItems: 'center', flexWrap: 'wrap' }}>
                  <RarityBadge rarity={card.rarity} />
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                    <Link 
                      href={`/set/${card.setCode}`}
                      sx={{ 
                        color: 'text.secondary',
                        textDecoration: 'none',
                        '&:hover': {
                          textDecoration: 'underline'
                        }
                      }}
                    >
                      {card.setName}
                    </Link>
                    <Typography variant="body2" color="text.secondary">
                      ({card.setCode?.toUpperCase()}) · #{card.collectorNumber}
                    </Typography>
                  </Box>
                </Box>
              </Box>

              {/* Oracle Text */}
              {card.oracleText && (
                <Box>
                  <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
                    Oracle Text
                  </Typography>
                  <Box sx={{ pl: 2 }}>
                    {formatOracleText(card.oracleText)}
                  </Box>
                </Box>
              )}

              {/* Flavor Text */}
              {card.flavorText && (
                <Box>
                  <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
                    Flavor Text
                  </Typography>
                  <Typography variant="body2" fontStyle="italic" sx={{ pl: 2 }}>
                    {card.flavorText}
                  </Typography>
                </Box>
              )}

              {/* P/T, Loyalty, Defense */}
              {(card.power || card.loyalty || card.defense) && (
                <Box sx={{ display: 'flex', gap: 3, justifyContent: card.power ? 'flex-end' : 'flex-start' }}>
                  {card.power && (
                    <Typography variant="h6">
                      {card.power}/{card.toughness}
                    </Typography>
                  )}
                  {card.loyalty && (
                    <Typography variant="h6">
                      Loyalty: {card.loyalty}
                    </Typography>
                  )}
                  {card.defense && (
                    <Typography variant="h6">
                      Defense: {card.defense}
                    </Typography>
                  )}
                </Box>
              )}

              <Divider />

              {/* Artist */}
              {card.artist && (
                <Box>
                  <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
                    Artist
                  </Typography>
                  <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                    {card.artist.split(/\s+(?:&|and)\s+/i).map((artistName, index) => (
                      <Link
                        key={index}
                        href={`?page=artist&name=${encodeURIComponent(artistName)}`}
                        sx={{
                          color: 'text.primary',
                          textDecoration: 'none',
                          '&:hover': {
                            textDecoration: 'underline'
                          }
                        }}
                      >
                        {artistName}
                      </Link>
                    ))}
                  </Box>
                </Box>
              )}

              <Divider />

              {/* Legalities */}
              <Box>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
                  <Typography variant="subtitle1" fontWeight="bold">
                    Legalities
                  </Typography>
                  <Tooltip title={
                    <Box sx={{ p: 1 }}>
                      <Typography variant="subtitle2" fontWeight="bold" gutterBottom>
                        Legality Key
                      </Typography>
                      {Object.entries(LEGALITY_DESCRIPTIONS).map(([key, desc]) => (
                        <Box key={key} sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 0.5 }}>
                          <Box sx={{ color: getLegalityColor(key) }}>
                            {LEGALITY_ICONS[key]}
                          </Box>
                          <Typography variant="caption">{desc}</Typography>
                        </Box>
                      ))}
                    </Box>
                  }>
                    <HelpOutlineIcon sx={{ fontSize: 18, color: 'text.secondary', cursor: 'help' }} />
                  </Tooltip>
                </Box>

                <Grid container spacing={1.5}>
                  {Object.entries(card.legalities || {})
                    .filter(([format]) => format !== '__typename')
                    .sort((a, b) => {
                      const order = ['standard', 'pioneer', 'modern', 'legacy', 'vintage', 'commander'];
                      const aIndex = order.indexOf(a[0]);
                      const bIndex = order.indexOf(b[0]);
                      if (aIndex !== -1 && bIndex !== -1) return aIndex - bIndex;
                      if (aIndex !== -1) return -1;
                      if (bIndex !== -1) return 1;
                      return a[0].localeCompare(b[0]);
                    })
                    .map(([format, legality]) => {
                      const isLegal = legality === 'legal';
                      const displayName = FORMAT_DISPLAY_NAMES[format] || format;

                      return (
                        <Grid key={format}>
                          <Tooltip title={`${displayName}: ${LEGALITY_DESCRIPTIONS[legality] || legality}`}>
                            <Box
                              sx={{
                                display: 'flex',
                                alignItems: 'center',
                                gap: 0.5,
                                cursor: 'help',
                                opacity: isLegal ? 1 : 0.7,
                                '&:hover': {
                                  opacity: 1
                                }
                              }}
                            >
                              <Box sx={{ color: getLegalityColor(legality) }}>
                                {LEGALITY_ICONS[legality] || LEGALITY_ICONS.not_legal}
                              </Box>
                              <Typography
                                variant="body2"
                                sx={{
                                  color: isLegal ? 'text.primary' : 'text.secondary',
                                  fontWeight: isLegal ? 500 : 400
                                }}
                              >
                                {displayName}
                              </Typography>
                            </Box>
                          </Tooltip>
                        </Grid>
                      );
                    })}
                </Grid>
              </Box>

              {/* Prices - only show if we have at least one price */}
              {card.prices && (card.prices.usd || card.prices.usdFoil || card.prices.eur || card.prices.eurFoil) && (
                <>
                  <Divider />
                  <Box>
                    <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
                      Market Prices
                    </Typography>
                    <Grid container spacing={2}>
                      {card.prices.usd && (
                        <Grid size={{ xs: 6, sm: 4 }}>
                          <PriceDisplay
                            price={card.prices.usd}
                            currency="USD"
                            label="Normal"
                          />
                        </Grid>
                      )}
                      {card.prices.usdFoil && (
                        <Grid size={{ xs: 6, sm: 4 }}>
                          <PriceDisplay
                            price={card.prices.usdFoil}
                            currency="USD"
                            label="Foil"
                          />
                        </Grid>
                      )}
                      {card.prices.eur && (
                        <Grid size={{ xs: 6, sm: 4 }}>
                          <PriceDisplay
                            price={card.prices.eur}
                            currency="EUR"
                            label="Normal"
                          />
                        </Grid>
                      )}
                    </Grid>
                  </Box>
                </>
              )}

              <Divider />

              {/* External Links */}
              <Box>
                <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
                  External Links
                </Typography>
                <Stack direction="row" spacing={2} flexWrap="wrap">
                  {card.purchaseUris?.tcgplayer && (
                    <Button
                      size="small"
                      startIcon={<OpenInNewIcon />}
                      href={card.purchaseUris.tcgplayer}
                      target="_blank"
                    >
                      TCGPlayer
                    </Button>
                  )}
                  {card.purchaseUris?.cardmarket && (
                    <Button
                      size="small"
                      startIcon={<OpenInNewIcon />}
                      href={card.purchaseUris.cardmarket}
                      target="_blank"
                    >
                      Cardmarket
                    </Button>
                  )}
                  {card.purchaseUris?.cardhoarder && (
                    <Button
                      size="small"
                      startIcon={<OpenInNewIcon />}
                      href={card.purchaseUris.cardhoarder}
                      target="_blank"
                    >
                      Cardhoarder
                    </Button>
                  )}
                  {card.relatedUris?.gatherer && (
                    <Button
                      size="small"
                      startIcon={<OpenInNewIcon />}
                      href={card.relatedUris.gatherer}
                      target="_blank"
                    >
                      Gatherer
                    </Button>
                  )}
                  {card.relatedUris?.edhrec && (
                    <Button
                      size="small"
                      startIcon={<OpenInNewIcon />}
                      href={card.relatedUris.edhrec}
                      target="_blank"
                    >
                      EDHREC
                    </Button>
                  )}
                  {card.scryfallUri && (
                    <Button
                      size="small"
                      startIcon={<OpenInNewIcon />}
                      href={card.scryfallUri}
                      target="_blank"
                    >
                      Scryfall
                    </Button>
                  )}
                  {card.name && (
                    <Button
                      size="small"
                      startIcon={<OpenInNewIcon />}
                      href={generateCardKingdomSearchUrl(card.name)}
                      target="_blank"
                    >
                      Card Kingdom
                    </Button>
                  )}
                </Stack>
              </Box>

              {/* Rulings */}
              {card.rulingsUri && (
                <>
                  <RulingsDisplay rulingsUri={card.rulingsUri} />
                  <Divider sx={{ mt: 2 }} />
                </>
              )}

              {/* Related Cards */}
              {card.allParts && card.allParts.length > 1 && (
                <>
                  <Box>
                    <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
                      Related Cards
                    </Typography>
                    <RelatedCardsDisplay 
                      relatedCardIds={card.allParts.map(part => part.id)}
                      currentCardId={card.id}
                    />
                  </Box>
                  <Divider sx={{ mt: 2 }} />
                </>
              )}

              {/* Other Printings */}
              <AllPrintingsDisplay cardName={card.name} currentCardId={card.id} />
            </Stack>
          </Box>
        </Box>
      </ModalErrorBoundary>
    </ModalContainer>
  );
};