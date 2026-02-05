using System.Threading;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal interface IUserCardsForSigningEntryService
{
    Task<IOperationResponse<SigningResultOutEntity>> Execute(IUserCardsForSigningArgEntity input, CancellationToken cancellationToken);
}
