using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Args.UserSetCards;
using App.MtgDiscovery.GraphQL.Entities.Types.ResponseModels;
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

    public UserSetCardsQueryMethods(ILogger logger) : this(new EntryService(logger))
    {
    }

    private UserSetCardsQueryMethods(IEntryService entryService) => _entryService = entryService;

    [GraphQLType(typeof(UserSetCardResponseModelUnionType))]
    public async Task<ResponseModel> UserSetCards(UserSetCardArgEntity setCardArgs)
    {
        IOperationResponse<UserSetCardOutEntity> response = await _entryService
            .GetUserSetCardByUserAndSetAsync(setCardArgs)
            .ConfigureAwait(false);

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

        return new SuccessDataResponseModel<UserSetCardOutEntity>() { Data = response.ResponseData };
    }
}
