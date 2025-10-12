using Lib.Aggregator.UserSetCards.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserSetCardArgToItrMapper : ICreateMapper<IUserSetCardArgEntity, IUserSetCardItrEntity>;
