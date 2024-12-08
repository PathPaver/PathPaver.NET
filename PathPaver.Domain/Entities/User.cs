using PathPaver.Domain.Common;

namespace PathPaver.Domain.Entities;

public sealed class User(string? username, string email) : BaseEntity
{
    #region Properties
    
    private string Email { get; set; } = email;
    private string Username { get; set; } = username ?? $"Anonymous";
    #endregion
    
    /**
     * For testing
     */
    public void ShowInfo()
    {
        Console.WriteLine(Id + 
                          Username +
                          Email +
                          DateCreated +
                          IsVisible);
    }
}