using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Commands.Mappers;

internal interface IAddCardToCollectionArgsToItrMapper : ICreateMapper<IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>;