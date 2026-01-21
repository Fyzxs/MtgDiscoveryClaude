using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Commands.Mappers;

internal interface IAddUserCardArgToItrMapper : ICreateMapper<AddCardToCollectionArgsEntity, IUserCardItrEntity>;
