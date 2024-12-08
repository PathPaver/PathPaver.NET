using PathPaver.Domain.Common;

namespace PathPaver.Domain.Entities;

public sealed class User(string? username, string email, string? biography) : BaseEntity
{
    #region Properties
    
    private string Email { get; } = email;
    private string Username { get; } = username ?? $"Anonymous";
    private string Biography { get; set; } = biography ?? "No bio";
    
    #endregion
    
    /**
     * For testing
     */
    public void ShowInfo()
    {
        Console.WriteLine(Id + 
                          Username +
                          Biography +
                          Email +
                          DateCreated +
                          IsVisible);
    }
}