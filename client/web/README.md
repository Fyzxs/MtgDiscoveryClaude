# MTG Discovery React Client

React frontend for the MTG Discovery collection management platform.

## Prerequisites

- Node.js 18+
- The GraphQL backend running on port 65203

## Setup

```bash
# Install dependencies
npm install

# Start the GraphQL backend (from src directory)
dotnet run --project App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj
# The backend will run on https://localhost:65203 (primary) and http://localhost:65204 (secondary)

# Start the React development server (from client directory)
npm run dev
# The frontend will run on http://localhost:5173
```

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run codegen` - Generate TypeScript types from GraphQL schema
- `npm run codegen:watch` - Watch mode for GraphQL code generation
- `npm run lint` - Run ESLint

## Architecture

### Technology Stack
- **Vite** - Build tool and dev server
- **React 19** - UI framework
- **TypeScript** - Type safety
- **Apollo Client** - GraphQL client with caching
- **Tailwind CSS** - Utility-first styling
- **GraphQL Code Generator** - Auto-generate TypeScript types from schema

### Folder Structure
```
src/
├── components/          # Atomic Design components
│   ├── atoms/          # Basic building blocks
│   ├── molecules/      # Simple component groups
│   ├── organisms/      # Complex component structures
│   ├── templates/      # Page layouts
│   └── pages/          # Full page components
├── features/           # Feature-based modules
│   ├── cards/         # Card-related functionality
│   ├── collection/    # Collection management
│   └── search/        # Search functionality
├── graphql/           # GraphQL configuration and queries
├── hooks/             # Custom React hooks
├── services/          # API and utility services
├── types/             # TypeScript type definitions
├── utils/             # Utility functions
└── generated/         # Auto-generated GraphQL types
```

## Development Workflow

1. Ensure the backend is running on port 65203
2. Run `npm run codegen` to generate TypeScript types from the GraphQL schema
3. Start the dev server with `npm run dev`
4. The Vite proxy will forward `/graphql` requests to `https://localhost:65203` (with self-signed certificate support)

## Key Features

- **GraphQL-First Development** - Schema drives component props via code generation
- **Optimistic UI** - Apollo Cache enables instant UI updates
- **Type Safety** - Full TypeScript coverage with auto-generated GraphQL types
- **Mobile-First Design** - Responsive layouts with Tailwind CSS
- **Component Architecture** - Atomic Design pattern for scalable UI development