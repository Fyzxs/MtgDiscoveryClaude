using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserCards.Commands;

internal interface IAddUserCardAggregatorService
    : IOperationResponseService<IUserCardItrEntity, IUserCardOufEntity>;
