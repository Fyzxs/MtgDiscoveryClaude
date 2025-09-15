using System.Threading.Tasks;
using Lib.Aggregator.UserCards.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Exceptions;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Domain.UserCards.Apis;

public sealed class UserCardsDomainService : IUserCardsDomainService
{
    private readonly IUserCardsAggregatorService _userCardsAggregatorService;

    public UserCardsDomainService(ILogger logger) : this(new UserCardsAggregatorService(logger))
    { }

    private UserCardsDomainService(IUserCardsAggregatorService userCardsAggregatorService) =>
        _userCardsAggregatorService = userCardsAggregatorService;

    public async Task<IOperationResponse<IUserCardCollectionItrEntity>> AddUserCardAsync(IUserCardCollectionItrEntity userCard)
    {
        if (userCard is null)
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("User card cannot be null"));
        }

        if (string.IsNullOrWhiteSpace(userCard.UserId))
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("UserId is required"));
        }

        if (string.IsNullOrWhiteSpace(userCard.CardId))
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("CardId is required"));
        }

        if (string.IsNullOrWhiteSpace(userCard.SetId))
        {
            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("SetId is required"));
        }

        foreach (ICollectedItemItrEntity item in userCard.CollectedList)
        {
            if (0 < item.Count) { continue; }

            return new FailureOperationResponse<IUserCardCollectionItrEntity>(
                new BadRequestOperationException("Count must be greater than zero"));
        }

        return await _userCardsAggregatorService
            .AddUserCardAsync(userCard)
            .ConfigureAwait(false);
    }
}
