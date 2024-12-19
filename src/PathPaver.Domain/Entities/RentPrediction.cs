using MongoDB.Bson;
using PathPaver.Domain.Common;

namespace PathPaver.Domain.Entities;

public class RentPrediction(
    float price,
    float baths,
    float beds,
    float latitude,
    float longitude,
    string region,
    float squareFeet,
    string state,
    string street
) : BaseEntity
{
    #region Properties

    public float Price { get; set; } = price;
    public float Baths { get; set; } = baths;
    public float Beds { get; set; } = beds;
    public float Latitude { get; set; } = latitude;
    public float Longitude { get; set; } = longitude;
    public string Region { get; set; } = region;
    public string State { get; set; } = state;
    public float SquareFeet { get; set; } = squareFeet;
    public string Street { get; set; } = street;
    #endregion

    #region Methods

    // Class Methods should go here

    #endregion
}