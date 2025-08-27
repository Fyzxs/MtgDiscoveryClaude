import { createTheme, alpha } from '@mui/material/styles';

declare module '@mui/material/styles' {
  interface Palette {
    rarity: {
      common: string;
      uncommon: string;
      rare: string;
      mythic: string;
      special: string;
      bonus: string;
      timeshifted: string;
    };
    legality: {
      legal: string;
      notLegal: string;
      restricted: string;
      banned: string;
    };
    mtg: {
      cardBorder: string;
      cardOverlayGradient: string;
      selectedGlow: string;
      hoverShadow: string;
    };
  }
  
  interface PaletteOptions {
    rarity?: {
      common?: string;
      uncommon?: string;
      rare?: string;
      mythic?: string;
      special?: string;
      bonus?: string;
      timeshifted?: string;
    };
    legality?: {
      legal?: string;
      notLegal?: string;
      restricted?: string;
      banned?: string;
    };
    mtg?: {
      cardBorder?: string;
      cardOverlayGradient?: string;
      selectedGlow?: string;
      hoverShadow?: string;
    };
  }
  
  interface Theme {
    mtg: {
      spacing: {
        cardGap: number;
        filterGap: number;
        sectionGap: number;
      };
      dimensions: {
        cardWidth: {
          xs: string;
          sm: string;
          md: string;
          lg: string;
        };
        cardAspectRatio: string;
      };
      transitions: {
        card: string;
        filter: string;
      };
      gradients: {
        header: string;
        footer: string;
        cardOverlay: string;
        hover: string;
      };
      shadows: {
        card: {
          normal: string;
          hover: string;
          selected: string;
        };
        rarity: {
          common: string;
          uncommon: string;
          rare: string;
          mythic: string;
        };
      };
    };
  }
  
  interface ThemeOptions {
    mtg?: {
      spacing?: {
        cardGap?: number;
        filterGap?: number;
        sectionGap?: number;
      };
      dimensions?: {
        cardWidth?: {
          xs?: string;
          sm?: string;
          md?: string;
          lg?: string;
        };
        cardAspectRatio?: string;
      };
      transitions?: {
        card?: string;
        filter?: string;
      };
      gradients?: {
        header?: string;
        footer?: string;
        cardOverlay?: string;
        hover?: string;
      };
      shadows?: {
        card?: {
          normal?: string;
          hover?: string;
          selected?: string;
        };
        rarity?: {
          common?: string;
          uncommon?: string;
          rare?: string;
          mythic?: string;
        };
      };
    };
  }
}

export const theme = createTheme({
  palette: {
    mode: 'dark',
    primary: {
      main: '#1976d2',
      light: '#42a5f5',
      dark: '#1565c0',
    },
    secondary: {
      main: '#9c27b0',
      light: '#ba68c8',
      dark: '#7b1fa2',
    },
    background: {
      default: '#0a0e1a',
      paper: '#1a1f2e',
    },
    text: {
      primary: '#ffffff',
      secondary: 'rgba(255, 255, 255, 0.7)',
      disabled: 'rgba(255, 255, 255, 0.5)',
    },
    // MTG Rarity colors
    rarity: {
      common: '#424242',      // Gray for common
      uncommon: '#9e9e9e',    // Silver for uncommon
      rare: '#ffc107',        // Gold for rare
      mythic: '#ff6f00',      // Orange for mythic
      special: '#9c27b0',     // Purple for special
      bonus: '#e91e63',       // Pink for bonus
      timeshifted: '#673ab7', // Deep purple for timeshifted
    },
    // MTG Legality colors
    legality: {
      legal: '#4CAF50',       // Green
      notLegal: '#424242',    // Gray
      restricted: '#2196F3',  // Blue
      banned: '#F44336',      // Red
    },
    // Additional MTG-specific colors
    mtg: {
      cardBorder: 'rgba(255, 255, 255, 0.12)',
      cardOverlayGradient: 'linear-gradient(to top, rgba(0,0,0,1) 0%, rgba(0,0,0,0.95) 60%, rgba(0,0,0,0) 100%)',
      selectedGlow: '#2196F3',
      hoverShadow: 'rgba(33, 150, 243, 0.2)',
    },
  },
  typography: {
    fontFamily: '"Roboto", "Helvetica", "Arial", sans-serif',
    h1: {
      fontSize: '2.5rem',
      fontWeight: 700,
    },
    h2: {
      fontSize: '2rem',
      fontWeight: 600,
    },
    h3: {
      fontSize: '1.75rem',
      fontWeight: 600,
    },
    h4: {
      fontSize: '1.5rem',
      fontWeight: 600,
    },
    h5: {
      fontSize: '1.25rem',
      fontWeight: 600,
    },
    h6: {
      fontSize: '1rem',
      fontWeight: 600,
    },
    body1: {
      fontSize: '1rem',
    },
    body2: {
      fontSize: '0.875rem',
    },
    caption: {
      fontSize: '0.75rem',
    },
  },
  shape: {
    borderRadius: 8,
  },
  spacing: 8,
  components: {
    MuiCard: {
      styleOverrides: {
        root: {
          backgroundImage: 'none',
          backgroundColor: 'transparent',
        },
      },
    },
    MuiChip: {
      styleOverrides: {
        root: {
          borderRadius: 16,
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: 'none',
          borderRadius: 8,
        },
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: {
          '& .MuiOutlinedInput-root': {
            borderRadius: 8,
          },
        },
      },
    },
    MuiFab: {
      styleOverrides: {
        root: {
          boxShadow: '0 4px 14px rgba(0, 0, 0, 0.4)',
        },
      },
    },
  },
  // Custom MTG-specific theme extensions
  mtg: {
    spacing: {
      cardGap: 1.5,  // 12px
      filterGap: 2,   // 16px
      sectionGap: 4,  // 32px
    },
    dimensions: {
      cardWidth: {
        xs: '150px',
        sm: '180px',
        md: '200px',
        lg: '250px',
      },
      cardAspectRatio: '1.395', // Standard MTG card ratio (2.5" x 3.5")
    },
    transitions: {
      card: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
      filter: 'all 0.2s ease-in-out',
    },
    gradients: {
      header: 'linear-gradient(45deg, #1976d2 30%, #42a5f5 90%)',
      footer: 'linear-gradient(45deg, #1976d2 30%, #42a5f5 90%)',
      cardOverlay: 'linear-gradient(to top, rgba(0,0,0,1) 0%, rgba(0,0,0,0.95) 60%, rgba(0,0,0,0) 100%)',
      hover: 'linear-gradient(45deg, #1565c0 30%, #1976d2 90%)',
    },
    shadows: {
      card: {
        normal: '0 4px 14px rgba(0, 0, 0, 0.4)',
        hover: '0 12px 24px rgba(0, 0, 0, 0.6)',
        selected: '0 0 60px rgba(25, 118, 210, 1), 0 0 40px rgba(33, 150, 243, 0.8), 0 0 20px rgba(33, 150, 243, 0.6)',
      },
      rarity: {
        common: `0 0 10px ${alpha('#424242', 0.5)}`,
        uncommon: `0 0 10px ${alpha('#9e9e9e', 0.5)}`,
        rare: `0 0 10px ${alpha('#ffc107', 0.5)}`,
        mythic: `0 0 15px ${alpha('#ff6f00', 0.7)}`,
      },
    },
  },
});

// Helper function to get rarity color
export const getRarityColor = (rarity?: string): string => {
  if (!rarity) return theme.palette.rarity.common;
  
  const rarityLower = rarity.toLowerCase();
  switch (rarityLower) {
    case 'common':
      return theme.palette.rarity.common;
    case 'uncommon':
      return theme.palette.rarity.uncommon;
    case 'rare':
      return theme.palette.rarity.rare;
    case 'mythic':
      return theme.palette.rarity.mythic;
    case 'special':
      return theme.palette.rarity.special;
    case 'bonus':
      return theme.palette.rarity.bonus;
    case 'timeshifted':
      return theme.palette.rarity.timeshifted;
    default:
      return theme.palette.rarity.common;
  }
};

// Helper function to get rarity shadow
export const getRarityShadow = (rarity?: string): string => {
  if (!rarity) return theme.mtg.shadows.rarity.common;
  
  const rarityLower = rarity.toLowerCase();
  switch (rarityLower) {
    case 'mythic':
      return theme.mtg.shadows.rarity.mythic;
    case 'rare':
      return theme.mtg.shadows.rarity.rare;
    case 'uncommon':
      return theme.mtg.shadows.rarity.uncommon;
    default:
      return theme.mtg.shadows.rarity.common;
  }
};

// Helper function to get legality color
export const getLegalityColor = (legality?: string): string => {
  if (!legality) return theme.palette.legality.notLegal;
  
  const legalityLower = legality.toLowerCase();
  switch (legalityLower) {
    case 'legal':
      return theme.palette.legality.legal;
    case 'not_legal':
      return theme.palette.legality.notLegal;
    case 'restricted':
      return theme.palette.legality.restricted;
    case 'banned':
      return theme.palette.legality.banned;
    default:
      return theme.palette.legality.notLegal;
  }
};

export default theme;