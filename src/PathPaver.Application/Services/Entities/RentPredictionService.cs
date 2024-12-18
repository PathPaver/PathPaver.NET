using MongoDB.Bson;
using PathPaver.Application.Repository.Entities;
using PathPaver.Domain.Entities;

namespace PathPaver.Application.Services.Entities;

public class RentPredictionService(IRentPredictionRepository rentPredictionRepository)
{
    public RentPrediction? GetById(string id)
    {
        return rentPredictionRepository.Get(new ObjectId(id));
    }
    
    public void Create(RentPrediction inst)
    {
        rentPredictionRepository.Create(inst);
    }

    public void Delete(string id)
    {
        rentPredictionRepository.Delete(id);
    }
}