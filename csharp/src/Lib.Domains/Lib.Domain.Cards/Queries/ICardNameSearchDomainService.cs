using Lib.Shared.DataModels.Entities.Itrs.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Services;

namespace Lib.Domain.Cards.Queries;

/// <summary>
/// Marker interface for card name search operation.
/// Implements single-method delegation pattern with Execute method.
/// </summary>
internal interface ICardNameSearchDomainService
    : IOperationResponseService<ICardSearchTermItrEntity, ICardNameSearchCollectionOufEntity>;
