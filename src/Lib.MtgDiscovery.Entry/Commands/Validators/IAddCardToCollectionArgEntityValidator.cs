using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal interface IAddCardToCollectionArgEntityValidator : IValidatorAction<IAddCardToCollectionArgEntity, IOperationResponse<IUserCardCollectionItrEntity>>;
