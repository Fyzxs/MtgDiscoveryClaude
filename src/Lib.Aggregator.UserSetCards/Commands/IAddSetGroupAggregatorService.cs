using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Aggregator.UserSetCards.Commands;

internal interface IAddSetGroupAggregatorService
    : IOperationResponseService<IAddSetGroupToUserSetCardItrEntity, IUserSetCardOufEntity>;
