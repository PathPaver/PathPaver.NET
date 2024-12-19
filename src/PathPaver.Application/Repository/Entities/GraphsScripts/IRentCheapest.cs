namespace PathPaver.Application.Repository.Entities.RentsScripts
{
    public interface IRentCheapest
    {
        Task<string> GetCheapest();
    }
}