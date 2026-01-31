# Examples

## Purpose
Educational code demonstrating how to use platform layers and patterns. Show developers concrete examples without production complexity.

## Example.Core

Base class and utilities for all examples:
- `ExampleApplication` — Template method: `StartUp()` → `Execute()`
- `SimpleConsoleLogger` — Basic logging
- Configuration loading from `appsettings.json` + `local.settings.json`

Extend `ExampleApplication` to get automatic config and logging setup.

## Creating Examples

1. Extend `Example.Core.ExampleApplication`
2. Name project: `Example.<FeatureName>`
3. Implement `Execute()` async method
4. Provide `appsettings.json`
5. Focus on ONE pattern only

## Code Style

- File-scoped namespaces
- Constructor injection only
- Skip production null checks if clarity matters
- No defensive programming for non-critical paths
- Only add comments for non-obvious patterns

## Key Principle

Examples are for **clarity, not robustness**. Show "how to use X", not "how to build production X". Reader should immediately see the pattern.
