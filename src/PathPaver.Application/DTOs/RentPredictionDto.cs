using PathPaver.Domain.Entities;

namespace PathPaver.Application.DTOs;

public record RentPredictionDto(
    float Baths,
    float Beds,
    float[] Coordinates,
    string Region,
    float SquareFeet,
    string State,
    string Street
)
{
    public static RentPredictionDto FromRentPrediction(RentPrediction rentPrediction)
    {
        return new RentPredictionDto(
            Baths: rentPrediction.Baths,
            Beds: rentPrediction.Beds,
            Coordinates: [rentPrediction.Latitude, rentPrediction.Longitude],
            Region: rentPrediction.Region,
            State: rentPrediction.State,
            Street: rentPrediction.Street,
            SquareFeet: rentPrediction.SquareFeet
        );
    }
}