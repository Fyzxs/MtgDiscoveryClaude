using System.Security.Claims;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Args;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface IAddCardToCollectionArgsMapper : ICreateMapper<ClaimsPrincipal, IAddUserCardArgEntity, IAddCardToCollectionArgsEntity>
{
}
