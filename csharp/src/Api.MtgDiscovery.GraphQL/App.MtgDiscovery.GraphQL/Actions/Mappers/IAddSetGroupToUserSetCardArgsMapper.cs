using System.Security.Claims;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal interface IAddSetGroupToUserSetCardArgsMapper : ICreateMapper<ClaimsPrincipal, IAddSetGroupToUserSetCardArgEntity, IAddSetGroupToUserSetCardArgsEntity>
{
}
