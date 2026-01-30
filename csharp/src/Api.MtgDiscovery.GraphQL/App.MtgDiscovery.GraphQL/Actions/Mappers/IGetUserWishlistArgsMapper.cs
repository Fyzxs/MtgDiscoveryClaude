using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Actions.Mappers;

namespace App.MtgDiscovery.GraphQL.Actions.Mappers;

internal interface IGetUserWishlistArgsMapper : ICreateMapper<string, IGetUserWishlistArgsEntity>;

