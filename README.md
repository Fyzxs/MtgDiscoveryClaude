# MtgDiscoveryVibe

A comprehensive Magic: The Gathering collection management and deck building platform that unifies the best features from MtgDiscovery, Scryfall, and MoxField into a single, powerful application.

## 🎯 Overview

MtgDiscoveryVibe is a web application designed for serious Magic: The Gathering collectors and players who need sophisticated tools to manage their collections, build decks, and explore the vast world of MTG cards. Built with modern technologies and following the MicroObjects architectural pattern, it provides a robust, scalable solution for comprehensive MTG collection management.

## ✨ Features

### 📚 Collection Management
- **Multi-dimensional Card Tracking**: Track quantities across multiple attributes:
  - Card finish (foil, non-foil, etched, etc.)
  - Language variants
  - Condition (NM, LP, MP, HP, Damaged)
  - Special attributes (signed, altered, artist proofs)
  - Graded/slabbed cards
- **Bulk Import/Export**: Support for common collection formats
- **Wishlist & In-Transit Tracking**: Never buy duplicates again
- **Trade Management**: Flag cards for trade with specific quantities

### 🎴 Deck Building
- **Format Validation**: Build legal decks for Standard, Modern, Commander, and more
- **Advanced Statistics**: Mana curve, color distribution, average CMC
- **Draw Simulator**: Test your deck's consistency
- **Version History**: Track changes and revert when needed
- **Collection Integration**: See what you own vs. what you need

### 🔍 Card Explorer
- **Advanced Search**: Full Scryfall syntax support
- **Set Browsing**: Explore sets with collection overlay
- **Quick Add**: Add cards to your collection from any view
- **Price Tracking**: Current and historical price data

### 🔄 Data Integration
- **Scryfall API**: Complete card database with daily updates
- **Price Updates**: TCGPlayer and MTGJson integration
- **Image Storage**: Optimized card images in Azure Blob Storage
- **Smart Caching**: Efficient in-memory caching for performance

## 🛠️ Technology Stack

### Backend
- **Language**: C# (.NET 9.0)
- **Architecture**: MicroObjects Pattern
- **API**: GraphQL (Hot Chocolate)
- **Database**: Azure CosmosDB
- **Storage**: Azure Blob Storage
- **Authentication**: Azure Entra ID
- **Caching**: MonoStateMemoryCache

### Frontend
- **Framework**: React + TypeScript
- **Build Tool**: Vite
- **Styling**: Tailwind CSS
- **Components**: shadcn/ui
- **API Client**: GraphQL

### Infrastructure
- **Hosting**: Azure Container Apps
- **CI/CD**: GitHub Actions
- **Monitoring**: Application Insights

## 📁 Project Structure

```
MtgDiscoveryVibe/
├── src/
│   ├── Lib.Universal/          # Core utilities and patterns
│   ├── Lib.Cosmos/             # CosmosDB integration
│   ├── Lib.BlobStorage/        # Azure Blob Storage integration
│   ├── Lib.Scryfall/           # Scryfall API client (to be created)
│   ├── Api.GraphQL/            # GraphQL API server (to be created)
│   ├── Example.*/              # Example applications for testing
│   └── TestConvenience.Core/   # Testing utilities
├── frontend/                    # React application (to be created)
├── docs/                        # Additional documentation
├── CLAUDE.md                    # AI assistant guidelines
├── PRD.md                      # Product Requirements Document
├── CODING_CRITERIA.md          # Coding standards
├── TESTING_GUIDELINES.md       # Testing conventions
└── microobjects_coding_guidelines.md  # MicroObjects patterns
```

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- Node.js 18+ and npm
- Azure subscription (for CosmosDB and Blob Storage)
- Azure Entra ID tenant (for authentication)
- Visual Studio 2022 or VS Code

### Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/fyzxs/MtgDiscoveryVibe.git
   cd MtgDiscoveryVibe
   ```

2. **Backend Setup**
   ```bash
   # Build the solution
   dotnet build src/MtgDiscoveryVibe.sln
   
   # Run tests
   dotnet test src/MtgDiscoveryVibe.sln
   ```

3. **Frontend Setup** (when implemented)
   ```bash
   cd frontend
   npm install
   npm run dev
   ```

4. **Configure Azure Resources**
   - Create CosmosDB account
   - Create Blob Storage account
   - Configure Azure Entra ID application
   - Update configuration files with connection strings

### Running Example Applications

Example applications demonstrate specific functionality:

```bash
# Run specific example
dotnet run --project src/Example.Core/Example.Core.csproj
```

## 🧪 Testing

The project follows strict testing guidelines using MSTest and AwesomeAssertions:

```bash
# Run all tests
dotnet test src/MtgDiscoveryVibe.sln

# Run specific test project
dotnet test src/Lib.Cosmos.Tests/Lib.Cosmos.Tests.csproj

# Run with coverage
dotnet test src/MtgDiscoveryVibe.sln --collect:"XPlat Code Coverage"
```

## 📖 Development Guidelines

### MicroObjects Philosophy
This project strictly follows the MicroObjects architectural pattern:
- Every concept has explicit representation
- No primitives (wrap everything in domain objects)
- No nulls (use Null Object pattern)
- Immutable objects only
- Interface for every class
- No public statics, no enums
- Composition over inheritance

### Code Style
- File-scoped namespaces
- No greater than operators (use `<` only)
- No boolean negation (`!`), use `is false`
- `ConfigureAwait(false)` on all async calls
- Follow patterns in `CODING_CRITERIA.md`

### Testing
- Self-contained tests (no test class variables)
- Arrange-Act-Assert pattern
- Fakes over mocks
- See `TESTING_GUIDELINES.md` for details

## 🗺️ Roadmap

### Phase 1: Foundation ✅ (Current)
- [x] Project setup and structure
- [x] Core libraries (Cosmos, BlobStorage, Universal)
- [ ] Scryfall API integration
- [ ] Basic collection CRUD

### Phase 2: Collection Management
- [ ] Complex quantity tracking matrix
- [ ] Import/Export functionality
- [ ] Wishlist and trade management

### Phase 3: Deck Building
- [ ] Deck creation and validation
- [ ] Statistics and analysis
- [ ] Draw simulator

### Phase 4: Price Integration
- [ ] TCGPlayer integration
- [ ] MTGJson data
- [ ] Historical price tracking

### Phase 5: Polish
- [ ] Performance optimization
- [ ] UI/UX improvements
- [ ] Advanced features

## 🤝 Contributing

This is currently a personal project, but contributions may be welcomed in the future. Please follow:
- MicroObjects architectural patterns
- Coding criteria in `CODING_CRITERIA.md`
- Testing guidelines in `TESTING_GUIDELINES.md`
- Create example apps to demonstrate new functionality

## 📝 Documentation

- [Product Requirements Document](PRD.md) - Detailed product specifications
- [Coding Criteria](CODING_CRITERIA.md) - Project-specific coding patterns
- [Testing Guidelines](TESTING_GUIDELINES.md) - Testing conventions
- [MicroObjects Guidelines](microobjects_coding_guidelines.md) - Architecture philosophy
- [Claude Assistant Guide](CLAUDE.md) - AI assistant configuration

## 🔒 Security

- Authentication via Azure Entra ID
- All data encrypted at rest
- HTTPS for all communications
- Personal data isolation between users

## 📄 License

This project is currently private and for personal use. License to be determined for future public release.

## 🙏 Acknowledgments

- [Scryfall](https://scryfall.com) for comprehensive card data
- [TCGPlayer](https://tcgplayer.com) for pricing data
- [MTGJson](https://mtgjson.com) for additional data sources
- The Magic: The Gathering community

## 📧 Contact

Project maintained by fyzxs

---

*Magic: The Gathering is a trademark of Wizards of the Coast LLC. This project is not affiliated with or endorsed by Wizards of the Coast.*