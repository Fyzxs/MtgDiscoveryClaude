using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserSetCardArgToItrMapper : ICreateMapper<IUserSetCardArgEntity, IUserSetCardItrEntity>;
