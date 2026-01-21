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
public sealed class UserSetCardsQueryMethods
{
    private readonly IEntryService _entryService;
    private readonly IOperationResponseToResponseModelMapper<UserSetCardOutEntity> _userSetCardResponseMapper;

    public UserSetCardsQueryMethods(ILogger logger) : this(
        new EntryService(logger),
        new OperationResponseToResponseModelMapper<UserSetCardOutEntity>())
    {
    }

    private UserSetCardsQueryMethods(
        IEntryService entryService,
        IOperationResponseToResponseModelMapper<UserSetCardOutEntity> userSetCardResponseMapper)
    {
        _entryService = entryService;
        _userSetCardResponseMapper = userSetCardResponseMapper;
    }

    [GraphQLType(typeof(UserSetCardResponseModelUnionType))]
    public async Task<ResponseModel> UserSetCards(UserSetCardArgEntity setCardArgs)
    {
        IOperationResponse<UserSetCardOutEntity> response = await _entryService
            .UserSetCardByUserAndSetAsync(setCardArgs)
            .ConfigureAwait(false);
        return await _userSetCardResponseMapper.Map(response).ConfigureAwait(false);
    }
}
