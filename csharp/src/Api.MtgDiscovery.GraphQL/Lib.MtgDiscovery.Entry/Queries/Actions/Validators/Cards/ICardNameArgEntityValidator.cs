using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Cards;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Cards;

internal interface ICardNameArgEntityValidator : IValidatorAction<ICardNameArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>;
