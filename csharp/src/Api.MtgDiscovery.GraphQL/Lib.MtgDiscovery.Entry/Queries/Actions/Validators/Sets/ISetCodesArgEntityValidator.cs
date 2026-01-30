using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Oufs.Sets;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Sets;

internal interface ISetCodesArgEntityValidator : IValidatorAction<ISetCodesArgEntity, IOperationResponse<ISetItemCollectionOufEntity>>;
