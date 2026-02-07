using System.Threading;
using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.Cards.Apis;

public interface ICardsQueryAggregatorService
{
    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByIdsAsync(
        ICardIdsItrEntity args,
        CancellationToken cancellationToken);
    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsBySetCodeAsync(
        ISetCodeItrEntity setCode,
        CancellationToken cancellationToken);
    Task<IOperationResponse<ICardItemCollectionOufEntity>> CardsByNameAsync(
        ICardNameItrEntity cardName,
        CancellationToken cancellationToken);
    Task<IOperationResponse<ICardNameSearchCollectionOufEntity>> CardNameSearchAsync(
        ICardSearchTermItrEntity searchTerm,
        CancellationToken cancellationToken);
}
