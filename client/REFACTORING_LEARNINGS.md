# Refactoring Learnings

## Date: 2025-08-26

### Context
During a refactoring attempt, the application showed a blank white page due to TypeScript compilation errors introduced by dependency updates and code changes.

## Key Issues Encountered

### 1. TypeScript verbatimModuleSyntax Configuration
**Problem:** When `verbatimModuleSyntax` is enabled in tsconfig.json, TypeScript requires explicit `import type` syntax for type-only imports.

**Error Example:**
```
error TS1484: 'ReactNode' is a type and must be imported using a type-only import when 'verbatimModuleSyntax' is enabled.
```

**Solution:**
```typescript
// Before (causes error)
import { useEffect, useRef, ReactNode } from 'react';
import { Box, SxProps, Theme } from '@mui/material';

// After (correct)
import { useEffect, useRef } from 'react';
import type { ReactNode } from 'react';
import { Box } from '@mui/material';
import type { SxProps, Theme } from '@mui/material';
```

### 2. MUI Grid v2 Migration
**Problem:** MUI Grid v2 (in @mui/material v7) has breaking API changes from v1.

**Key Changes:**
- Removed `item` prop - Grid items are now determined by size props
- Removed `container` prop - use `Grid` for containers
- Changed component structure

**Error Example:**
```
Property 'item' does not exist on type 'IntrinsicAttributes & GridBaseProps...'
```

**Solution:**
```typescript
// Before (Grid v1)
<Grid container spacing={2}>
  <Grid item xs={12} md={6}>
    Content
  </Grid>
</Grid>

// After (Grid v2)
<Grid container spacing={2}>
  <Grid size={{ xs: 12, md: 6 }}>
    Content
  </Grid>
</Grid>
```

### 3. TypeScript Strict Mode Issues
**Problems Encountered:**
- Unused variables and imports
- Undefined type assertions without proper checks
- Type incompatibilities with event handlers

**Examples:**
```typescript
// Unused imports
import { Stack } from '@mui/material'; // Never used

// Undefined as array index
const value = someObject[possiblyUndefined]; // Error: Type 'undefined' cannot be used as an index

// Event handler type mismatches
interface ToggleButtonProps extends ButtonProps {
  onToggle: (active: boolean) => void; // Conflicts with MUI's ToggleEventHandler
}
```

## Refactoring Best Practices

### 1. Pre-Refactoring Checklist
- [ ] Create a backup branch before major dependency updates
- [ ] Run `npm run build` to ensure clean baseline
- [ ] Document current dependency versions
- [ ] Review breaking changes in dependency changelogs

### 2. During Refactoring
- [ ] Update dependencies incrementally, not all at once
- [ ] Run TypeScript compiler after each change: `npx tsc --noEmit`
- [ ] Fix compilation errors before moving to next change
- [ ] Test in development server frequently

### 3. Common TypeScript Configuration Pitfalls
When updating TypeScript or enabling strict features:
- `verbatimModuleSyntax`: Requires explicit `import type` syntax
- `strict`: Enables all strict type checking options
- `noUncheckedIndexedAccess`: Requires undefined checks for array/object access
- `exactOptionalPropertyTypes`: Changes how optional properties work

### 4. MUI Version Migration Strategy
When upgrading MUI versions:
1. Check migration guides: https://mui.com/material-ui/migration/
2. Update imports first (components might move packages)
3. Fix component API changes
4. Update theme configuration if needed
5. Test all components thoroughly

### 5. Debugging Blank Page Issues
When the app shows a blank page:
1. Check browser console for errors
2. Run `npm run build` to see compilation errors
3. Check network tab for failed resource loads
4. Verify GraphQL endpoint is accessible
5. Check for infinite loops in useEffect hooks

## Recovery Strategy
If refactoring breaks the application:
1. Git reset to last working commit: `git reset --hard HEAD~1`
2. Or checkout previous branch: `git checkout <working-branch>`
3. Review this document before attempting again
4. Consider smaller, incremental changes

## Tools for Safer Refactoring
- TypeScript compiler: `npx tsc --noEmit --watch`
- ESLint with auto-fix: `npm run lint -- --fix`
- Build verification: `npm run build`
- Dependency audit: `npm audit`
- Bundle size analysis: Add bundle analyzer to vite config

## Specific Package Version Compatibility Notes
As of this incident:
- React 19.1 + TypeScript 5.8 requires careful type imports
- MUI v7 has significant Grid component changes from v6
- Vite v7 works well but requires proper TypeScript configuration
- Apollo Client v4 is stable with current setup

## Recommended Refactoring Order
1. Fix TypeScript configuration issues first
2. Update type imports to use `import type` syntax
3. Update component APIs (like MUI Grid)
4. Fix event handler types
5. Remove unused imports and variables
6. Add proper undefined checks
7. Test thoroughly before committing

## Commands to Remember
```bash
# Check TypeScript errors without building
npx tsc --noEmit

# Run build to see all errors
npm run build

# Start dev server
npm run dev

# Kill stuck dev servers
pkill -f "vite"
lsof -ti:5173,5174,5175 | xargs kill -9 2>/dev/null
```

## Conclusion
Large refactorings should be done incrementally with frequent testing. TypeScript's strict mode and modern configurations like `verbatimModuleSyntax` require careful attention to import syntax. Always maintain a working baseline to revert to if needed.