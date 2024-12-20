using PathPaver.Domain.Entities;

namespace PathPaver.Application.DTOs;

public record RentPreviewDto(string Id, float Price, string Street, string Region)
{
    public static RentPreviewDto FromRentPrediction(RentPrediction prediction)
    {
        return new RentPreviewDto(
           Id: prediction.Id.ToString(),
           Price: prediction.Price,
           Street: prediction.Street,
           Region: prediction.Region
        );
    }
}