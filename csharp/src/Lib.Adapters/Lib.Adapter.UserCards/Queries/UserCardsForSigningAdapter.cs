using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Adapter.UserCards.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Retrieves all user cards by multiple artists for convention signing planning.
/// </summary>
internal sealed class UserCardsForSigningAdapter : IUserCardsForSigningAdapter
{
    private readonly ICosmosInquisition<UserCardItemsByArtistsExtEntitys> _userCardsArtistsInquisition;
    private readonly IUserCardsForSigningXfrToArgsMapper _forSigningXfrToArgsMapper;

    public UserCardsForSigningAdapter(ILogger logger) : this(new UserCardItemsByArtistsInquisition(logger), new UserCardsForSigningXfrToArgsMapper()) { }

    private UserCardsForSigningAdapter(ICosmosInquisition<UserCardItemsByArtistsExtEntitys> userCardsArtistsInquisition, IUserCardsForSigningXfrToArgsMapper forSigningXfrToArgsMapper)
    {
        _userCardsArtistsInquisition = userCardsArtistsInquisition;
        _forSigningXfrToArgsMapper = forSigningXfrToArgsMapper;
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute(
        [NotNull] IUserCardsForSigningXfrEntity input,
        CancellationToken cancellationToken)
    {
        UserCardItemsByArtistsExtEntitys args = await _forSigningXfrToArgsMapper.Map(input).ConfigureAwait(false);

        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsArtistsInquisition.QueryAsync<UserCardExtEntity>(args, cancellationToken).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve [user={input.UserId}] cards for signing with [artistCount={System.Linq.Enumerable.Count(input.ArtistIds)}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(response.Value);
    }
}
