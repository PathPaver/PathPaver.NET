namespace PathPaver.Application.Repository.Entities.RentsScripts;

public interface IRentAppCount
{
    Task<string> GetAppCount();
}