using Microsoft.ML.Data;

namespace PathPaver.ML;

public class ApartmentOutput
{
    /**
     * This is what the model should output
     *
     * Price is predicted rent price
     * Probability is the probability that this prediction is right
     */
    
    [ColumnName("Score")] 
    public float Price { get; set; }
}