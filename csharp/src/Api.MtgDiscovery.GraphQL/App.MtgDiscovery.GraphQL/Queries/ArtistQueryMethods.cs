using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Actions.Mappers;
using App.MtgDiscovery.GraphQL.Entities.Args;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.Artists;
using Lib.MtgDiscovery.Entry.Entities.Outs.Cards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
internal sealed class ArtistQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<List<ArtistSearchResultOutEntity>> _artistSearchResponseMapper;
    private readonly IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> _cardResponseMapper;

    public ArtistQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<ArtistSearchResultOutEntity>>(),
        new OperationResponseToResponseModelMapper<List<CardItemOutEntity>>())
    {
    }

    private ArtistQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<ArtistSearchResultOutEntity>> artistSearchResponseMapper,
        IOperationResponseToResponseModelMapper<List<CardItemOutEntity>> cardResponseMapper)
    {
        _entryService = entryService;
        _artistSearchResponseMapper = artistSearchResponseMapper;
        _cardResponseMapper = cardResponseMapper;
    }

    [GraphQLType(typeof(ArtistSearchResponseModelUnionType))]
    public async Task<ResponseModel> ArtistSearch(
        ArtistSearchTermArgEntity searchTerm,
        CancellationToken cancellationToken)
    {
        IOperationResponse<List<ArtistSearchResultOutEntity>> response = await _entryService.ArtistSearchAsync(searchTerm, cancellationToken).ConfigureAwait(false);
        return await _artistSearchResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(CardsByArtistResponseModelUnionType))]
    public async Task<ResponseModel> CardsByArtist(
        ArtistIdArgEntity artistId,
        CancellationToken cancellationToken)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByArtistAsync(artistId, cancellationToken).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }

    [GraphQLType(typeof(CardsByArtistResponseModelUnionType))]
    public async Task<ResponseModel> CardsByArtistName(
        ArtistNameArgEntity artistName,
        CancellationToken cancellationToken)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByArtistNameAsync(artistName, cancellationToken).ConfigureAwait(false);
        return await _cardResponseMapper.Map(response).ConfigureAwait(false);
    }
}
