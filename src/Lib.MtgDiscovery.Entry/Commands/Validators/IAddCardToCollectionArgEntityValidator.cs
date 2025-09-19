using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Commands.Validators;

internal interface IAddCardToCollectionArgEntityValidator : IValidatorAction<IAddUserCardArgEntity, IOperationResponse<IUserCardOufEntity>>;
