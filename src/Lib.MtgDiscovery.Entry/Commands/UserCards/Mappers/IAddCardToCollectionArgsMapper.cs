using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Args;

namespace Lib.MtgDiscovery.Entry.Commands.UserCards.Mappers;

internal interface IAddCardToCollectionArgsMapper : ICreateMapper<IAuthUserArgEntity, IAddUserCardArgEntity, IAddCardToCollectionArgsEntity>
{
}
