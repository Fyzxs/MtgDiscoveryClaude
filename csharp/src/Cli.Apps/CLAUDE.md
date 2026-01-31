# Cli.Apps Projects

## Purpose
Production command-line applications that perform batch operations, data migrations, and periodic maintenance for the MTG Discovery platform. These are executable entry points that run asynchronously, often with interactive dashboards.

## Application Categories

### Data Operations (ETL/Ingestion)
## Execution Patterns

**Pattern 1: Simple Orchestration** - See `Cli.MtgDiscovery.DataMigration/DataMigrationApplication.cs`: ExampleApplication.StartUp() loads config → Execute() runs synchronously → logs results → returns exit code

**Pattern 2: Background Task + Interactive Dashboard** - See `Cli.MtgDiscovery.PriceUpdate/PriceUpdateApplication.cs:Execute()`: creates RazorConsole dashboard, spawns Task.Run() for async processing, main thread blocks on dashboard.RunUiAsync(), then awaits processing task

## Common Characteristics

All platform CLI apps (except StandaloneGroupingsScraper):
- Extend `ExampleApplication` from `Example.Core`
- Load configuration via `IConfiguration` + `MonoStateConfig`
- Use `Microsoft.Extensions.Logging` for structured logging
- Handle errors gracefully (continue on individual failures, log summary)
- Support `appsettings.json` + optional `local.settings.json` overrides
- Command-line argument parsing for filtering/options
- Return sensible exit codes

## Layer Interaction

CLI apps interact with platform layers:
```
CLI App
    ↓
Orchestrator (coordinates work)
    ├→ Adapters (read/write via Gophers, Scribes, Inquisitors)
    ├→ Domain Services (business logic where needed)
    └→ Entry Layer (validation/mapping when required)
```

**Dependencies flow downward** - CLI → Entry/Aggregators → Domain → Adapters → Infrastructure

## Creating New CLI Apps

1. Create project: `Cli.<Feature>` or `Example.Scryfall.<Feature>`
2. Extend `ExampleApplication` with your application class
3. Implement `Execute()` async method with orchestration logic
4. Create `<Feature>Configuration` class for settings
5. Provide `appsettings.json` with your configuration sections
6. For interactive CLI: use `RazorConsole.Core` dashboard abstraction
7. For background work: spawn `Task.Run()` and `await` processing task
8. Create tests stub (deferred implementation acceptable if noted)

**With Interactive Dashboard** - See `Cli.MtgDiscovery.PriceUpdate/PriceUpdateApplication.cs:Execute()`: Creates dashboard via `DashboardFactory`, spawns background task with `Task.Run()`, awaits both dashboard UI and processing task

**Without Interactive UI** - See `Cli.MtgDiscovery.DataMigration/DataMigrationApplication.cs:Execute()`: Directly await processing logic, logging happens automatically via ILogger

## Configuration Pattern

All CLI apps use configuration sections in `appsettings.json` - see examples:
- `Cli.MtgDiscovery.DataMigration/appsettings.json` - MigrationConfiguration structure
- `Cli.MtgDiscovery.PriceUpdate/appsettings.json` - PriceUpdateConfiguration structure

Configuration classes access settings via constructor injection: See `Cli.MtgDiscovery.DataMigration/Configuration/MigrationConfiguration.cs` for the pattern

## Testing

- **Current state**: Test infrastructure incomplete
- **DataMigration.Tests**: Stub implementation (deferred - T207)
- **Pattern**: Use MsTest + AwesomeAssertions (same as test projects)
- **Location**: Parallel `*.Tests` project with same namespace structure

## Code Style

All CLI apps follow main project rules:
- File-scoped namespaces
- `sealed` or `abstract` classes
- No boolean negation (`is false` instead of `!`)
- `ConfigureAwait(false)` on all async calls
- Constructor injection only
- No public statics (except configuration static in Example.Core)
- Explicit types over `var`

## Key Principle

CLI apps are **production operational code** - focus on reliability, logging, error recovery, and user feedback (dashboards). They serve as the platform's background worker tier: migrations, maintenance, ingestion, and asset processing.
