using Lib.MtgDiscovery.Entry.Entities.Outs.Signing;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.Invocation.Services;

namespace Lib.MtgDiscovery.Entry.Queries.UserCards;

internal interface IUserCardsForSigningEntryService : IOperationResponseService<IUserCardsForSigningArgEntity, SigningResultOutEntity>;
