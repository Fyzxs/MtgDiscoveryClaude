import React from 'react';
import { Box, Chip } from '@mui/material';

interface CardBadgesProps {
  finishes?: string[];
  promoTypes?: string[];
  frameEffects?: string[] | null;
  isPromo?: boolean;
  excludeFinishes?: string[];
  excludePromoTypes?: string[];
  excludeFrameEffects?: string[];
}

// Default exclusions
const DEFAULT_EXCLUDE_FINISHES = [];  // Include all finishes now
const DEFAULT_EXCLUDE_PROMO_TYPES = [];  // Include all promo types now
const DEFAULT_EXCLUDE_FRAME_EFFECTS = [];

export const CardBadges: React.FC<CardBadgesProps> = ({
  finishes,
  promoTypes,
  frameEffects,
  isPromo = false,
  excludeFinishes = DEFAULT_EXCLUDE_FINISHES,
  excludePromoTypes = DEFAULT_EXCLUDE_PROMO_TYPES,
  excludeFrameEffects = DEFAULT_EXCLUDE_FRAME_EFFECTS
}) => {
  // Ensure arrays are not null
  const safeFinishes = finishes || [];
  const safePromoTypes = promoTypes || [];
  const safeFrameEffects = frameEffects || [];
  
  // Filter out excluded finishes
  const displayFinishes = safeFinishes.filter(
    finish => !excludeFinishes.includes(finish.toLowerCase())
  );
  
  // Filter out excluded promo types
  const displayPromoTypes = safePromoTypes.filter(
    promoType => !excludePromoTypes.includes(promoType.toLowerCase())
  );

  // Filter out excluded frame effects
  const displayFrameEffects = safeFrameEffects.filter(
    effect => !excludeFrameEffects.includes(effect.toLowerCase())
  );

  // Format finish text for display
  const formatFinishText = (finish: string): string => {
    const formatMap: Record<string, string> = {
      'foil': 'Foil',
      'etched': 'Etched',
      'glossy': 'Glossy',
      'nonfoil': 'Non-Foil',
    };
    return formatMap[finish.toLowerCase()] || finish;
  };

  // Format promo type text for display
  const formatPromoText = (promoType: string): string => {
    const formatMap: Record<string, string> = {
      'prerelease': 'Prerelease',
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
    };
    return formatMap[promoType.toLowerCase()] || promoType.replace(/_/g, ' ')
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
  if (displayFinishes.length === 0 && displayPromoTypes.length === 0 && displayFrameEffects.length === 0 && !isPromo) {
    return null;
  }

  return (
    <Box 
      sx={{ 
        display: 'flex', 
        flexDirection: 'column',
        gap: 0.5,
        position: 'absolute',
        top: 40,
        right: 8,
        zIndex: 15,
        alignItems: 'flex-end'
      }}
    >
      {/* Finish badges */}
      {displayFinishes.map((finish, index) => (
        <Chip
          key={`finish-${index}`}
          label={formatFinishText(finish)}
          size="small"
          color={getFinishColor(finish)}
          sx={{
            height: 20,
            fontSize: '0.625rem',
            fontWeight: 600,
            '& .MuiChip-label': {
              px: 1
            },
            backdropFilter: 'blur(4px)',
            backgroundColor: finish.toLowerCase() === 'foil' 
              ? 'rgba(33, 150, 243, 0.9)' 
              : finish.toLowerCase() === 'etched'
              ? 'rgba(156, 39, 176, 0.9)'
              : finish.toLowerCase() === 'nonfoil'
              ? 'rgba(158, 158, 158, 0.9)'
              : 'rgba(97, 97, 97, 0.9)',
            color: 'white',
            border: '1px solid rgba(255, 255, 255, 0.2)',
            boxShadow: '0 2px 4px rgba(0,0,0,0.2)'
          }}
        />
      ))}
      
      {/* Promo type badges */}
      {displayPromoTypes.map((promoType, index) => (
        <Chip
          key={`promo-${index}`}
          label={formatPromoText(promoType)}
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
      ))}
      
      {/* Generic promo badge if isPromo is true but no specific types */}
      {isPromo && displayPromoTypes.length === 0 && (
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