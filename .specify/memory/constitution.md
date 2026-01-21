<!--
Sync Impact Report:
Version Change: None (initial constitution) → 1.0.0
Modified Principles: N/A (initial creation)
Added Sections: All sections are new
Removed Sections: None
Templates Requiring Updates:
  - ✅ .specify/templates/plan-template.md (reviewed, aligns with principles)
  - ✅ .specify/templates/spec-template.md (reviewed, aligns with principles)
  - ✅ .specify/templates/tasks-template.md (to be reviewed when tasks are generated)
Follow-up TODOs: None
-->

# MtgDiscoveryVibe Constitution

## Core Principles

### I. MicroObjects Architecture (NON-NEGOTIABLE)

Every concept MUST have explicit representation through interfaces and classes. This is the foundational architectural pattern that permeates the entire codebase.

**Core Rules:**
- Every non-primitive-obsession class MUST have an interface in its hierarchy
- Primitives MUST be wrapped in domain objects (except in DTOs for simplicity)
- No nulls MUST be used (use Null Object pattern)
- Objects MUST be immutable with `private readonly` fields
- Constructor injection only - NO logic in constructors
- NO public statics (except MonoState pattern, LoggerMessage attributes, framework requirements)
- NO enums, NO reflection at runtime
- Composition over inheritance
- Methods expose behavior, not data (no getters/setters except DTOs)

**Rationale:** MicroObjects provides compile-time safety, explicit design, improved testability, and makes implicit concepts explicit. This extreme OOP approach ensures every decision is represented as an object.

### II. Layered Architecture Flow

The system MUST follow strict unidirectional data flow through seven distinct layers.

**Data Flow (Request → Response):**
1. App Layer (GraphQL) - Translate request to ArgEntity
2. Entry Layer - Validate ArgEntity, map to ItrEntity
3. Shared Layer - Apply cross-cutting rules
4. Domain Layer - Apply business logic (ALWAYS rules)
5. Aggregator Layer - Orchestrate adapter calls
6. Adapter Layer - Map to ExtEntity, call external systems
7. Infrastructure Layer - Core utilities (Cosmos, Universal)

**Return Flow (Response ← Request):**
- Aggregator aggregates responses
- Domain applies invariants
- Shared applies rules
- Entry maps OufEntity to OutEntity
- App translates to GraphQL response

**Entity Type Requirements:**
- **ArgEntity**: GraphQL/external input (App → Entry)
- **ItrEntity**: Internal transfer (Entry ↔ Shared ↔ Domain ↔ Aggregator)
- **XfrEntity**: Adapter internal operations
- **ExtEntity**: External system entities (Cosmos documents)
- **OufEntity**: Domain/aggregator output (internal)
- **OutEntity**: GraphQL output (Entry → App)

**Rationale:** Strict layering prevents coupling, ensures clear separation of concerns, and makes testing straightforward. Each layer has a single, well-defined responsibility.

### III. Test-First Development (NON-NEGOTIABLE)

Testing MUST follow strict patterns using MSTest and AwesomeAssertions.

**Testing Requirements:**
- Framework: MSTest with AwesomeAssertions
- Each test completely self-contained (NO test class variables)
- Arrange-Act-Assert pattern mandatory
- Test naming: `MethodName_Scenario_ExpectedBehavior`
- Fakes over mocks (fakes in `Fakes` folder with "Fake" suffix)
- TypeWrapper pattern for private constructors
- ALWAYS verify fake invocation counts
- `ConfigureAwait(false)` on all async calls
- Return value named `actual` or use `_` discard

**Production Code Immutability:**
- NEVER modify production code for test scenarios
- Tests MUST work with production code as-is
- Use TypeWrapper or test implementations instead of reflection

**Rationale:** Self-contained tests ensure isolation, prevent flaky tests, and make debugging straightforward. Each test is independently understandable without context.

### IV. Null Boundary Guards

Validators MUST check for null at system boundaries and protect the interior from null references.

**Validation Architecture:**
- Validators check for null at boundaries (GraphQL input, external data)
- Validators return `Task<bool>` indicating validity
- Null checks in validators are CORRECT and NECESSARY
- Once past validation, interior code assumes non-null
- Use Null Object pattern for optional behavior (not null references)

**Multi-Class Validation Pattern:**
- Container class composes multiple validators in sequence
- Each validator class implements specific validation logic (NOT Func delegates)
- Nested Validator class for typed behavior
- Nested Message class for typed error messages (NOT strings)

**Rationale:** This pattern provides test isolation (each validator independently testable), compile-time safety (typed messages), and clear failure reasons. The "class explosion" is precision, not complexity - each class does ONE thing and fails for ONE reason.

### V. Scope and Access Control

Scope MUST be restricted to prevent unintended coupling and maintain encapsulation.

**Scope Rules:**
- **Public scope**: ONLY in `Apis` folders
- **Internal scope**: Everything outside `Apis` folders
- **Test projects**: Have `InternalsVisibleTo` access to source projects

**Exceptions:**
- `CosmosItem` classes MUST be `public` (Cosmos requirement)
- Framework-required public members (minimal)

**Rationale:** Explicit scope control prevents accidental exposure of internal implementation details and maintains clean API boundaries.

### VI. Code Style Consistency

Code MUST follow strict style guidelines for readability and consistency.

**Backend (.NET) Requirements:**
- File-scoped namespaces
- NO greater than operators (use `<` only)
- NO boolean negation (`!`) - use `is false` or explicit inverse methods
- `ConfigureAwait(false)` on ALL async calls
- `init` setters for DTO-style classes
- NO comments unless explicitly requested
- Classes MUST be `sealed` or `abstract` (very few exceptions)
- Explicit types preferred over `var`

**Frontend (React/TypeScript) Requirements:**
- Material-UI ONLY (Tailwind being phased out)
- Use MUI `sx` props (NOT className for custom styles)
- Domain-organized atoms (Cards/, Sets/, shared/)
- Each component has dedicated Props interface
- Atomic design folder structure
- GraphQL: Use generated types from codegen (NO manual definitions)

**Rationale:** Consistency reduces cognitive load, makes code predictable, and prevents subtle bugs from inconsistent patterns.

### VII. NoArgsEntity Pattern

Operations requiring no input MUST use `NoArgsEntity` instead of void, null, or omitting parameters.

**Pattern Requirements:**
- Use `NoArgsEntity` as input type for parameter-less operations
- Skip validation and mapping (nothing to validate/map)
- Maintain consistency with `IOperationResponseService<TInput, TOutput>`

**When to Use:**
- Operations retrieving all items without filtering (e.g., `AllSetsAsync()`)
- Operations requiring no input parameters
- Maintaining interface consistency

**Rationale:** Type-safe way to express "no input needed" while maintaining architectural consistency. Avoids void (can't be generic type parameter) and null (violates No Nulls principle).

## Technology Stack Standards

### Backend Requirements

**Language & Framework:**
- C# .NET 9.0 ONLY
- GraphQL API using HotChocolate
- Authentication: Auth0 JWT (Azure Entra ID capable)

**Data & Storage:**
- Database: Azure Cosmos DB
- Storage: Azure Blob Storage
- Caching: MonoStateMemoryCache pattern

**JSON Serialization:**
- Newtonsoft.Json ONLY
- DO NOT use System.Text.Json

**Project References:**
- Add via dotnet commands: `dotnet add reference ../ProjectName/ProjectName.csproj`
- DO NOT manually edit project files

### Frontend Requirements

**Framework & Language:**
- React 19 with TypeScript
- Build tool: Vite
- UI library: Material-UI (@mui/material)

**State & Data:**
- GraphQL: Apollo Client with code generation
- Routing: React Router DOM
- Auth: Auth0 React SDK

**Code Generation:**
- Run `npm run codegen` after schema changes
- Use generated hooks from `src/generated/`
- NO manual GraphQL type definitions

### Infrastructure

**Deployment:**
- Azure Container Apps
- CI/CD: Azure DevOps Pipelines
- Monitoring: Application Insights

**DevOps Integration:**
- Azure Boards for work item tracking
- Azure Repos for Git and PRs
- Azure CLI (`az boards`, `az repos`) pre-approved

## Development Workflow

### Feature Development Process

1. **Specification Phase:**
   - Create feature specification in `/specs/###-feature-name/spec.md`
   - Run `/speckit.specify` to generate/update spec
   - Clarify requirements using `/speckit.clarify`

2. **Planning Phase:**
   - Run `/speckit.plan` to generate implementation plan
   - Generate `research.md`, `data-model.md`, `contracts/`, `quickstart.md`
   - Review constitution compliance

3. **Implementation Phase:**
   - Run `/speckit.tasks` to generate task breakdown
   - Execute tasks following plan
   - Create interfaces before implementations
   - Write tests using TypeWrapper for private constructors

4. **Review Phase:**
   - Run `/speckit.analyze` for cross-artifact consistency
   - Verify all constitution principles followed
   - Azure DevOps PR process with required reviewers

### Complexity Constraints

**3-Build-Failure Limit:**
- STOP implementation after 3 consecutive build failures
- Report to user with diagnostic information
- Do NOT continue without user intervention

**Pattern Consistency:**
- ALWAYS check existing patterns in neighboring files
- Follow established patterns for similar functionality
- Do NOT invent new patterns without justification

**Pragma Directive Policy:**
- AVOID #pragma directives (they accumulate as technical debt)
- Every #pragma MUST have specific justification comment
- Review and remove unnecessary pragmas during code reviews
- Scope as narrowly as possible (line-specific, not file-wide)

### GraphQL Development Standards

**Query/Mutation Structure:**
- Queries/mutations return union types (success/failure scenarios)
- Use `... on TypeName` fragments for type-specific fields
- ALWAYS include `__typename` for type discrimination

**Type Definition Pattern (CRITICAL):**

Every GraphQL response type MUST follow three-part pattern:
1. **Union Type Class**: Extends `UnionType` (not `UnionType<T>`)
2. **Success Response Type Class**: `{Entity}SuccessDataResponseModelType`
3. **Entity Type Classes**: `{Entity}OutEntityType` for each entity

**Schema Registration:**
- ALL types MUST be registered in schema extensions
- Use `AddType<T>()` for each type class
- Union types reference dedicated type classes (NOT `ObjectType<T>`)

**Validation Checklist:**
- [ ] Union type class exists
- [ ] Success response type class exists
- [ ] Entity type classes exist for all entities
- [ ] All types registered in schema
- [ ] No inline `ObjectType<T>` in union types

## Quality Gates

### Pre-Commit Requirements

**Code Quality:**
- All tests MUST pass
- Build MUST succeed without warnings
- NO #pragma directives without justification
- Follow naming conventions

**Architecture Compliance:**
- Correct entity types for each layer
- Proper data flow (unidirectional through layers)
- Service dependencies flow downward ONLY
- Validators at boundaries check for null

**Documentation:**
- Public APIs have XML documentation
- Internal classes do NOT need documentation
- README updates for new features
- CLAUDE.md updates for new patterns

### Pre-PR Requirements

**Testing:**
- Unit tests for all new code
- Integration tests for cross-layer functionality
- Fake invocation counts verified
- No test class variables

**Consistency:**
- Patterns match existing code in same layer
- Entity naming follows conventions (ArgEntity, ItrEntity, etc.)
- GraphQL types follow three-part pattern
- Material-UI sx props used (not Tailwind)

**Review:**
- Self-review using constitution as checklist
- Run `/speckit.analyze` for consistency
- Azure DevOps work items linked
- PR template completed

## Governance

### Amendment Process

**Constitution Changes:**
1. Proposal document with rationale
2. Impact analysis on existing code
3. Template updates required
4. Migration plan for existing code
5. Version increment following semantic versioning

**Version Bump Rules:**
- **MAJOR**: Backward incompatible principle changes
- **MINOR**: New principles added or existing expanded
- **PATCH**: Clarifications, wording fixes, non-semantic changes

### Compliance Verification

**All PRs MUST:**
- Verify alignment with core principles
- Document any justified complexity
- Update affected templates
- Include constitution version reference

**Complexity Justification:**
Required when violating limits or adding complexity:
- Document violation in PR
- Explain why needed
- List simpler alternatives rejected and why

### Runtime Guidance

For day-to-day development guidance:
- **Backend**: `CLAUDE.md`, `CODING_CRITERIA.md`, `TESTING_GUIDELINES.md`
- **Frontend**: `client/CLAUDE.md`, `client/README.md`
- **Architecture**: `Architecture_Layers_Patterns.md`, `MicroObjects_Summary.md`
- **Product**: `PRD.md` for requirements and features

Constitution supersedes all other documentation when conflicts arise.

**Version**: 1.0.0 | **Ratified**: 2026-01-17 | **Last Amended**: 2026-01-17
