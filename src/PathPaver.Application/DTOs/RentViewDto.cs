using PathPaver.Domain.Entities;

namespace PathPaver.Application.DTOs;

public record RentViewDto(
    float Price,
    float Baths,
    float Beds,
    float Latitude,
    float Longitude,
    string Region,
    float SquareFeet,
    string State,
    string Street
)
{
    public static RentViewDto FromRentPrediction(RentPrediction prediction)
    {
        return new RentViewDto(
            Price: prediction.Price,
            Baths: prediction.Baths,
            Beds: prediction.Beds,
            Latitude: prediction.Latitude,
            Longitude: prediction.Longitude,
            Region: prediction.Region,
            SquareFeet: prediction.SquareFeet,
            State: prediction.State,
            Street: prediction.Street
        );
    }
}