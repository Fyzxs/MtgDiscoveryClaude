using Lib.MtgDiscovery.Entry.Apis;
using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Cards;

/// <summary>
/// Validator interface for CardIds argument entities.
/// </summary>
internal interface ICardIdsArgEntityValidator : IValidatorAction<ICardIdsArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>;
