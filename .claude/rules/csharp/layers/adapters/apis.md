---
paths:
  - "csharp/src/Lib.Adapters/**/Apis/**"
---

The APIs folder is the public contract for this adapter. Anything in the public folder must be scoped `public`. If it is not intended for external use; do not put it here.

The adapter here is follows the naming convention of the project `{Type}AdapterService`.
The methods implemented by a concrete service here MUST only accept a single param, plus the `CancellationToken`. This param MUST be an interface defined in the `./Entities`. 
The 'AdapterService' implements an interface (`I{Type}AdapterService`) that delegates to the appropriate CQRS pattern interface, also defined here in the APIs folder.

Any specific CQRS behavior interfaces do not belong here.