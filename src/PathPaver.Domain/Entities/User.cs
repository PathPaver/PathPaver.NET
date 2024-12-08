using PathPaver.Domain.Common;

namespace PathPaver.Domain.Entities;

public sealed class User(string? username, string email, string biography = "No Bio") : BaseEntity
{
    #region Properties
    
    public string Email { get; } = email;
    public string Username { get; } = username ?? $"Anonymous";
    public string Biography { get; set; } = biography;
    
    #endregion
    
    public string ShowInfo()
    {
        return $"{Username} - " +
               $"{Biography} - " +
               $"{Email} - " +
               $"{Id} - ";
    }
}