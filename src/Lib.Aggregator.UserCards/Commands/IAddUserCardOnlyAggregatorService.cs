using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.Aggregator.UserCards.Commands;

/// <summary>
/// Single-method aggregator service for adding a user card without updating UserSetCards.
/// Used by migration tools to separate UserCards and UserSetCards operations.
/// </summary>
internal interface IAddUserCardOnlyAggregatorService
{
    Task<IOperationResponse<IUserCardOufEntity>> Execute(IUserCardItrEntity input);
}
