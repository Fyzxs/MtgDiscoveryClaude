---
paths:
  - "csharp/src/**/Exceptions/**"
---

Exceptions almost always need to have `#pragma warning disable CA1032 // Implement standard exception constructors` around the constructor because we do not want to implemnent all standard exception constructors.