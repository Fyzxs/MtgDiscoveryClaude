using Lib.Shared.Abstractions.Actions.Validators;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Artists;

internal interface IArtistIdArgEntityValidator : IValidatorAction<IArtistIdArgEntity, IOperationResponse<ICardItemCollectionOufEntity>>;
