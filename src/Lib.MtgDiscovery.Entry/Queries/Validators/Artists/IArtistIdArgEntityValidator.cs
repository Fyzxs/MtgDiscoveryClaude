using Lib.Shared.Abstractions.Actions;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.Invocation.Operations;

namespace Lib.MtgDiscovery.Entry.Queries.Validators.Artists;

internal interface IArtistIdArgEntityValidator : IValidatorAction<IArtistIdArgEntity, IOperationResponse<ICardItemCollectionItrEntity>>;
