using Lib.Shared.Abstractions.Mappers;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal interface IUserCardsSetArgToItrMapper : ICreateMapper<IUserCardsSetArgEntity, IUserCardsSetItrEntity>;
