using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.UserCards.Apis;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Mappers;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning;

internal sealed class UserCardsForSigningAggregatorService : IUserCardsForSigningAggregatorService
{
    private readonly IUserCardsAdapterService _userCardsAdapterService;
    private readonly IUserCardsForSigningItrToXfrMapper _itrToXfrMapper;
    private readonly IUserCardsToSigningResultMapper _signingResultMapper;

    public UserCardsForSigningAggregatorService(ILogger logger) : this(
        new UserCardsAdapterService(logger),
        new UserCardsForSigningItrToXfrMapper(),
        new UserCardsToSigningResultMapper())
    { }

    private UserCardsForSigningAggregatorService(
        IUserCardsAdapterService userCardsAdapterService,
        IUserCardsForSigningItrToXfrMapper itrToXfrMapper,
        IUserCardsToSigningResultMapper signingResultMapper)
    {
        _userCardsAdapterService = userCardsAdapterService;
        _itrToXfrMapper = itrToXfrMapper;
        _signingResultMapper = signingResultMapper;
    }

    public async Task<IOperationResponse<ISigningResultOufEntity>> Execute(
        IUserCardsForSigningItrEntity input,
        CancellationToken cancellationToken)
    {
        IUserCardsForSigningXfrEntity xfrEntity = await _itrToXfrMapper.Map(input).ConfigureAwait(false);
        IOperationResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsAdapterService.UserCardsForSigningAsync(xfrEntity, cancellationToken).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureOperationResponse<ISigningResultOufEntity>(response.OuterException);
        }

        ISigningResultOufEntity result = await _signingResultMapper.Map(response.ResponseData, [.. input.ArtistIds]).ConfigureAwait(false);
        return new SuccessOperationResponse<ISigningResultOufEntity>(result);
    }
}
