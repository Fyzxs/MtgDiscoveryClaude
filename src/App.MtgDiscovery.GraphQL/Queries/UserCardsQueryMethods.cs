using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class UserCardsQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IUserCardItrToOutMapper _mapper;
    private readonly IUserCardCollectionItrToOutMapper _collectionMapper;

    public UserCardsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new UserCardItrToOutMapper(),
        new UserCardCollectionItrToOutMapper())
    {
    }

    private UserCardsQueryMethods(
        IEntryService entryService,
        IUserCardItrToOutMapper mapper,
        IUserCardCollectionItrToOutMapper collectionMapper)
    {
        _entryService = entryService;
        _mapper = mapper;
        _collectionMapper = collectionMapper;
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCardsBySet(UserCardsSetArgEntity setArgs)
    {
        IOperationResponse<IEnumerable<IUserCardItrEntity>> response = await _entryService
            .UserCardsBySetAsync(setArgs)
            .ConfigureAwait(false);

        if (response.IsFailure) return new FailureResponseModel()
        {
            Status = new StatusDataModel()
            {
                Message = response.OuterException.StatusMessage,
                StatusCode = response.OuterException.StatusCode
            }
        };

        List<UserCardOutEntity> results = await _collectionMapper.Map(response.ResponseData).ConfigureAwait(false);

        return new SuccessDataResponseModel<List<UserCardOutEntity>>() { Data = results };
    }
}
