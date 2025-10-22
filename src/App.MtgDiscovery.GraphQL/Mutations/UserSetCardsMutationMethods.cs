using System.Security.Claims;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
using App.MtgDiscovery.GraphQL.Mappers;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Lib.MtgDiscovery.Entry.Apis;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.MtgDiscovery.Entry.Entities.Outs.UserSetCards;
using Lib.Shared.Invocation.Operations;
using Lib.Shared.Invocation.Response.Models;
using Microsoft.Extensions.Logging;

namespace App.MtgDiscovery.GraphQL.Mutations;

[ExtendObjectType(typeof(ApiMutation))]
public sealed class UserSetCardsMutationMethods
{
    private readonly IEntryService _entryService;
    private readonly IAddSetGroupToUserSetCardArgsMapper _argsMapper;
    private readonly IOperationResponseToResponseModelMapper<UserSetCardOutEntity> _userSetCardResponseMapper;

    public UserSetCardsMutationMethods(ILogger logger) : this(
        new EntryService(logger),
        new AddSetGroupToUserSetCardArgsMapper(),
        new OperationResponseToResponseModelMapper<UserSetCardOutEntity>())
    {
    }

    private UserSetCardsMutationMethods(
        IEntryService entryService,
        IAddSetGroupToUserSetCardArgsMapper argsMapper,
        IOperationResponseToResponseModelMapper<UserSetCardOutEntity> userSetCardResponseMapper)
    {
        _entryService = entryService;
        _argsMapper = argsMapper;
        _userSetCardResponseMapper = userSetCardResponseMapper;
    }

    [Authorize]
    [GraphQLType(typeof(UserSetCardResponseModelUnionType))]
    public async Task<ResponseModel> AddSetGroupToUserSetCardAsync(
        ClaimsPrincipal claimsPrincipal,
        AddSetGroupToUserSetCardArgEntity input)
    {
        IAddSetGroupToUserSetCardArgsEntity combinedArgs = await _argsMapper.Map(claimsPrincipal, input).ConfigureAwait(false);
        IOperationResponse<UserSetCardOutEntity> response = await _entryService
            .AddSetGroupToUserSetCardAsync(combinedArgs)
            .ConfigureAwait(false);
        return await _userSetCardResponseMapper.Map(response).ConfigureAwait(false);
    }
}
