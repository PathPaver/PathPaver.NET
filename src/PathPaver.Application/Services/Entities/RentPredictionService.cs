using PathPaver.Application.Repository.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Application.Services.Entities;

public class RentPredictionService(IRentPredictionRepository rentPredictionRepository)
{
    public void Create(RentPrediction inst)
    {
        rentPredictionRepository.Create(inst);
    }

    public void Delete(string id)
    {
        rentPredictionRepository.Delete(id);
    }
}