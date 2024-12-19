using PathPaver.Domain.Entities;

namespace PathPaver.Application.DTOs;

public record RentPredictionDto(float Baths, float Beds, float[] Coordinates, string Region, float SquareFeet, string State, string Street) { }