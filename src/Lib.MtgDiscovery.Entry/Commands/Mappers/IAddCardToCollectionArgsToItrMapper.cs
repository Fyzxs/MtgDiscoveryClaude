using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Commands.Mappers;

internal interface IAddCardToCollectionArgsToItrMapper : ICreateMapper<IAuthUserArgEntity, IAddCardToCollectionArgEntity, IUserCardCollectionItrEntity>;
