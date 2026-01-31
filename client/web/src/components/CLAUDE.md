# Components Folder

## Purpose
React components organized by Atomic Design pattern. All components compose together to build pages. **CRITICAL: Never import from `@mui/material` directly — use atom wrappers instead.**

## Architecture: Atomic Design

```
components/
  ├─ atoms/         (Smallest building blocks)
  ├─ molecules/     (Combinations of atoms)
  ├─ organisms/     (Complex component combinations)
  ├─ templates/     (Page layout structures)
  ├─ pages/         (Complete page implementations)
  ├─ providers/     (Context providers)
  ├─ auth/          (Auth-specific components)
  └─ utils/         (Component utilities)
```

See: `components/README.md`, `components/atoms/README.md`, `components/templates/pages/README.md`

## Atoms: Building Blocks

**Two Types of Atoms:**

1. **MUI Wrappers** (`atoms/mui-wrappers/`): Every @mui/material component gets wrapped
   - See: `atoms/mui-wrappers/Button.tsx` — pattern for wrapping MUI components

2. **Domain Atoms** (domain-organized):
   - Cards: `atoms/Cards/CardName.tsx`, `atoms/Cards/CardImage.tsx`
   - Sets: `atoms/Sets/SetIcon.tsx`
   - See: `atoms/` directory for complete atom structure

**When to Create Atoms:**
- Single, focused responsibility
- Reusable across multiple molecules
- No knowledge of sibling atoms
- Receives data via props, no internal state (usually)

See: `atoms/Cards/CardName.tsx`, `atoms/shared/DebouncedSearchInput.ts`

## Molecules: Atom Combinations

Combine atoms into functional units. See: `molecules/Cards/CardImage.tsx`, `molecules/Sets/MtgSetCard.tsx`

**When to Create Molecules:**
- Combines 2+ atoms with specific behavior
- Encapsulates one interaction pattern
- Still domain-focused, not generic container

## Organisms: Complex Components

Large, self-contained functional units. See: `organisms/` directory for examples.

**When to Create Organisms:**
- Multiple molecules combined
- Significant internal state or logic
- Handles complex interactions

## Templates: Page Layouts

See: `components/templates/pages/README.md` for the three main templates:
- BrowseTemplate
- SearchTemplate
- DetailTemplate

## Pages: Complete Pages

Implementations using templates. See: `pages/` directory.

## Providers: Context Providers

Context-based state management. See: `providers/` directory.

## Critical Rule: Atom Imports

**NEVER import from @mui/material:**
```typescript
// ❌ WRONG
import { Button } from '@mui/material'

// ✅ CORRECT
import { AppButton } from '@/components/atoms/mui-wrappers'
```

See: `atoms/README.md` for wrapper list

## Naming Conventions

| Item | Pattern | Example |
|------|---------|---------|
| **Component file** | PascalCase | `CardName.tsx` |
| **Component export** | PascalCase | `export function CardName` |
| **Props interface** | `{Component}Props` | `CardNameProps` |
| **Barrel file** | `index.ts` | Exports from directory |

## Directory Structure Checklist

- ✓ Create component in appropriate atomic level
- ✓ Place in domain folder (Cards/, Sets/, Binder/, etc.)
- ✓ Define explicit Props interface
- ✓ Export from `index.ts` barrel file
- ✓ Use atom imports, never raw @mui/material
- ✓ Responsive design with theme tokens
- ✓ Accessibility: alt text, ARIA labels, keyboard support
- ✓ Colocate tests in `__tests__/` subfolder

## Styling Approach

Use MUI `sx` props. See: `styles/cardStyles.ts`, `theme/designTokens.ts`

## When to Create New Components

1. **New Atom**: Reusable visual element, single responsibility
2. **New Molecule**: Atom combination with specific interaction
3. **New Organism**: Complex component with significant logic
4. **New Template**: New page layout pattern
5. **New Page**: Instance of template with data

Reference existing patterns in `components/atoms/`, `components/molecules/` before creating new components.
