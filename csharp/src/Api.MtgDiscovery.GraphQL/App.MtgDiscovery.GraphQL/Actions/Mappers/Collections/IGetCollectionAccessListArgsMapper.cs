using System.Security.Claims;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal interface IGetCollectionAccessListArgsMapper : ICreateMapper<ClaimsPrincipal, string, IGetCollectionAccessListArgsEntity>;
