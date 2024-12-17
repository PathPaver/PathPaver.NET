using PathPaver.Domain.Entities;

namespace PathPaver.Application.Repository.Entities;

/**
 * 
 * Here there's a IRentPredictionRepository because we can't access to the RentPredictionRepository Impl from the PathPaver.Application project
 * 
 * This is due to the Clean architecture blocking us to do assembly reference directly to the Persistence project, but we
 * can create Interfaces for the repository in Application and then
 * link the interface and its implementation together in the main solution file [ Program.cs ]
 *
 *
 * This interface is used implemented by RentPredictionRepository.cs in PathPaver.Persistence/Entities/ 
 */
public interface IRentPredictionRepository : IBaseRepository<RentPrediction>
{
}