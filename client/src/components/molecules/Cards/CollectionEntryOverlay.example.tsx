import React, { useState } from 'react';
import { Box, Button, Stack, Typography, Divider } from '@mui/material';
import { CollectionEntryOverlay } from './CollectionEntryOverlay';
import type { CardFinish, CardSpecial } from '../../../types/collection';

/**
 * Example demonstrating CollectionEntryOverlay usage for both card and sealed variants
 * across different sizes. This shows how the component intelligently scales based on
 * container size while always showing all critical information.
 */

interface CardDemoProps {
  width: number;
  height: number;
  title: string;
}

const CardDemo: React.FC<CardDemoProps> = ({ width, height, title }) => {
  const [overlayState, setOverlayState] = useState({
    visible: false,
    count: 0,
    isNegative: false,
    finish: 'non-foil' as CardFinish,
    special: 'none' as CardSpecial,
    mode: 'collection' as 'collection' | 'wishlist',
    flash: false
  });

  const showOverlay = (count: number, isNegative = false) => {
    setOverlayState({
      ...overlayState,
      visible: true,
      count,
      isNegative
    });
  };

  const hideOverlay = () => {
    setOverlayState({ ...overlayState, visible: false });
  };

  const toggleMode = () => {
    setOverlayState({
      ...overlayState,
      mode: overlayState.mode === 'collection' ? 'wishlist' : 'collection'
    });
  };

  const toggleFinish = () => {
    const finishes: CardFinish[] = ['non-foil', 'foil', 'etched'];
    const currentIndex = finishes.indexOf(overlayState.finish);
    const nextFinish = finishes[(currentIndex + 1) % finishes.length];
    setOverlayState({ ...overlayState, finish: nextFinish });
  };

  const toggleSpecial = () => {
    const specials: CardSpecial[] = ['none', 'signed', 'artist-proof', 'altered'];
    const currentIndex = specials.indexOf(overlayState.special);
    const nextSpecial = specials[(currentIndex + 1) % specials.length];
    setOverlayState({ ...overlayState, special: nextSpecial });
  };

  const flashError = () => {
    setOverlayState({ ...overlayState, flash: true, visible: true });
    setTimeout(() => {
      setOverlayState(s => ({ ...s, flash: false }));
    }, 150);
  };

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
      <Typography variant="h6">{title}</Typography>
      <Typography variant="caption" color="text.secondary">
        Notice how finish and special are ALWAYS visible, just scaled down
      </Typography>

      {/* Mock Card */}
      <Box
        sx={{
          position: 'relative',
          width: `${width}px`,
          height: `${height}px`,
          bgcolor: 'grey.800',
          borderRadius: 2,
          overflow: 'hidden',
          border: '2px solid',
          borderColor: 'grey.700',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center'
        }}
      >
        <Typography variant="body2" color="text.secondary">
          {width} × {height}px
        </Typography>

        {/* Card Variant Overlay */}
        <CollectionEntryOverlay
          visible={overlayState.visible}
          count={overlayState.count}
          isNegative={overlayState.isNegative}
          mode={overlayState.mode}
          variant="card"
          finish={overlayState.finish}
          special={overlayState.special}
          flash={overlayState.flash}
        />
      </Box>

      {/* Controls */}
      <Stack direction="row" spacing={1} flexWrap="wrap">
        <Button size="small" onClick={() => showOverlay(1)}>
          Show 1
        </Button>
        <Button size="small" onClick={() => showOverlay(3)}>
          Show 3
        </Button>
        <Button size="small" onClick={() => showOverlay(9)}>
          Show 9
        </Button>
        <Button size="small" onClick={() => showOverlay(2, true)}>
          Show -2
        </Button>
        <Button size="small" onClick={hideOverlay}>
          Hide
        </Button>
        <Button size="small" onClick={flashError} color="error">
          Flash
        </Button>
      </Stack>

      <Stack direction="row" spacing={1} flexWrap="wrap">
        <Button size="small" variant="outlined" onClick={toggleMode}>
          Mode: {overlayState.mode}
        </Button>
        <Button size="small" variant="outlined" onClick={toggleFinish}>
          Finish: {overlayState.finish}
        </Button>
        <Button size="small" variant="outlined" onClick={toggleSpecial}>
          Special: {overlayState.special}
        </Button>
      </Stack>
    </Box>
  );
};

interface SealedDemoProps {
  width: number;
  height: number;
  title: string;
}

const SealedDemo: React.FC<SealedDemoProps> = ({ width, height, title }) => {
  const [overlayState, setOverlayState] = useState({
    visible: false,
    count: 0,
    isNegative: false,
    mode: 'collection' as 'collection' | 'wishlist',
    flash: false
  });

  const showOverlay = (count: number, isNegative = false) => {
    setOverlayState({
      ...overlayState,
      visible: true,
      count,
      isNegative
    });
  };

  const hideOverlay = () => {
    setOverlayState({ ...overlayState, visible: false });
  };

  const toggleMode = () => {
    setOverlayState({
      ...overlayState,
      mode: overlayState.mode === 'collection' ? 'wishlist' : 'collection'
    });
  };

  const flashError = () => {
    setOverlayState({ ...overlayState, flash: true, visible: true });
    setTimeout(() => {
      setOverlayState(s => ({ ...s, flash: false }));
    }, 150);
  };

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
      <Typography variant="h6">{title}</Typography>
      <Typography variant="caption" color="text.secondary">
        Sealed products only track quantity, no finish/special
      </Typography>

      {/* Mock Sealed Product */}
      <Box
        sx={{
          position: 'relative',
          width: `${width}px`,
          height: `${height}px`,
          bgcolor: 'grey.800',
          borderRadius: 2,
          overflow: 'hidden',
          border: '2px solid',
          borderColor: 'grey.700',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center'
        }}
      >
        <Typography variant="body2" color="text.secondary">
          {width} × {height}px
        </Typography>

        {/* Sealed Variant Overlay */}
        <CollectionEntryOverlay
          visible={overlayState.visible}
          count={overlayState.count}
          isNegative={overlayState.isNegative}
          mode={overlayState.mode}
          variant="sealed"
          flash={overlayState.flash}
        />
      </Box>

      {/* Controls */}
      <Stack direction="row" spacing={1} flexWrap="wrap">
        <Button size="small" onClick={() => showOverlay(1)}>
          Show 1
        </Button>
        <Button size="small" onClick={() => showOverlay(3)}>
          Show 3
        </Button>
        <Button size="small" onClick={() => showOverlay(5)}>
          Show 5
        </Button>
        <Button size="small" onClick={() => showOverlay(1, true)}>
          Show -1
        </Button>
        <Button size="small" onClick={hideOverlay}>
          Hide
        </Button>
        <Button size="small" onClick={flashError} color="error">
          Flash
        </Button>
      </Stack>

      <Stack direction="row" spacing={1} flexWrap="wrap">
        <Button size="small" variant="outlined" onClick={toggleMode}>
          Mode: {overlayState.mode}
        </Button>
      </Stack>
    </Box>
  );
};

/**
 * Main demo component showing overlay for both variants across different sizes
 */
export const CollectionEntryOverlayDemo: React.FC = () => {
  return (
    <Box sx={{ p: 4, bgcolor: 'background.default', minHeight: '100vh' }}>
      <Typography variant="h4" gutterBottom>
        CollectionEntryOverlay Demo
      </Typography>
      <Typography variant="body1" color="text.secondary" paragraph>
        Demonstrates the overlay for both card and sealed variants across different sizes.
        Key principle: NEVER hide critical information, just scale it down.
      </Typography>

      {/* Card Variant Section */}
      <Typography variant="h5" gutterBottom sx={{ mt: 4 }}>
        Card Variant (variant="card")
      </Typography>
      <Typography variant="body2" color="text.secondary" paragraph>
        Always shows finish and special. Notice how the text scales down on smaller cards
        but NEVER disappears. Users MUST see this information to add the correct card.
      </Typography>

      <Stack spacing={6}>
        {/* Tiny Binder Card */}
        <CardDemo
          width={150}
          height={210}
          title="Tiny Binder Card (150×210) - Text scales down"
        />

        {/* Small Binder Card */}
        <CardDemo
          width={200}
          height={280}
          title="Small Binder Card (200×280) - Text scales down"
        />

        {/* Medium Card */}
        <CardDemo
          width={250}
          height={350}
          title="Medium Card (250×350) - Balanced sizing"
        />

        {/* Regular Card */}
        <CardDemo
          width={300}
          height={418}
          title="Regular Card (300×418) - Standard MTG aspect ratio"
        />

        {/* Large Card */}
        <CardDemo
          width={400}
          height={558}
          title="Large Card (400×558) - Generous spacing"
        />
      </Stack>

      <Divider sx={{ my: 6 }} />

      {/* Sealed Variant Section */}
      <Typography variant="h5" gutterBottom>
        Sealed Variant (variant="sealed")
      </Typography>
      <Typography variant="body2" color="text.secondary" paragraph>
        Simpler layout for sealed products. Only shows mode and count since sealed products
        don't have finish or special attributes.
      </Typography>

      <Stack spacing={6}>
        {/* Small Sealed Product */}
        <SealedDemo
          width={200}
          height={200}
          title="Small Sealed Product (200×200)"
        />

        {/* Medium Sealed Product */}
        <SealedDemo
          width={300}
          height={300}
          title="Medium Sealed Product (300×300)"
        />

        {/* Large Sealed Product */}
        <SealedDemo
          width={400}
          height={400}
          title="Large Sealed Product (400×400)"
        />
      </Stack>

      {/* Usage Instructions */}
      <Box sx={{ mt: 6, p: 3, bgcolor: 'grey.900', borderRadius: 2 }}>
        <Typography variant="h6" gutterBottom>
          Integration Notes
        </Typography>
        <Typography variant="body2" component="div" sx={{ whiteSpace: 'pre-line' }}>
          {`
Key Principles:

1. Use variant="card" for MTG cards (ALWAYS provide finish)
2. Use variant="sealed" for sealed products (NO finish/special props)
3. Don't switch variants based on size - the component handles scaling
4. Always show critical information - never hide finish/special

Card Integration:

<Box sx={{ position: 'relative' }}>
  <CardImage src={card.imageUrl} />
  <CollectionEntryOverlay
    visible={overlayVisible}
    count={entryCount}
    isNegative={isRemoving}
    mode={entryMode}
    variant="card"
    finish={selectedFinish}
    special={selectedSpecial}
    flash={hasError}
  />
</Box>

Sealed Product Integration:

<Box sx={{ position: 'relative' }}>
  <ProductImage src={product.imageUrl} />
  <CollectionEntryOverlay
    visible={overlayVisible}
    count={entryCount}
    isNegative={isRemoving}
    mode={entryMode}
    variant="sealed"
    flash={hasError}
  />
</Box>

The component automatically scales text and spacing based on screen breakpoints.
No need for manual size detection!
          `}
        </Typography>
      </Box>
    </Box>
  );
};

/**
 * Keyboard shortcut integration example for card variant
 */
export const useCardCollectionEntryOverlay = () => {
  const [state, setState] = useState({
    visible: false,
    count: 0,
    isNegative: false,
    finish: 'non-foil' as CardFinish,
    special: 'none' as CardSpecial,
    mode: 'collection' as 'collection' | 'wishlist',
    flash: false
  });

  const handleKeyPress = (key: string) => {
    const num = parseInt(key);
    if (!isNaN(num) && num >= 1 && num <= 9) {
      setState(s => ({
        ...s,
        visible: true,
        count: num,
        isNegative: false
      }));
    } else if (key === '-') {
      setState(s => ({ ...s, isNegative: !s.isNegative }));
    } else if (key === 'Escape') {
      setState(s => ({ ...s, visible: false }));
    } else if (key === 'f') {
      const finishes: CardFinish[] = ['non-foil', 'foil', 'etched'];
      const idx = finishes.indexOf(state.finish);
      const next = finishes[(idx + 1) % finishes.length];
      setState(s => ({ ...s, finish: next }));
    } else if (key === 's') {
      const specials: CardSpecial[] = ['none', 'signed', 'artist-proof', 'altered'];
      const idx = specials.indexOf(state.special);
      const next = specials[(idx + 1) % specials.length];
      setState(s => ({ ...s, special: next }));
    } else if (key === 'm') {
      setState(s => ({
        ...s,
        mode: s.mode === 'collection' ? 'wishlist' : 'collection'
      }));
    }
  };

  const showError = () => {
    setState(s => ({ ...s, flash: true, visible: true }));
    setTimeout(() => {
      setState(s => ({ ...s, flash: false }));
    }, 150);
  };

  const hide = () => {
    setState(s => ({ ...s, visible: false }));
  };

  return {
    overlayState: state,
    handleKeyPress,
    showError,
    hide
  };
};

/**
 * Keyboard shortcut integration example for sealed variant
 */
export const useSealedCollectionEntryOverlay = () => {
  const [state, setState] = useState({
    visible: false,
    count: 0,
    isNegative: false,
    mode: 'collection' as 'collection' | 'wishlist',
    flash: false
  });

  const handleKeyPress = (key: string) => {
    const num = parseInt(key);
    if (!isNaN(num) && num >= 1 && num <= 9) {
      setState(s => ({
        ...s,
        visible: true,
        count: num,
        isNegative: false
      }));
    } else if (key === '-') {
      setState(s => ({ ...s, isNegative: !s.isNegative }));
    } else if (key === 'Escape') {
      setState(s => ({ ...s, visible: false }));
    } else if (key === 'm') {
      setState(s => ({
        ...s,
        mode: s.mode === 'collection' ? 'wishlist' : 'collection'
      }));
    }
  };

  const showError = () => {
    setState(s => ({ ...s, flash: true, visible: true }));
    setTimeout(() => {
      setState(s => ({ ...s, flash: false }));
    }, 150);
  };

  const hide = () => {
    setState(s => ({ ...s, visible: false }));
  };

  return {
    overlayState: state,
    handleKeyPress,
    showError,
    hide
  };
};
