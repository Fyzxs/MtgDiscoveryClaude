import React, { useState } from 'react';
import {
  Box,
  Typography,
  IconButton,
  Divider,
  Stack,
  Button,
  Tooltip,
  Drawer
} from '../../atoms';
import { getLegalityColor } from '../../../theme';
import type { Card } from '../../../types/card';
import { useCollectorParam } from '../../../hooks/useCollectorParam';
import { CollectionSummary, ManaCost } from '../../molecules';
import { RarityBadge, PriceDisplay } from '../../atoms';
import { ReservedListShield } from '../../atoms/Cards/ReservedListShield';
import { CardImageDisplay } from './CardImageDisplay';
import { CardBadges } from '../../molecules/Cards/CardBadges';
import { SetLink } from '../../atoms';
import { ArtistLinks } from '../../molecules';
import {
  NavigateBeforeIcon,
  NavigateNextIcon,
  OpenInNewIcon,
  CircleIcon,
  CircleOutlinedIcon,
  RemoveCircleIcon,
  WarningIcon,
  CloseIcon,
  KeyboardArrowDownIcon
} from '../../atoms/Icons';
import { useTheme } from '../../atoms';

interface CardDetailsSheetProps {
  open: boolean;
  onClose: () => void;
  card?: Card;
  onPrevious?: () => void;
  onNext?: () => void;
  hasPrevious?: boolean;
  hasNext?: boolean;
}

const LEGALITY_ICONS: Record<string, React.ReactNode> = {
  legal: <CircleIcon sx={{ fontSize: 14 }} />,
  not_legal: <CircleOutlinedIcon sx={{ fontSize: 14 }} />,
  restricted: <WarningIcon sx={{ fontSize: 14 }} />,
  banned: <RemoveCircleIcon sx={{ fontSize: 14 }} />
};

const LEGALITY_DESCRIPTIONS: Record<string, string> = {
  legal: 'Legal',
  not_legal: 'Not legal',
  restricted: 'Restricted',
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

// Simplified format list for mobile
const MOBILE_FORMATS = ['standard', 'pioneer', 'modern', 'legacy', 'vintage', 'commander'];

const FORMAT_DISPLAY_NAMES: Record<string, string> = {
  standard: 'Std',
  pioneer: 'Pio',
  modern: 'Mod',
  legacy: 'Leg',
  vintage: 'Vin',
  commander: 'EDH'
};

/**
 * Mobile-optimized card details displayed as a bottom sheet.
 * Shows condensed information with expandable sections.
 */
export const CardDetailsSheet: React.FC<CardDetailsSheetProps> = ({
  open,
  onClose,
  card,
  onPrevious,
  onNext,
  hasPrevious,
  hasNext
}) => {
  const theme = useTheme();
  const { hasCollector } = useCollectorParam();
  const [showMoreLegalities, setShowMoreLegalities] = useState(false);

  if (!card) return null;

  // Check if there are any displayable treatments
  const hasDisplayableTreatments = (
    card.foil ||
    card.finishes?.includes('etched') ||
    (card.promoTypes && card.promoTypes.length > 0) ||
    (card.frameEffects && card.frameEffects.length > 0) ||
    card.digital
  );

  const formatOracleText = (text?: string) => {
    if (!text) return null;

    return text.split('\n').map((line, index) => (
      <Typography key={index} variant="body2" paragraph component="div" sx={{ mb: 1 }}>
        {line.split(/(\{[^}]+\})/).map((part, i) => {
          if (part.match(/^\{.*\}$/)) {
            return (
              <Box key={i} component="span" sx={{ display: 'inline-flex', verticalAlign: 'middle' }}>
                <ManaCost manaCost={part} size="small" />
              </Box>
            );
          }
          return <span key={i}>{part}</span>;
        })}
      </Typography>
    ));
  };

  // Filter legalities for mobile view
  const allLegalities = Object.entries(card.legalities || {})
    .filter(([format]) => format !== '__typename');

  const mobileLegalities = allLegalities.filter(([format]) => MOBILE_FORMATS.includes(format));
  const additionalLegalities = allLegalities.filter(([format]) => !MOBILE_FORMATS.includes(format));

  return (
    <Drawer
      anchor="bottom"
      open={open}
      onClose={onClose}
      PaperProps={{
        sx: {
          maxHeight: '85vh',
          borderTopLeftRadius: theme.mtg.mobile.sheetBorderRadius,
          borderTopRightRadius: theme.mtg.mobile.sheetBorderRadius,
          bgcolor: 'background.paper',
        }
      }}
    >
      {/* Drag handle */}
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          pt: 1,
          pb: 0.5
        }}
      >
        <Box
          sx={{
            width: 40,
            height: 4,
            bgcolor: 'grey.600',
            borderRadius: 2
          }}
        />
      </Box>

      {/* Header with navigation */}
      <Box sx={{
        px: 2,
        py: 1,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        borderBottom: 1,
        borderColor: 'divider',
        minHeight: 44, // Touch target
      }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          {(onPrevious || onNext) && (
            <>
              <IconButton
                onClick={onPrevious}
                disabled={!hasPrevious}
                size="small"
                sx={{ minWidth: 44, minHeight: 44 }}
              >
                <NavigateBeforeIcon />
              </IconButton>
              <IconButton
                onClick={onNext}
                disabled={!hasNext}
                size="small"
                sx={{ minWidth: 44, minHeight: 44 }}
              >
                <NavigateNextIcon />
              </IconButton>
            </>
          )}
        </Box>
        <IconButton
          onClick={onClose}
          sx={{ minWidth: 44, minHeight: 44 }}
        >
          <CloseIcon />
        </IconButton>
      </Box>

      {/* Scrollable content */}
      <Box sx={{ overflow: 'auto', flex: 1 }}>
        {/* Card Image - centered and sized appropriately */}
        <Box sx={{
          p: 2,
          display: 'flex',
          justifyContent: 'center',
          bgcolor: 'grey.900'
        }}>
          <Box sx={{
            width: '60%',
            maxWidth: 300,
            aspectRatio: '745 / 1040'
          }}>
            <CardImageDisplay
              card={card}
              size="normal"
              borderRadius="6.75%"
              sx={{
                boxShadow: 3,
                width: '100%',
                height: '100%'
              }}
            />
          </Box>
        </Box>

        {/* Card Details */}
        <Box sx={{ p: 2 }}>
          <Stack spacing={2}>
            {/* Card name, mana cost, and type */}
            <Box>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flexWrap: 'wrap', mb: 0.5 }}>
                <Typography variant="h6" fontWeight="bold" sx={{ fontSize: '1.125rem' }}>
                  {card.name}
                </Typography>
                {card.manaCost && <ManaCost manaCost={card.manaCost} size="small" />}
              </Box>
              <Typography variant="body2" color="text.secondary">
                {card.typeLine}
              </Typography>
            </Box>

            {/* Rarity, Set, Collector # row */}
            <Box sx={{ display: 'flex', gap: 1, alignItems: 'center', flexWrap: 'wrap' }}>
              {card.rarity && <RarityBadge rarity={card.rarity} size="small" />}
              <SetLink
                setCode={card.setCode}
                setName={card.setName}
                rarity={card.rarity}
              />
              <Typography variant="caption" color="text.secondary">
                #{card.collectorNumber}
              </Typography>
              {card.reserved && (
                <ReservedListShield size="small" />
              )}
            </Box>

            {/* Oracle Text */}
            {card.oracleText && (
              <Box>
                <Typography variant="subtitle2" fontWeight="bold" gutterBottom>
                  Oracle Text
                </Typography>
                <Box sx={{ pl: 1 }}>
                  {formatOracleText(card.oracleText)}
                </Box>
              </Box>
            )}

            {/* P/T, Loyalty, Defense */}
            {(card.power || card.loyalty || card.defense) && (
              <Box sx={{ display: 'flex', gap: 2 }}>
                {card.power && (
                  <Typography variant="body1" fontWeight="bold">
                    {card.power}/{card.toughness}
                  </Typography>
                )}
                {card.loyalty && (
                  <Typography variant="body1">
                    Loyalty: {card.loyalty}
                  </Typography>
                )}
                {card.defense && (
                  <Typography variant="body1">
                    Defense: {card.defense}
                  </Typography>
                )}
              </Box>
            )}

            <Divider />

            {/* Collection (if collector) */}
            {hasCollector && card.userCollection && (
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <Typography variant="subtitle2" fontWeight="bold">
                  Collection:
                </Typography>
                <CollectionSummary
                  collectionData={card.userCollection}
                  size="small"
                />
              </Box>
            )}

            {/* Treatments */}
            {hasDisplayableTreatments && (
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flexWrap: 'wrap' }}>
                <Typography variant="subtitle2" fontWeight="bold">
                  Treatments:
                </Typography>
                <CardBadges
                  foil={card.foil}
                  nonfoil={card.nonFoil}
                  etched={card.finishes?.includes('etched')}
                  promoTypes={card.promoTypes}
                  frameEffects={card.frameEffects}
                  isPromo={card.promo}
                  digital={card.digital}
                  inline={true}
                />
              </Box>
            )}

            {/* Artist */}
            {card.artist && (
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flexWrap: 'wrap' }}>
                <Typography variant="subtitle2" fontWeight="bold">
                  Artist:
                </Typography>
                <ArtistLinks
                  artist={card.artist}
                  artistIds={card.artistIds}
                />
              </Box>
            )}

            <Divider />

            {/* Legalities - condensed for mobile */}
            <Box>
              <Typography variant="subtitle2" fontWeight="bold" gutterBottom>
                Legalities
              </Typography>
              <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1 }}>
                {mobileLegalities.map(([format, legality]) => {
                  const isLegal = legality === 'legal';
                  return (
                    <Tooltip key={format} title={`${format}: ${LEGALITY_DESCRIPTIONS[legality] || legality}`}>
                      <Box
                        sx={{
                          display: 'flex',
                          alignItems: 'center',
                          gap: 0.25,
                          px: 0.75,
                          py: 0.25,
                          bgcolor: isLegal ? 'success.dark' : 'grey.800',
                          borderRadius: 1,
                          opacity: isLegal ? 1 : 0.6
                        }}
                      >
                        <Box sx={{ color: getLegalityColor(legality), display: 'flex' }}>
                          {LEGALITY_ICONS[legality] || LEGALITY_ICONS.not_legal}
                        </Box>
                        <Typography variant="caption" sx={{ fontSize: '0.625rem' }}>
                          {FORMAT_DISPLAY_NAMES[format] || format}
                        </Typography>
                      </Box>
                    </Tooltip>
                  );
                })}
                {additionalLegalities.length > 0 && (
                  <Button
                    size="small"
                    onClick={() => setShowMoreLegalities(!showMoreLegalities)}
                    endIcon={<KeyboardArrowDownIcon sx={{
                      transform: showMoreLegalities ? 'rotate(180deg)' : 'none',
                      transition: 'transform 0.2s'
                    }} />}
                    sx={{ fontSize: '0.625rem', minHeight: 24, px: 1 }}
                  >
                    {showMoreLegalities ? 'Less' : `+${additionalLegalities.length} more`}
                  </Button>
                )}
              </Box>
              {showMoreLegalities && (
                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5, mt: 1 }}>
                  {additionalLegalities.map(([format, legality]) => {
                    const isLegal = legality === 'legal';
                    return (
                      <Box
                        key={format}
                        sx={{
                          display: 'flex',
                          alignItems: 'center',
                          gap: 0.25,
                          px: 0.5,
                          py: 0.25,
                          bgcolor: isLegal ? 'success.dark' : 'grey.800',
                          borderRadius: 0.5,
                          opacity: isLegal ? 1 : 0.5
                        }}
                      >
                        <Box sx={{ color: getLegalityColor(legality), display: 'flex' }}>
                          {LEGALITY_ICONS[legality] || LEGALITY_ICONS.not_legal}
                        </Box>
                        <Typography variant="caption" sx={{ fontSize: '0.5rem' }}>
                          {format}
                        </Typography>
                      </Box>
                    );
                  })}
                </Box>
              )}
            </Box>

            {/* Prices - condensed row */}
            {card.prices && (card.prices.usd || card.prices.usdFoil) && (
              <>
                <Divider />
                <Box sx={{ display: 'flex', gap: 2 }}>
                  {card.prices.usd && (
                    <PriceDisplay
                      price={card.prices.usd}
                      currency="USD"
                      label="Normal"
                      compact
                    />
                  )}
                  {card.prices.usdFoil && (
                    <PriceDisplay
                      price={card.prices.usdFoil}
                      currency="USD"
                      label="Foil"
                      compact
                    />
                  )}
                </Box>
              </>
            )}

            <Divider />

            {/* External Links - as icon buttons */}
            <Box>
              <Typography variant="subtitle2" fontWeight="bold" gutterBottom>
                Links
              </Typography>
              <Stack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
                {card.purchaseUris?.tcgplayer && (
                  <Button
                    size="small"
                    variant="outlined"
                    startIcon={<OpenInNewIcon sx={{ fontSize: '1rem' }} />}
                    href={card.purchaseUris.tcgplayer}
                    target="_blank"
                    sx={{ minHeight: 36, fontSize: '0.75rem' }}
                  >
                    TCG
                  </Button>
                )}
                {card.scryfallUri && (
                  <Button
                    size="small"
                    variant="outlined"
                    startIcon={<OpenInNewIcon sx={{ fontSize: '1rem' }} />}
                    href={card.scryfallUri}
                    target="_blank"
                    sx={{ minHeight: 36, fontSize: '0.75rem' }}
                  >
                    Scryfall
                  </Button>
                )}
                {card.relatedUris?.edhrec && (
                  <Button
                    size="small"
                    variant="outlined"
                    startIcon={<OpenInNewIcon sx={{ fontSize: '1rem' }} />}
                    href={card.relatedUris.edhrec}
                    target="_blank"
                    sx={{ minHeight: 36, fontSize: '0.75rem' }}
                  >
                    EDHREC
                  </Button>
                )}
                {card.name && (
                  <Button
                    size="small"
                    variant="outlined"
                    startIcon={<OpenInNewIcon sx={{ fontSize: '1rem' }} />}
                    href={generateCardKingdomSearchUrl(card.name)}
                    target="_blank"
                    sx={{ minHeight: 36, fontSize: '0.75rem' }}
                  >
                    CK
                  </Button>
                )}
              </Stack>
            </Box>

            {/* Bottom padding for safe area */}
            <Box sx={{ height: 'env(safe-area-inset-bottom, 16px)' }} />
          </Stack>
        </Box>
      </Box>
    </Drawer>
  );
};
