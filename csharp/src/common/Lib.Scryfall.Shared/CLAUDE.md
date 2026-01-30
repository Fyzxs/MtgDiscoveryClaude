# Lib.Scryfall.Shared CLAUDE.md

## Purpose
Shared Scryfall data models, interfaces, and utilities used across ingestion, storage, and aggregation layers for consistent MTG data representation.

## Narrative Summary
This library provides the common data models and interfaces that represent Scryfall API data structures throughout the MTG Discovery platform. It defines the contracts for cards, sets, artists, and rulings as they exist in the Scryfall ecosystem, enabling consistent data handling across ingestion, storage, and aggregation layers. The library follows MicroObjects principles with interface-based design and value object patterns for all Scryfall-specific data structures.

## Key Files
### Scryfall Data Model Interfaces
- `Apis/Models/IScryfallCard.cs` - Scryfall card data interface
- `Apis/Models/IScryfallSet.cs` - Scryfall set data interface
- `Apis/Models/IScryfallRuling.cs` - Scryfall ruling data interface
- `Apis/Models/IScryfallSearchUri.cs` - Scryfall search URI interface

### Artist Information Models
- `Apis/Models/IArtistIdNamePair.cs` - Artist identification interface
- `Apis/Models/ArtistIdNamePair.cs` - Artist identification implementation

### Card Image Models
- `Apis/Models/ICardImageInfo.cs` - Card image information interface
- `Apis/Models/ICardImageInfoCollection.cs` - Card image collection interface

### Aggregated Data Models
- `Apis/Models/AggregatedRulingData.cs` - Aggregated ruling information

## Data Model Categories
### Core Scryfall Entities
- **Cards**: Complete Scryfall card data representation
- **Sets**: Scryfall set information and metadata
- **Rulings**: Official card ruling and errata data
- **Artists**: Artist identification and attribution

### Supporting Data Structures
- **Image Information**: Card image URLs and metadata
- **Search URIs**: Scryfall API search and navigation URIs
- **Aggregated Data**: Processed and combined data structures

## Interface Design Principles
### Scryfall API Alignment
- Interfaces directly mirror Scryfall API data structures
- Property names and types match Scryfall API responses
- Nullable properties reflect Scryfall API optionality
- Collection properties support Scryfall's data patterns

### MicroObjects Implementation
- Interface for every data structure
- Value object patterns for primitive wrapping
- Immutable data representations
- No direct primitive exposure

## Artist Data Management
### Artist Identification
- `IArtistIdNamePair` - Artist ID and name pairing
- `ArtistIdNamePair` - Concrete implementation with ID/name tracking
- Consistent artist identification across card data
- Support for multiple artists per card

### Artist Data Flow
- Ingestion from Scryfall API artist arrays
- Storage in Cosmos DB with proper indexing
- Aggregation for artist-based queries and searches

## Image Information System
### Image Data Structures
- `ICardImageInfo` - Individual image information (URL, format, size)
- `ICardImageInfoCollection` - Collection of image variations
- Support for multiple image formats and sizes
- Image URL management and validation

### Image Types and Formats
- Normal card images for standard display
- Large images for detailed viewing
- Small images for compact displays
- PNG and JPG format support

## Ruling Data Aggregation
### Ruling Information
- `AggregatedRulingData` - Processed ruling information
- Official ruling text and source tracking
- Publication date and authority information
- Card-specific ruling associations

### Ruling Data Flow
- Ingestion from Scryfall rulings API
- Aggregation and processing for display
- Storage with card associations
- Retrieval for card detail views

## Dependencies
- Universal: `Lib.Universal` - Base types and utilities
- External: Standard .NET collections and nullable types
- No domain-specific dependencies (shared foundation library)

## Integration Points
### Provides
- Scryfall data model contracts to ingestion layer
- Storage entity interfaces for adapter layers
- Aggregation data structures for processing layers
- Consistent Scryfall data representation across system

### Usage Across Layers
- **Ingestion Layer**: API response deserialization targets
- **Storage Layer**: Entity mapping and transformation contracts
- **Aggregation Layer**: Data processing and filtering contracts
- **Domain Layer**: Business logic data structure contracts

## Data Structure Patterns
### Collection Interfaces
- Consistent collection patterns for related data
- Support for Scryfall's nested data structures
- Enumerable patterns for data processing
- Memory-efficient collection handling

### Optional Data Handling
- Nullable properties for optional Scryfall fields
- Default value patterns for missing data
- Validation patterns for required fields
- Consistent null handling across interfaces

## Scryfall API Compatibility
### API Version Alignment
- Interfaces match current Scryfall API version
- Property naming follows Scryfall conventions
- Data type alignment with API specifications
- Future compatibility considerations

### Data Mapping Support
- Direct mapping from Scryfall JSON responses
- Support for Scryfall's data hierarchies
- Collection and array handling patterns
- Nested object relationship support

## Key Patterns
- Interface Segregation for focused data contracts
- Value Object pattern for data integrity
- Collection pattern for related data grouping
- Factory pattern support for data creation
- Immutable data pattern for consistency

## Performance Considerations
### Memory Efficiency
- Lightweight interface definitions
- Minimal object allocation patterns
- Efficient collection implementations
- Memory-conscious data structures

### Serialization Support
- JSON serialization compatibility
- Efficient serialization patterns
- Minimal serialization overhead
- Support for partial data structures

## Related Documentation
- `../Lib.Scryfall.Ingestion/CLAUDE.md` - Data ingestion consumer
- `../Lib.Adapter.Scryfall.Cosmos/CLAUDE.md` - Storage implementation
- `../Lib.Aggregator.Scryfall.Shared/CLAUDE.md` - Aggregation utilities
- `../Lib.Universal/CLAUDE.md` - Base types and utilities