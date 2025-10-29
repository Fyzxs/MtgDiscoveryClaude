using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserCards.Commands;

internal interface IAddUserCardAggregatorService
    : IOperationResponseService<IUserCardItrEntity, IUserCardOufEntity>;
