using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Cards.Apis.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.Adapter.Cards.Queries;

/// <summary>
/// Single-method adapter for searching card names using trigram matching.
/// </summary>
internal interface ISearchCardNamesAdapter
{
    Task<IOperationResponse<IEnumerable<string>>> Execute(
        ICardSearchTermXfrEntity input,
        CancellationToken cancellationToken);
}
