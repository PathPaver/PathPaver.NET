namespace PathPaver.Application.DTOs;

public record RentPredictionDto(
    float Baths,
    float Beds,
    Tuple<float, float> Coordinates,
    string Region,
    float SquareFeet,
    string Street
);