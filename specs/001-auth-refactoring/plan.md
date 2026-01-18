# Implementation Plan: Authentication Flow Refactoring

**Branch**: `001-auth-refactoring` | **Date**: 2026-01-17 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-auth-refactoring/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Refactor the authentication flow to eliminate the "first time every time" problem where returning users are forced through the registration flow on every login. The solution implements a **server-authoritative architecture** where the backend is the source of truth for user status, with localStorage used as a performance optimization cache. Includes a new backend user verification endpoint, frontend callback routing improvements, simplified token management, and optimized GraphQL caching.

## Technical Context

### Frontend

**Language/Version**: TypeScript (React 19, modern ES2020+)

**Primary Dependencies**:
- React 19 with TypeScript
- Material-UI (@mui/material) for UI components
- Auth0 React SDK (@auth0/auth0-react) for authentication
- Apollo Client for GraphQL state management
- React Router DOM v6 for routing

**Storage**:
- localStorage for caching backend verification responses (client-side performance optimization)
- Apollo InMemoryCache for GraphQL query caching
- Graceful fallback to in-memory cache if localStorage disabled

**Testing**: Vitest with React Testing Library (verified in client/package.json)

**Target Platform**: Modern web browsers (Chrome, Firefox, Safari, Edge - ES2020+ support)

### Backend

**Language/Version**: C# .NET 9.0

**Framework**: HotChocolate GraphQL API

**Database**: Azure Cosmos DB (existing user collection)

**New Endpoint Required**: User verification query (returns user status and needsOnboarding flag)

**Testing**: MSTest with existing patterns

### Integration

**Performance Goals**:
- Returning user redirect < 1 second total (including backend verification)
- Backend verification endpoint responds < 200ms (p95)
- localStorage enables instant UI (0ms perceived delay) with background verification
- Eliminate unnecessary registration calls for returning users
- Remove token subscription race conditions

**Constraints**:
- Backend endpoint must be idempotent (safe to call multiple times)
- Backend must auto-create user records if they don't exist (upsert pattern)
- GraphQL schema changes must be additive only (no breaking changes)
- Auth0 callback URL update may be required (Phase 2)
- Phased rollout to minimize risk

**Scale/Scope**:
- **Frontend**: ~10 files modified/created (client-side TypeScript/React)
- **Backend**: ~5 files modified/created (.NET C# GraphQL)
- **Total**: ~15 files across frontend + backend
- **Phases**: 6 implementation phases (added backend phase)

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Initial Check (Pre-Phase 0)

**Status**: ✅ CONDITIONAL PASS - Frontend refactoring with clarifications needed

| Principle | Compliance | Notes |
|-----------|------------|-------|
| I. MicroObjects Architecture | ⚠️ PARTIAL | Frontend uses React components (not MicroObjects OOP), but follows composition patterns. DTOs acceptable for data transfer. |
| II. Layered Architecture Flow | ✅ PASS | Frontend has its own layering: Components → Hooks → GraphQL → Apollo Client. Not applicable to backend layers. |
| III. Test-First Development | ✅ RESOLVED | Testing framework confirmed: Frontend uses Vitest + React Testing Library, Backend uses MSTest (see research.md). |
| IV. Null Boundary Guards | ✅ PASS | TypeScript provides compile-time null safety. Runtime checks at Auth0 integration points. |
| V. Scope and Access Control | N/A | Frontend code is public (bundled). Principle applies to backend only. |
| VI. Code Style Consistency | ✅ PASS | Follows frontend requirements: Material-UI sx props, TypeScript, atomic design, generated GraphQL types. |
| VII. NoArgsEntity Pattern | N/A | Backend pattern, not applicable to frontend React hooks. |

**Technology Stack Standards:**
- ✅ Frontend Requirements: React 19, TypeScript, Material-UI, Apollo Client, Auth0 SDK all confirmed
- ✅ Code Generation: Uses `npm run codegen` for GraphQL types (confirmed in CLAUDE.md)
- ✅ DevOps Integration: Azure DevOps for work items and PRs

**Development Workflow:**
- ✅ Follows 4-phase process (Specification → Planning → Implementation → Review)
- ✅ Constitution compliance checked at each gate

**Quality Gates:**
- ⚠️ Testing framework needs clarification before implementation
- ✅ Material-UI sx props (not Tailwind)
- ✅ Generated GraphQL types from codegen
- ✅ TypeScript with proper interfaces

**Clarifications Needed (Phase 0 Research):**
1. ✅ Frontend testing framework (Vitest/React Testing Library confirmed)
2. ✅ MicroObjects principles translation (mapping table in research.md)
3. ✅ localStorage validation approach (type guards in data-model.md)

**Re-evaluation Required:** After Phase 1 design artifacts generated

---

### Post-Phase 1 Re-Evaluation

**Status**: ✅ PASS - All design artifacts complete and constitution-compliant

| Principle | Compliance | Evidence |
|-----------|------------|----------|
| I. MicroObjects Architecture | ✅ PASS | All entities have TypeScript interfaces (StoredUserData, AuthCallbackResult, TokenReadyState). See contracts/ |
| III. Test-First Development | ✅ PASS | Testing framework confirmed (Vitest). Test patterns defined in contracts. Quickstart includes test checklist. |
| IV. Null Boundary Guards | ✅ PASS | Type guards implemented for localStorage (isValidStoredUserData). Boundary validation in data-model.md. |
| VI. Code Style Consistency | ✅ PASS | Material-UI sx props patterns in contracts. TypeScript interfaces for all props. Atomic design structure. |

**Design Artifacts Generated:**
- ✅ research.md: All clarifications resolved, decisions documented
- ✅ data-model.md: TypeScript interfaces with validation rules
- ✅ contracts/: Complete API contracts for utilities, hooks, components, Apollo
- ✅ quickstart.md: Step-by-step implementation guide with constitution gates

**Quality Gates Met:**
- ✅ All interfaces defined with TypeScript
- ✅ Validation patterns documented (type guards)
- ✅ Testing approach defined (Vitest + RTL)
- ✅ Material-UI patterns specified
- ✅ No unjustified complexity

**Ready for Phase 2: Implementation** (/speckit.tasks)

## Project Structure

### Documentation (this feature)

```text
specs/001-auth-refactoring/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

#### Frontend (client/)

```text
client/
├── src/
│   ├── components/
│   │   ├── auth/
│   │   │   ├── Auth0TokenProvider.tsx    # MODIFIED (Phase 4)
│   │   │   └── AuthButton.tsx            # MODIFIED (Phase 2)
│   │   └── pages/
│   │       ├── AuthCallbackPage.tsx      # NEW (Phase 3)
│   │       └── SignInRedirectPage.tsx    # MODIFIED (Phase 2)
│   ├── hooks/
│   │   └── auth/
│   │       ├── useAuthCallback.ts        # NEW (Phase 3)
│   │       ├── useBackendVerification.ts # NEW (Phase 2)
│   │       └── useTokenReady.ts          # NEW (Phase 4)
│   ├── utils/
│   │   ├── userStorage.ts                # NEW (Phase 2)
│   │   └── storageKeys.ts                # NEW (Phase 6)
│   ├── graphql/
│   │   ├── apollo-client.ts              # MODIFIED (Phase 4, 5)
│   │   └── queries/
│   │       └── user-verification.graphql # NEW (Phase 2)
│   ├── hooks/
│   │   └── user/
│   │       └── useUserSync.ts            # MODIFIED (Phase 5)
│   ├── main.tsx                          # MODIFIED (Phase 3)
│   └── App.tsx                           # MODIFIED (Phase 3)
```

#### Backend (src/)

```text
src/
├── App.MtgDiscovery.GraphQL/
│   ├── Queries/
│   │   └── UserVerificationQuery.cs      # NEW (Phase 1)
│   └── Entities/Types/
│       └── UserVerificationResponseType.cs # NEW (Phase 1)
├── Lib.MtgDiscovery.Entry/
│   └── UserVerificationEntryService.cs   # NEW (Phase 1)
├── Lib.Domain.User/
│   └── UserVerificationDomainService.cs  # NEW (Phase 1)
└── Lib.Aggregator.User/
    └── UserVerificationAggregatorService.cs # NEW (Phase 1)
```

**Structure Decision**: This is a full-stack web application refactoring with both backend and frontend changes. Backend follows the MicroObjects layered architecture (GraphQL → Entry → Domain → Aggregator → Adapter). Frontend follows React atomic design principles (atoms, molecules, organisms, pages) with domain organization (auth, utils, hooks).

**Files by Phase (aligned with tasks.md):**
- **Phase 1** (Setup): Environment verification (6 tasks)
- **Phase 2** (Foundation): Existing infrastructure verification (8 tasks)
- **Phase 3** (Backend - User Verification Endpoint): Backend GraphQL implementation following MicroObjects (49 tasks: entities, services, GraphQL types, tests)
- **Phase 4** (US1 - Returning User Fast Auth): GraphQL query, backend verification hook, error UI, localStorage, fast redirect, automatic retry (68 tasks including M3, M4, C1 enhancements)
- **Phase 5** (US2 - New User Registration): Callback page, backend integration, routing, Auth0 config (38 tasks including M1 integration)
- **Phase 6** (US3 - Session Termination): Logout data cleanup and validation (11 tasks)
- **Phase 7** (US4 - Resilient Token Management): Token simplification (24 tasks)
- **Phase 8** (US5 - Optimized Data Fetching): Caching optimization (15 tasks)
- **Phase 9** (Polish): Cross-cutting concerns and storage cleanup (22 tasks)

**Total Scope:** ~20 unique files (4 backend + 16 frontend including error UI components) across 9 phases, **241 tasks**

**Note**: Original quickstart Phase 1-5 mapping differs from tasks.md phases. Backend implementation (tasks.md Phase 3) is BLOCKING prerequisite for all frontend user stories.

**Coverage Improvements:** M1 (backend integration), M3 (conflict resolution), M4 (error UI), C1 (FR-028 automatic retry) addressed with +35 tasks (29 from initial remediation + 6 from FR-028) for 100% FR coverage. All FRs now explicitly or implicitly tagged.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

**Status**: No violations requiring justification.

**Rationale**: This is a frontend refactoring that follows constitution's frontend requirements (React 19, TypeScript, Material-UI, Apollo Client). MicroObjects OOP principles apply to backend .NET code, not React components. Frontend follows its own architectural patterns (atomic design, hooks, composition) which align with constitution's frontend standards.
