# Product Steering Document

## Product Vision
MtgDiscoveryVibe is a comprehensive Magic: The Gathering collection management and deck building platform that serves as both a practical tool for serious collectors and a flagship demonstration of exceptional software development practices. It consolidates the best features from existing platforms (MtgDiscovery, Scryfall, MoxField) with a strong focus on collector-centric functionality.

## Primary Purpose
1. **Personal Collection Management**: Built primarily for personal use to manage a serious MTG collection
2. **Development Excellence Showcase**: Demonstrates ideal coding practices and architectural patterns
3. **Community Resource**: Available for others who appreciate quality software and need robust collection tracking

## Target Users

### Primary User - The Developer/Collector (You)
- Serious MTG collector with thousands of cards
- Software developer who values code quality above all else
- Needs precise tracking of card conditions, languages, alterations, and special attributes
- Values local control and ownership of data

### Secondary Users - Serious Collectors
- MTG collectors who need more sophisticated tracking than existing tools provide
- Users who appreciate well-built software and are willing to self-host
- Collectors tracking special attributes (signed, altered, artist proofs, graded cards)

### Tertiary Users - Developers
- Software developers studying MicroObjects architecture
- Teams looking for examples of enterprise-grade C# development
- Developers learning proper testing patterns and clean architecture

## Core Features

### Collection Management (Primary Focus)
- **Multi-dimensional tracking**: Every permutation of card attributes
  - Card identity (set, number, name)
  - Finish types (foil, non-foil, etched, special)
  - Languages (all printed languages)
  - Conditions (NM, LP, MP, HP, Damaged, Not Specified)
  - Special attributes (signed, altered, artist proof)
  - Graded/slabbed status
- **Wishlist and shipping tracking**: Prevent duplicate purchases
- **Trade management**: Flag specific quantities for trade
- **Set completion tracking**: Visual progress on collecting full sets

### Deck Building
- **Format validation**: All major formats (Standard, Modern, Commander, etc.)
- **Deck statistics**: Mana curves, color distribution, CMC analysis
- **Draw simulation**: Test opening hands and consistency
- **Version history**: Track changes and iterations
- **Collection integration**: See owned vs. needed cards

### Card Explorer (Scryfall-like Interface)
- **Advanced search**: Full Scryfall syntax support
- **Set browsing**: Explore cards by set with collection overlay
- **Quick collection management**: Add/remove cards from any view
- **Price tracking**: Current and historical pricing data

### Data Integration
- **Scryfall API**: Complete card database with daily updates
- **Price sources**: TCGPlayer and MTGJson integration
- **Image management**: Local storage in Azure Blob Storage
- **Smart caching**: Performance optimization without external dependencies

## Success Metrics

### Personal Success (Primary)
- "I enjoy using this more than my current tools"
- "The code makes me proud to share it"
- "Adding new features is straightforward and maintainable"

### Technical Success
- Zero runtime exceptions in normal operation
- 100% unit test coverage on business logic
- Sub-second response times for common operations
- Clean architecture that's easy to understand

### Community Success (Future)
- Other developers reference it as a good example
- Collectors find it more capable than existing tools
- Contributors maintain the quality standards

## Product Principles

### Quality Over Features
Every line of code should be something you're proud to show others. It's better to have fewer features implemented perfectly than many features with technical debt.

### Collector-First Design
Unlike general-purpose tools, every feature should consider the serious collector's needs. Complex tracking scenarios should be first-class citizens, not afterthoughts.

### Local-First Philosophy
Your data belongs to you. The system should work entirely within your control, with external services only for data updates, not core functionality.

### Demonstration of Excellence
This codebase serves as a living example of how enterprise software should be written. Every pattern, every test, every abstraction should be teachable.

## Development Philosophy

### Iterative Perfection
Build small pieces perfectly, then compose them. Each library, each feature should be complete and polished before moving on.

### Example-Driven Development
Every significant component gets an Example app that demonstrates its functionality clearly. If you can't build a clear example, the API needs work.

### Test-Driven Confidence
Tests aren't just for coverage—they're documentation, they're contracts, and they're confidence. A feature without tests doesn't exist.

## Future Vision

### Near Term (Personal Use)
- Complete Scryfall data ingestion
- Basic collection CRUD operations
- Simple deck building
- Local operation only

### Medium Term (Polish)
- Advanced collection analytics
- Sophisticated deck building tools
- Price trend analysis
- Migration tools from other platforms

### Long Term (Public Release)
- Infrastructure as Code deployment
- Multi-user support (maintaining single-user option)
- Public API for third-party tools
- Mobile companion apps

## Out of Scope
- Social features (trading community, forums)
- Marketplace functionality
- Tournament organization
- Store inventory management
- Budget/casual player features that complicate the collector experience

## Product Decisions

### Why Not Contribute to Existing Tools?
Existing tools make architectural compromises for broad appeal. This project prioritizes code quality and collector features over mass market appeal.

### Why Azure?
It's an enterprise cloud platform that demonstrates professional deployment patterns. The architecture isn't Azure-specific and could be adapted to other clouds.

### Why MicroObjects?
It represents the extreme end of OOP design—every concept is explicit, everything is testable, nothing is implicit. It's a philosophy worth demonstrating at scale.

### Why GraphQL?
It provides a flexible API that can evolve with needs, demonstrates modern API patterns, and pairs well with strongly-typed backends.

## Guiding Questions for Features

When considering new features, ask:
1. Would I personally use this feature?
2. Does it serve serious collectors specifically?
3. Can it be implemented without compromising code quality?
4. Does it have a clear, testable implementation?
5. Can it be demonstrated with an Example app?

If the answer to any of these is "no," the feature should be reconsidered or redesigned.