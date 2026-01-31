using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Oufs.UserCards.Signing;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.UserCards;

internal interface IUserCardsForSigningArgEntityValidator : IValidatorAction<IUserCardsForSigningArgEntity, IOperationResponse<ISigningResultOufEntity>>;
