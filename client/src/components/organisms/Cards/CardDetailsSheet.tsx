import React, { useState } from 'react';
import {
  Box,
  Typography,
  IconButton,
  Divider,
  Stack,
  Button,
  Tooltip,
  Drawer,
  Collapse
} from '../../atoms';
import { getLegalityColor, getRarityColor } from '../../../theme';
import type { Card } from '../../../types/card';
import { useCollectorParam } from '../../../hooks/useCollectorParam';
import { CollectionSummary, WishlistSummary, ManaCost, RulingsDisplay } from '../../molecules';
import { RarityBadge, PriceDisplay } from '../../atoms';
import { ReservedListShield } from '../../atoms/Cards/ReservedListShield';
import { CardImageDisplay } from './CardImageDisplay';
import { CardBadges } from '../../molecules/Cards/CardBadges';
import { SetLink } from '../../atoms';
import { ArtistLinks } from '../../molecules';
import { RelatedCardsDisplay } from './RelatedCardsDisplay';
import { AllPrintingsDisplay } from './AllPrintingsDisplay';
import {
  NavigateBeforeIcon,
  NavigateNextIcon,
  OpenInNewIcon,
  CircleIcon,
  CircleOutlinedIcon,
  RemoveCircleIcon,
  WarningIcon,
  CloseIcon,
  KeyboardArrowDownIcon,
  ExpandMoreIcon
} from '../../atoms/Icons';
import { useTheme } from '../../atoms';
import { useResponsiveBreakpoints } from '../../../hooks/useResponsiveBreakpoints';

interface CardDetailsSheetProps {
  open: boolean;
  onClose: () => void;
  card?: Card;
  onPrevious?: () => void;
  onNext?: () => void;
  hasPrevious?: boolean;
  hasNext?: boolean;
}

// Collapsible section component for mobile
interface CollapsibleSectionProps {
  title: string;
  defaultExpanded?: boolean;
  children: React.ReactNode;
}

const CollapsibleSection: React.FC<CollapsibleSectionProps> = ({
  title,
  defaultExpanded = false,
  children
}) => {
  const [expanded, setExpanded] = useState(defaultExpanded);

  return (
    <Box>
      <Button
        fullWidth
        onClick={() => setExpanded(!expanded)}
        sx={{
          justifyContent: 'space-between',
          py: 1,
          px: 0,
          color: 'text.primary',
          textTransform: 'none',
          '&:hover': {
            bgcolor: 'transparent'
          }
        }}
        endIcon={
          <ExpandMoreIcon
            sx={{
              transform: expanded ? 'rotate(180deg)' : 'rotate(0deg)',
              transition: 'transform 0.2s'
            }}
          />
        }
      >
        <Typography variant="subtitle2" fontWeight="bold">
          {title}
        </Typography>
      </Button>
      <Collapse in={expanded}>
        <Box sx={{ pb: 1 }}>
          {children}
        </Box>
      </Collapse>
    </Box>
  );
};

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

const FORMAT_DISPLAY_NAMES: Record<string, string> = {
  standard: 'Std',
  pioneer: 'Pio',
  modern: 'Mod',
  legacy: 'Leg',
  vintage: 'Vin',
  commander: 'EDH',
  historic: 'His',
  timeless: 'Tim',
  pauper: 'Pau',
  penny: 'Pen',
  brawl: 'Brl',
  alchemy: 'Alc',
  explorer: 'Exp',
  duel: 'Duel',
  oldschool: 'Old',
  premodern: 'Pre'
};

/**
 * Mobile-optimized card details displayed as a bottom sheet.
 * Shows all information with collapsible sections.
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
  const { isTablet } = useResponsiveBreakpoints();

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

  // All legalities for display
  const allLegalities = Object.entries(card.legalities || {})
    .filter(([format]) => format !== '__typename')
    .sort((a, b) => {
      const order = ['standard', 'pioneer', 'modern', 'legacy', 'vintage', 'commander'];
      const aIndex = order.indexOf(a[0]);
      const bIndex = order.indexOf(b[0]);
      if (aIndex !== -1 && bIndex !== -1) return aIndex - bIndex;
      if (aIndex !== -1) return -1;
      if (bIndex !== -1) return 1;
      return a[0].localeCompare(b[0]);
    });

  // Check for related cards
  const hasRelatedCards = card.allParts && card.allParts.length > 1;

  return (
    <Drawer
      anchor="bottom"
      open={open}
      onClose={onClose}
      PaperProps={{
        sx: {
          maxHeight: '90vh',
          borderTopLeftRadius: theme.mtg.mobile.sheetBorderRadius,
          borderTopRightRadius: theme.mtg.mobile.sheetBorderRadius,
          bgcolor: 'grey.900',
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
        minHeight: 44,
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
          onClick={(e) => {
            e.preventDefault();
            e.stopPropagation();
            onClose();
          }}
          sx={{ minWidth: 44, minHeight: 44 }}
        >
          <CloseIcon />
        </IconButton>
      </Box>

      {/* Scrollable content */}
      <Box sx={{ overflow: 'auto', flex: 1 }}>
        {/* Card Image - full width on mobile, 2/3 on tablet */}
        <Box sx={{
          width: '100%',
          display: 'flex',
          justifyContent: 'center',
          bgcolor: 'black'
        }}>
          <Box sx={{
            width: isTablet ? '66.67%' : '100%',
            aspectRatio: '745 / 1040'
          }}>
            <CardImageDisplay
              card={card}
              size="large"
              borderRadius={0}
              sx={{
                width: '100%',
                height: '100%'
              }}
            />
          </Box>
        </Box>

        {/* Card Details */}
        <Box sx={{ p: 2 }}>
          <Stack spacing={1.5}>
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

            {/* Flavor Text */}
            {card.flavorText && (
              <Box>
                <Typography variant="subtitle2" fontWeight="bold" gutterBottom>
                  Flavor Text
                </Typography>
                <Typography variant="body2" fontStyle="italic" sx={{ pl: 1, color: 'text.secondary' }}>
                  {card.flavorText}
                </Typography>
              </Box>
            )}

            {/* P/T, Loyalty, Defense */}
            {(card.power || card.loyalty || card.defense) && (
              <Box sx={{ display: 'flex', gap: 2, justifyContent: card.power ? 'flex-end' : 'flex-start' }}>
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

            {/* Collection & Wishlist (if collector) */}
            {hasCollector && (card.userCollection || card.userWishlist) && (
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, flexWrap: 'wrap' }}>
                {card.userCollection && (
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <Typography variant="subtitle2" fontWeight="bold">
                      Collection:
                    </Typography>
                    <CollectionSummary
                      collectionData={card.userCollection}
                      size="small"
                      forceInteractive
                    />
                  </Box>
                )}
                {card.userWishlist && (
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <Typography variant="subtitle2" fontWeight="bold">
                      Wishlist:
                    </Typography>
                    <WishlistSummary
                      wishlistData={card.userWishlist}
                      size="small"
                      forceInteractive
                    />
                  </Box>
                )}
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

            {/* Legalities - Collapsible */}
            <CollapsibleSection title="Legalities" defaultExpanded={false}>
              <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.75 }}>
                {allLegalities.map(([format, legality]) => {
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
              </Box>
            </CollapsibleSection>

            {/* Market Prices - Collapsible */}
            {card.prices && (card.prices.usd || card.prices.usdFoil || card.prices.eur || card.prices.eurFoil) && (
              <CollapsibleSection title="Market Prices" defaultExpanded={false}>
                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
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
                  {card.prices.eur && (
                    <PriceDisplay
                      price={card.prices.eur}
                      currency="EUR"
                      label="Normal"
                      compact
                    />
                  )}
                  {card.prices.eurFoil && (
                    <PriceDisplay
                      price={card.prices.eurFoil}
                      currency="EUR"
                      label="Foil"
                      compact
                    />
                  )}
                </Box>
              </CollapsibleSection>
            )}

            {/* External Links - Collapsible */}
            <CollapsibleSection title="External Links" defaultExpanded={false}>
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
                    TCGPlayer
                  </Button>
                )}
                {card.purchaseUris?.cardmarket && (
                  <Button
                    size="small"
                    variant="outlined"
                    startIcon={<OpenInNewIcon sx={{ fontSize: '1rem' }} />}
                    href={card.purchaseUris.cardmarket}
                    target="_blank"
                    sx={{ minHeight: 36, fontSize: '0.75rem' }}
                  >
                    Cardmarket
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
                {card.relatedUris?.gatherer && (
                  <Button
                    size="small"
                    variant="outlined"
                    startIcon={<OpenInNewIcon sx={{ fontSize: '1rem' }} />}
                    href={card.relatedUris.gatherer}
                    target="_blank"
                    sx={{ minHeight: 36, fontSize: '0.75rem' }}
                  >
                    Gatherer
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
                    Card Kingdom
                  </Button>
                )}
              </Stack>
            </CollapsibleSection>

            {/* Rulings - has its own collapse */}
            {card.rulingsUri && (
              <RulingsDisplay rulingsUri={card.rulingsUri} />
            )}

            {/* Related Cards - has its own collapse */}
            {hasRelatedCards && (
              <RelatedCardsDisplay
                relatedCardIds={card.allParts!.map(part => part.id).filter((id): id is string => !!id)}
                currentCardId={card.id}
              />
            )}

            {/* Other Printings - has its own collapse */}
            <AllPrintingsDisplay cardName={card.name} currentCardId={card.id} />

            {/* Bottom padding for safe area */}
            <Box sx={{ height: 'env(safe-area-inset-bottom, 16px)' }} />
          </Stack>
        </Box>
      </Box>
    </Drawer>
  );
};
