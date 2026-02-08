using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.UserCards.Queries;

internal interface IUserCardDomain
    : IOperationResponseService<IUserCardItrEntity, IEnumerable<IUserCardOufEntity>>;
