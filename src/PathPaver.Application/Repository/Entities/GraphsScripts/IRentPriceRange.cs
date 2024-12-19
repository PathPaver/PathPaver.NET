namespace PathPaver.Application.Repository.Entities.RentsScripts
{
    public interface IRentPriceRange
    {
        Task<string> GetPriceRange();
    }
}