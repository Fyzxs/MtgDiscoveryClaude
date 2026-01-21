using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserCardsSetArgToItrMapper : ICreateMapper<IUserCardsSetArgEntity, IUserCardsSetItrEntity>;
