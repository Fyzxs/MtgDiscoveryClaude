using System.Collections.Generic;
using System.Threading.Tasks;
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
public class ArtistQueryMethods
{
    private readonly IEntryService _entryService;

    public ArtistQueryMethods(ILogger logger) : this(new EntryService(logger))
    {
    }

    private ArtistQueryMethods(IEntryService entryService) => _entryService = entryService;

    [GraphQLType(typeof(ArtistSearchResponseModelUnionType))]
    public async Task<ResponseModel> ArtistSearch(ArtistSearchTermArgEntity searchTerm)
    {
        IOperationResponse<List<ArtistSearchResultOutEntity>> response = await _entryService.ArtistSearchAsync(searchTerm).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureResponseModel()
            {
                Status = new StatusDataModel()
                {
                    Message = response.OuterException.StatusMessage,
                    StatusCode = response.OuterException.StatusCode
                }
            };
        }

        return new SuccessDataResponseModel<List<ArtistSearchResultOutEntity>>() { Data = response.ResponseData };
    }

    [GraphQLType(typeof(CardsByArtistResponseModelUnionType))]
    public async Task<ResponseModel> CardsByArtist(ArtistIdArgEntity artistId)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByArtistAsync(artistId).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureResponseModel()
            {
                Status = new StatusDataModel()
                {
                    Message = response.OuterException.StatusMessage,
                    StatusCode = response.OuterException.StatusCode
                }
            };
        }

        return new SuccessDataResponseModel<List<CardItemOutEntity>>() { Data = response.ResponseData };
    }

    [GraphQLType(typeof(CardsByArtistResponseModelUnionType))]
    public async Task<ResponseModel> CardsByArtistName(ArtistNameArgEntity artistName)
    {
        IOperationResponse<List<CardItemOutEntity>> response = await _entryService.CardsByArtistNameAsync(artistName).ConfigureAwait(false);

        if (response.IsFailure)
        {
            return new FailureResponseModel()
            {
                Status = new StatusDataModel()
                {
                    Message = response.OuterException.StatusMessage,
                    StatusCode = response.OuterException.StatusCode
                }
            };
        }

        return new SuccessDataResponseModel<List<CardItemOutEntity>>() { Data = response.ResponseData };
    }
}
