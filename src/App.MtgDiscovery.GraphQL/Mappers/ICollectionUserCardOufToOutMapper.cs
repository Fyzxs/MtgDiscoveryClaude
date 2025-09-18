using System.Collections.Generic;
using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface ICollectionUserCardOufToOutMapper : ICreateMapper<IEnumerable<IUserCardOufEntity>, List<UserCardOutEntity>>;
