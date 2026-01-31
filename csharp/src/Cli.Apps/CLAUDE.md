# CLI Apps

## Purpose
Production batch operations: migrations, maintenance, data ingestion, periodic tasks. Executable entry points with optional interactive dashboards.

## Architecture

All CLI apps extend `ExampleApplication` from `Example.Core`:

**Simple Pattern**: Load config → Execute() → log results → exit code
- Example: `Cli.MtgDiscovery.DataMigration/DataMigrationApplication.cs`

**Interactive Pattern**: Load config → create dashboard → spawn Task.Run() for work → await both UI and task
- Example: `Cli.MtgDiscovery.PriceUpdate/PriceUpdateApplication.cs:Execute()`

## Common Setup

- Extend `ExampleApplication`
- Load configuration via `MonoStateConfig`
- Use `Microsoft.Extensions.Logging`
- Support `appsettings.json` + `local.settings.json` overrides
- Return sensible exit codes
- Handle errors gracefully (continue on failure, log summary)

## Layer Interaction

```
CLI App  →  Orchestrator  →  [Domain/Aggregators/Adapters]  →  External Systems
```

No business logic in CLI layer; delegate to Domain/Aggregators.

## Creating New CLI Apps

1. Create project: `Cli.<Feature>`
2. Extend `ExampleApplication` with `Execute()` method
3. Create `<Feature>Configuration` class from `appsettings.json`
4. For dashboards: use `RazorConsole.Core` abstraction
5. For background work: spawn `Task.Run()` + `await`
6. Create tests stub (parallel `*.Tests` project)

See: `Cli.MtgDiscovery.DataMigration/` (simple), `Cli.MtgDiscovery.PriceUpdate/` (with dashboard)

## Code Style

Follow C# source code rules: file-scoped namespaces, sealed classes, constructor injection, `ConfigureAwait(false)` on async calls, no public statics, explicit types.

See: `csharp/src/CLAUDE.md`
