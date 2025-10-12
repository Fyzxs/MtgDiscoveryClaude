using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Outs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserSetCardOufToOutMapper : ICreateMapper<IUserSetCardOufEntity, UserSetCardOutEntity>;
