using Lib.Shared.Abstractions.Actions.Mappers;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers.Signing;

internal interface IUserCardsForSigningArgToItrMapper : ICreateMapper<IUserCardsForSigningArgEntity, IUserCardsForSigningItrEntity>;
