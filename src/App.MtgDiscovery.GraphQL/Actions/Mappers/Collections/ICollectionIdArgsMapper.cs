using System.Security.Claims;
using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.Collections;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers.Collections;

internal interface ICollectionIdArgsMapper : ICreateMapper<ClaimsPrincipal, string, ICollectionIdArgEntity>;
