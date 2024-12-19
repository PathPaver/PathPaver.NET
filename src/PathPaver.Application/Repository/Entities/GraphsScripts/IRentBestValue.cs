namespace PathPaver.Application.Repository.Entities.RentsScripts
{
    public interface IRentBestValue
    {
        Task<string> GetBestValue();
    }
}