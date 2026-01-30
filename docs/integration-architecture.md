# Integration Architecture

Generated: 2026-01-29

## Overview

MtgDiscoveryVibe is a multi-part project with a React frontend and .NET backend communicating via GraphQL. This document describes how the parts integrate.

---

## Integration Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         CLIENT (React)                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐          │
│  │   Auth0      │  │   Apollo     │  │  React       │          │
│  │   React SDK  │  │   Client     │  │  Router      │          │
│  └──────┬───────┘  └──────┬───────┘  └──────────────┘          │
│         │                 │                                     │
│         ▼                 ▼                                     │
│  ┌──────────────────────────────────────────────────┐          │
│  │              Auth0TokenProvider                   │          │
│  │         (JWT token → Apollo headers)              │          │
│  └──────────────────────┬───────────────────────────┘          │
└─────────────────────────┼───────────────────────────────────────┘
                          │ HTTPS + GraphQL
                          │ Authorization: Bearer <JWT>
                          ▼
┌─────────────────────────────────────────────────────────────────┐
│                        BACKEND (.NET)                           │
│  ┌──────────────────────────────────────────────────┐          │
│  │              HotChocolate GraphQL                 │          │
│  │         (Schema + Resolvers + Authorization)      │          │
│  └──────────────────────┬───────────────────────────┘          │
│                         │                                       │
│  ┌──────────────────────▼───────────────────────────┐          │
│  │              JWT Bearer Authentication            │          │
│  │         (Auth0 domain + audience validation)      │          │
│  └──────────────────────┬───────────────────────────┘          │
│                         │                                       │
│  ┌──────────────────────▼───────────────────────────┐          │
│  │              MicroObjects Layers                  │          │
│  │    App → Entry → Domain → Aggregator → Adapter    │          │
│  └──────────────────────┬───────────────────────────┘          │
│                         │                                       │
└─────────────────────────┼───────────────────────────────────────┘
                          │
           ┌──────────────┼──────────────┐
           ▼              ▼              ▼
    ┌────────────┐ ┌────────────┐ ┌────────────┐
    │ Azure      │ │ Azure      │ │ Scryfall   │
    │ Cosmos DB  │ │ Blob       │ │ API        │
    │            │ │ Storage    │ │            │
    └────────────┘ └────────────┘ └────────────┘
```

---

## Integration Points

### Client → Backend

| Integration | Protocol | Description |
|-------------|----------|-------------|
| GraphQL API | HTTPS/GraphQL | Primary data communication |
| Authentication | JWT/Bearer | Auth0 tokens for authorization |
| WebSocket | (Planned) | Real-time subscriptions |

### Backend → External Services

| Integration | Service | Purpose |
|-------------|---------|---------|
| Azure Cosmos DB | Database | User data, cards, collections |
| Azure Blob Storage | Storage | Card images, assets |
| Scryfall API | External API | Card data, bulk imports |
| Auth0 | Identity | Token validation |
| Azure App Configuration | Config | Runtime configuration |
| Application Insights | Monitoring | Telemetry, logging |

---

## GraphQL Communication

### Schema Location

- **Backend**: `src/App.MtgDiscovery.GraphQL/Schemas/`
- **Frontend**: Auto-fetched at build time via codegen

### Code Generation

The frontend uses GraphQL Code Generator to create TypeScript types and React hooks:

```bash
cd client
npm run codegen
```

This generates:
- `src/generated/gql.ts` - Document types
- `src/generated/graphql.ts` - Types and hooks

### Query Example

**Frontend Query Definition:**
```typescript
// client/src/graphql/queries/cards.ts
const GET_CARDS_BY_SET = gql`
  query CardsBySet($setCode: String!) {
    cardsBySet(setCode: $setCode) {
      id
      name
      manaCost
      rarity
    }
  }
`;
```

**Backend Resolver:**
```csharp
// src/App.MtgDiscovery.GraphQL/Queries/CardsQueryMethods.cs
[ExtendObjectType(typeof(ApiQuery))]
public sealed class CardsQueryMethods
{
    public async Task<IEnumerable<CardOutEntity>> CardsBySetAsync(
        string setCode,
        [Service] ICardEntryService service)
    {
        return await service.GetBySetCodeAsync(setCode);
    }
}
```

---

## Authentication Flow

```
1. User clicks Login
   ↓
2. Auth0 React SDK redirects to Auth0
   ↓
3. User authenticates with Auth0
   ↓
4. Auth0 redirects back with authorization code
   ↓
5. Auth0 SDK exchanges code for tokens
   ↓
6. JWT stored in Auth0 SDK state
   ↓
7. Auth0TokenProvider intercepts Apollo requests
   ↓
8. JWT added to Authorization header
   ↓
9. Backend validates JWT against Auth0 JWKS
   ↓
10. User identity extracted from claims
```

### Token Handling

**Frontend (Auth0TokenProvider):**
```typescript
const getAccessTokenSilently = useAuth0().getAccessTokenSilently;

// Added to every GraphQL request
headers.authorization = `Bearer ${await getAccessTokenSilently()}`;
```

**Backend (JWT Validation):**
```csharp
// Configured in Startup.cs
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{auth0Domain}/";
        options.Audience = auth0Audience;
    });
```

---

## Data Flow Patterns

### Read Operation (Query)

```
Client: useCardsQuery({ setCode: "neo" })
   ↓
Apollo Client: POST /graphql
   ↓
HotChocolate: CardsQueryMethods.CardsBySetAsync()
   ↓
Entry: CardEntryService.GetBySetCodeAsync()
   ↓
Domain: CardDomainService (apply business rules)
   ↓
Aggregator: CardAggregator (coordinate data)
   ↓
Adapter: CardsBySetAdapter (query Cosmos)
   ↓
Cosmos DB: SELECT * FROM cards WHERE setCode = "neo"
   ↓
Response bubbles up through layers
   ↓
Apollo Cache: Store result
   ↓
React: Re-render with data
```

### Write Operation (Mutation)

```
Client: useAddCardMutation({ cardId, quantity })
   ↓
Apollo Client: POST /graphql (with JWT)
   ↓
HotChocolate: Authorization check + UserCardsMutationMethods.AddCard()
   ↓
Entry: Validate ArgEntity + Create ItrEntity
   ↓
Domain: Apply business rules (limits, validation)
   ↓
Aggregator: Coordinate with user's collection
   ↓
Adapter: UserCardsAdapter.AddAsync()
   ↓
Cosmos Scribe: Upsert document
   ↓
Response with updated card data
   ↓
Apollo Cache: Invalidate/update cache
   ↓
React: Optimistic update or refetch
```

---

## Error Handling

### Frontend

Apollo Client handles errors:
- Network errors → Retry with exponential backoff
- GraphQL errors → Displayed to user
- Auth errors → Redirect to login

### Backend

HotChocolate error handling:
- Validation errors → GraphQL errors with details
- Business errors → Typed error responses
- System errors → Logged to Application Insights

---

## Environment Configuration

### Development

| Part | Port | URL |
|------|------|-----|
| Frontend | 5173 | http://localhost:5173 |
| Backend | 65203 | https://localhost:65203 |

### Production

| Part | Service | URL |
|------|---------|-----|
| Frontend | Azure Static Web Apps | swa-mtg-*.azurestaticapps.net |
| Backend | Azure Container Apps | api-mtg-*.azurecontainerapps.io |

---

## Shared Dependencies

### Cosmos DB Containers

Both client and backend depend on these Cosmos DB containers:

| Container | Purpose | Partition Key |
|-----------|---------|---------------|
| Cards | MTG card data | /setCode |
| Sets | Set metadata | /id |
| UserCards | User collections | /userId |
| UserWishlist | Wishlist items | /userId |
| Collections | Named collections | /ownerId |
| UserInfo | User profiles | /id |

### Auth0 Configuration

Shared Auth0 application:
- **Domain**: Configured in both frontend and backend
- **Audience**: API identifier for backend validation
- **Client ID**: Frontend application identifier
