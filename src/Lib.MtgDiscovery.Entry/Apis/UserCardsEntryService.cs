using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.Domain.UserCards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.MtgDiscovery.Entry.Apis;

public sealed class UserCardsEntryService : IUserCardsEntryService
{
    private readonly IUserCardsDomainService _userCardsDomainService;

    public UserCardsEntryService(ILogger logger) : this(new UserCardsDomainService(logger))
    { }

    private UserCardsEntryService(IUserCardsDomainService userCardsDomainService) => _userCardsDomainService = userCardsDomainService;

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddCardToCollectionAsync(IAddCardToCollectionArgEntity args)
    {
        System.ArgumentNullException.ThrowIfNull(args);
        IUserCardCollectionItrEntity itrEntity = MapToItrEntity(args);
        return await _userCardsDomainService.AddUserCardAsync(itrEntity).ConfigureAwait(false);
    }

    private static IUserCardCollectionItrEntity MapToItrEntity(IAddCardToCollectionArgEntity args)
    {
        ICollection<ICollectedItemItrEntity> collectedItems = [.. args.CollectedList.Select(MapToCollectedItrEntity)];

        return new UserCardCollectionItrEntity
        {
            UserId = args.UserId,
            CardId = args.CardId,
            SetId = args.SetId,
            CollectedList = collectedItems
        };
    }

    private static ICollectedItemItrEntity MapToCollectedItrEntity(ICollectedItemArgEntity argItem)
    {
        return new CollectedItemItrEntity
        {
            Finish = argItem.Finish,
            Special = argItem.Special,
            Count = argItem.Count
        };
    }
}

internal sealed class UserCardCollectionItrEntity : IUserCardCollectionItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public ICollection<ICollectedItemItrEntity> CollectedList { get; init; }
}

internal sealed class CollectedItemItrEntity : ICollectedItemItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
