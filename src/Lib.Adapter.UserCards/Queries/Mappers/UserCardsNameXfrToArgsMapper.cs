using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Shared.Abstractions.Identifiers;

namespace Lib.Adapter.UserCards.Queries.Mappers;

internal sealed class UserCardsNameXfrToArgsMapper : IUserCardsNameXfrToArgsMapper
{
    private readonly ICardNameGuidGenerator _guidGenerator;

    public UserCardsNameXfrToArgsMapper() : this(new CardNameGuidGenerator())
    { }

    private UserCardsNameXfrToArgsMapper(ICardNameGuidGenerator guidGenerator) => _guidGenerator = guidGenerator;

    public Task<UserCardItemsByNameExtEntitys> Map(IUserCardsNameXfrEntity source)
    {
        // Generate deterministic GUID from card name (matches CardsByName collection)
        CardNameGuid nameGuid = _guidGenerator.GenerateGuid(source.CardName);
        string cardNameGuid = nameGuid.AsSystemType().ToString();

        UserCardItemsByNameExtEntitys args = new()
        {
            UserId = source.UserId,
            CardNameGuid = cardNameGuid
        };

        return Task.FromResult(args);
    }
}
