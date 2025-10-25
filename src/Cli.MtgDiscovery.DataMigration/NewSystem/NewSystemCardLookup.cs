using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Exceptions;
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
            return new FailureOperationResponse<ICardItemItrEntity>(response.OuterException);
        }

        ICardItemItrEntity card = response.ResponseData.Data.FirstOrDefault();

        if (card is null)
        {
            BadRequestOperationException notFoundException = new($"Card with Scryfall ID {scryfallId} not found");
            return new FailureOperationResponse<ICardItemItrEntity>(notFoundException);
        }

        return new SuccessOperationResponse<ICardItemItrEntity>(card);
    }

    private sealed class CardIdsItrEntity : ICardIdsItrEntity
    {
        public CardIdsItrEntity(string[] ids) => CardIds = ids;

        public ICollection<string> CardIds { get; }
    }
}
