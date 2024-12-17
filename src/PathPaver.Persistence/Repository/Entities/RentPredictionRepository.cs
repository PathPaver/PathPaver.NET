using PathPaver.Application.Repository.Entities;
using PathPaver.Domain.Entities;
using PathPaver.Persistence.Context;

namespace PathPaver.Persistence.Repository.Entities;

public sealed class RentPredictionRepository(AppDbContext context) : BaseRepository<RentPrediction>(context), IRentPredictionRepository
{
    #region Overrided Methods from BaseRepository

    #endregion
}
