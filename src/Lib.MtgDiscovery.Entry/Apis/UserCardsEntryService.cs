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

    private UserCardsEntryService(IUserCardsDomainService userCardsDomainService) =>
        _userCardsDomainService = userCardsDomainService;

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddCardToCollectionAsync(IAddCardToCollectionArgEntity args)
    {
        if (args is null)
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("Arguments cannot be null"));
        }

        IOperationResponse<IUserCardCollectionItrEntity> validationResponse = ValidateArgEntity(args);
        if (validationResponse.IsFailure)
        {
            return validationResponse;
        }

        IUserCardCollectionItrEntity itrEntity = MapToItrEntity(args);
        return await _userCardsDomainService
            .AddUserCardAsync(itrEntity)
            .ConfigureAwait(false);
    }

    private static IOperationResponse<IUserCardCollectionItrEntity> ValidateArgEntity(IAddCardToCollectionArgEntity args)
    {
        if (string.IsNullOrWhiteSpace(args.UserId))
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("UserId is required"));
        }

        if (string.IsNullOrWhiteSpace(args.CardId))
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("CardId is required"));
        }

        if (string.IsNullOrWhiteSpace(args.SetId))
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("SetId is required"));
        }

        if (args.CollectedList is null || args.CollectedList.Count is 0)
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("CollectedList must contain at least one item"));
        }

        foreach (ICollectedItemArgEntity item in args.CollectedList)
        {
            if (string.IsNullOrWhiteSpace(item.Finish))
            {
                return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                    new BadRequestOperationException("Finish is required for all collected items"));
            }

            if (string.IsNullOrWhiteSpace(item.Special))
            {
                return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                    new BadRequestOperationException("Special is required for all collected items"));
            }

            if (0 < item.Count) { continue; }

            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("Count must be greater than zero for all collected items"));
        }

        return new SuccessOperationResponse<IUserCardCollectionItrEntity>(null);
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
