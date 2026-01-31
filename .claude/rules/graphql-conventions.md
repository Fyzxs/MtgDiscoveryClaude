# GraphQL Query/Mutation Implementation

Request → Response Flow (Unidirectional)

1. GraphQL Input → ArgEntity (in Entities/Args/)
2. Query/Mutation Method → calls IEntryService
3. IEntryService → returns IOperationResponse<OutEntity>
4. Mapper → converts to ResponseModel (union: Success|Failure)
5. GraphQL Output → ResponseModel returned to client

Implementation Checklist

## Layer 1: GraphQL Endpoint (Thin)
- ArgEntity example: Entities/Args/CardIdsArgEntity.cs
  - Pattern: Implement interface, public properties for input
- Query method example: Queries/CardQueryMethods.cs:42-46
  - Constructor pattern: public (ILogger) → private with IEntryService + mappers
  - Method signature: async Task<ResponseModel> MethodName(ArgEntity arg)
  - Decorators: [GraphQLType] + [Authorize] if needed
- Mutation method example: Mutations/UserMutationMethods.cs:37-43
  - Same pattern: inject services, call _entryService, map response

## Layer 2: Type Registration (Schemas)
- Reference file: Schemas/ApiQueryExtensions.cs
  - Pattern: .AddTypeExtension<MethodClass>()
  - Pattern: .AddType<ArgEntityInputType>() for each input
  - Pattern: .AddType<ResponseModelUnionType>() for each response

## Layer 3 & Below: Entry/Domain (Already Exists)
- Entry service interface: Lib.MtgDiscovery.Entry/Apis/IEntryService.cs
- Example entry service: Lib.MtgDiscovery.Entry/Queries/UserEntryService.cs
  - These handle validation, mapping, domain calls
  - GraphQL never touches these layers

## Key Rules
- GraphQL = Request/Response translation only
- No business logic, validation, or mapping in queries/mutations
- ArgEntity properties = GraphQL input fields (1:1)
- All errors come as ResponseModel (union type), never throw
- Always use ConfigureAwait(false) on async calls

## Real Examples to Copy From
- Complete query endpoint: Queries/CardQueryMethods.cs (lines 17-68)
- Complete mutation endpoint: Mutations/UserMutationMethods.cs (lines 17-45)
- Full type registration: Schemas/ApiQueryExtensions.cs (lines 17-72)
- Mapper usage: Queries/CardQueryMethods.cs:45-46 or Mutations/UserMutationMethods.cs:42-43