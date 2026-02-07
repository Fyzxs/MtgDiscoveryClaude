using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Cli.MtgDiscovery.DataMigration.NewSystem;

internal interface INewSystemCardAdder
{
    Task<IOperationResponse<List<CardItemOutEntity>>> AddCardToCollectionAsync(IAddCardToCollectionArgsEntity args, CancellationToken cancellationToken);
    Task<IOperationResponse<List<CardItemOutEntity>>> AddUserCardOnlyAsync(IAddCardToCollectionArgsEntity args, CancellationToken cancellationToken);
}
