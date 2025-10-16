using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Cli.MtgDiscovery.DataMigration.NewSystem;

internal sealed class NewSystemCardLookup : INewSystemCardLookup
{
    private readonly ILogger _logger;
    private readonly ICardDomainService _cardDomainService;

    public NewSystemCardLookup(ILogger logger, ICardDomainService cardDomainService)
    {
        _logger = logger;
        _cardDomainService = cardDomainService;
    }

    public async Task<IOperationResponse<ICardItemItrEntity>> LookupCardByScryfallIdAsync(string scryfallId)
    {
        ICardIdsItrEntity cardIdsArg = new CardIdsItrEntity(new[] { scryfallId });

        IOperationResponse<ICardItemCollectionOufEntity> response = await _cardDomainService
            .CardsByIdsAsync(cardIdsArg)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return OperationResponse<ICardItemItrEntity>.Failure(response.OuterException);
        }

        ICardItemItrEntity card = response.ResponseData.Cards.FirstOrDefault();

        if (card is null)
        {
            return OperationResponse<ICardItemItrEntity>.Failure(
                new System.InvalidOperationException($"Card with Scryfall ID {scryfallId} not found"));
        }

        return OperationResponse<ICardItemItrEntity>.Success(card);
    }

    private sealed class CardIdsItrEntity : ICardIdsItrEntity
    {
        public CardIdsItrEntity(string[] ids)
        {
            Ids = ids;
        }

        public string[] Ids { get; }
    }
}
