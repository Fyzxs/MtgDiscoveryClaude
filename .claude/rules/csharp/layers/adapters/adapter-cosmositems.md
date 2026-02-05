---
paths:
  - "csharp/src/Lib.Adapters/**/CosmosItems/*"
---

CosmosItems are the representation of the information in cosmos, for how it's retrieved. All of these must be the entity type `ExtEntity` and inherit from `IExtEntity`.
The base class of these MUST be `CosmosItem`.
All properties require an attribute showing if it's serialized out, or ignored. Any type that is inherited MUST define the attribute for any property and the derived class MUST NOT re-define the serializeation behavior.

These are DTOs only.