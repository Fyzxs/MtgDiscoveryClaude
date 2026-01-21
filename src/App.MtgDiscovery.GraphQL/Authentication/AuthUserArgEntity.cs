using System;
using System.Security.Claims;
using Lib.Shared.DataModels.Entities;
using Lib.Universal.Utilities;

namespace App.MtgDiscovery.GraphQL.Authentication;

public sealed class AuthUserArgEntity : IAuthUserArgEntity
{
    // Use a consistent namespace GUID for all user subjects
    // This is "MtgUserSubject" encoded as a GUID
    private static readonly Guid s_userSubjectNamespace = new("4d746755-7365-7253-7562-6a6563744775");

    private readonly ClaimsPrincipal _claimsPrincipal;

    public AuthUserArgEntity(ClaimsPrincipal claimsPrincipal)
    {
        _claimsPrincipal = claimsPrincipal;
    }

    // IAuthUserArgEntity implementation - extract values from JWT on demand
    public string UserId
    {
        get
        {
            string subject = _claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            Guid guid = GuidUtility.Create(s_userSubjectNamespace, subject);
            return guid.ToString();
        }
    }

    public string SourceId => _claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

    public string DisplayName => _claimsPrincipal.FindFirst("nickname")!.Value;
}
