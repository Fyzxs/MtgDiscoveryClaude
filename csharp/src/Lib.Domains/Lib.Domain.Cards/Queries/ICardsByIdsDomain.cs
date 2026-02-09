using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Cards.Queries;

internal interface ICardsByIdsDomain
    : IOperationResponseService<ICardIdsItrEntity, ICardItemCollectionOufEntity>;
