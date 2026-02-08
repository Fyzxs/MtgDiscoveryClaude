using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistCards;
using Lib.Aggregator.Scryfall.Shared.Mappers;
using Lib.Shared.DataModels.Entities.Oufs.Cards;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistCardExtToOufEntityMapper : IArtistCardExtToOufEntityMapper
{
    private readonly IDynamicToCardItemOufEntityMapper _mapper;

    public ArtistCardExtToOufEntityMapper() : this(new DynamicToCardItemOufEntityMapper())
    { }

    private ArtistCardExtToOufEntityMapper(IDynamicToCardItemOufEntityMapper mapper) => _mapper = mapper;

    public async Task<ICardItemOufEntity> Map([NotNull] ScryfallArtistCardExtEntity source) => await _mapper.Map(source.Data).ConfigureAwait(false);
}
