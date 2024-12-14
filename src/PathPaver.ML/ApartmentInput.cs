using System.ComponentModel.DataAnnotations.Schema;

namespace PathPaver.ML;

public class ApartmentInput
{
    /**
     * This is the model inputs for
     * an apartment rent prediction
     */
    
    public float
        Price,
        Longitude, 
        Latitude;
    
    public string? 
        Region, 
        State;
    
    public int 
        SquareFeet, 
        Baths, 
        Beds;
}