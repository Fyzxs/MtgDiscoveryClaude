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
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class UserCardsQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IUserCardCollectionItrToOutMapper _mapper;

    public UserCardsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new UserCardCollectionItrToOutMapper())
    {
    }

    private UserCardsQueryMethods(
        IEntryService entryService,
        IUserCardCollectionItrToOutMapper mapper)
    {
        _entryService = entryService;
        _mapper = mapper;
    }

    [GraphQLType(typeof(UserCardsCollectionResponseModelUnionType))]
    public async Task<ResponseModel> UserCardsBySet(UserCardsSetArgEntity setArgs)
    {
        IOperationResponse<IEnumerable<IUserCardCollectionItrEntity>> response = await _entryService
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

        // TODO: Create a mapper to handle the looping. This shoud be List<UserCardCollectionOutEntity> results = _mapper.Map(response.ResponseData) 
        List<UserCardCollectionOutEntity> results = [];
        foreach (IUserCardCollectionItrEntity userCard in response.ResponseData)
        {
            UserCardCollectionOutEntity mapped = await _mapper.Map(userCard).ConfigureAwait(false);
            results.Add(mapped);
        }

        return new SuccessDataResponseModel<List<UserCardCollectionOutEntity>>() { Data = results };
    }
}
