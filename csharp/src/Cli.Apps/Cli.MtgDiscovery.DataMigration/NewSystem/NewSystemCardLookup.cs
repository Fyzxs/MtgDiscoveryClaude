using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Domain.Cards.Apis;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
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

    public async Task<IOperationResponse<ICardItemOufEntity>> LookupCardByScryfallIdAsync(string scryfallId)
    {
        // TODO: Propagate CancellationToken when CLI apps support cancellation
        ICardIdsItrEntity cardIdsArg = new CardIdsItrEntity([scryfallId]);

        IOperationResponse<ICardItemCollectionOufEntity> response = await _cardDomainService
            .CardsByIdsAsync(cardIdsArg, CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ICardItemOufEntity>(response.OuterException);
        }

        ICardItemOufEntity card = response.ResponseData.Data.FirstOrDefault();

        if (card is null)
        {
            BadRequestOperationException notFoundException = new($"Card with Scryfall ID {scryfallId} not found");
            return new FailureOperationResponse<ICardItemOufEntity>(notFoundException);
        }

        return new SuccessOperationResponse<ICardItemOufEntity>(card);
    }

    private sealed class CardIdsItrEntity : ICardIdsItrEntity
    {
        public CardIdsItrEntity(string[] ids) => CardIds = ids;

        public ICollection<string> CardIds { get; }
    }
}
