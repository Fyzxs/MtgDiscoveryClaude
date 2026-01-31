using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args.Artists;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Validators.Artists;

internal interface IArtistNameArgEntityValidator : IValidatorAction<IArtistNameArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>;
