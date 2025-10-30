using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class AllUserSetCardsQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<List<UserSetCardOutEntity>> _responseMapper;

    public AllUserSetCardsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<List<UserSetCardOutEntity>>())
    {
    }

    private AllUserSetCardsQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<List<UserSetCardOutEntity>> responseMapper)
    {
        _entryService = entryService;
        _responseMapper = responseMapper;
    }

    [GraphQLType(typeof(AllUserSetCardsResponseModelUnionType))]
    public async Task<ResponseModel> AllUserSetCards(AllUserSetCardsArgEntity userSetCardsArgs)
    {
        IOperationResponse<List<UserSetCardOutEntity>> response = await _entryService
            .AllUserSetCardsAsync(userSetCardsArgs)
            .ConfigureAwait(false);
        return await _responseMapper.Map(response).ConfigureAwait(false);
    }
}
