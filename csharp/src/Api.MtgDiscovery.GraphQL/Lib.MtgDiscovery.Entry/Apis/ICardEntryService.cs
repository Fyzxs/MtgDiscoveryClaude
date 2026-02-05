using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.DataModels.Entities.Args.Cards;
using Lib.Shared.DataModels.Entities.Args.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Apis;

public interface ICardEntryService
{
    Task<IOperationResponse<List<CardItemOutEntity>>> CardsByIdsAsync(
        ICardIdsArgEntity args,
        CancellationToken cancellationToken);

    Task<IOperationResponse<List<CardItemOutEntity>>> CardsBySetCodeAsync(
        ISetCodeArgEntity setCode,
        CancellationToken cancellationToken);

    Task<IOperationResponse<List<CardItemOutEntity>>> CardsByNameAsync(
        ICardNameArgEntity cardName,
        CancellationToken cancellationToken);

    Task<IOperationResponse<List<CardNameSearchResultOutEntity>>> CardNameSearchAsync(
        ICardSearchTermArgEntity searchTerm,
        CancellationToken cancellationToken);
}
