import React, { useState } from 'react';
import { 
  Card as MuiCard, 
  CardMedia, 
  Box, 
  Typography,
  Link,
  IconButton,
  Modal
} from '@mui/material';
import { ZoomIn } from '@mui/icons-material';
import type { Card, CardContext } from '../../types/card';
import { PriceDisplay } from '../atoms/PriceDisplay';
import { CollectorNumber } from '../atoms/CollectorNumber';
import { RarityBadge } from '../atoms/RarityBadge';
import { SetIcon } from '../atoms/SetIcon';
import { ExternalLinkIcon } from '../atoms/ExternalLinkIcon';

interface MtgCardProps {
  card: Card;
  context?: CardContext;
  isSelected?: boolean;
  onSelectionChange?: (cardId: string, selected: boolean) => void;
  onCardClick?: (cardId?: string) => void;
  onSetClick?: (setCode?: string) => void;
  onArtistClick?: (artistName: string, artistId?: string) => void;
  className?: string;
}

export const MtgCard: React.FC<MtgCardProps> = ({ 
  card, 
  context = {},
  isSelected = false,
  onSelectionChange,
  onCardClick,
  onSetClick,
  onArtistClick,
  className = ''
}) => {
  const [imageLoaded, setImageLoaded] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [isZoomClicked, setIsZoomClicked] = useState(false);

  // Parse multiple artists
  const parseArtists = (artistString?: string): string[] => {
    if (!artistString) return [];
    return artistString.split(/\s+(?:&|and)\s+/i);
  };

  const artists = parseArtists(card.artist);
  const displayArtists = context.isOnArtistPage && context.currentArtist
    ? artists.filter(a => a !== context.currentArtist)
    : artists;

  // Debug: Log what we're receiving
  // console.log('MtgCard Debug:', {
  //   cardName: card?.name,
  //   artist: card?.artist,
  //   collectorNumber: card?.collectorNumber,
  //   rarity: card?.rarity,
  //   displayArtists,
  //   context
  // });


  // Format date
  const formatDate = (dateString?: string): string => {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { month: 'short', year: 'numeric' });
  };

  const handleCardClick = (e: React.MouseEvent) => {
    // Don't trigger selection if zoom was just clicked
    if (isZoomClicked) {
      setIsZoomClicked(false);
      return;
    }
    
    // Don't trigger selection if clicking on links
    const target = e.target as HTMLElement;
    const clickedLink = target.closest('a');
    
    if (!clickedLink && onSelectionChange) {
      e.preventDefault();
      e.stopPropagation();
      // Only select, never deselect when clicking the card
      if (!isSelected) {
        onSelectionChange(card.id || '', true);
      }
    }
  };

  return (
    <MuiCard 
      elevation={4}
      onClick={handleCardClick}
      onFocus={() => {
        // Select card when it gains focus
        if (onSelectionChange && !isSelected) {
          onSelectionChange(card.id || '', true);
        }
      }}
      onBlur={() => {
        // Deselect card when it loses focus
        if (onSelectionChange && isSelected) {
          onSelectionChange(card.id || '', false);
        }
      }}
      data-mtg-card="true"
      tabIndex={0}
      sx={{
        position: 'relative',
        width: '280px',
        bgcolor: 'grey.800',
        borderRadius: 6,
        border: isSelected ? '4px solid' : '3px solid',
        borderColor: isSelected ? '#2196F3' : 'grey.700',
        overflow: 'hidden',
        boxShadow: isSelected 
          ? '0 0 50px rgba(33, 150, 243, 0.8), 0 0 30px rgba(33, 150, 243, 0.6), 0 25px 50px -12px rgba(0, 0, 0, 0.25)'
          : '0 25px 50px -12px rgba(0, 0, 0, 0.25)',
        transition: 'all 0.3s ease-in-out',
        transform: isSelected ? 'scale(1.03)' : 'scale(1)',
        cursor: onSelectionChange ? 'pointer' : 'default',
        outline: 'none',
        '&:focus': {
          outline: '2px solid',
          outlineColor: 'primary.light',
          outlineOffset: '2px'
        },
        // Combined hover effect with rarity-based glow and overlay fade
        ...(card.rarity ? {
          '&:hover': {
            boxShadow: isSelected 
              ? '0 0 50px rgba(33, 150, 243, 0.8), 0 25px 50px -12px rgba(0, 0, 0, 0.5)'
              : card.rarity.toLowerCase() === 'common' ? '0 0 40px rgba(156, 163, 175, 0.5), 0 0 20px rgba(156, 163, 175, 0.3)' :
                card.rarity.toLowerCase() === 'uncommon' ? '0 0 40px rgba(156, 163, 175, 0.4), 0 0 20px rgba(156, 163, 175, 0.2)' :
                card.rarity.toLowerCase() === 'rare' ? '0 0 40px rgba(217, 119, 6, 0.5), 0 0 20px rgba(217, 119, 6, 0.3)' :
                card.rarity.toLowerCase() === 'mythic' ? '0 0 40px rgba(234, 88, 12, 0.6), 0 0 20px rgba(234, 88, 12, 0.4)' :
                card.rarity.toLowerCase() === 'special' || card.rarity.toLowerCase() === 'bonus' ? '0 0 40px rgba(147, 51, 234, 0.5), 0 0 20px rgba(147, 51, 234, 0.3)' :
                '0 0 40px rgba(156, 163, 175, 0.4), 0 0 20px rgba(156, 163, 175, 0.2)',
            transform: isSelected ? 'scale(1.03)' : 'scale(1.01)',
            '& .card-overlay': {
              opacity: 0.5
            },
            '& .zoom-indicator': {
              opacity: 1,
              transform: 'scale(1)'
            }
          }
        } : {
          '&:hover': {
            boxShadow: isSelected 
              ? '0 0 50px rgba(33, 150, 243, 0.8), 0 25px 50px -12px rgba(0, 0, 0, 0.5)'
              : '0 0 40px rgba(156, 163, 175, 0.4), 0 25px 50px -12px rgba(0, 0, 0, 0.5)',
            transform: isSelected ? 'scale(1.03)' : 'scale(1.01)',
            '& .card-overlay': {
              opacity: 0.5
            },
            '& .zoom-indicator': {
              opacity: 1,
              transform: 'scale(1)'
            }
          }
        })
      }}
      className={className}
    >
      <Box sx={{ position: 'relative', pointerEvents: 'none' }}>
        {/* Card Back Placeholder - always visible initially */}
        <CardMedia
          component="img"
          image="/cardback.jpeg"
          alt="Magic card back"
          sx={{
            width: '100%',
            aspectRatio: '745/1040',
            objectFit: 'cover',
            position: 'absolute',
            top: 0,
            left: 0,
            zIndex: 1,
            pointerEvents: 'none'
          }}
        />
        
        {/* Actual Card Image - fades in over placeholder */}
        <CardMedia
          component="img"
          image={card.imageUris?.normal || card.imageUris?.large || ''}
          alt={card.name}
          sx={{
            width: '100%',
            aspectRatio: '745/1040',
            objectFit: 'cover',
            position: 'relative',
            zIndex: 2,
            opacity: imageLoaded ? 1 : 0,
            transition: 'opacity 0.8s ease-in-out',
            pointerEvents: 'none'
          }}
          onLoad={() => {
            // Add delay to see the card back placeholder
            setTimeout(() => {
              setImageLoaded(true);
            }, 500);
          }}
          loading="lazy"
        />
      </Box>

      {/* Zoom Modal Indicator - appears on hover */}
      <IconButton
        tabIndex={-1}
        onClick={(e) => {
          e.preventDefault();
          e.stopPropagation();
          setIsZoomClicked(true);
          setModalOpen(true);
        }}
        onMouseDown={(e) => {
          e.stopPropagation();
        }}
        sx={{
          position: 'absolute',
          top: 12,
          right: 12,
          bgcolor: 'rgba(0, 0, 0, 0.15)',
          color: 'rgba(255, 255, 255, 0.4)',
          zIndex: 15,
          opacity: 0,
          transform: 'scale(0.9)',
          transition: 'all 0.3s ease-in-out',
          width: 70, // Quarter of the card width (280px)
          height: 70,
          borderRadius: 3,
          '&:hover': {
            bgcolor: 'rgba(0, 0, 0, 0.6)',
            color: 'white',
            transform: 'scale(1.02)',
          }
        }}
        className="zoom-indicator"
      >
        <ZoomIn sx={{ fontSize: '3rem' }} />
      </IconButton>
      
      {/* Card Info Overlay - positioned at bottom of entire card */}
      <Box
        sx={{
          position: 'absolute',
          bottom: 0,
          left: 0,
          right: 0,
          background: isSelected 
            ? 'linear-gradient(to top, rgba(1,32,96,1) 0%, rgba(1,32,96,0.95) 60%, rgba(1,32,96,0) 100%)'
            : 'linear-gradient(to top, rgba(0,0,0,1) 0%, rgba(0,0,0,0.95) 60%, rgba(0,0,0,0) 100%)',
          pt: 7,
          zIndex: 10,
          opacity: 1,
          transition: 'all 0.3s ease-in-out'
        }}
        className="card-overlay"
      >
        <Box sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 0.5 }}>
          {/* Collector Info Row */}
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            {/* Separate but touching Rarity and Collector Number Badges */}
            <Box sx={{ display: 'flex', alignItems: 'center' }}>
              {card.rarity && (
                <Box
                  sx={{
                    display: 'inline-flex',
                    alignItems: 'center',
                    px: 0.75,
                    py: 0.25,
                    borderTopLeftRadius: '4px',
                    borderBottomLeftRadius: '4px',
                    borderTopRightRadius: 0,
                    borderBottomRightRadius: 0,
                    background: card.rarity.toLowerCase() === 'common' ? 'linear-gradient(135deg, #4B5563 0%, #374151 100%)' :
                              card.rarity.toLowerCase() === 'uncommon' ? 'linear-gradient(135deg, #9CA3AF 0%, #6B7280 100%)' :
                              card.rarity.toLowerCase() === 'rare' ? 'linear-gradient(135deg, #F59E0B 0%, #D97706 100%)' :
                              card.rarity.toLowerCase() === 'mythic' ? 'linear-gradient(135deg, #EA580C 0%, #DC2626 100%)' :
                              card.rarity.toLowerCase() === 'special' || card.rarity.toLowerCase() === 'bonus' ? 'linear-gradient(135deg, #A855F7 0%, #9333EA 100%)' :
                              'linear-gradient(135deg, #4B5563 0%, #374151 100%)',
                    color: 'white',
                    fontSize: '0.75rem',
                    fontWeight: 'bold',
                    textTransform: 'uppercase',
                    boxShadow: '0 1px 3px rgba(0, 0, 0, 0.3)'
                  }}
                >
                  <Typography variant="caption" sx={{ fontSize: 'inherit', fontWeight: 'inherit' }}>
                    {card.rarity.charAt(0)}
                  </Typography>
                </Box>
              )}
              {card.collectorNumber && (
                <Box
                  sx={{
                    display: 'inline-flex',
                    alignItems: 'center',
                    px: 0.75,
                    py: 0.25,
                    borderTopLeftRadius: card.rarity ? 0 : '4px',
                    borderBottomLeftRadius: card.rarity ? 0 : '4px',
                    borderTopRightRadius: '4px',
                    borderBottomRightRadius: '4px',
                    background: card.rarity?.toLowerCase() === 'common' ? 'linear-gradient(135deg, #374151 0%, #1F2937 100%)' :
                              card.rarity?.toLowerCase() === 'uncommon' ? 'linear-gradient(135deg, #6B7280 0%, #4B5563 100%)' :
                              card.rarity?.toLowerCase() === 'rare' ? 'linear-gradient(135deg, #D97706 0%, #B45309 100%)' :
                              card.rarity?.toLowerCase() === 'mythic' ? 'linear-gradient(135deg, #DC2626 0%, #991B1B 100%)' :
                              card.rarity?.toLowerCase() === 'special' || card.rarity?.toLowerCase() === 'bonus' ? 'linear-gradient(135deg, #9333EA 0%, #7C3AED 100%)' :
                              'linear-gradient(135deg, #374151 0%, #1F2937 100%)',
                    color: 'white',
                    fontSize: '0.75rem',
                    fontFamily: 'monospace',
                    boxShadow: '0 1px 3px rgba(0, 0, 0, 0.3)'
                  }}
                >
                  <Typography variant="caption" sx={{ fontSize: 'inherit', fontFamily: 'inherit' }}>
                    #<span style={{ fontWeight: 'bold' }}>{card.collectorNumber}</span>
                  </Typography>
                </Box>
              )}
            </Box>
            {/* Date moved to top row */}
            {card.releasedAt && (!context.isOnSetPage || context.currentSetCode !== card.setCode) && (
              <Typography variant="caption" sx={{ fontSize: '0.625rem', color: 'grey.400' }}>
                {formatDate(card.releasedAt)}
              </Typography>
            )}
          </Box>

          {/* Artist(s) */}
          {displayArtists.length > 0 && (
            <Typography variant="caption" sx={{ fontSize: '0.75rem' }}>
              {displayArtists.map((artistName, index) => (
                <React.Fragment key={index}>
                  {index > 0 && <Typography component="span" sx={{ color: 'grey.500' }}> & </Typography>}
                  <Link
                    href={`/artists/${encodeURIComponent(artistName.toLowerCase().replace(/\s+/g, '-'))}`}
                    tabIndex={-1}
                    onClick={(e) => {
                      e.stopPropagation();
                      if (onArtistClick) {
                        e.preventDefault();
                        onArtistClick(artistName, card.artistIds?.[index]);
                      }
                    }}
                    sx={{
                      color: 'white',
                      textDecoration: 'none',
                      px: 0.5,
                      py: 0.25,
                      borderRadius: 1,
                      display: 'inline-block',
                      '&:hover': {
                        bgcolor: 'rgba(0, 0, 0, 1)',
                        color: 'primary.main',
                        textDecoration: 'none'
                      },
                      transition: 'all 0.2s ease'
                    }}
                    aria-label={`View cards by ${artistName}`}
                  >
                    {artistName}
                  </Link>
                </React.Fragment>
              ))}
            </Typography>
          )}

          {/* Card Name */}
          {!context.isOnCardPage && (
            <Box>
              <Link
                href={`/cards/${card.id}`}
                tabIndex={-1}
                onClick={(e) => {
                  e.stopPropagation();
                  if (onCardClick) {
                    e.preventDefault();
                    onCardClick(card.id);
                  }
                }}
                sx={{
                  color: 'white',
                  textDecoration: 'none',
                  display: 'inline-block',
                  px: 0.5,
                  py: 0.25,
                  borderRadius: 1,
                  '&:hover': {
                    bgcolor: 'rgba(0, 0, 0, 1)',
                    color: 'primary.main'
                  },
                  transition: 'all 0.2s ease'
                }}
                aria-label={`View all versions of ${card.name}`}
              >
                <Typography 
                  variant="subtitle2" 
                  component="span" 
                  sx={{ 
                    fontWeight: 'bold', 
                    lineHeight: 1.2,
                    fontSize: '0.875rem'
                  }}
                >
                  {card.name}
                </Typography>
              </Link>
            </Box>
          )}

          {/* Set Name with Icon */}
          {!context.isOnSetPage && card.setName && (
            <Box>
              <Link
                href={`/sets/${card.setCode?.toLowerCase()}`}
                tabIndex={-1}
                onClick={(e) => {
                  e.stopPropagation();
                  if (onSetClick) {
                    e.preventDefault();
                    onSetClick(card.setCode);
                  }
                }}
                sx={{
                  display: 'inline-flex',
                  alignItems: 'center',
                  gap: 0.5,
                  color: 'white',
                  textDecoration: 'none',
                  fontSize: '0.75rem',
                  px: 0.5,
                  py: 0.25,
                  borderRadius: 1,
                  '&:hover': {
                    bgcolor: 'rgba(0, 0, 0, 1)',
                    color: 'primary.main'
                  },
                  transition: 'all 0.2s ease'
                }}
                aria-label={`View all cards from ${card.setName}`}
              >
                {card.setCode && (
                  <SetIcon 
                    setCode={card.setCode} 
                    rarity={card.rarity}
                    size="small"
                    className="group-hover/set:opacity-100"
                  />
                )}
                <Typography 
                  variant="caption" 
                  sx={{ 
                    fontSize: 'inherit',
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                    whiteSpace: 'nowrap'
                  }}
                >
                  {card.setName}{card.setCode && ` (${card.setCode})`}
                </Typography>
              </Link>
            </Box>
          )}

          {/* Price and Links Row */}
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', pt: 0.5 }}>
            <PriceDisplay 
              price={card.prices?.usd} 
              currency="usd"
              className="text-sm"
            />
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
              <ExternalLinkIcon 
                type="scryfall" 
                url={card.scryfallUri} 
                size="small"
              />
              <ExternalLinkIcon 
                type="tcgplayer" 
                url={card.purchaseUris?.tcgplayer || `https://www.tcgplayer.com/search/magic/product?productLineName=magic&q=${encodeURIComponent(card.name || '')}`}
                size="small"
              />
            </Box>
          </Box>
        </Box>
      </Box>

      {/* Enlarged Card Modal */}
      <Modal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        sx={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          bgcolor: 'rgba(0, 0, 0, 0.8)'
        }}
        onClick={() => setModalOpen(false)}
      >
        <Box
          sx={{
            outline: 'none',
            maxWidth: '90vw',
            maxHeight: '90vh',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center'
          }}
          onClick={(e) => e.stopPropagation()}
        >
          <CardMedia
            component="img"
            image={card.imageUris?.large || card.imageUris?.png || card.imageUris?.normal || ''}
            alt={card.name}
            sx={{
              maxWidth: '100%',
              maxHeight: '100%',
              width: 'auto',
              height: 'auto',
              borderRadius: 12,
              boxShadow: '0 25px 50px -12px rgba(0, 0, 0, 0.8)',
              cursor: 'default'
            }}
            onClick={() => setModalOpen(false)}
          />
        </Box>
      </Modal>
    </MuiCard>
  );
};