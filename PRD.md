# Product Requirements Document (PRD)
## MtgDiscoveryVibe - Magic: The Gathering Collection and Deck Management Platform

### Version 1.0
### Date: 2025-08-07

---

## Executive Summary

MtgDiscoveryVibe is a comprehensive web application for Magic: The Gathering collectors and players to manage their card collections, build decks, and explore the entire MTG card database. The platform consolidates the best features from existing services (MtgDiscovery, Scryfall, MoxField) into a unified, personal-use platform with potential for future public availability.

## Product Vision

### Problem Statement
Magic: The Gathering collectors and players currently need to use multiple disconnected platforms to:
- Track their personal collection (MtgDiscovery)
- Search and explore card information (Scryfall)
- Build and manage decks (MoxField)

This fragmentation leads to inefficiency, data duplication, and poor user experience.

### Solution
MtgDiscoveryVibe provides a unified platform that combines collection management, deck building, and card exploration with advanced tracking capabilities for serious collectors.

### Success Metrics
- Complete migration of existing personal collection data
- Sub-second card search performance
- Accurate tracking of complex collection attributes (condition, language, finish, alterations)
- Reliable synchronization with external data sources (Scryfall, TCGPlayer, MTGJson)

## User Personas

### Primary Persona: Serious Collector
- Owns thousands of cards across multiple sets
- Tracks detailed card attributes (condition, language, signatures, alterations)
- Needs accurate inventory for trading and selling
- Values comprehensive data and organization

### Secondary Persona: Competitive Player
- Builds and maintains multiple decks
- Needs format legality validation
- Requires deck statistics and playtesting tools
- Tracks deck performance and iterations

### Future Persona: Casual User
- Basic collection tracking
- Simple deck building
- Card discovery and exploration
- Price tracking for budget management

## Core Features

### 1. Data Ingestion System

#### Scryfall Integration
- **API Consumption**: Full Scryfall data ingestion with rate limiting (max 1 call/100ms)
- **Caching Layer**: In-memory caching using MonoStateMemoryCache
- **Data Storage**: Structured storage in CosmosDB
- **Incremental Updates**: Daily sync for new cards and data changes
- **Bulk Data**: Support for Scryfall bulk data downloads

#### Price Data Integration
- **TCGPlayer**: Market price updates
- **MTGJson**: Alternative pricing sources
- **Update Frequency**: Configurable price update intervals
- **Historical Tracking**: Price history storage for trend analysis

#### Image Management
- **Source**: Scryfall card images
- **Storage**: Azure Blob Storage
- **Optimization**: Multiple resolution support
- **Caching**: CDN integration for performance

### 2. Collection Management

#### Card Tracking Matrix
Complex multi-dimensional tracking system:

**Base Dimensions:**
- **Card Identity**: Set, collector number, card name
- **Finish Types**: 
  - Non-foil
  - Traditional foil
  - Etched foil
  - Other special finishes
- **Language**: All printed languages per card
- **Condition**:
  - Not Specified (default)
  - Near Mint
  - Lightly Played
  - Moderately Played
  - Heavily Played
  - Damaged

**Special Attributes (per card/finish/language combination):**
- **Quantity Types**:
  - Regular quantity
  - Signed quantity
  - Altered quantity
  - Artist Proof quantity
- **Slabbed/Graded**: Separate tracking for graded cards
- **Location**: Physical storage location (binder, box, etc.)

#### Collection Features
- **Bulk Import**: CSV and other common format support
- **Wishlist Management**: Track wanted cards
- **In-Transit Tracking**: "Being shipped" status to prevent duplicate purchases
- **Trade Management**: 
  - Flag cards available for trade
  - Specify trade quantity per card variant
- **Collection Statistics**: 
  - Set completion tracking
  - Value calculations
  - Quantity summaries at set level

### 3. Deck Building

#### Core Functionality
- **Deck Creation**: Build decks from collection or entire card pool
- **Format Validation**: 
  - Standard
  - Modern
  - Legacy
  - Vintage
  - Commander/EDH
  - Pioneer
  - Pauper
  - Custom formats
- **Deck Zones**:
  - Main deck
  - Sideboard
  - Considering board
  - Maybe board
- **Deck Metadata**:
  - Name and description
  - Tags for organization
  - Format designation
  - Creation/modification dates

#### Advanced Features
- **Statistics & Analysis**:
  - Mana curve visualization
  - Color distribution
  - Card type breakdown
  - Average mana value
  - Price calculations
- **Draw Simulator**: Test opening hands and draws
- **Version History**: Track deck changes over time with undo capability
- **Collection Integration**: Show owned vs needed cards
- **Deck Sharing**: Export/import deck lists

### 4. Card Explorer (Collector View)

#### Search & Discovery
- **Advanced Search**: 
  - Full Scryfall syntax support
  - Visual query builder
  - Saved searches
- **Set Explorer**: Browse cards by set with collection overlay
- **Card Details**: 
  - Full card information
  - Rulings
  - Price history
  - Reprints
  - Related cards

#### Collection Integration
- **Quick Add**: Add cards to collection from any view
- **Collection Overlay**: Visual indicators of owned quantities
- **Batch Operations**: Bulk add/remove cards

### 5. API Layer

#### GraphQL API (Hot Chocolate)
- **Endpoints**:
  - Card queries and mutations
  - Collection management
  - Deck operations
  - User authentication
  - Price data
- **Features**:
  - Pagination
  - Filtering
  - Sorting
  - Real-time subscriptions
- **Security**:
  - Azure Entra ID integration
  - Role-based access control
  - Rate limiting

## Technical Architecture

### Backend Stack
- **Language**: C# (.NET 9)
- **Architecture**: MicroObjects pattern
- **API**: GraphQL (Hot Chocolate)
- **Database**: Azure CosmosDB
- **Storage**: Azure Blob Storage
- **Authentication**: Azure Entra ID
- **Caching**: MonoStateMemoryCache (in-memory)
- **Libraries**: 
  - Lib.Cosmos (existing)
  - Lib.BlobStorage (existing)
  - Lib.Universal (existing)

### Frontend Stack
- **Framework**: React with TypeScript
- **Build Tool**: Vite
- **Styling**: Tailwind CSS
- **UI Components**: shadcn/ui
- **State Management**: TBD during development
- **API Client**: GraphQL client (Apollo/URQL/etc.)

### Infrastructure
- **Hosting**: Azure Container Apps (ACA) or Azure App Service
- **CDN**: Azure CDN for static assets
- **Monitoring**: Application Insights
- **CI/CD**: GitHub Actions

## Development Approach

### Methodology
- **Iterative Development**: Features built incrementally
- **Example Apps**: Proof-of-concept applications for each component
- **Test-Driven Development**: Following existing testing guidelines
- **MicroObjects Pattern**: Strict adherence to established coding patterns

### Phase 1: Foundation (MVP)
1. **Scryfall Integration**
   - API client with rate limiting
   - Caching layer
   - Basic data storage
2. **Collection Management**
   - Basic CRUD operations
   - Simple quantity tracking
   - Set-level views
3. **Collector View**
   - Card search
   - Set browsing
   - Add to collection

### Phase 2: Enhanced Collection
1. **Complex Tracking**
   - Full quantity matrix implementation
   - Condition tracking
   - Language variants
2. **Import/Export**
   - CSV import
   - Collection export
3. **Trade Management**
   - Trade flagging
   - Wishlist

### Phase 3: Deck Building
1. **Basic Deck Builder**
   - Deck creation
   - Format validation
2. **Advanced Features**
   - Statistics
   - Draw simulator
   - Version history

### Phase 4: Price & Value
1. **Price Integration**
   - TCGPlayer prices
   - MTGJson data
2. **Value Tracking**
   - Collection value
   - Price history

### Phase 5: Polish & Optimization
1. **Performance Tuning**
2. **UI/UX Refinements**
3. **Advanced Search Features**
4. **Migration Tools**

## Data Models

### Core Entities

#### Card
- Scryfall data structure
- Extended with local metadata
- Cached pricing information

#### CollectionItem
- User reference
- Card reference
- Quantity matrix (finish, language, condition, alterations)
- Acquisition data
- Location information

#### Deck
- User reference
- Format
- Card list with quantities
- Metadata (name, description, tags)
- Version history

#### User
- Azure Entra ID integration
- Preferences
- Collection statistics

### CosmosDB Structure
*To be designed collaboratively during implementation*

## Security & Privacy

### Authentication & Authorization
- Azure Entra ID for user authentication
- Role-based access control
- API key management for external services

### Data Protection
- Encrypted data at rest
- HTTPS for all communications
- Personal data isolation between users

## Future Considerations

### Potential Enhancements
- Mobile applications (iOS/Android)
- Trading marketplace
- Tournament tracking
- Social features (deck sharing, collection comparison)
- AI-powered deck suggestions
- Inventory management for stores/vendors

### Scalability
- Design for multi-tenant architecture
- Horizontal scaling capabilities
- Efficient caching strategies
- CDN utilization

## Constraints & Assumptions

### Technical Constraints
- Scryfall API rate limits (1 call/100ms)
- Azure service quotas
- CosmosDB RU limits

### Business Constraints
- Initial development for personal use
- No monetization in initial phases
- Compliance with Wizards of the Coast policies

### Assumptions
- Scryfall API stability
- Continued availability of price data sources
- User familiarity with MTG terminology

## Success Criteria

### Functional Requirements
- ✓ Complete Scryfall data ingestion
- ✓ Multi-dimensional collection tracking
- ✓ Deck building with validation
- ✓ User authentication via Azure Entra ID
- ✓ GraphQL API implementation

### Non-Functional Requirements
- Sub-second search response times
- 99.9% uptime
- Support for collections with 100,000+ cards
- Responsive design for desktop and tablet

## Risks & Mitigations

### Technical Risks
- **API Changes**: Regular monitoring of external API changes
- **Data Volume**: Efficient indexing and caching strategies
- **Performance**: Incremental optimization based on usage patterns

### Business Risks
- **License Compliance**: Regular review of WotC and data provider policies
- **Data Accuracy**: Multiple data source validation
- **User Adoption**: Focus on core features first

## Glossary

- **MTG**: Magic: The Gathering
- **Finish**: Card surface treatment (foil, non-foil, etched)
- **Slabbed**: Professionally graded and encased card
- **Commander/EDH**: Popular multiplayer format
- **RU**: Request Units (CosmosDB pricing metric)
- **WotC**: Wizards of the Coast (MTG publisher)