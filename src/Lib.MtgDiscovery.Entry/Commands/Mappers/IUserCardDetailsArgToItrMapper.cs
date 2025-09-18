using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Commands.Mappers;

internal interface IUserCardDetailsArgToItrMapper : ICreateMapper<IUserCardDetailsArgEntity, IUserCardDetailsItrEntity>;
