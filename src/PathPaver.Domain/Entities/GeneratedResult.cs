using PathPaver.Domain.Common;

namespace PathPaver.Domain.Entities;

public class GeneratedResult(
    float baths,
    float beds,
    float latitude,
    float longitude,
    string region,
    float squareFeet,
    string street,
    string userId
) : BaseEntity
{
    #region Properties

    public float Baths { get; set; } = baths;
    public float Beds { get; set; } = beds;
    public float Latitude { get; set; } = latitude;
    public float Longitude { get; set; } = longitude;
    public string Region { get; set; } = region;
    public float SquareFeet { get; set; } = squareFeet;
    public string Street { get; set; } = street;
    public string UserId { get; set; } = userId;

    #endregion

    #region Methods

    // Class Methods should go here

    #endregion
}