# .examples Projects

## Purpose
Educational code demonstrating how to use the MTG Discovery platform layers and patterns. Each project shows developers concrete examples of interacting with specific components (Cosmos DB, Scryfall API, filtering patterns, etc.).

## Projects

### Example.Core
Shared base for all example applications:
- `ExampleApplication` abstract class: Template method pattern with `StartUp()` → `Execute()` lifecycle
- `SimpleConsoleLogger`: Basic logging for console examples
- Configuration loading: Reads `appsettings.json` and `local.settings.json`
- Static `MonoStateConfig`: Sets application-wide configuration (acceptable for examples only)

Extend `ExampleApplication` in executable projects to get automatic configuration and logging setup.

## Code Style Rules
- Skip production-level null checks and validation where clarity matters
- Omit defensive programming for non-critical paths
- Focus on demonstrating the pattern, not production hardening

**Always follow:**
- File-scoped namespaces
- Constructor injection only
- No comments unless explaining a non-obvious pattern

## Creating New Examples

1. Extend `Example.Core.ExampleApplication` for new executable examples
2. Name your project `Example.<FeatureName>` (e.g., `Example.SealedProducts`)
3. Implement `Execute()` async method with your demonstration logic
4. Update `Example.Core.csproj` if adding shared utilities
5. Provide `appsettings.json` with configuration for your example
6. Keep code focused on demonstrating ONE pattern or layer interaction

## Key Principle

Examples prioritize **clarity and demonstration** over production robustness. They show "how to use X" not "how to build production X". When developers read your example, they should immediately see the pattern you're demonstrating.
