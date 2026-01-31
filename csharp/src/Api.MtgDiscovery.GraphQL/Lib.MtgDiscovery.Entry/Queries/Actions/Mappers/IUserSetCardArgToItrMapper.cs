using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.UserSetCards;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal interface IUserSetCardArgToItrMapper : ICreateMapper<IUserSetCardArgEntity, IUserSetCardItrEntity>;
