using System.Security.Claims;
using Lib.MtgDiscovery.Entry.Entities.Collections;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal interface IDeleteCollectionArgsMapper : ICreateMapper<ClaimsPrincipal, IDeleteCollectionArgEntity, IDeleteCollectionArgsEntity>;
