# Frontend CLAUDE.md

This file provides comprehensive guidance for working with the React frontend of the MTG Discovery application.

## Architecture Overview

The frontend is a React 19 application built with Material-UI, Apollo Client, and TypeScript, following atomic design principles for component organization. It serves as the UI layer for a Magic: The Gathering card collection management platform.

### Technology Stack

- **React 19** - Latest React with concurrent features
- **Material-UI v7** - Primary UI component library with custom MTG theme
- **Apollo Client v4** - GraphQL client with caching
- **Auth0** - Authentication and authorization
- **TypeScript** - Type safety throughout
- **Vite** - Build tool and development server
- **React Router DOM v7** - Client-side routing

## Component Architecture (Atomic Design)

The application follows atomic design principles with a clear component hierarchy:

### Directory Structure
```
src/components/
├── atoms/           # Basic building blocks
│   ├── Cards/       # Card-specific atoms (RarityBadge, ManaSymbol, etc.)
│   ├── Sets/        # Set-specific atoms (SetIcon, SetCodeBadge, etc.)
│   ├── layouts/     # Layout atoms (ResponsiveGrid)
│   └── shared/      # Reusable atoms (AppButton, AppCard, AppInput)
├── molecules/       # Component combinations
│   ├── Cards/       # Card molecules (CardImageDisplay, ManaCost, etc.)
│   ├── Sets/        # Set molecules
│   └── shared/      # Shared molecules
├── organisms/       # Complex components
├── templates/       # Page layout structures
├── pages/           # Complete page implementations
└── auth/            # Authentication components
```

### Component Naming Conventions

1. **App* Prefix for Wrapped Components**: Custom components that wrap Material-UI components use "App" prefix
   - `AppButton` wraps `Button` with loading state
   - `AppCard` wraps `Card` with consistent styling
   - `AppInput` wraps `TextField` with validation

2. **Domain-Specific Naming**: Components are named by their purpose, not implementation
   - `RarityBadge` displays card rarity with color coding
   - `ManaCost` renders mana symbols with proper styling
   - `CardImageDisplay` handles card images with zoom and flip functionality

3. **Context-Aware Components**: Many components accept `CardContext` for conditional display
   - `isOnSetPage` - Hide set information when viewing a specific set
   - `isOnArtistPage` - Hide artist information when viewing artist's cards
   - `showCollectorInfo` - Show/hide collector-specific information

## Material-UI Usage Patterns

### Theme System

The application uses a heavily customized Material-UI theme with MTG-specific extensions:

```typescript
// Theme extensions for MTG-specific styling
theme.palette.rarity.*    // MTG rarity colors (common, uncommon, rare, mythic)
theme.palette.legality.*  // Format legality colors
theme.mtg.spacing.*       // MTG-specific spacing values
theme.mtg.dimensions.*    // Card dimensions and aspect ratios
theme.mtg.shadows.*       // Rarity-based shadows and glows
theme.mtg.gradients.*     // Custom gradients for cards and UI
```

### Styling Patterns

1. **sx Props Over className**: Use Material-UI's `sx` prop for component-specific styling
   ```typescript
   <Box sx={{ bgcolor: 'grey.900', borderRadius: 2 }} />
   ```

2. **Theme Reference**: Always reference theme values for consistency
   ```typescript
   sx={{ color: 'rarity.mythic', boxShadow: theme.mtg.shadows.card.hover }}
   ```

3. **Responsive Design**: Use breakpoint objects for responsive styling
   ```typescript
   sx={{ width: { xs: '100%', sm: 'auto' }, fontSize: { xs: '0.8rem', md: '1rem' } }}
   ```

4. **Layout Helpers**: Use predefined layout styles from `styles/layoutStyles.ts`
   ```typescript
   import { flexBetween, centeredContainer } from '../styles/layoutStyles';
   sx={{ ...flexBetween, gap: 2 }}
   ```

### Component Override Patterns

Global component overrides in theme configuration:
- `MuiCard` - Transparent background for glass-morphism effects
- `MuiButton` - No text transform, consistent border radius
- `MuiChip` - Rounded corners for badge-like appearance

## GraphQL/Apollo Integration

### Code Generation

GraphQL types and hooks are automatically generated using GraphQL Code Generator:

```bash
npm run codegen        # Generate types from schema
npm run codegen:watch  # Watch mode for development
```

Configuration in `codegen.ts`:
- Schema: `https://localhost:65203/graphql`
- Documents: All `.ts` and `.tsx` files in `src/`
- Output: `src/generated/` directory

### Apollo Client Setup

The Apollo Client is configured with:

1. **Authentication**: Automatic JWT token injection via Auth0
2. **Caching**: InMemoryCache with custom merge policies for pagination
3. **Error Handling**: Global error policies for graceful degradation

```typescript
// Generated hooks usage
import { useCardsQuery, useSetQuery } from '../generated/graphql';

const { data, loading, error } = useCardsQuery({
  variables: { filters: { setCode } }
});
```

### Authentication Flow

Auth0 integration with JWT token management:

1. **Auth0Provider** - Wraps app with authentication context
2. **Auth0TokenProvider** - Bridges Auth0 tokens to Apollo Client
3. **UserContext** - Manages user profile and collector data
4. **Automatic Headers** - JWT tokens automatically attached to GraphQL requests

## State Management Approaches

### Context Pattern

The application uses React Context for global state:

1. **UserContext** - Authentication and user profile state
   ```typescript
   const { userProfile, isAuthenticated, syncUser } = useUser();
   ```

2. **CardContext** - Contextual information for card display
   ```typescript
   const context: CardContext = {
     isOnSetPage: true,
     showCollectorInfo: hasCollector,
     currentSet: setCode
   };
   ```

### Local State Patterns

1. **Component State** - useState for component-specific state
2. **Form State** - Controlled components with validation
3. **Loading States** - Separate loading states for different operations

## Routing Structure

React Router DOM v7 with error boundaries per route:

```typescript
<Routes>
  <Route path="/" element={<PageErrorBoundary name="HomePage"><HomePage /></PageErrorBoundary>} />
  <Route path="/sets" element={<PageErrorBoundary name="AllSetsPage"><AllSetsPage /></PageErrorBoundary>} />
  <Route path="/set/:setCode" element={<PageErrorBoundary name="SetPage"><SetPage /></PageErrorBoundary>} />
  // ... other routes
</Routes>
```

### Route Patterns

- `/sets` - All MTG sets listing
- `/set/:setCode` - Individual set with cards
- `/search/cards` - Card search functionality
- `/search/artists` - Artist search
- `/artists/:artistName` - Artist's cards
- `/card/:cardName` - All printings of a card

### Legacy Redirect Handling

Backward compatibility for old query parameter URLs:
- `?page=all-sets` → `/sets`
- `?page=set&set=CODE` → `/set/CODE`

## Development Workflows

### Environment Setup

```bash
# Install dependencies
npm install

# Start development server
npm run dev

# Build for production
npm run build

# Run linting
npm run lint

# Preview production build
npm run preview
```

### Environment Variables

Required environment variables (in `.env.local`):
```
VITE_AUTH0_DOMAIN=your-auth0-domain
VITE_AUTH0_CLIENT_ID=your-auth0-client-id
VITE_AUTH0_REDIRECT_URI=http://localhost:5173/signin-redirect
```

### Development Server Configuration

Vite configuration includes:
- WSL2 polling for file watching
- GraphQL proxy to backend server
- Self-signed certificate support

## Key UI Patterns

### MTG-Specific Patterns

1. **Rarity Color Coding**: All card elements use consistent rarity colors
   ```typescript
   const rarityColor = getRarityColor(card.rarity);
   const rarityShadow = getRarityShadow(card.rarity);
   ```

2. **Card Aspect Ratio**: Standard MTG card ratio (1.395) maintained across all card displays

3. **Mana Symbol Rendering**: Automatic parsing and display of mana costs with proper symbols

4. **Set Symbol Integration**: Official MTG set symbols with fallback text

### Responsive Design Patterns

1. **Mobile-First Approach**: All components designed mobile-first with desktop enhancements

2. **Adaptive Layouts**:
   - Horizontal card layout on mobile
   - Vertical card layout on desktop
   - Grid layouts that adapt to screen size

3. **Responsive Grid System**: Custom `ResponsiveGrid` component using CSS Grid
   ```typescript
   <ResponsiveGrid minItemWidth={250} spacing={3}>
     {cards.map(card => <CardDisplay key={card.id} card={card} />)}
   </ResponsiveGrid>
   ```

### Card Display Patterns

1. **CardDisplay** (Responsive) - Full card information with hover effects
2. **CardCompact** - Condensed card view for grids
3. **CardImageDisplay** - Card image with zoom and flip functionality

### Context-Aware Display

Components adapt based on viewing context:
- Hide set information when on set page
- Hide artist information when on artist page
- Show/hide collector information based on user state

## Performance Optimizations

### React Performance

1. **React.memo** - Memoize expensive card components
2. **useMemo/useCallback** - Optimize expensive calculations and event handlers
3. **Lazy Loading** - Code splitting for route-based components

### Apollo Client Optimizations

1. **Cache Policies** - Appropriate fetch policies for different data types
2. **Query Batching** - Automatic query batching for multiple requests
3. **Normalized Cache** - Efficient cache updates with normalized data

### Image Optimization

1. **Multiple Image Sizes** - Scryfall provides multiple resolutions
2. **Lazy Loading** - Images load as they enter viewport
3. **Error Fallbacks** - Graceful handling of missing images

## Accessibility Features

### ARIA Implementation

1. **Semantic HTML** - Proper heading hierarchy and landmarks
2. **ARIA Labels** - Descriptive labels for interactive elements
3. **Focus Management** - Proper tab order and focus indicators
4. **Skip Navigation** - Skip links for keyboard users

### Screen Reader Support

1. **Alt Text** - Descriptive alt text for card images
2. **Live Regions** - Announcements for dynamic content updates
3. **Accessible Forms** - Proper form labeling and validation

## Error Handling

### Error Boundary Pattern

Multi-level error boundaries:
1. **Root Level** - Catches application-wide errors
2. **Page Level** - Catches route-specific errors
3. **Component Level** - Catches component-specific errors

### GraphQL Error Handling

1. **Apollo Error Policies** - Graceful degradation on errors
2. **Error UI Components** - User-friendly error displays
3. **Retry Mechanisms** - Automatic retry for transient errors

## Testing Patterns

### Component Testing

1. **Props Interface Testing** - Verify all required props
2. **Interaction Testing** - User event simulation
3. **Accessibility Testing** - ARIA and keyboard navigation

### Integration Testing

1. **Apollo Client Mocking** - Mock GraphQL responses
2. **Auth0 Mocking** - Mock authentication state
3. **Router Testing** - Navigation and route parameter testing

## Code Style Guidelines

### Import Organization

```typescript
// 1. React and external libraries
import React from 'react';
import { Box, Typography } from '@mui/material';

// 2. Internal types
import type { Card, CardContext } from '../../types/card';

// 3. Components (atoms → molecules → organisms)
import { CardImage } from '../atoms/Cards/CardImage';
import { CardMetadata } from '../molecules/Cards/CardMetadata';

// 4. Hooks and utilities
import { useCardData } from '../../hooks/useCardData';
```

### TypeScript Patterns

1. **Interface Definitions** - Explicit props interfaces for all components
2. **Type Guards** - Runtime type checking where needed
3. **Generic Components** - Reusable components with proper typing

### Component Structure

```typescript
interface ComponentProps {
  // Props interface
}

export const Component: React.FC<ComponentProps> = ({
  // Destructured props with defaults
}) => {
  // Hooks
  // Local state
  // Event handlers
  // Render helpers

  return (
    // JSX
  );
};
```

## Best Practices

### Component Design

1. **Single Responsibility** - Each component has one clear purpose
2. **Composition Over Inheritance** - Prefer composition patterns
3. **Props Interface** - Always define explicit props interfaces
4. **Default Props** - Provide sensible defaults for optional props

### State Management

1. **Lift State Up** - Move state to common ancestor when needed
2. **Local State First** - Start with local state, extract to context when needed
3. **Immutable Updates** - Always create new objects for state updates

### Performance

1. **Avoid Inline Objects** - Extract objects and functions to prevent re-renders
2. **Memoize Expensive Calculations** - Use useMemo for complex computations
3. **Debounce User Input** - Prevent excessive API calls

### Accessibility

1. **Keyboard Navigation** - Ensure all interactive elements are keyboard accessible
2. **Color Contrast** - Maintain WCAG AA contrast ratios
3. **Focus Indicators** - Clear visual focus indicators for all interactive elements

## Common Patterns and Examples

### Card Component with Context

```typescript
<CardDisplay
  card={card}
  context={{
    isOnSetPage: true,
    showCollectorInfo: hasCollector
  }}
  onCardClick={(cardId) => navigate(`/card/${cardId}`)}
  onSetClick={(setCode) => navigate(`/set/${setCode}`)}
  onArtistClick={(artistName) => navigate(`/artists/${artistName}`)}
/>
```

### Responsive Grid Layout

```typescript
<ResponsiveGrid
  minItemWidth={250}
  spacing={3}
  justifyContent="center"
>
  {cards.map(card => (
    <CardCompact key={card.id} card={card} context={context} />
  ))}
</ResponsiveGrid>
```

### Material-UI sx Prop Usage

```typescript
<Box
  sx={{
    bgcolor: 'grey.900',
    borderRadius: 2,
    p: 3,
    border: '2px solid',
    borderColor: 'rarity.mythic',
    boxShadow: theme.mtg.shadows.card.hover,
    '&:hover': {
      transform: 'translateY(-4px)',
      boxShadow: theme.mtg.shadows.card.selected
    }
  }}
>
```

### Apollo Client Query Pattern

```typescript
const { data, loading, error, refetch } = useCardsQuery({
  variables: {
    filters: { setCode, rarity },
    pagination: { limit: 20, offset: 0 }
  },
  errorPolicy: 'all'
});

if (loading) return <LoadingSpinner />;
if (error) return <ErrorAlert error={error} onRetry={refetch} />;
```

This frontend architecture provides a scalable, maintainable, and accessible foundation for the MTG Discovery application while maintaining consistency with Material-UI design principles and React best practices.