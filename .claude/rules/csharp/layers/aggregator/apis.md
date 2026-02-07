---
paths:
  - "csharp/src/Lib.Aggregators/**/Apis/**"
---

The APIs folder is the public contract for this aggregator. Anything in the public folder must be scoped `public`. If it is not intended for external use; do not put it here.

The aggregator here is follows the naming convention of the project `{Type}AggregatorService`.
The methods implemented by a concrete service here MUST only accept a single param, plus the `CancellationToken`. This param MUST be an `ItrEntity` that inherits `IItrEntity`.
The 'AggregatorService' implements an interface (`I{Type}AggregatorService`) that delegates to the appropriate CQRS pattern interface, also defined here in the APIs folder.

Any specific CQRS behavior interfaces do not belong here.
