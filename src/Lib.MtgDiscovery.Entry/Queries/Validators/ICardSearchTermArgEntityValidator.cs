using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators;

internal interface ICardSearchTermArgEntityValidator : IValidatorAction<ICardSearchTermArgEntity, IOperationResponse<ICardNameSearchResultCollectionItrEntity>>;
