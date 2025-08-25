import React, { useState } from 'react';
import { Box, Link } from '@mui/material';

type LinkType = 'scryfall' | 'tcgplayer' | 'cardmarket' | 'cardhoarder' | 'edhrec' | 'gatherer';

interface ExternalLinkIconProps {
  type: LinkType;
  url?: string | null;
  className?: string;
  size?: 'small' | 'medium' | 'large';
}

export const ExternalLinkIcon: React.FC<ExternalLinkIconProps> = ({ 
  type, 
  url,
  className = '',
  size = 'medium'
}) => {
  const [imageError, setImageError] = useState(false);
  
  if (!url) return null;

  const getSizeValue = () => {
    switch (size) {
      case 'small': return 20;
      case 'medium': return 30;
      case 'large': return 40;
      default: return 30;
    }
  };

  const sizeValue = getSizeValue();

  const getSiteImage = (siteType: string): string => {
    // For now, we'll use a fallback approach with proper favicons
    // In production, you'd want to download and store these locally
    switch (siteType) {
      case 'scryfall':
        return 'https://scryfall.com/favicon.ico';
      case 'tcgplayer':
        return 'https://www.tcgplayer.com/favicon.ico';
      case 'cardmarket':
        return 'https://www.cardmarket.com/favicon.ico';
      case 'cardhoarder':
        return 'https://www.cardhoarder.com/favicon.ico';
      case 'edhrec':
        return 'https://edhrec.com/favicon.ico';
      case 'gatherer':
        return 'https://gatherer.wizards.com/favicon.ico';
      default:
        return '';
    }
  };

  const getIcon = () => {
    const imageUrl = getSiteImage(type);
    
    if (imageUrl && !imageError) {
      return (
        <Box
          sx={{
            width: sizeValue,
            height: sizeValue,
            borderRadius: '50%',
            overflow: 'hidden',
            border: '1px solid rgba(255,255,255,0.2)',
            boxShadow: '0 2px 4px rgba(0,0,0,0.3)',
            bgcolor: 'rgba(255,255,255,0.9)',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center'
          }}
        >
          <img
            src={imageUrl}
            alt={`${type} logo`}
            style={{
              width: '100%',
              height: '100%',
              objectFit: 'contain',
              padding: '2px'
            }}
            onError={() => setImageError(true)}
          />
        </Box>
      );
    }
    
    return getTextFallback();
  };

  const getTextFallback = () => {
    const configs = {
      scryfall: { bg: 'linear-gradient(135deg, #EA580C 0%, #DC2626 100%)', text: 'SF' },
      tcgplayer: { bg: 'linear-gradient(135deg, #2563EB 0%, #1D4ED8 100%)', text: 'TCG' },
      cardmarket: { bg: 'linear-gradient(135deg, #16A34A 0%, #15803D 100%)', text: 'MKM' },
      cardhoarder: { bg: 'linear-gradient(135deg, #7C3AED 0%, #6D28D9 100%)', text: 'CH' },
      edhrec: { bg: 'linear-gradient(135deg, #4F46E5 0%, #4338CA 100%)', text: 'EDH' },
      gatherer: { bg: 'linear-gradient(135deg, #4B5563 0%, #374151 100%)', text: 'WTC' }
    };

    const config = configs[type as keyof typeof configs] || 
                  { bg: 'linear-gradient(135deg, #6B7280 0%, #4B5563 100%)', text: '?' };

    return (
      <Box
        sx={{
          width: sizeValue,
          height: sizeValue,
          borderRadius: '50%',
          background: config.bg,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          color: 'white',
          fontWeight: 'bold',
          fontSize: '0.6rem',
          border: '1px solid rgba(255,255,255,0.2)',
          boxShadow: '0 2px 4px rgba(0,0,0,0.3)'
        }}
      >
        {config.text}
      </Box>
    );
  };

  return (
    <Link
      href={url}
      target="_blank"
      rel="noopener noreferrer"
      className={className}
      tabIndex={-1}
      onClick={(e) => {
        e.stopPropagation();
      }}
      sx={{
        display: 'inline-flex',
        transition: 'opacity 0.2s ease',
        '&:hover': {
          opacity: 0.8
        }
      }}
      title={`View on ${type}`}
    >
      {getIcon()}
    </Link>
  );
};