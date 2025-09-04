import React from 'react';
import { Box, Chip } from '@mui/material';

interface CardBadgesProps {
  foil?: boolean;       // Individual boolean properties instead of arrays
  nonfoil?: boolean;
  promoTypes?: string[];
  frameEffects?: string[] | null;
  isPromo?: boolean;
  digital?: boolean;    // Whether this card is digital
  excludePromoTypes?: string[];
  excludeFrameEffects?: string[];
  inline?: boolean;  // When true, display inline rather than absolute positioned
}

// Default exclusions - ignore these badge types
const DEFAULT_EXCLUDE_PROMO_TYPES: string[] = ['boosterfun'];  // Ignore boosterfun
const DEFAULT_EXCLUDE_FRAME_EFFECTS: string[] = ['inverted', 'legendary', 'enchantment', 'etched'];  // Ignore these frame effects

export const CardBadges: React.FC<CardBadgesProps> = ({
  foil = false,
  nonfoil = false,
  promoTypes,
  frameEffects,
  isPromo = false,
  digital = false,
  excludePromoTypes = DEFAULT_EXCLUDE_PROMO_TYPES,
  excludeFrameEffects = DEFAULT_EXCLUDE_FRAME_EFFECTS,
  inline = false
}) => {
  // Ensure arrays are not null
  const safePromoTypes = promoTypes || [];
  const safeFrameEffects = frameEffects || [];
  
  // Special foil promo types: check for entries ending in foil, galaxyfoil, ripplefoil, or exactly oilslick
  const hasSpecialFoilPromo = safePromoTypes.some(promo => {
    const lowerPromo = promo.toLowerCase();
    return lowerPromo.endsWith('foil') || 
           lowerPromo.endsWith('galaxyfoil') || 
           lowerPromo.endsWith('ripplefoil') ||
           lowerPromo === 'oilslick';
  });
  
  // Check if card is serialized - if so, only show that badge
  const isSerialized = safePromoTypes.some(promo => promo.toLowerCase() === 'serialized');
  
  // Apply new foil badge logic using boolean properties
  const displayProperties: string[] = [];
  
  if (!isSerialized) {
    // Non-foil badge: only show if card is BOTH non-foil AND foil (hybrid cards)
    if (nonfoil && foil) {
      displayProperties.push('nonfoil');
    }
    
    // Foil badge: only show if card is foil BUT no special foil promo types
    if (foil && !hasSpecialFoilPromo) {
      displayProperties.push('foil');
    }
  }
  
  // Filter out excluded promo types - if serialized, only show that
  const displayPromoTypes = isSerialized 
    ? ['serialized'] 
    : safePromoTypes.filter(
        promoType => !excludePromoTypes.includes(promoType.toLowerCase())
      );

  // Filter out excluded frame effects - hide all if serialized
  const displayFrameEffects = isSerialized 
    ? [] 
    : safeFrameEffects.filter(
        effect => !excludeFrameEffects.includes(effect.toLowerCase())
      );

  // Format finish text for display
  const formatFinishText = (finish: string): string => {
    const formatMap: Record<string, string> = {
      'foil': 'Foil',
      'etched': 'Etched',
      'glossy': 'Glossy',
      'nonfoil': 'Non-Foil',
      'galaxyfoil': 'Galaxy Foil',
    };
    return formatMap[finish.toLowerCase()] || finish;
  };

  // Format promo type text for display
  const formatPromoText = (promoType: string): string => {
    const lowerType = promoType.toLowerCase();
    
    // Handle FF + Roman numerals pattern (e.g., "ffxiv" -> "FF XIV")
    const ffRomanMatch = lowerType.match(/^ff([ivxlcdm]+)$/);
    if (ffRomanMatch) {
      return `FF ${ffRomanMatch[1].toUpperCase()}`;
    }
    
    const formatMap: Record<string, string> = {
      'prerelease': 'Pre-Release',
      'promobundle': 'Promo Bundle',
      'buyabox': 'Buy-a-Box',
      'intro_pack': 'Intro Pack',
      'gameday': 'Game Day',
      'release': 'Release',
      'fnm': 'FNM',
      'judge_gift': 'Judge Gift',
      'arena_league': 'Arena League',
      'player_rewards': 'Player Rewards',
      'gateway': 'Gateway',
      'wizards_play_network': 'WPN',
      'duels': 'Duels',
      'premiere_shop': 'Premiere Shop',
      'grand_prix': 'Grand Prix',
      'pro_tour': 'Pro Tour',
      'worlds': 'Worlds',
      'convention': 'Convention',
      'treasure_chest': 'Treasure Chest',
      'boosterfun': 'Booster Fun',
      'serialized': 'Serialized',
      'surgefoil': 'Surge Foil',
      'raisedfoil': 'Raised Foil',
      'sldbonus': 'SLD Bonus',
      'datestamped': 'Date Stamped',
      'promopack': 'Promo Pack',
      'starterdeck': 'Starter Deck',
    };
    return formatMap[lowerType] || promoType.replace(/_/g, ' ')
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  };

  // Format frame effect text for display
  const formatFrameEffectText = (effect: string): string => {
    const formatMap: Record<string, string> = {
      'legendary': 'Legendary',
      'miracle': 'Miracle',
      'nyxtouched': 'Nyxtouched',
      'draft': 'Draft',
      'devoid': 'Devoid',
      'tombstone': 'Tombstone',
      'colorshifted': 'Colorshifted',
      'inverted': 'Inverted',
      'sunmoondfc': 'Sun/Moon DFC',
      'compasslanddfc': 'Compass Land DFC',
      'originpwdfc': 'Origin PW DFC',
      'mooneldrazidfc': 'Moon Eldrazi DFC',
      'moonreversemoondfc': 'Moon Reverse Moon DFC',
      'showcase': 'Showcase',
      'extendedart': 'Extended Art',
      'companion': 'Companion',
      'etched': 'Etched',
      'snow': 'Snow',
      'lesson': 'Lesson',
      'shatteredglass': 'Shattered Glass',
      'convertdfc': 'Convert DFC',
      'fandfc': 'Fan DFC',
      'upsidedowndfc': 'Upside Down DFC',
      'fullart': 'Full Art',
      'textless': 'Textless',
    };
    return formatMap[effect.toLowerCase()] || effect.replace(/_/g, ' ')
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  };

  // Get finish color  
  const getFinishColor = (finish: string): 'primary' | 'secondary' | 'default' | 'info' => {
    switch (finish.toLowerCase()) {
      case 'foil':
        return 'primary';
      case 'etched':
        return 'secondary';
      case 'nonfoil':
        return 'info';
      default:
        return 'default';
    }
  };

  // If no badges to display, return null
  if (displayProperties.length === 0 && displayPromoTypes.length === 0 && displayFrameEffects.length === 0 && !isPromo && !digital) {
    return null;
  }

  return (
    <Box 
      sx={{ 
        display: 'flex', 
        flexDirection: inline ? 'row' : 'column-reverse',  // Row for inline, column-reverse for overlay
        flexWrap: inline ? 'wrap' : 'nowrap',
        gap: 0.5,
        ...(inline ? {
          // Inline mode - normal document flow
          alignItems: 'center'
        } : {
          // Overlay mode - absolute positioning on card
          position: 'absolute',
          bottom: 185,  // Move higher up to avoid overlapping release date
          right: 8,
          zIndex: 15,
          alignItems: 'flex-end'
        })
      }}
    >
      {/* Property badges (foil/non-foil) */}
      {displayProperties.map((property, index) => (
        <Chip
          key={`property-${index}`}
          label={formatFinishText(property)}
          size="small"
          color={getFinishColor(property)}
          sx={{
            height: 20,
            fontSize: '0.625rem',
            fontWeight: 600,
            '& .MuiChip-label': {
              px: 1
            },
            backdropFilter: 'blur(4px)',
            backgroundColor: property.toLowerCase() === 'foil' 
              ? 'rgba(33, 150, 243, 0.9)' 
              : property.toLowerCase() === 'etched'
              ? 'rgba(156, 39, 176, 0.9)'
              : property.toLowerCase() === 'nonfoil'
              ? 'rgba(158, 158, 158, 0.9)'
              : 'rgba(97, 97, 97, 0.9)',
            color: 'white',
            border: '1px solid rgba(255, 255, 255, 0.2)',
            boxShadow: '0 2px 4px rgba(0,0,0,0.2)'
          }}
        />
      ))}
      
      {/* Digital badge */}
      {digital && (
        <Chip
          label="Digital"
          size="small"
          sx={{
            height: 20,
            fontSize: '0.625rem',
            fontWeight: 600,
            '& .MuiChip-label': {
              px: 1
            },
            backdropFilter: 'blur(4px)',
            backgroundColor: 'rgba(138, 43, 226, 0.9)', // Purple for digital
            color: 'white',
            border: '1px solid rgba(255, 255, 255, 0.2)',
            boxShadow: '0 2px 4px rgba(0,0,0,0.2)'
          }}
        />
      )}
      
      {/* Promo type badges */}
      {displayPromoTypes.map((promoType, index) => (
        <Chip
          key={`promo-${index}`}
          label={formatPromoText(promoType)}
          size="small"
          sx={{
            height: 20,
            fontSize: '0.625rem',
            fontWeight: promoType.toLowerCase() === 'serialized' ? 700 : 600,
            '& .MuiChip-label': {
              px: 1
            },
            backdropFilter: 'blur(4px)',
            backgroundColor: promoType.toLowerCase() === 'serialized' 
              ? 'rgba(255, 215, 0, 0.95)'  // Gold color for serialized
              : 'rgba(255, 152, 0, 0.9)',
            color: promoType.toLowerCase() === 'serialized' ? '#000' : 'white',
            border: promoType.toLowerCase() === 'serialized' 
              ? '1px solid rgba(255, 215, 0, 0.5)'
              : '1px solid rgba(255, 255, 255, 0.2)',
            boxShadow: promoType.toLowerCase() === 'serialized'
              ? '0 2px 8px rgba(255, 215, 0, 0.5)'
              : '0 2px 4px rgba(0,0,0,0.2)'
          }}
        />
      ))}
      
      {/* Generic promo badge if isPromo is true but no specific types (unless serialized) */}
      {isPromo && displayPromoTypes.length === 0 && !isSerialized && (
        <Chip
          label="Promo"
          size="small"
          sx={{
            height: 20,
            fontSize: '0.625rem',
            fontWeight: 600,
            '& .MuiChip-label': {
              px: 1
            },
            backdropFilter: 'blur(4px)',
            backgroundColor: 'rgba(255, 152, 0, 0.9)',
            color: 'white',
            border: '1px solid rgba(255, 255, 255, 0.2)',
            boxShadow: '0 2px 4px rgba(0,0,0,0.2)'
          }}
        />
      )}
      
      {/* Frame effect badges */}
      {displayFrameEffects.map((effect, index) => (
        <Chip
          key={`effect-${index}`}
          label={formatFrameEffectText(effect)}
          size="small"
          sx={{
            height: 20,
            fontSize: '0.625rem',
            fontWeight: 600,
            '& .MuiChip-label': {
              px: 1
            },
            backdropFilter: 'blur(4px)',
            backgroundColor: effect.toLowerCase() === 'showcase' 
              ? 'rgba(76, 175, 80, 0.9)'
              : effect.toLowerCase() === 'extendedart'
              ? 'rgba(0, 188, 212, 0.9)'
              : effect.toLowerCase() === 'fullart'
              ? 'rgba(255, 87, 34, 0.9)'
              : 'rgba(121, 85, 72, 0.9)',
            color: 'white',
            border: '1px solid rgba(255, 255, 255, 0.2)',
            boxShadow: '0 2px 4px rgba(0,0,0,0.2)'
          }}
        />
      ))}
    </Box>
  );
};