using System.Security.Claims;
using App.MtgDiscovery.GraphQL.Authentication;
using App.MtgDiscovery.GraphQL.Entities.Types.User;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public sealed class UserInfoQueryMethods
{
    [Authorize]
    [GraphQLType(typeof(UserInfoOutEntityType))]
#pragma warning disable CA1062 // Validate arguments of public methods - HotChocolate ensures non-null
    public UserInfoOutEntity GetUserInfo(ClaimsPrincipal claimsPrincipal)
    {
        // TEMPORARY HACK: This is a temporary implementation to extract user info from JWT

        AuthUserArgEntity authUser = new(claimsPrincipal);

        return new UserInfoOutEntity
        {
            UserId = authUser.UserId,
            Email = authUser.Email
        };
    }
#pragma warning restore CA1062
}
