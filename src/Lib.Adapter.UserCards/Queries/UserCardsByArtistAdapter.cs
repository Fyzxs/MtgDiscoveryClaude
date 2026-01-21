using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Adapter.UserCards.Exceptions;
using Lib.Adapter.UserCards.Queries.Mappers;
using Lib.Cosmos.Apis.Operators;
using Lib.Shared.Invocation.Operations;
using Microsoft.Extensions.Logging;

namespace Lib.Adapter.UserCards.Queries;

/// <summary>
/// Retrieves all user cards by a specific artist.
/// </summary>
internal sealed class UserCardsByArtistAdapter : IUserCardsByArtistAdapter
{
    private readonly ICosmosInquisition<UserCardItemsByArtistExtEntitys> _userCardsArtistInquisition;
    private readonly IUserCardsArtistXfrToArgsMapper _artistXfrToArgsMapper;

    public UserCardsByArtistAdapter(ILogger logger) : this(new UserCardItemsByArtistInquisition(logger), new UserCardsArtistXfrToArgsMapper()) { }

    private UserCardsByArtistAdapter(ICosmosInquisition<UserCardItemsByArtistExtEntitys> userCardsArtistInquisition, IUserCardsArtistXfrToArgsMapper artistXfrToArgsMapper)
    {
        _userCardsArtistInquisition = userCardsArtistInquisition;
        _artistXfrToArgsMapper = artistXfrToArgsMapper;
    }

    public async Task<IOperationResponse<IEnumerable<UserCardExtEntity>>> Execute([NotNull] IUserCardsArtistXfrEntity input)
    {
        UserCardItemsByArtistExtEntitys args = await _artistXfrToArgsMapper.Map(input).ConfigureAwait(false);

        OpResponse<IEnumerable<UserCardExtEntity>> response = await _userCardsArtistInquisition.QueryAsync<UserCardExtEntity>(args).ConfigureAwait(false);

        if (response.IsNotSuccessful())
        {
            return new FailureOperationResponse<IEnumerable<UserCardExtEntity>>(
                new UserCardsAdapterException($"Failed to retrieve [user={input.UserId}] cards for [artist]{input.ArtistId}]", response.Exception()));
        }

        return new SuccessOperationResponse<IEnumerable<UserCardExtEntity>>(response.Value);
    }
}
