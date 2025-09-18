using App.MtgDiscovery.GraphQL.Entities.Outs.UserCards;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal interface IUserCardItrToOutMapper : ICreateMapper<IUserCardItrEntity, UserCardOutEntity>;
