using System.Security.Claims;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.UserCards;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal interface IAddCardToCollectionArgsMapper : ICreateMapper<ClaimsPrincipal, IAddUserCardArgEntity, IAddCardToCollectionArgsEntity>
{
}
