import React from 'react';
import { Box, Typography, useTheme } from '../../../atoms';

/** Card width in pixels (used for scroll calculations) */
export const SET_PREVIEW_CARD_WIDTH = 180;

interface SetPreviewCardProps {
  code: string;
  name: string;
  cardCount: number;
  releasedAt: string;
  iconSvgUri: string;
  onClick: (code: string) => void;
}

export const SetPreviewCard: React.FC<SetPreviewCardProps> = React.memo(({
  code,
  name,
  cardCount,
  releasedAt,
  iconSvgUri,
  onClick,
}) => {
  const theme = useTheme();

  const formattedDate = new Date(releasedAt).toLocaleDateString('en-US', {
    month: 'short',
    year: 'numeric',
  });

  return (
    <Box
      onClick={() => onClick(code)}
      sx={{
        width: { xs: 160, sm: 180 },
        flexShrink: 0,
        cursor: 'pointer',
        bgcolor: 'background.paper',
        borderRadius: 2,
        border: 1,
        borderColor: 'divider',
        p: 2,
        display: 'flex',
        flexDirection: 'column',
        gap: 1.5,
        transition: theme.mtg.transitions.card,
        '&:hover': {
          transform: 'scale(1.02)',
          boxShadow: theme.mtg.shadows.card.hover,
        },
      }}
    >
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          height: 40,
        }}
      >
        <img
          src={iconSvgUri}
          alt={`${name} set icon`}
          style={{ width: 40, height: 40 }}
        />
      </Box>

      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
        <Typography
          variant="subtitle2"
          sx={{
            fontWeight: 600,
            lineHeight: 1.3,
            overflow: 'hidden',
            textOverflow: 'ellipsis',
            display: '-webkit-box',
            WebkitLineClamp: 2,
            WebkitBoxOrient: 'vertical',
          }}
        >
          {name}
        </Typography>

        <Typography variant="caption" color="text.secondary">
          {cardCount} cards
        </Typography>

        <Typography variant="caption" color="text.secondary">
          {formattedDate}
        </Typography>
      </Box>
    </Box>
  );
});
