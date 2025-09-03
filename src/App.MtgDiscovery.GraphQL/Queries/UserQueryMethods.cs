using System;
using System.Security.Claims;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;

namespace App.MtgDiscovery.GraphQL.Queries;

[ExtendObjectType(typeof(ApiQuery))]
public class UserQueryMethods
{
    [Authorize]
    public string AuthenticatedHello(ClaimsPrincipal claimsPrincipal)
    {
        ArgumentNullException.ThrowIfNull(claimsPrincipal);

        string nickname = claimsPrincipal.FindFirst("nickname")?.Value ?? "Unknown User";
        string email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? "no-email";

        return $"Hello {nickname}! Your email is {email}";
    }

    [Authorize]
    public UserInfo GetCurrentUser(ClaimsPrincipal claimsPrincipal)
    {
        ArgumentNullException.ThrowIfNull(claimsPrincipal);

        return new UserInfo
        {
            Id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            Email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? "unknown",
            Nickname = claimsPrincipal.FindFirst("nickname")?.Value ?? "Unknown User",
            Name = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown User"
        };
    }
}

public class UserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}