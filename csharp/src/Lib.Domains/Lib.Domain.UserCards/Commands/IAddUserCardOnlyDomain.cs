using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserCards.Commands;

internal interface IAddUserCardOnlyDomain
    : IOperationResponseService<IUserCardItrEntity, IUserCardOufEntity>;
