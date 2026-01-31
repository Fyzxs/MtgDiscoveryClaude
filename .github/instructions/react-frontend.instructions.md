# React Frontend Guidelines

This file provides guidance for work in `/client/web` (React 19 / Vite / MUI frontend).

## Tech Stack
- React 19
- Vite
- MUI (Material-UI)

## MicroObjects Principles Apply
The MicroObjects principles from the main instructions apply to React components as well:
- **Have a representation for every concept** - Create reusable component types
- **No Getters/Setters** - Props should be simple data, not accessors
- **Immutable** - React components should be pure, no internal state mutations
- **Composition Over Inheritance** - Use component composition, not class inheritance
- **No Logic in Constructors** - Use hooks for side effects and initialization
- **If Only as Guard Clauses** - Early returns in JSX only
- **No Switch/Else** - Use conditional rendering patterns or polymorphic components
- **No Nulls** - Use Null Object pattern or empty states
- **No Primitives** - Wrap related data in interfaces/types
- **Sealed by default** - Mark components as const unless inheritance needed

## Component Organization

Keep components focused and composable:
- Single responsibility per component
- Props should represent minimal needed data
- Extract complex logic to custom hooks
- Use composition for complex UIs

## Styling
- Use MUI components and theming
- Prefer MUI's `sx` prop for component-level styles
- Leverage MUI's theme system for consistent design

## State Management
- Use React hooks for local state
- Leverage context for app-wide state when needed
- Keep state as close to where it's used as possible

## Type Safety
- Use TypeScript interfaces for all props
- Define strict types for API responses from GraphQL
- Avoid `any` types

## GraphQL Integration
- Queries/mutations should match the backend `*OutEntity` types
- Parse responses according to ResponseModel union type (Success|Failure)
- Handle both success and failure cases explicitly

## Testing
- Follow the testing patterns from `.github/instructions/testing.instructions.md`
- Test component behavior, not implementation
- Mock GraphQL queries appropriately
