# MtgDiscoveryVibe - Project Documentation Index

Generated: 2026-01-29 | Scan Level: Quick | Mode: Initial Scan

---

## Project Overview

- **Type:** Multi-part (monorepo with client + backend)
- **Primary Language:** TypeScript (frontend), C# (backend)
- **Architecture:** Atomic Design (frontend) + MicroObjects Layered (backend)

---

## Quick Reference

### Client (React Frontend)

| Aspect | Details |
|--------|---------|
| **Type** | Web Application |
| **Tech Stack** | React 19, TypeScript, Material-UI, Apollo Client, Auth0 |
| **Root** | `client/` |
| **Entry Point** | `src/main.tsx` |
| **Architecture** | Atomic Design |

### Backend (.NET)

| Aspect | Details |
|--------|---------|
| **Type** | GraphQL API |
| **Tech Stack** | .NET 10.0, HotChocolate, Cosmos DB, Auth0 JWT |
| **Root** | `src/` |
| **Entry Point** | `App.MtgDiscovery.GraphQL/Program.cs` |
| **Architecture** | MicroObjects Layered |

---

## Generated Documentation

### Core Documentation

- [Technology Stack](./technology-stack.md) - Complete technology inventory
- [Source Tree Analysis](./source-tree-analysis.md) - Directory structure with annotations
- [Development Guide](./development-guide.md) - Setup, commands, and workflows
- [Integration Architecture](./integration-architecture.md) - How parts communicate
- [Project Parts](./project-parts.json) - Machine-readable project metadata

### Inventories

- [Existing Documentation Inventory](./existing-documentation-inventory.md) - Catalog of existing docs

---

## Existing Documentation

### Root Level

| Document | Description |
|----------|-------------|
| [README.md](../README.md) | Project overview and getting started |
| [CLAUDE.md](../CLAUDE.md) | AI assistant guidelines (comprehensive) |
| [PRD.md](../PRD.md) | Product Requirements Document |
| [CODING_CRITERIA.md](../CODING_CRITERIA.md) | Project-specific coding patterns |
| [TESTING_GUIDELINES.md](../TESTING_GUIDELINES.md) | Testing conventions |
| [microobjects_coding_guidelines.md](../microobjects_coding_guidelines.md) | MicroObjects philosophy |
| [Architecture_Layers_Patterns.md](../Architecture_Layers_Patterns.md) | Layer patterns guide |
| [TECHNICAL_ARCHITECTURE_MANUAL.md](../TECHNICAL_ARCHITECTURE_MANUAL.md) | Architecture manual |

### Backend (src/)

| Document | Description |
|----------|-------------|
| [Architecture.md](../src/Architecture.md) | Backend architecture overview |

### Frontend (client/)

| Document | Description |
|----------|-------------|
| [client/README.md](../client/README.md) | Frontend overview |
| [client/CLAUDE.md](../client/CLAUDE.md) | Frontend AI guidelines |
| [client/LOGGING_GUIDE.md](../client/LOGGING_GUIDE.md) | Logging patterns |

### Feature Plans (.docs/)

| Document | Description |
|----------|-------------|
| [010-sqlite-migration-and-search-design.md](../.docs/010-sqlite-migration-and-search-design.md) | SQLite migration plan |
| [040a-COLLECTION_IDENTITY_IMPLEMENTATION_PLAN.md](../.docs/040a-COLLECTION_IDENTITY_IMPLEMENTATION_PLAN.md) | Collection identity plan |
| [040b-COLLECTION_IDENTITY_ARCHITECTURE.md](../.docs/040b-COLLECTION_IDENTITY_ARCHITECTURE.md) | Collection architecture |
| [070-i18n-Implementation-Plan.md](../.docs/070-i18n-Implementation-Plan.md) | Internationalization |
| [090-Accessibility-Implementation-Plan.md](../.docs/090-Accessibility-Implementation-Plan.md) | Accessibility plan |

### DevOps

| Document | Description |
|----------|-------------|
| [.pipelines/README.md](../.pipelines/README.md) | CI/CD documentation |
| [.pipelines/SECURITY.md](../.pipelines/SECURITY.md) | Security practices |

---

## Getting Started

### For New Developers

1. Read [README.md](../README.md) for project overview
2. Review [CLAUDE.md](../CLAUDE.md) for coding guidelines
3. Follow [Development Guide](./development-guide.md) for setup
4. Explore [Source Tree Analysis](./source-tree-analysis.md) for codebase layout

### For Backend Development

1. Review [Architecture_Layers_Patterns.md](../Architecture_Layers_Patterns.md)
2. Understand entity flow: ArgEntity → ItrEntity → XfrEntity → ExtEntity
3. Follow MicroObjects patterns in [microobjects_coding_guidelines.md](../microobjects_coding_guidelines.md)
4. Use [CODING_CRITERIA.md](../CODING_CRITERIA.md) for style rules

### For Frontend Development

1. Read [client/CLAUDE.md](../client/CLAUDE.md) for frontend patterns
2. Understand atomic design in [Source Tree Analysis](./source-tree-analysis.md)
3. Use Material-UI sx props (not Tailwind)
4. Generate types with `npm run codegen`

### For Feature Development

1. Check [.docs/](../.docs/) for existing feature plans
2. Review [Integration Architecture](./integration-architecture.md) for data flow
3. Follow the layer pattern: App → Entry → Domain → Aggregator → Adapter

---

## Key Commands

### Backend

```bash
dotnet build src/MtgDiscoveryVibe.sln
dotnet test src/MtgDiscoveryVibe.sln
dotnet run --project src/App.MtgDiscovery.GraphQL/App.MtgDiscovery.GraphQL.csproj
```

### Frontend

```bash
cd client
npm install
npm run codegen
npm run dev
```

### Azure DevOps

```bash
az boards work-item show --id <id>
az repos pr create --title "Title"
```

---

## Architecture Layers

### Backend Flow

```
Request → App → Entry → Domain → Aggregator → Adapter → Cosmos
                   ↓        ↓          ↓           ↓
              ArgEntity → ItrEntity → XfrEntity → ExtEntity
```

### Frontend Flow

```
atoms → molecules → organisms → templates → pages
```

---

## Contact

Project maintained by fyzxs

---

*Generated by BMAD Document Project workflow*
