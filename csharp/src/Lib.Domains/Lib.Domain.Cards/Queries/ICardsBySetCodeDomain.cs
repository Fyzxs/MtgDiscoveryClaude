using Lib.Shared.DataModels.Entities.Itrs.Sets;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Cards.Queries;

internal interface ICardsBySetCodeDomain
    : IOperationResponseService<ISetCodeItrEntity, ICardItemCollectionOufEntity>;
