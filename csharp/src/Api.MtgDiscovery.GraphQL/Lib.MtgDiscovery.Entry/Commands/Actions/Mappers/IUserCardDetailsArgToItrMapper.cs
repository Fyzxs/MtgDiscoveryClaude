using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Commands.Actions.Mappers;

internal interface IUserCardDetailsArgToItrMapper : ICreateMapper<IUserCardDetailsArgEntity, IUserCardDetailsItrEntity>;
