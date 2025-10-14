using System.Security.Claims;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface IAddSetGroupToUserSetCardArgsMapper : ICreateMapper<ClaimsPrincipal, IAddSetGroupToUserSetCardArgEntity, IAddSetGroupToUserSetCardArgsEntity>
{
}
