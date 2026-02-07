---
paths:
  - "csharp/src/Lib.Adapters/**/Apis/Entities/*"
---

These are the the interfaces for calling layers to create a concrete instance of and apss into this Adapter Layer.
These must ONLY be `*XfrEntity` types, and inherit the marker interface `IXfrEntity`.
These must ONLY be interfaces.