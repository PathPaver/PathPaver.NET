namespace PathPaver.Application.Repository.Entities.RentsScripts
{
    public interface IRentLargest
    {
        Task<string> GetLargest();
    }
}