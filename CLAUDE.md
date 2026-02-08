# CLAUDE.md — Project Instructions (.NET)

## What this repo is
Magic: The Gathering Collection Tracking site. This codebase is the canonical reference implementation of how I expect software to be built. Precision of implementation of the patterns and practice is paramount.

## Tech stack
- .NET 10 / C# (modern style)
- React 19 / Vite / MUI

## Repo map
- `/csharp/src` → GraphQL backend and custom CLI Tools
- `/client/web` → Front end site

## Quick navigation

When working on tasks, consult the appropriate guide:

| Task | Guide |
|------|-------|
| Adding a GraphQL query or mutation | `.claude/rules/csharp/graphql-conventions.md` |
| Understanding architecture & layers | `.claude/rules/csharp/architecture-guide.md` |
| Writing C# code | `.claude/rules/csharp/csharp-code-style.md` |
| Writing or maintaining tests | `.claude/rules/csharp/testing-guide.md` |
| Working in the Entry layer | `.claude/rules/csharp/layers/entry/` |
| Working in the App/GraphQL layer | `.claude/rules/csharp/layers/app/` |
| Writing validators | `.claude/rules/csharp/actions/validators.md` |
| Writing enrichments | `.claude/rules/csharp/actions/enrichments.md` |

## Core Principles

1. **Dependencies flow inward only** — No layer crossing backward
2. **Constructor chains for DI** — No container frameworks
3. **Explicit mappers** — Every layer crossing gets a dedicated mapper class
4. **Async always** — Use `ConfigureAwait(false)`
5. **No nulls** — Use Null Object pattern instead

These principles are lived in the codebase—review existing code to see them in action.